using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Platform;
using TimerApp.ViewModels;

namespace TimerApp.Views;

/// <summary>
/// Full-screen timer display window for presentations and external monitors.
/// Automatically detects and uses external screens when available.
/// Provides OBS-style presentation mode with large, visible timer.
/// </summary>
public partial class TimerDisplayWindow : Window
{
    /// <summary>
    /// Initializes the timer display window.
    /// Sets up fullscreen mode and positions on external screen if available.
    /// </summary>
    public TimerDisplayWindow()
    {
        InitializeComponent();
        DataContext = new TimerDisplayViewModel();
        
        // Configure window for fullscreen presentation mode
        ConfigureFullscreenMode();
    }
    
    /// <summary>
    /// Configures the window for fullscreen presentation display.
    /// Detects external screens and positions window accordingly.
    /// </summary>
    private void ConfigureFullscreenMode()
    {
        // Get all available screens
        var screens = Screens.All;
        
        Screen? targetScreen = null;
        
        if (screens.Count > 1)
        {
            // Multiple screens detected - use external screen
            // Find the screen that's NOT the primary screen
            targetScreen = screens.FirstOrDefault(s => !s.IsPrimary) ?? screens[0];
        }
        else
        {
            // Single screen - use primary screen
            targetScreen = screens[0];
        }
        
        if (targetScreen != null)
        {
            // Position window on the target screen
            var bounds = targetScreen.Bounds;
            
            // Set window position to the screen's top-left corner
            Position = new Avalonia.PixelPoint(bounds.X, bounds.Y);
            
            // Set window size to fill the screen
            Width = bounds.Width;
            Height = bounds.Height;
        }
        
        // Enable fullscreen mode
        WindowState = WindowState.FullScreen;
        
        // Additional fullscreen settings
        CanResize = false;           // Prevent resizing
        SystemDecorations = SystemDecorations.None; // Remove title bar and borders
        
        // Set always on top for presentation mode
        Topmost = true;
    }
    
    /// <summary>
    /// Override to allow Escape key to exit fullscreen.
    /// </summary>
    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        
        // Refresh settings to ensure colors and backgrounds are up to date
        if (DataContext is TimerDisplayViewModel viewModel)
        {
            viewModel.RefreshSettings();
        }
        
        // Add keyboard handler for Escape key
        KeyDown += (s, ev) =>
        {
            if (ev.Key == Avalonia.Input.Key.Escape)
            {
                // Exit fullscreen and close window
                Close();
            }
        };
    }
}
