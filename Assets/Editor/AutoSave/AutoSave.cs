using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.IO;

namespace Final_Survivors.Editor
{
    [InitializeOnLoad]
    public class AutoSave : EditorWindow
    {
        private string saveMode; // Currently active AutoSave mode name
        private bool saveStatus; // SaveScene return result (OK or ERROR!)

        private AutoSave() // CTOR
        {
            EditorApplication.update += SaveOnTime; // Add a method to the update event
            EditorApplication.hierarchyChanged += SaveOnDirty; // Add a method to the hierarchy changed event
            EditorApplication.playModeStateChanged += SaveOnPlay; // Add a method to the play mode state changed event

            AutoSaveData.savePath = "Assets/Scenes/Backup"; // Default "save as copy" save path
            AutoSaveData.saveTime = 300; // Default autosave time interval in seconds
        }

        [MenuItem("Tools/AutoSave")]
        public static void ShowWindow()
        {
            GetWindow<AutoSave>("AutoSave");
        }

        private void OnFocus()
        {
            AutoSaveData.LoadSettings();
        }

        private void OnLostFocus()
        {
            AutoSaveData.SaveSettings();
        }

        private void OnDestroy()
        {
            AutoSaveData.SaveSettings();
        }

        private void OnGUI()
        {
            // CENTERED TITLE
            EditorGUILayout.LabelField("AutoSave tool", EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUI.BeginChangeCheck(); // Begin tracking changes

            // AUTOSAVE TOGGLES
            if(GUILayout.Toggle(AutoSaveData.autoSave, "Enable AutoSave", "Button"))
            {
                AutoSaveData.autoSave = true;

                if(!AutoSaveData.saveOnTime && !AutoSaveData.saveOnDirty && !AutoSaveData.saveOnPlay)
                {
                    AutoSaveData.saveOnTime = true; // If all toggles are off, enable the save on time toggle by default
                }

                EditorGUILayout.BeginHorizontal(); // Begin horizontal layout

                if(GUILayout.Toggle(AutoSaveData.saveOnTime, "Save OnTime", "Button"))
                {
                    saveMode = "OnTime";
                    AutoSaveData.saveOnTime = true;
                    AutoSaveData.saveOnDirty = false;
                    AutoSaveData.saveOnPlay = false;
                }
                else
                {
                    AutoSaveData.saveOnTime = false;
                }

                if(GUILayout.Toggle(AutoSaveData.saveOnDirty, "Save OnDirty", "Button"))
                {
                    saveMode = "OnDirty";
                    AutoSaveData.saveOnTime = false;
                    AutoSaveData.saveOnDirty = true;
                    AutoSaveData.saveOnPlay = false;
                }
                else
                {
                    AutoSaveData.saveOnDirty = false;
                }

                if(GUILayout.Toggle(AutoSaveData.saveOnPlay, "Save OnPlay", "Button"))
                {
                    saveMode = "OnPlay";
                    AutoSaveData.saveOnTime = false;
                    AutoSaveData.saveOnDirty = false;
                    AutoSaveData.saveOnPlay = true;
                }
                else
                {
                    AutoSaveData.saveOnPlay = false;
                }

                EditorGUILayout.EndHorizontal(); // End horizontal layout
            }
            else
            {
                AutoSaveData.autoSave = false;
                AutoSaveData.saveOnTime = false;
                AutoSaveData.saveOnDirty = false;
                AutoSaveData.saveOnPlay = false;
            }

            if (EditorGUI.EndChangeCheck()) // End tracking changes
            {
                AutoSaveData.SaveSettings(); // Save settings
            }

            // AUTOSAVE STATUS MESSAGES
            if (!AutoSaveData.autoSave)
            {
                EditorGUILayout.LabelField("AutoSave is currently disabled.", EditorStyles.centeredGreyMiniLabel);
            }
            else if (AutoSaveData.saveOnTime || AutoSaveData.saveOnDirty || AutoSaveData.saveOnPlay)
            {
                EditorGUILayout.LabelField("Save " + saveMode + " activated.", EditorStyles.centeredGreyMiniLabel);
            }

            // SETTINGS
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Backup", EditorStyles.boldLabel);
            AutoSaveData.saveAsCopy = EditorGUILayout.Toggle(new GUIContent("Save As Copy", "Save the scene as a copy with an incremental label."), AutoSaveData.saveAsCopy);
            AutoSaveData.savePath = EditorGUILayout.TextField(new GUIContent("Save Path", "The path where the copy of the scene will be saved."), AutoSaveData.savePath);
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Assets", EditorStyles.boldLabel);
            AutoSaveData.saveAssets = EditorGUILayout.Toggle(new GUIContent("Save Assets", "Writes all unsaved asset changes to disk."), AutoSaveData.saveAssets);
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
            AutoSaveData.showDebugLog = EditorGUILayout.Toggle(new GUIContent("Show Debug Log", "Show debug messages in the console."), AutoSaveData.showDebugLog);

            // SAVE ON TIME ADDITIONAL SETTINGS
            if(AutoSaveData.saveOnTime)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                EditorGUILayout.LabelField("Timer", EditorStyles.boldLabel);
                AutoSaveData.saveTime = EditorGUILayout.IntField("Delay (seconds)", (int)Mathf.Clamp((int)AutoSaveData.saveTime, 5, 3600)); // Delay in seconds
                EditorGUILayout.LabelField("Save Each:", AutoSaveData.saveTime + " seconds");
                EditorGUILayout.LabelField("Next Save:", AutoSaveData.timeToSave.ToString() + " seconds");
                Repaint();
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        // SAVE ON TIME
        private void SaveOnTime()
        {
            if(EditorApplication.isPlaying){ return; } // Early out if the game is playing

            if(AutoSaveData.autoSave && AutoSaveData.saveOnTime)
            {
                AutoSaveData.timeToSave = (int)AutoSaveData.nextSave - (int)EditorApplication.timeSinceStartup; // Calculate the time to the next save

                if (EditorApplication.timeSinceStartup > AutoSaveData.nextSave)
                {
                    SaveScene();
                    SaveAssets();

                    AutoSaveData.nextSave = (float)EditorApplication.timeSinceStartup + AutoSaveData.saveTime; // Set the next save time
                }

                if (AutoSaveData.timeToSave > AutoSaveData.saveTime) // If the time to the next save is greater than the delay
                {
                    AutoSaveData.nextSave = (float)EditorApplication.timeSinceStartup + AutoSaveData.saveTime; // Reset the next save time
                }
            }
            else
            {
                AutoSaveData.nextSave = (float)EditorApplication.timeSinceStartup + AutoSaveData.saveTime; // Reset the next save time
            }
        }

        // SAVE ON DIRTY
        private void SaveOnDirty()
        {
            if(EditorApplication.isPlaying){ return; } // Early out if the game is playing

            if(AutoSaveData.autoSave && AutoSaveData.saveOnDirty)
            {
                if (EditorSceneManager.GetActiveScene().isDirty)
                {   
                    SaveScene();
                    SaveAssets();
                }
            }
        }

        // SAVE ON PLAY
        private void SaveOnPlay(PlayModeStateChange state)
        {
            if(EditorApplication.isPlaying){ return; } // Early out if the game is playing

            if(AutoSaveData.autoSave && AutoSaveData.saveOnPlay)
            {
                if(state == PlayModeStateChange.ExitingEditMode)
                {
                    SaveScene();
                    SaveAssets();
                }
            }
        }

        // SAVE SCENE
        private void SaveScene()
        {
            Scene scene = SceneManager.GetActiveScene();
            string sceneName = scene.name;

            if (!AutoSaveData.saveAsCopy)
            {
                saveStatus = EditorSceneManager.SaveScene(scene);
            }
            else // Save as copy (create backups)
            {
                if (!Directory.Exists(AutoSaveData.savePath))
                {
                    Directory.CreateDirectory(AutoSaveData.savePath);
                }

                string date = System.DateTime.Now.ToString("dd.MM.yyyy"); // Get the current date
                string hour = System.DateTime.Now.ToString("HH"); // Get the current hour
                string minute = System.DateTime.Now.ToString("mm"); // Get the current minute
                string second = System.DateTime.Now.ToString("ss"); // Get the current second

                saveStatus = EditorSceneManager.SaveScene(scene, AutoSaveData.savePath + "/" + sceneName + " (" + date + " @ " + hour + "h" + minute + "m" + second + "s)" + ".unity", true);
            }

            if(AutoSaveData.showDebugLog)
            {
                Debug.Log("Saved '" + sceneName + "' " + (AutoSaveData.saveAsCopy ? "(as backup copy) " : "") + saveMode + (saveStatus ? " : OK" : " : ERROR!"));
            }
        }

        // SAVE ASSETS
        private void SaveAssets()
        {
            if(AutoSaveData.saveAssets)
            {
                AssetDatabase.SaveAssets();

                if(AutoSaveData.showDebugLog)
                {
                    Debug.Log("Saved all unsaved 'Assets' changes to disk : OK");
                }
            }
        }
    }
}
