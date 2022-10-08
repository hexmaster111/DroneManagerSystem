using System.Runtime.InteropServices;

namespace HaileysHelpers;

public static class WpfConsoleHelper
{
    [DllImport("Kernel32")]
    private static extern void AllocConsole();

    [DllImport("Kernel32")]
    private static extern void FreeConsole();


    public static void ShowConsole()
    {
        AllocConsole();
    }
    
    public static void HideConsole()
    {
        FreeConsole();
    }
}