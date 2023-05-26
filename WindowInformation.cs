public class Point
{
    public required int X { get; set; }

    public required int Y { get; set; }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}

public class WindowInformation
{
    public required string Title { get; set; }

    public required IntPtr Handle { get; set; }

    public required string ClassName { get; set; }

    public required Point Location { get; set; }

    public required Point Size { get; set; }

    public required Rectangle Rect { get; set; }

    public override string ToString()
    {
        return $"Title: {Title}, ClassName: {ClassName}, Location: {Location}, Size: {Size}";
    }
}