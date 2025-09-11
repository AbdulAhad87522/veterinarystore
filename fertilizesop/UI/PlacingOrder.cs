using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fertilizesop.BL.Models;
using fertilizesop.DL;
using System.IO;
using fertilizesop.BL.Bl;

using Newtonsoft.Json;

namespace fertilizesop.UI
{
    public partial class PlacingOrder : Form

    {
        OrderBl o=new OrderBl();
        private DataGridView dgvProductSearch;
        private DataTable allProducts; // holds all products from DB or dummy
        private int? editingRowIndex = null;
        int selectedProductId = 0;
        int SelctedOrder;
        string supplierName;
        public PlacingOrder(int passedOrderId, string supplierName)
        {
            InitializeComponent();
            SelctedOrder = passedOrderId;
            ConfigureInvoiceGrid();
            SetupSearchGrid();
            LoadProductData(); // Fill allProducts
            dgvInvoice.AllowUserToAddRows = false;
            this.supplierName = supplierName;
            this.KeyPreview = true; // Put this in OrdersMain constructor
           
        }

        private void ConfigureInvoiceGrid()
        {
            dgvInvoice.Columns.Clear();

            dgvInvoice.Columns.Add("product_id", "Product ID");
            //dgvInvoice.Columns["product_id"].Visible = false; // hide it
            dgvInvoice.Columns.Add("Name", "Product Name");
            dgvInvoice.Columns.Add("Description", "Description");
            dgvInvoice.Columns.Add("Price", "Price");
            dgvInvoice.Columns.Add("Quantity", "Quantity");
            dgvInvoice.Columns.Add("Total", "Total");

            dgvInvoice.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void LoadProductData()
        {
            allProducts = o.GetProducts();

        }

        private void SetupSearchGrid()
        {
            dgvProductSearch = new DataGridView
            {
                Location = new Point(txtProductName.Left, txtProductName.Bottom + 5),
                Width = txtProductName.Width + 1200,
                Height = 580,
                Visible = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                MultiSelect = false,
                BackgroundColor = SystemColors.Window,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.Fixed3D
            };
            dgvProductSearch.Columns.Add("product_id", "product_id");
            dgvProductSearch.Columns.Add("Name", "Product Name");
            dgvProductSearch.Columns.Add("Description", "Description");
            dgvProductSearch.Columns.Add("Price", "sale_price");

            //dgvProductSearch.Columns["product_id"].Visible = false;
            dgvProductSearch.CellClick += DgvProductSearch_CellClick;
            this.Controls.Add(dgvProductSearch);
        }

        private void DgvProductSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var value = dgvProductSearch.Rows[e.RowIndex].Cells["product_id"].Value;
                if (value != null && int.TryParse(value.ToString(), out int id))
                {
                    selectedProductId = id;
                }
                else
                {
                    // Handle the error (e.g., show a message)
                    MessageBox.Show("Invalid product ID.");
                }

                string name = dgvProductSearch.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                string desc = dgvProductSearch.Rows[e.RowIndex].Cells["Description"].Value.ToString();
                string prices = dgvProductSearch.Rows[e.RowIndex].Cells["Price"].Value.ToString();


                txtProductName.Text = name;
                txtdescription.Text = desc;
                Price.Text = prices;

                dgvProductSearch.Visible = false;
                txtQuantity.Focus();
            }
        }

        private void CalculateTotalAmount()
        {
            decimal total = 0;

            foreach (DataGridViewRow row in dgvInvoice.Rows)
            {
                if (row.Cells["Total"]?.Value != null)
                {
                    decimal rowTotal;
                    if (decimal.TryParse(row.Cells["Total"].Value.ToString(), out rowTotal))
                    {
                        total += rowTotal;
                    }
                }
            }

            texttotal.Text = total.ToString("0.00"); // format to 2 decimal places

        }

