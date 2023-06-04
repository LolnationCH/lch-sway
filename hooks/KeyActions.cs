public static class KeyActions
{
    static public WindowInformation? window { get => WindowHandler.GetCurrentWindowInformation(); }

    static public SnapZoneLayout? snapZoneLayout
    {
        get
        {
            if (window == null)
                return null;

            var layout = ConfigurationUtils.GetLayoutOfProgram(window);
            if (layout == null)
                return null;

            return SnapLayoutsFactory.GetLayout(layout.Layout);
        }
    }

    public static Dictionary<Keys, Action> actions = new(){
        { Keys.Left, MovementActions.LeftMovement },
        { Keys.Right, MovementActions.RightMovement },
        { Keys.Up, MovementActions.UpMovement },
        { Keys.Down, MovementActions.DownMovement },
        { Keys.V , () => Environment.Exit(0) }
    };
}