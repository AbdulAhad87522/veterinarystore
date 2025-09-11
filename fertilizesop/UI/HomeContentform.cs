using fertilizesop.BL.Bl;
using fertilizesop.BL.Models;
using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace fertilizesop.UI
{
    public partial class HomeContentform : Form
    {
        private readonly IdashboardBl ibl;
        public HomeContentform(IdashboardBl ibl)
        {
            InitializeComponent();
            this.ibl = ibl;
            UIHelper.StyleGridView(dataGridView1);
            UIHelper.StyleGridView(dataGridView2);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
        private void loadgrid()
        {
            var list = ibl.recentlogs();
            dataGridView1.DataSource = list;
            dataGridView1.Columns["log_date"].Visible = false;
            dataGridView1.Columns["remark"].Visible = false;

            var lists = ibl.outofstock();
            dataGridView2.DataSource = lists;
            dataGridView2.Columns["id"].Visible = false;


        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label9.Text = DateTime.Now.ToString("hh:mm:ss tt");

            // Determine greeting based on time
            var hour = DateTime.Now.Hour;
            string greeting;

            if (hour >= 5 && hour < 12)
                greeting = "Good Morning";
            else if (hour >= 12 && hour < 17)
                greeting = "Good Afternoon";
            else if (hour >= 17 && hour < 21)
                greeting = "Good Evening";
            else
                greeting = "Good Night";

            // Get user's name from session
            string name = Usersession.FullName ?? "Nadir Jamal";

            label10.Text = $"{greeting}, {name}";
        }

        private void HomeContentform_Load(object sender, EventArgs e)
        {
            timer1.Start();
            load();
            LoadChartData();
            LoadCompareChart();
            LoadSupplierChart();
            loadgrid();
            LoadTopSellingProductsChart();

        }
        private void LoadChartData()
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            chart1.Titles.Clear();
            chart1.Legends.Clear();

            chart1.BackColor = Color.Transparent;

            var area = new ChartArea("SalesArea")
            {
                BackColor = Color.FromArgb(24, 161, 68),
            };
            area.AxisX.MajorGrid.LineColor = Color.WhiteSmoke;
            area.AxisY.MajorGrid.LineColor = Color.WhiteSmoke;
            area.AxisX.LabelStyle.ForeColor = Color.White;
            area.AxisY.LabelStyle.ForeColor = Color.White;
            area.AxisX.LineColor = Color.White;
            area.AxisY.LineColor = Color.White;
            area.AxisX.Title = "Date";
            area.AxisY.Title = "Total Sales (Rs)";
            area.AxisX.TitleFont = new Font("Segoe UI", 10, FontStyle.Bold);
            area.AxisY.TitleFont = new Font("Segoe UI", 10, FontStyle.Bold);
            area.AxisX.TitleForeColor = Color.White;
            area.AxisY.TitleForeColor = Color.White;
            area.AxisX.LabelStyle.Angle = -45;
            chart1.ChartAreas.Add(area);

            var legend = new Legend("SalesLegend")
            {
                Docking = Docking.Top,
                Alignment = StringAlignment.Center,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.Black
            };
            chart1.Legends.Add(legend);

            var series = new Series("Daily Sales")
            {
                ChartType = SeriesChartType.Spline,
                BorderWidth = 3,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 6,
                Color = Color.FromArgb(255, 214, 102), // Light Yellow-Orange
                MarkerColor = Color.White,
                XValueType = ChartValueType.String,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                IsValueShownAsLabel = false
            };
            chart1.Series.Add(series);

            chart1.Titles.Add("Sales Trend - Current Month");
            chart1.Titles[0].Font = new Font("Segoe UI", 13, FontStyle.Bold);
            chart1.Titles[0].ForeColor = Color.White;

            var salesData = ibl.GetMonthlySalesTrend();
            foreach (var entry in salesData)
            {
                int pointIndex = series.Points.AddXY(entry.Day.ToString("dd MMM"), entry.TotalSales);
                series.Points[pointIndex].ToolTip = $"{entry.Day:dd MMM}: Rs {entry.TotalSales:N0}";
            }
        }


        private void LoadCompareChart()
        {
            chart2.Series.Clear();
            chart2.ChartAreas.Clear();
            chart2.Titles.Clear();
            chart2.Legends.Clear();

            chart2.BackColor = Color.Transparent;

            var area = new ChartArea("CompareArea")
            {
                BackColor = Color.FromArgb(24, 161, 68)
            };
            area.AxisX.MajorGrid.LineColor = Color.WhiteSmoke;
            area.AxisY.MajorGrid.LineColor = Color.WhiteSmoke;
            area.AxisX.LineColor = Color.White;
            area.AxisY.LineColor = Color.White;
            area.AxisX.LabelStyle.ForeColor = Color.White;
            area.AxisY.LabelStyle.ForeColor = Color.White;
            area.AxisX.Title = "Month";
            area.AxisY.Title = "Sales (Rs)";
            area.AxisX.TitleFont = new Font("Segoe UI", 10, FontStyle.Bold);
            area.AxisY.TitleFont = new Font("Segoe UI", 10, FontStyle.Bold);
            area.AxisX.TitleForeColor = Color.White;
            area.AxisY.TitleForeColor = Color.White;
            area.AxisX.LabelStyle.Angle = -45;
            chart2.ChartAreas.Add(area);

            var legend = new Legend("CompareLegend")
            {
                Docking = Docking.Top,
                Alignment = StringAlignment.Center,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.Black
            };
            chart2.Legends.Add(legend);

            var series = new Series("Monthly Sales")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(102, 217, 255), // Cyan
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                IsValueShownAsLabel = true,
                XValueType = ChartValueType.String,
                LabelForeColor = Color.Black
            };
            chart2.Series.Add(series);

            chart2.Titles.Add("Monthly Sales - Current Year");
            chart2.Titles[0].Font = new Font("Segoe UI", 13, FontStyle.Bold);
            chart2.Titles[0].ForeColor = Color.White;

            var data = ibl.GetMonthlySalesComparison();
            foreach (var entry in data)
            {
                var point = series.Points.AddXY(entry.MonthName, entry.TotalSales);
                series.Points[point].ToolTip = $"{entry.MonthName}: Rs {entry.TotalSales:N0}";
            }
        }
        private void LoadTopSellingProductsChart()
        {
            chart6.Series.Clear();
            chart6.ChartAreas.Clear();
            chart6.Titles.Clear();
            chart6.Legends.Clear();

            chart6.BackColor = Color.Transparent;

            var area = new ChartArea("TopProductsArea")
            {
                BackColor = Color.FromArgb(24, 161, 68)
            };
            area.AxisX.MajorGrid.LineColor = Color.WhiteSmoke;
            area.AxisY.MajorGrid.LineColor = Color.WhiteSmoke;
            area.AxisX.LineColor = Color.White;
            area.AxisY.LineColor = Color.White;
            area.AxisX.LabelStyle.ForeColor = Color.White;
            area.AxisY.LabelStyle.ForeColor = Color.White;
            area.AxisX.Title = "Quantity Sold";
            area.AxisY.Title = "Product Name";
            area.AxisX.TitleFont = new Font("Segoe UI", 10, FontStyle.Bold);
            area.AxisY.TitleFont = new Font("Segoe UI", 10, FontStyle.Bold);
            area.AxisX.TitleForeColor = Color.White;
            area.AxisY.TitleForeColor = Color.White;
            area.AxisY.LabelStyle.Angle = 0;
            chart6.ChartAreas.Add(area);

            var legend = new Legend("TopProductsLegend")
            {
                Docking = Docking.Top,
                Alignment = StringAlignment.Center,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.Black
            };
            chart6.Legends.Add(legend);

            var series = new Series("Top Selling Products")
            {
                ChartType = SeriesChartType.Bar,
                Color = Color.FromArgb(0, 191, 255), // DeepSkyBlue
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                IsValueShownAsLabel = true,
                XValueType = ChartValueType.String,
                LabelForeColor = Color.White
            };
            chart6.Series.Add(series);

            chart6.Titles.Add("Top 10 Selling Products (This Month)");
            chart6.Titles[0].Font = new Font("Segoe UI", 13, FontStyle.Bold);
            chart6.Titles[0].ForeColor = Color.White;

            // Get data from BL
            var topProducts = ibl.gettopsellingproducts(); // List<(string ProductName, int QuantitySold)>

            foreach (var product in topProducts)
            {
                int pointIndex = series.Points.AddXY(product.name, product.quantity);
                series.Points[pointIndex].ToolTip = $"{product.name}: {product.quantity} units";
            }
        }

        private void LoadSupplierChart()
        {
            chart3.Series.Clear();
            chart3.ChartAreas.Clear();
            chart3.Titles.Clear();
            chart3.Legends.Clear();

            chart3.BackColor = Color.Transparent;

            var area = new ChartArea("SupplierArea")
            {
                BackColor = Color.FromArgb(24, 161, 68)
            };
            area.AxisX.MajorGrid.LineColor = Color.WhiteSmoke;
            area.AxisY.MajorGrid.LineColor = Color.WhiteSmoke;
            area.AxisX.LineColor = Color.White;
            area.AxisY.LineColor = Color.White;
            area.AxisX.LabelStyle.ForeColor = Color.White;
            area.AxisY.LabelStyle.ForeColor = Color.White;
            area.AxisX.Title = "Supplier";
            area.AxisY.Title = "Batches Supplied";
            area.AxisX.TitleFont = new Font("Segoe UI", 10, FontStyle.Bold);
            area.AxisY.TitleFont = new Font("Segoe UI", 10, FontStyle.Bold);
            area.AxisX.TitleForeColor = Color.White;
            area.AxisY.TitleForeColor = Color.White;
            area.AxisX.LabelStyle.Angle = -45;
            chart3.ChartAreas.Add(area);

            var legend = new Legend("SupplierLegend")
            {
                Docking = Docking.Top,
                Alignment = StringAlignment.Center,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.Black
            };
            chart3.Legends.Add(legend);

            var series = new Series("Top Suppliers")
            {
                ChartType = SeriesChartType.Bar,
                Color = Color.FromArgb(255, 193, 7), // Amber
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                IsValueShownAsLabel = true,
                XValueType = ChartValueType.String,
                LabelForeColor = Color.Black
            };
            chart3.Series.Add(series);

            chart3.Titles.Add("Top Supplier Contributions");
            chart3.Titles[0].Font = new Font("Segoe UI", 13, FontStyle.Bold);
            chart3.Titles[0].ForeColor = Color.White;

            var data = ibl.GetTopSupplierContributions();
            foreach (var entry in data)
            {
                var point = series.Points.AddXY(entry.SupplierName, entry.TotalBatches);
                series.Points[point].ToolTip = $"{entry.SupplierName}: {entry.TotalBatches} batches";
            }
        }


        private void load()
        {
            lbltotalp.Text=ibl.GetDashboardSummary().totalproducts.ToString();
            lblcustomers.Text=ibl.GetDashboardSummary().totalcustomers.ToString();
            lblsupp.Text=ibl.GetDashboardSummary().totalsuppliers.ToString();
            lblsales.Text=ibl.GetDashboardSummary().salestodays.ToString();
            lblout.Text=ibl.GetDashboardSummary().outproduct.ToString();
            lbltstock.Text=ibl.GetDashboardSummary().total_stock.ToString();
            lblbills.Text=ibl.GetDashboardSummary().pendingbills.ToString();
            lblstockvalue.Text=ibl.GetDashboardSummary().total_stock_value.ToString();

        }

    }
}
