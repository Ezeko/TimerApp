# Quick Reference Guide - Timer App v1.1

## 🚀 What's New in v1.1

### 1. Smart Time Display
- **MM:SS format** when timer is under 1 hour (e.g., "05:30")
- **HH:MM:SS format** when timer is 1 hour or more (e.g., "01:25:30")
- Cleaner, more readable display

### 2. Fullscreen Presentation Mode
- Display window opens in **true fullscreen**
- **Automatically detects external monitors**
- Perfect for presentations, teaching, streaming

### 3. Multi-Monitor Magic
- **External screen connected?** Display goes there automatically
- **Single screen?** Fullscreen on primary display
- **Zero configuration required**

---

## ⚡ Quick Actions

### Start a Timer
```
1. Set time (or click preset like "🍅 Pomodoro")
2. Press SPACE or click Start
3. Done!
```

### Fullscreen Display
```
OPTION A - With External Monitor:
1. Connect projector/external screen
2. Click "🖥️ Open Display"
3. Display appears fullscreen on external screen
4. Control from main window

OPTION B - Single Screen:
1. Click "🖥️ Open Display"
2. Display goes fullscreen
3. Press ESC to return to controls
```

### Exit Fullscreen
```
Press ESC key
```

---

## ⌨️ Keyboard Shortcuts

| Key | Action |
|-----|--------|
| **Space** | Start/Pause |
| **R** | Reset |
| **Ctrl+S** | Save as Preset |
| **ESC** | Exit Fullscreen (Display Window) |

---

## 📊 Time Format Examples

| Duration Set | Display Shows | Format |
|--------------|---------------|--------|
| 30 seconds | 00:30 | MM:SS |
| 5 minutes | 05:00 | MM:SS |
| 25 minutes | 25:00 | MM:SS |
| 59:59 | 59:59 | MM:SS |
| 1 hour | 01:00:00 | HH:MM:SS |
| 1:30:00 | 01:30:00 | HH:MM:SS |
| -10 secs (overtime) | -00:10 | -MM:SS |
| -1:05:00 (overtime) | -01:05:00 | -HH:MM:SS |

---

## 🎯 Common Use Cases

### Presentation (External Screen)
1. Connect laptop to projector
2. Set timer (use "🎤 Presentation" preset if needed)
3. Click "🖥️ Open Display"
4. Timer appears on projector automatically
5. You control from laptop

### OBS Streaming
1. Set timer for stream segment
2. Click "🖥️ Open Display"
3. In OBS: Add "Window Capture"
4. Select "Timer Display" window
5. Perfect timer overlay!

### Classroom Teaching
1. Connect to classroom display
2. Set activity timer
3. Click "🖥️ Open Display"
4. Students see large countdown
5. You control from computer

### Workout Timer
1. Connect to TV (or use laptop screen)
2. Set workout duration
3. Click "🖥️ Open Display"
4. Large timer visible from anywhere in room

---

## 🖥️ Screen Behavior

### Automatic Screen Selection
```
IF external monitor connected:
    → Display opens on external screen (fullscreen)
ELSE:
    → Display opens on primary screen (fullscreen)
```

### Manual Control
- Click "🖥️ Open Display" to open fullscreen
- Press **ESC** to close fullscreen
- Window always stays on top during display

---

## 💡 Pro Tips

1. **For Presentations**: Use external screen auto-detection - just plug in and click "Open Display"
2. **For OBS**: Capture the fullscreen window for clean overlay
3. **For Time Management**: Use presets like "🍅 Pomodoro" for instant 25-minute timer
4. **For Overtime Tracking**: Timer continues into negative (shown in red)
5. **For Quick Exit**: Press ESC key when in fullscreen display

---

## 🎨 Display Window Features

- ✅ **True Fullscreen** - No borders or title bar
- ✅ **External Screen Detection** - Automatic positioning
- ✅ **Massive Font** - Visible from across the room
- ✅ **Perfect Scaling** - Works on any resolution
- ✅ **Always on Top** - Stays visible
- ✅ **Gradient Background** - Professional appearance
- ✅ **Drop Shadow** - Enhanced readability
- ✅ **Color-Coded** - Cyan (time) / Red (overtime)

---

## 📱 .NET Version

**Current:** .NET 7.0
**Status:** Fully supported and optimized

---

## 🔧 System Compatibility

| Platform | Status | Notes |
|----------|--------|-------|
| **Windows** | ✅ Fully Supported | Auto-detects external displays |
| **macOS** | ✅ Fully Supported | Works with external monitors |
| **Linux** | ✅ Fully Supported | Compatible with X11/Wayland |

---

## 🚦 Status Indicators

### Timer Display Color
- **Cyan (#00D9FF)** = Normal countdown
- **Red (#FF4757)** = Overtime (negative time)

### Button Text
- **"Start"** = Timer ready to start
- **"Pause"** = Timer running (click to pause)
- **"Resume"** = Timer paused (click to continue)

---

## 📁 Project Files

```
TimerApp/
├── README.md          → User guide
├── FEATURES.md        → Complete feature list
├── DEPLOYMENT.md      → How to distribute
├── UPDATES.md         → Latest updates (v1.1) ⭐
├── CODE_DOCUMENTATION.md → Developer docs
└── This file (QUICK_REFERENCE.md)
```

---

## 🆘 Troubleshooting

### Display window not opening on external screen
- Ensure external monitor is connected before clicking "Open Display"
- Try unplugging and reconnecting the monitor
- Restart the app after connecting new display

### Can't exit fullscreen
- Press **ESC** key
- Or use Alt+F4 (Windows) / Cmd+Q (Mac)

### Timer display too small/large
- Window automatically scales to fit screen
- Viewbox ensures perfect sizing on any resolution
- Display optimized for 1080p and above

---

## ✨ Quick Start Examples

### Example 1: 5-Minute Break Timer
```
1. Click "☕ Quick Break" preset
2. Display shows: 05:00
3. Press Space to start
4. Alarm sounds when done
```

### Example 2: Presentation with Projector
```
1. Plug in projector
2. Click "🎤 Presentation" preset (1 hour)
3. Click "🖥️ Open Display"
4. Fullscreen timer appears on projector
5. Control from laptop
```

### Example 3: Pomodoro Workflow
```
1. Click "🍅 Pomodoro" (25 min)
2. Press Space to start
3. Work until alarm
4. Click "⏸️ Short Break" (15 min)
5. Repeat!
```

---

## 🎯 Remember

- **Minutes and seconds** always show (MM:SS minimum)
- **Hours** only show when needed (HH:MM:SS)
- **External screens** detected automatically
- **ESC key** exits fullscreen
- **Space bar** starts/pauses timer

---

**Version:** 1.1.0  
**Build Date:** 2025-11-19  
**Status:** Production Ready ✅

Happy timing! ⏱️
