# Defender SafeZone Tool

![Defender SafeZone Tool](screenshot.png)

A simple, fast, and multi-lingual graphical user interface to effortlessly manage Windows Defender exclusions. 

Windows Defender provides robust security, but adding exclusions (for example, for developer tools, build folders, or safe game modifications) via the standard Windows Security menus is deeply buried and tedious. **Defender SafeZone Tool** solves this by providing a clean, accessible interface directly on your desktop.

## Features

- **Add Target Folders:** Instantly exclude entire directories (e.g. `C:\Dev` or `C:\Work`) from Defender scans.
- **Add File Extensions:** Globally exclude specific file types (e.g., `.exe`, `.dll`, `.iso`) from being scanned anywhere on your system.
- **Manage Exclusions:** View all your current Defender exclusions in a clean list and easily remove them with a single click.
- **16 Languages Supported:** The tool and its installer automatically adapt to your system language! Supported languages include English, German, Chinese, Hindi, Arabic, Spanish, French, Bengali, Portuguese, Russian, Urdu, Indonesian, Japanese, Turkish, Vietnamese, Korean, and Persian.
- **Direct Access to Windows Security:** Open the native Windows Security dashboard via a dedicated button for deeper configurations or to view your protection history.

## Installation

You can find the ready-to-use installer executable in the `Output` folder of this repository:

1. Download the installer file: [`Output/DefenderSafeZoneTool_Setup.exe`](Output/DefenderSafeZoneTool_Setup.exe)
2. Run the executable as an Administrator (required to modify Windows Defender rules).
3. Follow the setup instructions. The installer will automatically select your preferred language.
4. Launch the application from your desktop or start menu!

## How to Build from Source

To compile the application yourself, you need the **.NET 5.0 SDK** (or newer compatible versions) installed on your machine.

1. Clone this repository:
   ```bash
   git clone https://github.com/Thatseasy/defender-safe-zone.git
   ```
2. Open a terminal in the project directory and build the application:
   ```bash
   dotnet build -c Release
   ```
3. To build the Single Installer EXE, you need [Inno Setup 6](https://jrsoftware.org/isinfo.php). Right-click on `setup.iss` and select "Compile", or run via command line:
   ```bash
   iscc setup.iss
   ```

## Requirements
- Windows 10 or Windows 11
- Administrator privileges (for setting Defender rules)

## Disclaimer
Always exercise caution when excluding folders or file types from your antivirus. Only exclude directories you completely trust (such as dedicated development folders). Never exclude your entire `C:\` drive or your `Downloads` folder!
