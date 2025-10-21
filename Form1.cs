using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtentionFinder
{
    public partial class Form1 : Form
    {
        private FolderBrowserDialog folderDialog;

        public Form1()
        {
            InitializeComponent();

            folderDialog = new FolderBrowserDialog();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                tbxFolder.Text = folderDialog.SelectedPath;
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            string directoryPath = tbxFolder.Text.Trim();
            if (string.IsNullOrEmpty(directoryPath) || !Directory.Exists(directoryPath))
            {
                MessageBox.Show("Please select a valid directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lbxResults.Items.Clear();
            try
            {
                var allFiles = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);

                var extensionCounts = allFiles
                    .Select(file => Path.GetExtension(file)?.ToLowerInvariant() ?? "")
                    .GroupBy(ext => ext)
                    .ToDictionary(g => g.Key, g => g.Count());

                lbxResults.Items.Add($"Total files: {allFiles.Length}");
                lbxResults.Items.Add("-----------------------------");

                foreach (var kvp in extensionCounts.OrderByDescending(a => a.Value))
                {
                    string extDisplay = string.IsNullOrWhiteSpace(kvp.Key) ? "[no extension]" : kvp.Key;
                    lbxResults.Items.Add($"{extDisplay}: {kvp.Value}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
