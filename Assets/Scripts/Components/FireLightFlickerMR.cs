using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class FireLightFlickerMR : FireLightFlicker
{
    private MeshRenderer mr;
    private Material mat;

    protected override void Awake()
    {
        base.Awake();
        mr = GetComponent<MeshRenderer>();
        mat = new Material(mr.material) { name = mr.material.name + " (Runtime)" };
        mat.hideFlags = HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild;
        mr.material = mat;
    }

    protected override void SetColor(Color c)
    {
        mat.SetColor("_Color", c);
    }
}
