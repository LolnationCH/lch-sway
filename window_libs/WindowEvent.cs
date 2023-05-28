using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Automation;
using System.Windows.Threading;

public class WindowEvent
{
    public WindowEvent()
    {
        BackgroundWorker worker = new();
        worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
        worker.RunWorkerAsync();
    }

    private void Worker_DoWork(object? sender, DoWorkEventArgs e)
    {
        AutomationFocusChangedEventHandler focusHandler = OnFocusChanged;
        Automation.AddAutomationFocusChangedEventHandler(focusHandler);
        while (true)
        {
            Thread.Sleep(1000);
        }
    }

    private void OnFocusChanged(object sender, AutomationFocusChangedEventArgs e)
    {
        AutomationElement? focusedElement = sender as AutomationElement;
        if (focusedElement != null)
        {
            try
            {
                int processId = focusedElement.Current.ProcessId;
                var currentProcess = Process.GetCurrentProcess();
                if (processId == currentProcess.Id)
                    return;

                WindowHandler.HandleWindows();
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}