using System;
using System.Collections.ObjectModel;
using System.Timers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TimerApp.Models;
using TimerApp.Services;

namespace TimerApp.ViewModels;

/// <summary>
/// ViewModel for the main timer window.
/// Handles all timer logic, presets, and alarm functionality.
/// Uses MVVM pattern with CommunityToolkit.Mvvm for observable properties and commands.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    #region Observable Properties
    
    /// <summary>
    /// The formatted time string displayed to the user (e.g., "00:25:30" or "-00:05:12").
    /// Updates every 100ms when timer is running.
    /// </summary>
    [ObservableProperty]
    private string displayTime = "00:00:00";
    
    /// <summary>
    /// Indicates whether the timer is currently running.
    /// Used to show/hide input controls and change button text.
    /// </summary>
    [ObservableProperty]
    private bool isRunning = false;

    /// <summary>
    /// Indicates if the timer session is active (Running OR Paused).
    /// Controls visibility of input fields.
    /// </summary>
    [ObservableProperty]
    private bool isTimerActive;


    
    /// <summary>
    /// Indicates whether the timer has gone into negative/overtime.
    /// When true, display color changes from cyan to red.
    /// </summary>
    [ObservableProperty]
    private bool isNegative = false;
    
    /// <summary>
    /// Text displayed on the start/pause button.
    /// Changes between "Start", "Pause", and "Resume" based on timer state.
    /// </summary>
    [ObservableProperty]
    private string startButtonText = "Start";
    
    /// <summary>
    /// Hours component of the timer duration (0-23).
    /// Bound to NumericUpDown control in UI.
    /// </summary>
    [ObservableProperty]
    private int hours = 0;
    
    /// <summary>
    /// Minutes component of the timer duration (0-59).
    /// Bound to NumericUpDown control in UI.
    /// </summary>
    [ObservableProperty]
    private int minutes = 1;
    
    /// <summary>
    /// Seconds component of the timer duration (0-59).
    /// Bound to NumericUpDown control in UI.
    /// </summary>
    [ObservableProperty]
    private int seconds = 0;
    
    /// <summary>
    /// Currently selected preset from the presets list.
    /// When changed, applies the preset's time values.
    /// </summary>
    [ObservableProperty]
    private TimerPreset? selectedPreset;
    
    /// <summary>
    /// Whether to play an alarm sound when timer reaches zero.
    /// Can be toggled in settings.
    /// </summary>
    [ObservableProperty]
    private bool playAlarmSound = true;
    
    /// <summary>
    /// Type of alarm sound to play (Default, Beep, Alert, Chime).
    /// Can be changed in settings.
    /// </summary>
    [ObservableProperty]
    private AlarmType selectedAlarmType = AlarmType.Default;
    
    /// <summary>
    /// Collection of timer presets (both built-in and custom).
    /// Displayed as buttons in the UI for quick timer setup.
    /// </summary>
    public ObservableCollection<TimerPreset> Presets { get; } = new();
    
    /// <summary>
    /// All available alarm types for the settings dropdown.
    /// </summary>
    public Array AlarmTypes => Enum.GetValues(typeof(AlarmType));
    
    #endregion
    
    #region Private Fields
    
    /// <summary>
    /// The internal timer that fires every 100ms to update the countdown.
    /// Null when timer is stopped.
    /// </summary>
    private Timer? timer;
    
    /// <summary>
    /// Current remaining time on the timer.
    /// Can be negative (overtime).
    /// </summary>
    private TimeSpan remainingTime;
    
    /// <summary>
    /// Initial time set when timer starts.
    /// Stored for reference/reset purposes.
    /// </summary>
    private TimeSpan initialTime;
    
    /// <summary>
    /// Service for playing cross-platform alarm sounds.
    /// </summary>
    private readonly AlarmSoundService alarmService = new();
    
    /// <summary>
    /// Flag to ensure alarm only plays once per timer cycle.
    /// Reset when timer is reset or restarted.
    /// </summary>
    private bool alarmPlayed = false;
    
    #endregion

    #region Constructor
    
    /// <summary>
    /// Initializes the MainWindowViewModel.
    /// Loads default presets and sets initial display time.
    /// </summary>
    public MainWindowViewModel()
    {
        LoadDefaultPresets();
        UpdateDisplayTime();
    }
    
    #endregion
    
    #region Preset Management
    
    /// <summary>
    /// Loads the built-in timer presets (Pomodoro, breaks, meetings, etc.).
    /// Called once during initialization.
    /// </summary>
    private void LoadDefaultPresets()
    {
        // 5-minute quick break
        Presets.Add(new TimerPreset("Quick Break", 0, 5, 0, "☕"));
        
        // 25-minute Pomodoro work session
        Presets.Add(new TimerPreset("Pomodoro", 0, 25, 0, "🍅"));
        
        // 15-minute short break
        Presets.Add(new TimerPreset("Short Break", 0, 15, 0, "⏸️"));
        
        // 30-minute long break
        Presets.Add(new TimerPreset("Long Break", 0, 30, 0, "🛋️"));
        
        // 1-hour presentation timer
        Presets.Add(new TimerPreset("Presentation", 1, 0, 0, "🎤"));
        
        // 45-minute workout session
        Presets.Add(new TimerPreset("Workout", 0, 45, 0, "💪"));
        
        // 1.5-hour meeting timer
        Presets.Add(new TimerPreset("Meeting", 1, 30, 0, "👥"));
    }
    
    #endregion
    
    #region Commands
    
    /// <summary>
    /// Command to start or stop the timer.
    /// Toggles between Start/Pause/Resume states.
    /// Bound to the main action button and Space key.
    /// </summary>
    [RelayCommand]
    private void StartStop()
    {
        if (IsRunning)
        {
            // Timer is running, so pause it
            StopTimer();
        }
        else
        {
            // Timer is stopped, so start it
            StartTimer();
        }
    }
    
    /// <summary>
    /// Command to reset the timer to zero.
    /// Stops the timer if running and clears all state.
    /// Bound to the Reset button and R key.
    /// </summary>
    [RelayCommand]
    private void Reset()
    {
        // Stop the timer if it's running
        StopTimer();
        
        // Reset session active state
        IsTimerActive = false;
        
        // Reset all state flags
        IsNegative = false;
        alarmPlayed = false;
        
        // Reset time to the user's input values (not zero)
        remainingTime = new TimeSpan(Hours, Minutes, Seconds);
        
        // Reset button text
        StartButtonText = "Start";
        
        // Update the display
        UpdateDisplayTime();
    }
    
    /// <summary>
    /// Command to apply a preset's time values to the timer.
    /// Sets Hours, Minutes, and Seconds from the preset.
    /// </summary>
    /// <param name="preset">The preset to apply (from button click)</param>
    [RelayCommand]
    private void ApplyPreset(TimerPreset? preset)
    {
        if (preset == null) return;
        
        // Stop timer if currently running
        if (IsRunning)
        {
            StopTimer();
        }
        
        // Reset session
        IsTimerActive = false;
        
        // Apply preset time values
        Hours = preset.Hours;
        Minutes = preset.Minutes;
        Seconds = preset.Seconds;
        
        // Reset flags
        IsNegative = false;
        alarmPlayed = false;
        
        // Update display with new time
        UpdateInitialTime();
    }
    
    /// <summary>
    /// Command to save the current time as a custom preset.
    /// Creates a new preset and adds it to the collection.
    /// Bound to Ctrl+S keyboard shortcut.
    /// </summary>
    [RelayCommand]
    private void AddCustomPreset()
    {
        // Create a new preset with current time values
        var preset = new TimerPreset(
            $"Custom {Presets.Count + 1}", // Name like "Custom 1", "Custom 2", etc.
            Hours,
            Minutes,
            Seconds,
            "⭐" // Star icon for custom presets
        );
        
        // Add to the presets collection (automatically updates UI)
        Presets.Add(preset);
    }
    
    /// <summary>
    /// Command to delete a preset from the collection.
    /// Only user-created presets should be deletable in practice.
    /// </summary>
    /// <param name="preset">The preset to delete</param>
    [RelayCommand]
    private void DeletePreset(TimerPreset? preset)
    {
        if (preset != null && Presets.Contains(preset))
        {
            Presets.Remove(preset);
        }
    }
    
    #endregion
    
    #region Timer Control Methods
    
    /// <summary>
    /// Starts the countdown timer.
    /// Creates a new Timer instance that fires every 100ms.
    /// </summary>
    private void StartTimer()
    {
        // If starting fresh (not resuming), initialize the time
        // We check IsTimerActive because it remains true when paused
        if (!IsTimerActive)
        {
            initialTime = new TimeSpan(Hours, Minutes, Seconds);
            remainingTime = initialTime;
        }
        
        // Create timer that fires every 100ms for smooth updates
        timer = new Timer(100);
        timer.Elapsed += OnTimerElapsed;
        timer.Start();
        
        // Update UI state
        IsRunning = true;
        IsTimerActive = true;
        StartButtonText = "Pause";
    }
    
    /// <summary>
    /// Stops/pauses the timer but preserves the remaining time.
    /// Timer can be resumed later.
    /// </summary>
    private void StopTimer()
    {
        // Dispose of timer if it exists
        if (timer != null)
        {
            timer.Stop();
            timer.Dispose();
            timer = null;
        }
        
        // Update UI state
        IsRunning = false;
        // IsTimerActive remains true because we are just paused
        StartButtonText = "Resume";
    }
    
    #endregion
    
    #region Timer Event Handlers
    /// Updates remaining time and triggers alarm when reaching zero.
    /// </summary>
    /// <param name="sender">The timer instance</param>
    /// <param name="e">Event arguments with elapsed time</param>
    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        // Store previous time to detect zero crossing
        var previousTime = remainingTime;
        
        // Subtract 100ms from remaining time
        remainingTime = remainingTime.Subtract(TimeSpan.FromMilliseconds(100));
        
        // Check if we just crossed from positive to zero/negative
        if (previousTime.TotalMilliseconds > 0 && 
            remainingTime.TotalMilliseconds <= 0 && 
            !alarmPlayed)
        {
            // Play alarm if enabled in settings
            if (SettingsService.Instance.Settings.PlayAlarmSound)
            {
                // Parse the alarm type from settings string
                if (Enum.TryParse<AlarmType>(SettingsService.Instance.Settings.AlarmType, out var alarmType))
                {
                    alarmService.PlayAlarm(alarmType);
                }
                else
                {
                    alarmService.PlayAlarm(AlarmType.Default);
                }
                
                alarmPlayed = true; // Ensure alarm only plays once
            }
        }
        
        // Update negative flag when time goes below zero
        if (remainingTime.TotalMilliseconds < 0 && !IsNegative)
        {
            IsNegative = true;
        }
        
        // Update the display
        UpdateDisplayTime();
    }
    
    #endregion
    
    #region Display Update Methods
    
    /// <summary>
    /// Updates the DisplayTime property with formatted time string.
    /// Handles both positive and negative time display.
    /// Format: "MM:SS" when hours = 0, "HH:MM:SS" when hours > 0
    /// </summary>
    private void UpdateDisplayTime()
    {
        var time = remainingTime;
        var isNeg = time.TotalMilliseconds < 0;
        
        // Make time positive for formatting if it's negative
        if (isNeg)
        {
            time = time.Negate();
        }
        
        // Extract time components
        var totalHours = (int)time.TotalHours;
        var mins = time.Minutes;
        var secs = time.Seconds;
        
        // Format conditionally based on whether hours exist
        // If no hours: show "MM:SS" (e.g., "05:30")
        // If hours exist: show "HH:MM:SS" (e.g., "1:25:30")
        if (totalHours > 0)
        {
            DisplayTime = $"{(isNeg ? "-" : "")}{totalHours:D2}:{mins:D2}:{secs:D2}";
        }
        else
        {
            DisplayTime = $"{(isNeg ? "-" : "")}{mins:D2}:{secs:D2}";
        }
    }
    
    #endregion
    
    #region Property Change Handlers
    
    /// <summary>
    /// Called when Hours property changes.
    /// Updates the display if timer is not running.
    /// Auto-generated partial method by CommunityToolkit.Mvvm.
    /// </summary>
    partial void OnHoursChanged(int value)
    {
        if (!IsRunning)
        {
            UpdateInitialTime();
        }
    }
    
    /// <summary>
    /// Called when Minutes property changes.
    /// Updates the display if timer is not running.
    /// Auto-generated partial method by CommunityToolkit.Mvvm.
    /// </summary>
    partial void OnMinutesChanged(int value)
    {
        if (!IsRunning)
        {
            UpdateInitialTime();
        }
    }
    
    /// <summary>
    /// Called when Seconds property changes.
    /// Updates the display if timer is not running.
    /// Auto-generated partial method by CommunityToolkit.Mvvm.
    /// </summary>
    partial void OnSecondsChanged(int value)
    {
        if (!IsRunning)
        {
            UpdateInitialTime();
        }
    }
    
    /// <summary>
    /// Updates the initial time and display when input values change.
    /// Only called when timer is not running.
    /// </summary>
    private void UpdateInitialTime()
    {
        // Create TimeSpan from current input values
        remainingTime = new TimeSpan(Hours, Minutes, Seconds);
        
        // Reset alarm flag since we're setting a new time
        alarmPlayed = false;
        
        // Update the display
        UpdateDisplayTime();
    }
    
    /// <summary>
    /// Called when SelectedPreset property changes.
    /// Automatically applies the preset if one is selected.
    /// Auto-generated partial method by CommunityToolkit.Mvvm.
    /// </summary>
    partial void OnSelectedPresetChanged(TimerPreset? value)
    {
        if (value != null)
        {
            ApplyPreset(value);
        }
    }
    
    #endregion
}
