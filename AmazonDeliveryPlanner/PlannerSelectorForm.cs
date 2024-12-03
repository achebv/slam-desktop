using AmazonDeliveryPlanner.API.data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace AmazonDeliveryPlanner
{
    public partial class PlannerSelectorForm : Form
    {
        PlannerEntity[] planners;
        IEnumerable<PlannerEntity> filteredPlanners;
        PlannerEntity selectedPlanner;
        bool displayOnlyPlannerGroupName;

        public PlannerEntity[] Planners { get => planners; /*set => planners = value;*/ }
        public PlannerEntity SelectedPlanner { get => selectedPlanner; set => selectedPlanner = value; }
        public bool DisplayOnlyPlannerGroupName { get => displayOnlyPlannerGroupName; set => displayOnlyPlannerGroupName = value; }

        public PlannerSelectorForm(PlannerEntity[] planners)
        {
            InitializeComponent();

            this.planners = planners;
            this.filteredPlanners = planners.ToArray();
            // this.planners.CopyTo(this.filteredDrivers, 0);

            PlannerEntity._ListModeToString = true;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            OpenPlanner();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void EditPlanningNotesForm_Load(object sender, EventArgs e)
        {
            displayOnlyPlannerGroupNameCheckBox.Checked = displayOnlyPlannerGroupName;

            this.ActiveControl = filterDriversTextBox;
            filterDriversTextBox.Focus();

            RefreshFilteredDriverList();
        }

        //void SelectDriver()
        //{            
        //    this.DialogResult = DialogResult.OK;
        //}

        private void OpenDriverForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        private void filteredDriversListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            staffPasswordField.Enabled = filteredDriversListBox.SelectedIndex >= 0;
            staffPasswordField.Focus();
        }

        PlannerEntity GetSelectedPlanner()
        {
            if (filteredDriversListBox.SelectedIndex < 0)
                return null;
            else
                return (PlannerEntity)filteredDriversListBox.SelectedItem;
        }

        private void filteredDriversListBox_Click(object sender, EventArgs e)
        {

        }

        private void filteredDriversListBox_DoubleClick(object sender, EventArgs e)
        {
            OpenPlanner();
        }

        private void OpenPlanner()
        {
            PlannerEntity selectedPlanner = GetSelectedPlanner();

            string userPass = GetSHA1HashData(staffPasswordField.Text);
            //userPass = GetSHA1HashData("test1234");

            if (selectedPlanner.password != userPass)
            {
                MessageBox.Show("Wrong password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (selectedPlanner != null)
            {
                this.selectedPlanner = selectedPlanner;

                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void filterDriversTextBox_TextChanged(object sender, EventArgs e)
        {
            filteredPlanners = planners.Where(dr => (dr.last_name + " " + dr.first_name + " " + dr.role_name + " " + dr.email).IndexOf(filterDriversTextBox.Text, StringComparison.CurrentCultureIgnoreCase) >= 0);

            RefreshFilteredDriverList();
        }

        void RefreshFilteredDriverList()
        {
            this.filteredDriversListBox.Items.Clear();
            this.filteredDriversListBox.Items.AddRange(filteredPlanners.ToArray());
        }

        private void OpenDriverForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (!string.IsNullOrWhiteSpace(filterDriversTextBox.Text))
                    filterDriversTextBox.Text = "";
                else
                    Close();
            }

            if (filteredDriversListBox.Items.Count > 0 && this.ActiveControl != filteredDriversListBox)
            {
                if (e.KeyCode == Keys.Down)
                {
                    this.ActiveControl = filteredDriversListBox;
                    filteredDriversListBox.Focus();

                    filteredDriversListBox.SelectedIndex = 0;
                }
                else
                if (e.KeyCode == Keys.Up)
                {
                    this.ActiveControl = filteredDriversListBox;
                    filteredDriversListBox.Focus();

                    filteredDriversListBox.SelectedIndex = filteredDriversListBox.Items.Count - 1;
                }
            }

            if (e.KeyCode == Keys.Enter)
                OpenPlanner();
            // Save();
        }

        private void OpenDriverForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            PlannerEntity._ListModeToString = false;
        }

        private void displayOnlyDriverGroupNameCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            PlannerEntity._ListModeToString = !displayOnlyPlannerGroupNameCheckBox.Checked;
            displayOnlyPlannerGroupName = displayOnlyPlannerGroupNameCheckBox.Checked;
            RefreshFilteredDriverList();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            saveButton.Enabled = staffPasswordField.Text.Length > 0;
        }

        private string GetSHA1HashData(string input)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }

    }
}
