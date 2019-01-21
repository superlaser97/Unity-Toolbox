using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Text;
using SUPERLASER;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SUPERLASER
{
    public class FeatureFlags : MonoBehaviour
    {
        private static FeatureFlags instance;
        public static FeatureFlags Instance
        {
            get
            {
                if (instance)
                    return instance;
                else
                {
                    return Instantiate(Resources.Load<GameObject>("SUPERLASER/FeatureFlags/FeatureFlags")).GetComponent<FeatureFlags>();
                }
            }
        }

        public static readonly string FLAGNOTFOUNDERROR = "FLAGNOTFOUNDERROR";
        private string FlagsStoragePath;
        private Dictionary<Flags, string> FlagsDictionary = new Dictionary<Flags, string>();

        public enum Flags
        {
            NULL = 0,
            FLAG1,
            FLAG2,
        }

        private void LoadDefaultFlagValues()
        {
            FlagsDictionary.Add(Flags.FLAG1, "1");
            FlagsDictionary.Add(Flags.FLAG2, "0");
        }

#if UNITY_EDITOR
        [MenuItem("Helpers/FeatureFlags/Open File")]
        public static void OpenConfigFile()
        {
            Application.OpenURL(Path.Combine(Application.persistentDataPath, "Config/Flags.txt"));
        }
        [MenuItem("Helpers/FeatureFlags/Open in Explorer")]
        public static void OpenConfigFileInExplorer()
        {
            Application.OpenURL(Path.Combine(Application.persistentDataPath, "Config"));
        }
#endif

        [RuntimeInitializeOnLoadMethod]
        private static void InitalizeSelf()
        {
            var featureFlags = Instance;
        }

        private void OnEnable()
        {
            if (!instance)
                instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            FlagsStoragePath = Path.Combine(Application.persistentDataPath, "Config/Flags.txt");
            DebugTools.Log("FlagsStoragePath: " + FlagsStoragePath);

            if (!File.Exists(FlagsStoragePath))
            {
                DebugTools.Log("Flags File doesnt exist, creating new file", DebugTools.DebugLevel.WARNING);
                LoadDefaultFlagValues();
                SaveFlagsToFile();
            }
            else
            {
                LoadFlagsFromFile();
            }
        }

        private void LoadFlagsFromFile()
        {
            List<string> flags = File.ReadAllLines(FlagsStoragePath).ToList();

            if (flags.Count >= 1)
            {
                if (flags[0].ToString().Split('-')[0] != Versioning.GetVersion())
                {
                    LoadDefaultFlagValues();
                    Debug.LogWarning("Different Unity Version, loading default flag values instead");
                    SaveFlagsToFile();
                    return;
                }
            }

            DebugTools.Log("");
            DebugTools.Log("Loading Flags");
            DebugTools.Log("-------------------");
            foreach (string flag in flags.Skip(1))
            {
                try
                {
                    if (flag.Length < 1)
                        continue;
                    string flagName = flag.Split('=')[0];
                    string flagContent = flag.Split('=')[1];

                    Flags flagENUM = flagName.ToEnum<Flags>();
                    if (flagENUM == Flags.NULL)
                    {
                        DebugTools.Log($"Invalid Flag ({flagName}), skipping flag addition", DebugTools.DebugLevel.ERROR);
                    }
                    DebugTools.Log(flag);

                    FlagsDictionary.Add(flagENUM, flagContent);
                }
                catch (Exception e)
                {
                    DebugTools.Log(e.ToString(), DebugTools.DebugLevel.ERROR);
                }
            }
            DebugTools.Log("-------------------");
            DebugTools.Log("Flag load complete");
            DebugTools.Log("");
        }

        private void SaveFlagsToFile()
        {
            string flags = string.Empty;
            flags += Versioning.GetVersion(true);
            flags += Environment.NewLine;
            foreach (var flag in FlagsDictionary)
            {
                flags += flag.Key.ToString() + "=" + flag.Value.ToString();
                flags += Environment.NewLine;
            }
            try
            {
                DebugTools.Log("Runtime: " + Application.platform.ToString());

                if (!Directory.Exists(Path.GetDirectoryName(FlagsStoragePath)))
                {
                    DebugTools.Log("Directory Path not found, creating path");
                    Directory.CreateDirectory(Path.GetDirectoryName(FlagsStoragePath));
                }

                if (!File.Exists(FlagsStoragePath))
                {
                    File.Create(FlagsStoragePath).Dispose();

                    using (StreamWriter sr = new StreamWriter(FlagsStoragePath, false))
                    {
                        sr.Write(flags);
                    }
                }

                DebugTools.Log("Saved Flags File to " + FlagsStoragePath);
            }
            catch (Exception e)
            {
                DebugTools.Log(e.ToString(), DebugTools.DebugLevel.ERROR);
                DebugTools.Log("Failed to save Flags File");
            }
        }

        public string GetFlagValue(Flags flag)
        {
            if (FlagsDictionary.ContainsKey(flag))
                return FlagsDictionary[flag];
            else
                return FLAGNOTFOUNDERROR;
        }
    }
}