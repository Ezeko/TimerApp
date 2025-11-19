using System;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TimerApp.Services;

namespace TimerApp.ViewModels
{
    /// <summary>
    /// ViewModel for the settings window.
    /// Manages all application settings with immediate persistence.
    /// </summary>
    public partial class SettingsViewModel : ViewModelBase
    {
        private readonly SettingsService _settingsService;

        #region Observable Properties

        [ObservableProperty]
        private bool _alwaysOnTop;

        [ObservableProperty]
        private string _selectedTheme;

        [ObservableProperty]
        private bool _playAlarmSound;

        [ObservableProperty]
        private AlarmType _selectedAlarmType;

        [ObservableProperty]
        private int _fontSize;

        [ObservableProperty]
        private bool _minimizeToTray;

        [ObservableProperty]
        private bool _showInTaskbar;

        [ObservableProperty]
        private string _timerColor;

        [ObservableProperty]
        private string _overtimeColor;

        [ObservableProperty]
        private string _backgroundColor;

        [ObservableProperty]
        private string _backgroundImagePath;

        [ObservableProperty]
        private bool _useBackgroundImage;

        [ObservableProperty]
        private double _backgroundImageOpacity;

        #endregion

        public string[] AvailableThemes { get; } = new[] { "Light", "Dark" };
        public Array AlarmTypes => Enum.GetValues(typeof(AlarmType));

        /// <summary>
        /// Predefined professional color palette for quick selection.
        /// </summary>
        public string[] ColorPresets { get; } = new[]
        {
            "#00D9FF", // Cyan (default timer)
            "#FF4757", // Red (default overtime)
            "#00E5A0", // Mint Green
            "#FFD700", // Gold
            "#FF6B9D", // Pink
            "#9B59B6", // Purple
            "#3498DB", // Blue
            "#E67E22", // Orange
            "#1ABC9C", // Turquoise
            "#FFFFFF"  // White
        };

        public SettingsViewModel()
        {
            _settingsService = SettingsService.Instance;
            LoadSettings();
        }

        /// <summary>
        /// Loads current settings from persistence layer.
        /// </summary>
        private void LoadSettings()
        {
            var settings = _settingsService.Settings;
            
            AlwaysOnTop = settings.AlwaysOnTop;
            SelectedTheme = settings.Theme;
            PlayAlarmSound = settings.PlayAlarmSound;
            SelectedAlarmType = Enum.Parse<AlarmType>(settings.AlarmType);
            FontSize = settings.FontSize;
            MinimizeToTray = settings.MinimizeToTray;
            ShowInTaskbar = settings.ShowInTaskbar;
            TimerColor = settings.TimerColor;
            OvertimeColor = settings.OvertimeColor;
            BackgroundColor = settings.BackgroundColor;
            BackgroundImagePath = settings.BackgroundImagePath;
            UseBackgroundImage = settings.UseBackgroundImage;
            BackgroundImageOpacity = settings.BackgroundImageOpacity;
        }

        /// <summary>
        /// Persists all settings to disk.
        /// </summary>
        [RelayCommand]
        private void SaveSettings()
        {
            _settingsService.UpdateSettings(settings =>
            {
                settings.AlwaysOnTop = AlwaysOnTop;
                settings.Theme = SelectedTheme;
                settings.PlayAlarmSound = PlayAlarmSound;
                settings.AlarmType = SelectedAlarmType.ToString();
                settings.FontSize = FontSize;
                settings.MinimizeToTray = MinimizeToTray;
                settings.ShowInTaskbar = ShowInTaskbar;
                settings.TimerColor = TimerColor;
                settings.OvertimeColor = OvertimeColor;
                settings.BackgroundColor = BackgroundColor;
                settings.BackgroundImagePath = BackgroundImagePath;
                settings.UseBackgroundImage = UseBackgroundImage;
                settings.BackgroundImageOpacity = BackgroundImageOpacity;
            });
        }

        /// <summary>
        /// Tests the currently selected alarm sound.
        /// </summary>
        [RelayCommand]
        private void TestAlarm()
        {
            var alarmService = new AlarmSoundService();
            alarmService.PlayAlarm(SelectedAlarmType);
        }

        /// <summary>
        /// Opens file picker for background image selection.
        /// </summary>
        [RelayCommand]
        private async void BrowseBackgroundImage()
        {
            try
            {
                var dialog = new Avalonia.Controls.OpenFileDialog
                {
                    Title = "Select Background Image",
                    AllowMultiple = false
                };

                dialog.Filters.Add(new Avalonia.Controls.FileDialogFilter
                {
                    Name = "Image Files",
                    Extensions = { "png", "jpg", "jpeg", "bmp", "gif" }
                });

                if (Avalonia.Application.Current?.ApplicationLifetime 
                    is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
                {
                    var result = await dialog.ShowAsync(desktop.MainWindow);
                    if (result != null && result.Length > 0)
                    {
                        BackgroundImagePath = result[0];
                        UseBackgroundImage = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error selecting background image: {ex.Message}");
            }
        }

        /// <summary>
        /// Resets all settings to default values.
        /// </summary>
        [RelayCommand]
        private void ResetToDefaults()
        {
            _settingsService.Settings = new AppSettings();
            LoadSettings();
        }

        #region Property Change Handlers
        
        // Auto-save on property changes for immediate persistence
        partial void OnAlwaysOnTopChanged(bool value) => SaveSettings();
        partial void OnSelectedThemeChanged(string value) => SaveSettings();
        partial void OnPlayAlarmSoundChanged(bool value) => SaveSettings();
        partial void OnSelectedAlarmTypeChanged(AlarmType value) => SaveSettings();
        partial void OnFontSizeChanged(int value) => SaveSettings();
        partial void OnMinimizeToTrayChanged(bool value) => SaveSettings();
        partial void OnShowInTaskbarChanged(bool value) => SaveSettings();
        partial void OnTimerColorChanged(string value) => SaveSettings();
        partial void OnOvertimeColorChanged(string value) => SaveSettings();
        partial void OnBackgroundColorChanged(string value) => SaveSettings();
        partial void OnBackgroundImagePathChanged(string value) => SaveSettings();
        partial void OnUseBackgroundImageChanged(bool value) => SaveSettings();
        partial void OnBackgroundImageOpacityChanged(double value) => SaveSettings();

        #endregion
    }
}
