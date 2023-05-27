TracesHandler.Run();

var windows = WindowFetcher.GetWindowsOnCurrentScreen();
WindowLayoutSetter.SetProgramsToLayout(windows);
