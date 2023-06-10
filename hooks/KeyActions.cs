public static class KeyActions
{
    static public WindowInformation? window { get => WindowHandler.GetCurrentWindowInformation(); }

    static public SnapZoneLayout? snapZoneLayout
    {
        get
        {
            if (window == null)
                return null;

            var layout = ConfigurationUtils.GetLayoutOfProgram(window);
            if (layout == null)
                return null;

            return SnapLayoutsFactory.GetLayout(layout.Layout);
        }
    }

    public static bool KeyHasAction(Keys key)
    {
        return Configuration.Instance.KeyBindings.TryGetValue(key, out string? actionName);
    }

    public static Action? GetAction(Keys key)
    {
        if (Configuration.Instance.KeyBindings.TryGetValue(key, out string? actionName))
        {
            ActionNames.actions.TryGetValue(actionName!, out Action? action);
            return action;
        }
        return null;
    }
}