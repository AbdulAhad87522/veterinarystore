using fertilizesop.BL.Bl;
using fertilizesop.BL.Models;
using fertilizesop.Interfaces.BLInterfaces;
using KIMS;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
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
using System.Xml.Linq;
namespace fertilizesop.UI
{
    public partial class Addbatchdetailsform : Form
    {
        private int selectedProductId;
        private string selectedProductName;
        private string selectedProductDescription;
        private readonly IbatchdetailsBl ibl;
        public string InitialBatchName { get; set; }

        public Addbatchdetailsform(IbatchdetailsBl ibl)
        {

            InitializeComponent();
            this.ibl = ibl;
            UIHelper.StyleGridView(dataGridView2);
            this.txtBname.TextChanged += txtBname_TextChanged;
            this.txtBname.TextUpdate += txtBname_TextUpdate;
            this.dataGridView2.CellClick += dataGridView2_CellClick;
            this.KeyPreview = true;
            this.KeyDown += Addbatchdetailsform_KeyDown;
            UIHelper.StyleGridView(dataGridView1);
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;

        }
        private void Addbatchdetailsform_KeyDown(object sender, KeyEventArgs e)
        {
            // Ctrl + S for Save
            if (e.Control && e.KeyCode == Keys.S)
            {
                btnsave.PerformClick();
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Enter && !dataGridView2.Focused && txtproducts.Focused)
            {
                AddProductToGrid();
                e.Handled = true;
            }
            // Arrow key navigation in DataGridView
            if (dataGridView2.Focused)
            {
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                {
                    int rowIndex = dataGridView2.CurrentCell.RowIndex;
                    int columnIndex = dataGridView2.CurrentCell.ColumnIndex;

                    if (e.KeyCode == Keys.Up && rowIndex > 0)
                    {
                        dataGridView2.CurrentCell = dataGridView2.Rows[rowIndex - 1].Cells[columnIndex];
                        e.Handled = true;
                    }

                    else if (e.KeyCode == Keys.Down && rowIndex < dataGridView2.Rows.Count - 2)
                    {
                        dataGridView2.CurrentCell = dataGridView2.Rows[rowIndex + 1].Cells[columnIndex];
                        e.Handled = true;
                    }
                }
            }
        }
        private string GetTempFilePath()
        {
            // ✅ Use AppData instead of Program Files
            string folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Fertilizer",
                "TempBatches"
            );

            try
            {
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating temp folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Use batch name if available, otherwise default
            string fileName = string.IsNullOrWhiteSpace(txtBname.Text)
                ? "temp.json"
                : $"{txtBname.Text.Trim()}.json";

            return Path.Combine(folder, fileName);
        }


        private void SaveTempData()
        {
            if (string.IsNullOrWhiteSpace(txtBname.Text)) return;

            var data = new List<TempBatchDetail>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                data.Add(new TempBatchDetail
                {
                    ProductId = Convert.ToInt32(row.Cells[0].Value),
                    ProductName = row.Cells[1].Value.ToString(),
                    Description = row.Cells[2].Value.ToString(),
                    Cost = Convert.ToDecimal(row.Cells[3].Value),
                    Sale = Convert.ToDecimal(row.Cells[4].Value),
                    Quantity = Convert.ToInt32(row.Cells[5].Value)
                });
            }

            File.WriteAllText(GetTempFilePath(), JsonConvert.SerializeObject(data));
        }

        private void LoadTempData()
        {
            dataGridView1.Rows.Clear();

            string path = GetTempFilePath();
            if (!File.Exists(path)) return;

            var data = JsonConvert.DeserializeObject<List<TempBatchDetail>>(File.ReadAllText(path));
            if (data != null && data.Any())
            {
                foreach (var item in data)
                {
                    dataGridView1.Rows.Add(item.ProductId, item.ProductName, item.Description, item.Cost, item.Sale, item.Quantity);
                }
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            string batchname = txtBname.Text.Trim();

            if (string.IsNullOrWhiteSpace(batchname))
            {
                MessageBox.Show("Batch name is required.", "Missing Info");
                return;
            }

            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No batch details to save.", "Empty List");
                return;
            }

            bool allSaved = true;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                int pid = Convert.ToInt32(row.Cells[0].Value);
                string pname = row.Cells[1].Value.ToString();
                string desc = row.Cells[2].Value.ToString();
                decimal cost = Convert.ToDecimal(row.Cells[3].Value);
                decimal sale = Convert.ToDecimal(row.Cells[4].Value);
                int qty = Convert.ToInt32(row.Cells[5].Value);

                var detail = new BatchDetails(0, batchname, cost, sale, pid, pname, qty);
                bool result = ibl.adddetails(detail);

                if (!result) allSaved = false;
            }

