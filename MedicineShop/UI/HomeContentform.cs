using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TechStore.BusinessLogic;
using TechStore.Models;
using TechStore.Interfaces;

namespace MedicineShop.UI
{
    public partial class HomeContentform : Form
    {
        private readonly IDashboardService _dashboardService;
        private System.Windows.Forms.Timer refreshTimer;

        // Summary Cards Controls
        private Panel summaryPanel;
        private Label lblTotalProducts, lblTotalCompanies, lblLowStock, lblExpiringItems;
        private Label lblTodaySales, lblTodayRevenue, lblPendingPayments, lblInventoryValue;

        // Data Grid Controls
        private DataGridView dgvLowStock, dgvExpiringItems, dgvPendingPurchases;
        private Panel lowStockPanel, expiringPanel, purchasesPanel;

        // Additional UI Components
        private Panel mainContentPanel;
        private Label lblWelcome, lblDateTime;

        public HomeContentform()
        {
            InitializeComponent();
            _dashboardService = new DashboardService();
            InitializeDashboard();
            SetupRefreshTimer();
            LoadDashboardData();
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

        private void InitializeDashboard()
        {
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(240, 244, 247);

            // Clear existing content
            panel4.Controls.Clear();
            panel4.AutoScroll = true;

            CreateWelcomeSection();
            CreateSummaryCards();
            CreateDataGridPanels();
            CreateAdditionalInfoPanel();
            ArrangeLayout();

            // Handle resize event
            this.Resize += HomeContentform_Resize;
            panel4.Resize += Panel4_Resize;
        }

        private void CreateWelcomeSection()
        {
            var welcomePanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(20, 10, 20, 10)
            };

            lblWelcome = new Label
            {
                Text = "Pharmacy Management Dashboard",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            lblDateTime = new Label
            {
                Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy - hh:mm tt"),
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(127, 140, 141),
                AutoSize = true,
                Location = new Point(20, 35)
            };

            welcomePanel.Controls.Add(lblWelcome);
            welcomePanel.Controls.Add(lblDateTime);
            panel4.Controls.Add(welcomePanel);
        }

        private void CreateSummaryCards()
        {
            summaryPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 160,
                BackColor = Color.Transparent,
                Padding = new Padding(10, 10, 10, 20)
            };

            // Calculate responsive card dimensions
            var availableWidth = panel4.Width - 40; // Subtract padding
            var cardWidth = (availableWidth - 30) / 4; // 4 cards per row with spacing
            var cardHeight = 65;
            var spacing = 10;

            // Row 1 Cards
            CreateSummaryCard("Total Products", "0", Color.FromArgb(52, 152, 219), 0, 0, cardWidth, cardHeight, lblTotalProducts = new Label());
            CreateSummaryCard("Companies", "0", Color.FromArgb(46, 204, 113), cardWidth + spacing, 0, cardWidth, cardHeight, lblTotalCompanies = new Label());
            CreateSummaryCard("Low Stock Items", "0", Color.FromArgb(231, 76, 60), (cardWidth + spacing) * 2, 0, cardWidth, cardHeight, lblLowStock = new Label());
            CreateSummaryCard("Expiring Soon", "0", Color.FromArgb(243, 156, 18), (cardWidth + spacing) * 3, 0, cardWidth, cardHeight, lblExpiringItems = new Label());

            // Row 2 Cards
            CreateSummaryCard("Today's Sales", "0", Color.FromArgb(155, 89, 182), 0, cardHeight + spacing, cardWidth, cardHeight, lblTodaySales = new Label());
            CreateSummaryCard("Today's Revenue", "₹0", Color.FromArgb(52, 73, 94), cardWidth + spacing, cardHeight + spacing, cardWidth, cardHeight, lblTodayRevenue = new Label());
            CreateSummaryCard("Pending Payments", "₹0", Color.FromArgb(230, 126, 34), (cardWidth + spacing) * 2, cardHeight + spacing, cardWidth, cardHeight, lblPendingPayments = new Label());
            CreateSummaryCard("Inventory Value", "₹0", Color.FromArgb(22, 160, 133), (cardWidth + spacing) * 3, cardHeight + spacing, cardWidth, cardHeight, lblInventoryValue = new Label());

            panel4.Controls.Add(summaryPanel);
        }

