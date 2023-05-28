public static class SnapLayoutsFactory
{
    public static SnapZoneLayout full = new(){
        new SnapZone() { Bounds = new Rectangle(0, 0, 1920, 1080), Name = "Full" },
    };

    public static SnapZoneLayout left = new(){
        new SnapZone() { Bounds = new Rectangle(0, 0, 960, 1080), Name = "Left" },
    };

    public static SnapZoneLayout right = new(){
        new SnapZone() { Bounds = new Rectangle(960, 0, 960, 1080), Name = "Right" },
    };

    public static SnapZoneLayout splitHorizontal = new(){
        new SnapZone() { Bounds = new Rectangle(0, 0, 960, 1080), Name = "Left" },
        new SnapZone() { Bounds = new Rectangle(960, 0, 960, 1080), Name = "Right" },
    };

    public static SnapZoneLayout splitVertical = new(){
        new SnapZone() { Bounds = new Rectangle(0, 0, 1920, 540), Name = "Top" },
        new SnapZone() { Bounds = new Rectangle(0, 540, 1920, 540), Name = "Bottom" },
    };

    public static SnapZoneLayout quad = new(){
        new SnapZone() { Bounds = new Rectangle(0, 0, 960, 540), Name = "Top Left" },
        new SnapZone() { Bounds = new Rectangle(960, 0, 960, 540), Name = "Top Right" },
        new SnapZone() { Bounds = new Rectangle(0, 540, 960, 540), Name = "Bottom Left" },
        new SnapZone() { Bounds = new Rectangle(960, 540, 960, 540), Name = "Bottom Right" },
    };

    public static SnapZoneLayout GetLayout(string layout)
    {
        return layout switch
        {
            "full" => full,
            "left" => left,
            "right" => right,
            "splitHorizontal" => splitHorizontal,
            "splitVertical" => splitVertical,
            "quad" => quad,
            _ => full,
        };
    }
}