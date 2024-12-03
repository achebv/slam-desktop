namespace AmazonDeliveryPlanner
{
    partial class PlannerSelectorForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.filterDriversTextBox = new System.Windows.Forms.TextBox();
            this.filteredDriversListBox = new System.Windows.Forms.ListBox();
            this.displayOnlyPlannerGroupNameCheckBox = new System.Windows.Forms.CheckBox();
            this.staffPasswordField = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Filter:";
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveButton.Enabled = false;
            this.saveButton.Location = new System.Drawing.Point(198, 419);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 2;
            this.saveButton.Text = "&Log in";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(279, 419);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "&Exit";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(233, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Select your account and then fill your password.";
            // 
            // filterDriversTextBox
            // 
            this.filterDriversTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterDriversTextBox.Location = new System.Drawing.Point(16, 29);
            this.filterDriversTextBox.Name = "filterDriversTextBox";
            this.filterDriversTextBox.Size = new System.Drawing.Size(537, 20);
            this.filterDriversTextBox.TabIndex = 5;
            this.filterDriversTextBox.TextChanged += new System.EventHandler(this.filterDriversTextBox_TextChanged);
            // 
            // filteredDriversListBox
            // 
            this.filteredDriversListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filteredDriversListBox.FormattingEnabled = true;
            this.filteredDriversListBox.Location = new System.Drawing.Point(16, 72);
            this.filteredDriversListBox.Name = "filteredDriversListBox";
            this.filteredDriversListBox.Size = new System.Drawing.Size(537, 316);
            this.filteredDriversListBox.TabIndex = 6;
            this.filteredDriversListBox.Click += new System.EventHandler(this.filteredDriversListBox_Click);
            this.filteredDriversListBox.SelectedIndexChanged += new System.EventHandler(this.filteredDriversListBox_SelectedIndexChanged);
            this.filteredDriversListBox.DoubleClick += new System.EventHandler(this.filteredDriversListBox_DoubleClick);
            // 
            // displayOnlyPlannerGroupNameCheckBox
            // 
            this.displayOnlyPlannerGroupNameCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.displayOnlyPlannerGroupNameCheckBox.AutoSize = true;
            this.displayOnlyPlannerGroupNameCheckBox.Location = new System.Drawing.Point(447, 425);
            this.displayOnlyPlannerGroupNameCheckBox.Name = "displayOnlyPlannerGroupNameCheckBox";
            this.displayOnlyPlannerGroupNameCheckBox.Size = new System.Drawing.Size(106, 17);
            this.displayOnlyPlannerGroupNameCheckBox.TabIndex = 7;
            this.displayOnlyPlannerGroupNameCheckBox.Text = "Only group name";
            this.displayOnlyPlannerGroupNameCheckBox.UseVisualStyleBackColor = true;
            this.displayOnlyPlannerGroupNameCheckBox.Visible = false;
            this.displayOnlyPlannerGroupNameCheckBox.CheckedChanged += new System.EventHandler(this.displayOnlyDriverGroupNameCheckBox_CheckedChanged);
            // 
            // staffPasswordField
            // 
            this.staffPasswordField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.staffPasswordField.Enabled = false;
            this.staffPasswordField.Location = new System.Drawing.Point(72, 393);
            this.staffPasswordField.Name = "staffPasswordField";
            this.staffPasswordField.Size = new System.Drawing.Size(201, 20);
            this.staffPasswordField.TabIndex = 8;
            this.staffPasswordField.UseSystemPasswordChar = true;
            this.staffPasswordField.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 398);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(279, 398);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(189, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Ask your admin if you have any issues.";
            // 
            // PlannerSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(565, 456);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.staffPasswordField);
            this.Controls.Add(this.displayOnlyPlannerGroupNameCheckBox);
            this.Controls.Add(this.filteredDriversListBox);
            this.Controls.Add(this.filterDriversTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "PlannerSelectorForm";
            this.ShowInTaskbar = false;
            this.Text = "Log in planner";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OpenDriverForm_FormClosed);
            this.Load += new System.EventHandler(this.EditPlanningNotesForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OpenDriverForm_KeyUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.OpenDriverForm_PreviewKeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox filterDriversTextBox;
        private System.Windows.Forms.ListBox filteredDriversListBox;
        private System.Windows.Forms.CheckBox displayOnlyPlannerGroupNameCheckBox;
        private System.Windows.Forms.TextBox staffPasswordField;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}