        private void CreateSummaryCard(string title, string value, Color bgColor, int x, int y, int width, int height, Label valueLabel)
        {
            var card = new Panel
            {
                Size = new Size(width, height),
                Location = new Point(x + 10, y + 10),
                BackColor = bgColor,
                BorderStyle = BorderStyle.None,
                Cursor = Cursors.Hand
            };

            // Add rounded corners effect
            card.Paint += (s, e) =>
            {
                var rect = new Rectangle(0, 0, card.Width - 1, card.Height - 1);
                using (var path = GetRoundedRectanglePath(rect, 8))
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    using (var brush = new SolidBrush(bgColor))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }
            };

            // Add hover effect
            card.MouseEnter += (s, e) => { card.BackColor = ChangeColorBrightness(bgColor, -0.1f); card.Invalidate(); };
            card.MouseLeave += (s, e) => { card.BackColor = bgColor; card.Invalidate(); };

            var titleLabel = new Label
            {
                Text = title,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                Location = new Point(15, 12),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            valueLabel.Text = value;
            valueLabel.ForeColor = Color.White;
            valueLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            valueLabel.Location = new Point(15, 32);
            valueLabel.AutoSize = true;
            valueLabel.BackColor = Color.Transparent;

            card.Controls.Add(titleLabel);
            card.Controls.Add(valueLabel);
            summaryPanel.Controls.Add(card);
        }

        private System.Drawing.Drawing2D.GraphicsPath GetRoundedRectanglePath(Rectangle rect, int cornerRadius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(rect.X, rect.Y, cornerRadius, cornerRadius, 180, 90);
            path.AddArc(rect.X + rect.Width - cornerRadius, rect.Y, cornerRadius, cornerRadius, 270, 90);
            path.AddArc(rect.X + rect.Width - cornerRadius, rect.Y + rect.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
            path.AddArc(rect.X, rect.Y + rect.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
            path.CloseAllFigures();
            return path;
        }

        private Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = color.R;
            float green = color.G;
            float blue = color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }

        private void CreateDataGridPanels()
        {
            var panelsContainer = new Panel
            {
                Dock = DockStyle.Top,
                Height = 320,
                BackColor = Color.Transparent,
                Padding = new Padding(10)
            };

            var availableWidth = panel4.Width - 40;
            var panelWidth = (availableWidth - 20) / 3;
            var panelHeight = 300;

            // Low Stock Panel
            lowStockPanel = CreateDataPanel("⚠️ Low Stock Items", 10, 0, panelWidth, panelHeight, Color.FromArgb(231, 76, 60));
            dgvLowStock = CreateDataGrid(lowStockPanel);
            SetupLowStockGrid();

            // Expiring Items Panel
            expiringPanel = CreateDataPanel("⏰ Items Expiring Soon", panelWidth + 20, 0, panelWidth, panelHeight, Color.FromArgb(243, 156, 18));
            dgvExpiringItems = CreateDataGrid(expiringPanel);
            SetupExpiringItemsGrid();

            // Pending Purchases Panel
            purchasesPanel = CreateDataPanel("💰 Pending Purchases", (panelWidth * 2) + 30, 0, panelWidth, panelHeight, Color.FromArgb(155, 89, 182));
            dgvPendingPurchases = CreateDataGrid(purchasesPanel);
            SetupPendingPurchasesGrid();

            panelsContainer.Controls.Add(lowStockPanel);
            panelsContainer.Controls.Add(expiringPanel);
            panelsContainer.Controls.Add(purchasesPanel);

            panel4.Controls.Add(panelsContainer);
        }

        private Panel CreateDataPanel(string title, int x, int y, int width, int height, Color headerColor)
        {
            var panel = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            // Add shadow effect
            panel.Paint += (s, e) =>
            {
                var shadowRect = new Rectangle(2, 2, panel.Width - 2, panel.Height - 2);
                using (var shadowBrush = new SolidBrush(Color.FromArgb(30, 0, 0, 0)))
                {
                    e.Graphics.FillRectangle(shadowBrush, shadowRect);
                }

                var mainRect = new Rectangle(0, 0, panel.Width - 2, panel.Height - 2);
                e.Graphics.FillRectangle(Brushes.White, mainRect);
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(230, 230, 230)), mainRect);
            };

            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = headerColor
            };

