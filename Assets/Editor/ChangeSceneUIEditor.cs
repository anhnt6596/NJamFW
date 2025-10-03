using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
[CustomEditor(typeof(ChangeSceneUI))]
public class ChangeSceneUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ChangeSceneUI ui = (ChangeSceneUI)target;
        DrawDefaultInspector();

        EditorGUILayout.LabelField("ACTION!!");
        if (GUILayout.Button("COVER"))
        {
            ui.Cover();
        }
        if (GUILayout.Button("EXPOSE"))
        {
            ui.Expose();
        }
    }
}