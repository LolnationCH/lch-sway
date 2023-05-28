public static class ConfigurationDefaultData
{
    public static List<ProgramToLayout> programsToLayout { get; set; } = new() { new ProgramToLayout() };

    public static Dictionary<string, List<SnapZone>> customSnapLayouts { get; set; } = new() { { "Custom Layout", new List<SnapZone>(){
        new SnapZone() { Rectangle = new SnapRectangle(0, 0, 960, 1080), Name = "Left" },
        new SnapZone() { Rectangle = new SnapRectangle(960, 0, 960, 1080), Name = "Right" }
    } } };
}