namespace AmazonDeliveryPlanner
{
    partial class DriverUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.locationLabel = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.openAddressGoogleMapsButton = new System.Windows.Forms.Button();
            this.addBrowserTabButton = new System.Windows.Forms.Button();
            this.fileDownloadedLabel = new System.Windows.Forms.Label();
            this.autoDownloadStatusLabel = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 37;
            this.label1.Text = "Location:";
            this.label1.Visible = false;
            // 
            // locationLabel
            // 
            this.locationLabel.AutoSize = true;
            this.locationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.locationLabel.Location = new System.Drawing.Point(89, 8);
            this.locationLabel.Name = "locationLabel";
            this.locationLabel.Size = new System.Drawing.Size(17, 18);
            this.locationLabel.TabIndex = 41;
            this.locationLabel.Text = "_";
            this.locationLabel.Visible = false;
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeButton.Location = new System.Drawing.Point(1140, 3);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(56, 23);
            this.closeButton.TabIndex = 57;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // openAddressGoogleMapsButton
            // 
            this.openAddressGoogleMapsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.openAddressGoogleMapsButton.Location = new System.Drawing.Point(899, 3);
            this.openAddressGoogleMapsButton.Name = "openAddressGoogleMapsButton";
            this.openAddressGoogleMapsButton.Size = new System.Drawing.Size(96, 23);
            this.openAddressGoogleMapsButton.TabIndex = 65;
            this.openAddressGoogleMapsButton.Text = "Open in G. Maps";
            this.openAddressGoogleMapsButton.UseVisualStyleBackColor = true;
            this.openAddressGoogleMapsButton.Click += new System.EventHandler(this.openAddressGoogleMapsButton_Click);
            // 
            // addBrowserTabButton
            // 
            this.addBrowserTabButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addBrowserTabButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addBrowserTabButton.Location = new System.Drawing.Point(1001, 3);
            this.addBrowserTabButton.Name = "addBrowserTabButton";
            this.addBrowserTabButton.Size = new System.Drawing.Size(96, 23);
            this.addBrowserTabButton.TabIndex = 66;
            this.addBrowserTabButton.Text = "Add new page";
            this.addBrowserTabButton.UseVisualStyleBackColor = true;
            this.addBrowserTabButton.Click += new System.EventHandler(this.addBrowserTabButton_Click);
            // 
            // fileDownloadedLabel
            // 
            this.fileDownloadedLabel.AutoSize = true;
            this.fileDownloadedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileDownloadedLabel.Location = new System.Drawing.Point(384, 4);
            this.fileDownloadedLabel.Name = "fileDownloadedLabel";
            this.fileDownloadedLabel.Size = new System.Drawing.Size(17, 18);
            this.fileDownloadedLabel.TabIndex = 67;
            this.fileDownloadedLabel.Text = "_";
            // 
            // autoDownloadStatusLabel
            // 
            this.autoDownloadStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.autoDownloadStatusLabel.AutoSize = true;
            this.autoDownloadStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoDownloadStatusLabel.Location = new System.Drawing.Point(565, 8);
            this.autoDownloadStatusLabel.Name = "autoDownloadStatusLabel";
            this.autoDownloadStatusLabel.Size = new System.Drawing.Size(13, 13);
            this.autoDownloadStatusLabel.TabIndex = 68;
            this.autoDownloadStatusLabel.Text = "_";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 29);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Size = new System.Drawing.Size(1193, 725);
            this.splitContainer1.SplitterDistance = 893;
            this.splitContainer1.TabIndex = 69;
            // 
            // DriverUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.autoDownloadStatusLabel);
            this.Controls.Add(this.fileDownloadedLabel);
            this.Controls.Add(this.addBrowserTabButton);
            this.Controls.Add(this.openAddressGoogleMapsButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.locationLabel);
            this.Controls.Add(this.label1);
            this.Name = "DriverUserControl";
            this.Size = new System.Drawing.Size(1199, 754);
            this.Load += new System.EventHandler(this.DriverUserControl_Load);
            this.Resize += new System.EventHandler(this.DriverUserControl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label locationLabel;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button openAddressGoogleMapsButton;
        private System.Windows.Forms.Button addBrowserTabButton;
        private System.Windows.Forms.Label fileDownloadedLabel;
        private System.Windows.Forms.Label autoDownloadStatusLabel;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}
