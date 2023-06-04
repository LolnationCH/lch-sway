
public class ProgramToLayout
{
    public List<string> ProgramTitles { get; set; } = new() { "visual studio", "visual studio code" };

    public List<string> ProgramExecutableNames { get; set; } = new() { "devenv.exe", "code.exe" };

    public string Layout { get; set; } = "splitHorizontal";

    public bool IsWindowForLayout(WindowInformation window)
    {
        return ProgramTitles.Any(title => window.Title.ToLower().Contains(title));
    }
}