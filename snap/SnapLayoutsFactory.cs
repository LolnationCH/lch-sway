public static class SnapLayoutsFactory
{
    public static Dictionary<string, SnapZoneLayout> snapZoneLayouts = new()
    {
        {
            "full", new SnapZoneLayout(){ Zones = new List<SnapZone>()
            {
                new SnapZone() { Rectangle = new SnapRectangle(0, 0, 1920, 1080), Name = "Full" }
            },
            Name = "Full"}
        },
        {
            "left", new SnapZoneLayout() { Zones = new List<SnapZone>()
            {
                new SnapZone() { Rectangle = new SnapRectangle(0, 0, 960, 1080), Name = "Left" }
            },
            Name = "Left" }
        },
        {
            "right", new SnapZoneLayout(){ Zones = new List<SnapZone>()
            {
                new SnapZone() { Rectangle = new SnapRectangle(960, 0, 960, 1080), Name = "Right" }
            },
            Name = "Right" }
        },
        {
            "splitHorizontal", new SnapZoneLayout(){ Zones = new List<SnapZone>()
            {
                new SnapZone() { Rectangle = new SnapRectangle(0, 0, 960, 1080), Name = "Left" },
                new SnapZone() { Rectangle = new SnapRectangle(960, 0, 960, 1080), Name = "Right" }
            },
            Name = "Split Horizontal"}
        },
        {
            "splitVertical", new SnapZoneLayout(){ Zones = new List<SnapZone>()
            {
                new SnapZone() { Rectangle = new SnapRectangle(0, 0, 1920, 540), Name = "Top" },
                new SnapZone() { Rectangle = new SnapRectangle(0, 540, 1920, 540), Name = "Bottom" }
            },
            Name = "Split Vertical"}
        },
        {
            "quad", new SnapZoneLayout(){ Zones = new List<SnapZone>()
            {
                new SnapZone() { Rectangle = new SnapRectangle(0, 0, 960, 540), Name = "Top Left" },
                new SnapZone() { Rectangle = new SnapRectangle(960, 0, 960, 540), Name = "Top Right" },
                new SnapZone() { Rectangle = new SnapRectangle(0, 540, 960, 540), Name = "Bottom Left" },
                new SnapZone() { Rectangle = new SnapRectangle(960, 540, 960, 540), Name = "Bottom Right" }
            },
            Name = "Quad" }
        }
    };

    public static Dictionary<string, SnapZoneLayout> LayoutIndexes = new();
    public static SnapZoneLayout GetLayout(string layout = "full")
    {
        if (Configuration.Instance.customSnapLayouts.ContainsKey(layout))
        {
            if (!LayoutIndexes.ContainsKey(layout))
            {
                LayoutIndexes.Add(layout, new SnapZoneLayout() { Zones = Configuration.Instance.customSnapLayouts[layout], Name = layout });
            }
            return LayoutIndexes[layout];
        }
        else if (snapZoneLayouts.ContainsKey(layout))
        {
            return snapZoneLayouts[layout];
        }
        return snapZoneLayouts["full"];
    }
}
