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
            this.BackColor = Color.FromArgb(245, 248, 250);

            // Clear and setup main container
            panel4.Controls.Clear();
            panel4.AutoScroll = true;
            panel4.Dock = DockStyle.Fill;
            panel4.Padding = new Padding(0);

            // Create a temporary list to add controls in proper order
            var controlsToAdd = new List<Control>();

            // Create all sections first
            var welcomeSection = CreateWelcomeSectionPanel();
            var summarySection = CreateSummaryCardsPanel();
            var dataSection = CreateDataGridPanelsSection();
            var footerSection = CreateAdditionalInfoPanelSection();

            // Add in correct visual order (top to bottom)
            panel4.Controls.Add(footerSection);   // Added first (appears at bottom)
            panel4.Controls.Add(dataSection);     // Added second
            panel4.Controls.Add(summarySection);  // Added third
            panel4.Controls.Add(welcomeSection);  // Added last (appears at top)

            // Handle resize events
            this.Resize += HomeContentform_Resize;
            panel4.Resize += Panel4_Resize;
        }
        private Panel CreateWelcomeSectionPanel()
        {
            var welcomePanel = new Panel
            {
                Height = 120,
                Dock = DockStyle.Top,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 15),
                BorderStyle = BorderStyle.None
            };

            welcomePanel.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(230, 235, 240)))
                {
                    e.Graphics.DrawLine(pen, 0, welcomePanel.Height - 1, welcomePanel.Width, welcomePanel.Height - 1);
                }
            };

            var contentContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(25, 20, 25, 20)
            };

            lblWelcome = new Label
            {
                Text = "Pharmacy Management Dashboard",
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(0, 0),
                BackColor = Color.Transparent
            };

            lblDateTime = new Label
            {
                Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy - hh:mm tt"),
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.FromArgb(127, 140, 141),
                AutoSize = true,
                Location = new Point(0, 45),
                BackColor = Color.Transparent
            };

            contentContainer.Controls.Add(lblWelcome);
            //contentContainer.Controls.Add(lblDateTime);
            welcomePanel.Controls.Add(contentContainer);

            return welcomePanel;
        }

        private Panel CreateSummaryCardsPanel()
        {
            summaryPanel = new Panel
            {
                Height = 180,
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                Padding = new Padding(25, 15, 25, 25),
                Margin = new Padding(0, 0, 0, 20)
            };

            RefreshSummaryCardLayout();
            return summaryPanel;
        }

        private Panel CreateDataGridPanelsSection()
        {
            var dataContainer = new Panel
            {
                Height = 400,
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                Padding = new Padding(25, 0, 25, 20),
                Margin = new Padding(0, 0, 0, 20)
            };

            RefreshDataPanelLayout(dataContainer);
            return dataContainer;
        }

        private Panel CreateAdditionalInfoPanelSection()
        {
            var infoPanel = new Panel
            {
                Height = 80,
                Dock = DockStyle.Top,
                BackColor = Color.White,
                Padding = new Padding(25, 20, 25, 20)
            };

            infoPanel.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(230, 235, 240)))
                {
                    e.Graphics.DrawLine(pen, 0, 0, infoPanel.Width, 0);
                }
            };

            var refreshBtn = new Button
            {
                Text = "🔄 Refresh Dashboard",
                Font = new Font("Segoe UI", 11F, FontStyle.Regular),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(180, 42),
                Location = new Point(0, 19),
                Cursor = Cursors.Hand
            };
            refreshBtn.FlatAppearance.BorderSize = 0;
            refreshBtn.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            refreshBtn.Click += (s, e) => RefreshDashboard();

            var exportBtn = new Button
            {
                Text = "📊 Export Data",
                Font = new Font("Segoe UI", 11F, FontStyle.Regular),
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(140, 42),
                Location = new Point(195, 19),
                Cursor = Cursors.Hand
            };
            exportBtn.FlatAppearance.BorderSize = 0;
            exportBtn.FlatAppearance.MouseOverBackColor = Color.FromArgb(5, 150, 105);
            exportBtn.Click += (s, e) => ExportDashboardData();

            var lastUpdateLabel = new Label
            {
                Text = $"Last Updated: {DateTime.Now:HH:mm:ss}",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.FromArgb(108, 117, 125),
                AutoSize = true,
                Location = new Point(355, 30)
            };

            infoPanel.Controls.Add(refreshBtn);
            infoPanel.Controls.Add(exportBtn);
            infoPanel.Controls.Add(lastUpdateLabel);

            return infoPanel;
        }


        private void CreateWelcomeSection()
        {
            var welcomePanel = new Panel
            {
                Height = 120, // Increased height to ensure proper containment
                Dock = DockStyle.Top,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 15),
                BorderStyle = BorderStyle.None
            };

            // Add bottom border
            welcomePanel.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(230, 235, 240)))
                {
                    e.Graphics.DrawLine(pen, 0, welcomePanel.Height - 1, welcomePanel.Width, welcomePanel.Height - 1);
                }
            };

            // Create a container panel with proper padding
            var contentContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(25, 20, 25, 20)
            };

            lblWelcome = new Label
            {
                Text = "Pharmacy Management Dashboard",
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(0, 0),
                BackColor = Color.Transparent
            };

            lblDateTime = new Label
            {
                Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy - hh:mm tt"),
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.FromArgb(127, 140, 141),
                AutoSize = true,
                Location = new Point(0, 45), // Position below welcome label with proper spacing
                BackColor = Color.Transparent
            };

            // Add labels to the content container, not directly to welcome panel
            contentContainer.Controls.Add(lblWelcome);
            contentContainer.Controls.Add(lblDateTime);

            // Add content container to welcome panel
            welcomePanel.Controls.Add(contentContainer);

            // Finally add welcome panel to main container
            panel4.Controls.Add(welcomePanel);
        }

        private void CreateSummaryCards()
        {
            summaryPanel = new Panel
            {
                Height = 180,
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                Padding = new Padding(25, 15, 25, 25),
                Margin = new Padding(0, 0, 0, 20)
            };

            RefreshSummaryCardLayout();
            panel4.Controls.Add(summaryPanel);
        }

        private void RefreshSummaryCardLayout()
        {
            if (summaryPanel == null) return;

            summaryPanel.Controls.Clear();

            // Get actual available width
            var containerWidth = summaryPanel.ClientSize.Width - 50; // Account for padding
            var spacing = 10;

            // Determine layout based on available width
            int cardsPerRow = 4;

            if (containerWidth < 1000)
            {
                cardsPerRow = 2;
                summaryPanel.Height = 320; // Increase height for 4 rows
            }
            else
            {
                summaryPanel.Height = 180; // Original height for 2 rows
            }

            var cardWidth = (containerWidth - (spacing * (cardsPerRow - 1))) / cardsPerRow;
            var cardHeight = 75;

            // Ensure minimum card width
            if (cardWidth < 180)
            {
                cardWidth = Math.Max(180, (containerWidth - spacing) / 2);
                cardsPerRow = 2;
                summaryPanel.Height = 320;
            }

            // Create cards with proper positioning
            var cardData = new[]
            {
        new { Title = "Total Products", Value = "0", Color = Color.FromArgb(52, 152, 219) },
        new { Title = "Companies", Value = "0", Color = Color.FromArgb(46, 204, 113) },
        new { Title = "Low Stock Items", Value = "0", Color = Color.FromArgb(231, 76, 60) },
        new { Title = "Expiring Soon", Value = "0", Color = Color.FromArgb(243, 156, 18) },
        new { Title = "Today's Sales", Value = "0", Color = Color.FromArgb(155, 89, 182) },
        new { Title = "Today's Revenue", Value = "Rs 0", Color = Color.FromArgb(52, 73, 94) },
        new { Title = "Pending Payments", Value = "Rs 0", Color = Color.FromArgb(230, 126, 34) },
        new { Title = "Inventory Value", Value = "Rs 0", Color = Color.FromArgb(22, 160, 133) }
    };

            // Create label references
            var labels = new[] { lblTotalProducts, lblTotalCompanies, lblLowStock, lblExpiringItems,
                        lblTodaySales, lblTodayRevenue, lblPendingPayments, lblInventoryValue };

            // Position cards in grid layout
            for (int i = 0; i < cardData.Length; i++)
            {
                int row = i / cardsPerRow;
                int col = i % cardsPerRow;

                int x = col * (cardWidth + spacing);
                int y = row * (cardHeight + 15);

                var card = cardData[i];
                var label = labels[i] ?? new Label();

                // Update the label reference
                switch (i)
                {
                    case 0: lblTotalProducts = label; break;
                    case 1: lblTotalCompanies = label; break;
                    case 2: lblLowStock = label; break;
                    case 3: lblExpiringItems = label; break;
                    case 4: lblTodaySales = label; break;
                    case 5: lblTodayRevenue = label; break;
                    case 6: lblPendingPayments = label; break;
                    case 7: lblInventoryValue = label; break;
                }

                CreateSummaryCard(card.Title, card.Value, card.Color, x, y, cardWidth, cardHeight, label);
            }
        }
        private void CreateSummaryCard(string title, string value, Color bgColor, int x, int y, int width, int height, Label valueLabel)
        {
            var card = new Panel
            {
                Size = new Size(width, height),
                Location = new Point(x, y),
                BackColor = bgColor,
                BorderStyle = BorderStyle.None,
                Cursor = Cursors.Hand
            };

            // Add rounded corners and shadow effect
            card.Paint += (s, e) =>
            {
                var graphics = e.Graphics;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Draw shadow
                var shadowRect = new Rectangle(3, 3, card.Width - 3, card.Height - 3);
                using (var shadowBrush = new SolidBrush(Color.FromArgb(30, 0, 0, 0)))
                {
                    graphics.FillRoundedRectangle(shadowBrush, shadowRect, 10);
                }

                // Draw main card
                var mainRect = new Rectangle(0, 0, card.Width - 3, card.Height - 3);
                using (var brush = new SolidBrush(bgColor))
                {
                    graphics.FillRoundedRectangle(brush, mainRect, 10);
                }
            };

            // Hover effects
            var originalColor = bgColor;
            card.MouseEnter += (s, e) =>
            {
                card.BackColor = ChangeColorBrightness(originalColor, -0.15f);
                card.Invalidate();
            };
            card.MouseLeave += (s, e) =>
            {
                card.BackColor = originalColor;
                card.Invalidate();
            };

            var titleLabel = new Label
            {
                Text = title,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                Location = new Point(20, 15),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            valueLabel.Text = value;
            valueLabel.ForeColor = Color.White;
            valueLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            valueLabel.Location = new Point(20, 40);
            valueLabel.AutoSize = true;
            valueLabel.BackColor = Color.Transparent;

            card.Controls.Add(titleLabel);
            card.Controls.Add(valueLabel);
            summaryPanel.Controls.Add(card);
        }

        private void CreateDataGridPanels()
        {
            var dataContainer = new Panel
            {
                Height = 400, // Initial height, will be adjusted in RefreshDataPanelLayout
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                Padding = new Padding(25, 0, 25, 20),
                Margin = new Padding(0, 0, 0, 20)
            };

            RefreshDataPanelLayout(dataContainer);
            panel4.Controls.Add(dataContainer);
        }

        private void RefreshDataPanelLayout(Panel dataContainer)
        {
            dataContainer.Controls.Clear();

            var availableWidth = dataContainer.Width - 50; // Account for padding

            // Show maximum 2 panels per row for better visibility
            var panelsPerRow = Math.Min(2, 3);
            var totalPanels = 3;
            var rows = (int)Math.Ceiling((double)totalPanels / panelsPerRow);

            var panelWidth = (availableWidth - (10 * (panelsPerRow - 1))) / panelsPerRow;
            var panelHeight = 350;
            var spacing = 10;

            // Ensure minimum panel width
            if (panelWidth < 400)
            {
                panelsPerRow = 1;
                panelWidth = availableWidth;
                dataContainer.Height = panelHeight * totalPanels + (spacing * (totalPanels - 1)) + 20;
            }
            else if (panelsPerRow == 2)
            {
                dataContainer.Height = (panelHeight * rows) + (spacing * (rows - 1)) + 20;
            }

            // Position panels
            int currentPanel = 0;

            // Row 1: Low Stock and Expiring Items
            lowStockPanel = CreateDataPanel("⚠️ Low Stock Items", 0, 10, panelWidth, panelHeight, Color.FromArgb(231, 76, 60));
            dgvLowStock = CreateDataGrid(lowStockPanel);
            SetupLowStockGrid();
            dataContainer.Controls.Add(lowStockPanel);

            if (panelsPerRow >= 2)
            {
                expiringPanel = CreateDataPanel("⏰ Items Expiring Soon", panelWidth + spacing, 10, panelWidth, panelHeight, Color.FromArgb(243, 156, 18));
                dgvExpiringItems = CreateDataGrid(expiringPanel);
                SetupExpiringItemsGrid();
                dataContainer.Controls.Add(expiringPanel);

                // Row 2: Pending Purchases (centered if only one in second row)
                var row2Y = panelHeight + spacing + 10;
                var row2X = panelsPerRow == 2 ? (availableWidth - panelWidth) / 2 : 0;

                purchasesPanel = CreateDataPanel("💰 Pending Purchases", row2X, row2Y, panelWidth, panelHeight, Color.FromArgb(155, 89, 182));
                dgvPendingPurchases = CreateDataGrid(purchasesPanel);
                SetupPendingPurchasesGrid();
                dataContainer.Controls.Add(purchasesPanel);
            }
            else
            {
                // Stack vertically if not enough width
                expiringPanel = CreateDataPanel("⏰ Items Expiring Soon", 0, panelHeight + spacing + 10, panelWidth, panelHeight, Color.FromArgb(243, 156, 18));
                dgvExpiringItems = CreateDataGrid(expiringPanel);
                SetupExpiringItemsGrid();
                dataContainer.Controls.Add(expiringPanel);

                purchasesPanel = CreateDataPanel("💰 Pending Purchases", 0, (panelHeight + spacing) * 2 + 10, panelWidth, panelHeight, Color.FromArgb(155, 89, 182));
                dgvPendingPurchases = CreateDataGrid(purchasesPanel);
                SetupPendingPurchasesGrid();
                dataContainer.Controls.Add(purchasesPanel);
            }
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

            // Add shadow and rounded corners
            panel.Paint += (s, e) =>
            {
                var graphics = e.Graphics;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Shadow
                var shadowRect = new Rectangle(3, 3, panel.Width - 3, panel.Height - 3);
                using (var shadowBrush = new SolidBrush(Color.FromArgb(20, 0, 0, 0)))
                {
                    graphics.FillRoundedRectangle(shadowBrush, shadowRect, 8);
                }

                // Main panel
                var mainRect = new Rectangle(0, 0, panel.Width - 3, panel.Height - 3);
                using (var brush = new SolidBrush(Color.White))
                {
                    graphics.FillRoundedRectangle(brush, mainRect, 8);
                }

                // Border
                using (var pen = new Pen(Color.FromArgb(230, 235, 240)))
                {
                    graphics.DrawRoundedRectangle(pen, mainRect, 8);
                }
            };

            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 45,
                BackColor = headerColor
            };

            var titleLabel = new Label
            {
                Text = title,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 0, 0, 0)
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
                GridColor = Color.FromArgb(240, 244, 247),
                ColumnHeadersHeight = 40,
                RowTemplate = { Height = 32 },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(64, 64, 64),
                    SelectionBackColor = Color.FromArgb(230, 244, 255),
                    SelectionForeColor = Color.FromArgb(44, 62, 80),
                    Padding = new Padding(12, 6, 12, 6),
                    Alignment = DataGridViewContentAlignment.MiddleLeft
                },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(248, 250, 252),
                    ForeColor = Color.FromArgb(73, 80, 87),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Padding = new Padding(12, 6, 12, 6)
                },
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(250, 252, 255)
                }
            };

            var containerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(2, 0, 2, 2),
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
            dgvLowStock.Columns["Stock"].Width = 80;
            dgvLowStock.Columns["Status"].Width = 100;

            // Enhanced color coding
            dgvLowStock.CellFormatting += (s, e) =>
            {
                if (e.ColumnIndex == dgvLowStock.Columns["Status"].Index && e.Value != null)
                {
                    string status = e.Value.ToString();
                    switch (status)
                    {
                        case "OUT_OF_STOCK":
                            e.CellStyle.BackColor = Color.FromArgb(254, 226, 226);
                            e.CellStyle.ForeColor = Color.FromArgb(185, 28, 28);
                            e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                            break;
                        case "CRITICAL":
                            e.CellStyle.BackColor = Color.FromArgb(255, 237, 213);
                            e.CellStyle.ForeColor = Color.FromArgb(194, 65, 12);
                            e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                            break;
                        case "LOW":
                            e.CellStyle.BackColor = Color.FromArgb(254, 249, 195);
                            e.CellStyle.ForeColor = Color.FromArgb(161, 98, 7);
                            e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                            break;
                    }
                }

                if (e.ColumnIndex == dgvLowStock.Columns["Stock"].Index && e.Value != null)
                {
                    if (int.TryParse(e.Value.ToString(), out int stock))
                    {
                        if (stock == 0)
                            e.CellStyle.ForeColor = Color.FromArgb(185, 28, 28);
                        else if (stock <= 5)
                            e.CellStyle.ForeColor = Color.FromArgb(194, 65, 12);
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
            dgvExpiringItems.Columns["Quantity"].Width = 60;
            dgvExpiringItems.Columns["ExpiryDate"].Width = 90;
            dgvExpiringItems.Columns["DaysLeft"].Width = 80;

            // Enhanced color coding for urgency
            dgvExpiringItems.CellFormatting += (s, e) =>
            {
                if (e.ColumnIndex == dgvExpiringItems.Columns["DaysLeft"].Index && e.Value != null)
                {
                    if (int.TryParse(e.Value.ToString(), out int days))
                    {
                        if (days <= 3)
                        {
                            e.CellStyle.BackColor = Color.FromArgb(254, 226, 226);
                            e.CellStyle.ForeColor = Color.FromArgb(185, 28, 28);
                            e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                        }
                        else if (days <= 7)
                        {
                            e.CellStyle.BackColor = Color.FromArgb(255, 237, 213);
                            e.CellStyle.ForeColor = Color.FromArgb(194, 65, 12);
                            e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                        }
                        else if (days <= 15)
                        {
                            e.CellStyle.BackColor = Color.FromArgb(254, 249, 195);
                            e.CellStyle.ForeColor = Color.FromArgb(161, 98, 7);
                        }
                        else if (days <= 30)
                        {
                            e.CellStyle.BackColor = Color.FromArgb(240, 253, 244);
                            e.CellStyle.ForeColor = Color.FromArgb(22, 101, 52);
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

            dgvPendingPurchases.Columns["TotalPrice"].DefaultCellStyle.Format = "Rs #,##0";
            dgvPendingPurchases.Columns["Paid"].DefaultCellStyle.Format = "Rs #,##0";
            dgvPendingPurchases.Columns["Remaining"].DefaultCellStyle.Format = "Rs #,##0";

            dgvPendingPurchases.Columns["TotalPrice"].Width = 90;
            dgvPendingPurchases.Columns["Paid"].Width = 90;
            dgvPendingPurchases.Columns["Remaining"].Width = 90;

            // Enhanced color coding for amounts
            dgvPendingPurchases.CellFormatting += (s, e) =>
            {
                if (e.ColumnIndex == dgvPendingPurchases.Columns["Remaining"].Index && e.Value != null)
                {
                    if (decimal.TryParse(e.Value.ToString(), out decimal remaining))
                    {
                        if (remaining > 500000) // Very high amount
                        {
                            e.CellStyle.BackColor = Color.FromArgb(254, 226, 226);
                            e.CellStyle.ForeColor = Color.FromArgb(185, 28, 28);
                            e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                        }
                        else if (remaining > 200000) // High amount
                        {
                            e.CellStyle.BackColor = Color.FromArgb(255, 237, 213);
                            e.CellStyle.ForeColor = Color.FromArgb(194, 65, 12);
                        }
                        else if (remaining > 50000) // Medium amount
                        {
                            e.CellStyle.BackColor = Color.FromArgb(254, 249, 195);
                            e.CellStyle.ForeColor = Color.FromArgb(161, 98, 7);
                        }
                    }
                }
            };
        }

        private void CreateAdditionalInfoPanel()
        {
            var infoPanel = new Panel
            {
                Height = 80,
                Dock = DockStyle.Top,
                BackColor = Color.White,
                Padding = new Padding(25, 20, 25, 20)
            };

            // Add top border
            infoPanel.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(230, 235, 240)))
                {
                    e.Graphics.DrawLine(pen, 0, 0, infoPanel.Width, 0);
                }
            };

            var refreshBtn = new Button
            {
                Text = "🔄 Refresh Dashboard",
                Font = new Font("Segoe UI", 11F, FontStyle.Regular),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(180, 42),
                Location = new Point(25, 19),
                Cursor = Cursors.Hand
            };
            refreshBtn.FlatAppearance.BorderSize = 0;
            refreshBtn.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            refreshBtn.Click += (s, e) => RefreshDashboard();

            var exportBtn = new Button
            {
                Text = "📊 Export Data",
                Font = new Font("Segoe UI", 11F, FontStyle.Regular),
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(140, 42),
                Location = new Point(220, 19),
                Cursor = Cursors.Hand
            };
            exportBtn.FlatAppearance.BorderSize = 0;
            exportBtn.FlatAppearance.MouseOverBackColor = Color.FromArgb(5, 150, 105);
            exportBtn.Click += (s, e) => ExportDashboardData();

            var lastUpdateLabel = new Label
            {
                Text = $"Last Updated: {DateTime.Now:HH:mm:ss}",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.FromArgb(108, 117, 125),
                AutoSize = true,
                Location = new Point(380, 30)
            };

            infoPanel.Controls.Add(refreshBtn);
            infoPanel.Controls.Add(exportBtn);
            infoPanel.Controls.Add(lastUpdateLabel);
            panel4.Controls.Add(infoPanel);
        }

        // Helper method for rounded rectangles
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

        private void SetupRefreshTimer()
        {
            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 300000; // 5 minutes
            refreshTimer.Tick += (s, e) =>
            {
                LoadDashboardData();
                UpdateTimestamps();
            };
            refreshTimer.Start();
        }

        private void UpdateTimestamps()
        {
            RefreshWelcomeSection();

            // Update last updated label in footer
            var infoPanel = panel4.Controls.OfType<Panel>().LastOrDefault();
            var lastUpdateLabel = infoPanel?.Controls.OfType<Label>().FirstOrDefault(l => l.Text.StartsWith("Last Updated"));
            if (lastUpdateLabel != null)
                lastUpdateLabel.Text = $"Last Updated: {DateTime.Now:HH:mm:ss}";
        }
        private void LoadDashboardData()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                var summary = _dashboardService.GetDashboardSummary();
                UpdateSummaryCards(summary);

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
            if (lblTodayRevenue != null) lblTodayRevenue.Text = $"Rs {summary.TodayRevenue:N0}";
            if (lblPendingPayments != null) lblPendingPayments.Text = $"Rs {summary.PendingPayments:N0}";
            if (lblInventoryValue != null) lblInventoryValue.Text = $"Rs {summary.TotalInventoryValue:N0}";
        }

        private void LoadLowStockData()
        {
            try
            {
                var lowStockItems = _dashboardService.GetLowStockItems();
                dgvLowStock.Rows.Clear();

                foreach (var item in lowStockItems.Take(10))
                {
                    dgvLowStock.Rows.Add(
                        item.Name.Length > 25 ? item.Name.Substring(0, 25) + "..." : item.Name,
                        item.CompanyName.Length > 18 ? item.CompanyName.Substring(0, 18) + "..." : item.CompanyName,
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

                foreach (var item in expiringItems.Take(10))
                {
                    dgvExpiringItems.Rows.Add(
                        item.Name.Length > 25 ? item.Name.Substring(0, 25) + "..." : item.Name,
                        item.CompanyName.Length > 15 ? item.CompanyName.Substring(0, 15) + "..." : item.CompanyName,
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

                foreach (var purchase in pendingPurchases.Take(10))
                {
                    dgvPendingPurchases.Rows.Add(
                        purchase.BatchName.Length > 20 ? purchase.BatchName.Substring(0, 20) + "..." : purchase.BatchName,
                        purchase.CompanyName.Length > 15 ? purchase.CompanyName.Substring(0, 15) + "..." : purchase.CompanyName,
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
        private void RefreshWelcomeSection()
        {
            if (lblWelcome != null && lblDateTime != null)
            {
                // Ensure proper positioning within the welcome panel
                lblWelcome.Location = new Point(0, 0);
                lblDateTime.Location = new Point(0, 40);

                // Update the datetime
                lblDateTime.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy - hh:mm tt");
            }
        }
        // Event Handlers for Responsive Design
        private void HomeContentform_Resize(object sender, EventArgs e)
        {
            RefreshSummaryCardLayout();
            RefreshAllDataPanels();
        }

        private void Panel4_Resize(object sender, EventArgs e)
        {
            RefreshSummaryCardLayout();
            RefreshAllDataPanels();
        }

        private void RefreshAllDataPanels()
        {
            // Find data container and refresh layout
            foreach (Control control in panel4.Controls)
            {
                if (control is Panel panel && panel.Controls.Count > 0)
                {
                    var firstChild = panel.Controls[0];
                    if (firstChild is Panel && (firstChild.Controls.Count == 0 || firstChild.Controls[0] is Panel))
                    {
                        RefreshDataPanelLayout(panel);
                        break;
                    }
                }
            }
        }

        private void RefreshDashboard()
        {
            LoadDashboardData();
            UpdateTimestamps();

            MessageBox.Show("Dashboard refreshed successfully!", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExportDashboardData()
        {
            try
            {
                var summary = _dashboardService.GetDashboardSummary();
                var sb = new StringBuilder();

                sb.AppendLine("PHARMACY MANAGEMENT DASHBOARD SUMMARY");
                sb.AppendLine($"Generated on: {DateTime.Now}");
                sb.AppendLine(new string('=', 60));
                sb.AppendLine();

                sb.AppendLine("SUMMARY STATISTICS:");
                sb.AppendLine($"• Total Products: {summary.TotalProducts:N0}");
                sb.AppendLine($"• Total Companies: {summary.TotalCompanies:N0}");
                sb.AppendLine($"• Low Stock Items: {summary.LowStockItems:N0}");
                sb.AppendLine($"• Items Expiring Soon: {summary.ExpiringItems:N0}");
                sb.AppendLine();

                sb.AppendLine("TODAY'S PERFORMANCE:");
                sb.AppendLine($"• Sales Count: {summary.TodaySales:N0}");
                sb.AppendLine($"• Revenue: Rs {summary.TodayRevenue:N2}");
                sb.AppendLine();

                sb.AppendLine("FINANCIAL OVERVIEW:");
                sb.AppendLine($"• Pending Payments: Rs {summary.PendingPayments:N2}");
                sb.AppendLine($"• Total Inventory Value: Rs {summary.TotalInventoryValue:N2}");
                sb.AppendLine();

                // Add detailed data
                var lowStockItems = _dashboardService.GetLowStockItems();
                if (lowStockItems.Any())
                {
                    sb.AppendLine("LOW STOCK ITEMS:");
                    sb.AppendLine(new string('-', 40));
                    foreach (var item in lowStockItems.Take(10))
                    {
                        sb.AppendLine($"• {item.Name} ({item.CompanyName}) - Stock: {item.CurrentStock} - Status: {item.StockStatus}");
                    }
                    sb.AppendLine();
                }

                var expiringItems = _dashboardService.GetExpiringItems();
                if (expiringItems.Any())
                {
                    sb.AppendLine("ITEMS EXPIRING SOON:");
                    sb.AppendLine(new string('-', 40));
                    foreach (var item in expiringItems.Take(10))
                    {
                        sb.AppendLine($"• {item.Name} ({item.CompanyName}) - Expires: {item.ExpiryDate:dd/MM/yyyy} ({item.DaysToExpiry} days)");
                    }
                    sb.AppendLine();
                }

                var pendingPurchases = _dashboardService.GetPendingPurchases();
                if (pendingPurchases.Any())
                {
                    sb.AppendLine("PENDING PURCHASES:");
                    sb.AppendLine(new string('-', 40));
                    foreach (var purchase in pendingPurchases.Take(10))
                    {
                        sb.AppendLine($"• {purchase.BatchName} ({purchase.CompanyName}) - Remaining: Rs {purchase.RemainingAmount:N2}");
                    }
                }

                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                    sfd.FileName = $"Pharmacy_Dashboard_Report_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                    sfd.Title = "Export Dashboard Report";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        System.IO.File.WriteAllText(sfd.FileName, sb.ToString());
                        MessageBox.Show($"Dashboard report exported successfully to:\n{sfd.FileName}", "Export Complete",
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

        // Context menu for enhanced functionality
        private void SetupContextMenus()
        {
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("View Details", null, (s, e) => ViewSelectedItemDetails());
            contextMenu.Items.Add("Refresh Data", null, (s, e) => RefreshDashboard());
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add("Export Report", null, (s, e) => ExportDashboardData());

            dgvLowStock.ContextMenuStrip = contextMenu;
            dgvExpiringItems.ContextMenuStrip = contextMenu;
            dgvPendingPurchases.ContextMenuStrip = contextMenu;
        }

        private void ViewSelectedItemDetails()
        {
            try
            {
                DataGridView activeGrid = null;
                string gridType = "";

                if (dgvLowStock.Focused)
                {
                    activeGrid = dgvLowStock;
                    gridType = "Low Stock Item";
                }
                else if (dgvExpiringItems.Focused)
                {
                    activeGrid = dgvExpiringItems;
                    gridType = "Expiring Item";
                }
                else if (dgvPendingPurchases.Focused)
                {
                    activeGrid = dgvPendingPurchases;
                    gridType = "Pending Purchase";
                }

                if (activeGrid?.SelectedRows.Count > 0)
                {
                    var row = activeGrid.SelectedRows[0];
                    var details = new StringBuilder();
                    details.AppendLine($"{gridType} Details:");
                    details.AppendLine(new string('=', 30));

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null)
                        {
                            string columnName = activeGrid.Columns[cell.ColumnIndex].HeaderText;
                            details.AppendLine($"{columnName}: {cell.Value}");
                        }
                    }

                    MessageBox.Show(details.ToString(), $"{gridType} Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Please select a row to view details.", "No Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error viewing details: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cleanup resources
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            refreshTimer?.Stop();
            refreshTimer?.Dispose();
            base.OnFormClosed(e);
        }

        // Initialize context menus on first load
        private void InitializeContextMenus()
        {
            SetupContextMenus();
        }

        // Call this after creating data grids
        private void FinalizeSetup()
        {
            SetupContextMenus();
            LoadDashboardData();
        }
    }
}

// Extension method for rounded rectangles
public static class GraphicsExtensions
{
    public static void FillRoundedRectangle(this Graphics graphics, Brush brush, Rectangle rect, int cornerRadius)
    {
        using (var path = GetRoundedRectanglePath(rect, cornerRadius))
        {
            graphics.FillPath(brush, path);
        }
    }

    public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, Rectangle rect, int cornerRadius)
    {
        using (var path = GetRoundedRectanglePath(rect, cornerRadius))
        {
            graphics.DrawPath(pen, path);
        }
    }

    private static System.Drawing.Drawing2D.GraphicsPath GetRoundedRectanglePath(Rectangle rect, int cornerRadius)
    {
        var path = new System.Drawing.Drawing2D.GraphicsPath();

        if (cornerRadius <= 0)
        {
            path.AddRectangle(rect);
            return path;
        }

        int diameter = cornerRadius * 2;
        var arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));

        // Top left arc
        path.AddArc(arcRect, 180, 90);

        // Top right arc
        arcRect.X = rect.Right - diameter;
        path.AddArc(arcRect, 270, 90);

        // Bottom right arc
        arcRect.Y = rect.Bottom - diameter;
        path.AddArc(arcRect, 0, 90);

        // Bottom left arc
        arcRect.X = rect.Left;
        path.AddArc(arcRect, 90, 90);

        path.CloseFigure();
        return path;
    }


}