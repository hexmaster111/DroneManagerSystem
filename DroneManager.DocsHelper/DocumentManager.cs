namespace DroneManager.DocsHelper;

public static class DocumentManager
{
    public static List<Document> GetDocuments()
    {
        var documents = new List<Document>();
        var path = Path.Combine(Environment.CurrentDirectory, "docs");
        var files = Directory.GetFiles(path, "*.ddoc", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            var document = new Document
            {
                Name = Path.GetFileNameWithoutExtension(file),
                Path = file,
                Content = GetDocumentContent(file)
            };
            documents.Add(document);
        }
        return documents;
    }

    private static DocumentContent GetDocumentContent(string path)
    {
        var content = File.ReadAllLines(path);
        var headers = content.Where(x => x.StartsWith("#")).ToArray();
        return new DocumentContent
        {
            Content = content,
            Headers = headers
        };
    }
}