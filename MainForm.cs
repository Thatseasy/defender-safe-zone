using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DefenderSafeZoneTool
{
    public partial class MainForm : Form
    {
        private TextBox txtFolderPath;
        private Button btnBrowse;
        private TextBox txtExtensions;
        private Button btnAdd;
        private Button btnRemove;
        private Button btnRefresh;
        private Button btnOpenSecurity;
        private ListBox listExclusions;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private Label lblInfo;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Defender SafeZone Tool";
            this.Size = new Size(700, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.MinimumSize = new Size(600, 500);

            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(10)
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Eingabe
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Buttons
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f)); // Liste
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Info

            // 1. Eingabebereich
            var inputPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 3,
                RowCount = 2,
            };
            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            inputPanel.Controls.Add(new Label { Text = "Projektordner:", Anchor = AnchorStyles.Left | AnchorStyles.Top, AutoSize = true, Margin = new Padding(0, 7, 5, 0) }, 0, 0);
            txtFolderPath = new TextBox { Dock = DockStyle.Fill, Text = @"C:\Work\SafeZone" };
            inputPanel.Controls.Add(txtFolderPath, 1, 0);
            btnBrowse = new Button { Text = "Ordner wählen...", AutoSize = true };
            btnBrowse.Click += BtnBrowse_Click;
            inputPanel.Controls.Add(btnBrowse, 2, 0);

            inputPanel.Controls.Add(new Label { Text = "Erweiterungen:", Anchor = AnchorStyles.Left | AnchorStyles.Top, AutoSize = true, Margin = new Padding(0, 7, 5, 0) }, 0, 1);
            txtExtensions = new TextBox { Dock = DockStyle.Fill, Text = ".zip, .rar, .7z, .dll, .exe" };
            inputPanel.Controls.Add(txtExtensions, 1, 1);

            mainLayout.Controls.Add(inputPanel, 0, 0);

            // 2. Buttons
            var btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0, 10, 0, 10)
            };

            btnAdd = new Button { Text = "Ausschlüsse hinzufügen", AutoSize = true, Height = 30, BackColor = Color.LightGreen };
            btnAdd.Click += BtnAdd_Click;
            btnRemove = new Button { Text = "Ausschlüsse entfernen", AutoSize = true, Height = 30, BackColor = Color.LightSalmon };
            btnRemove.Click += BtnRemove_Click;
            btnRefresh = new Button { Text = "Aktualisieren", AutoSize = true, Height = 30 };
            btnRefresh.Click += BtnRefresh_Click;
            btnOpenSecurity = new Button { Text = "Windows-Sicherheit öffnen", AutoSize = true, Height = 30 };
            btnOpenSecurity.Click += BtnOpenSecurity_Click;

            btnPanel.Controls.AddRange(new Control[] { btnAdd, btnRemove, btnRefresh, btnOpenSecurity });
            mainLayout.Controls.Add(btnPanel, 0, 1);

            // 3. Liste
            var listGroup = new GroupBox { Text = "Aktuelle Ausschlüsse", Dock = DockStyle.Fill };
            listExclusions = new ListBox { Dock = DockStyle.Fill, IntegralHeight = false };
            listGroup.Controls.Add(listExclusions);
            mainLayout.Controls.Add(listGroup, 0, 2);

            // 4. Info
            lblInfo = new Label
            {
                Text = "HINWEISE:\n" +
                       "• Es wird empfohlen, dedizierte Projektordner zu nutzen (z.B. C:\\Work). Füge niemals ganze Laufwerke oder den Downloads-Ordner hinzu!\n" +
                       "• Bereits gelöschte oder blockierte Dateien findest du in der Windows-Sicherheit unter \"Virenschutz & Bedrohungsschutz\" > \"Schutzverlauf\". Dort können sie wiederhergestellt werden.",
                Dock = DockStyle.Fill,
                AutoSize = true,
                ForeColor = Color.DarkBlue,
                Margin = new Padding(0, 10, 0, 0)
            };
            mainLayout.Controls.Add(lblInfo, 0, 3);

            // 5. Status
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel { Text = "Bereit." };
            statusStrip.Items.Add(statusLabel);
            
            // Reihenfolge ist wichtig für DockStyle.Fill vs DockStyle.Bottom
            this.Controls.Add(mainLayout);
            this.Controls.Add(statusStrip);
            mainLayout.BringToFront();

            this.Load += MainForm_Load;
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            _ = LoadExclusionsAsync();
        }

        private void BtnBrowse_Click(object? sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
            dialog.Description = "Wähle einen Projektordner für Defender-Ausschlüsse aus:";
            dialog.UseDescriptionForTitle = true;
            
            if (!string.IsNullOrWhiteSpace(txtFolderPath.Text) && System.IO.Directory.Exists(txtFolderPath.Text))
            {
                dialog.SelectedPath = txtFolderPath.Text;
            }

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                txtFolderPath.Text = dialog.SelectedPath;
            }
        }

        private async void BtnAdd_Click(object? sender, EventArgs e)
        {
            var folder = txtFolderPath.Text;
            var exts = txtExtensions.Text.Split(',', StringSplitOptions.RemoveEmptyEntries);

            await ExecuteActionAsync("Hinzufügen", () =>
            {
                DefenderService.AddExclusions(folder, exts);
            });
            SetStatus($"Ausschlüsse erfolgreich hinzugefügt.", Color.Green);
        }

        private async void BtnRemove_Click(object? sender, EventArgs e)
        {
            var folder = txtFolderPath.Text;
            var exts = txtExtensions.Text.Split(',', StringSplitOptions.RemoveEmptyEntries);

            await ExecuteActionAsync("Entfernen", () =>
            {
                DefenderService.RemoveExclusions(folder, exts);
            });
            SetStatus($"Ausschlüsse erfolgreich entfernt.", Color.Green);
        }

        private async void BtnRefresh_Click(object? sender, EventArgs e)
        {
            await ExecuteActionAsync("Aktualisieren", () => { /* No-op, LoadExclusionsAsync is called next */ });
            SetStatus("Liste der Ausschlüsse aktualisiert.", Color.Black);
        }

        private void BtnOpenSecurity_Click(object? sender, EventArgs e)
        {
            try
            {
                DefenderService.OpenWindowsSecurity();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Öffnen der Windows-Sicherheit: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadExclusionsAsync()
        {
            try
            {
                listExclusions.Items.Clear();
                listExclusions.Items.Add("Lade Ausschlüsse...");
                var list = await Task.Run(() => DefenderService.GetExclusions());
                UpdateListUI(list);
            }
            catch (Exception ex)
            {
                listExclusions.Items.Clear();
                listExclusions.Items.Add($"Fehler beim Laden: {ex.Message}");
            }
        }

        private void UpdateListUI(List<string> list)
        {
            listExclusions.Items.Clear();
            if (list.Count == 0)
            {
                listExclusions.Items.Add("Keine Ausschlüsse gefunden.");
            }
            else
            {
                foreach (var item in list)
                {
                    listExclusions.Items.Add(item);
                }
            }
        }

        private async Task ExecuteActionAsync(string actionName, Action action)
        {
            SetControlsEnabled(false);
            statusLabel.Text = $"{actionName} läuft...";
            statusLabel.ForeColor = Color.Black;
            
            try
            {
                await Task.Run(() => action());
                await LoadExclusionsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim {actionName}:\n{ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus($"Fehler beim {actionName}.", Color.Red);
            }
            finally
            {
                SetControlsEnabled(true);
            }
        }

        private void SetControlsEnabled(bool enabled)
        {
            btnAdd.Enabled = enabled;
            btnRemove.Enabled = enabled;
            btnRefresh.Enabled = enabled;
            txtFolderPath.Enabled = enabled;
            txtExtensions.Enabled = enabled;
            btnBrowse.Enabled = enabled;
        }

        private void SetStatus(string message, Color color)
        {
            statusLabel.Text = message;
            statusLabel.ForeColor = color;
        }
    }
}
