using static PInvoke.User32;

public static class WindowHandler
{
    // TODO : Right now the lastSnapZone is the global one, we need to make it per layout.
    #region Members
    public static List<WindowInformation> windowsHandled = new();
    public static SnapZoneLayout currentLayout = SnapZoneLayouts.full;
    public static SnapZone? lastSnapZone = null;

    private static WindowEvent windowEvent = new();
    #endregion

    #region Api
    public static void HandleWindows()
    {
        var windows = WindowFetcher.GetWindowsOnCurrentScreen();
        WindowLayoutSetter.SetProgramsToLayout(windows);
    }

    public static void MoveWindowToNextZone(WindowInformation window, SnapZoneLayout layout)
    {
        if (IsWindowHandled(window)) { return; }
        currentLayout = layout;
        lastSnapZone = GetNextSnapZone();
        MoveWindowToZone(window, lastSnapZone);
    }

    public static void MoveWindowToPreviousZone(WindowInformation window, SnapZoneLayout layout)
    {
        if (IsWindowHandled(window)) { return; }
        currentLayout = layout;
        lastSnapZone = GetPreviousSnapZone();
        MoveWindowToZone(window, lastSnapZone);
    }

    public static void MoveWindowToCurrentZone(WindowInformation window, SnapZoneLayout layout)
    {
        if (IsWindowHandled(window)) { return; }
        currentLayout = layout;
        if (lastSnapZone == null)
            lastSnapZone = GetDefaultSnapZone();
        MoveWindowToZone(window, lastSnapZone);
    }

    public static void MoveWindowToZone(WindowInformation window, SnapZone snapZone)
    {
        windowsHandled.Add(window);
        TracesHandler.PrintObj("Moving", new() { { "title", window.Title }, { "zone", snapZone } });
        var handle = window.Handle;

        UnMaximizeWindow(handle);
        RemoveMaximizeBoxStyle(handle);
        SetWindowPosition(snapZone, handle);
        SetForegroundWindow(handle);
        AddingMaximizeBoxStyle(handle);
    }

    public static void ResetSnapZoneIndex()
    {
        lastSnapZone = null;
    }
    #endregion

    #region Private
    private static void RemoveMaximizeBoxStyle(nint handle)
    {
        var style = (SetWindowLongFlags)GetWindowLong(handle, WindowLongIndexFlags.GWL_STYLE);
        style &= ~SetWindowLongFlags.WS_MAXIMIZEBOX;
        SetWindowLong(handle, WindowLongIndexFlags.GWL_STYLE, style);
    }

    private static void UnMaximizeWindow(nint handle)
    {
        ShowWindow(handle, WindowShowStyle.SW_RESTORE);
    }

    private static void AddingMaximizeBoxStyle(nint handle)
    {
        var style = (SetWindowLongFlags)GetWindowLong(handle, WindowLongIndexFlags.GWL_STYLE);
        style |= SetWindowLongFlags.WS_MAXIMIZEBOX;
        SetWindowLong(handle, WindowLongIndexFlags.GWL_STYLE, style);
    }

    private static void SetWindowPosition(SnapZone snapZone, nint handle)
    {
        SetWindowPos(handle, SpecialWindowHandles.HWND_TOP,
                             snapZone.Bounds.Left, snapZone.Bounds.Top, snapZone.Bounds.Width, snapZone.Bounds.Height,
                             SetWindowPosFlags.SWP_SHOWWINDOW);
    }

    private static SnapZone GetNextSnapZone()
    {
        if (lastSnapZone == null)
            return currentLayout[0];

        var index = currentLayout.IndexOf(lastSnapZone);
        if (index == currentLayout.Count - 1)
            return currentLayout[0];

        return currentLayout[index + 1];
    }

    private static SnapZone GetPreviousSnapZone()
    {
        if (lastSnapZone == null)
            return currentLayout[0];

        var index = currentLayout.IndexOf(lastSnapZone);
        if (index == 0)
            return currentLayout[currentLayout.Count - 1];

        return currentLayout[index - 1];
    }

    private static bool IsWindowHandled(WindowInformation window)
    {
        return windowsHandled.Any(x => x.Handle == window.Handle);
    }

    private static SnapZone GetDefaultSnapZone()
    {
        return currentLayout.First();
    }
    #endregion
}