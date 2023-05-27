using System.Diagnostics;

public static class TracesHandler
{
    public static void Run()
    {
        TextWriterTraceListener writer = new TextWriterTraceListener("trace.log");
        Trace.Listeners.Add(writer);
        Trace.AutoFlush = true;
        Debug.WriteLine(new string('=', 75));
        Debug.WriteLine($"Date : {DateTime.Now}");
    }

    public static void Print(string command, string message)
    {
        Debug.WriteLine($"{command} : {message}");
    }

    public static void PrintObj(string command, Dictionary<string, object> messages)
    {
        Debug.WriteLine($"{command} :");
        Debug.Indent();

        var maxLenght = messages.Keys.Max(x => x.Length);
        foreach (var message in messages)
        {
            Debug.WriteLine($"{message.Key.PadRight(maxLenght)} : {message.Value}");
        }
        Debug.Unindent();
    }
}