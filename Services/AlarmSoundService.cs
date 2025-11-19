using System;
using System.Media;
using System.Runtime.InteropServices;

namespace TimerApp.Services;

/// <summary>
/// Cross-platform service for playing alarm sounds when timer reaches zero.
/// Automatically detects the operating system and uses platform-specific sound APIs.
/// Provides multiple alarm types with different sound patterns.
/// </summary>
public class AlarmSoundService
{
    // Platform detection - determined once at startup
    private static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    private static readonly bool IsMacOS = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    private static readonly bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    
    /// <summary>
    /// Plays an alarm sound appropriate for the current platform.
    /// Automatically selects Windows, macOS, or Linux implementation.
    /// Falls back to system beep if platform-specific method fails.
    /// </summary>
    /// <param name="alarmType">Type of alarm sound to play (Default, Beep, Alert, Chime)</param>
    public void PlayAlarm(AlarmType alarmType = AlarmType.Default)
    {
        try
        {
            // Route to platform-specific implementation
            if (IsWindows)
            {
                PlayWindowsAlarm(alarmType);
            }
            else if (IsMacOS)
            {
                PlayMacOSAlarm(alarmType);
            }
            else if (IsLinux)
            {
                PlayLinuxAlarm(alarmType);
            }
        }
        catch (Exception ex)
        {
            // Fallback: Use system beep if all else fails
            try
            {
                Console.Beep(); // Basic beep sound
            }
            catch
            {
                // Even beep failed, silently continue
            }
            Console.WriteLine($"Error playing alarm: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Plays alarm sounds on Windows using Console.Beep with different frequencies.
    /// Creates different patterns by varying frequency and duration.
    /// </summary>
    /// <param name="alarmType">Type of alarm pattern to play</param>
    private void PlayWindowsAlarm(AlarmType alarmType)
    {
        // Windows Console.Beep(frequency, duration)
        // Frequency in Hz, Duration in milliseconds
        switch (alarmType)
        {
            case AlarmType.Beep:
                // Single medium beep
                Console.Beep(800, 500); // 800 Hz for 500ms
                break;
                
            case AlarmType.Alert:
                // Double high beep
                Console.Beep(1000, 200); // First beep
                System.Threading.Thread.Sleep(100); // Pause
                Console.Beep(1000, 200); // Second beep
                break;
                
            case AlarmType.Chime:
                // Ascending three-tone chime
                Console.Beep(600, 150);  // Low tone
                System.Threading.Thread.Sleep(50);
                Console.Beep(800, 150);  // Medium tone
                System.Threading.Thread.Sleep(50);
                Console.Beep(1000, 150); // High tone
                break;
                
            default:
                // Default sound - single high beep
                Console.Beep(1000, 500);
                break;
        }
    }
    
    /// <summary>
    /// Plays alarm sounds on macOS using the 'afplay' command-line tool.
    /// Uses built-in macOS system sounds from /System/Library/Sounds/.
    /// </summary>
    /// <param name="alarmType">Type of alarm sound to play</param>
    private void PlayMacOSAlarm(AlarmType alarmType)
    {
        try
        {
            // Map alarm type to macOS system sound file
            string soundPath = alarmType switch
            {
                AlarmType.Beep => "/System/Library/Sounds/Ping.aiff",    // Short ping sound
                AlarmType.Alert => "/System/Library/Sounds/Glass.aiff",  // Glass breaking sound
                AlarmType.Chime => "/System/Library/Sounds/Sosumi.aiff", // Classic Mac chime
                _ => "/System/Library/Sounds/Glass.aiff"                 // Default: glass
            };
            
            // Create process to run afplay command
            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "afplay",          // macOS audio player
                    Arguments = soundPath,         // Path to sound file
                    CreateNoWindow = true,         // Don't show terminal window
                    UseShellExecute = false        // Direct process creation
                }
            };
            
            // Play the sound (non-blocking)
            process.Start();
        }
        catch
        {
            // Fallback: Terminal bell character
            Console.WriteLine("\a"); // ASCII bell character
        }
    }
    
    /// <summary>
    /// Plays alarm sounds on Linux using the 'paplay' command (PulseAudio).
    /// Uses FreeDesktop sound theme alarm sound.
    /// </summary>
    /// <param name="alarmType">Type of alarm sound to play</param>
    private void PlayLinuxAlarm(AlarmType alarmType)
    {
        try
        {
            // Use PulseAudio's paplay to play system sound
            // Most Linux distributions have this sound file
            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "paplay",  // PulseAudio player
                    // FreeDesktop standard alarm sound
                    Arguments = "/usr/share/sounds/freedesktop/stereo/alarm-clock-elapsed.oga",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true  // Capture errors
                }
            };
            
            // Play the sound
            process.Start();
        }
        catch
        {
            // Fallback: Terminal bell
            Console.WriteLine("\a");
        }
    }
}

/// <summary>
/// Types of alarm sounds available.
/// Each type produces a different sound pattern or uses different system sounds.
/// </summary>
public enum AlarmType
{
    /// <summary>
    /// Default alarm sound - single clear tone.
    /// </summary>
    Default,
    
    /// <summary>
    /// Simple beep sound - single medium tone.
    /// </summary>
    Beep,
    
    /// <summary>
    /// Alert sound - double high tone for urgency.
    /// </summary>
    Alert,
    
    /// <summary>
    /// Chime sound - ascending three-tone melody.
    /// </summary>
    Chime
}
