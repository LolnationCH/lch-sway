public static class HookHandler
{
    static public nint KeyPressedCallback(HookCallbackData data)
    {
        try
        {
            if (IsHyperPressed(data.GetModifier()))
            {
                try
                {
                    if (HandleKeyDown(data))
                    {
                        data.ResetKeys();
                        return 1;
                    }

                    return 0;
                }
                finally
                {
                    WindowHandler.IgnoredHandle = false;
                }
            }
        }

        catch (Exception e)
        {
            TracesHandler.Print("KeyPressedCallback", e.ToString());
            return 0;
        }

        return 0;
    }

    static private bool IsHyperPressed(int modifierCode)
    {
        return modifierCode == ((int)ModEnum.MOD_CONTROL |
                                (int)ModEnum.MOD_SHIFT |
                                (int)ModEnum.MOD_ALT |
                                (int)ModEnum.MOD_WIN);
    }

    static private bool HandleKeyDown(HookCallbackData data)
    {
        WindowHandler.IgnoredHandle = true;
        TracesHandler.Print("Keyboard hook", "Hyper pressed");
        var act = data.GetAction();
        if (act != null)
        {
            TracesHandler.Print("Keyboard hook", "Action found");
            act();
            return true;
        }

        return false;
    }
}