using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Platform;
using TimerApp.ViewModels;

namespace TimerApp.Views;

/// <summary>
/// Main window of the Timer App.
/// Handles UI events, system tray integration, and window management.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// System tray icon instance for minimize-to-tray functionality.
    /// Null if tray feature is not initialized.
    /// </summary>
    private TrayIcon? trayIcon;
    
    /// <summary>
    /// Initializes the main window and sets up event handlers.
    /// Connects buttons to their click events and initializes system tray.
    /// </summary>
    public MainWindow()
    {
        // Initialize the window and load XAML components
        InitializeComponent();
        
        // ========== HOOK UP BUTTON EVENTS ==========
        
        // Settings button - opens the settings window
        var settingsButton = this.FindControl<Button>("SettingsButton");
        if (settingsButton != null)
        {
            settingsButton.Click += OnSettingsClick;
        }
        
        // Display button - opens secondary display window for projectors/second monitors
        var displayButton = this.FindControl<Button>("DisplayButton");
        if (displayButton != null)
        {
            displayButton.Click += OnDisplayClick;
        }
        
        // Minimize to tray button - hides window to system tray
        var minimizeToTrayButton = this.FindControl<Button>("MinimizeToTrayButton");
        if (minimizeToTrayButton != null)
        {
            minimizeToTrayButton.Click += OnMinimizeToTrayClick;
        }
        
        // Initialize system tray icon and menu
        SetupTrayIcon();
        
        // Hook up input validation
        SetupInputValidation();
    }
    
    private void SetupInputValidation()
    {
        var inputs = new[] { "HoursInput", "MinutesInput", "SecondsInput" };
        foreach (var name in inputs)
        {
            var input = this.FindControl<NumericUpDown>(name);
            if (input != null)
            {
                // When focus is lost, check value
                input.LostFocus += (s, e) =>
                {
                    if (input.Value == null)
                    {
                        input.Value = 0;
                        input.Classes.Remove("error");
                    }
                };
                
                // When text changes (if empty, show red)
                input.ValueChanged += (s, e) =>
                {
                    if (input.Value == null)
                    {
                        input.Classes.Add("error");
                    }
                    else
                    {
                        input.Classes.Remove("error");
                    }
                };
            }
        }
    }
    
    #region System Tray Management
    
    /// <summary>
    /// Creates and configures the system tray icon with context menu.
    /// Tray icon allows app to run in background and provides quick access.
    /// </summary>
    private void SetupTrayIcon()
    {
        // Create the tray icon (initially hidden)
        trayIcon = new TrayIcon
        {
            ToolTipText = "Timer App", // Text shown on hover
            IsVisible = false          // Hide by default, show when minimized
        };
        
        // ========== LOAD TRAY ICON IMAGE ==========
        // Try to load the app icon for the tray
        try
        {
            // avares:// is Avalonia's asset loading URI scheme
            var uri = new Uri("avares://TimerApp/Assets/avalonia-logo.ico");
            trayIcon.Icon = new WindowIcon(AssetLoader.Open(uri));
        }
        catch
        {
            // If icon loading fails, continue without a custom icon
            // The OS will use a default icon
        }
        
        // ========== CREATE TRAY CONTEXT MENU ==========
        var menu = new NativeMenu();
        
        // Menu item: Show Timer (restore window)
        var showMenuItem = new NativeMenuItem("Show Timer");
        showMenuItem.Click += (s, e) => ShowWindow();
        menu.Add(showMenuItem);
        
        // Separator line
        menu.Add(new NativeMenuItemSeparator());
        
        // Menu item: Exit (close application)
        var exitMenuItem = new NativeMenuItem("Exit");
        exitMenuItem.Click += (s, e) => ExitApplication();
        menu.Add(exitMenuItem);
        
        // Attach menu to tray icon
        trayIcon.Menu = menu;
        
        // Double-click tray icon to show window
        trayIcon.Clicked += (s, e) => ShowWindow();
    }
    
    /// <summary>
    /// Hides the window and shows the tray icon.
    /// Timer continues running in background.
    /// </summary>
    private void MinimizeToTray()
    {
        if (trayIcon != null)
        {
            this.Hide();              // Hide the window
            trayIcon.IsVisible = true; // Show tray icon
        }
    }
    
    /// <summary>
    /// Restores the window from tray and hides the tray icon.
    /// Brings window to front and gives it focus.
    /// </summary>
    private void ShowWindow()
    {
        this.Show();     // Show the window
        this.Activate(); // Bring to front and focus
        
        if (trayIcon != null)
        {
            trayIcon.IsVisible = false; // Hide tray icon
        }
    }
    
    /// <summary>
    /// Completely exits the application.
    /// Called from tray menu or window close.
    /// </summary>
    private void ExitApplication()
    {
        // Get the application lifetime manager
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown(); // Properly shut down the application
        }
    }
    
    #endregion
    
    #region Button Event Handlers
    
    /// <summary>
    /// Handles the "Minimize to Tray" button click.
    /// Hides the window to system tray.
    /// </summary>
    private void OnMinimizeToTrayClick(object? sender, RoutedEventArgs e)
    {
        MinimizeToTray();
    }
    
    /// <summary>
    /// Handles the "Settings" button click.
    /// Opens the settings window and synchronizes settings between windows.
    /// </summary>
    private void OnSettingsClick(object? sender, RoutedEventArgs e)
    {
        // Create a new settings window
        var settingsWindow = new SettingsWindow();
        
        // ========== SYNCHRONIZE SETTINGS ==========
        // Share settings between main window and settings window
        if (DataContext is MainWindowViewModel mainViewModel && 
            settingsWindow.DataContext is SettingsViewModel settingsViewModel)
        {
            // Copy current settings to settings window
            settingsViewModel.PlayAlarmSound = mainViewModel.PlayAlarmSound;
            settingsViewModel.SelectedAlarmType = mainViewModel.SelectedAlarmType;
            
            // Listen for changes in settings window and update main window
            settingsViewModel.PropertyChanged += (s, args) =>
            {
                // Alarm sound toggle changed
                if (args.PropertyName == nameof(SettingsViewModel.PlayAlarmSound))
                {
                    mainViewModel.PlayAlarmSound = settingsViewModel.PlayAlarmSound;
                }
                // Alarm type changed
                else if (args.PropertyName == nameof(SettingsViewModel.SelectedAlarmType))
                {
                    mainViewModel.SelectedAlarmType = settingsViewModel.SelectedAlarmType;
                }
                // Always on top changed
                else if (args.PropertyName == nameof(SettingsViewModel.AlwaysOnTop))
                {
                    this.Topmost = settingsViewModel.AlwaysOnTop;
                }
            };
        }
        
        // Show the settings window
        settingsWindow.Show();
    }
    
    /// <summary>
    /// Handles the "Open Display" button click.
    /// Opens a secondary display window for presentations or second monitors.
    /// Synchronizes timer display in real-time.
    /// </summary>
    private void OnDisplayClick(object? sender, RoutedEventArgs e)
    {
        // Create a new display window
        var displayWindow = new TimerDisplayWindow();
        
        // ========== SYNCHRONIZE TIMER DISPLAY ==========
        // Connect the main timer to the display window
        if (DataContext is MainWindowViewModel mainViewModel)
        {
            if (displayWindow.DataContext is TimerDisplayViewModel displayViewModel)
            {
                // Subscribe to timer updates from main window
                mainViewModel.PropertyChanged += (s, args) =>
                {
                    // Time display changed (e.g., "00:25:30" -> "00:25:29")
                    if (args.PropertyName == nameof(MainWindowViewModel.DisplayTime))
                    {
                        displayViewModel.DisplayTime = mainViewModel.DisplayTime;
                    }
                    // Negative flag changed (timer went into overtime)
                    else if (args.PropertyName == nameof(MainWindowViewModel.IsNegative))
                    {
                        displayViewModel.IsNegative = mainViewModel.IsNegative;
                    }
                };
                
                // Set initial values before showing window
                displayViewModel.DisplayTime = mainViewModel.DisplayTime;
                displayViewModel.IsNegative = mainViewModel.IsNegative;
            }
        }
        
        // Show the display window (user can move to second monitor)
        displayWindow.Show();
    }
    
    #endregion
    
    #region Window Lifecycle
    
    /// <summary>
    /// Called when the window is about to close.
    /// Cleans up the tray icon before exiting.
    /// </summary>
    /// <param name="e">Close event arguments</param>
    protected override void OnClosing(WindowClosingEventArgs e)
    {
        // Call base implementation
        base.OnClosing(e);
        
        // Clean up tray icon if it exists
        if (trayIcon != null)
        {
            trayIcon.IsVisible = false; // Hide it
            trayIcon.Dispose();         // Release resources
        }
    }
    
    #endregion
}