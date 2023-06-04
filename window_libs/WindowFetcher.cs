using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

public class WindowFetcher
{
    #region User32.dll imports
    [DllImport("user32.dll")]
    private static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
    #endregion

    [DllImport("coredll.dll", SetLastError = true)]
    private static extern int GetModuleFileName(IntPtr hModule, StringBuilder lpFilename, int nSize);
    [DllImport("kernel32.dll")]
    static extern int GetProcessId(IntPtr handle);

    public static List<WindowInformation> GetWindowsOnCurrentScreen()
    {
        List<WindowInformation> windows = new List<WindowInformation>();
        IntPtr desktopWindow = GetDesktopWindow();
        EnumChildWindows(desktopWindow, (hWnd, lParam) =>
        {
            if (!IsWindowVisible(hWnd))
                return true;

            GetWindowInformationByHandle(hWnd).IfNotNull(window =>
            {
                windows.Add(window!);
                TracesHandler.Print("WindowFetcher", windows.Last().ToString());
            });

            return true;
        }, IntPtr.Zero);
        return windows;
    }

    public static WindowInformation? GetWindowInformationByHandle(IntPtr hWnd, bool applyRules = true)
    {
        string title = GetWindowTitle(hWnd);
        if (string.IsNullOrEmpty(title))
            return null;

        string className = GetWindowClassName(hWnd);
        if (ShouldIgnoreWindow(className) && applyRules)
            return null;

        RECT rect;
        if (!GetWindowRect(hWnd, out rect) && applyRules)
            return null;

        Rectangle windowBounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
        string? moduleFileName = WindowModuleGetter.GetWindowModuleFileName(hWnd);

        return new WindowInformation()
        {
            Handle = hWnd,
            Title = title,
            ClassName = className,
            Location = new Point() { X = rect.Left, Y = rect.Top },
            Size = new Point() { X = rect.Right - rect.Left, Y = rect.Bottom - rect.Top },
            Rect = windowBounds,
            ModuleFileName = moduleFileName
        };
    }

    private static IEnumerable<string> getClassNameToIgnore()
    {
        yield return "Shell_TrayWnd";
        yield return "Windows.UI.Core.CoreComponentInputSource";
        yield return "Windows.UI.Core.CoreWindow";
        yield return "Progman";
        yield return "Windows.UI.Composition.DesktopWindowContentBridge";
        yield return "MSTaskSwWClass";
        yield return "ToolbarWindow32";
        yield return "Chrome_RenderWidgetHostHWND";
    }

    private static string GetWindowTitle(IntPtr hWnd)
    {
        StringBuilder sbWindowText = new StringBuilder(256);
        GetWindowText(hWnd, sbWindowText, sbWindowText.Capacity);
        return sbWindowText.ToString();
    }

    private static string GetWindowClassName(IntPtr hWnd)
    {
        StringBuilder sbClassName = new StringBuilder(256);
        GetClassName(hWnd, sbClassName, sbClassName.Capacity);
        return sbClassName.ToString();
    }

    private static bool ShouldIgnoreWindow(string className)
    {
        return getClassNameToIgnore().Contains(className);
    }

    private static IntPtr GetDesktopWindow()
    {
        return IntPtr.Zero; // implementation omitted for brevity
    }
}