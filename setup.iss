[Setup]
AppName=Defender SafeZone Tool
AppVersion=1.0
AppPublisher=SafeZone
DefaultDirName={autopf}\Defender SafeZone Tool
DefaultGroupName=Defender SafeZone Tool
OutputDir=.\Output
OutputBaseFilename=DefenderSafeZoneTool_Setup
Compression=lzma
SolidCompression=yes
PrivilegesRequired=admin
ArchitecturesInstallIn64BitMode=x64
DisableWelcomePage=no
WizardStyle=modern

[Tasks]
Name: "desktopicon"; Description: "Desktop-Symbol erstellen"; GroupDescription: "Zusätzliche Symbole:"

[Files]
Source: "bin\Release\net5.0-windows\win-x64\publish\DefenderSafeZoneTool.exe"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\Defender SafeZone Tool"; Filename: "{app}\DefenderSafeZoneTool.exe"
Name: "{autodesktop}\Defender SafeZone Tool"; Filename: "{app}\DefenderSafeZoneTool.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\DefenderSafeZoneTool.exe"; Description: "Defender SafeZone Tool starten"; Flags: nowait postinstall skipifsilent
