public static class ActionNames
{
    public static Dictionary<string, Action> actions { get; internal set; } = new(){
        { "MoveToPreviousZone", MovementActions.LeftMovement },
        { "MoveToNextZone", MovementActions.RightMovement },
        { "MaximizeCurrentWindow", MovementActions.UpMovement },
        { "SetCurrentWindowToLastZone", MovementActions.DownMovement },
        { "Exit" , () => Environment.Exit(0) },
        { "Refresh", ConfigAction.Refresh },
        { "OpenConfiguration", ConfigAction.OpenConfig }
    };
}