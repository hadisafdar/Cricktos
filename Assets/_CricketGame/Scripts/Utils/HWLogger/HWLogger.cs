using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


[System.Serializable]
public class HWLoggerData
{
    public string ClassName;
    public bool LoggingEnabled;
    public Color LogPrefixColor;

    public HWLoggerData(string className, bool loggingEnabled, Color logPrefixColor)
    {
        ClassName = className;
        LoggingEnabled = loggingEnabled;
        LogPrefixColor = logPrefixColor;
    }
}


[CreateAssetMenu(menuName ="HyyderWorks/HWLogger")]
public class HWLogger : ScriptableObject
{

    #region [Singleton]
    private static HWLogger instance;
    public static HWLogger Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<HWLogger>("HWLogger");
                if (instance == null)
                {
                    instance = CreateInstance<HWLogger>();
#if UNITY_EDITOR
                    UnityEditor.AssetDatabase.CreateAsset(instance, "Assets/Resources/HWLogger.asset");
#endif
                }
            }

            return instance;
        }
    }


    #endregion


    [SerializeField]private static Dictionary<string, HWLoggerData> loggerDataDictionary;

    #region [Data]
    [SerializeField]private List<HWLoggerData> loggerDB;
    [SerializeField]public List<HWLoggerData> LoggerDB=>loggerDB;
    public void AddData(HWLoggerData data)
    {

        HWLoggerData existingData = GetData(data.ClassName);
        if (existingData != null) return;

        if (!loggerDB.Contains(data))
        {
            loggerDB.Add(data);
        }
    }

    public void RemoveByClassName(string className)
    {
        HWLoggerData data = GetData(className);
        if (data != null)
        {
            RemoveData(data);
        }
    }

    public void RemoveData(HWLoggerData data)
    {
        if (loggerDB.Contains(data))
        {
            loggerDB.Remove(data);
        }
    }


    public HWLoggerData GetData(string className)
    {
        if (loggerDB == null) return null;
        HWLoggerData data = loggerDB.FirstOrDefault(d => d.ClassName.Equals(className));
        return data;
    }

    #endregion



    public static void Log(Type callingClass,string message, UnityEngine.Object caller = null)
    {   
        LogMessage(callingClass,message, HWLoggerMessageType.Info);
    }

    public static void LogWarning(Type callingClass,string message, UnityEngine.Object caller = null)
    {
        LogMessage(callingClass,message, HWLoggerMessageType.Warning);
    }

    public static void LogError(Type callingClass,string message, UnityEngine.Object caller = null)
    {
        LogMessage(callingClass,message, HWLoggerMessageType.Error);
    }

    private static void LogMessage(Type callingClass,string message, HWLoggerMessageType messageType, UnityEngine.Object caller = null)
    {

        if (Instance.LoggerDB == null)
        {
            Debug.Log("No Logger DB");
            return;
        }

        //Create a new dictionary for each class and it's HWLoggerData
        if (loggerDataDictionary == null)
        {
            CreateDictionary();
        }

        string callingClassName = callingClass.Name;

        loggerDataDictionary.TryGetValue(callingClassName, out HWLoggerData loggerData);
        if (loggerData == null)
        {
            loggerData = AddNewLoggerData(callingClassName);
            AddToLoggerDataDict(callingClassName, loggerData);
        }
        if (!loggerData.LoggingEnabled)
        {
            return; // Logging is disabled for this class
        }
        string prefixColor = ColorUtility.ToHtmlStringRGB(loggerData.LogPrefixColor);
        string formattedMessage = $"🗒<color=#{prefixColor}><b><size=13>[{callingClassName}]</size></b>_{message}</color>";


        switch (messageType)
        {
            case HWLoggerMessageType.Info:
                Debug.Log(formattedMessage,caller);
                break;
            case HWLoggerMessageType.Warning:
                Debug.LogWarning(formattedMessage,caller);
                break;
            case HWLoggerMessageType.Error:
                Debug.LogError(formattedMessage,caller);
                break;
        }
    }

    private static HWLoggerData GetLoggerData(string callingClass)
    {
        HWLoggerData loggerData = Instance.GetData(callingClass);
        if (loggerData == null)
        {
            loggerData = AddNewLoggerData(callingClass);
        }

        AddToLoggerDataDict(callingClass, loggerData);
        return loggerData;
    }

    private static HWLoggerData AddNewLoggerData(string callingClass)
    {
        HWLoggerData loggerData = new HWLoggerData(callingClass, true, Color.white);
        Instance.AddData(loggerData);
        return loggerData;
    }

    private static void CreateDictionary()
    {
        loggerDataDictionary = new();
        foreach (HWLoggerData data in Instance.loggerDB)
        {
            AddToLoggerDataDict(data.ClassName, data);
        }
    }


    private static void AddToLoggerDataDict(string callingClass, HWLoggerData loggerData)
    {
        if (loggerDataDictionary == null)
        {
            return;
        }
        if (!loggerDataDictionary.ContainsKey(callingClass))
        {
            loggerDataDictionary.Add(callingClass, loggerData);
        }
    }

    private static void RemoveFromLoggerDataDict(string callingClass)
    {
        if (loggerDataDictionary == null) return;
        if (loggerDataDictionary.ContainsKey(callingClass))
        {
            loggerDataDictionary.Remove(callingClass);
        }
    }

#if UNITY_EDITOR
    public static void DetectClasses(string scriptFolderPath)
    {
        string[] scripts = AssetDatabase.FindAssets("t:Script")
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .Where(path => !path.Contains("/Editor/") && !path.Contains("/Plugins/") && path.StartsWith(scriptFolderPath))
            .ToArray();

        if (scripts.Length == 0)
        {
            return;
        }

        foreach (var scriptPath in scripts)
        {
            var script = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);
            var scriptText = script.text;

            var _class = script.GetClass();
            if (_class == null || _class == typeof(HWLogger)) continue;

            if (scriptText.Contains("HWLogger.Log"))
            {
                UpdateEnabledClasses(_class.FullName, true);
            }
            else
            {
                Instance.RemoveByClassName(_class.FullName);
                RemoveFromLoggerDataDict(_class.FullName);
            }
        }
    }
#endif

    public static void UpdateEnabledClasses(string className, bool isEnabled)
    {
        HWLoggerData data = GetLoggerData(className);
        data.LoggingEnabled = isEnabled;
    }

}
