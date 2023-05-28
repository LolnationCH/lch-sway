public class SnapZone
{
    public required Rectangle Bounds { get; set; }
    public required string Name { get; set; }

    public override string ToString()
    {
        return $"Name => \"{Name}\" ({Bounds.ToString()})";
    }
}

public class SnapZoneLayout : List<SnapZone> { }