# Timer App

A modern, cross-platform desktop timer application built with C# and Avalonia UI.

## Features

### ⏱️ Core Timer Functionality
- **Countdown Timer**: Set hours, minutes, and seconds for precise timing
- **Negative Time Counting**: Timer continues into negative values after reaching zero, allowing you to see how much time has passed
- **Start/Pause/Resume**: Full control over timer state with keyboard shortcuts
- **Reset**: Quickly reset to default or last set time

### 🎨 Modern UI Design
- **Dark Theme**: Beautiful gradient-based dark theme with vibrant accent colors
- **Responsive Controls**: Smooth animations and hover effects
- **Color-Coded Display**: 
  - Cyan (`#00D9FF`) for positive time
  - Red (`#FF4757`) for negative time (overtime)
- **Large, Easy-to-Read Display**: 96px font size for the main timer

### 🖥️ Multi-Screen Support
- **Secondary Display Window**: Open a dedicated display window for second screen
- **Large Format Display**: 144px font optimized for viewing from a distance
- **Real-time Sync**: Display window automatically syncs with main timer
- **Perfect for Presentations**: Ideal for presentations, meetings, or monitoring from across the room

### ⭐ Timer Presets
- **Pre-configured Presets**:
  - ☕ Quick Break (5 min)
  - 🍅 Pomodoro (25 min)
  - ⏸️ Short Break (15 min)
  - 🛋️ Long Break (30 min)
  - 🎤 Presentation (1 hour)
  - 💪 Workout (45 min)
  - 👥 Meeting (1.5 hours)
- **Custom Presets**: Save your own timer configurations
- **One-Click Start**: Instantly start any preset with a single click
- **Visual Icons**: Each preset has a unique emoji icon for easy identification

### ⌨️ Keyboard Shortcuts
- **Space**: Start/Pause timer
- **R**: Reset timer
- **Ctrl+S**: Save current time as a custom preset

### 🔊 Custom Alarm Sounds
- **Alarm on Timer End**: Automatically plays when timer reaches zero
- **Multiple Alarm Types**:
  - Default
  - Beep
  - Alert
  - Chime
- **Platform-Specific Sounds**: Uses native system sounds for best experience
- **Test Feature**: Preview alarm sounds before selecting
- **Enable/Disable**: Toggle alarms on or off in settings

### 📌 System Tray Integration
- **Minimize to Tray**: Hide the app to system tray while running
- **Quick Access Menu**: Right-click tray icon for quick actions
- **Show/Hide Window**: Double-click tray icon to show timer
- **Background Operation**: Timer continues running when minimized

### ⚙️ Comprehensive Settings
- **Display Settings**:
  - Font size adjustment (36-144px)
  - Theme selection (Light/Dark)
- **Window Settings**:
  - Always on Top option
  - Show in Taskbar toggle
  - Minimize to System Tray option
- **Sound Settings**:
  - Alarm sound type selection
  - Test alarm button
  - Enable/disable alarms

## Platform Support

This application runs on:
- ✅ **Windows** 10/11
- ✅ **macOS** 10.15+
- ✅ **Linux** (Ubuntu, Fedora, etc.)

## Requirements

- .NET 7.0 SDK or higher
- Supported on Windows, macOS, and Linux

## Building and Running

### Build the application:
```bash
cd /Users/ezeko/dotnet/TimerApp
dotnet build
```

### Run the application:
```bash
dotnet run
```

### Publish for distribution:
```bash
# For current platform
dotnet publish -c Release

# For specific platform (example for macOS)
dotnet publish -c Release -r osx-x64 --self-contained

# For Windows
dotnet publish -c Release -r win-x64 --self-contained

# For Linux
dotnet publish -c Release -r linux-x64 --self-contained
```

## Usage Guide

### Basic Timer Operation

#### Setting a Timer Manually
1. When the timer is not running, use the numeric inputs to set:
   - Hours (0-23)
   - Minutes (0-59)
   - Seconds (0-59)
