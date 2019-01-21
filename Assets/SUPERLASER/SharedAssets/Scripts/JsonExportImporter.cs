using System.IO;

public static class JsonExportImporter
{
    public static void ExportToJson<T>(string path, ref T target)
    {
        using (StreamWriter stream = new StreamWriter(path))
        {
            string json = UnityEngine.JsonUtility.ToJson(target);
            stream.Write(json);
        }
    }

    public static void InportFromJson<T>(string path, ref T target)
    {
        using (StreamReader stream = new StreamReader(path))
        {
            string json = stream.ReadToEnd();
            UnityEngine.JsonUtility.FromJsonOverwrite(json, target);
        }
    }
}
