using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

/// <summary>
/// Build grid dùng trục local của chính Transform:
/// - X(local) = trục cột, Z(local) = trục hàng.
/// - Ô (x,y) có tâm tại TransformPoint((x+0.5, 0, y+0.5) * cellSize).
/// - Hỗ trợ chiếm chỗ dạng hình chữ nhật hoặc mask tùy chỉnh + xoay 0/90/180/270.
/// - Lưu chiếm chỗ bằng id (int) để tiện release về sau.
/// </summary>
[ExecuteAlways]
public class BuildGrid : MonoBehaviour, IGrid
{
    #region Interface
    public int Width => width;
    public int Height => height;
    public float CellSize => cellSize;
    #endregion Interface

    [Header("Grid Settings")]
    [Min(1)] public int width = 64;
    [Min(1)] public int height = 64;
    [Min(0.01f)] public float cellSize = 1f;

    [Tooltip("-1 = trống; >=0 = id công trình chiếm ô")]
    [SerializeField] private int[,] occ;

    [Tooltip("true = ô bị cấm bởi địa hình (sông, núi...), không thể xây")]
    [SerializeField] private bool[,] blocked;

    [SerializeField] private List<MonoBehaviour> blockPolygons, allowPolygons;
    private List<IPolygon> BlockPolygons => blockPolygons.Select(a => a.GetComponent<IPolygon>()).ToList();
    private List<IPolygon> AllowPolygons => allowPolygons.Select(a => a.GetComponent<IPolygon>()).ToList();

    //======== Lifecycle =========
    void OnEnable()
    {
        EnsureAlloc();
    }

    void OnValidate()
    {
        width = Mathf.Max(1, width);
        height = Mathf.Max(1, height);
        cellSize = Mathf.Max(0.01f, cellSize);
        EnsureAlloc();
    }

    void EnsureAlloc()
    {
        if (occ == null || occ.GetLength(0) != width || occ.GetLength(1) != height)
        {
            var oldOcc = occ;
            occ = new int[width, height];

            var oldBlk = blocked;
            blocked = new bool[width, height];

            if (oldOcc != null)
            {
                int wmin = Mathf.Min(width, oldOcc.GetLength(0));
                int hmin = Mathf.Min(height, oldOcc.GetLength(1));
                for (int y = 0; y < hmin; y++)
                    for (int x = 0; x < wmin; x++)
                        occ[x, y] = oldOcc[x, y];
            }
            else
            {
                ClearAll();
            }

            if (oldBlk != null)
            {
                int wmin = Mathf.Min(width, oldBlk.GetLength(0));
                int hmin = Mathf.Min(height, oldBlk.GetLength(1));
                for (int y = 0; y < hmin; y++)
                    for (int x = 0; x < wmin; x++)
                        blocked[x, y] = oldBlk[x, y];
            }
            else
            {
                ClearBlocked();
            }
        }
    }

    //======== API chính =========

    /// <summary> Xóa toàn bộ chiếm chỗ → -1. </summary>
    [ContextMenu("Clear All")]
    public void ClearAll()
    {
        EnsureAlloc();
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                occ[x, y] = -1;
    }

    [ContextMenu("Clear Blocked")]
    public void ClearBlocked()
    {
        EnsureAlloc();
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                blocked[x, y] = false;
    }


    [ContextMenu("Build Blocked")]
    public void BuildBlocked()
    {
        BakeBlockedFromPolygonsWorld(
            PolygonHelper.ToPolygonList(BlockPolygons),
            PolygonHelper.ToPolygonList(AllowPolygons));
    }

    /// <summary> Ô (x,y) nằm trong lưới? </summary>
    public bool InBounds(int x, int y) => (uint)x < (uint)width && (uint)y < (uint)height;

    /// <summary> Ô trống? </summary>
    public bool IsEmpty(int x, int y) => InBounds(x, y) && occ[x, y] == -1 && !blocked[x, y];

    /// <summary> Chuyển World → Cell (floor). Dựa trên transform (position/rotation). </summary>
    public bool WorldToCell(Vector3 world, out int x, out int y)
    {
        Vector3 local = transform.InverseTransformPoint(world);
        x = Mathf.FloorToInt(local.x / cellSize);
        y = Mathf.FloorToInt(local.z / cellSize);
        return InBounds(x, y);
    }

