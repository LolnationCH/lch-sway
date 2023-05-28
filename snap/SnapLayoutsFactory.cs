public static class SnapLayoutsFactory
{
    public static Dictionary<string, SnapZoneLayout> snapZoneLayouts = new()
    {
        {
            "full", new SnapZoneLayout()
            {
                new SnapZone() { Rectangle = new SnapRectangle(0, 0, 1920, 1080), Name = "Full" }
            }
        },
        {
            "left", new SnapZoneLayout()
            {
                new SnapZone() { Rectangle = new SnapRectangle(0, 0, 960, 1080), Name = "Left" }
            }
        },
        {
            "right", new SnapZoneLayout()
            {
                new SnapZone() { Rectangle = new SnapRectangle(960, 0, 960, 1080), Name = "Right" }
            }
        },
        {
            "splitHorizontal", new SnapZoneLayout()
            {
                new SnapZone() { Rectangle = new SnapRectangle(0, 0, 960, 1080), Name = "Left" },
                new SnapZone() { Rectangle = new SnapRectangle(960, 0, 960, 1080), Name = "Right" }
            }
        },
        {
            "splitVertical", new SnapZoneLayout()
            {
                new SnapZone() { Rectangle = new SnapRectangle(0, 0, 1920, 540), Name = "Top" },
                new SnapZone() { Rectangle = new SnapRectangle(0, 540, 1920, 540), Name = "Bottom" }
            }
        },
        {
            "quad", new SnapZoneLayout()
            {
                new SnapZone() { Rectangle = new SnapRectangle(0, 0, 960, 540), Name = "Top Left" },
                new SnapZone() { Rectangle = new SnapRectangle(960, 0, 960, 540), Name = "Top Right" },
                new SnapZone() { Rectangle = new SnapRectangle(0, 540, 960, 540), Name = "Bottom Left" },
                new SnapZone() { Rectangle = new SnapRectangle(960, 540, 960, 540), Name = "Bottom Right" }
            }
        }
    };

    public static SnapZoneLayout GetLayout(string layout = "full")
    {
        if (Configuration.Instance.customSnapLayouts.ContainsKey(layout))
        {
            return new SnapZoneLayout(Configuration.Instance.customSnapLayouts[layout]);
        }
        else if (snapZoneLayouts.ContainsKey(layout))
        {
            return snapZoneLayouts[layout];
        }
        return snapZoneLayouts["full"];
    }
}