public static class WindowLayoutSetter
{
    public static void SetProgramsToLayout(List<WindowInformation> windowInformation)
    {
        Configuration.Instance.programsToLayout.ForEach(x => SetProgramsWithLayout(windowInformation, x));
    }

    private static void SetProgramsWithLayout(List<WindowInformation> windowInformation, ProgramToLayout programToLayout)
    {
        var windows = GetWindowsFitConfig(windowInformation, programToLayout);
        if (windows.Count == 0)
            return;

        SetWindowsToLayout(programToLayout, windows);
    }

    private static void SetWindowsToLayout(ProgramToLayout programToLayout, List<WindowInformation> windows)
    {
        var currentLayout = SnapLayoutsFactory.GetLayout(programToLayout.Layout);
        windows.ForEach(y => WindowHandler.MoveWindowToNextZone(y, currentLayout));
    }

    private static List<WindowInformation> GetWindowsFitConfig(List<WindowInformation> windowInformation, ProgramToLayout programToLayout)
    {
        var windows = new HashSet<WindowInformation>();
        programToLayout.ProgramTitles.ForEach(titleThatApplyToLayout =>
        {
            windowInformation.Where(windowInfo => TitleContains(windowInfo, titleThatApplyToLayout))
                             .ToList().ForEach(x => windows.Add(x));
        });
        return windows.ToList();
    }

    private static bool TitleContains(WindowInformation windowInformation, string title)
    {
        return windowInformation.Title.ToLower().Contains(title.ToLower());
    }
}