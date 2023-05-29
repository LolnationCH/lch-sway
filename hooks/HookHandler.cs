public static class HookHandler
{
    static public nint KeyPressedCallstack(HookCallbackData data)
    {
        if (IsHyperPressed(data.GetModifier()))
        {
            Console.WriteLine("Hyper pressed");
            return 1;
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
}