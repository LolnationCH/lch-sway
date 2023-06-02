public static class MovementActions
{
    public static Action LeftMovement = () =>
    {
        TracesHandler.Print("Keyboard hook", "Left pressed");
        if (KeyActions.window == null || KeyActions.snapZoneLayout == null)
            return;
        WindowHandler.MoveWindowToNextZone(KeyActions.window, KeyActions.snapZoneLayout);
    };

    public static Action RightMovement = () =>
    {
        TracesHandler.Print("Keyboard hook", "Right pressed");
        if (KeyActions.window == null || KeyActions.snapZoneLayout == null)
            return;
        WindowHandler.MoveWindowToPreviousZone(KeyActions.window, KeyActions.snapZoneLayout);
    };

    public static Action UpMovement = () =>
    {
        TracesHandler.Print("Keyboard hook", "Up pressed");
        if (KeyActions.window == null || KeyActions.snapZoneLayout == null)
            return;
        WindowHandler.MoveWindowToCurrentZone(KeyActions.window, KeyActions.snapZoneLayout);
    };

    public static Action DownMovement = () =>
    {
        TracesHandler.Print("Keyboard hook", "Down pressed");
        if (KeyActions.window == null)
            return;
        WindowHandler.MaximaizeWindow(KeyActions.window);
    };
}