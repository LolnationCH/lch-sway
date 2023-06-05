using System.Diagnostics;

public static class ConfigAction
{
    public static Action Refresh = () =>
    {
        TracesHandler.Print("Refresh", "Reloading config");

        Configuration.Refresh();
        WindowHandler.Refresh();
        SnapLayoutsFactory.Refresh();
    };

    public static Action OpenConfig = () =>
    {
        TracesHandler.Print("OpenConfig", "Opening config");

        var process = new Process();
        process.StartInfo.UseShellExecute = true;
        process.StartInfo.FileName = Configuration.ConfigurationPath;
        process.Start();
    };
}