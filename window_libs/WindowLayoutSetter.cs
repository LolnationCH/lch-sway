public static class WindowLayoutSetter
{
    public static void SetProgramsToLayout(List<WindowInformation> windowInformation)
    {
        Configuration.Instance.programsToLayout.ForEach(x => SetProgramsWithLayout(windowInformation, x));
    }

    private static void SetProgramsWithLayout(List<WindowInformation> windowInformation, ProgramToLayout programToLayout)
    {
        var windows = ConfigurationUtils.GetWindowsFitConfig(windowInformation, programToLayout);
        if (windows.Count == 0)
            return;

        SetWindowsToLayout(programToLayout, windows);
    }

    private static void SetWindowsToLayout(ProgramToLayout programToLayout, List<WindowInformation> windows)
    {
        var currentLayout = SnapLayoutsFactory.GetLayout(programToLayout.Layout);
        windows.ForEach(y => WindowHandler.MoveWindowToNextZone(y, currentLayout));
    }
}