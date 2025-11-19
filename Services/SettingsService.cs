using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Avalonia.Media;

namespace TimerApp.Services
{
    /// <summary>
    /// Manages application settings with automatic persistence to disk.
    /// Implements singleton pattern for global access.
    /// </summary>
    public class SettingsService
    {
        private static readonly Lazy<SettingsService> _instance = new(() => new SettingsService());
        private readonly string _settingsPath;
        private AppSettings _settings;

        public static SettingsService Instance => _instance.Value;

        private SettingsService()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appFolder = Path.Combine(appDataPath, "TimerApp");
            Directory.CreateDirectory(appFolder);
            _settingsPath = Path.Combine(appFolder, "settings.json");
            
            _settings = LoadSettings();
        }

        public AppSettings Settings
        {
            get => _settings;
            set
            {
                _settings = value;
                SaveSettings();
            }
        }

        public void UpdateSettings(Action<AppSettings> updateAction)
        {
            updateAction(_settings);
            SaveSettings();
        }

        private AppSettings LoadSettings()
        {
            try
            {
                if (File.Exists(_settingsPath))
                {
                    var json = File.ReadAllText(_settingsPath);
                    return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load settings: {ex.Message}");
            }

            return new AppSettings();
        }

        private void SaveSettings()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.Never
                };
                
                var json = JsonSerializer.Serialize(_settings, options);
                File.WriteAllText(_settingsPath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save settings: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Application settings data model.
    /// All settings are persisted automatically.
    /// </summary>
    public class AppSettings
    {
        // Display Settings
        public string TimerColor { get; set; } = "#00D9FF"; // Cyan
        public string OvertimeColor { get; set; } = "#FF4757"; // Red
        public string BackgroundColor { get; set; } = "#0A0E27"; // Dark Navy
        public string BackgroundImagePath { get; set; } = string.Empty;
        public int FontSize { get; set; } = 72;
        public string Theme { get; set; } = "Dark";

        // Window Settings
        public bool AlwaysOnTop { get; set; } = false;
        public bool ShowInTaskbar { get; set; } = true;
        public bool MinimizeToTray { get; set; } = true;

        // Sound Settings
        public bool PlayAlarmSound { get; set; } = true;
        public string AlarmType { get; set; } = "Default";

        // Display Window Settings
        public bool UseBackgroundImage { get; set; } = false;
        public double BackgroundImageOpacity { get; set; } = 0.3;
        public string BackgroundImageStretch { get; set; } = "UniformToFill";
    }
}
