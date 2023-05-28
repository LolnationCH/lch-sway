using static PInvoke.User32;

public static class WindowHandler
{
    #region Members
    public static List<WindowInformation> windowsHandled = new();
    public static SnapZoneLayout currentLayout = SnapLayoutsFactory.full;

    public static Dictionary<SnapZoneLayout, int> LayoutIndexes = new();

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
        MoveWindowToZone(window, layout, GetNextSnapZone);
    }

    public static void MoveWindowToPreviousZone(WindowInformation window, SnapZoneLayout layout)
    {
        MoveWindowToZone(window, layout, GetPreviousSnapZone);
    }

    public static void MoveWindowToCurrentZone(WindowInformation window, SnapZoneLayout layout)
    {
        MoveWindowToZone(window, layout, GetCurrentSnapZone);
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
        LayoutIndexes = new();
    }
    #endregion

    #region Private
    private static void MoveWindowToZone(WindowInformation window, SnapZoneLayout layout, Func<SnapZone> getZone)
    {
        if (IsWindowHandled(window)) { return; }
        currentLayout = layout;
        var snapZone = getZone();
        MoveWindowToZone(window, snapZone);
    }

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
        if (LayoutIndexes.ContainsKey(currentLayout))
            LayoutIndexes[currentLayout] = IncrementIndexCurrentLayout(LayoutIndexes[currentLayout]);
        else
            LayoutIndexes.Add(currentLayout, 0);

        return GetCurrentSnapZone();
    }

    private static SnapZone GetPreviousSnapZone()
    {
        if (LayoutIndexes.ContainsKey(currentLayout))
            LayoutIndexes[currentLayout] = DecrementIndexCurrentLayout(LayoutIndexes[currentLayout]);
        else
            LayoutIndexes.Add(currentLayout, currentLayout.Count - 1);

        return GetCurrentSnapZone();
    }

    private static SnapZone GetCurrentSnapZone()
    {
        return currentLayout[LayoutIndexes[currentLayout]];
    }

    private static int IncrementIndexCurrentLayout(int index)
    {
        if (index == currentLayout.Count - 1)
            return 0;
        else
            return index + 1;
    }

    private static int DecrementIndexCurrentLayout(int index)
    {
        if (index == 0)
            return currentLayout.Count - 1;
        else
            return index - 1;
    }

    private static bool IsWindowHandled(WindowInformation window)
    {
        return windowsHandled.Any(x => x.Handle == window.Handle);
    }
    #endregion
}