2. Click **Start** or press **Space** to begin

#### Using Presets
1. Click any preset button (e.g., "🍅 Pomodoro")
2. The timer automatically sets to the preset duration
3. Click **Start** or press **Space** to begin

#### Saving Custom Presets
1. Set your desired time using the numeric inputs
2. Click **➕ Save Current** or press **Ctrl+S**
3. Your preset is saved with a star icon (⭐)

### Timer Controls

#### Starting/Pausing
- Click the **Start** button or press **Space**
- While running, the button changes to **Pause**
- Click **Pause** or press **Space** again to pause
- Click **Resume** to continue from where you paused

#### Resetting
- Click the **Reset** button or press **R**
- This stops the timer and resets to 00:00:00

#### Negative Time (Overtime)
- When the timer reaches 00:00:00, it continues counting into negative values
- The display turns **red** and shows a minus sign (e.g., **-00:05:23**)
- An alarm sounds (if enabled in settings)
- Perfect for tracking how much overtime has occurred

### Multi-Screen Display

#### Opening Display Window
1. Click **🖥️ Open Display**
2. A new window opens with a large timer display
3. Drag this window to your second monitor
4. The display automatically updates with the main timer

#### Use Cases
- **Presentations**: Display timer on projected screen
- **Meetings**: Show remaining time to all participants
- **Workouts**: Place on distant screen for visibility during exercise
- **Teaching**: Show time remaining for activities or exams

### System Tray

#### Minimizing to Tray
1. Click **📌 Minimize to Tray**
2. The app hides from view but continues running
3. A tray icon appears in your system tray

#### Accessing from Tray
- **Double-click** the tray icon to show the window
- **Right-click** for menu options:
  - Show Timer
  - Exit

### Settings Configuration

#### Opening Settings
1. Click **⚙️ Settings**
2. The settings window opens

#### Configuring Alarms
1. Toggle **Play Alarm Sound When Timer Ends**
2. Select your preferred **Alarm Sound Type**
3. Click **🔊 Test** to preview the sound
4. Click **Save Settings**

#### Window Preferences
- **Always on Top**: Keeps timer window above other windows
- **Show in Taskbar**: Toggle taskbar visibility
- **Minimize to System Tray**: Enable tray icon functionality

#### Display Customization
- **Font Size**: Adjust with slider (36-144px)
- **Theme**: Choose Light or Dark mode

## Keyboard Shortcuts Reference

| Shortcut | Action |
|----------|--------|
| **Space** | Start/Pause timer |
| **R** | Reset timer |
| **Ctrl+S** | Save current time as preset |

## Architecture

### Technology Stack
- **Framework**: .NET 7.0
- **UI Framework**: Avalonia UI 11.3.9
- **MVVM**: CommunityToolkit.Mvvm 8.2.1
- **Icons**: Projektanker.Icons.Avalonia 9.4.1
- **Pattern**: MVVM (Model-View-ViewModel)

### Project Structure
```
TimerApp/
├── Models/
│   └── TimerPreset.cs              # Preset data model
├── Services/
│   └── AlarmSoundService.cs        # Cross-platform alarm sounds
├── ViewModels/
│   ├── MainWindowViewModel.cs      # Main timer logic & presets
│   ├── SettingsViewModel.cs        # Settings management
│   ├── TimerDisplayViewModel.cs    # Display window data
│   └── ViewModelBase.cs            # Base ViewModel class
├── Views/
│   ├── MainWindow.axaml            # Main UI with presets
│   ├── MainWindow.axaml.cs         # Main logic & tray support
│   ├── SettingsWindow.axaml        # Settings UI with alarms
│   ├── SettingsWindow.axaml.cs     # Settings logic
│   ├── TimerDisplayWindow.axaml    # Large display UI
│   └── TimerDisplayWindow.axaml.cs # Display logic
├── App.axaml                        # Application resources
├── App.axaml.cs                     # Application entry
└── Program.cs                       # Program entry point
```

