using System.Windows.Forms;

namespace AmazonDeliveryPlanner
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                // If the user clicks "No," cancel the form closing event
                e.Cancel = true;
            }
            // If the user clicks "Yes," the form will be closed normally without further action
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.openSettingsButton = new System.Windows.Forms.Button();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.upcomingTabPage = new System.Windows.Forms.TabPage();
            this.upcomingTabBrowserTimerExportUserControl = new AmazonDeliveryPlanner.BrowserTimerExportUserControl();
            this.IntransitTabPage = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.intransitTabBrowserTimerExportUserControl = new AmazonDeliveryPlanner.BrowserTimerExportUserControl();
            this.historyTabPage = new System.Windows.Forms.TabPage();
            this.historyTabBrowserTimerExportUserControl = new AmazonDeliveryPlanner.BrowserTimerExportUserControl();
            this.loggingTabPage = new System.Windows.Forms.TabPage();
            this.autoScrollCheckBox = new System.Windows.Forms.CheckBox();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.toggleLeftPanelVisibilityButton = new System.Windows.Forms.Button();
            this.showOpenDriverFormButton = new System.Windows.Forms.Button();
            this.plannerLabel = new System.Windows.Forms.Label();
            this.changeUserButton = new System.Windows.Forms.Button();
            this.exportFileAutoDownloadEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.reloadConfigurationButton = new System.Windows.Forms.Button();
            this.buttonToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btn_dload_History = new System.Windows.Forms.Button();
            this.btn_dload_Transit = new System.Windows.Forms.Button();
            this.btn_dload_Upcomming = new System.Windows.Forms.Button();
            this.mainTabControl.SuspendLayout();
            this.upcomingTabPage.SuspendLayout();
            this.IntransitTabPage.SuspendLayout();
            this.historyTabPage.SuspendLayout();
            this.loggingTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // openSettingsButton
            // 
            this.openSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.openSettingsButton.Location = new System.Drawing.Point(1476, 2);
            this.openSettingsButton.Margin = new System.Windows.Forms.Padding(0);
            this.openSettingsButton.Name = "openSettingsButton";
            this.openSettingsButton.Size = new System.Drawing.Size(86, 20);
            this.openSettingsButton.TabIndex = 2;
            this.openSettingsButton.Text = "Configuratie";
            this.buttonToolTip.SetToolTip(this.openSettingsButton, "Settings");
            this.openSettingsButton.UseVisualStyleBackColor = true;
            this.openSettingsButton.Click += new System.EventHandler(this.openSettingsButton_Click);
            // 
            // mainTabControl
            // 
            this.mainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainTabControl.Controls.Add(this.upcomingTabPage);
            this.mainTabControl.Controls.Add(this.IntransitTabPage);
            this.mainTabControl.Controls.Add(this.historyTabPage);
            this.mainTabControl.Controls.Add(this.loggingTabPage);
            this.mainTabControl.Location = new System.Drawing.Point(2, 4);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(1565, 802);
            this.mainTabControl.TabIndex = 5;
            // 
            // upcomingTabPage
            // 
            this.upcomingTabPage.Controls.Add(this.upcomingTabBrowserTimerExportUserControl);
            this.upcomingTabPage.Location = new System.Drawing.Point(4, 22);
            this.upcomingTabPage.Name = "upcomingTabPage";
            this.upcomingTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.upcomingTabPage.Size = new System.Drawing.Size(1557, 776);
            this.upcomingTabPage.TabIndex = 3;
            this.upcomingTabPage.Text = "Upcoming";
            this.upcomingTabPage.UseVisualStyleBackColor = true;
            // 
            // upcomingTabBrowserTimerExportUserControl
            // 
            this.upcomingTabBrowserTimerExportUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.upcomingTabBrowserTimerExportUserControl.ExportFileAutoDownloadEnabled = false;
            this.upcomingTabBrowserTimerExportUserControl.Location = new System.Drawing.Point(3, 3);
            this.upcomingTabBrowserTimerExportUserControl.MaxRandomIntervalMinutes = 0;
            this.upcomingTabBrowserTimerExportUserControl.MinRandomIntervalMinutes = 0;
            this.upcomingTabBrowserTimerExportUserControl.Name = "upcomingTabBrowserTimerExportUserControl";
            this.upcomingTabBrowserTimerExportUserControl.Size = new System.Drawing.Size(1551, 770);
            this.upcomingTabBrowserTimerExportUserControl.TabIndex = 0;
            // 
            // IntransitTabPage
            // 
            this.IntransitTabPage.Controls.Add(this.intransitTabBrowserTimerExportUserControl);
            this.IntransitTabPage.Location = new System.Drawing.Point(4, 22);
            this.IntransitTabPage.Name = "IntransitTabPage";
            this.IntransitTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.IntransitTabPage.Size = new System.Drawing.Size(1557, 776);
            this.IntransitTabPage.TabIndex = 4;
            this.IntransitTabPage.Text = "In-transit";
            this.IntransitTabPage.UseVisualStyleBackColor = true;
            
            // 
            // intransitTabBrowserTimerExportUserControl
            // 
            this.intransitTabBrowserTimerExportUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.intransitTabBrowserTimerExportUserControl.ExportFileAutoDownloadEnabled = false;
            this.intransitTabBrowserTimerExportUserControl.Location = new System.Drawing.Point(3, 3);
            this.intransitTabBrowserTimerExportUserControl.MaxRandomIntervalMinutes = 0;
            this.intransitTabBrowserTimerExportUserControl.MinRandomIntervalMinutes = 0;
            this.intransitTabBrowserTimerExportUserControl.Name = "intransitTabBrowserTimerExportUserControl";
            this.intransitTabBrowserTimerExportUserControl.Size = new System.Drawing.Size(1551, 770);
            this.intransitTabBrowserTimerExportUserControl.TabIndex = 1;
            // 
            // historyTabPage
            // 
            this.historyTabPage.Controls.Add(this.historyTabBrowserTimerExportUserControl);
            this.historyTabPage.Location = new System.Drawing.Point(4, 22);
            this.historyTabPage.Name = "historyTabPage";
            this.historyTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.historyTabPage.Size = new System.Drawing.Size(1557, 776);
            this.historyTabPage.TabIndex = 5;
            this.historyTabPage.Text = "History";
            this.historyTabPage.UseVisualStyleBackColor = true;
            // 
            // historyTabBrowserTimerExportUserControl
            // 
            this.historyTabBrowserTimerExportUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.historyTabBrowserTimerExportUserControl.ExportFileAutoDownloadEnabled = false;
            this.historyTabBrowserTimerExportUserControl.Location = new System.Drawing.Point(3, 3);
            this.historyTabBrowserTimerExportUserControl.MaxRandomIntervalMinutes = 0;
            this.historyTabBrowserTimerExportUserControl.MinRandomIntervalMinutes = 0;
            this.historyTabBrowserTimerExportUserControl.Name = "historyTabBrowserTimerExportUserControl";
            this.historyTabBrowserTimerExportUserControl.Size = new System.Drawing.Size(1551, 770);
            this.historyTabBrowserTimerExportUserControl.TabIndex = 1;
            // 
            // loggingTabPage
            // 
            this.loggingTabPage.Controls.Add(this.autoScrollCheckBox);
            this.loggingTabPage.Controls.Add(this.logTextBox);
            this.loggingTabPage.Location = new System.Drawing.Point(4, 22);
            this.loggingTabPage.Name = "loggingTabPage";
            this.loggingTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.loggingTabPage.Size = new System.Drawing.Size(1557, 776);
            this.loggingTabPage.TabIndex = 2;
            this.loggingTabPage.Text = "Log";
            this.loggingTabPage.UseVisualStyleBackColor = true;
            // 
            // autoScrollCheckBox
            // 
            this.autoScrollCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.autoScrollCheckBox.AutoSize = true;
            this.autoScrollCheckBox.Location = new System.Drawing.Point(1458, 756);
            this.autoScrollCheckBox.Name = "autoScrollCheckBox";
            this.autoScrollCheckBox.Size = new System.Drawing.Size(75, 17);
            this.autoScrollCheckBox.TabIndex = 1;
            this.autoScrollCheckBox.Text = "Auto scroll";
            this.autoScrollCheckBox.UseVisualStyleBackColor = true;
            // 
            // logTextBox
            // 
            this.logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextBox.Location = new System.Drawing.Point(3, 3);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox.Size = new System.Drawing.Size(1551, 773);
            this.logTextBox.TabIndex = 0;
            // 
            // toggleLeftPanelVisibilityButton
            // 
            this.toggleLeftPanelVisibilityButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toggleLeftPanelVisibilityButton.Location = new System.Drawing.Point(1430, 2);
            this.toggleLeftPanelVisibilityButton.Margin = new System.Windows.Forms.Padding(0);
            this.toggleLeftPanelVisibilityButton.Name = "toggleLeftPanelVisibilityButton";
            this.toggleLeftPanelVisibilityButton.Size = new System.Drawing.Size(42, 20);
            this.toggleLeftPanelVisibilityButton.TabIndex = 6;
            this.toggleLeftPanelVisibilityButton.Text = "| █";
            this.buttonToolTip.SetToolTip(this.toggleLeftPanelVisibilityButton, "Toggle drivers panel on/off");
            this.toggleLeftPanelVisibilityButton.UseVisualStyleBackColor = true;
            // 
            // showOpenDriverFormButton
            // 
            this.showOpenDriverFormButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.showOpenDriverFormButton.Location = new System.Drawing.Point(1380, 2);
            this.showOpenDriverFormButton.Margin = new System.Windows.Forms.Padding(0);
            this.showOpenDriverFormButton.Name = "showOpenDriverFormButton";
            this.showOpenDriverFormButton.Size = new System.Drawing.Size(42, 20);
            this.showOpenDriverFormButton.TabIndex = 7;
            this.showOpenDriverFormButton.Text = "🔎";
            this.buttonToolTip.SetToolTip(this.showOpenDriverFormButton, "Search for driver");
            this.showOpenDriverFormButton.UseVisualStyleBackColor = true;
            // 
            // plannerLabel
            // 
            this.plannerLabel.Location = new System.Drawing.Point(1140, 4);
            this.plannerLabel.Name = "plannerLabel";
            this.plannerLabel.Size = new System.Drawing.Size(192, 17);
            this.plannerLabel.TabIndex = 6;
            this.plannerLabel.Text = "_____ logged in planner ____";
            // 
            // changeUserButton
            // 
            this.changeUserButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.changeUserButton.Location = new System.Drawing.Point(1331, 2);
            this.changeUserButton.Margin = new System.Windows.Forms.Padding(0);
            this.changeUserButton.Name = "changeUserButton";
            this.changeUserButton.Size = new System.Drawing.Size(42, 20);
            this.changeUserButton.TabIndex = 8;
            this.changeUserButton.Text = "👤";
            this.buttonToolTip.SetToolTip(this.changeUserButton, "Change user");
            this.changeUserButton.UseVisualStyleBackColor = true;
            this.changeUserButton.Visible = false;
            // 
            // exportFileAutoDownloadEnabledCheckBox
            // 
            this.exportFileAutoDownloadEnabledCheckBox.AutoSize = true;
            this.exportFileAutoDownloadEnabledCheckBox.Location = new System.Drawing.Point(954, 3);
            this.exportFileAutoDownloadEnabledCheckBox.Name = "exportFileAutoDownloadEnabledCheckBox";
            this.exportFileAutoDownloadEnabledCheckBox.Size = new System.Drawing.Size(102, 17);
            this.exportFileAutoDownloadEnabledCheckBox.TabIndex = 9;
            this.exportFileAutoDownloadEnabledCheckBox.Text = "AMZ Trips Sync";
            this.buttonToolTip.SetToolTip(this.exportFileAutoDownloadEnabledCheckBox, "Auto download on/off for exported file");
            this.exportFileAutoDownloadEnabledCheckBox.UseVisualStyleBackColor = true;
            this.exportFileAutoDownloadEnabledCheckBox.CheckedChanged += new System.EventHandler(this.exportFileAutoDownloadEnabledCheckBox_CheckedChanged);
            // 
            // reloadConfigurationButton
            // 
            this.reloadConfigurationButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.reloadConfigurationButton.Location = new System.Drawing.Point(1092, 1);
            this.reloadConfigurationButton.Margin = new System.Windows.Forms.Padding(0);
            this.reloadConfigurationButton.Name = "reloadConfigurationButton";
            this.reloadConfigurationButton.Size = new System.Drawing.Size(42, 20);
            this.reloadConfigurationButton.TabIndex = 10;
            this.reloadConfigurationButton.Text = "⟳⚙";
            this.buttonToolTip.SetToolTip(this.reloadConfigurationButton, "Reload configuration from server");
            this.reloadConfigurationButton.UseVisualStyleBackColor = true;
            this.reloadConfigurationButton.Click += new System.EventHandler(this.reloadConfigurationButton_Click);
            // 
            // buttonToolTip
            // 
            this.buttonToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // btn_dload_History
            // 
            this.btn_dload_History.Location = new System.Drawing.Point(806, 2);
            this.btn_dload_History.Name = "btn_dload_History";
            this.btn_dload_History.Size = new System.Drawing.Size(21, 23);
            this.btn_dload_History.TabIndex = 11;
            this.btn_dload_History.Text = "H";
            this.buttonToolTip.SetToolTip(this.btn_dload_History, "Download History CSV");
            this.btn_dload_History.UseVisualStyleBackColor = true;
            this.btn_dload_History.Click += new System.EventHandler(this.btn_dload_History_Click);
            // 
            // btn_dload_Transit
            // 
            this.btn_dload_Transit.Location = new System.Drawing.Point(833, 2);
            this.btn_dload_Transit.Name = "btn_dload_Transit";
            this.btn_dload_Transit.Size = new System.Drawing.Size(21, 23);
            this.btn_dload_Transit.TabIndex = 12;
            this.btn_dload_Transit.Text = "T";
            this.buttonToolTip.SetToolTip(this.btn_dload_Transit, "Download Transit CSV");
            this.btn_dload_Transit.UseVisualStyleBackColor = true;
            this.btn_dload_Transit.Click += new System.EventHandler(this.btn_dload_Transit_Click);
            // 
            // btn_dload_Upcomming
            // 
            this.btn_dload_Upcomming.Location = new System.Drawing.Point(860, 2);
            this.btn_dload_Upcomming.Name = "btn_dload_Upcomming";
            this.btn_dload_Upcomming.Size = new System.Drawing.Size(21, 23);
            this.btn_dload_Upcomming.TabIndex = 13;
            this.btn_dload_Upcomming.Text = "U";
            this.buttonToolTip.SetToolTip(this.btn_dload_Upcomming, "Download Upcomming  CSV");
            this.btn_dload_Upcomming.UseVisualStyleBackColor = true;
            this.btn_dload_Upcomming.Click += new System.EventHandler(this.btn_dload_Upcomming_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1565, 807);
            this.Controls.Add(this.btn_dload_Upcomming);
            this.Controls.Add(this.btn_dload_Transit);
            this.Controls.Add(this.btn_dload_History);
            this.Controls.Add(this.reloadConfigurationButton);
            this.Controls.Add(this.exportFileAutoDownloadEnabledCheckBox);
            this.Controls.Add(this.changeUserButton);
            this.Controls.Add(this.plannerLabel);
            this.Controls.Add(this.showOpenDriverFormButton);
            this.Controls.Add(this.toggleLeftPanelVisibilityButton);
            this.Controls.Add(this.openSettingsButton);
            this.Controls.Add(this.mainTabControl);
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainTabControl.ResumeLayout(false);
            this.upcomingTabPage.ResumeLayout(false);
            this.IntransitTabPage.ResumeLayout(false);
            this.historyTabPage.ResumeLayout(false);
            this.loggingTabPage.ResumeLayout(false);
            this.loggingTabPage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button openSettingsButton;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.Button toggleLeftPanelVisibilityButton;
        private System.Windows.Forms.TabPage loggingTabPage;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.CheckBox autoScrollCheckBox;
        private System.Windows.Forms.Button showOpenDriverFormButton;
        private System.Windows.Forms.Label plannerLabel;
        private System.Windows.Forms.Button changeUserButton;
        private TabPage upcomingTabPage;
        private TabPage IntransitTabPage;
        private TabPage historyTabPage;
        private BrowserTimerExportUserControl upcomingTabBrowserTimerExportUserControl;
        private BrowserTimerExportUserControl intransitTabBrowserTimerExportUserControl;
        private BrowserTimerExportUserControl historyTabBrowserTimerExportUserControl;
        private CheckBox exportFileAutoDownloadEnabledCheckBox;
        private Button reloadConfigurationButton;
        private ToolTip buttonToolTip;
        private Button button1;
        private Button btn_dload_History;
        private Button btn_dload_Transit;
        private Button btn_dload_Upcomming;
    }
}