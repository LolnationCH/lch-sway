public static class ConfigurationUtils
{
    public static ProgramToLayout? GetProgramToLayout(string programTitle)
    {
        return Configuration.Instance.programsToLayout.FirstOrDefault(x => x.ProgramTitles.Any(y => programTitle.ToLower().Contains(y)));
    }
}