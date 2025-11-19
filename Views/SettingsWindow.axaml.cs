using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using TimerApp.ViewModels;

namespace TimerApp.Views
{
    /// <summary>
    /// Settings window with enhanced customization UI.
    /// Provides color pickers, theme selection, and background customization.
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            
            // Ensure ViewModel is set
            DataContext = new SettingsViewModel();
            
            // Initialize color preset buttons
            InitializeColorPresets();
            
            // Hook up timer color preset button
            var timerColorPresetButton = this.FindControl<Button>("TimerColorPresetButton");
            if (timerColorPresetButton != null)
            {
                timerColorPresetButton.Click += OnTimerColorPresetClick;
            }
        }

        /// <summary>
        /// Creates color preset buttons for quick color selection.
        /// </summary>
        private void InitializeColorPresets()
        {
            var panel = this.FindControl<WrapPanel>("ColorPresetsPanel");
            if (panel == null || DataContext is not SettingsViewModel viewModel)
                return;

            foreach (var colorHex in viewModel.ColorPresets)
            {
                var button = new Button
                {
                    Classes = { "colorSwatch" },
                    Background = new SolidColorBrush(Color.Parse(colorHex))
                };
                ToolTip.SetTip(button, colorHex);

                button.Click += (s, e) =>
                {
                    viewModel.TimerColor = colorHex;
                };

                panel.Children.Add(button);
            }
        }

        /// <summary>
        /// Shows color presets popup for timer color selection.
        /// </summary>
        private void OnTimerColorPresetClick(object? sender, RoutedEventArgs e)
        {
            if (DataContext is not SettingsViewModel viewModel)
                return;

            // Create a simple color selection dialog
            var colorPanel = new StackPanel { Spacing = 10, Margin = new Avalonia.Thickness(20) };
            
            var wrapPanel = new WrapPanel();
            foreach (var colorHex in viewModel.ColorPresets)
            {
                var btn = new Button
                {
                    Width = 60,
                    Height = 60,
                    Margin = new Avalonia.Thickness(5),
                    Background = new SolidColorBrush(Color.Parse(colorHex)),
                    CornerRadius = new Avalonia.CornerRadius(8),
                    BorderThickness = new Avalonia.Thickness(2),
                    BorderBrush = new SolidColorBrush(Color.Parse("#2D3561"))
                };

                btn.Click += (s, ev) =>
                {
                    viewModel.TimerColor = colorHex;
                };

                wrapPanel.Children.Add(btn);
            }

            colorPanel.Children.Add(new TextBlock
            {
                Text = "Select Timer Color",
                FontSize = 16,
                FontWeight = Avalonia.Media.FontWeight.Bold,
                Foreground = new SolidColorBrush(Colors.White)
            });
            colorPanel.Children.Add(wrapPanel);

            var popup = new Window
            {
                Content = colorPanel,
                Width = 400,
                Height = 300,
                Background = new SolidColorBrush(Color.Parse("#1A1F3A")),
                Title = "Color Picker",
                CanResize = false,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            popup.ShowDialog(this);
        }
    }
}
