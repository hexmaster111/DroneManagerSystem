using TestAssetImpl;

namespace AssetImplTests // Note: actual namespace depends on the project name.
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            var asset = new TestAssetImpl.TestAssetImpl();
            var a = 1;
        }
    }
}