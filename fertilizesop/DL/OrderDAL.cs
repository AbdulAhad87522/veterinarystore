using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fertilizesop.UI;
using MySql.Data.MySqlClient;
using fertilizesop.BL.Models;
using fertilizesop.Interfaces.DLinterfaces;
using KIMS;
using System.Windows.Forms;
using System.Xml.Linq;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Drawing.Printing;
using System.Drawing;
using System.IO;
using fertilizesop.BL.Models.persons;

namespace fertilizesop.DL
{
    internal class OrderDAL : IOrder
    {
        public int InsertOrder(Order order)
        {
            int orderId = 0;
            int supplier_id=DatabaseHelper.Instance.getsuppierid(order.supplier_name);
            using (var con = DatabaseHelper.Instance.GetConnection())
            {
                string query = "INSERT INTO orders (supplier_id, date, order_status) VALUES (@supplierId, @date, @status); SELECT LAST_INSERT_ID();";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@supplierId", supplier_id);
                cmd.Parameters.AddWithValue("@date", order.OrderDate);
                cmd.Parameters.AddWithValue("@status", order.status);

                con.Open();
                orderId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return orderId;
        }

        public void InsertOrderDetail(OrderDetail detail)
        {
            using (var con = DatabaseHelper.Instance.GetConnection())
            {
                string query = "INSERT INTO orderdetails (order_id, product_id, quantity) VALUES (@orderId, @productId, @quantity)";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@orderId", detail.OrderId);
                cmd.Parameters.AddWithValue("@productId", detail.ProductId);
                cmd.Parameters.AddWithValue("@quantity", detail.Quantity);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateOrderDetail(OrderDetail detail)
        {
            using (var con = DatabaseHelper.Instance.GetConnection())
            {
                string query = "UPDATE orderdetails SET quantity = @quantity WHERE order_id = @order_id AND product_id = @product_id";

                MySqlCommand cmd = new MySqlCommand(query, con);

                cmd.Parameters.AddWithValue("@order_id", detail.OrderId);
                cmd.Parameters.AddWithValue("@product_id", detail.ProductId);
                cmd.Parameters.AddWithValue("@quantity", detail.Quantity);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool Delete(int orderDetailId)
        {
            using (var con = DatabaseHelper.Instance.GetConnection())
            {
                string query = "DELETE FROM orderdetails WHERE product_id = @id";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", orderDetailId);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }


        public DataTable LoadOrdersWithDetails()
        {
            using (var con = DatabaseHelper.Instance.GetConnection())
            {
                string query = @"
            SELECT 
                s.name                  AS SupplierName,
                o.date                  AS OrderDate,
                p.name                  AS ProductName,
                od.quantity             AS Quantity,
                p.sale_price            AS SalePrice
            FROM orders o
            INNER JOIN suppliers s ON o.supplier_id = s.supplier_id
            INNER JOIN orderdetails od ON o.order_id = od.order_id
            INNER JOIN products p ON od.product_id = p.product_id
            ORDER BY o.date DESC, o.order_id, od.orderdetail_id";

                MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetProducts()
        {
            using (var conn = DatabaseHelper.Instance.GetConnection())
            {
                string query = "SELECT product_id, name, description,sale_price FROM products";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetAllSuppliers()
        {
            DataTable dt = new DataTable();

            using (var conn = DatabaseHelper.Instance.GetConnection())
            {
                string query = "SELECT supplier_id, name FROM suppliers";
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                {
                    adapter.Fill(dt);
                }
            }

            return dt;
        }

        public DataTable GetOrders()
        {
            using (var conn = DatabaseHelper.Instance.GetConnection())
            {
                string query = @"
        SELECT 
            o.order_id AS OrderID,
            s.name AS SupplierName,
            o.date AS OrderDate,
            o.order_Status AS `Order Status`
        FROM orders o
        INNER JOIN suppliers s ON o.supplier_id = s.supplier_id
        ORDER BY o.date DESC, o.order_id;";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }


        public List<Suppliers> GetSuppliers(string searchText)
        {
            List<Suppliers> suppliers = new List<Suppliers>();

            using (var con = DatabaseHelper.Instance.GetConnection())
            {
                string query = "SELECT supplier_id, name FROM suppliers WHERE name LIKE @search";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");

                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        suppliers.Add(new Suppliers
                        {
                            Id = reader.GetInt32("supplier_id"),
                            first_Name = reader.GetString("name")
                        });
                    }
                }
            }

            return suppliers;
        }


        public DataTable GetOrderDetailsByOrderId(int orderId)
        {
            using (var con = DatabaseHelper.Instance.GetConnection())
            {
                string query = @"
            SELECT 
                p.name AS ProductName,
                p.description AS Description,
                p.sale_price AS Price,
                od.quantity AS Quantity
            FROM orderdetails od
            LEFT JOIN products p ON od.product_id = p.product_id
            WHERE od.order_id = @orderId";

                using (var cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public void MarkOrderAsCompleted(int orderId)
        {
            using (var con = DatabaseHelper.Instance.GetConnection())
            {
                string query = "UPDATE orders SET Order_Status = 'Completed' WHERE order_id = @orderId";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        // Pdf and Print Functions Here


        public void CreateOrderInvoicePdf(DataGridView cart, string filePath, string Name, DateTime saleDate)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Content().Column(content =>
                    {
                        // Header with logo and store info
                        content.Item().Row(row =>
                        {
                            row.RelativeItem(1).Column(col =>
                            {
                                col.Item().Image("Resources/logo for car.png", ImageScaling.FitWidth);
                            });

                            row.RelativeItem(3).Column(col =>
                            {
                                col.Item().AlignRight().Text("Fertilizer Shop").FontSize(22).Bold();
                                col.Item().AlignRight().Text("123 Market Road, CityName").FontSize(10);
                                col.Item().AlignRight().Text("Phone: +92-300-1234567").FontSize(10);
                                col.Item().AlignRight().Text("Email: support@Fertilizestore.com").FontSize(10);
                            });
                        });

                        content.Item().Element(e =>
                            e.PaddingVertical(10)
                             .LineHorizontal(1)
                             .LineColor(Colors.Grey.Lighten2)
                        );

                        // Invoice title and supplier info
                        content.Item().Text("Order Invoice").FontSize(20).Bold().AlignCenter();
                        content.Item().Text($"Supplier Name: {Name}").FontSize(14);
                        content.Item().Text($"Date: {saleDate.ToShortDateString()}").FontSize(12);

                        content.Item().Element(e =>
                            e.PaddingVertical(10)
                             .LineHorizontal(1)
                             .LineColor(Colors.Grey.Lighten2)
                        );

                        // Table of items
                        content.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3); // Name
                                columns.RelativeColumn(4); // Description
                                columns.RelativeColumn(2); // Price
                                columns.RelativeColumn(2); // Quantity
                                columns.RelativeColumn(2); // Total
                            });

                            // Header
                            table.Header(header =>
                            {
                                header.Cell().BorderBottom(1).BorderColor(Colors.Black).Text("Product").Bold();
                                header.Cell().BorderBottom(1).BorderColor(Colors.Black).Text("Description").Bold();
                                header.Cell().BorderBottom(1).BorderColor(Colors.Black).Text("Price").Bold();
                                header.Cell().BorderBottom(1).BorderColor(Colors.Black).Text("Quantity").Bold();
                                header.Cell().BorderBottom(1).BorderColor(Colors.Black).Text("Total").Bold();
                            });

                            decimal grandTotal = 0;

                            foreach (DataGridViewRow row in cart.Rows)
                            {
                                if (row.IsNewRow) continue;

                                string name = row.Cells["Name"]?.Value?.ToString();
                                string description = row.Cells["Description"]?.Value?.ToString();
                                string price = row.Cells["Price"]?.Value?.ToString();
                                string quantity = row.Cells["Quantity"]?.Value?.ToString();
                                string total = row.Cells["Total"]?.Value?.ToString();

                                decimal.TryParse(total, out decimal totalValue);
                                grandTotal += totalValue;

                                table.Cell().Text(name);
                                table.Cell().Text(description);
                                table.Cell().Text(price);
                                table.Cell().Text(quantity);
                                table.Cell().Text(total);
                            }

                            // Line before Grand Total
                            table.Cell().ColumnSpan(5).Element(e =>
                                e.PaddingTop(5).LineHorizontal(1).LineColor(Colors.Black)
                            );


                            // Grand total row
                            table.Cell().ColumnSpan(4).AlignRight().Text("Grand Total:").Bold();
                            table.Cell().Text(grandTotal.ToString("F2")).Bold();
                        });

                        content.Item().PaddingTop(10);

                        // Footer
                        page.Footer().Element(e =>
                        {
                            e.PaddingVertical(5).Column(col =>
                            {
                                col.Item().Element(x => x.ExtendHorizontal().LineHorizontal(1).LineColor(Colors.Grey.Lighten2));
                                col.Item().AlignCenter().Text("Developed By : jj").FontSize(12).Italic();
                            });
                        });
                    });
                });
            }).GeneratePdf(filePath);
        }


        public void PrintOrderInvoiceDirectly(DataGridView cart, string supplierName, DateTime Date)
        {
            if (PrinterSettings.InstalledPrinters.Count == 0)
            {
                MessageBox.Show("No printers are installed on this system.", "Printer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += (sender, e) =>
            {
                DrawPurchaseInvoice(e, cart, supplierName, Date);
            };

            PrintDialog dialog = new PrintDialog
            {
                Document = printDoc,
                AllowSomePages = false,
                UseEXDialog = true
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
                MessageBox.Show("Order Invoice sent to printer.", "Printed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void DrawPurchaseInvoice(PrintPageEventArgs e, DataGridView cart, string supplierName, DateTime Date)
        {
            Font titleFont = new Font("Arial", 18, FontStyle.Bold);
            Font headerFont = new Font("Arial", 12, FontStyle.Bold);
            Font regularFont = new Font("Arial", 11);
            Pen borderPen = new Pen(System.Drawing.Color.Gray, 0.5f);
            Brush brush = Brushes.Black;

            float x = 50, y = 60;
            float pageWidth = e.PageBounds.Width - 100;

            // Logo
            try
            {
                string imagePath = "Resources/logo for car.png";
                if (!File.Exists(imagePath))
                {
                    MessageBox.Show("Logo not found at: " + Path.GetFullPath(imagePath));
                }
                else
                {
                    System.Drawing.Image logo = System.Drawing.Image.FromFile(imagePath);
                    e.Graphics.DrawImage(logo, 50, 50, 200, 100);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Image load error: " + ex.Message);
            }

            // Company Info
            e.Graphics.DrawString("Fertilizer Shop", titleFont, brush, x + 400, y);
            y += 25;
            e.Graphics.DrawString("123 Market Road, CityName", regularFont, brush, x + 400, y);
            y += 18;
            e.Graphics.DrawString("Phone: +92-300-1234567", regularFont, brush, x + 400, y);
            y += 18;
            e.Graphics.DrawString("Email: support@Fertilizer.com", regularFont, brush, x + 400, y);

            y += 40;
            e.Graphics.DrawLine(Pens.Gray, x, y, x + pageWidth, y);
            y += 10;

            // Invoice Header
            e.Graphics.DrawString("Order Invoice", titleFont, brush, x + 250, y);
            y += 30;
            e.Graphics.DrawString($"Supplier: {supplierName}", regularFont, brush, x, y);
            y += 20;
            e.Graphics.DrawString($"Date: {Date.ToShortDateString()}", regularFont, brush, x, y);
            y += 25;

            // Table Headers (match your DataGridView column names)
            string[] headers = { "Product Name", "Description", "Price", "Quantity", "Total" };
            float[] widths = { 200, 250, 100, 100, 100 };
            float tableX = x;
            float rowHeight = 30;

            for (int i = 0; i < headers.Length; i++)
            {
                e.Graphics.FillRectangle(Brushes.LightGray, tableX, y, widths[i], rowHeight);
                e.Graphics.DrawRectangle(borderPen, tableX, y, widths[i], rowHeight);
                e.Graphics.DrawString(headers[i], headerFont, brush, tableX + 3, y + 5);
                tableX += widths[i];
            }

            y += rowHeight;

            // Table Rows
            foreach (DataGridViewRow row in cart.Rows)
            {
                if (row.IsNewRow) continue;

                string[] values =
                {
            row.Cells["Name"]?.Value?.ToString(),
            row.Cells["Description"]?.Value?.ToString(),
            row.Cells["Price"]?.Value?.ToString(),
            row.Cells["Quantity"]?.Value?.ToString(),
            row.Cells["Total"]?.Value?.ToString()
        };

                tableX = x;
                int columns = Math.Min(values.Length, widths.Length);

                for (int i = 0; i < columns; i++)
                {
                    e.Graphics.DrawRectangle(borderPen, tableX, y, widths[i], rowHeight);
                    e.Graphics.DrawString(values[i], regularFont, brush, new RectangleF(tableX + 3, y + 5, widths[i] - 6, rowHeight - 6));
                    tableX += widths[i];
                }

                y += rowHeight;

                // Page break logic
                if (y + rowHeight > e.PageBounds.Height - 100)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            y += 10;
            e.Graphics.DrawLine(Pens.Gray, x, y, x + pageWidth, y);

            // Footer at the very bottom
            float footerY = e.PageBounds.Height - 60;
            e.Graphics.DrawLine(Pens.Gray, x, footerY, x + pageWidth, footerY);
            e.Graphics.DrawString("Developed By : jj", new Font("Arial", 10, FontStyle.Italic), brush, x + pageWidth / 2 - 80, footerY + 5);
        }

    }
}
