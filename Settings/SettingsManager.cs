using System.IO;
using System.Text.Json;

namespace Winhanced_Shell.Settings
{
    public static class SettingsManager
    {
        public static AppSettings Settings { get; private set; }

        private static string settingsPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "settings.json"
        );

        public static void Load()
        {
            if (File.Exists(settingsPath))
            {
                var json = File.ReadAllText(settingsPath);
                Settings = JsonSerializer.Deserialize<AppSettings>(json);
            }
            else
            {
                Settings = new AppSettings(); // valores por defecto
                Save();
            }
        }

        public static void Save()
        {
            var json = JsonSerializer.Serialize(Settings, new JsonSerializerOptions { WriteIndented = true });
            Directory.CreateDirectory(Path.GetDirectoryName(settingsPath));
            File.WriteAllText(settingsPath, json);
        }

        public static Dictionary<string, Action<object>> SettingChangedCallbacks = new();

        public static void RegisterCallback(string key, Action<object> callback)
        {
            SettingChangedCallbacks[key] = callback;
        }

        internal static void NotifySettingChanged(string key, object value)
        {
            if (SettingChangedCallbacks.TryGetValue(key, out var callback))
                callback?.Invoke(value);
        }
    }

}