            var titleLabel = new Label
            {
                Text = title,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Padding = new Padding(15, 0, 0, 0)
            };

            headerPanel.Controls.Add(titleLabel);
            panel.Controls.Add(headerPanel);

            return panel;
        }

        private DataGridView CreateDataGrid(Panel parent)
        {
            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 9F),
                GridColor = Color.FromArgb(240, 240, 240),
                ColumnHeadersHeight = 35,
                RowTemplate = { Height = 28 },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(64, 64, 64),
                    SelectionBackColor = Color.FromArgb(51, 122, 183),
                    SelectionForeColor = Color.White,
                    Padding = new Padding(8, 4, 8, 4),
                    Alignment = DataGridViewContentAlignment.MiddleLeft
                },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(248, 249, 250),
                    ForeColor = Color.FromArgb(73, 80, 87),
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Padding = new Padding(8, 4, 8, 4)
                },
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(248, 249, 250)
                }
            };

            // Add container with padding
            var containerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(1, 0, 1, 1),
                BackColor = Color.White
            };

            containerPanel.Controls.Add(dgv);
            parent.Controls.Add(containerPanel);

            return dgv;
        }

        private void SetupLowStockGrid()
        {
            dgvLowStock.Columns.Add("Name", "Product Name");
            dgvLowStock.Columns.Add("Company", "Company");
            dgvLowStock.Columns.Add("Stock", "Stock");
            dgvLowStock.Columns.Add("Status", "Status");

            dgvLowStock.Columns["Stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvLowStock.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvLowStock.Columns["Stock"].Width = 60;
            dgvLowStock.Columns["Status"].Width = 80;

            // Color coding for stock status
            dgvLowStock.CellFormatting += (s, e) =>
            {
                if (e.ColumnIndex == dgvLowStock.Columns["Status"].Index && e.Value != null)
                {
                    string status = e.Value.ToString();
                    switch (status)
                    {
                        case "OUT_OF_STOCK":
                            e.CellStyle.BackColor = Color.FromArgb(255, 235, 238);
                            e.CellStyle.ForeColor = Color.FromArgb(220, 53, 69);
                            e.CellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
                            break;
                        case "CRITICAL":
                            e.CellStyle.BackColor = Color.FromArgb(255, 243, 205);
                            e.CellStyle.ForeColor = Color.FromArgb(255, 193, 7);
                            e.CellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
                            break;
                        case "LOW":
                            e.CellStyle.BackColor = Color.FromArgb(254, 247, 203);
                            e.CellStyle.ForeColor = Color.FromArgb(255, 235, 59);
                            e.CellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
                            break;
                    }
                }

                if (e.ColumnIndex == dgvLowStock.Columns["Stock"].Index && e.Value != null)
                {
                    if (int.TryParse(e.Value.ToString(), out int stock))
                    {
                        if (stock == 0)
                            e.CellStyle.ForeColor = Color.FromArgb(220, 53, 69);
                        else if (stock <= 5)
                            e.CellStyle.ForeColor = Color.FromArgb(255, 193, 7);
                    }
                }
            };
        }

        private void SetupExpiringItemsGrid()
        {
            dgvExpiringItems.Columns.Add("Name", "Product Name");
            dgvExpiringItems.Columns.Add("Company", "Company");
            dgvExpiringItems.Columns.Add("Quantity", "Qty");
            dgvExpiringItems.Columns.Add("ExpiryDate", "Expiry Date");
            dgvExpiringItems.Columns.Add("DaysLeft", "Days Left");

            dgvExpiringItems.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvExpiringItems.Columns["DaysLeft"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvExpiringItems.Columns["ExpiryDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvExpiringItems.Columns["Quantity"].Width = 50;
            dgvExpiringItems.Columns["ExpiryDate"].Width = 80;
            dgvExpiringItems.Columns["DaysLeft"].Width = 70;

            // Color coding for days left
            dgvExpiringItems.CellFormatting += (s, e) =>
            {
                if (e.ColumnIndex == dgvExpiringItems.Columns["DaysLeft"].Index && e.Value != null)
                {
                    if (int.TryParse(e.Value.ToString(), out int days))
                    {
                        if (days <= 7)
                        {
                            e.CellStyle.BackColor = Color.FromArgb(255, 235, 238);
                            e.CellStyle.ForeColor = Color.FromArgb(220, 53, 69);
                            e.CellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
                        }
                        else if (days <= 15)
                        {
                            e.CellStyle.BackColor = Color.FromArgb(255, 243, 205);
                            e.CellStyle.ForeColor = Color.FromArgb(255, 193, 7);
                            e.CellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
                        }
                        else if (days <= 30)
                        {
                            e.CellStyle.BackColor = Color.FromArgb(254, 247, 203);
                            e.CellStyle.ForeColor = Color.FromArgb(255, 235, 59);
                        }
                    }
                }
            };
        }

        private void SetupPendingPurchasesGrid()
        {
            dgvPendingPurchases.Columns.Add("BatchName", "Batch Name");
            dgvPendingPurchases.Columns.Add("Company", "Company");
            dgvPendingPurchases.Columns.Add("TotalPrice", "Total");
            dgvPendingPurchases.Columns.Add("Paid", "Paid");
            dgvPendingPurchases.Columns.Add("Remaining", "Remaining");

            dgvPendingPurchases.Columns["TotalPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPendingPurchases.Columns["Paid"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPendingPurchases.Columns["Remaining"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvPendingPurchases.Columns["TotalPrice"].DefaultCellStyle.Format = "₹#,##0";
            dgvPendingPurchases.Columns["Paid"].DefaultCellStyle.Format = "₹#,##0";
            dgvPendingPurchases.Columns["Remaining"].DefaultCellStyle.Format = "₹#,##0";

            dgvPendingPurchases.Columns["TotalPrice"].Width = 80;
            dgvPendingPurchases.Columns["Paid"].Width = 80;
            dgvPendingPurchases.Columns["Remaining"].Width = 80;

            // Color code remaining amounts
            dgvPendingPurchases.CellFormatting += (s, e) =>
            {
                if (e.ColumnIndex == dgvPendingPurchases.Columns["Remaining"].Index && e.Value != null)
                {
                    if (decimal.TryParse(e.Value.ToString(), out decimal remaining))
                    {
                        if (remaining > 100000) // High pending amount
                        {
                            e.CellStyle.BackColor = Color.FromArgb(255, 235, 238);
                            e.CellStyle.ForeColor = Color.FromArgb(220, 53, 69);
                            e.CellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
                        }
                        else if (remaining > 50000)
                        {
                            e.CellStyle.BackColor = Color.FromArgb(255, 243, 205);
                            e.CellStyle.ForeColor = Color.FromArgb(255, 193, 7);
                        }
                    }
                }
            };
        }

        private void CreateAdditionalInfoPanel()
        {
            var infoPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(20, 15, 20, 15)
            };

            var refreshBtn = new Button
            {
                Text = "🔄 Refresh Dashboard",
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(150, 35),
                Location = new Point(20, 12),
                Cursor = Cursors.Hand
            };
            refreshBtn.FlatAppearance.BorderSize = 0;
            refreshBtn.Click += (s, e) => RefreshDashboard();

            var lastUpdateLabel = new Label
            {
                Text = $"Last Updated: {DateTime.Now:HH:mm:ss}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(108, 117, 125),
                AutoSize = true,
                Location = new Point(200, 18)
            };

            infoPanel.Controls.Add(refreshBtn);
            infoPanel.Controls.Add(lastUpdateLabel);
            panel4.Controls.Add(infoPanel);
        }

        private void ArrangeLayout()
        {
            // Calculate total required height
            var totalHeight = 60 + 160 + 320 + 60 + 20; // Welcome + Summary + Data grids + Info + padding
            panel4.Height = Math.Max(totalHeight, this.ClientSize.Height);
        }

        private void SetupRefreshTimer()
        {
            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 300000; // 5 minutes
            refreshTimer.Tick += (s, e) =>
            {
                LoadDashboardData();
                if (panel4.Controls.OfType<Panel>().LastOrDefault()?.Controls.OfType<Label>().FirstOrDefault() != null)
                {
                    var lastUpdateLabel = panel4.Controls.OfType<Panel>().LastOrDefault()?.Controls.OfType<Label>().FirstOrDefault();
                    if (lastUpdateLabel != null)
                        lastUpdateLabel.Text = $"Last Updated: {DateTime.Now:HH:mm:ss}";
                }

                // Update date/time
                if (lblDateTime != null)
                    lblDateTime.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy - hh:mm tt");
            };
            refreshTimer.Start();
        }

        private void LoadDashboardData()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Load summary data
                var summary = _dashboardService.GetDashboardSummary();
                UpdateSummaryCards(summary);

                // Load grid data
                LoadLowStockData();
                LoadExpiringItemsData();
                LoadPendingPurchasesData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void UpdateSummaryCards(DashboardSummary summary)
        {
            if (lblTotalProducts != null) lblTotalProducts.Text = summary.TotalProducts.ToString("N0");
            if (lblTotalCompanies != null) lblTotalCompanies.Text = summary.TotalCompanies.ToString("N0");
            if (lblLowStock != null) lblLowStock.Text = summary.LowStockItems.ToString("N0");
            if (lblExpiringItems != null) lblExpiringItems.Text = summary.ExpiringItems.ToString("N0");
            if (lblTodaySales != null) lblTodaySales.Text = summary.TodaySales.ToString("N0");
            if (lblTodayRevenue != null) lblTodayRevenue.Text = $"₹{summary.TodayRevenue:N0}";
            if (lblPendingPayments != null) lblPendingPayments.Text = $"₹{summary.PendingPayments:N0}";
            if (lblInventoryValue != null) lblInventoryValue.Text = $"₹{summary.TotalInventoryValue:N0}";
        }

        private void LoadLowStockData()
        {
            try
            {
                var lowStockItems = _dashboardService.GetLowStockItems();
                dgvLowStock.Rows.Clear();

                foreach (var item in lowStockItems.Take(8)) // Limit to 8 items for better display
                {
                    dgvLowStock.Rows.Add(
                        item.Name.Length > 20 ? item.Name.Substring(0, 20) + "..." : item.Name,
                        item.CompanyName.Length > 15 ? item.CompanyName.Substring(0, 15) + "..." : item.CompanyName,
                        item.CurrentStock,
                        item.StockStatus
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading low stock data: {ex.Message}");
            }
        }

        private void LoadExpiringItemsData()
        {
            try
            {
                var expiringItems = _dashboardService.GetExpiringItems();
                dgvExpiringItems.Rows.Clear();

                foreach (var item in expiringItems.Take(8))
                {
                    dgvExpiringItems.Rows.Add(
                        item.Name.Length > 20 ? item.Name.Substring(0, 20) + "..." : item.Name,
                        item.CompanyName.Length > 12 ? item.CompanyName.Substring(0, 12) + "..." : item.CompanyName,
                        item.QuantityRemaining,
                        item.ExpiryDate,
                        item.DaysToExpiry
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading expiring items data: {ex.Message}");
            }
        }

        private void LoadPendingPurchasesData()
        {
            try
            {
                var pendingPurchases = _dashboardService.GetPendingPurchases();
                dgvPendingPurchases.Rows.Clear();

                foreach (var purchase in pendingPurchases.Take(8))
                {
                    dgvPendingPurchases.Rows.Add(
                        purchase.BatchName.Length > 15 ? purchase.BatchName.Substring(0, 15) + "..." : purchase.BatchName,
                        purchase.CompanyName.Length > 12 ? purchase.CompanyName.Substring(0, 12) + "..." : purchase.CompanyName,
                        purchase.TotalPrice,
                        purchase.Paid,
                        purchase.RemainingAmount
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading pending purchases data: {ex.Message}");
            }
        }

        // Event Handlers
        private void HomeContentform_Resize(object sender, EventArgs e)
        {
            if (summaryPanel != null)
                ResizeSummaryCards();

            if (lowStockPanel != null)
                ResizeDataPanels();
        }

        private void Panel4_Resize(object sender, EventArgs e)
        {
            ResizeSummaryCards();
            ResizeDataPanels();
        }

        private void ResizeSummaryCards()
        {
            if (summaryPanel == null) return;

            var availableWidth = panel4.Width - 40;
            var cardWidth = (availableWidth - 30) / 4;
            var spacing = 10;

            var cards = summaryPanel.Controls.OfType<Panel>().ToArray();
            for (int i = 0; i < cards.Length && i < 8; i++)
            {
                var card = cards[i];
                var row = i / 4;
                var col = i % 4;

                card.Size = new Size(cardWidth, 65);
                card.Location = new Point((col * (cardWidth + spacing)) + 10, (row * 75) + 10);
            }
        }

        private void ResizeDataPanels()
        {
            if (lowStockPanel == null || expiringPanel == null || purchasesPanel == null) return;

            var availableWidth = panel4.Width - 40;
            var panelWidth = (availableWidth - 20) / 3;
            var spacing = 10;

            lowStockPanel.Size = new Size(panelWidth, 300);
            lowStockPanel.Location = new Point(10, 0);

            expiringPanel.Size = new Size(panelWidth, 300);
            expiringPanel.Location = new Point(panelWidth + 20, 0);

            purchasesPanel.Size = new Size(panelWidth, 300);
            purchasesPanel.Location = new Point((panelWidth * 2) + 30, 0);
        }

        private void RefreshDashboard()
        {
            LoadDashboardData();

            // Update last updated label
            var infoPanel = panel4.Controls.OfType<Panel>().LastOrDefault();
            var lastUpdateLabel = infoPanel?.Controls.OfType<Label>().FirstOrDefault(l => l.Text.StartsWith("Last Updated"));
            if (lastUpdateLabel != null)
                lastUpdateLabel.Text = $"Last Updated: {DateTime.Now:HH:mm:ss}";

            // Update date/time
            if (lblDateTime != null)
                lblDateTime.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy - hh:mm tt");

            MessageBox.Show("Dashboard refreshed successfully!", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Cleanup
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            refreshTimer?.Stop();
            refreshTimer?.Dispose();
            base.OnFormClosed(e);
        }

        // Additional helper methods for better functionality
        private void ShowDetailsForLowStock(int productId)
        {
            try
            {
                var stockItems = _dashboardService.GetLowStockItems();
                var item = stockItems.FirstOrDefault(s => s.ProductId == productId);
                if (item != null)
                {
                    var details = $"Product: {item.Name}\n" +
                                $"Company: {item.CompanyName}\n" +
                                $"Current Stock: {item.CurrentStock}\n" +
                                $"Sale Price: ₹{item.SalePrice:N2}\n" +
                                $"Status: {item.StockStatus}";

                    MessageBox.Show(details, "Stock Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error showing details: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportDashboardData()
        {
            try
            {
                var summary = _dashboardService.GetDashboardSummary();
                var sb = new StringBuilder();

                sb.AppendLine("PHARMACY DASHBOARD SUMMARY");
                sb.AppendLine($"Generated on: {DateTime.Now}");
                sb.AppendLine(new string('-', 50));
                sb.AppendLine($"Total Products: {summary.TotalProducts}");
                sb.AppendLine($"Total Companies: {summary.TotalCompanies}");
                sb.AppendLine($"Low Stock Items: {summary.LowStockItems}");
                sb.AppendLine($"Expiring Items: {summary.ExpiringItems}");
                sb.AppendLine($"Today's Sales: {summary.TodaySales}");
                sb.AppendLine($"Today's Revenue: ₹{summary.TodayRevenue:N2}");
                sb.AppendLine($"Pending Payments: ₹{summary.PendingPayments:N2}");
                sb.AppendLine($"Total Inventory Value: ₹{summary.TotalInventoryValue:N2}");

                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                    sfd.FileName = $"Dashboard_Summary_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        System.IO.File.WriteAllText(sfd.FileName, sb.ToString());
                        MessageBox.Show("Dashboard data exported successfully!", "Export Complete",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting data: {ex.Message}", "Export Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Mouse event handlers for interactive cards
        private void Card_DoubleClick(object sender, EventArgs e)
        {
            var card = sender as Panel;
            if (card?.Tag != null)
            {
                string cardType = card.Tag.ToString();
                switch (cardType)
                {
                    case "LowStock":
                        // Navigate to low stock management
                        break;
                    case "Expiring":
                        // Navigate to expiry management
                        break;
                    case "Sales":
                        // Navigate to sales report
                        break;
                    case "Purchases":
                        // Navigate to purchase management
                        break;
                }
            }
        }

        // Context menu for data grids
        private void SetupContextMenus()
        {
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("View Details", null, (s, e) => ViewSelectedItemDetails());
            contextMenu.Items.Add("Refresh", null, (s, e) => RefreshDashboard());
            contextMenu.Items.Add("-");
            contextMenu.Items.Add("Export Data", null, (s, e) => ExportDashboardData());

            dgvLowStock.ContextMenuStrip = contextMenu;
            dgvExpiringItems.ContextMenuStrip = contextMenu;
            dgvPendingPurchases.ContextMenuStrip = contextMenu;
        }

        private void ViewSelectedItemDetails()
        {
            try
            {
                DataGridView activeGrid = null;

                if (dgvLowStock.Focused) activeGrid = dgvLowStock;
                else if (dgvExpiringItems.Focused) activeGrid = dgvExpiringItems;
                else if (dgvPendingPurchases.Focused) activeGrid = dgvPendingPurchases;

                if (activeGrid?.SelectedRows.Count > 0)
                {
                    var row = activeGrid.SelectedRows[0];
                    var details = string.Join("\n",
                        row.Cells.Cast<DataGridViewCell>()
                           .Where(c => c.Value != null)
                           .Select(c => $"{activeGrid.Columns[c.ColumnIndex].HeaderText}: {c.Value}"));

                    MessageBox.Show(details, "Item Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error viewing details: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Performance optimization methods
        private void OptimizeGridPerformance()
        {
            foreach (var dgv in new[] { dgvLowStock, dgvExpiringItems, dgvPendingPurchases })
            {
                if (dgv != null)
                {
                    dgv.SuspendLayout();
                    dgv.VirtualMode = false; // Set to true for large datasets
                    // DoubleBuffered is protected - cannot be accessed directly
                    // dgv.DoubleBuffered = true; // Removed to fix compilation error
                    dgv.ResumeLayout();
                }
            }
        }

        // Note: Remove this section if you already have InitializeComponent() from designer
        // private Panel panel4; // This should already be declared in your designer file
    }
}