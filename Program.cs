using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

namespace DefenderSafeZoneTool
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ApplyInstallerLanguage();

            if (!IsAdministrator())
            {
                RunAsAdministrator();
                return;
            }

            Application.Run(new MainForm());
        }

        private static void ApplyInstallerLanguage()
        {
            try
            {
                string langFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lang.ini");
                if (File.Exists(langFile))
                {
                    string[] lines = File.ReadAllLines(langFile);
                    foreach (string line in lines)
                    {
                        if (line.Trim().StartsWith("Language="))
                        {
                            string langCode = line.Substring(line.IndexOf('=') + 1).Trim();
                            string cultureName = langCode switch
                            {
                                "en" => "en",
                                "de" => "de",
                                "zh" => "zh-Hans",
                                "hi" => "hi",
                                "ar" => "ar",
                                "es" => "es",
                                "fr" => "fr",
                                "bn" => "bn",
                                "pt" => "pt-BR",
                                "ru" => "ru",
                                "ur" => "ur",
                                "id" => "id",
                                "ja" => "ja",
                                "tr" => "tr",
                                "vi" => "vi",
                                "ko" => "ko",
                                "fa" => "fa",
                                _ => ""
                            };
                            
                            if (!string.IsNullOrEmpty(cultureName))
                            {
                                CultureInfo culture = new CultureInfo(cultureName);
                                Thread.CurrentThread.CurrentCulture = culture;
                                Thread.CurrentThread.CurrentUICulture = culture;
                                CultureInfo.CurrentCulture = culture;
                                CultureInfo.CurrentUICulture = culture;
                            }
                            break;
                        }
                    }
                }
            }
            catch { /* ignore, use OS default */ }
        }

        private static bool IsAdministrator()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        private static void RunAsAdministrator()
        {
            var exeName = Process.GetCurrentProcess().MainModule?.FileName;
            if (string.IsNullOrEmpty(exeName)) return;

            ProcessStartInfo startInfo = new ProcessStartInfo(exeName)
            {
                UseShellExecute = true,
                Verb = "runas"
            };

            try
            {
                Process.Start(startInfo);
            }
            catch (Exception)
            {
                MessageBox.Show("Die Anwendung benötigt Administratorrechte, um Defender-Ausschlüsse zu konfigurieren.", "Administratorrechte erforderlich", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
