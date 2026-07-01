Add-Type -AssemblyName System.Windows.Forms

$resEN = New-Object System.Resources.ResXResourceWriter(".\Resources\Strings.resx")
$resDE = New-Object System.Resources.ResXResourceWriter(".\Resources\Strings.de.resx")

function Add-Res ($name, $valEN, $valDE) {
    $resEN.AddResource($name, $valEN)
    $resDE.AddResource($name, $valDE)
}

Add-Res "AppTitle" "Defender SafeZone Tool" "Defender SafeZone Tool"
Add-Res "StatusReady" "Ready." "Bereit."
Add-Res "HintsText" "HINTS:`n• Use dedicated target folders (e.g. C:\Work). Do not add entire drives or your Downloads folder!`n• Accidentally deleted files can be restored in Windows Security under `"Protection history`"." "HINWEISE:`n• Nutze dedizierte Zielordner (z.B. C:\Work). Füge keine ganzen Laufwerke oder Downloads hinzu!`n• Fälschlich gelöschte Dateien können in der Windows-Sicherheit unter `"Schutzverlauf`" wiederhergestellt werden."
Add-Res "TargetFolder" "Target folder:" "Zielordner:"
Add-Res "BrowseFolder" "Browse folder..." "Ordner wählen..."
Add-Res "AddExclusion" "Add exclusion" "Ausnahme hinzufügen"
Add-Res "Extensions" "Extensions:" "Erweiterungen:"
Add-Res "ExtensionsHelp" "File extensions (e.g. .exe, .dll) are excluded GLOBALLY for the entire system from Defender scans, not just in the target folder." "Dateitypen (z.B. .exe, .dll) werden GLOBAL für das gesamte System von Defender-Scans ausgeschlossen, nicht nur im Zielordner."
Add-Res "Refresh" "Refresh" "Aktualisieren"
Add-Res "OpenSecurity" "Open Windows Security" "Windows-Sicherheit öffnen"
Add-Res "CurrentExclusions" "Current Exclusions" "Aktuelle Ausnahmen"
Add-Res "RemoveSelected" "Remove selected exclusions" "Ausgewählte Ausnahmen entfernen"
Add-Res "BrowseDialogDesc" "Select a target folder for Defender exclusions:" "Wähle einen Zielordner für Defender-Ausnahmen aus:"
Add-Res "SuccessAdd" "Exclusion successfully added." "Ausnahme erfolgreich hinzugefügt."
Add-Res "SelectToRemove" "Please select exclusions from the list first." "Bitte wähle zuerst Ausnahmen aus der Liste aus."
Add-Res "NoSelection" "No selection" "Keine Auswahl"
Add-Res "SuccessRemove" "Selected exclusions successfully removed." "Ausgewählte Ausnahmen erfolgreich entfernt."
Add-Res "ListRefreshed" "Exclusion list refreshed." "Liste der Ausnahmen aktualisiert."
Add-Res "ErrorOpenSecurity" "Error opening Windows Security: {0}" "Fehler beim Öffnen der Windows-Sicherheit: {0}"
Add-Res "ErrorTitle" "Error" "Fehler"
Add-Res "Loading" "Loading exclusions..." "Lade Ausnahmen..."
Add-Res "ErrorLoading" "Error loading: {0}" "Fehler beim Laden: {0}"
Add-Res "NoExclusionsFound" "No exclusions found." "Keine Ausnahmen gefunden."
Add-Res "ActionRunning" "{0} running..." "{0} läuft..."
Add-Res "ErrorAction" "Error during {0}:`n{1}" "Fehler beim {0}:`n{1}"
Add-Res "ErrorActionStatus" "Error during {0}." "Fehler beim {0}."
Add-Res "PathPrefix" "Path: " "Pfad: "
Add-Res "ExtensionPrefix" "Extension: " "Erweiterung: "
Add-Res "ActionAdd" "Adding" "Hinzufügen"
Add-Res "ActionRemove" "Removing" "Entfernen"
Add-Res "ActionRefresh" "Refreshing" "Aktualisieren"
Add-Res "ErrorNoInput" "At least one target folder or file extension must be specified." "Es muss mindestens ein Zielordner oder eine Dateierweiterung angegeben werden."
Add-Res "ErrorInvalidPath" "The path '{0}' is not a valid absolute path (e.g. C:\Folder)." "Der Pfad '{0}' ist kein gültiger absoluter Pfad (z.B. C:\Ordner)."
Add-Res "ErrorAccessDenied" "Access denied when creating '{0}'. Please check your permissions." "Zugriff verweigert beim Erstellen von '{0}'. Bitte prüfe deine Berechtigungen."
Add-Res "ErrorCreateFolder" "The folder could not be created or checked: {0}" "Der Ordner konnte nicht erstellt oder geprüft werden: {0}"
Add-Res "ErrorFetchExclusions" "Error fetching exclusions:`n{0}" "Fehler beim Abrufen der Ausnahmen:`n{0}"

$resEN.Generate()
$resEN.Close()
$resDE.Generate()
$resDE.Close()
