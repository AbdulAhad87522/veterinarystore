using fertilizesop.BL.Models;
using fertilizesop.Interfaces.BLInterfaces;
using KIMS;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace fertilizesop.UI
{
    public partial class Addbatchform : Form
    {
        private readonly IBatchesBl ibl;

        public Addbatchform(IBatchesBl ibl)
        {
            InitializeComponent();
            this.ibl = ibl;
            this.KeyPreview = true;
            this.KeyDown += Addbatchform_KeyDown;
        }

        private void txtBname_TextUpdate(object sender, EventArgs e)
        {
            string searchText = comboBox1.Text.Trim();

            var filteredBatches = ibl.getsuppliernames(searchText);

            if (filteredBatches != null)
            {
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(filteredBatches.ToArray());
                comboBox1.SelectionStart = searchText.Length;
                comboBox1.SelectionLength = 0;
                comboBox1.DroppedDown = true;
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            string name = txtname.Text.Trim();
            DateTime date = dateTimePicker1.Value;
            string supplier_name = comboBox1.Text.Trim();

            // ✅ Input Validation
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter batch name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtname.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(supplier_name))
            {
                MessageBox.Show("Please enter supplier name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox1.Focus();
                return;
            }

            try
            {
                var batch = new Batches(0, name, date, supplier_name, "",0);

                var result = ibl.addbatches(batch);

                if (result)
                {
                    MessageBox.Show("Batch added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close(); // Optional: close form after success
                }
                else
                {
                    MessageBox.Show("Failed to add batch. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("Missing required data: " + ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Invalid input: " + ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Optional: Ctrl + S to Save
        private void Addbatchform_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                btnsave.PerformClick();
                e.Handled = true;
            }
        }

        private void Addbatchform_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value=DateTime.Now;
        }

        private void iconPictureBox2_Click(object sender, EventArgs e)
        {
            var f=Program.ServiceProvider.GetRequiredService<Addsupplier>();
            f.ShowDialog(this);
        }
    }
}
