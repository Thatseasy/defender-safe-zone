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

[Languages]

Name: "en"; MessagesFile: "compiler:Default.isl"
Name: "de"; MessagesFile: "compiler:Languages\German.isl"
Name: "zh"; MessagesFile: "compiler:Default.isl,Languages\zh.isl"
Name: "hi"; MessagesFile: "compiler:Default.isl,Languages\hi.isl"
Name: "ar"; MessagesFile: "compiler:Default.isl,Languages\ar.isl"
Name: "es"; MessagesFile: "compiler:Languages\Spanish.isl"
Name: "fr"; MessagesFile: "compiler:Languages\French.isl"
Name: "bn"; MessagesFile: "compiler:Default.isl,Languages\bn.isl"
Name: "pt"; MessagesFile: "compiler:Languages\Portuguese.isl"
Name: "ru"; MessagesFile: "compiler:Languages\Russian.isl"
Name: "ur"; MessagesFile: "compiler:Default.isl,Languages\ur.isl"
Name: "id"; MessagesFile: "compiler:Default.isl,Languages\id.isl"
Name: "ja"; MessagesFile: "compiler:Languages\Japanese.isl"
Name: "tr"; MessagesFile: "compiler:Languages\Turkish.isl"
Name: "vi"; MessagesFile: "compiler:Default.isl,Languages\vi.isl"
Name: "ko"; MessagesFile: "compiler:Default.isl,Languages\ko.isl"
Name: "fa"; MessagesFile: "compiler:Default.isl,Languages\fa.isl"

[CustomMessages]
en.DesktopIcon=Create a desktop shortcut
de.DesktopIcon=Desktop-Symbol erstellen
en.AdditionalIcons=Additional shortcuts:
de.AdditionalIcons=Zusätzliche Symbole:
en.RunApp=Launch Defender SafeZone Tool
de.RunApp=Defender SafeZone Tool starten
zh.DesktopIcon=创建桌面快捷方式
zh.AdditionalIcons=其他快捷方式：
zh.RunApp=启动 Defender SafeZone 工具
hi.DesktopIcon=एक डेस्कटॉप शॉर्टकट बनाएं
hi.AdditionalIcons=अतिरिक्त शॉर्टकट:
hi.RunApp=डिफेंडर सेफज़ोन टूल लॉन्च करें
ar.DesktopIcon=أنشئ اختصارًا على سطح المكتب
ar.AdditionalIcons=اختصارات إضافية:
ar.RunApp=قم بتشغيل Defender SafeZone Tool
es.DesktopIcon=Crear un acceso directo en el escritorio
es.AdditionalIcons=Accesos directos adicionales:
es.RunApp=Inicie la herramienta Defender SafeZone
fr.DesktopIcon=Créer un raccourci sur le bureau
fr.AdditionalIcons=Raccourcis supplémentaires :
fr.RunApp=Lancer Defender SafeZone Tool
bn.DesktopIcon=একটি ডেস্কটপ শর্টকাট তৈরি করুন
bn.AdditionalIcons=অতিরিক্ত শর্টকাট:
bn.RunApp=ডিফেন্ডার সেফজোন টুল চালু করুন
pt.DesktopIcon=Crie um atalho na área de trabalho
pt.AdditionalIcons=Atalhos adicionais:
pt.RunApp=Inicie a ferramenta Defender SafeZone
ru.DesktopIcon=Создайте ярлык на рабочем столе
ru.AdditionalIcons=Дополнительные ярлыки:
ru.RunApp=Запустите Defender SafeZone Tool
ur.DesktopIcon=ایک ڈیسک ٹاپ شارٹ کٹ بنائیں
ur.AdditionalIcons=اضافی شارٹ کٹس:
ur.RunApp=ڈیفنڈر سیف زون ٹول لانچ کریں
id.DesktopIcon=Buat pintasan desktop
id.AdditionalIcons=Pintasan tambahan:
id.RunApp=Luncurkan Alat SafeZone Defender
ja.DesktopIcon=デスクトップ ショートカットを作成します
ja.AdditionalIcons=追加のショートカット:
ja.RunApp=Defender SafeZone ツールを起動します
tr.DesktopIcon=Bir masaüstü kısayolu oluşturun
tr.AdditionalIcons=Ek kısayollar:
tr.RunApp=Defender SafeZone Aracını başlatın
vi.DesktopIcon=Tạo lối tắt trên màn hình
vi.AdditionalIcons=Phím tắt bổ sung:
vi.RunApp=Khởi chạy Công cụ SafeZone của Defender
ko.DesktopIcon=바탕 화면 바로 가기 만들기
ko.AdditionalIcons=추가 바로 가기:
ko.RunApp=Defender SafeZone 도구 실행
fa.DesktopIcon=ایجاد یک میانبر دسکتاپ
fa.AdditionalIcons=میانبرهای اضافی:
fa.RunApp=راه اندازی ابزار Defender SafeZone

[Tasks]
Name: "desktopicon"; Description: "{cm:DesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"

[Files]
Source: "bin\Release\net5.0-windows\win-x64\publish\DefenderSafeZoneTool.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net5.0-windows\win-x64\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; The line above copies all dlls and the 'de' language folder required by .NET 5

[Icons]
Name: "{group}\Defender SafeZone Tool"; Filename: "{app}\DefenderSafeZoneTool.exe"
Name: "{autodesktop}\Defender SafeZone Tool"; Filename: "{app}\DefenderSafeZoneTool.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\DefenderSafeZoneTool.exe"; Description: "{cm:RunApp}"; Flags: nowait postinstall skipifsilent
