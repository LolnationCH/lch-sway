public class SnapZone
{
    public required SnapRectangle Rectangle { get; set; }
    public required string Name { get; set; }

    public override string ToString()
    {
        return $"Name => \"{Name}\" ({Rectangle.ToString()})";
    }
}

public class SnapZoneLayout
{
    public required List<SnapZone> Zones { get; set; }

    public required string Name { get; set; }

    public SnapZoneLayout() { }
}