        private void SaveOrder()
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text) || string.IsNullOrWhiteSpace(txtQuantity.Text) ||
        string.IsNullOrWhiteSpace(txtdescription.Text) || string.IsNullOrWhiteSpace(Price.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (!int.TryParse(txtQuantity.Text.Trim(), out int quantity))
            {
                MessageBox.Show("Quantity should be in digits only.");
                txtQuantity.Focus();
                return;
            }

            if (selectedProductId == 0)
            {
                MessageBox.Show("Please select a valid product.");
                return;
            }

            decimal price = Convert.ToDecimal(Price.Text.Trim());
            decimal total = price * quantity;

            if (editingRowIndex != null)
            {
                dgvInvoice.Rows[(int)editingRowIndex].SetValues(
                    selectedProductId,
                    txtProductName.Text,
                    txtdescription.Text,
                    Price.Text,
                    txtQuantity.Text,
                    total.ToString("0.00")
                );

                editingRowIndex = null;
                MessageBox.Show("Row updated in grid.");
            }
            else
            {
                dgvInvoice.Rows.Add(
                    selectedProductId,
                    txtProductName.Text,
                    txtdescription.Text,
                    Price.Text,
                    txtQuantity.Text,
                    total.ToString("0.00")
                );
            }

            CalculateTotalAmount();

            // Clear inputs
            txtProductName.Clear();
            txtdescription.Clear();
            Price.Clear();
            txtQuantity.Clear();
            selectedProductId = 0;
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            SaveOrder();
            SaveTempInvoiceToJson(SelctedOrder); // ✅ Save temp after adding to grid
        }

        private void SaveAllGridToDatabase()
        {
            if (dgvInvoice.Rows.Count == 0)
            {
                MessageBox.Show("No products to save.");
                return;
            }

            try
            {
                foreach (DataGridViewRow row in dgvInvoice.Rows)
                {
                    if (row.IsNewRow) continue;

                    int productId = Convert.ToInt32(row.Cells["product_id"].Value); // ✅ Correct
                    int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);

                    OrderDetail detail = new OrderDetail(SelctedOrder, productId, quantity);
                    o.InsertOrderDetail(detail);
                }

                MessageBox.Show("All products saved to database successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while saving to database: " + ex.Message);
            }
        }

        private void btnedit_Click(object sender, EventArgs e)
        {

            if (dgvInvoice.SelectedRows.Count > 0)
            {
                var row = dgvInvoice.SelectedRows[0];
                txtProductName.Text = row.Cells["Name"].Value?.ToString();
                txtdescription.Text = row.Cells["Description"].Value?.ToString();
                txtQuantity.Text = row.Cells["Quantity"].Value?.ToString();
                Price.Text = row.Cells["Price"].Value?.ToString();

                editingRowIndex = row.Index;
            }
            else
            {
                MessageBox.Show("Please select a row to edit.");
            }
        }

        private void DeleteSelectedRow()
        {
            if (dgvInvoice.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                                     
                        dgvInvoice.Rows.RemoveAt(dgvInvoice.SelectedRows[0].Index);
                        CalculateTotalAmount();
                        MessageBox.Show("Item deleted successfully.");                   
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            DeleteSelectedRow();
            SaveTempInvoiceToJson(SelctedOrder); // ✅ Save temp after deleting from grid
        }

        private void txtProductName_TextChanged_1(object sender, EventArgs e)
        {
            string search = txtProductName.Text.Trim().ToLower();
            if (allProducts == null || string.IsNullOrEmpty(search))
            {
                dgvProductSearch.Visible = false;
                return;
            }

            var filtered = allProducts.AsEnumerable()
        .Where(row =>
                    row.Field<string>("name").ToLower().Contains(search) ||
                    row.Field<string>("description").ToLower().Contains(search) ||
                   row.Field<int>("sale_price").ToString().Contains(search)
             )
            .ToList();

            dgvProductSearch.Rows.Clear();

            if (filtered.Any())
            {
                foreach (var row in filtered)
                {
                    dgvProductSearch.Rows.Add(
                        row["product_id"].ToString(),
                        row["name"].ToString(),
                        row["description"].ToString(),
                        row["sale_price"].ToString()
                    );
                }
                dgvProductSearch.Visible = true;
                dgvProductSearch.BringToFront();
            }
            else
            {
                dgvProductSearch.Visible = false;
            }
        }

        //PDF button Code
        private void btnsave_Click(object sender, EventArgs e)
        {
            SaveAllGridToDatabase();

            
            DateTime Date = DateTime.Now;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            saveFileDialog.Title = "Save Order Invoice";
            saveFileDialog.FileName = $"OrderInvoice_{SelctedOrder}.pdf";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                o.CreateOrderInvoicePdf(dgvInvoice, filePath, supplierName, Date);
                MessageBox.Show("PDF Generated Successfully.");

                string tempFilePath = GetJsonFilePath(SelctedOrder);

                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }

                ClearInvoiceForm();
                o.MarkOrderAsCompleted(SelctedOrder);
            }
            else
            {
                MessageBox.Show("PDF generation was cancelled.");
            }
        }

        private void ClearInvoiceForm()
        {
           
            dgvInvoice.Rows.Clear();
            texttotal.Clear();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (dgvInvoice.Rows.Count == 0)
            {
                MessageBox.Show("No items to print in invoice.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            DateTime purchaseDate = DateTime.Now;
            o.PrintOrderInvoiceDirectly(dgvInvoice, supplierName, purchaseDate);


            string tempFilePath = GetJsonFilePath(SelctedOrder);

            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }


            ClearInvoiceForm();

            MessageBox.Show("Invoice printed", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

       
        //keys functions here
        

        private int selectedRowIndex;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
           
            if (keyData == Keys.Enter && !(ActiveControl is DataGridView))
            {
                btnadd.PerformClick();
                return true;
            }

            else if (keyData == (Keys.Control | Keys.S))
            {
                btnsave.PerformClick();
                return true;
            }

            else if (keyData == Keys.Delete)
            {
                btndelete.PerformClick();
                return true;
            }

            else if (keyData == Keys.Up)
            {
                if (dgvProductSearch.Visible && selectedRowIndex > 0)
                {
                    selectedRowIndex--;
                    dgvProductSearch.ClearSelection();
                    dgvProductSearch.Rows[selectedRowIndex].Selected = true;
                    return true;
                }
            }
            else if (keyData == Keys.Down)
            {
                if (dgvProductSearch.Visible && selectedRowIndex < dgvProductSearch.Rows.Count - 1)
                {
                    selectedRowIndex++;
                    dgvProductSearch.ClearSelection();
                    dgvProductSearch.Rows[selectedRowIndex].Selected = true;
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData); // Allow default behavior
        }

        //Json functions here

        private void PlacingOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveTempInvoiceToJson(SelctedOrder);
        }

        private void PlacingOrder_Load(object sender, EventArgs e)
        {
            LoadTempInvoiceFromJson(SelctedOrder);
        }

        private void PlacingOrder_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                SaveTempInvoiceToJson(SelctedOrder);
            }
        }

        private string GetJsonFilePath(int SelctedOrder)
        {
            // Store inside: %AppData%\Fertilizer\TempInvoices
            string folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Fertilizer",
                "TempInvoices"
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

            return Path.Combine(folder, $"Invoice_{SelctedOrder}.json");
        }


        private void SaveTempInvoiceToJson(int SelctedOrder)
        {
            try
            {
                var items = new List<TempInvoiceItem>();

                foreach (DataGridViewRow row in dgvInvoice.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        items.Add(new TempInvoiceItem
                        {
                            ProductId = Convert.ToInt32(row.Cells["product_id"].Value),
                            ProductName = row.Cells["Name"].Value?.ToString(),
                            Description = row.Cells["Description"].Value?.ToString(),
                            Price = Convert.ToDecimal(row.Cells["Price"].Value),
                            Quantity = Convert.ToInt32(row.Cells["Quantity"].Value)
                        });
                    }
                }

                string json = JsonConvert.SerializeObject(items, Formatting.Indented);
                File.WriteAllText(GetJsonFilePath(SelctedOrder), json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving temp invoice: {ex.Message}");
            }
        }


        private void LoadTempInvoiceFromJson(int SelctedOrder)
        {
            string filePath = GetJsonFilePath(SelctedOrder);

            if (!File.Exists(filePath)) return;

            try
            {
                string json = File.ReadAllText(filePath);
                var items = JsonConvert.DeserializeObject<List<TempInvoiceItem>>(json);

                dgvInvoice.Rows.Clear();

                foreach (var item in items)
                {
                    dgvInvoice.Rows.Add(
                        item.ProductId,
                        item.ProductName,
                        item.Description,
                        item.Price,
                        item.Quantity,
                        item.Total
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading temp invoice: {ex.Message}");
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {

        }
    }

}
