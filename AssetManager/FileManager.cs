using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace AssetManager;

public class FileManager
{
    public static string SavePath { get; set; } = Path.Combine(Environment.CurrentDirectory, "Data");
    public static string AssetPath { get; set; } = Path.Combine(SavePath, "Assets");
    public static string AssetExtension { get; set; } = ".asset";
    
    public FileManager()
    {
        if (!Directory.Exists(SavePath))
            Directory.CreateDirectory(SavePath);

        if (!Directory.Exists(AssetPath))
            Directory.CreateDirectory(AssetPath);
    }
    
    public void SaveAsset<T>(T asset, string name)
    {
        var path = Path.Combine(AssetPath, name + AssetExtension);
        var json = JsonConvert.SerializeObject(asset, Formatting.Indented);
        File.WriteAllText(path, json);
    }

    public T LoadAsset<T>(string name)
    {
        var path = Path.Combine(AssetPath, name + AssetExtension);
        var json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<T>(json);
    }
}




