using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Windows.Forms;
using QuestPDF.Helpers;
using System.Xml.Linq;
using System.Drawing.Printing;
using System.Drawing;

namespace MedicineShop.DL
{
    internal class Customersaledl
    {
        public DataTable GetProductThings(string text)
        {
            DataTable dt = new DataTable();
            using (var con = DatabaseHelper.Instance.GetConnection())
            {
                con.Open();
                string query = @"SELECT 
                                        m.name, 
                                        m.description,
                                        c.company_name, 
                                        m.sale_price,
                                        b.quantity_remaining,
                                        p.packing_name, 
                                        ca.category_name, 
                                        b.expiry_date
                                    FROM batch_items b
                                    JOIN medicines m ON m.product_id = b.product_id
                                    JOIN company c ON c.company_id = m.company_id
                                    JOIN packing p ON m.packing_id = p.packing_id
                                    JOIN categories ca ON ca.category_id = m.category_id
                                    WHERE m.name LIKE @text and b.quantity_remaining != 0 AND b.expiry_date > NOW()
                                    ORDER BY m.name, b.expiry_date;
                                    ";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@text", "%" + text + "%");

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public int getcustomerid(string text)
        {
            try
            {
                using (var con = DatabaseHelper.Instance.GetConnection())
                {
                    con.Open();
                    string query = "select customer_id from customers where full_name = @text";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@text", text);
                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : -1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to recieve customer_id " + ex);
            }
        }

        public static void CreateThermalReceiptPdf(DataGridView cart, string filePath, string customerName, decimal total, decimal paid , decimal totaldiscount)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(226, PageSizes.A4.Height, Unit.Point); // 80mm width
                    page.Margin(5);
                    page.DefaultTextStyle(x => x.FontFamily("Consolas").FontSize(9));

                    page.Content().Column(column =>
                    {
                        // --- Logo + Header ---
                        //column.Item().AlignCenter().Image(GetLogoImageStream(), ImageScaling.FitWidth);
                        column.Item().AlignCenter().Text("Ali Veterinary Store").Bold().FontSize(16);
                        //column.Item().AlignCenter().Text("MNS Computers").Bold().FontSize(12);
                        column.Item().AlignCenter().Text("main jalsai bazar, Tehsil Lahor district Swabi");
                        column.Item().AlignCenter().Text("Phone: 0300-6634245");
                        column.Item().PaddingBottom(5).LineHorizontal(0.5f);

                        // --- Invoice Info ---
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Text($"Customer: {customerName}");
                            row.RelativeItem().AlignRight().Text($"{DateTime.Now:dd-MMM-yyyy hh:mm tt}");
                        });

                        column.Item().PaddingBottom(5).LineHorizontal(0.5f);

                        // --- Table Header ---
                        column.Item().Text("----------------------------------------");
                        column.Item().Text("MEDIC         QTY PRICE DISC TOTAL");
                        column.Item().Text("----------------------------------------");

                        // --- Cart Items ---
                        decimal totalDiscount = 0;
                        decimal subTotal = 0;

                        foreach (DataGridViewRow row in cart.Rows)
                        {
                            if (row.IsNewRow) continue;

                            string name = row.Cells["name"].Value?.ToString() ?? "";
                            string qty = row.Cells["quantity"].Value?.ToString()?.PadLeft(2);
                            //string war = Truncate(row.Cells["Warranty"]?.Value?.ToString(), 3).PadRight(3);
                            string price = row.Cells["sale_price"].Value?.ToString()?.PadLeft(5);
                            string discount = row.Cells["discount"].Value?.ToString()?.PadLeft(3);
                            string totalPrice = row.Cells["final"].Value?.ToString()?.PadLeft(6);

                            if (decimal.TryParse(row.Cells["discount"].Value?.ToString(), out decimal discVal))
                                totalDiscount += discVal * Convert.ToInt32(row.Cells["quantity"].Value);
                            if (decimal.TryParse(row.Cells["final"].Value?.ToString(), out decimal itemTotal))
                                subTotal += itemTotal;

                            // Split name across lines
                            string[] nameParts = name.Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
                            string firstWord = nameParts.Length > 0 ? nameParts[0] : name;
                            string[] remainingWords = nameParts.Skip(1).ToArray();

                            // First line with first word and all data
                            string firstLine = $"{firstWord,-12}{qty} {price} {discount} {totalPrice}";
                            column.Item().Text(firstLine);

                            // Remaining words as new lines
                            foreach (var word in remainingWords)
                            {
                                column.Item().PaddingLeft(10).Text(word);
                            }
                        }

                        // --- Summary ---
                        column.Item().Text("----------------------------------------");
                        column.Item().Text($"SUBTOTAL:    Rs. {subTotal + totalDiscount:N0}");
                        column.Item().Text($"DISCOUNT:    Rs. {totaldiscount:N0}");
                        column.Item().Text($"TOTAL:       Rs. {total:N0}");
                        column.Item().Text($"PAID:        Rs. {paid:N0}");
                        column.Item().Text($"BALANCE:     Rs. {(total - paid):N0}");
                        column.Item().Text("----------------------------------------");

                        // --- Footer ---
                        column.Item().AlignCenter().Text("Thank you for your shopping here!").Bold();
                        column.Item().PaddingTop(5).LineHorizontal(0.5f);
                        column.Item().AlignCenter().Text("** SPECIAL OFFERS **").Bold();
                        column.Item().AlignCenter().Text("بل کے بغیر واپسی نہیں ہوگی");
                        column.Item().AlignCenter().Text("دوائیں استعمال ہونے کے بعد واپس نہیں ہوں گی");
                        column.Item().AlignCenter().Text("آپ کے اعتماد کا شکریہ");
                        column.Item().AlignCenter().Text($"Invoice #: INV-{DateTime.Now:yyMMddHHmm}");
                        column.Item().PaddingTop(5).AlignCenter().Text("Developed By:");
                        column.Item().PaddingTop(5).AlignCenter().Text("abdulahad18022@gmail.com");
                        column.Item().PaddingTop(5).AlignCenter().Text("03477048001");

                    });
                });
            }).GeneratePdf(filePath);
        }

        public static void PrintThermalReceipt(DataGridView cart, string customerName, decimal total, decimal paid, decimal totaldiscount)
        {
            PrintDocument printDocument = new PrintDocument();

            // Set up for 80mm thermal printer
            printDocument.DefaultPageSettings.PaperSize = new PaperSize("Receipt", 280, 0);
            printDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);

            Font receiptFont = new Font("Consolas", 8, FontStyle.Regular);
            Font boldFont = new Font("Consolas", 8, FontStyle.Bold);
            Font headerFont = new Font("Consolas", 12, FontStyle.Bold); // Bigger font for header
            int currentY = 5;

            printDocument.PrintPage += (sender, e) =>
            {
                Graphics g = e.Graphics;
                int width = e.PageBounds.Width;

                // Helper functions
                void DrawText(string text, Font font = null, int offsetX = 0)
                {
                    font = font ?? receiptFont;
                    g.DrawString(text, font, Brushes.Black, offsetX, currentY);
                    currentY += (int)g.MeasureString(text, font).Height + 1;
                }

                void DrawCenteredText(string text, Font font = null)
                {
                    font = font ?? receiptFont;
                    SizeF textSize = g.MeasureString(text, font);
                    float x = (width - textSize.Width) / 2;
                    g.DrawString(text, font, Brushes.Black, x, currentY);
                    currentY += (int)textSize.Height + 1;
                }

                void DrawLine()
                {
                    g.DrawLine(Pens.Black, 10, currentY, width - 10, currentY);
                    currentY += 5;
                }

                void DrawLeftRight(string left, string right)
                {
                    g.DrawString(left, receiptFont, Brushes.Black, 10, currentY);
                    SizeF rightSize = g.MeasureString(right, receiptFont);
                    g.DrawString(right, receiptFont, Brushes.Black, width - rightSize.Width - 10, currentY);
                    currentY += (int)rightSize.Height + 1;
                }

                // --- Header with Double Line Big Font ---
                DrawCenteredText("ALI VETERINARY", headerFont);
                DrawCenteredText("CLINIC", headerFont);
                DrawCenteredText("main jalsai bazar, Tehsil Lahor district Swabi");
                DrawCenteredText("Phone: 0300-6634245");
                DrawLine();

                // --- Customer Info ---
                DrawLeftRight($"Customer: {customerName}", $"{DateTime.Now:dd-MMM-yyyy hh:mm tt}");
                DrawLine();

                // --- Table Header ---
                DrawCenteredText("----------------------------------------");
                DrawText("MEDIC         QTY PRICE DISC TOTAL");
                DrawCenteredText("----------------------------------------");

                // --- Cart Items ---
                decimal totalDiscount = 0;
                decimal subTotal = 0;

                foreach (DataGridViewRow row in cart.Rows)
                {
                    if (row.IsNewRow) continue;

                    string name = row.Cells["name"].Value?.ToString() ?? "";
                    string qty = row.Cells["quantity"].Value?.ToString()?.PadLeft(2);
                    string price = row.Cells["sale_price"].Value?.ToString()?.PadLeft(5);
                    string discount = row.Cells["discount"].Value?.ToString()?.PadLeft(3);
                    string totalPrice = row.Cells["final"].Value?.ToString()?.PadLeft(6);

                    if (decimal.TryParse(row.Cells["discount"].Value?.ToString(), out decimal discVal))
                        totalDiscount += discVal * Convert.ToInt32(row.Cells["quantity"].Value);
                    if (decimal.TryParse(row.Cells["final"].Value?.ToString(), out decimal itemTotal))
                        subTotal += itemTotal;

                    // Split name across lines
                    string[] nameParts = name.Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
                    string firstWord = nameParts.Length > 0 ? nameParts[0] : name;
                    string[] remainingWords = nameParts.Skip(1).ToArray();

                    // First line with first word and all data
                    string firstLine = $"{firstWord,-12}{qty} {price} {discount} {totalPrice}";
                    DrawText(firstLine);

                    // Remaining words as new lines
                    foreach (var word in remainingWords)
                    {
                        DrawText($"    {word}"); // 4 spaces indentation
                    }
                }

                // --- Summary ---
                DrawCenteredText("----------------------------------------");
                DrawLeftRight($"SUBTOTAL:", $"Rs. {(subTotal + totalDiscount):N0}");
                DrawLeftRight($"DISCOUNT:", $"Rs. {totaldiscount:N0}");
                DrawLeftRight($"TOTAL:", $"Rs. {total:N0}");
                DrawLeftRight($"PAID:", $"Rs. {paid:N0}");
                DrawLeftRight($"BALANCE:", $"Rs. {(total - paid):N0}");
                DrawCenteredText("----------------------------------------");

                // --- Footer ---
                currentY += 3;
                DrawCenteredText("Thank you for your shopping here!", boldFont);
                DrawLine();
                DrawCenteredText("** SPECIAL OFFERS **", boldFont);
                DrawCenteredText("بل کے بغیر واپسی نہیں ہوگی");
                DrawCenteredText("دوائیں استعمال ہونے کے بعد واپس نہیں ہوں گی");
                DrawCenteredText("آپ کے اعتماد کا شکریہ");
                DrawCenteredText($"Invoice #: INV-{DateTime.Now:yyMMddHHmm}");
                currentY += 3;
                DrawCenteredText("Developed By:");
                DrawCenteredText("abdulahad18022@gmail.com");
                DrawCenteredText("03477048001");

            };

            // Show print dialog and print
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }


        public DataTable getallcustomer(string text)
        {
            DataTable dt = new DataTable();
            using (var con = DatabaseHelper.Instance.GetConnection())
            {
                con.Open();
                string query = "SELECT  full_name, address, phone FROM customers WHERE full_name LIKE @text";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@text", "%" + text + "%");
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public bool SaveDataToDatabase(int? id, DateTime? date, int? total_amount, int? paid_amount, DataGridView d)
        {
            using (var con = DatabaseHelper.Instance.GetConnection())
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        // Validate input data first
                        if (d.Rows.Count == 0 || (d.Rows.Count == 1 && d.Rows[0].IsNewRow))
                        {
                            throw new Exception("No products selected for sale");
                        }

                        string query = @"INSERT INTO sales (customer_id, total_amount, paid_amount, sale_date) 
                        VALUES (@id, @total_amount, @paid_amount, @date);
                        SELECT LAST_INSERT_ID();";
                        int billid;
                        using (MySqlCommand cmd = new MySqlCommand(query, con, tran))
                        {
                            cmd.Parameters.AddWithValue("@id", id ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@total_amount", total_amount ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@paid_amount", paid_amount ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@date", date ?? (object)DBNull.Value);

                            object result = cmd.ExecuteScalar();
                            if (result == null)
                            {
                                throw new Exception("Failed to get sale ID");
                            }
                            billid = Convert.ToInt32(result);
                        }

                        // Insert into customer price record
                        string query2 = "INSERT INTO customerpricerecord (customer_id, sale_id, date, payment) VALUES (@c_id, @s_id, @date, @payment)";
                        using (MySqlCommand cmd2 = new MySqlCommand(query2, con, tran))
                        {
                            cmd2.Parameters.AddWithValue("@c_id", id ?? (object)DBNull.Value);
                            cmd2.Parameters.AddWithValue("@s_id", billid);
                            cmd2.Parameters.AddWithValue("@date", date ?? (object)DBNull.Value);
                            cmd2.Parameters.AddWithValue("@payment", paid_amount ?? (object)DBNull.Value);
                            cmd2.ExecuteNonQuery();
                        }

                        foreach (DataGridViewRow row in d.Rows)
                        {
                            if (row.IsNewRow) continue;

                            int productid;
                            string name = row.Cells["name"]?.Value?.ToString()?.Trim();

                            if (string.IsNullOrEmpty(name))
                            {
                                throw new Exception("Product name is missing");
                            }

                            // Get product ID and sale price
                            string productidquery = "SELECT product_id, sale_price FROM medicines WHERE name = @name";
                            using (MySqlCommand command2 = new MySqlCommand(productidquery, con, tran))
                            {
                                command2.Parameters.AddWithValue("@name", name);
                                using (var reader = command2.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        productid = reader.GetInt32("product_id");
                                        int salePrice = reader.GetInt32("sale_price");
                                        reader.Close();

                                        // Parse and validate quantity
                                        if (!int.TryParse(row.Cells["quantity"]?.Value?.ToString(), out int qty) || qty <= 0)
                                        {
                                            throw new Exception($"Invalid quantity for product: {name}");
                                        }

                                        // Parse discount
                                        int discount = 0;
                                        if (row.Cells["discount"]?.Value != null)
                                        {
                                            if (!int.TryParse(row.Cells["discount"].Value.ToString(), out discount) || discount < 0)
                                            {
                                                throw new Exception($"Invalid discount for product: {name}");
                                            }
                                        }

                                        // Get expiry date
                                        DateTime expiry;
                                        if (row.Cells["expiry_date"]?.Value == null ||
                                            !DateTime.TryParse(row.Cells["expiry_date"].Value.ToString(), out expiry))
                                        {
                                            throw new Exception($"Invalid expiry date for product: {name}");
                                        }
                                        expiry = expiry.Date;

                                        // Get batch_item_id for the product with matching expiry date and available stock
                                        string batchItemQuery = @"SELECT batch_item_id 
                                                     FROM batch_items 
                                                     WHERE product_id = @product_id 
                                                     AND expiry_date = @expiry_date 
                                                     AND quantity_remaining >= @quantity 
                                                     ORDER BY expiry_date, batch_item_id 
                                                     LIMIT 1";

                                        int batchItemId = -1;
                                        using (MySqlCommand batchItemCmd = new MySqlCommand(batchItemQuery, con, tran))
                                        {
                                            batchItemCmd.Parameters.AddWithValue("@product_id", productid);
                                            batchItemCmd.Parameters.AddWithValue("@expiry_date", expiry);
                                            batchItemCmd.Parameters.AddWithValue("@quantity", qty);

                                            object batchResult = batchItemCmd.ExecuteScalar();
                                            if (batchResult == null)
                                            {
                                                throw new Exception($"No available batch found for product: {name} with expiry: {expiry:yyyy-MM-dd}");
                                            }
                                            batchItemId = Convert.ToInt32(batchResult);
                                        }

                                        // Insert into sale_items with batch_item_id
                                        string detailquery = @"INSERT INTO sale_items (sale_id, batch_item_id, quantity, price, Discount) 
                              VALUES (@bill_iid, @batch_item_id, @quantity, @price, @discount)";
                                        using (MySqlCommand command = new MySqlCommand(detailquery, con, tran))
                                        {
                                            command.Parameters.AddWithValue("@bill_iid", billid);
                                            command.Parameters.AddWithValue("@batch_item_id", batchItemId);
                                            command.Parameters.AddWithValue("@price", salePrice);
                                            command.Parameters.AddWithValue("@quantity", qty);
                                            command.Parameters.AddWithValue("@discount", discount);
                                            command.ExecuteNonQuery();
                                        }

                                        // Update batch stock
                                        string queryupdatequantity = @"UPDATE batch_items 
                                          SET quantity_remaining = quantity_remaining - @quantitysold 
                                          WHERE batch_item_id = @batch_item_id";

                                        using (MySqlCommand comma = new MySqlCommand(queryupdatequantity, con, tran))
                                        {
                                            comma.Parameters.AddWithValue("@batch_item_id", batchItemId);
                                            comma.Parameters.AddWithValue("@quantitysold", qty);

                                            int rowsAffected = comma.ExecuteNonQuery();
                                            if (rowsAffected == 0)
                                            {
                                                throw new Exception($"Failed to update stock for product: {name}");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception($"Product not found: {name}");
                                    }
                                }
                            }
                        }

                        tran.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            tran.Rollback();
                        }
                        catch (Exception rollbackEx)
                        {
                            // Log rollback error if needed
                            System.Diagnostics.Debug.WriteLine("Rollback failed: " + rollbackEx.Message);
                        }

                        // Log the error for debugging
                        System.Diagnostics.Debug.WriteLine("Sale save error: " + e.ToString());

                        // Re-throw with meaningful message
                        throw new Exception("Failed to save sale: " + e.Message, e);
                    }
                }
            }
        }

        public static Stream GetLogoImageStream()
        {
            var bytes = Properties.Resources.logo; // still byte[]

            using (var img = System.Drawing.Image.FromStream(new MemoryStream(bytes)))
            {
                var ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                ms.Position = 0;
                return ms;
            }
        }

        private static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return "";
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        //public static void CreateThermalReceiptPdf(DataGridView cart, string filePath, decimal total, decimal paid)
        //{
        //    QuestPDF.Settings.License = LicenseType.Community;

        //    Document.Create(container =>
        //    {
        //        container.Page(page =>
        //        {
        //            page.Size(226, PageSizes.A4.Height, Unit.Point); // 80mm width
        //            page.Margin(5);
        //            page.DefaultTextStyle(x => x.FontFamily("Consolas").FontSize(9));

        //            page.Content().Column(column =>
        //            {
        //                // --- Logo + Header ---
        //                column.Item().AlignCenter().Image(GetLogoImageStream(), ImageScaling.FitWidth);
        //                column.Item().AlignCenter().Text("MNS Computers").Bold().FontSize(12);
        //                column.Item().AlignCenter().Text("office # 39 & 40, 1st floor Gallery 3, Rex city, Sitiana Road");
        //                column.Item().AlignCenter().Text("Phone: 0300-6634245");
        //                column.Item().PaddingBottom(5).LineHorizontal(0.5f);

        //                // --- Invoice Info ---
        //                column.Item().Row(row =>
        //                {
        //                    row.RelativeItem().AlignRight().Text($"{DateTime.Now:dd-MMM-yyyy hh:mm tt}");
        //                });

        //                column.Item().PaddingBottom(5).LineHorizontal(0.5f);

        //                // --- Table Header ---
        //                column.Item().Text("----------------------------------------");
        //                column.Item().Text("ITEM         QTY PRICE DISC TOTAL");
        //                column.Item().Text("----------------------------------------");

        //                // --- Cart Items ---
        //                decimal totalDiscount = 0;
        //                decimal subTotal = 0;

        //                foreach (DataGridViewRow row in cart.Rows)
        //                {
        //                    if (row.IsNewRow) continue;

        //                    string name = row.Cells["name"].Value?.ToString() ?? "";
        //                    string qty = row.Cells["quantity"].Value?.ToString()?.PadLeft(2);
        //                    string price = row.Cells["total"].Value?.ToString()?.PadLeft(5);
        //                    string discount = row.Cells["discount"].Value?.ToString()?.PadLeft(3);
        //                    string totalPrice = row.Cells["final"].Value?.ToString()?.PadLeft(6);

        //                    if (decimal.TryParse(row.Cells["discount"].Value?.ToString(), out decimal discVal))
        //                        totalDiscount += discVal * Convert.ToInt32(row.Cells["quantity"].Value);
        //                    if (decimal.TryParse(row.Cells["total"].Value?.ToString(), out decimal itemTotal))
        //                        subTotal += itemTotal;

        //                    // Split name across lines
        //                    string[] nameParts = name.Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
        //                    string firstWord = nameParts.Length > 0 ? nameParts[0] : name;
        //                    string[] remainingWords = nameParts.Skip(1).ToArray();

        //                    // First line with first word and all data
        //                    string firstLine = $"{firstWord,-12}{qty} {price} {discount} {totalPrice}";
        //                    column.Item().Text(firstLine);

        //                    // Remaining words as new lines
        //                    foreach (var word in remainingWords)
        //                    {
        //                        column.Item().PaddingLeft(10).Text(word);
        //                    }
        //                }

        //                // --- Summary ---
        //                column.Item().Text("----------------------------------------");
        //                column.Item().Text($"SUBTOTAL:    Rs. {subTotal + totalDiscount:N0}");
        //                column.Item().Text($"DISCOUNT:    Rs. {totalDiscount:N0}");
        //                column.Item().Text($"TOTAL:       Rs. {total:N0}");
        //                column.Item().Text($"PAID:        Rs. {paid:N0}");
        //                column.Item().Text($"BALANCE:     Rs. {(total - paid):N0}");
        //                column.Item().Text("----------------------------------------");

        //                // --- Footer ---
        //                column.Item().AlignCenter().Text("Thank you for your shopping here!").Bold();
        //                column.Item().PaddingTop(5).LineHorizontal(0.5f);
        //                column.Item().AlignCenter().Text("** SPECIAL OFFERS **").Bold();
        //                column.Item().AlignCenter().Text("Free diagnostics with any repair");
        //                column.Item().AlignCenter().Text("10% discount on next purchase");
        //                column.Item().AlignCenter().Text("Ask about our warranty plans!");
        //                column.Item().AlignCenter().Text($"Invoice #: INV-{DateTime.Now:yyMMddHHmm}");
        //                column.Item().PaddingTop(5).AlignCenter().Text("Developed By:");
        //                column.Item().PaddingTop(5).AlignCenter().Text("abdulahad18022@gmail.com");
        //            });
        //        });
        //    }).GeneratePdf(filePath);
        //}



    }
}
