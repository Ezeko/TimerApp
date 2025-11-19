using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using TimerApp.Services;

namespace TimerApp.ViewModels
{
    /// <summary>
    /// ViewModel for the fullscreen timer display window.
    /// Subscribes to settings changes for real-time customization.
    /// </summary>
    public partial class TimerDisplayViewModel : ViewModelBase
    {
        private readonly SettingsService _settingsService;

        [ObservableProperty]
        private string _displayTime = "00:00";

        [ObservableProperty]
        private bool _isNegative = false;

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

        public TimerDisplayViewModel()
        {
            _settingsService = SettingsService.Instance;
            LoadDisplaySettings();
        }

        /// <summary>
        /// Loads display customization settings from persistent storage.
        /// </summary>
        private void LoadDisplaySettings()
        {
            var settings = _settingsService.Settings;
            TimerColor = settings.TimerColor;
            OvertimeColor = settings.OvertimeColor;
            BackgroundColor = settings.BackgroundColor;
            BackgroundImagePath = settings.BackgroundImagePath;
            UseBackgroundImage = settings.UseBackgroundImage;
            BackgroundImageOpacity = settings.BackgroundImageOpacity;
        }

        /// <summary>
        /// Gets the brush to use for the timer display based on current state.
        /// Returns OvertimeColor if negative, otherwise TimerColor.
        /// </summary>
        public IBrush CurrentDisplayBrush
        {
            get
            {
                try
                {
                    var colorStr = IsNegative ? OvertimeColor : TimerColor;
                    if (string.IsNullOrEmpty(colorStr)) colorStr = IsNegative ? "#FF4757" : "#00D9FF";
                    return Brush.Parse(colorStr);
                }
                catch
                {
                    return Brushes.White;
                }
            }
        }

        /// <summary>
        /// Refreshes settings from storage (call when settings window closes).
        /// </summary>
        public void RefreshSettings()
        {
            LoadDisplaySettings();
        }

        /// <summary>
        /// Gets the brush to use for the background.
        /// </summary>
        public IBrush BackgroundBrush
        {
            get
            {
                try
                {
                    return string.IsNullOrEmpty(BackgroundColor) 
                        ? Brush.Parse("#0F172A") // Default Dark Navy
                        : Brush.Parse(BackgroundColor);
                }
                catch
                {
                    return Brush.Parse("#0F172A");
                }
            }
        }

        partial void OnIsNegativeChanged(bool value) => OnPropertyChanged(nameof(CurrentDisplayBrush));
        partial void OnTimerColorChanged(string value) => OnPropertyChanged(nameof(CurrentDisplayBrush));
        partial void OnOvertimeColorChanged(string value) => OnPropertyChanged(nameof(CurrentDisplayBrush));
        partial void OnBackgroundColorChanged(string value) => OnPropertyChanged(nameof(BackgroundBrush));
    }
}
