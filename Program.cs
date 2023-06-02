// LCH-Sway : Inspired by Sway, made for windows.
// Author: LolnationCH

TracesHandler.Run();
WindowHandler.HandleWindows();
InterceptKeys.SetHook(HookHandler.KeyPressedCallback);

// Keep the program running
while (true)
{
    Thread.Sleep(1000);
}