            if (allSaved)
            {
                MessageBox.Show("All batch details saved.", "Success");

                dataGridView1.Rows.Clear();

                // ✅ Delete temp file after successful save
                string tempPath = GetTempFilePath();
                if (File.Exists(tempPath))
                    File.Delete(tempPath);
            }
            else
            {
                MessageBox.Show("Some details may not have saved correctly.", "Warning");
            }
        }

        private void txtBname_TextChanged(object sender, EventArgs e)
        {
            if (!txtBname.DroppedDown)
            {
                txtBname.DroppedDown = true;
                txtBname.SelectionStart = txtBname.Text.Length;
                txtBname.SelectionLength = 0;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string text = txtproducts.Text.Trim();

            if (!string.IsNullOrEmpty(text))
            {
                var list = DatabaseHelper.Instance.GetProductsByNames(text);
                dataGridView2.DataSource = list;
                dataGridView2.Columns["Id"].Visible = false;
                dataGridView2.Columns["Price"].Visible = false;
                dataGridView2.Columns["quantity"].Visible = false;
                dataGridView2.Visible = true;
                dataGridView2.BringToFront();
            }
            else
            {
                dataGridView2.Visible = false;
            }
        }

        private void loadbatches()
        {
            var batchNames =DatabaseHelper.Instance.Getbatches("");
            if (batchNames != null && batchNames.Count > 0)
            {
                txtBname.Items.Clear();
                txtBname.Items.AddRange(batchNames.ToArray());

                var autoSource = new AutoCompleteStringCollection();
                autoSource.AddRange(batchNames.ToArray());
                txtBname.AutoCompleteCustomSource = autoSource;
                txtBname.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtBname.SelectedIndex = -1;
            }
        }
        private void Addbatchdetailsform_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(InitialBatchName))
                txtBname.Text = InitialBatchName; // ✅ Set it first

  

            // Setup columns
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("ProductId", "Product ID");
            dataGridView1.Columns.Add("ProductName", "Product Name");
            dataGridView1.Columns.Add("Description", "Description");
            dataGridView1.Columns.Add("Cost", "Cost Price");
            dataGridView1.Columns.Add("Sale", "Sale Price");
            dataGridView1.Columns.Add("Quantity", "Quantity");

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.ReadOnly = false;
            dataGridView1.Columns["ProductId"].ReadOnly = true;

            dataGridView2.Visible = false;
            LoadTempData(); // ✅ Now this will load the correct JSON

            loadbatches();
            txtproducts.Focus();
        }


        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var rows = dataGridView2.Rows[e.RowIndex];
                selectedProductId = Convert.ToInt32(rows.Cells["Id"].Value);
                selectedProductDescription = rows.Cells["Description"].Value.ToString();
                selectedProductName = rows.Cells["Name"].Value.ToString();
                txtproducts.Text = selectedProductName;
                txtSprice.Text = ibl.getsaleprice(selectedProductId).ToString();

                dataGridView2.Visible = false; // Hide after selection

                MessageBox.Show($"Selected: ID={selectedProductId}, Name={selectedProductName}, Desc={selectedProductDescription}");
            }
        }

        private void txtBname_TextUpdate(object sender, EventArgs e)
        {
            string searchText = txtBname.Text.Trim();

            // Fetch filtered list from database
            var filteredBatches = DatabaseHelper.Instance.Getbatches(searchText);

            if (filteredBatches != null)
            {
                txtBname.Items.Clear();
                txtBname.Items.AddRange(filteredBatches.ToArray());

                txtBname.SelectionStart = searchText.Length;
                txtBname.SelectionLength = 0;
                txtBname.DroppedDown = true;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            dataGridView2.Columns.Clear();
        }
        private void AddProductToGrid()
        {
            if (string.IsNullOrWhiteSpace(txtproducts.Text) ||
                string.IsNullOrWhiteSpace(txtprice.Text) ||
                string.IsNullOrWhiteSpace(txtSprice.Text) ||
                string.IsNullOrWhiteSpace(txtquantity.Text))
            {
                MessageBox.Show("Please fill all product details before adding.", "Missing Info");
                return;
            }

            if (!decimal.TryParse(txtprice.Text, out decimal cost) ||
                !decimal.TryParse(txtSprice.Text, out decimal sale) ||
                !int.TryParse(txtquantity.Text, out int quantity))
            {
                MessageBox.Show("Invalid number in price or quantity.", "Input Error");
                return;
            }

            // Add row to gridview1
            dataGridView1.Rows.Add(selectedProductId, selectedProductName, selectedProductDescription, cost, sale, quantity);
            SaveTempData(); // 👈 Save after each add

            // Reset input fields
            txtproducts.Clear();
            txtprice.Clear();
            txtSprice.Clear();
            txtquantity.Clear();
            selectedProductId = 0;
            selectedProductName = null;
            selectedProductDescription = null;

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !dataGridView1.Rows[e.RowIndex].IsNewRow)
            {
                DialogResult result = MessageBox.Show("Do you want to delete this row?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    dataGridView1.Rows.RemoveAt(e.RowIndex);
                    SaveTempData(); // Save after deletion
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void btnadd_Click(object sender, EventArgs e)
        {
            AddProductToGrid();
            SaveTempData();
        }

        private void iconPictureBox3_Click(object sender, EventArgs e)
        {
            var f=Program.ServiceProvider.GetRequiredService<Addproductform>();
            f.ShowDialog(this);
        }
    }
}