    /// <summary> Trả về tâm ô (x,y) ở World. </summary>
    public Vector3 CellToWorld(int x, int y)
    {
        Vector3 local = new Vector3((x + 0.5f) * cellSize, 0f, (y + 0.5f) * cellSize);
        return transform.TransformPoint(local);
    }

    /// <summary> Snap một điểm world về tâm ô gần nhất (World). </summary>
    public Vector3 SnapWorldToCellCenter(Vector3 world)
    {
        WorldToCell(world, out int x, out int y);
        return CellToWorld(Mathf.Clamp(x, 0, width - 1), Mathf.Clamp(y, 0, height - 1));
    }

    /// <summary> Kiểm tra vùng chữ nhật rw×rh (sau xoay) từ gốc (x,y) có trống không. </summary>
    public bool IsAreaFreeRect(int x, int y, int rw, int rh)
    {
        for (int yy = 0; yy < rh; yy++)
            for (int xx = 0; xx < rw; xx++)
            {
                int gx = x + xx, gy = y + yy;
                if (!InBounds(gx, gy) || blocked[gx, gy] || occ[gx, gy] != -1) return false;
            }
        return true;
    }

    /// <summary> Đăng ký chiếm vùng chữ nhật rw×rh bởi id. </summary>
    public void ReserveRect(int x, int y, int rw, int rh, int id)
    {
        for (int yy = 0; yy < rh; yy++)
            for (int xx = 0; xx < rw; xx++)
                occ[x + xx, y + yy] = id;
    }

    /// <summary>
    /// Kiểm tra vùng theo mask (w×h, flatten theo hàng) với rotation 0/90/180/270.
    /// rotDeg tính theo trục Y (local), dương là quay cùng chiều kim đồng hồ.
    /// </summary>
    public bool IsAreaFreeMask(int x, int y, bool[] mask, int w, int h, int rotDeg)
    {
        foreach (var c in MaskCells(x, y, mask, w, h, rotDeg))
        {
            if (!InBounds(c.x, c.y) || blocked[c.x, c.y] || occ[c.x, c.y] != -1) return false;
        }
        return true;
    }

    /// <summary> Đăng ký vùng mask bởi id. </summary>
    public void ReserveMask(int x, int y, bool[] mask, int w, int h, int rotDeg, int id)
    {
        foreach (var c in MaskCells(x, y, mask, w, h, rotDeg))
            occ[c.x, c.y] = id;
    }

    /// <summary> Giải phóng tất cả ô có id. </summary>
    public void ReleaseById(int id)
    {
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                if (occ[x, y] == id) occ[x, y] = -1;
    }

    /// <summary> Lấy id tại ô, -1 nếu trống hoặc out-of-bounds. </summary>
    public int GetIdAt(int x, int y) => InBounds(x, y) ? occ[x, y] : -1;

    //======== Mask + Rotation helper =========

    struct Cell { public int x, y; public Cell(int x, int y) { this.x = x; this.y = y; } }

    /// <summary>
    /// Duyệt tất cả ô (grid) mà mask chiếm sau khi xoay.
    /// mask: length = w*h, index = row* w + col (row = 0..h-1, col = 0..w-1).
    /// rotDeg: 0/90/180/270. Anchor (x,y) là góc trái-dưới theo hướng sau xoay.
    /// </summary>
    IEnumerable<Cell> MaskCells(int baseX, int baseY, bool[] mask, int w, int h, int rotDeg)
    {
        rotDeg = ((rotDeg % 360) + 360) % 360;

        for (int my = 0; my < h; my++)
            for (int mx = 0; mx < w; mx++)
            {
                if (!mask[my * w + mx]) continue;

                int lx, ly; // tọa độ local sau xoay
                switch (rotDeg)
                {
                    case 0: lx = mx; ly = my; break;
                    case 90: lx = h - 1 - my; ly = mx; break;
                    case 180: lx = w - 1 - mx; ly = h - 1 - my; break;
                    case 270: lx = my; ly = w - 1 - mx; break;
                    default: lx = mx; ly = my; break;
                }

                int gx = baseX + lx;
                int gy = baseY + ly;
                yield return new Cell(gx, gy);
            }
    }
    /// <summary>
    /// Bake blocked từ nhiều polygon ở WORLD space.
    /// - forbiddenPolys: danh sách poly mà "bên trong = blocked".
    /// - allowedPolys: danh sách poly mà "bên trong = allowed, ngoài = blocked".
    /// Quy tắc xung đột: blocked thắng.
    /// </summary>
    public void BakeBlockedFromPolygonsWorld(
        List<List<Vector3>> blockPolys,
        List<List<Vector3>> allowPolys)
    {
        // chuyển sang local (x,z)
        List<List<Vector2>> forbLocal = new();
        if (blockPolys != null)
            foreach (var poly in blockPolys)
                forbLocal.Add(WorldPolyToLocalXZ(poly));

        List<List<Vector2>> allowLocal = new();
        if (allowPolys != null)
            foreach (var poly in allowPolys)
                allowLocal.Add(WorldPolyToLocalXZ(poly));

        BakeBlockedFromPolygonsLocal(forbLocal, allowLocal);
    }

