public static class RefreshAction
{
    public static Action Refresh = () =>
    {
        TracesHandler.Print("Refresh", "Reloading config");

        Configuration.Refresh();
        WindowHandler.Refresh();
        SnapLayoutsFactory.Refresh();
    };
}