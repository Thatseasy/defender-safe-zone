# UI and Logic Refactoring Plan

## 1. Text Replacements
- [x] Rename "Projektordner" to "Zielordner"
- [x] Rename all instances of "Ausschluss/Ausschlüsse" to "Ausnahme/Ausnahmen"

## 2. Input Area Layout
- [x] Row 1 (Folders): Label "Zielordner:", TextBox, FlowLayoutPanel containing "Ordner wählen..." and "Ausnahme hinzufügen"
- [x] Row 2 (Extensions): Label "Erweiterungen:", TextBox (empty default), Label "(?)" with ToolTip, Button "Ausnahme hinzufügen"

## 3. ToolTip for Extensions
- [x] Add a `ToolTip` component.
- [x] Set tool tip text on the "(?)" label explaining that extension exclusions apply globally.

## 4. ListBox and Remove Button
- [x] Set `listExclusions.SelectionMode = SelectionMode.MultiExtended`
- [x] Move the "Ausnahmen entfernen" button to the bottom of the ListBox (inside the GroupBox or listContainer).
- [x] Update `BtnRemove_Click` to read `listExclusions.SelectedItems`.
- [x] Parse "Pfad: [path]" or "Erweiterung: [ext]" from selected items.
- [x] Call `DefenderService.RemoveExclusions` with the parsed paths and extensions.

## 5. Answer Question about Localization
- [x] Address Question 5 in the chat reply.