    /// <summary>
    /// Bake blocked từ nhiều polygon ở LOCAL space (x,z).
    /// - forbiddenLocal: list poly "bên trong = blocked".
    /// - allowedLocal: list poly "bên trong = allowed, ngoài = blocked".
    /// Quy tắc xung đột: blocked thắng.
    /// </summary>
    public void BakeBlockedFromPolygonsLocal(
        List<List<Vector2>> blockLocal,
        List<List<Vector2>> allowLocal)
    {
        EnsureAlloc();

        // 1) reset về "mọi ô đều allowed" (chưa xét allowed mask toàn cục)
        ClearBlocked();

        // 2) Nếu có allowed polys: tính "ô ở trong ÍT NHẤT MỘT allowed poly"
        bool useAllowed = (allowLocal != null && allowLocal.Count > 0);
        bool[,] insideAnyAllowed = null;
        if (useAllowed)
        {
            insideAnyAllowed = new bool[width, height];

            foreach (var poly in allowLocal)
            {
                if (poly == null || poly.Count < 3) continue;

                // AABB của poly để giới hạn vùng duyệt
                (int x0, int y0, int x1, int y1) = PolyAabbCells(poly);
                for (int y = y0; y <= y1; y++)
                    for (int x = x0; x <= x1; x++)
                    {
                        // Tâm ô (local)
                        Vector2 center = new Vector2(
                            (x + 0.5f) * cellSize,
                            (y + 0.5f) * cellSize);

                        if (PolygonHelper.PointInPoly(center, poly))
                            insideAnyAllowed[x, y] = true;
                    }
            }
        }

        // 3) Áp allowed: nếu có allowed list → ô nào KHÔNG ở trong bất kỳ allowed poly nào → blocked
        if (useAllowed)
        {
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    if (!insideAnyAllowed[x, y]) blocked[x, y] = true;
        }

        // 4) Áp forbidden: bên trong forbidden → blocked
        if (blockLocal != null)
        {
            foreach (var poly in blockLocal)
            {
                if (poly == null || poly.Count < 3) continue;

                (int x0, int y0, int x1, int y1) = PolyAabbCells(poly);
                for (int y = y0; y <= y1; y++)
                    for (int x = x0; x <= x1; x++)
                    {
                        // Tâm ô (local)
                        Vector2 center = new Vector2(
                            (x + 0.5f) * cellSize,
                            (y + 0.5f) * cellSize);

                        if (PolygonHelper.PointInPoly(center, poly))
                            blocked[x, y] = true;
                    }
            }
        }
    }
    // Lấy AABB poly rồi đổi ra chỉ số cell
    (int x0, int y0, int x1, int y1) PolyAabbCells(IList<Vector2> poly)
    {
        float minX = float.PositiveInfinity, minY = float.PositiveInfinity;
        float maxX = float.NegativeInfinity, maxY = float.NegativeInfinity;
        foreach (var v in poly)
        {
            if (v.x < minX) minX = v.x;
            if (v.y < minY) minY = v.y;
            if (v.x > maxX) maxX = v.x;
            if (v.y > maxY) maxY = v.y;
        }
        int x0 = Mathf.Max(0, Mathf.FloorToInt(minX / cellSize));
        int y0 = Mathf.Max(0, Mathf.FloorToInt(minY / cellSize));
        int x1 = Mathf.Min(width - 1, Mathf.FloorToInt(maxX / cellSize));
        int y1 = Mathf.Min(height - 1, Mathf.FloorToInt(maxY / cellSize));
        return (x0, y0, x1, y1);
    }

    // Trả về rMin/rMax (local) của ô (x,y)
    void RectToLocal(int x, int y, out Vector2 rMin, out Vector2 rMax)
    {
        float xs = x * cellSize, ys = y * cellSize;
        rMin = new Vector2(xs, ys);
        rMax = new Vector2(xs + cellSize, ys + cellSize);
    }

