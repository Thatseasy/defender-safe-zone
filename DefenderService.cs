using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DefenderSafeZoneTool
{
    public static class DefenderService
    {
        public static List<string> GetExclusions()
        {
            var script = @"
$pref = Get-MpPreference
$result = @()
if ($pref.ExclusionPath) { foreach ($p in $pref.ExclusionPath) { $result += ""Pfad: $p"" } }
if ($pref.ExclusionExtension) { foreach ($e in $pref.ExclusionExtension) { $result += ""Erweiterung: $e"" } }
$result -join [Environment]::NewLine
";
            var (output, error) = PowerShellHelper.Execute(script);
            if (!string.IsNullOrWhiteSpace(error) && !error.Contains("Es wurde kein solcher Parameter gefunden", StringComparison.OrdinalIgnoreCase))
            {
                // Ignoriere harmlose Warnings, werfe Fehler bei echtem Problem
                // PowerShell schreibt oft "gelbe" Warnings in StdErr, bei echten Fehlern meist mehr Details.
                if (error.ToLower().Contains("error") || error.ToLower().Contains("ausnahme") || error.ToLower().Contains("verweigert"))
                {
                    throw new Exception($"Fehler beim Abrufen der Ausschlüsse:\n{error}");
                }
            }

            return output.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries)
                         .Where(line => !string.IsNullOrWhiteSpace(line))
                         .Select(line => line.Trim())
                         .ToList();
        }

        public static void AddExclusions(string folderPath, IEnumerable<string> extensions)
        {
            if (!string.IsNullOrWhiteSpace(folderPath))
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var script = $"Add-MpPreference -ExclusionPath '{folderPath.Replace("'", "''")}'";
                var (output, error) = PowerShellHelper.Execute(script);
                if (!string.IsNullOrWhiteSpace(error) && error.ToLower().Contains("error"))
                {
                    throw new Exception($"Fehler beim Hinzufügen des Ordners:\n{error}");
                }
            }

            var exts = NormalizeExtensions(extensions);
            if (exts.Any())
            {
                var extString = string.Join(",", exts.Select(e => $"'{e.Replace("'", "''")}'"));
                var script = $"Add-MpPreference -ExclusionExtension {extString}";
                var (output, error) = PowerShellHelper.Execute(script);
                if (!string.IsNullOrWhiteSpace(error) && error.ToLower().Contains("error"))
                {
                    throw new Exception($"Fehler beim Hinzufügen der Erweiterungen:\n{error}");
                }
            }
        }

        public static void RemoveExclusions(string folderPath, IEnumerable<string> extensions)
        {
            if (!string.IsNullOrWhiteSpace(folderPath))
            {
                var script = $"Remove-MpPreference -ExclusionPath '{folderPath.Replace("'", "''")}' -ErrorAction SilentlyContinue";
                var (output, error) = PowerShellHelper.Execute(script);
            }

            var exts = NormalizeExtensions(extensions);
            if (exts.Any())
            {
                var extString = string.Join(",", exts.Select(e => $"'{e.Replace("'", "''")}'"));
                var script = $"Remove-MpPreference -ExclusionExtension {extString} -ErrorAction SilentlyContinue";
                var (output, error) = PowerShellHelper.Execute(script);
            }
        }

        public static List<string> NormalizeExtensions(IEnumerable<string> extensions)
        {
            if (extensions == null) return new List<string>();

            return extensions
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .Select(e => e.Trim().ToLower())
                .Select(e => e.StartsWith(".") ? e : "." + e)
                .Distinct()
                .ToList();
        }

        public static void OpenWindowsSecurity()
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "windowsdefender:",
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(startInfo);
        }
    }
}
