using System.Diagnostics.CodeAnalysis;

public class HookCallbackData
{
    public required IntPtr hhk;
    public required int nCode;
    public required IntPtr wParam;
    public required IntPtr lParam;
    public required Dictionary<Keys, bool> keysPressed;

    [SetsRequiredMembers]
    public HookCallbackData(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam, Dictionary<Keys, bool> keysPressed)
    {
        this.hhk = hhk;
        this.nCode = nCode;
        this.wParam = wParam;
        this.lParam = lParam;
        this.keysPressed = keysPressed;
    }

    public override string ToString()
    {
        var keyStates = string.Join(Environment.NewLine, keysPressed.Keys.Select(key => $"{key}: {keysPressed[key]}"));
        return $"HookCallbackData: hhk: {hhk}, nCode: {nCode}, wParam: {wParam}, lParam: {lParam}{Environment.NewLine}key states: {keyStates}";
    }

    public bool IsKeyDown(Keys key)
    {
        if (keysPressed.ContainsKey(key))
            return keysPressed[key];
        return false;
    }

    private bool IsKeysDown(List<Keys> key)
    {
        return key.Any(IsKeyDown);
    }

    public int GetModifier()
    {
        int modifier = 0;
        modifier |= IsKeysDown(new() { Keys.LControlKey, Keys.RControlKey }) ? (int)ModEnum.MOD_CONTROL : 0;
        modifier |= IsKeysDown(new() { Keys.LShiftKey, Keys.RShiftKey }) ? (int)ModEnum.MOD_SHIFT : 0;
        modifier |= IsKeysDown(new() { Keys.LMenu, Keys.RMenu }) ? (int)ModEnum.MOD_ALT : 0;
        modifier |= IsKeysDown(new() { Keys.LWin, Keys.RWin }) ? (int)ModEnum.MOD_WIN : 0;
        return modifier;
    }
}