    List<Vector2> WorldPolyToLocalXZ(IList<Vector3> worldPoly)
    {
        var outList = new List<Vector2>(worldPoly.Count);
        foreach (var w in worldPoly)
        {
            var l = transform.InverseTransformPoint(w);
            outList.Add(new Vector2(l.x, l.z));
        }
        return outList;
    }

    /// <summary>
    /// Đặt trạng thái blocked cho một vùng chữ nhật theo tọa độ Ô (x,z) và kích thước (w,h).
    /// Trả về số ô thực sự thay đổi trạng thái.
    /// </summary>
    protected int SetBlockedRect(int x, int z, int w, int h, bool state = true)
    {
        EnsureAlloc();
        if (w <= 0 || h <= 0) return 0;

        // Clamp vào biên grid
        int x0 = Mathf.Max(0, x);
        int y0 = Mathf.Max(0, z);
        int x1 = Mathf.Min(width, x + w);
        int y1 = Mathf.Min(height, z + h);

        int changed = 0;
        for (int gy = y0; gy < y1; gy++)
            for (int gx = x0; gx < x1; gx++)
            {
                if (blocked[gx, gy] != state)
                {
                    blocked[gx, gy] = state;
                    changed++;
                }
            }
        return changed;
    }

    /// <summary> Block nhanh một vùng chữ nhật ô. </summary>
    public int AddBlockedRect(int x, int z, int w, int h) => SetBlockedRect(x, z, w, h, true);

    /// <summary> Unblock nhanh một vùng chữ nhật ô. </summary>
    public int RemoveBlockedRect(int x, int z, int w, int h) => SetBlockedRect(x, z, w, h, false);

#if UNITY_EDITOR
    [Header("Gizmos")]
    public bool drawGridGizmos = true;
    public bool drawBlockedCells = true;
    [Range(0f, 1f)] public float blockedAlpha = 0.35f;
    public Color gridColor = new Color(1f, 1f, 1f, 0.08f);
    public Color boundsColor = new Color(0f, 1f, 1f, 0.35f);
    public Color blockedColor = new Color(1f, 0f, 0f, 0.35f);

    void OnDrawGizmos()
    {
        if (!drawGridGizmos && !drawBlockedCells) return;
        EnsureAlloc();

        // ----- Vẽ khung bao ngoài -----
        Gizmos.color = boundsColor;
        Vector3 p0 = transform.TransformPoint(new Vector3(0, 0, 0));
        Vector3 p1 = transform.TransformPoint(new Vector3(width * cellSize, 0, 0));
        Vector3 p2 = transform.TransformPoint(new Vector3(width * cellSize, 0, height * cellSize));
        Vector3 p3 = transform.TransformPoint(new Vector3(0, 0, height * cellSize));
        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p0);

        // ----- Vẽ ô blocked -----
        if (drawBlockedCells && blocked != null)
        {
            Gizmos.color = new Color(blockedColor.r, blockedColor.g, blockedColor.b, blockedAlpha);
            float y = 0.02f; // nhấc lên một chút cho dễ nhìn
            Vector3 size = new Vector3(cellSize, 0.001f, cellSize);
            for (int gy = 0; gy < height; gy++)
                for (int gx = 0; gx < width; gx++)
                {
                    if (!blocked[gx, gy]) continue;
                    Vector3 localCenter = new Vector3((gx + 0.5f) * cellSize, y, (gy + 0.5f) * cellSize);
                    Vector3 worldCenter = transform.TransformPoint(localCenter);
                    Gizmos.DrawCube(worldCenter, size);
                }
        }

        // ----- Vẽ lưới -----
        if (drawGridGizmos)
        {
            Gizmos.color = gridColor;
            // dọc theo X
            for (int x = 0; x <= width; x++)
            {
                Vector3 a = transform.TransformPoint(new Vector3(x * cellSize, 0, 0));
                Vector3 b = transform.TransformPoint(new Vector3(x * cellSize, 0, height * cellSize));
                Gizmos.DrawLine(a, b);
            }
            // dọc theo Z
            for (int y = 0; y <= height; y++)
            {
                Vector3 a = transform.TransformPoint(new Vector3(0, 0, y * cellSize));
                Vector3 b = transform.TransformPoint(new Vector3(width * cellSize, 0, y * cellSize));
                Gizmos.DrawLine(a, b);
            }
        }
    }
#endif

}
