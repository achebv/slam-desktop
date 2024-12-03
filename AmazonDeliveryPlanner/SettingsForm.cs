using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AmazonDeliveryPlanner
{
    public partial class SettingsForm : Form
    {
        List<string> urls;

        public SettingsForm()
        {
            InitializeComponent();

            // urls = new string[0];
            // GlobalContext.SerializedConfiguration.DefaultTabs.CopyTo(urls, 0);
            urls = new List<string>(GlobalContext.SerializedConfiguration.DefaultTabs);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            GlobalContext.SerializedConfiguration.DefaultTabs = urls.ToArray();
            this.DialogResult = DialogResult.OK;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            AddUrlToList();
        }

        void RefreshUrlList()
        {
            urlListBox.Items.Clear();

            // urlListBox.Items.AddRange(GlobalContext.Urls.ToArray());

            urlListBox.Items.AddRange(urls.ToArray());
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            RefreshUrlList();
        }

        void AddUrlToList()
        {
            if (!string.IsNullOrWhiteSpace(urlTextBox.Text))
            {
                // GlobalContext.Urls.Add(urlTextBox.Text);
                // GlobalContext.SerializedConfiguration.DefaultTabs = GlobalContext.SerializedConfiguration.DefaultTabs.Concat(new string[] { urlTextBox.Text }).ToArray();
                urls.Add(urlTextBox.Text);

                urlTextBox.Text = "";

                RefreshUrlList();
            }
        }

        void DeleteSelectedURL()
        {
            if (urlListBox.SelectedIndex >= 0)
            {
                // GlobalContext.Urls.RemoveAt(urlListBox.SelectedIndex);                
                // GlobalContext.SerializedConfiguration.DefaultTabs.Where(v => v != urlListBox.SelectedItem.ToString());
                urls.RemoveAt(urlListBox.SelectedIndex);

                RefreshUrlList();
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            DeleteSelectedURL();
        }
    }
}
