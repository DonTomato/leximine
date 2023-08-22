using System.Runtime.InteropServices;
using Microsoft.Web.WebView2.Core;

namespace leximine.CsTmp;

class Program
{
    private static readonly IntPtr HWND_MESSAGE = new IntPtr(-3);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr CreateWindowEx(
        uint dwExStyle, string lpClassName, string lpWindowName, uint dwStyle, int x, int y,
        int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern Boolean DestroyWindow(IntPtr hWnd);
    
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting WebView2 browser...");

        IntPtr dummy = CreateWindowEx(0, "Message", null, 0, 0, 0, 0, 0,
            HWND_MESSAGE, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

        var browserEnv = await CoreWebView2Environment.CreateAsync(@"C:\Program Files (x86)\Microsoft\EdgeWebView\Application\115.0.1901.203");

        var browserController = await browserEnv.CreateCoreWebView2ControllerAsync(dummy);

        // This never executes
        Console.ReadKey();

        DestroyWindow(dummy);
    }
}