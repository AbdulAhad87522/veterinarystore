using MedicineShop.DL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TechStore.UI;

namespace MedicineShop.UI
{
    public partial class SaleDetailsform : Form
    {
        public int CustomerId { get; set; }
        private List<Custbilldl.CustomerSale> allCustomerSales;

        public SaleDetailsform()
        {
            InitializeComponent();
            UIHelper.StyleGridView(dataGridView2);

            // Add search functionality if you have a search textbox
            // Assuming you have a textbox named textBox1 for searching
            if (this.Controls.Find("textBox1", true).FirstOrDefault() is TextBox searchBox)
            {
                searchBox.TextChanged += TextBox1_TextChanged;
            }
        }

        public void LoadCustomerSales()
        {
            try
            {
                // Load all sales for the specified CustomerId using the static method
                allCustomerSales = Custbilldl.GetCustomerSales(CustomerId);

                // Bind to DataGridView
                dataGridView2.DataSource = allCustomerSales.Select(sale => new
                {
                    SaleId = sale.SaleId,
                    SaleDate = sale.SaleDate.ToString("dd/MM/yyyy HH:mm"),
                    TotalAmount = sale.TotalAmount,
                    PaidAmount = sale.PaidAmount,
                    RemainingAmount = sale.RemainingAmount,
                    Status = sale.Status
                }).ToList();

                // Configure columns
                ConfigureGridColumns();

                // Update form title or label to show customer information
                if (allCustomerSales.Any())
                {
                    this.Text = $"Sale Details - {allCustomerSales.First().CustomerName}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customer sales: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureGridColumns()
        {
            // Configure column headers and formatting
            if (dataGridView2.Columns["SaleId"] != null)
                dataGridView2.Columns["SaleId"].HeaderText = "Sale ID";

            if (dataGridView2.Columns["SaleDate"] != null)
                dataGridView2.Columns["SaleDate"].HeaderText = "Sale Date";

            if (dataGridView2.Columns["TotalAmount"] != null)
            {
                dataGridView2.Columns["TotalAmount"].HeaderText = "Total Amount";
                dataGridView2.Columns["TotalAmount"].DefaultCellStyle.Format = "C2";
            }

            if (dataGridView2.Columns["PaidAmount"] != null)
            {
                dataGridView2.Columns["PaidAmount"].HeaderText = "Paid Amount";
                dataGridView2.Columns["PaidAmount"].DefaultCellStyle.Format = "C2";
            }

            if (dataGridView2.Columns["RemainingAmount"] != null)
            {
                dataGridView2.Columns["RemainingAmount"].HeaderText = "Remaining";
                dataGridView2.Columns["RemainingAmount"].DefaultCellStyle.Format = "C2";
            }

            if (dataGridView2.Columns["Status"] != null)
            {
                dataGridView2.Columns["Status"].HeaderText = "Payment Status";

                // Add color coding for status
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.Cells["Status"].Value != null)
                    {
                        string status = row.Cells["Status"].Value.ToString();
                        switch (status)
                        {
                            case "Paid":
                                row.Cells["Status"].Style.BackColor = Color.LightGreen;
                                break;
                            case "Partial":
                                row.Cells["Status"].Style.BackColor = Color.Yellow;
                                break;
                            case "Unpaid":
                                row.Cells["Status"].Style.BackColor = Color.LightCoral;
                                break;
                        }
                    }
                }
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            var searchBox = sender as TextBox;
            if (searchBox != null && CustomerId > 0)
            {
                try
                {
                    List<Custbilldl.CustomerSale> filteredSales;

                    if (string.IsNullOrWhiteSpace(searchBox.Text))
                    {
                        // Show all sales if search is empty
                        filteredSales = allCustomerSales ?? new List<Custbilldl.CustomerSale>();
                    }
                    else
                    {
                        // Use the static search method
                        filteredSales = Custbilldl.SearchCustomerSales(CustomerId, searchBox.Text);
                    }

                    // Update the DataGridView with filtered results
                    dataGridView2.DataSource = filteredSales.Select(sale => new
                    {
                        SaleId = sale.SaleId,
                        SaleDate = sale.SaleDate.ToString("dd/MM/yyyy HH:mm"),
                        TotalAmount = sale.TotalAmount,
                        PaidAmount = sale.PaidAmount,
                        RemainingAmount = sale.RemainingAmount,
                        Status = sale.Status
                    }).ToList();

                    ConfigureGridColumns();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error searching customer sales: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle any cell click events if needed
            // For example, if you want to show sale item details

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                try
                {
                    // Get the selected sale ID
                    int saleId = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["SaleId"].Value);

                    // You could open another form to show sale items for this sale
                    // var saleItemsForm = new SaleItemsDetailsForm();
                    // saleItemsForm.SaleId = saleId;
                    // saleItemsForm.LoadSaleItems();
                    // saleItemsForm.ShowDialog();

                    MessageBox.Show($"Selected Sale ID: {saleId}", "Sale Selected",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error selecting sale: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // This method calls the search functionality
            TextBox1_TextChanged(sender, e);
        }

        private void SaleDetailsform_Load(object sender, EventArgs e)
        {
            // Load customer sales when form loads
            if (CustomerId > 0)
            {
                LoadCustomerSales();
            }
        }

        // Method to refresh the data
        public void RefreshData()
        {
            LoadCustomerSales();
        }
    }
}