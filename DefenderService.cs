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
            var pathPrefix = Strings.Get("PathPrefix");
            var extPrefix = Strings.Get("ExtensionPrefix");

            var script = $@"
$pref = Get-MpPreference
$result = @()
if ($pref.ExclusionPath) {{ foreach ($p in $pref.ExclusionPath) {{ $result += ""{pathPrefix}$p"" }} }}
if ($pref.ExclusionExtension) {{ foreach ($e in $pref.ExclusionExtension) {{ $result += ""{extPrefix}$e"" }} }}
$result -join [Environment]::NewLine
";
            var (output, error) = PowerShellHelper.Execute(script);
            if (!string.IsNullOrWhiteSpace(error) && !error.Contains("Es wurde kein solcher Parameter gefunden", StringComparison.OrdinalIgnoreCase))
            {
                // Ignoriere harmlose Warnings, werfe Fehler bei echtem Problem
                // PowerShell schreibt oft "gelbe" Warnings in StdErr, bei echten Fehlern meist mehr Details.
                if (error.ToLower().Contains("error") || error.ToLower().Contains("ausnahme") || error.ToLower().Contains("verweigert"))
                {
                    throw new Exception(Strings.Get("ErrorFetchExclusions", error));
                }
            }

            return output.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries)
                         .Where(line => !string.IsNullOrWhiteSpace(line))
                         .Select(line => line.Trim())
                         .ToList();
        }

        public static void AddExclusions(string folderPath, IEnumerable<string> extensions)
        {
            var exts = NormalizeExtensions(extensions);

            if (string.IsNullOrWhiteSpace(folderPath) && !exts.Any())
            {
                throw new ArgumentException(Strings.Get("ErrorNoInput"));
            }

            if (!string.IsNullOrWhiteSpace(folderPath))
            {
                try
                {
                    if (!Path.IsPathRooted(folderPath))
                    {
                        throw new ArgumentException(Strings.Get("ErrorInvalidPath", folderPath));
                    }

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    throw new Exception(Strings.Get("ErrorAccessDenied", folderPath));
                }
                catch (Exception ex) when (!(ex is ArgumentException))
                {
                    throw new Exception(Strings.Get("ErrorCreateFolder", ex.Message));
                }

                var script = $"Add-MpPreference -ExclusionPath '{folderPath.Replace("'", "''")}'";
                var (output, error) = PowerShellHelper.Execute(script);
                if (!string.IsNullOrWhiteSpace(error) && error.ToLower().Contains("error"))
                {
                    throw new Exception(Strings.Get("ErrorAction", Strings.Get("ActionAdd"), error));
                }
            }

            if (exts.Any())
            {
                var extString = string.Join(",", exts.Select(e => $"'{e.Replace("'", "''")}'"));
                var script = $"Add-MpPreference -ExclusionExtension {extString}";
                var (output, error) = PowerShellHelper.Execute(script);
                if (!string.IsNullOrWhiteSpace(error) && error.ToLower().Contains("error"))
                {
                    throw new Exception(Strings.Get("ErrorAction", Strings.Get("ActionAdd"), error));
                }
            }
        }

        public static void RemoveExclusions(IEnumerable<string> folderPaths, IEnumerable<string> extensions)
        {
            if (folderPaths != null && folderPaths.Any())
            {
                var pathsString = string.Join(",", folderPaths.Select(p => $"'{p.Replace("'", "''")}'"));
                var script = $"Remove-MpPreference -ExclusionPath {pathsString} -ErrorAction SilentlyContinue";
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
