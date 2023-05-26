using static PInvoke.User32;

public static class WindowHandler
{
    public static SnapZoneLayout currentLayout = SnapZoneLayouts.full;
    public static SnapZone? lastSnapZone = null;

    public static void MoveWindowToNextZone(WindowInformation window)
    {
        lastSnapZone = GetNextSnapZone();
        MoveWindowToZone(window, lastSnapZone);
    }

    public static void MoveWindowToPreviousZone(WindowInformation window)
    {
        lastSnapZone = GetPreviousSnapZone();
        MoveWindowToZone(window, lastSnapZone);
    }

    public static void MoveWindowToCurrentZone(WindowInformation window)
    {
        if (lastSnapZone == null)
            lastSnapZone = GetDefaultSnapZone();
        MoveWindowToZone(window, lastSnapZone);
    }

    public static void MoveWindowToZone(WindowInformation window, SnapZone snapZone)
    {
        var handle = window.Handle;

        RemoveMaximizeBoxStyle(handle);
        SetWindowPosition(snapZone, handle);
        SetForegroundWindow(handle);
    }

    public static void ResetSnapZoneIndex()
    {
        lastSnapZone = null;
    }

    private static void RemoveMaximizeBoxStyle(nint handle)
    {
        var style = (SetWindowLongFlags)GetWindowLong(handle, WindowLongIndexFlags.GWL_STYLE);
        style &= ~SetWindowLongFlags.WS_MAXIMIZEBOX;
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

    private static SnapZone GetDefaultSnapZone()
    {
        return currentLayout[0];
    }
}