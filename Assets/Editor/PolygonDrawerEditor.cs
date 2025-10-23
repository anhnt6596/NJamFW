using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PolygonDrawer))]
public class PolygonDrawerEditor : Editor
{
    PolygonDrawer poly;
    int selectedIndex = -1;

    private void OnEnable()
    {
        poly = (PolygonDrawer)target;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Polygon Editor", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Node At Origin"))
        {
            Undo.RecordObject(poly, "Add Node");
            poly.AddNode(poly.transform.position);
            EditorUtility.SetDirty(poly);
        }
        if (GUILayout.Button("Clear Nodes"))
        {
            if (EditorUtility.DisplayDialog("Clear Nodes", "Remove all nodes?", "Yes", "No"))
            {
                Undo.RecordObject(poly, "Clear Nodes");
                poly.ClearNodes();
                EditorUtility.SetDirty(poly);
            }
        }
        EditorGUILayout.EndHorizontal();

        if (poly.nodes != null && poly.nodes.Count > 0)
        {
            if (GUILayout.Button("Export Points (Debug.Log)"))
            {
                var pts = poly.GetPolygon2D();
                string s = "Polygon points:\n";
                for (int i = 0; i < pts.Count; i++)
                    s += $"{i}: {pts[i]}\n";
                Debug.Log(s);
            }
        }
    }

    void OnSceneGUI(SceneView sv)
    {
        if (poly == null) return;

        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        Event e = Event.current;

        // Left Ctrl + Click => add node at mouse position (XY plane, z = 0)
        if (e.type == EventType.MouseDown && e.button == 0 && e.control && !e.alt && !e.shift)
        {
            Vector3 worldPoint = GetMouseWorldOnYPlane(e.mousePosition, 0f);
            Vector3 localPoint = poly.transform.InverseTransformPoint(worldPoint);
            Undo.RecordObject(poly, "Add Polygon Node");
            poly.AddNode(localPoint);
            EditorUtility.SetDirty(poly);
            e.Use();
        }

        // Draw and handle node position handles
        for (int i = 0; i < poly.nodes.Count; i++)
        {
            var node = poly.nodes[i];
            if (node == null) continue;

            // Draw label
            Handles.color = Color.white;
            Handles.Label(node.position + Vector3.up * 0.2f, $"N{i}");

            // Position handle
            EditorGUI.BeginChangeCheck();
            Vector3 newPos = Handles.PositionHandle(node.position, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(node, "Move Polygon Node");
                node.position = newPos;
                EditorUtility.SetDirty(node);
            }

            // Node selection by clicking small button
            float pickSize = HandleUtility.GetHandleSize(node.position) * 0.1f;
            if (Handles.Button(node.position, Quaternion.identity, pickSize, pickSize, Handles.DotHandleCap))
            {
                selectedIndex = i;
                Selection.activeTransform = node;
                e.Use();
            }
        }

        // delete selected node with Delete key
        if (selectedIndex >= 0 && selectedIndex < poly.nodes.Count)
        {
            if (e.type == EventType.KeyDown && (e.keyCode == KeyCode.Delete || e.keyCode == KeyCode.Backspace))
            {
                Undo.RecordObject(poly, "Remove Polygon Node");
                poly.RemoveNodeAt(selectedIndex);
                selectedIndex = -1;
                EditorUtility.SetDirty(poly);
                e.Use();
            }
        }

        // Draw polygon edges in Scene
        Handles.color = poly.lineColor;
        for (int i = 0; i < poly.nodes.Count - 1; i++)
        {
            if (poly.nodes[i] != null && poly.nodes[i + 1] != null)
                Handles.DrawLine(poly.nodes[i].position, poly.nodes[i + 1].position);
        }
        if (poly.loop && poly.nodes.Count > 1)
        {
            var a = poly.nodes[poly.nodes.Count - 1];
            var b = poly.nodes[0];
            if (a != null && b != null) Handles.DrawLine(a.position, b.position);
        }

        // Indicate instructions
        Handles.BeginGUI();
        GUILayout.BeginArea(new Rect(10, 10, 300, 120), GUI.skin.box);
        GUILayout.Label("Polygon Editor Controls", EditorStyles.boldLabel);
        GUILayout.Label("Ctrl + LeftClick : Add node at mouse (XY plane, z=0)");
        GUILayout.Label("Drag handles : Move node");
        GUILayout.Label("Click dot : Select node");
        GUILayout.Label("Delete/Backspace : Remove selected node");
        GUILayout.EndArea();
        Handles.EndGUI();
    }

    // convert mouse pos (GUI coords) to world XY plane at zPlane
    static Vector3 GetMouseWorldOnYPlane(Vector2 guiMousePos, float y = 0f)
    {
        // Lấy ray theo GUI coords chuẩn của OnSceneGUI
        Ray ray = HandleUtility.GUIPointToWorldRay(guiMousePos);

        // Mặt phẳng y = const
        Plane plane = new Plane(Vector3.up, new Vector3(0f, y, 0f));
        return plane.Raycast(ray, out float dist) ? ray.GetPoint(dist) : Vector3.zero;
    }
}
