
namespace AssetManager;

public class AssetManager
{
    private static AssetManager _instance = null;
    public static AssetManager Instance => _instance ??= new AssetManager();

    public FileManager FileManager = new();
    
    public AssetManager()
    {
        if(_instance != null)
            throw new Exception("AssetManager is a singleton, use AssetManager.Instance instead");
        _instance = this;
    }
    
    
}