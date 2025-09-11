using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using KIMS;
using MySql.Data.MySqlClient;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;



namespace fertilizesop.DL
{
    internal class FinanceReportDAL
    {
        private MySqlConnection conn => DatabaseHelper.Instance.GetConnection();
        public DataTable GetMonthlyReport(int month, int year)
        {
            DataTable dt = new DataTable();

            using (var connection = conn)
            {
                string query = @"
    SELECT 
        dates.bill_date AS Date,
        COALESCE(SUM(cb.total_price), 0) AS TotalSales,
        COALESCE(SUM(sb.total_price), 0) AS TotalExpenses,
        (COALESCE(SUM(cb.total_price), 0) - COALESCE(SUM(sb.total_price), 0)) AS Profit
    FROM (
        SELECT DISTINCT DATE(SaleDate) as bill_date 
        FROM customerbills 
        WHERE MONTH(SaleDate) = @month AND YEAR(SaleDate) = @year
        UNION 
        SELECT DISTINCT DATE(Date) as bill_date 
        FROM supplierbills 
        WHERE MONTH(Date) = @month AND YEAR(Date) = @year
    ) dates
    LEFT JOIN customerbills cb ON DATE(cb.SaleDate) = dates.bill_date
    LEFT JOIN supplierbills sb ON DATE(sb.Date) = dates.bill_date
    GROUP BY dates.bill_date
    ORDER BY dates.bill_date ASC;
";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);

                connection.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }

        public DataTable GetYearlyReport(int year)
        {
            DataTable dt = new DataTable();

            using (var connection = conn)
            {
                string query = @"
    SELECT 
        MONTH(dates.month_date) AS Month,
        COALESCE(SUM(cb.total_price), 0) AS TotalSales,
        COALESCE(SUM(sb.total_price), 0) AS TotalExpenses,
        (COALESCE(SUM(cb.total_price), 0) - COALESCE(SUM(sb.total_price), 0)) AS Profit
    FROM (
        SELECT DISTINCT DATE(CONCAT(@year, '-', MONTH(SaleDate), '-01')) as month_date
        FROM customerbills 
        WHERE YEAR(SaleDate) = @year
        UNION 
        SELECT DISTINCT DATE(CONCAT(@year, '-', MONTH(Date), '-01')) as month_date
        FROM supplierbills 
        WHERE YEAR(Date) = @year
    ) dates
    LEFT JOIN customerbills cb ON MONTH(cb.SaleDate) = MONTH(dates.month_date) 
        AND YEAR(cb.SaleDate) = @year
    LEFT JOIN supplierbills sb ON MONTH(sb.Date) = MONTH(dates.month_date) 
        AND YEAR(sb.Date) = @year
    GROUP BY MONTH(dates.month_date)
    ORDER BY MONTH(dates.month_date) ASC;
";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@year", year);

                connection.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }

        public (decimal customerDue, decimal supplierDue) GetDueAmounts()
        {
            decimal customerDue = 0;
            decimal supplierDue = 0;

            using (var connection = conn)
            {
                connection.Open();

                // Customer Due
                using (var cmd = new MySqlCommand(
                    "SELECT IFNULL(SUM(paid_amount), 0) FROM customerbills WHERE payment_status = 'Due'",
                    connection))
                {
                    customerDue = Convert.ToDecimal(cmd.ExecuteScalar());
                }

                // Supplier Due
                using (var cmd = new MySqlCommand(
                    "SELECT IFNULL(SUM(paid_amount), 0) FROM supplierbills WHERE payment_status = 'Due'",
                    connection))
                {
                    supplierDue = Convert.ToDecimal(cmd.ExecuteScalar());
                }
            }

            return (customerDue, supplierDue);
        }


        // PDF Generation for Monthly
        public void GenerateMonthlyReportPDF(DataTable reportData, string filePath, int month, int year)
        {
            string title = $"Monthly Finance Report - {month}/{year}";
            DrawFinanceReportPDF(reportData, filePath, title);
        }

        // PDF Generation for Yearly
        public void GenerateYearlyReportPDF(DataTable reportData, string filePath, int year)
        {
            string title = $"Yearly Finance Report - {year}";
            DrawFinanceReportPDF(reportData, filePath, title);
        }

        public void DrawFinanceReportPDF(DataTable dt, string filePath, string title)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            decimal totalSales = 0, totalExpenses = 0, totalProfit = 0;

            foreach (DataRow row in dt.Rows)
            {
                totalSales += Convert.ToDecimal(row["TotalSales"]);
                totalExpenses += Convert.ToDecimal(row["TotalExpenses"]);
                totalProfit += Convert.ToDecimal(row["Profit"]);
            }

            // ✅ Get Due Amounts from DB
            var (customerDue, supplierDue) = GetDueAmounts();

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    // HEADER
                    page.Header().Column(col =>
                    {
                        col.Item().AlignCenter().Text("Jamal's shop").FontSize(18).Bold();
                        col.Item().AlignCenter().Text("HaroonAbad Sanghera 34").FontSize(12);
                        col.Item()
                            .PaddingTop(10f)
                            .PaddingBottom(12) // 👈 extra space
                            .AlignCenter()
                            .Text(title).FontSize(16).Bold();
                    });

                    // CONTENT
                    page.Content().Column(content =>
                    {
                        // ✅ Due amounts side-by-side
                        content.Item().Row(row =>
                        {
                            row.RelativeItem().Text($"Total Customer Due: {customerDue:F2}")
                                .FontSize(13).Bold().FontColor(Colors.Red.Darken1);

                            row.RelativeItem().AlignRight().Text($"Total Supplier Due: {supplierDue:F2}")
                                .FontSize(13).Bold().FontColor(Colors.Orange.Darken1);
                        });

                        // Thin separator
                        // Thin separator with padding
                        content.Item().PaddingBottom(5).Height(1).Background(Colors.Grey.Lighten2);

                        // Finance Table
                        content.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                foreach (DataColumn col in dt.Columns)
                                    columns.RelativeColumn();
                            });

                            // Table header
                            table.Header(header =>
                            {
                                foreach (DataColumn col in dt.Columns)
                                {
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5)
                                          .AlignCenter().Text(col.ColumnName).Bold();
                                }
                            });

                            // Table rows
                            foreach (DataRow row in dt.Rows)
                            {
                                foreach (var item in row.ItemArray)
                                {
                                    table.Cell().Padding(5).AlignCenter().Text(item?.ToString() ?? "");
                                }
                            }

                            // Totals row
                            table.Cell()
                                .ColumnSpan((uint)Math.Max(1, dt.Columns.Count - 3))
                                .Padding(5)
                                .AlignRight()
                                .Text("TOTAL").FontSize(13).Bold();

                            table.Cell().AlignCenter().Padding(5).Text(totalSales.ToString("F2")).FontSize(13).Bold();
                            table.Cell().AlignCenter().Padding(5).Text(totalExpenses.ToString("F2")).FontSize(13).Bold();
                            table.Cell().AlignCenter().Padding(5).Text(totalProfit.ToString("F2")).FontSize(13).Bold();
                        });
                    });

                    // FOOTER
                    page.Footer().AlignRight()
                        .Text($"Generated on: {DateTime.Now:dd/MM/yyyy HH:mm}")
                        .FontSize(10).Italic();
                });
            })
            .GeneratePdf(filePath);
        }

    }
}

