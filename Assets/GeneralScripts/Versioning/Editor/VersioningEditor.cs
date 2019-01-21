using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SUPERLASER;
using System;

namespace SUPERLASER
{
    public class VersioningEditor : EditorWindow
    {
        private static VersioningEditor window;
        private const string VERSIONING_FILE_NAME = "Versioning";
        private const string VERSIONING_FILE_TYPE = ".txt";

        private static int majorVersion = 0;
        private static int minorVersion = 0;
        private static int patchVersion = 0;
        private enum StabilityVersion { Stable, Beta, Debug };
        private static StabilityVersion stabilityVersion = StabilityVersion.Debug;
        private static bool showVersionPrint = false;

        [MenuItem("Helpers/Open Versioning Window")]
        private static void OnEnable()
        {
            // Get existing open window or if none, make a new one
            window = GetWindow<VersioningEditor>();
            window.titleContent = new GUIContent("Versioning");

            LoadVersioningFileData();
            window.Show();
        }

        private string GetCombinedVersionString()
        {
            return
                majorVersion.ToString() + "." +
                minorVersion.ToString() + "." +
                patchVersion.ToString() + " " +
                stabilityVersion;
        }

        private void OnGUI()
        {
            GUILayout.Space(5);

            EditorGUILayout.LabelField("Current Version:  ");
            EditorGUILayout.LabelField(GetCombinedVersionString(), EditorStyles.boldLabel);

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Major Version");
            if (GUILayout.Button("+ 1")) majorVersion++;
            if (GUILayout.Button("- 1")) majorVersion--;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Minor Version");
            if (GUILayout.Button("+ 1")) minorVersion++;
            if (GUILayout.Button("- 1")) minorVersion--;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Patch Version");
            if (GUILayout.Button("+ 1")) patchVersion++;
            if (GUILayout.Button("- 1")) patchVersion--;
            GUILayout.EndHorizontal();

            if (majorVersion < 0)
                majorVersion = 0;
            if (minorVersion < 0)
                minorVersion = 0;
            if (patchVersion < 0)
                patchVersion = 0;

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Stability Version");
            if (stabilityVersion == StabilityVersion.Stable) GUI.backgroundColor = Color.cyan;
            if (GUILayout.Button("Stable")) stabilityVersion = StabilityVersion.Stable;
            GUI.backgroundColor = Color.white;

            if (stabilityVersion == StabilityVersion.Beta) GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Beta")) stabilityVersion = StabilityVersion.Beta;
            GUI.backgroundColor = Color.white;

            if (stabilityVersion == StabilityVersion.Debug) GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Debug")) stabilityVersion = StabilityVersion.Debug;
            GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            showVersionPrint = EditorGUILayout.Toggle("Show Version Print in app", showVersionPrint);

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save"))
            {
                Save();
                window.Close();
            }
            if (GUILayout.Button("Copy Version Text"))
            {
                EditorGUIUtility.systemCopyBuffer = GetCombinedVersionString();
            }
            GUILayout.EndHorizontal();
        }

        private void Save()
        {
            string[] version = { GetCombinedVersionString() + "-" + showVersionPrint.ToString() };
            File.WriteAllLines(Path.Combine(Application.dataPath, "Resources", VERSIONING_FILE_NAME + VERSIONING_FILE_TYPE), version);
            AssetDatabase.Refresh();
        }

        private static void LoadVersioningFileData()
        {
            if (!File.Exists(Path.Combine(Application.dataPath, "Resources", VERSIONING_FILE_NAME + VERSIONING_FILE_TYPE)))
            {
                string[] def = { "0.0.0 Dev-True" };
                File.WriteAllLines(Path.Combine(Application.dataPath, "Resources", VERSIONING_FILE_NAME + VERSIONING_FILE_TYPE), def);
                AssetDatabase.Refresh();
            }
            string[] version = File.ReadAllLines(Path.Combine(Application.dataPath, "Resources", VERSIONING_FILE_NAME + VERSIONING_FILE_TYPE));

            string[] combinedVersion = version[0].Split('-')[0].Split('.');
            majorVersion = int.Parse(combinedVersion[0]);
            minorVersion = int.Parse(combinedVersion[1]);

            patchVersion = int.Parse(combinedVersion[2].Split(' ')[0]);
            string revVer = string.Empty;
            stabilityVersion = combinedVersion[2].Split(' ')[1].ToEnum<StabilityVersion>();

            showVersionPrint = bool.Parse(version[0].Split('-')[1]);
        }
    }
}