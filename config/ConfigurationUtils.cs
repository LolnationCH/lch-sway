public static class ConfigurationUtils
{
    public static ProgramToLayout? GetLayoutOfProgram(WindowInformation windowInformation)
    {
        return Configuration.Instance.programsToLayout
            .FirstOrDefault(x => GetWindowsFitConfig(new List<WindowInformation> { windowInformation }, x).Any());
    }

    private static bool ModuleFileNameContains(string moduleFileNameFilter, WindowInformation windowInformation)
    {
        if (windowInformation.ModuleFileName == null)
            return false;

        return windowInformation.ModuleFileName.ToLower().Contains(moduleFileNameFilter);
    }

    private static bool TitleContains(string title, WindowInformation windowInformation)
    {
        return windowInformation.Title.ToLower().Contains(title.ToLower());
    }

    public static List<WindowInformation> GetWindowsFitConfig(List<WindowInformation> windowInformation, ProgramToLayout programToLayout)
    {
        var windows = new HashSet<WindowInformation>();
        programToLayout.ProgramTitles.ForEach(titleFilter =>
        {
            windowInformation.Where(windowInfo => TitleContains(titleFilter, windowInfo))
                             .ToList().ForEach(x => windows.Add(x));
        });
        programToLayout.ProgramExecutableNames.ForEach(moduleFileNameFilter =>
        {
            windowInformation.Where(windowInfo => ModuleFileNameContains(moduleFileNameFilter, windowInfo))
                             .ToList().ForEach(x => windows.Add(x));
        });
        return windows.ToList();
    }
}