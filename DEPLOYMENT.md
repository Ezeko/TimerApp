# Timer App - Deployment and Distribution Guide

## 📦 How to Package and Distribute Your App to Other Computers

This guide explains how to build and distribute the Timer App so others can run it on their computers.

---

## Quick Summary

**Where to find the built app files:**
```
/Users/ezeko/dotnet/TimerApp/bin/Release/net7.0/publish/
```

After running the publish command, this folder contains all files needed to run the app on another computer.

---

## Table of Contents

1. [Understanding the Build Process](#understanding-the-build-process)
2. [Building for Your Current Platform](#building-for-your-current-platform)
3. [Building for Specific Platforms](#building-for-specific-platforms)
4. [Distribution Methods](#distribution-methods)
5. [File Structures Explained](#file-structures-explained)
6. [Troubleshooting](#troubleshooting)

---

## Understanding the Build Process

### Development vs Release Builds

- **Debug Build** (`dotnet build`):  
  - Located in: `bin/Debug/net7.0/`
  - Larger file size
  - Includes debugging symbols
  - For development only

- **Release Build** (`dotnet build -c Release`):  
  - Located in: `bin/Release/net7.0/`
  - Optimized for performance
  - Smaller file size
  - For distribution

### Self-Contained vs Framework-Dependent

- **Self-Contained** (`--self-contained`):  
  - ✅ Includes .NET runtime
  - ✅ Users don't need .NET installed
  - ✅ Larger download size (~50-70 MB)
  - ✅ **Recommended for distribution**

- **Framework-Dependent** (`--no-self-contained`):  
  - ❌ Requires .NET 7.0 installed on target computer
  - ✅ Smaller download size (~5 MB)
  - ❌ Not recommended unless you know users have .NET

---

## Building for Your Current Platform

### Step 1: Navigate to Project Directory

```bash
cd /Users/ezeko/dotnet/TimerApp
```

### Step 2: Publish the App (Self-Contained)

```bash
dotnet publish -c Release --self-contained
```

### Step 3: Find the Built Files

The complete app will be in:
```
/Users/ezeko/dotnet/TimerApp/bin/Release/net7.0/osx-x64/publish/
```

**What's in this folder:**
- `TimerApp` (or `TimerApp.exe` on Windows) - The main executable
- `TimerApp.dll` - The application library
- All dependency DLLs (Avalonia, etc.)
- `Assets/` folder - Icons and resources
- .NET runtime files (if self-contained)

### Step 4: Test the Published Build

```bash
cd bin/Release/net7.0/osx-x64/publish/
./TimerApp
```

If it runs correctly, you're ready to distribute!

---

## Building for Specific Platforms

### For macOS (Current Platform)

#### Intel Macs (x64):
```bash
dotnet publish -c Release -r osx-x64 --self-contained
```
Output: `bin/Release/net7.0/osx-x64/publish/`

#### Apple Silicon Macs (M1/M2/M3):
```bash
dotnet publish -c Release -r osx-arm64 --self-contained
```
Output: `bin/Release/net7.0/osx-arm64/publish/`

#### Universal macOS Build (Both Intel & Apple Silicon):
```bash
# Build for x64
dotnet publish -c Release -r osx-x64 --self-contained

# Build for arm64
dotnet publish -c Release -r osx-arm64 --self-contained
```

Then create a macOS app bundle (see macOS App Bundle section below).

---

### For Windows

#### Windows 64-bit (Most Common):
```bash
dotnet publish -c Release -r win-x64 --self-contained
```
Output: `bin/Release/net7.0/win-x64/publish/`

#### Windows 32-bit:
```bash
dotnet publish -c Release -r win-x86 --self-contained
```
Output: `bin/Release/net7.0/win-x86/publish/`

#### Windows ARM64:
```bash
dotnet publish -c Release -r win-arm64 --self-contained
```

**Files for Windows Users:**
Send the entire `win-x64/publish/` folder. Users run `TimerApp.exe`.

---

### For Linux

#### Linux 64-bit:
```bash
dotnet publish -c Release -r linux-x64 --self-contained
```
Output: `bin/Release/net7.0/linux-x64/publish/`

#### Linux ARM64 (Raspberry Pi, etc.):
```bash
dotnet publish -c Release -r linux-arm64 --self-contained
```

**Files for Linux Users:**
Send the entire `linux-x64/publish/` folder. Users run `./TimerApp` from terminal.

---

## Distribution Methods

### Method 1: ZIP File (Simplest)

1. **Build for target platform:**
   ```bash
   dotnet publish -c Release -r win-x64 --self-contained
   ```

2. **Compress the publish folder:**
   ```bash
   cd bin/Release/net7.0/win-x64/
   zip -r TimerApp-Windows-v1.0.zip publish/
   ```

3. **Distribute the ZIP file:**
   - Upload to cloud storage (Dropbox, Google Drive, etc.)
   - Send via email (if under 25 MB)
   - Upload to GitHub Releases
   - Share via file transfer service

4. **User instructions:**
   ```
   1. Download and extract TimerApp-Windows-v1.0.zip
   2. Open the extracted folder
   3. Double-click TimerApp.exe (Windows) or TimerApp (Mac/Linux)
   ```

---

### Method 2: Installer (Advanced)

#### For Windows - Create MSI Installer

You can use tools like:
- **WiX Toolset** - Free, open-source
- **Advanced Installer** - Commercial with free version
- **Squirrel.Windows** - Modern installer framework

#### For macOS - Create .app Bundle

1. Create app bundle structure:
   ```bash
   mkdir -p TimerApp.app/Contents/MacOS
   mkdir -p TimerApp.app/Contents/Resources
   ```

2. Copy files:
   ```bash
   cp -r bin/Release/net7.0/osx-x64/publish/* TimerApp.app/Contents/MacOS/
   ```

3. Create Info.plist:
   ```xml
   <?xml version="1.0" encoding="UTF-8"?>
   <!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
   <plist version="1.0">
   <dict>
       <key>CFBundleName</key>
       <string>Timer App</string>
       <key>CFBundleExecutable</key>
       <string>TimerApp</string>
       <key>CFBundleIdentifier</key>
       <string>com.yourname.timerapp</string>
       <key>CFBundleVersion</key>
       <string>1.0.0</string>
   </dict>
   </plist>
   ```

4. Create DMG for distribution:
   ```bash
   hdiutil create -volname "Timer App" -srcfolder TimerApp.app -ov -format UDZO TimerApp-v1.0.dmg
   ```

#### For Linux - Create .deb or AppImage

Tools:
- **dpkg** - Create .deb packages for Debian/Ubuntu
- **rpmbuild** - Create .rpm packages for Fedora/RedHat
- **AppImage** - Universal Linux package format

---

### Method 3: Package Managers

#### Homebrew (macOS)
Create a Homebrew formula for easy installation:
```bash
brew install yourname/timerapp/timerapp
```

#### Chocolatey (Windows)
Create a Chocolatey package:
```bash
choco install timerapp
```

#### Snap/Flatpak (Linux)
Package as Snap or Flatpak for universal Linux distribution.

---

## File Structures Explained

### Complete Project Structure

```
TimerApp/
├── Assets/                          # Icon and resource files
│   └── avalonia-logo.ico
├── Models/                          # Data models
│   └── TimerPreset.cs
├── Services/                        # Business logic services
│   └── AlarmSoundService.cs
├── ViewModels/                      # MVVM ViewModels
│   ├── MainWindowViewModel.cs
│   ├── SettingsViewModel.cs
│   ├── TimerDisplayViewModel.cs
│   └── ViewModelBase.cs
├── Views/                           # UI Views (XAML)
│   ├── MainWindow.axaml
│   ├── MainWindow.axaml.cs
│   ├── SettingsWindow.axaml
│   ├── SettingsWindow.axaml.cs
│   ├── TimerDisplayWindow.axaml
│   └── TimerDisplayWindow.axaml.cs
├── App.axaml                        # Application resources
├── App.axaml.cs                     # Application startup
├── Program.cs                       # Entry point
├── ViewLocator.cs                   # MVVM view resolution
├── TimerApp.csproj                  # Project configuration
├── app.manifest                     # Windows manifest
├── obj/                             # Build intermediate files (don't distribute)
├── bin/                             # Build output
│   ├── Debug/                       # Debug builds
│   └── Release/                     # Release builds
│       └── net7.0/
│           └── [platform]/
│               └── publish/         # ✅ DISTRIBUTE THIS FOLDER
├── README.md                        # Documentation
└── FEATURES.md                      # Feature list
```

### What Files to Include in Distribution

**Include (from `publish/` folder):**
- ✅ TimerApp executable (`.exe` on Windows, no extension on Mac/Linux)
- ✅ TimerApp.dll
- ✅ All .dll files (dependencies)
- ✅ Assets/ folder
- ✅ Any .json config files
- ✅ .runtimeconfig.json
- ✅ .deps.json

**Don't Include:**
- ❌ obj/ folder
- ❌ .pdb files (debugging symbols) - unless needed for debugging
- ❌ .cs source code files
- ❌ .csproj project file
- ❌ bin/Debug/ folder

---

## Platform-Specific Notes

### macOS

**Gatekeeper Issues:**
Users may see "App can't be opened because it is from an unidentified developer"

**Solutions:**
1. Right-click app → Open → Open anyway
2. Or run in terminal: `xattr -cr TimerApp.app`
3. Or sign the app with Apple Developer certificate

**Making it easier:**
- Create a .dmg installer
- Sign and notarize with Apple Developer account ($99/year)

---

### Windows

**SmartScreen Warning:**
Users may see "Windows protected your PC"

**Solutions:**
1. Click "More info" → "Run anyway"
2. Or sign the .exe with a code signing certificate

**Making it easier:**
- Create an MSI installer
- Sign with code signing certificate
- Submit to Microsoft for reputation

---

### Linux

**Permission Issues:**
Users might need to make file executable:
```bash
chmod +x TimerApp
```

**Making it easier:**
- Create .deb/.rpm packages
- Or use AppImage/Flatpak/Snap
- Include a simple install.sh script

---

## Quick Distribution Commands

### Build for All Platforms at Once

Create a script `build-all.sh`:

```bash
#!/bin/bash

# Build for macOS Intel
echo "Building for macOS (x64)..."
dotnet publish -c Release -r osx-x64 --self-contained
cd bin/Release/net7.0/osx-x64/
zip -r ../../../../TimerApp-macOS-Intel-v1.0.zip publish/
cd ../../../..

# Build for macOS Apple Silicon
echo "Building for macOS (ARM64)..."
dotnet publish -c Release -r osx-arm64 --self-contained
cd bin/Release/net7.0/osx-arm64/
zip -r ../../../../TimerApp-macOS-AppleSilicon-v1.0.zip publish/
cd ../../../..

# Build for Windows 64-bit
echo "Building for Windows (x64)..."
dotnet publish -c Release -r win-x64 --self-contained
cd bin/Release/net7.0/win-x64/
zip -r ../../../../TimerApp-Windows-v1.0.zip publish/
cd ../../../..

# Build for Linux 64-bit
echo "Building for Linux (x64)..."
dotnet publish -c Release -r linux-x64 --self-contained
cd bin/Release/net7.0/linux-x64/
zip -r ../../../../TimerApp-Linux-v1.0.zip publish/
cd ../../../..

echo "All builds complete! ZIP files created in project root."
```

Make it executable and run:
```bash
chmod +x build-all.sh
./build-all.sh
```

---

## Sizing Information

Typical published app sizes:

| Platform | Self-Contained | Framework-Dependent |
|----------|----------------|---------------------|
| Windows x64 | ~65 MB | ~5 MB |
| macOS x64 | ~60 MB | ~5 MB |
| macOS ARM64 | ~55 MB | ~5 MB |
| Linux x64 | ~70 MB | ~5 MB |

**Note:** Self-contained is larger but doesn't require .NET installation.

---

## Sharing Your App

### Option 1: GitHub Releases (Recommended)

1. Push code to GitHub
2. Go to Releases → Create new release
3. Upload ZIP files for each platform
4. Users download from GitHub

### Option 2: Cloud Storage

Upload to:
- Google Drive
- Dropbox
- OneDrive
- WeTransfer

Share the link!

### Option 3: Personal Website

Host the ZIP files on your own website.

---

## User Installation Instructions

### For Windows Users

```
1. Download TimerApp-Windows-v1.0.zip
2. Right-click → Extract All
3. Open the extracted folder
4. Double-click TimerApp.exe
5. If Windows SmartScreen appears, click "More info" → "Run anyway"
```

### For macOS Users

```
1. Download TimerApp-macOS-Intel-v1.0.zip (or AppleSilicon for M1/M2/M3)
2. Double-click to extract
3. Double-click TimerApp
4. If Gatekeeper blocks it, right-click → Open → Open
```

### For Linux Users

```
1. Download TimerApp-Linux-v1.0.zip
2. Extract: unzip TimerApp-Linux-v1.0.zip
3. Make executable: chmod +x publish/TimerApp
4. Run: ./publish/TimerApp
```

---

## Troubleshooting

### "The application cannot be loaded"
- Ensure .NET 7.0 is installed (if framework-dependent)
- Or use self-contained build

### "Missing DLL" errors
- Use `--self-contained` flag
- Don't delete any files from publish folder

### App doesn't start on macOS
- Run: `xattr -cr TimerApp` to remove quarantine attribute
- Or right-click → Open instead of double-clicking

### Large file size
- This is normal for self-contained apps
- Consider framework-dependent for smaller size
- Or use single-file publish: `-p:PublishSingleFile=true`

---

## Advanced: Single File Executable

Create a single .exe/.app file instead of folder:

```bash
dotnet publish -c Release -r win-x64 --self-contained \
  -p:PublishSingleFile=true \
  -p:IncludeNativeLibrariesForSelfExtract=true
```

**Pros:**
- Single file to distribute
- Easier for users

**Cons:**
- Slightly slower startup
- Larger file size
- May trigger antivirus warnings

---

## Summary

**Quick Steps to Distribute:**

1. **Build:**
   ```bash
   dotnet publish -c Release -r win-x64 --self-contained
   ```

2. **Package:**
   ```bash
   cd bin/Release/net7.0/win-x64/
   zip -r TimerApp-Windows.zip publish/
   ```

3. **Share:**
   Upload `TimerApp-Windows.zip` to Google Drive, GitHub, etc.

4. **Instruct Users:**
   "Download, extract, and run TimerApp.exe"

**That's it!** Your app is ready for distribution! 🎉

---

## Additional Resources

- [.NET Publishing Docs](https://docs.microsoft.com/en-us/dotnet/core/deploying/)
- [Avalonia Deployment Guide](https://docs.avaloniaui.net/docs/deployment)
- [Runtime Identifiers (RIDs)](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog)

---

## Questions?

If users have issues running the app:
1. Check they extracted the ZIP file
2. Check they're running the right platform version
3. Try self-contained build if framework-dependent fails
4. Check antivirus isn't blocking the app
