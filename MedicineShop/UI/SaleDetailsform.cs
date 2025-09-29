using MedicineShop.DL;
using MySql.Data.MySqlClient;
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
        public int SaleId { get; set; }  // Add SaleId property
        private List<Custbilldl.CustomerSale> allCustomerSales;

        public SaleDetailsform()
        {
            InitializeComponent();
            UIHelper.StyleGridView(dataGridView2);

            if (this.Controls.Find("textBox1", true).FirstOrDefault() is TextBox searchBox)
            {
                searchBox.TextChanged += TextBox1_TextChanged;
            }
        }

        public void LoadSaleItems()
        {
            try
            {
                var saleItems = GetSaleItems(SaleId);

                var saleInfo = GetSaleInfo(SaleId);
                var displayTable = new DataTable();
                displayTable.Columns.Add("Product Name", typeof(string));
                displayTable.Columns.Add("Quantity", typeof(int));
                displayTable.Columns.Add("Unit Price", typeof(decimal));
                displayTable.Columns.Add("Discount %", typeof(decimal));
                displayTable.Columns.Add("Subtotal", typeof(decimal));

                decimal grandTotal = 0;
                foreach (var item in saleItems)
                {
                    displayTable.Rows.Add(
                        item.ProductName,
                        item.Quantity,
                        item.Price,
                        item.Discount,
                        item.Total
                    );
                    grandTotal += item.Total;
                }

                dataGridView2.DataSource = displayTable;

                // Format currency columns
                if (dataGridView2.Columns["Unit Price"] != null)
                    dataGridView2.Columns["Unit Price"].DefaultCellStyle.Format = "N2";

                if (dataGridView2.Columns["Subtotal"] != null)
                    dataGridView2.Columns["Subtotal"].DefaultCellStyle.Format = "N2";

                // Update form title with sale information
                if (saleInfo != null)
                {
                    this.Text = $"Sale Details - Sale #{SaleId} - {saleInfo.CustomerName} - {saleInfo.SaleDate:dd/MM/yyyy}";
                }

                // Hide search textbox if loading specific sale items
                if (this.Controls.Find("textBox1", true).FirstOrDefault() is TextBox searchBox)
                {
                    searchBox.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading sale items: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private SaleHeaderInfo GetSaleInfo(int saleId)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            s.sale_id,
                            s.sale_date,
                            s.total_amount,
                            s.paid_amount,
                            c.full_name as customer_name
                        FROM sales s
                        INNER JOIN customers c ON s.customer_id = c.customer_id
                        WHERE s.sale_id = @SaleId";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SaleId", saleId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new SaleHeaderInfo
                                {
                                    SaleId = reader.GetInt32("sale_id"),
                                    SaleDate = reader.GetDateTime("sale_date"),
                                    TotalAmount = reader.GetDecimal("total_amount"),
                                    PaidAmount = reader.GetDecimal("paid_amount"),
                                    CustomerName = reader.GetString("customer_name")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting sale info: {ex.Message}");
            }
            return null;
        }

        private List<SaleItemInfo> GetSaleItems(int saleId)
        {
            var items = new List<SaleItemInfo>();

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            si.sale_item_id,
                            si.quantity,
                            si.price,
                            si.discount,
                            m.name,
                            c.company_name,
                            cat.category_name,
                            p.packing_name,
                            (si.quantity * si.price * (1 - si.discount/100)) as total
                        FROM sale_items si
                        INNER JOIN batch_items bi ON si.batch_item_id = bi.batch_item_id
                        INNER JOIN medicines m ON bi.product_id = m.product_id
                        INNER JOIN company c ON m.company_id = c.company_id
                        INNER JOIN categories cat ON m.category_id = cat.category_id
                        INNER JOIN packing p ON m.packing_id = p.packing_id
                        WHERE si.sale_id = @SaleId";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SaleId", saleId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string productName = $"{reader.GetString("company_name")} - {reader.GetString("category_name")} - {reader.GetString("packing_name")}";

                                items.Add(new SaleItemInfo
                                {
                                    SaleItemId = reader.GetInt32("sale_item_id"),
                                    ProductName = productName,
                                    Quantity = reader.GetInt32("quantity"),
                                    Price = reader.GetDecimal("price"),
                                    Discount = reader.GetDecimal("discount"),
                                    Total = reader.GetDecimal("total")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading sale items from database: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return items;
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
                dataGridView2.Columns["TotalAmount"].DefaultCellStyle.Format = "N2";
            }

            if (dataGridView2.Columns["PaidAmount"] != null)
            {
                dataGridView2.Columns["PaidAmount"].HeaderText = "Paid Amount";
                dataGridView2.Columns["PaidAmount"].DefaultCellStyle.Format = "N2";
            }

            if (dataGridView2.Columns["RemainingAmount"] != null)
            {
                dataGridView2.Columns["RemainingAmount"].HeaderText = "Remaining";
                dataGridView2.Columns["RemainingAmount"].DefaultCellStyle.Format = "N2";
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
                        filteredSales = allCustomerSales ?? new List<Custbilldl.CustomerSale>();
                    }
                    else
                    {
                        filteredSales = Custbilldl.SearchCustomerSales(CustomerId, searchBox.Text);
                    }

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
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                try
                {
                    int saleId = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["SaleId"].Value);
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
            TextBox1_TextChanged(sender, e);
        }

        private void SaleDetailsform_Load(object sender, EventArgs e)
        {
            // Check which mode we're in
            if (SaleId > 0)
            {
                // Load specific sale items
                LoadSaleItems();
            }
            else if (CustomerId > 0)
            {
                // Load all customer sales
                LoadCustomerSales();
            }
        }

        public void RefreshData()
        {
            if (SaleId > 0)
            {
                LoadSaleItems();
            }
            else if (CustomerId > 0)
            {
                LoadCustomerSales();
            }
        }

        // Helper classes
        public class SaleItemInfo
        {
            public int SaleItemId { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal Discount { get; set; }
            public decimal Total { get; set; }
        }

        public class SaleHeaderInfo
        {
            public int SaleId { get; set; }
            public DateTime SaleDate { get; set; }
            public decimal TotalAmount { get; set; }
            public decimal PaidAmount { get; set; }
            public string CustomerName { get; set; }
        }
    }
}