## Key Features Implementation

### Negative Time Counting
The timer uses a `System.Timers.Timer` that updates every 100ms. When the remaining time goes below zero:
- The `IsNegative` property triggers
- UI color changes from cyan to red
- Alarm plays (if enabled)
- Minus sign appears in display

### Timer Presets
Presets are stored in an `ObservableCollection<TimerPreset>`:
- Default presets loaded on startup
- Custom presets can be added dynamically
- Each preset has a name, duration, and icon
- One-click application of preset times

### Keyboard Shortcuts
Implemented using Avalonia's `KeyBinding` system:
- Global shortcuts work when window is focused
- Commands bound to ViewModel RelayCommands
- Hints displayed in UI for discoverability

### Cross-Platform Alarm Sounds
The `AlarmSoundService` detects the operating system:
- **Windows**: Uses `Console.Beep()` with different frequencies
- **macOS**: Uses `afplay` command with system sounds
- **Linux**: Uses `paplay` with FreeDesktop sounds
- Graceful fallback to terminal bell if sounds unavailable

### System Tray Integration
Avalonia's `TrayIcon` provides:
- Native system tray icon
- Context menu with actions
- Click/double-click handling
- Show/hide functionality

### Multi-Screen Display
Property change notifications propagate state:
- Main ViewModel publishes timer updates
- Display ViewModel subscribes to changes
- Real-time synchronization across windows
- Independent window positioning

## Tips and Best Practices

### For Presentations
1. Start the timer before your presentation
2. Open the Display window and move to projector screen
3. Enable "Always on Top" in settings
4. Use a longer preset like "Presentation (1 hour)"

### For Pomodoro Technique
1. Use the built in "🍅 Pomodoro (25 min)" preset
2. Enable alarm sounds for break reminders
3. Alternate with "⏸️ Short Break (15 min)"
4. Take a "🛋️ Long Break (30 min)" after 4 pomodoros

### For Meetings
1. Set timer to meeting duration
2. Open Display window for all to see
3. Minimize main window to tray
4. Alarm will notify when time is up

### For Workouts
1. Use "💪 Workout (45 min)" or create custom intervals
2. Open Display window on visible screen
3. Enable loud alarm sound for rest periods
4. Track overtime to see how long you pushed

## Troubleshooting

### Alarm Not Playing
- Check Settings: Ensure "Play Alarm Sound" is enabled
- Test the alarm using the Test button in settings
- On Linux: Ensure `paplay` is installed
- On macOS: Check system sound permissions

### Tray Icon Not Showing
- Check Settings: Ensure "Minimize to System Tray" is enabled
- On Linux: Ensure system tray is available in your desktop environment
- Try clicking "Minimize to Tray" button manually

### Display Window Not Updating
- Ensure the main timer is running
- Close and reopen the Display window
- Check that both windows are from the same app instance

## Future Enhancements

Potential features for future versions:
- [ ] Settings persistence (save preferences)
- [ ] Timer history and logging
- [ ] Statistics and analytics
- [ ] Multiple simultaneous timers
- [ ] Custom alarm sound files
- [ ] Network sync between devices
- [ ] Mobile companion app
- [ ] Voice commands
- [ ] Customizable themes and colors
- [ ] Timer groups and categories
- [ ] Export timer data

## License

This project is open source and available for personal and commercial use.

## Credits

Built with:
- [Avalonia UI](https://avaloniaui.net/) - Cross-platform UI framework
- [CommunityToolkit.Mvvm](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/) - MVVM helpers
- [Projektanker.Icons.Avalonia](https://github.com/Projektanker/Icons.Avalonia) - Icon support

## Screenshots

The app features a modern dark theme with:
- **Main Window**: Timer display, presets, and controls
- **Settings Window**: Comprehensive configuration options
- **Display Window**: Large format timer for second screens
- **System Tray**: Background operation support

All with beautiful gradients, smooth animations, and premium aesthetics.
