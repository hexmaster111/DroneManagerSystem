
namespace DroneManager.DocsHelper;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("Hello World!");
        Console.WriteLine(DocumentManager.GetDocuments()[0].Name);
        Console.WriteLine(DocumentManager.GetDocuments()[0].Content.Content[0]);
    }
}