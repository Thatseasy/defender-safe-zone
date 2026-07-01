using System;
using System.Diagnostics;
using System.Security.Principal;
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

            if (!IsAdministrator())
            {
                RunAsAdministrator();
                return;
            }

            Application.Run(new MainForm());
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
