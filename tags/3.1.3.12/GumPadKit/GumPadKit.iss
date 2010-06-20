[Setup]
AppName=GumPad
AppVerName=GumPad 3.1.3.7
AppPublisher=Pradyumna Revur
AppCopyright=© 2007-2010 Pradyumna Revur. All rights reserved.
AppPublisherURL=http://gumpad.org/
AppSupportURL=http://gumpad.org/
AppUpdatesURL=http://gumpad.org/
AppVersion=3.1.3.7
DefaultDirName={pf}\GumPad
DefaultGroupName=GumPad
LicenseFile=..\GumLib\License.txt
OutputDir=.
OutputBaseFilename=gumpad-3.1.3.7
VersionInfoVersion=3.1.3.7
VersionInfoDescription=A notepad like editor and Microsoft Office Word Add In for composing text in Indian languages.
SetupIconFile=..\img\gumpad.ico
;WizardImageFile=..\img\installer.bmp
Compression=lzma
SolidCompression=yes
ChangesAssociations=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked


[Registry]
Root: HKCR; Subkey: ".gpd"; ValueType: string; ValueName: ""; ValueData: "GumPadFile"; Flags: uninsdeletevalue
;".gpd" is the extension we're associating. "GumPadFile" is the internal name for the file type as stored in the registry. Make sure you use a unique name for this so you don't inadvertently overwrite another application's registry key.
Root: HKCR; Subkey: "GumPadFile"; ValueType: string; ValueName: ""; ValueData: "GumPad File"; Flags: uninsdeletekey
;"GumPad File" above is the name for the file type as shown in Explorer.
;Root: HKCR; Subkey: "GumPadFile\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\gumpad_explorer.ico"
;"DefaultIcon" is the registry key that specifies the filename containing the icon to associate with the file type. ",0" tells Explorer to use the first icon from GumPad.exe. (",1" would mean the second icon.)
Root: HKCR; Subkey: "GumPadFile\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\GumPad.exe"" ""%1"""
;"shell\open\command" is the registry key that specifies the program to execute when a file of the type is double-clicked in Explorer. The surrounding quotes are in the command line so it handles long filenames correctly.

[Files]
Source: "..\GumPad\bin\Release\GumLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\GumPad\bin\Release\GumPad.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\gumc\bin\Release\gumc.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\GumPad\bin\Release\License.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\GumPad\bin\Release\gumpad.map"; DestDir: "{app}\maps"; Flags: ignoreversion
Source: "..\GumPad\bin\Release\gumpad.xsl"; DestDir: "{app}\maps"; Flags: ignoreversion
Source: "..\GumPad\bin\Release\gumpad.css"; DestDir: "{app}\maps"; Flags: ignoreversion
Source: "..\GumPad\bin\Release\itrans.map"; DestDir: "{app}\maps"; Flags: ignoreversion
Source: "..\GumPad\bin\Release\itrans_no_swaras.map"; DestDir: "{app}\maps"; Flags: ignoreversion
;Source: "..\publish\GumPadWordAddIn_3_0_3_8.zip"; DestDir: "{app}\Office2007WordAddIn"; Flags: ignoreversion
Source: "Gumpad.exe.local"; DestDir: "{app}"; Flags: ignoreversion
Source: "gumc.exe.local"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\img\gumpad.ico"; DestDir: "{app}"; Flags: ignoreversion
;Source: "..\img\gumpad_explorer.ico"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\GumPad"; Filename: "{app}\GumPad.exe"; IconFileName: "{app}\GumPad.ico";
Name: "{commondesktop}\GumPad"; Filename: "{app}\GumPad.exe"; IconFileName: "{app}\GumPad.ico"; Tasks: desktopicon

[Run]
Filename: "{app}\GumPad.exe"; Description: "{cm:LaunchProgram,GumPad}"; Flags: nowait postinstall skipifsilent
