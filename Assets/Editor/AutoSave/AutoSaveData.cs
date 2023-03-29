using UnityEditor;

namespace Final_Survivors.Editor
{
    [InitializeOnLoad]
    static public class AutoSaveData
    {
        // Toggle to enable/disable autosave modes
        static public bool autoSave; // Enable/disable all autosave modes
        static public bool saveOnTime; // Enable/disable autosave on time
        static public bool saveOnDirty; // Enable/disable autosave on dirty
        static public bool saveOnPlay; // Enable/disable autosave on play

        // Save modes debug messages
        static public bool showDebugLog;

        // Save as copy parameters
        static public bool saveAsCopy; // Save as copy (or overwrite if false)
        static public string savePath; // Save as copy save path

        // Save assets parameter
        static public bool saveAssets; // Save assets (or not if false)

        // Time interval for SaveOnTime mode
        static public float saveTime; // Autosave time interval in seconds
        static public float nextSave;
        static public float timeToSave;

        static public void SaveSettings()
        {
            EditorPrefs.SetBool("autoSave", autoSave);
            EditorPrefs.SetBool("saveOnTime", saveOnTime);
            EditorPrefs.SetBool("saveOnDirty", saveOnDirty);
            EditorPrefs.SetBool("saveOnPlay", saveOnPlay);
            EditorPrefs.SetBool("showDebugLog", showDebugLog);
            EditorPrefs.SetBool("saveAsCopy", saveAsCopy);
            EditorPrefs.SetString("savePath", savePath);
            EditorPrefs.SetBool("saveAssets", saveAssets);
            EditorPrefs.SetFloat("saveTime", saveTime);
        }

        static public void LoadSettings()
        {
            if(EditorPrefs.HasKey("autoSave")){ autoSave = EditorPrefs.GetBool("autoSave");}
            if(EditorPrefs.HasKey("saveOnTime")){ saveOnTime = EditorPrefs.GetBool("saveOnTime"); }
            if(EditorPrefs.HasKey("saveOnDirty")){ saveOnDirty = EditorPrefs.GetBool("saveOnDirty"); }
            if(EditorPrefs.HasKey("saveOnPlay")){ saveOnPlay = EditorPrefs.GetBool("saveOnPlay"); }
            if(EditorPrefs.HasKey("showDebugLog")){ showDebugLog = EditorPrefs.GetBool("showDebugLog"); }
            if(EditorPrefs.HasKey("saveAsCopy")){ saveAsCopy = EditorPrefs.GetBool("saveAsCopy"); }
            if(EditorPrefs.HasKey("savePath")){ savePath = EditorPrefs.GetString("savePath"); }
            if(EditorPrefs.HasKey("saveAssets")){ saveAssets = EditorPrefs.GetBool("saveAssets"); }
            if(EditorPrefs.HasKey("saveTime")){ saveTime = EditorPrefs.GetFloat("saveTime"); }
        }
    }
}
