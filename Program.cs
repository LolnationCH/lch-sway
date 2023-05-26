var windows = WindowFetcher.GetWindowsOnCurrentScreen();

var w = windows.Where(x => x.Title.ToLower().Contains("firefox")).ToList();

WindowHandler.currentLayout = SnapZoneLayouts.splitHorizontal;

w.ToList().ForEach(x => WindowHandler.MoveWindowToNextZone(x));