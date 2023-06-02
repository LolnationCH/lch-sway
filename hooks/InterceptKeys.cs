using System.Diagnostics;
using System.Runtime.InteropServices;

public static class InterceptKeys
{
    #region DLL Imports
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook,
        LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x0101;
    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
    private static LowLevelKeyboardProc _proc = HookCallback;
    private static IntPtr _hookID = IntPtr.Zero;
    #endregion

    #region private
    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule? curModule = curProcess.MainModule)
        {
            if (curModule == null)
                throw new ArgumentNullException(nameof(curModule));

            return SetWindowsHook(proc, GetModuleHandle(curModule.ModuleName));
        }
    }

    private static IntPtr SetWindowsHook(LowLevelKeyboardProc lpfn, IntPtr hMod)
    {
        return SetWindowsHookEx(WH_KEYBOARD_LL, lpfn, hMod, 0);
    }
    #endregion

    public static void SetHook(Func<HookCallbackData, IntPtr>? KeyPressedCallback)
    {
        InterceptKeys.KeyPressedCallstack = KeyPressedCallback;
        _hookID = SetHook(_proc);
        Application.Run();
        UnhookWindowsHookEx(_hookID);
    }
    static Func<HookCallbackData, IntPtr>? KeyPressedCallstack;
    static Dictionary<Keys, bool> keysPressed = new Dictionary<Keys, bool>();

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            Debug.Print((Keys)vkCode + " down");
            keysPressed[(Keys)vkCode] = true;
        }
        else if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            Debug.Print((Keys)vkCode + " up");
            keysPressed[(Keys)vkCode] = false;
        }

        IntPtr res = 0;
        if (KeyPressedCallstack != null)
            res = KeyPressedCallstack(new HookCallbackData(_hookID, nCode, wParam, lParam, keysPressed));

        if (res != 0)
            return res;

        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }
}