using UnityEditor;
using UnityEngine;

public class HWLoggerEditor : EditorWindow
{
    [SerializeField]private string selectedFolderPath = "Assets"; // Default folder path
    private Vector2 scrollPosition = Vector2.zero;

    private string searchTerm;

    [MenuItem("Window/HWLogger")]
    public static void ShowWindow()
    {
        GetWindow<HWLoggerEditor>("HWLogger");
        // HWLogger.DetectClasses(); // Removed automatic detection
    }

    protected void OnEnable()
    {
        // Here we retrieve the data if it exists or we save the default field initialisers we set above
        var data = EditorPrefs.GetString("HWLogger", JsonUtility.ToJson(this, false));
        // Then we apply them to this window
        JsonUtility.FromJsonOverwrite(data, this);
    }
    protected void OnDisable()
    {
        // We get the Json data
        var data = JsonUtility.ToJson(this, false);
        // And we save it
        EditorPrefs.SetString("HWLogger", data);


    }

    private void OnGUI()
    {
        GUILayout.Label("Logger Settings", EditorStyles.boldLabel);

        // Folder selection
        GUILayout.BeginHorizontal();
        GUILayout.Label("Select Folder:", GUILayout.Width(100));
        selectedFolderPath = EditorGUILayout.TextField(selectedFolderPath, GUILayout.ExpandWidth(true));
        if (GUILayout.Button("Browse", GUILayout.Width(80)))
        {
            selectedFolderPath = EditorUtility.OpenFolderPanel("Select Folder", selectedFolderPath, "");
            selectedFolderPath = ConvertToRelativePath(selectedFolderPath);
        }
        GUILayout.EndHorizontal();


        // Detection button
        if (GUILayout.Button("Detect Classes"))
        {
            HWLogger.DetectClasses(selectedFolderPath);
        }
        if (HWLogger.Instance.LoggerDB == null) return;


        GUILayout.BeginHorizontal();
        GUILayout.Label("Search", EditorStyles.boldLabel, GUILayout.Width(200));
        searchTerm = EditorGUILayout.TextField(searchTerm, GUILayout.ExpandWidth(true));
        GUILayout.EndHorizontal();
       

        // Display classes in a scrollable table
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Class Name", EditorStyles.boldLabel, GUILayout.Width(200));
        GUILayout.Label("Prefix Color", EditorStyles.boldLabel, GUILayout.Width(200));
        GUILayout.Label("Enable/Disable", EditorStyles.boldLabel, GUILayout.Width(100));
        GUILayout.EndHorizontal();

        

        foreach (HWLoggerData data in HWLogger.Instance.LoggerDB)
        {
            if ((!string.IsNullOrEmpty(searchTerm)&& !data.ClassName.Contains(searchTerm))) continue;

            GUILayout.BeginHorizontal();

            // Class Name
            GUILayout.Label(data.ClassName, GUILayout.Width(200));

         
            // Color Selection
            Color logPrefixColor = EditorGUILayout.ColorField(data.LogPrefixColor, GUILayout.Width(200));
            if (logPrefixColor != data.LogPrefixColor)
            {
               data.LogPrefixColor = logPrefixColor;
            }

            // Enable/Disable Toggle
            bool loggingEnabled = EditorGUILayout.Toggle(data.LoggingEnabled, GUILayout.Width(100));
            if (loggingEnabled != data.LoggingEnabled)
            {
                HWLogger.UpdateEnabledClasses(data.ClassName, loggingEnabled);
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }

    private string ConvertToRelativePath(string absolutePath)
    {
        if (absolutePath.StartsWith(Application.dataPath))
        {
            return "Assets" + absolutePath.Substring(Application.dataPath.Length);
        }

        // Handle other cases or return the original path
        return absolutePath;
    }
}
