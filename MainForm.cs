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
        private TextBox txtExtensions;
        private Button btnBrowse;
        private Button btnAddFolder;
        private Button btnAddExtension;
        private Button btnRemove;
        private Button btnRefresh;
        private Button btnOpenSecurity;
        private ListBox listExclusions;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private Label lblInfo;
        private ToolTip toolTipExtensions;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = Strings.Get("AppTitle");
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.MinimumSize = new Size(600, 500);

            // 1. StatusStrip
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel { Text = Strings.Get("StatusReady") };
            statusStrip.Items.Add(statusLabel);

            // 2. Info Label
            lblInfo = new Label
            {
                Text = Strings.Get("HintsText"),
                Dock = DockStyle.Bottom,
                AutoSize = true,
                MaximumSize = new Size(560, 0),
                ForeColor = Color.DarkBlue,
                Padding = new Padding(10)
            };

            // 3. Eingabebereich
            var inputPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 4,
                RowCount = 2,
                Padding = new Padding(10, 10, 10, 0)
            };
            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            inputPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            inputPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            inputPanel.Controls.Add(new Label { Text = Strings.Get("TargetFolder"), Anchor = AnchorStyles.Left, AutoSize = true, Margin = new Padding(0, 0, 5, 0) }, 0, 0);
            txtFolderPath = new TextBox { Dock = DockStyle.Fill, Text = @"C:\Users\imise\Downloads\Safezone", Margin = new Padding(0, 3, 5, 3) };
            inputPanel.Controls.Add(txtFolderPath, 1, 0);
            
            var folderButtonsPanel = new FlowLayoutPanel { AutoSize = true, WrapContents = false, Margin = new Padding(0), Padding = new Padding(0) };
            btnBrowse = new Button { Text = Strings.Get("BrowseFolder"), AutoSize = true, Margin = new Padding(0, 2, 5, 2) };
            btnBrowse.Click += BtnBrowse_Click;
            btnAddFolder = new Button { Text = Strings.Get("AddExclusion"), AutoSize = true, BackColor = Color.LightGreen, Margin = new Padding(0, 2, 0, 2) };
            btnAddFolder.Click += BtnAddFolder_Click;
            folderButtonsPanel.Controls.AddRange(new Control[] { btnBrowse, btnAddFolder });
            inputPanel.Controls.Add(folderButtonsPanel, 2, 0);
            inputPanel.SetColumnSpan(folderButtonsPanel, 2);

            inputPanel.Controls.Add(new Label { Text = Strings.Get("Extensions"), Anchor = AnchorStyles.Left, AutoSize = true, Margin = new Padding(0, 0, 5, 0) }, 0, 1);
            txtExtensions = new TextBox { Dock = DockStyle.Fill, Text = "", Margin = new Padding(0, 3, 5, 3) };
            inputPanel.Controls.Add(txtExtensions, 1, 1);

            var lblExtHelp = new Label { Text = "(?)", Anchor = AnchorStyles.Left, AutoSize = true, ForeColor = Color.Blue, Cursor = Cursors.Help, Margin = new Padding(0, 0, 5, 0) };
            toolTipExtensions = new ToolTip();
            toolTipExtensions.SetToolTip(lblExtHelp, Strings.Get("ExtensionsHelp"));
            inputPanel.Controls.Add(lblExtHelp, 2, 1);

            btnAddExtension = new Button { Text = Strings.Get("AddExclusion"), AutoSize = true, BackColor = Color.LightGreen, Margin = new Padding(0, 2, 0, 2) };
            btnAddExtension.Click += BtnAddExtension_Click;
            inputPanel.Controls.Add(btnAddExtension, 3, 1);

            // 4. Top Toolbar (Refresh, Security)
            var btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(10)
            };

            btnRefresh = new Button { Text = Strings.Get("Refresh"), AutoSize = true, Height = 30 };
            btnRefresh.Click += BtnRefresh_Click;
            btnOpenSecurity = new Button { Text = Strings.Get("OpenSecurity"), AutoSize = true, Height = 30 };
            btnOpenSecurity.Click += BtnOpenSecurity_Click;

            btnPanel.Controls.AddRange(new Control[] { btnRefresh, btnOpenSecurity });

            // 5. Liste und Entfernen
            var listGroup = new GroupBox { Text = Strings.Get("CurrentExclusions"), Dock = DockStyle.Fill, Padding = new Padding(10) };
            listExclusions = new ListBox { Dock = DockStyle.Fill, IntegralHeight = false, SelectionMode = SelectionMode.MultiExtended };
            listGroup.Controls.Add(listExclusions);
            
            var listContainer = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10, 0, 10, 0) };
            listContainer.Controls.Add(listGroup);

            var removePanel = new Panel { Dock = DockStyle.Bottom, AutoSize = true, Padding = new Padding(10, 10, 10, 10) };
            btnRemove = new Button { Text = Strings.Get("RemoveSelected"), AutoSize = true, Height = 30, BackColor = Color.LightSalmon };
            btnRemove.Click += BtnRemove_Click;
            removePanel.Controls.Add(btnRemove);

            var innerFillPanel = new Panel { Dock = DockStyle.Fill };
            innerFillPanel.Controls.Add(listContainer);
            innerFillPanel.Controls.Add(removePanel);

            // HINZUFÜGEN ZUM FORMULAR IN DER RICHTIGEN REIHENFOLGE!
            this.Controls.Add(innerFillPanel); // Fill
            this.Controls.Add(btnPanel);       // Top
            this.Controls.Add(inputPanel);     // Top
            this.Controls.Add(lblInfo);        // Bottom
            this.Controls.Add(statusStrip);    // Bottom

            this.Load += MainForm_Load;
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            _ = LoadExclusionsAsync();
        }

        private void BtnBrowse_Click(object? sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
            dialog.Description = Strings.Get("BrowseDialogDesc");
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

        private async void BtnAddFolder_Click(object? sender, EventArgs e)
        {
            var folder = txtFolderPath.Text;
            if (string.IsNullOrWhiteSpace(folder)) return;

            await ExecuteActionAsync(Strings.Get("ActionAdd"), () =>
            {
                DefenderService.AddExclusions(folder, new string[0]);
            });
            SetStatus(Strings.Get("SuccessAdd"), Color.Green);
        }

        private async void BtnAddExtension_Click(object? sender, EventArgs e)
        {
            var exts = txtExtensions.Text.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (!exts.Any()) return;

            await ExecuteActionAsync(Strings.Get("ActionAdd"), () =>
            {
                DefenderService.AddExclusions("", exts);
            });
            txtExtensions.Text = "";
            SetStatus(Strings.Get("SuccessAdd"), Color.Green);
        }

        private async void BtnRemove_Click(object? sender, EventArgs e)
        {
            var selectedItems = listExclusions.SelectedItems.Cast<string>().ToList();
            if (!selectedItems.Any())
            {
                MessageBox.Show(Strings.Get("SelectToRemove"), Strings.Get("NoSelection"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var pathsToRemove = new List<string>();
            var extsToRemove = new List<string>();

            var pathPrefix = Strings.Get("PathPrefix");
            var extPrefix = Strings.Get("ExtensionPrefix");

            foreach (var item in selectedItems)
            {
                if (item.StartsWith(pathPrefix)) pathsToRemove.Add(item.Substring(pathPrefix.Length));
                else if (item.StartsWith(extPrefix)) extsToRemove.Add(item.Substring(extPrefix.Length));
            }

            if (!pathsToRemove.Any() && !extsToRemove.Any()) return;

            await ExecuteActionAsync(Strings.Get("ActionRemove"), () =>
            {
                DefenderService.RemoveExclusions(pathsToRemove, extsToRemove);
            });
            SetStatus(Strings.Get("SuccessRemove"), Color.Green);
        }

        private async void BtnRefresh_Click(object? sender, EventArgs e)
        {
            await ExecuteActionAsync(Strings.Get("ActionRefresh"), () => { /* No-op */ });
            SetStatus(Strings.Get("ListRefreshed"), Color.Black);
        }

        private void BtnOpenSecurity_Click(object? sender, EventArgs e)
        {
            try
            {
                DefenderService.OpenWindowsSecurity();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Strings.Get("ErrorOpenSecurity", ex.Message), Strings.Get("ErrorTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadExclusionsAsync()
        {
            try
            {
                listExclusions.Items.Clear();
                listExclusions.Items.Add(Strings.Get("Loading"));
                var list = await Task.Run(() => DefenderService.GetExclusions());
                UpdateListUI(list);
            }
            catch (Exception ex)
            {
                listExclusions.Items.Clear();
                listExclusions.Items.Add(Strings.Get("ErrorLoading", ex.Message));
            }
        }

        private void UpdateListUI(List<string> list)
        {
            listExclusions.Items.Clear();
            if (list.Count == 0)
            {
                listExclusions.Items.Add(Strings.Get("NoExclusionsFound"));
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
            statusLabel.Text = Strings.Get("ActionRunning", actionName);
            statusLabel.ForeColor = Color.Black;
            
            try
            {
                await Task.Run(() => action());
                await LoadExclusionsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Strings.Get("ErrorAction", actionName, ex.Message), Strings.Get("ErrorTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus(Strings.Get("ErrorActionStatus", actionName), Color.Red);
            }
            finally
            {
                SetControlsEnabled(true);
            }
        }

        private void SetControlsEnabled(bool enabled)
        {
            btnAddFolder.Enabled = enabled;
            btnAddExtension.Enabled = enabled;
            btnRemove.Enabled = enabled;
            btnRefresh.Enabled = enabled;
            txtFolderPath.Enabled = enabled;
            txtExtensions.Enabled = enabled;
            btnBrowse.Enabled = enabled;
            listExclusions.Enabled = enabled;
        }

        private void SetStatus(string message, Color color)
        {
            statusLabel.Text = message;
            statusLabel.ForeColor = color;
        }
    }
}
