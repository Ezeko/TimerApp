using System;

namespace TimerApp.Models;

/// <summary>
/// Represents a timer preset with a name, duration, and icon.
/// Presets allow users to quickly set common timer durations without manual input.
/// Examples: Pomodoro (25 min), Quick Break (5 min), Meeting (1.5 hours)
/// </summary>
public class TimerPreset
{
    /// <summary>
    /// Display name of the preset (e.g., "Pomodoro", "Quick Break").
    /// Shown on the preset button in the UI.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Duration of the timer for this preset.
    /// Stored as TimeSpan for easy manipulation.
    /// </summary>
    public TimeSpan Duration { get; set; }
    
    /// <summary>
    /// Emoji icon representing the preset (e.g., "🍅" for Pomodoro, "☕" for break).
    /// Displayed on the preset button for visual identification.
    /// </summary>
    public string Icon { get; set; } = "⏱️";
    
    /// <summary>
    /// Hours component of the duration (0-23).
    /// Extracted from Duration.TotalHours.
    /// </summary>
    public int Hours => (int)Duration.TotalHours;
    
    /// <summary>
    /// Minutes component of the duration (0-59).
    /// Extracted from Duration.Minutes.
    /// </summary>
    public int Minutes => Duration.Minutes;
    
    /// <summary>
    /// Seconds component of the duration (0-59).
    /// Extracted from Duration.Seconds.
    /// </summary>
    public int Seconds => Duration.Seconds;
    
    /// <summary>
    /// Default constructor for serialization/deserialization.
    /// Creates an empty preset.
    /// </summary>
    public TimerPreset()
    {
    }
    
    /// <summary>
    /// Creates a new timer preset with specified values.
    /// </summary>
    /// <param name="name">Display name of the preset</param>
    /// <param name="hours">Hours component (0-23)</param>
    /// <param name="minutes">Minutes component (0-59)</param>
    /// <param name="seconds">Seconds component (0-59)</param>
    /// <param name="icon">Emoji icon for visual identification (default: ⏱️)</param>
    public TimerPreset(string name, int hours, int minutes, int seconds, string icon = "⏱️")
    {
        Name = name;
        Duration = new TimeSpan(hours, minutes, seconds);
        Icon = icon;
    }
    
    /// <summary>
    /// Returns a string representation of the preset.
    /// Format: "Icon Name (HH:MM:SS)"
    /// Example: "🍅 Pomodoro (00:25:00)"
    /// </summary>
    public override string ToString()
    {
        return $"{Icon} {Name} ({Hours:D2}:{Minutes:D2}:{Seconds:D2})";
    }
}
