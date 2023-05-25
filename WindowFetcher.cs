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

    public static List<WindowInformation> GetWindowsOnCurrentScreen()
    {
        List<WindowInformation> windows = new List<WindowInformation>();
        IntPtr desktopWindow = GetDesktopWindow();
        EnumChildWindows(desktopWindow, (hWnd, lParam) =>
        {
            if (!IsWindowVisible(hWnd))
                return true;

            string title = GetWindowTitle(hWnd);
            if (string.IsNullOrEmpty(title))
                return true;

            string className = GetWindowClassName(hWnd);
            if (ShouldIgnoreWindow(className))
                return true;

            RECT rect;
            if (!GetWindowRect(hWnd, out rect))
                return true;

            Rectangle windowBounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            Screen currentScreen = Screen.FromRectangle(windowBounds);
            if (!currentScreen.Primary)
                return true;

            windows.Add(new WindowInformation()
            {
                Handle = hWnd,
                Title = title,
                ClassName = className,
                Location = new Point() { X = rect.Left, Y = rect.Top },
                Size = new Point() { X = rect.Right - rect.Left, Y = rect.Bottom - rect.Top }
            });

            return true;
        }, IntPtr.Zero);
        return windows;
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