using fertilizesop.BL.Bl;
using fertilizesop.BL.Models;
using System;
using System.Windows.Forms;

namespace fertilizesop.UI
{
    public partial class Addreturmform : Form
    {
        private readonly IReturnsBl ibl;
        private int selectedProductId;
        private string selectedProductName;
        private string selectedProductDescription;

        public Addreturmform(IReturnsBl ibl)
        {
            InitializeComponent();
            this.ibl = ibl;

            // Style search grid and main grid
            UIHelper.StyleGridView(dataGridView2);
            UIHelper.StyleGridView(dataGridView1);

            dataGridView2.Visible = false;

            // Add columns to the return products grid
            InitializeProductGrid();

            // Hook events
            this.dataGridView2.CellClick += dataGridView2_CellClick;
        }

        /// <summary>
        /// Creates the required columns for dataGridView1
        /// </summary>
        private void InitializeProductGrid()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            var colId = new DataGridViewTextBoxColumn
            {
                Name = "ProductId",
                HeaderText = "Product ID",
                Visible = false
            };
            dataGridView1.Columns.Add(colId);

            var colName = new DataGridViewTextBoxColumn
            {
                Name = "ProductName",
                HeaderText = "Product Name",
                Width = 200
            };
            dataGridView1.Columns.Add(colName);

            var colDesc = new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Description",
                Width = 250
            };
            dataGridView1.Columns.Add(colDesc);

            var colRefund = new DataGridViewTextBoxColumn
            {
                Name = "RefundAmount",
                HeaderText = "Refund Amount"
            };
            dataGridView1.Columns.Add(colRefund);

            var colQty = new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                HeaderText = "Quantity"
            };
            dataGridView1.Columns.Add(colQty);
        }

        /// <summary>
        /// Add a selected product to the main grid
        /// </summary>
        private void AddProductToGrid()
        {
            if (string.IsNullOrWhiteSpace(txtname.Text) ||
                string.IsNullOrWhiteSpace(txtprice.Text) ||
                string.IsNullOrWhiteSpace(txtbill.Text) ||
                string.IsNullOrWhiteSpace(txtquantity.Text))
            {
                MessageBox.Show("Please fill all product details before adding.", "Missing Info");
                return;
            }

            if (!decimal.TryParse(txtprice.Text, out decimal refund) ||
                !int.TryParse(txtquantity.Text, out int quantity))
            {
                MessageBox.Show("Invalid number in price or quantity.", "Input Error");
                return;
            }

            // Add row to gridview1
            dataGridView1.Rows.Add(selectedProductId, selectedProductName, selectedProductDescription, refund, quantity);

            // Reset input fields
            txtname.Clear();
            txtprice.Clear();
            txtquantity.Clear();
            selectedProductId = 0;
            selectedProductName = null;
            selectedProductDescription = null;
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            AddProductToGrid();
        }

        /// <summary>
        /// Save all return items to DB
        /// </summary>
        private void btnsave_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No products to save.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;

                    var r = new returns
                    {
                        product_id = Convert.ToInt32(row.Cells["ProductId"].Value),
                        bill_id = Convert.ToInt32(txtbill.Text),
                        amount = Convert.ToDecimal(row.Cells["RefundAmount"].Value),
                        quantity_returned = Convert.ToInt32(row.Cells["Quantity"].Value),
                        return_date = DateTime.Now
                    };

                    bool success = ibl.AddReturn(r);
                    if (!success)
                    {
                        MessageBox.Show($"Failed to save return for product ID {r.product_id}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                MessageBox.Show("All returns saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Live product search
        /// </summary>
        private void txtname_TextChanged(object sender, EventArgs e)
        {
            string name = txtname.Text.Trim();

            if (!string.IsNullOrEmpty(name))
            {
                var list = ibl.GetProducts(name);
                dataGridView2.DataSource = list;

                if (dataGridView2.Columns.Contains("Id"))
                    dataGridView2.Columns["Id"].Visible = false;
                if (dataGridView2.Columns.Contains("Price"))
                    dataGridView2.Columns["Price"].Visible = false;
                if (dataGridView2.Columns.Contains("quantity"))
                    dataGridView2.Columns["quantity"].Visible = false;

                dataGridView2.Visible = true;
                dataGridView2.BringToFront();
            }
            else
            {
                dataGridView2.Visible = false;
            }
        }

        /// <summary>
        /// Select product from search results
        /// </summary>
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var rows = dataGridView2.Rows[e.RowIndex];
                selectedProductId = Convert.ToInt32(rows.Cells["Id"].Value);
                selectedProductDescription = rows.Cells["Description"].Value.ToString();
                selectedProductName = rows.Cells["Name"].Value.ToString();
                txtname.Text = selectedProductName;
                dataGridView2.Visible = false;
            }
        }

        public void SetBillId(int billId)
        {
            txtbill.Text = billId.ToString();
        }

        private void Addreturmform_Load(object sender, EventArgs e)
        {
            // Nothing needed for now
        }
    }
}
