using UnityEditor;
using UnityEngine;

public static class ClearPlayerPrefsTool
{
    [MenuItem("Tools/Clear PlayerPrefs %#d")] // Ctrl/Cmd + Shift + D
    public static void ClearPlayerPrefs()
    {
        if (EditorUtility.DisplayDialog("Clear PlayerPrefs",
                "Are you sure you want to delete all PlayerPrefs?", "Yes", "Cancel"))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("<color=orange>✅ PlayerPrefs cleared successfully!</color>");
        }
    }
}