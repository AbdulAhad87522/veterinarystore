using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fertilizesop.BL.Bl;

namespace fertilizesop.UI
{
    public partial class FinanceReportForm : Form
    {
        private readonly FinanceReportBL reportBLL;
        private DataTable currentReportData;
        public FinanceReportForm()
        {
            InitializeComponent();
            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReport.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvReport.DefaultCellStyle.WrapMode = DataGridViewTriState.True;          
            dgvReport.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgvReport.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReport.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dgvReport.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            reportBLL = new FinanceReportBL();
            InitControls();
        }

        private void InitControls()
        {
            // Fill months with names
            comboMonth.Items.Clear();
            for (int m = 1; m <= 12; m++)
                comboMonth.Items.Add(new { MonthNumber = m, Name = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(m) });

            comboMonth.DisplayMember = "Name";
            comboMonth.ValueMember = "MonthNumber";
            comboMonth.SelectedIndex = DateTime.Today.Month - 1;

            // Fill years (example: 2020..next year)
            comboYear.Items.Clear();
            int startYear = 2020;
            int endYear = DateTime.Today.Year + 1;
            for (int y = startYear; y <= endYear; y++) comboYear.Items.Add(y);
            comboYear.SelectedItem = DateTime.Today.Year;

            radioMonthly.Checked = true;
            comboMonth.Enabled = true;
            // Add event handlers for radio buttons
            radioMonthly.CheckedChanged += RadioButton_CheckedChanged;
            radioYearly.CheckedChanged += RadioButton_CheckedChanged;
        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            comboMonth.Enabled = radioMonthly.Checked;
        }
        private void iconButton9_Click(object sender, EventArgs e)
        {
            int month = int.Parse(comboMonth.SelectedItem.ToString());
            int year = int.Parse(comboYear.SelectedItem.ToString());

            DataTable dt = reportBLL.GetMonthlyReport(month, year);
            dgvReport.DataSource = dt;
        }

        private void dgvReport_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                // Only format if this is yearly report and we have data
                if (!radioMonthly.Checked &&
                    e.ColumnIndex >= 0 &&
                    e.ColumnIndex < dgvReport.ColumnCount &&
                    dgvReport.Columns[e.ColumnIndex].Name == "Month" &&
                    e.Value != null)
                {
                    if (int.TryParse(e.Value.ToString(), out int monthNum) && monthNum >= 1 && monthNum <= 12)
                    {
                        e.Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthNum);
                        e.FormattingApplied = true;
                    }
                }
            }
            catch (Exception)
            {
                // Ignore formatting errors
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboYear.SelectedItem == null)
                {
                    MessageBox.Show("Please select a year.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int year = Convert.ToInt32(comboYear.SelectedItem);
                DataTable dt;

                if (radioMonthly.Checked)
                {
                    if (comboMonth.SelectedItem == null)
                    {
                        MessageBox.Show("Please select a month.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    dynamic msel = comboMonth.SelectedItem;
                    int month = (int)msel.MonthNumber;
                    dt = reportBLL.GetMonthlyReport(month, year);

                    // Format datagrid for monthly report
                    dgvReport.DataSource = dt;
                    dgvReport.CellFormatting -= dgvReport_CellFormatting; // Remove formatting for monthly

                    if (dgvReport.Columns.Contains("Date"))
                    {
                        dgvReport.Columns["Date"].HeaderText = "Date";
                        dgvReport.Columns["Date"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    }
                }
                else
                {
                    dt = reportBLL.GetYearlyReport(year);
                    dgvReport.DataSource = dt;

                    // Format month column for yearly report
                    if (dgvReport.Columns.Contains("Month"))
                    {
                        dgvReport.Columns["Month"].HeaderText = "Month";

                        // Handle the CellFormatting event to display month names
                        dgvReport.CellFormatting -= dgvReport_CellFormatting; // Remove existing handler first
                        dgvReport.CellFormatting += dgvReport_CellFormatting;
                    }
                }

                // Store current data for PDF generation
                currentReportData = dt;

                // Format numeric columns
                foreach (DataGridViewColumn col in dgvReport.Columns)
                {
                    if (col.Name == "TotalSales" || col.Name == "TotalExpenses" || col.Name == "Profit")
                    {
                        col.DefaultCellStyle.Format = "N2";
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        col.HeaderText = col.Name == "TotalSales" ? "Total Sales" :
                                       col.Name == "TotalExpenses" ? "Total Expenses" : "Profit";
                    }
                }

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No data found for the selected period.", "Information",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating report: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentReportData == null || currentReportData.Rows.Count == 0)
                {
                    MessageBox.Show("No report data to export. Please generate a report first.",
                                  "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveFileDialog.Title = "Save Finance Report";

                string defaultFileName = "";
                if (radioMonthly.Checked)
                {
                    dynamic msel = comboMonth.SelectedItem;
                    int month = (int)msel.MonthNumber;
                    int year = Convert.ToInt32(comboYear.SelectedItem);
                    defaultFileName = $"Monthly_Report_{month:00}_{year}.pdf";
                }
                else
                {
                    int year = Convert.ToInt32(comboYear.SelectedItem);
                    defaultFileName = $"Yearly_Report_{year}.pdf";
                }

                saveFileDialog.FileName = defaultFileName;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (radioMonthly.Checked)
                    {
                        dynamic msel = comboMonth.SelectedItem;
                        int month = (int)msel.MonthNumber;
                        int year = Convert.ToInt32(comboYear.SelectedItem);
                        reportBLL.GenerateMonthlyReportPDF(currentReportData, saveFileDialog.FileName, month, year);
                    }
                    else
                    {
                        int year = Convert.ToInt32(comboYear.SelectedItem);
                        reportBLL.GenerateYearlyReportPDF(currentReportData, saveFileDialog.FileName, year);
                    }

                    MessageBox.Show("PDF report generated successfully!", "Success",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating PDF: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
