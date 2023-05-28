public class SnapZone
{
    public required SnapRectangle Rectangle { get; set; }
    public required string Name { get; set; }

    public override string ToString()
    {
        return $"Name => \"{Name}\" ({Rectangle.ToString()})";
    }
}

public class SnapZoneLayout : List<SnapZone>
{
    public SnapZoneLayout() : base() { }

    public SnapZoneLayout(IEnumerable<SnapZone> collection) : base(collection) { }
}