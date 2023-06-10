public static class ConfigurationDefaultData
{
    public static List<ProgramToLayout> programsToLayout { get; internal set; } = new() { new ProgramToLayout() };

    public static Dictionary<string, List<SnapZone>> customSnapLayouts { get; internal set; } = new() { { "Custom Layout", new List<SnapZone>(){
        new SnapZone() { Rectangle = new SnapRectangle(0, 0, 960, 1080), Name = "Left" },
        new SnapZone() { Rectangle = new SnapRectangle(960, 0, 960, 1080), Name = "Right" }
    } } };
    public static Dictionary<Keys, string> KeyBindings { get; internal set; } = new() {
        { Keys.Left,  "MoveToPreviousZone" },
        { Keys.Right, "MoveToNextZone" },
        { Keys.Up,    "MaximizeCurrentWindow" },
        { Keys.Down,  "SetCurrentWindowToLastZone" },
        { Keys.V ,    "Exit" },
        { Keys.R,     "Refresh" },
        { Keys.C,     "OpenConfiguration" }
    };
}