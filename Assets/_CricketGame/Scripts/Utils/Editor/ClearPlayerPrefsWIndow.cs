using UnityEngine;
using UnityEditor;

public class ClearPlayerPrefsWindow : EditorWindow
{
    [MenuItem("Tools/Clear Player Prefs")]
    public static void ShowWindow()
    {
        GetWindow<ClearPlayerPrefsWindow>("Clear Player Prefs");
    }

    private void OnGUI()
    {
        GUILayout.Label("Clear Player Preferences", EditorStyles.boldLabel);

        if (GUILayout.Button("Clear Player Prefs"))
        {
            if (EditorUtility.DisplayDialog(
                "Confirm Clear",
                "Are you sure you want to clear all Player Prefs? This action cannot be undone.",
                "Yes",
                "No"))
            {
                PlayerPrefs.DeleteAll();
                EditorUtility.DisplayDialog("Player Prefs Cleared", "All Player Prefs have been cleared.", "OK");
            }
        }
    }
}
