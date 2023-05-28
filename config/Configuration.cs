using System.IO;
using System.Text.Json;

public class Configuration
{
    public List<ProgramToLayout> programsToLayout { get; set; } = ConfigurationDefaultData.programsToLayout;

    public Dictionary<string, List<SnapZone>> customSnapLayouts { get; set; } = ConfigurationDefaultData.customSnapLayouts;

    #region generic setter
    public void prepend(string key, string value)
    {
        var prop = GetProperty(key);

        switch (prop.PropertyType)
        {
            case Type t when t == typeof(string):
                FormatString(key, value, "{0}{1}");
                break;
            case Type t when t == typeof(List<string>):
                InsertList(key, value, 0);
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Operation not supported for property '{key}', type '{prop.PropertyType}'. It must be of type string or List<string>");
                Console.ResetColor();
                break;
        }
    }

    public void append(string key, string value)
    {
        var prop = GetProperty(key);

        switch (prop.PropertyType)
        {
            case Type t when t == typeof(string):
                FormatString(key, value, "{1}{0}");
                break;
            case Type t when t == typeof(List<string>):
                InsertList(key, value, -1);
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Operation not supported for property '{key}', type '{prop.PropertyType}'. It must be of type string or List<string>");
                Console.ResetColor();
                break;
        }
    }

    public void set(string key, IEnumerable<string> value)
    {
        var prop = GetProperty(key);

        switch (prop.PropertyType)
        {
            case Type t when t == typeof(string):
                prop.SetValue(this, string.Join(" ", value));
                break;
            case Type t when t == typeof(List<string>):
                var list = new List<string>();
                prop.SetValue(this, value.ToList());
                break;
            default:
                var parseMethod = prop.PropertyType.GetMethod("Parse", new Type[] { typeof(string) });
                if (parseMethod == null)
                {
                    throw new Exception($"Property '{key}' of type '{prop.PropertyType}' does not have a Parse method.");
                }
                var res = parseMethod.Invoke(null, new object[] { value.First() });
                prop.SetValue(this, res);
                break;
        }
    }

    public object? get(string key)
    {
        var prop = GetProperty(key);
        return prop.GetValue(this);
    }

    public void remove(string key)
    {
        var prop = GetProperty(key);
        var defaultValue = GetPropertyDefaultValue(key);

        switch (prop.PropertyType)
        {
            default:
                prop.SetValue(this, defaultValue);
                break;
        }
    }

    private void FormatString(string key, string value, string format)
    {
        var prop = GetProperty(key);
        var currentValue = prop.GetValue(this) as string;
        prop.SetValue(this, string.Format(format, value, currentValue));
    }

    private void InsertList(string key, string value, int index)
    {
        var prop = GetProperty(key);
        var currentValue = prop.GetValue(this) as List<string>;
        if (currentValue == null)
        {
            currentValue = new List<string>();
        }

        var positionToInsertAt = index == -1 ? currentValue.Count : index;
        currentValue.Insert(positionToInsertAt, value);
        prop.SetValue(this, currentValue);
    }

    private System.Reflection.PropertyInfo GetProperty(string key)
    {
        var props = typeof(Configuration).GetProperties();
        var prop = props.FirstOrDefault(p => p.Name.ToLower() == key.ToLower());
        if (prop == null)
        {
            throw new Exception($"No property found with name '{key}'");
        }
        return prop;
    }

    private object? GetPropertyDefaultValue(string key)
    {
        var props = typeof(ConfigurationDefaultData).GetProperties();
        var prop = props.FirstOrDefault(p => p.Name.ToLower() == key.ToLower());
        if (prop == null)
        {
            throw new Exception($"No property found with name '{key}'");
        }

        return prop.GetValue(this);
    }

    #endregion

    #region File Stuff
    public override string ToString()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        return JsonSerializer.Serialize(this, options);
    }

    public static string ConfigurationPath { get; set; } =
      Path.Join(Environment.GetEnvironmentVariable("LOCALAPPDATA"), System.AppDomain.CurrentDomain.FriendlyName, "config.json");

    public static void Save(Configuration configuration)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(ConfigurationPath)!);
        var t = configuration.ToString();
        File.WriteAllText(ConfigurationPath, configuration.ToString());
    }

    public static Configuration Load()
    {
        if (File.Exists(ConfigurationPath))
        {
            var json = File.ReadAllText(ConfigurationPath);
            var configuration = JsonSerializer.Deserialize<Configuration>(json);
            if (configuration == null)
            {
                throw new Exception("Failed to deserialize configuration");
            }
            return configuration;
        }
        else
        {
            Save(new Configuration());
            return new Configuration();
        }
    }
    #endregion

    #region Singleton part
    private static Configuration _instance = Load();

    public static Configuration Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion
}