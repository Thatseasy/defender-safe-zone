using System;
using System.Diagnostics;
using System.Text;

namespace DefenderSafeZoneTool
{
    public static class PowerShellHelper
    {
        public static (string output, string error) Execute(string script)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = "-NoProfile -NonInteractive -ExecutionPolicy Bypass -Command -",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            using var process = Process.Start(startInfo);
            if (process == null)
            {
                throw new InvalidOperationException("Konnte PowerShell nicht starten.");
            }

            process.StandardInput.WriteLine(script);
            process.StandardInput.Close();

            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();

            process.OutputDataReceived += (s, e) => { if (e.Data != null) outputBuilder.AppendLine(e.Data); };
            process.ErrorDataReceived += (s, e) => { if (e.Data != null) errorBuilder.AppendLine(e.Data); };

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            bool exited = process.WaitForExit(30000); // 30 Sekunden Timeout
            if (!exited)
            {
                try { process.Kill(); } catch { /* Ignore if it already exited */ }
                throw new TimeoutException("Die Ausführung des PowerShell-Befehls hat das Zeitlimit von 30 Sekunden überschritten (Timeout).");
            }

            return (outputBuilder.ToString().Trim(), errorBuilder.ToString().Trim());
        }
    }
}
