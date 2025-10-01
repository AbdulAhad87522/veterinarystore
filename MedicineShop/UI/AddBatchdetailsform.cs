using FontAwesome.Sharp;
using MedicineShop.BL;
using MedicineShop.BL.Models;
using MedicineShop.DL;
using MedicineShop.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TechStore.UI;

namespace MedicineShop.UI
{
    public partial class AddBatchdetailsform : Form
    {
        private IBatchesBl batchesBl;
        private IBatchItemsBl batchItemsBl;
        private DatabaseHelper dbHelper;
        private BatchSessionManager sessionManager;
        private int selectedCompanyId = 0;
        private int selectedProductId = 0;
        private string currentBatchName = "";
        private int editingBatchItemId = 0;
        private bool isEditing = false;
        private bool batchSavedToDatabase = false;
        private DataTable batchItemsTable;
        private bool suppressTextChanged = false;
        private BindingSource batchBindingSource = new BindingSource();

        public AddBatchdetailsform(IBatchItemsBl batchItemsBl, IBatchesBl batchesBl)
        {
            InitializeComponent();
            this.batchesBl = batchesBl;
            this.batchItemsBl = batchItemsBl;
            dbHelper = DatabaseHelper.Instance;

            this.KeyPreview = true;
            sessionManager = null;

            // ✅ Panel is now always visible since everything is in it
            paneldetails.Visible = true;

            UIHelper.StyleGridView(dgvbatches);
            UIHelper.StyleGridView(dgvcompany);
            UIHelper.StyleGridView(dgvmedicines);

            InitializeBatchItemsTable();

            this.Load += AddBatchdetailsform_Load;
            this.FormClosing += AddBatchdetailsform_FormClosing;
            this.VisibleChanged += AddBatchdetailsform_VisibleChanged;
        }

        private string GetTempBatchFilePath()
        {
            string folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "MedicineShop",
                "TempData"
            );

            try
            {
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating temp folder: {ex.Message}");
            }

            return Path.Combine(folder, "TempBatchData.json");
        }

        private void SaveTempBatch()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtBnames.Text))
                    return;

                var data = new BatchSessionData
                {
                    BatchName = txtBnames.Text.Trim(),
                    CompanyID = selectedCompanyId,
                    CompanyName = txtcompany.Text.Trim(),
                    TotalAmount = decimal.TryParse(txttotalamont.Text, out decimal total) ? total : 0,
                    PaidAmount = decimal.TryParse(txtpaid.Text, out decimal paid) ? paid : 0,
                    BatchSaved = batchSavedToDatabase,
                    DetailsPanelVisible = paneldetails.Visible,
                    SessionDate = DateTime.Now,
                    BatchItems = new List<BatchItemData>()
                };

                // Save all batch items from the in-memory table
                if (batchItemsTable != null && batchItemsTable.Rows.Count > 0)
                {
                    foreach (DataRow row in batchItemsTable.Rows)
                    {
                        data.BatchItems.Add(new BatchItemData
                        {
                            BatchItemID = Convert.ToInt32(row["BatchItemID"]),
                            BatchID = Convert.ToInt32(row["BatchID"]),
                            MedicineID = Convert.ToInt32(row["MedicineID"]),
                            MedicineName = row["MedicineName"].ToString(),
                            Quantity = Convert.ToInt32(row["Quantity"]),
                            PurchasePrice = Convert.ToDecimal(row["PurchasePrice"]),
                            SalePrice = Convert.ToDecimal(row["SalePrice"]),
                            ExpiryDate = Convert.ToDateTime(row["ExpiryDate"]),
                            TotalCost = Convert.ToDecimal(row["TotalCost"])
                        });
                    }
                }

                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(GetTempBatchFilePath(), json);

                System.Diagnostics.Debug.WriteLine($"✓ Saved temp batch: {data.BatchName} with {data.BatchItems.Count} items");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving temp batch: {ex.Message}");
            }
        }

        private void LoadTempBatch()
        {
            try
            {
                string filePath = GetTempBatchFilePath();
                if (!File.Exists(filePath))
                {
                    System.Diagnostics.Debug.WriteLine("No temp batch file found");
                    return;
                }

                string json = File.ReadAllText(filePath);
                var data = JsonConvert.DeserializeObject<BatchSessionData>(json);

                if (data == null || string.IsNullOrWhiteSpace(data.BatchName))
                {
                    System.Diagnostics.Debug.WriteLine("Invalid temp batch data");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"Found temp batch: {data.BatchName} with {data.BatchItems?.Count ?? 0} items");

                DialogResult result = MessageBox.Show(
                    $"Found unsaved batch: '{data.BatchName}'\n" +
                    $"Items: {data.BatchItems?.Count ?? 0}\n" +
                    $"Created: {data.SessionDate:yyyy-MM-dd HH:mm}\n\n" +
                    "Would you like to restore this session?",
                    "Restore Session",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    File.Delete(filePath);
                    return;
                }

                // Restore basic batch info
                txtBnames.Text = data.BatchName;
                txtcompany.Text = data.CompanyName;
                txttotalamont.Text = data.TotalAmount.ToString("F2");
                txtpaid.Text = data.PaidAmount.ToString("F2");
                selectedCompanyId = data.CompanyID;
                currentBatchName = data.BatchName;
                batchSavedToDatabase = data.BatchSaved;

                int batchId = DatabaseHelper.Instance.getbatchid(data.BatchName);

                if (batchId > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"Batch found in DB with ID: {batchId}");
                    SetBatchFormEnabled(false);
                    LoadBatchItemsFromDatabase(batchId);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Batch NOT in DB - restoring from temp file");

                    dgvbatches.SuspendLayout();
                    dgvbatches.DataSource = null;
                    dgvbatches.Rows.Clear();
                    dgvbatches.Columns.Clear();

                    batchItemsTable = new DataTable();
                    batchItemsTable.Columns.Add("BatchItemID", typeof(int));
                    batchItemsTable.Columns.Add("BatchID", typeof(int));
                    batchItemsTable.Columns.Add("MedicineID", typeof(int));
                    batchItemsTable.Columns.Add("MedicineName", typeof(string));
                    batchItemsTable.Columns.Add("Quantity", typeof(int));
                    batchItemsTable.Columns.Add("PurchasePrice", typeof(decimal));
                    batchItemsTable.Columns.Add("SalePrice", typeof(decimal));
                    batchItemsTable.Columns.Add("ExpiryDate", typeof(DateTime));
                    batchItemsTable.Columns.Add("TotalCost", typeof(decimal));

                    if (data.BatchItems != null && data.BatchItems.Count > 0)
                    {
                        foreach (var item in data.BatchItems)
                        {
                            try
                            {
                                DataRow newRow = batchItemsTable.NewRow();
                                newRow["BatchItemID"] = item.BatchItemID;
                                newRow["BatchID"] = item.BatchID;
                                newRow["MedicineID"] = item.MedicineID;
                                newRow["MedicineName"] = item.MedicineName ?? "Unknown";
                                newRow["Quantity"] = item.Quantity;
                                newRow["PurchasePrice"] = item.PurchasePrice;
                                newRow["SalePrice"] = item.SalePrice;
                                newRow["ExpiryDate"] = item.ExpiryDate;
                                newRow["TotalCost"] = item.TotalCost;
                                batchItemsTable.Rows.Add(newRow);
                            }
                            catch (Exception itemEx)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error adding item: {itemEx.Message}");
                            }
                        }
                    }

                    batchItemsTable.AcceptChanges();
                    dgvbatches.AutoGenerateColumns = true;
                    dgvbatches.DataSource = batchItemsTable;

                    if (dgvbatches.Columns.Count > 0)
                    {
                        if (dgvbatches.Columns.Contains("BatchItemID"))
                            dgvbatches.Columns["BatchItemID"].Visible = false;
                        if (dgvbatches.Columns.Contains("BatchID"))
                            dgvbatches.Columns["BatchID"].Visible = false;
                        if (dgvbatches.Columns.Contains("MedicineID"))
                            dgvbatches.Columns["MedicineID"].Visible = false;

                        if (dgvbatches.Columns.Contains("PurchasePrice"))
                            dgvbatches.Columns["PurchasePrice"].DefaultCellStyle.Format = "C2";
                        if (dgvbatches.Columns.Contains("SalePrice"))
                            dgvbatches.Columns["SalePrice"].DefaultCellStyle.Format = "C2";
                        if (dgvbatches.Columns.Contains("TotalCost"))
                            dgvbatches.Columns["TotalCost"].DefaultCellStyle.Format = "C2";
                        if (dgvbatches.Columns.Contains("ExpiryDate"))
                            dgvbatches.Columns["ExpiryDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    }

                    dgvbatches.ResumeLayout();
                    dgvbatches.Refresh();

                    SetBatchFormEnabled(!data.BatchSaved);

                    // ✅ Focus on appropriate control
                    if (data.BatchItems != null && data.BatchItems.Count > 0)
                    {
                        txtproduct.Focus();
                    }
                    else
                    {
                        txtBnames.Focus();
                    }
                }

                this.Text = $"Add Batch Details - Restored ({batchItemsTable.Rows.Count} items)";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading temp batch: {ex.Message}");
                MessageBox.Show($"Error restoring session: {ex.Message}\n\nStarting with clean form.",
                    "Session Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (keyData == Keys.Enter)
                {
                    if (txtBnames.Focused)
                    {
                        txtcompany.Focus();
                    }
                    else if(txtcompany.Focused)
                    {
                        txttotalamont.Focus();
                    }
                    else if (txttotalamont.Focused)
                    {
                        txtpaid.Focus();
                    }
                    else if(txtpaid.Focused)
                    {
                        iconButton1.PerformClick();
                    }
                    else if(txtproduct.Focused)
                    {
                        txtquantity.Focus();
                    }
                    else if(txtquantity.Focused)
                    {
                        txtcost.Focus();
                    }
                    else if(txtcost.Focused)
                    {
                        txtsaleprice.Focus();
                    }
                    else if( txtsaleprice.Focused)
                    {
                        iconButton2.PerformClick();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error in event listener", ex.Message);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private BatchSessionData CreateCurrentSessionData()
        {
            if (string.IsNullOrWhiteSpace(txtBnames.Text))
                return null;

            var sessionData = new BatchSessionData
            {
                BatchName = txtBnames.Text.Trim(),
                CompanyID = selectedCompanyId,
                CompanyName = txtcompany.Text.Trim(),
                TotalAmount = decimal.TryParse(txttotalamont.Text, out decimal total) ? total : 0,
                PaidAmount = decimal.TryParse(txtpaid.Text, out decimal paid) ? paid : 0,
                BatchSaved = batchSavedToDatabase,
                DetailsPanelVisible = paneldetails.Visible,
                BatchItems = new List<BatchItemData>()
            };

            // Save all batch items from the in-memory table
            if (batchItemsTable != null && batchItemsTable.Rows.Count > 0)
            {
                foreach (DataRow row in batchItemsTable.Rows)
                {
                    sessionData.BatchItems.Add(new BatchItemData
                    {
                        BatchItemID = Convert.ToInt32(row["BatchItemID"]),
                        BatchID = Convert.ToInt32(row["BatchID"]),
                        MedicineID = Convert.ToInt32(row["MedicineID"]),
                        MedicineName = row["MedicineName"].ToString(),
                        Quantity = Convert.ToInt32(row["Quantity"]),
                        PurchasePrice = Convert.ToDecimal(row["PurchasePrice"]),
                        SalePrice = Convert.ToDecimal(row["SalePrice"]),
                        ExpiryDate = Convert.ToDateTime(row["ExpiryDate"]),
                        TotalCost = Convert.ToDecimal(row["TotalCost"])
                    });
                }
            }

            return sessionData;
        }
        private void AddBatchdetailsform_Load(object sender, EventArgs e)
        {
            LoadCompanies();
            LoadMedicines();
            SetupDataGridViews();

            // Initially hide both grids
            dgvcompany.Visible = false;
            dgvmedicines.Visible = false;

            // Add keyboard event handlers
            SetupKeyboardHandlers();

            // ✅ Load temp batch after everything is set up
            LoadTempBatch();
        }


     
        private void AddBatchdetailsform_Shown(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("=== FORM SHOWN EVENT ===");

            // NOW restore session after form is fully rendered
            RestoreSession();
        }

        private void RestoreSession()
        {
            if (sessionManager == null)
            {
                System.Diagnostics.Debug.WriteLine("Session manager is null, skipping restore");
                return;
            }

            try
            {
                var sessionData = sessionManager.RestoreSession(out bool shouldRestore);

                if (shouldRestore && sessionData != null)
                {
                    System.Diagnostics.Debug.WriteLine("=== User chose to restore session ===");

                    // Restore immediately without BeginInvoke
                    RestoreSessionData(sessionData);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("=== No session to restore or user declined ===");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in RestoreSession: {ex.Message}");
                MessageBox.Show($"Failed to restore session: {ex.Message}\n\nStarting with fresh form.",
                    "Session Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DebugSessionData(BatchSessionData sessionData, string context)
        {
            System.Diagnostics.Debug.WriteLine($"=== {context} ===");
            System.Diagnostics.Debug.WriteLine($"BatchName: {sessionData?.BatchName}");
            System.Diagnostics.Debug.WriteLine($"CompanyID: {sessionData?.CompanyID}");
            System.Diagnostics.Debug.WriteLine($"CompanyName: {sessionData?.CompanyName}");
            System.Diagnostics.Debug.WriteLine($"TotalAmount: {sessionData?.TotalAmount}");
            System.Diagnostics.Debug.WriteLine($"PaidAmount: {sessionData?.PaidAmount}");
            System.Diagnostics.Debug.WriteLine($"BatchSaved: {sessionData?.BatchSaved}");
            System.Diagnostics.Debug.WriteLine($"DetailsPanelVisible: {sessionData?.DetailsPanelVisible}");
            System.Diagnostics.Debug.WriteLine($"BatchItems Count: {sessionData?.BatchItems?.Count ?? 0}");

            if (sessionData?.BatchItems != null && sessionData.BatchItems.Count > 0)
            {
                foreach (var item in sessionData.BatchItems)
                {
                    System.Diagnostics.Debug.WriteLine($"  - {item.MedicineName} (ID: {item.MedicineID}, Qty: {item.Quantity}, Price: {item.PurchasePrice})");
                }
            }
            System.Diagnostics.Debug.WriteLine("=================");
        }

        private void RestoreSessionData(BatchSessionData sessionData)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== Starting Session Restore ===");

                txtBnames.Text = sessionData.BatchName;
                txtcompany.Text = sessionData.CompanyName;
                txttotalamont.Text = sessionData.TotalAmount.ToString("F2");
                txtpaid.Text = sessionData.PaidAmount.ToString("F2");
                selectedCompanyId = sessionData.CompanyID;
                currentBatchName = sessionData.BatchName;

                int batchId = DatabaseHelper.Instance.getbatchid(sessionData.BatchName);

                if (batchId > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"Batch found in database with ID: {batchId}");
                    batchSavedToDatabase = true;
                    SetBatchFormEnabled(false);
                    LoadBatchItemsFromDatabase(batchId);
                    this.Text = $"Add Batch Details - Restored from DB ({batchItemsTable.Rows.Count} items)";
                    txtproduct.Focus();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Batch NOT in database - restoring from session");

                    batchSavedToDatabase = sessionData.BatchSaved;
                    bool hasBatchItems = sessionData.BatchItems != null && sessionData.BatchItems.Count > 0;

                    dgvbatches.DataSource = null;
                    dgvbatches.Columns.Clear();
                    dgvbatches.Rows.Clear();

                    batchItemsTable = new DataTable();
                    batchItemsTable.Columns.Add("BatchItemID", typeof(int));
                    batchItemsTable.Columns.Add("BatchID", typeof(int));
                    batchItemsTable.Columns.Add("MedicineID", typeof(int));
                    batchItemsTable.Columns.Add("MedicineName", typeof(string));
                    batchItemsTable.Columns.Add("Quantity", typeof(int));
                    batchItemsTable.Columns.Add("PurchasePrice", typeof(decimal));
                    batchItemsTable.Columns.Add("SalePrice", typeof(decimal));
                    batchItemsTable.Columns.Add("ExpiryDate", typeof(DateTime));
                    batchItemsTable.Columns.Add("TotalCost", typeof(decimal));

                    if (hasBatchItems)
                    {
                        foreach (var item in sessionData.BatchItems)
                        {
                            try
                            {
                                DataRow newRow = batchItemsTable.NewRow();
                                newRow["BatchItemID"] = item.BatchItemID;
                                newRow["BatchID"] = item.BatchID;
                                newRow["MedicineID"] = item.MedicineID;
                                newRow["MedicineName"] = item.MedicineName ?? "Unknown";
                                newRow["Quantity"] = item.Quantity;
                                newRow["PurchasePrice"] = item.PurchasePrice;
                                newRow["SalePrice"] = item.SalePrice;
                                newRow["ExpiryDate"] = item.ExpiryDate;
                                newRow["TotalCost"] = item.TotalCost;
                                batchItemsTable.Rows.Add(newRow);
                            }
                            catch (Exception itemEx)
                            {
                                System.Diagnostics.Debug.WriteLine($"Failed to add item: {itemEx.Message}");
                            }
                        }
                    }

                    dgvbatches.AutoGenerateColumns = true;
                    dgvbatches.DataSource = batchItemsTable;

                    if (dgvbatches.Columns.Count > 0)
                    {
                        if (dgvbatches.Columns.Contains("BatchItemID"))
                            dgvbatches.Columns["BatchItemID"].Visible = false;
                        if (dgvbatches.Columns.Contains("BatchID"))
                            dgvbatches.Columns["BatchID"].Visible = false;
                        if (dgvbatches.Columns.Contains("MedicineID"))
                            dgvbatches.Columns["MedicineID"].Visible = false;

                        if (dgvbatches.Columns.Contains("PurchasePrice"))
                            dgvbatches.Columns["PurchasePrice"].DefaultCellStyle.Format = "C2";
                        if (dgvbatches.Columns.Contains("SalePrice"))
                            dgvbatches.Columns["SalePrice"].DefaultCellStyle.Format = "C2";
                        if (dgvbatches.Columns.Contains("TotalCost"))
                            dgvbatches.Columns["TotalCost"].DefaultCellStyle.Format = "C2";
                        if (dgvbatches.Columns.Contains("ExpiryDate"))
                            dgvbatches.Columns["ExpiryDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    }

                    dgvbatches.Refresh();
                    SetBatchFormEnabled(!batchSavedToDatabase);

                    this.Text = $"Add Batch Details - Restored ({batchItemsTable.Rows.Count} items)";

                    if (hasBatchItems)
                    {
                        txtproduct.Focus();
                    }
                    else
                    {
                        txtBnames.Focus();
                    }
                }

                System.Diagnostics.Debug.WriteLine("=== Session Restore Complete ===");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR DURING RESTORE: {ex.Message}");
                MessageBox.Show($"Error restoring session: {ex.Message}\n\nStarting with clean form.",
                    "Session Restore Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ResetForm();
            }
        }

        private void AddBatchdetailsform_FormClosing(object sender, FormClosingEventArgs e)
        {
            // ✅ Save temp batch on close
            SaveTempBatch();
        }
        private void AddBatchdetailsform_VisibleChanged(object sender, EventArgs e)
        {
            // ✅ Save when form becomes invisible
            if (!this.Visible)
            {
                SaveTempBatch();
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            AddBatch();
        }

        // Add/Update Product - Only add to grid, not database

        private void iconButton2_Click(object sender, EventArgs e)
        {
            AddBatchItem();
        }
        private void CancelEdit()
        {
            // Reset editing mode
            isEditing = false;
            editingBatchItemId = 0;

            // Reset button appearance
            iconButton2.Text = "Add Product";
            iconButton2.IconChar = FontAwesome.Sharp.IconChar.Plus;

            ResetEditingVisuals();

            // Reset form title
            this.Text = "Add Batch Details";
        }

        private void ResetEditingVisuals()
        {
            iconButton2.BackColor = Color.FromArgb(109, 148, 197);  // ✅ YOUR BLUE
            iconButton2.ForeColor = Color.White;
        }

        // Save Button - Save all data to database at once

        private void iconButton3_Click(object sender, EventArgs e)
        {
            SaveBatchItems();
        }
        #region Helper Methods

        private void InitializeBatchItemsTable()
        {
            batchItemsTable = new DataTable();
            batchItemsTable.Columns.Add("BatchItemID", typeof(int));
            batchItemsTable.Columns.Add("BatchID", typeof(int));
            batchItemsTable.Columns.Add("MedicineID", typeof(int));
            batchItemsTable.Columns.Add("MedicineName", typeof(string));
            batchItemsTable.Columns.Add("Quantity", typeof(int));
            batchItemsTable.Columns.Add("PurchasePrice", typeof(decimal));
            batchItemsTable.Columns.Add("SalePrice", typeof(decimal));
            batchItemsTable.Columns.Add("ExpiryDate", typeof(DateTime));
            batchItemsTable.Columns.Add("TotalCost", typeof(decimal));
        }

        private void RefreshBatchItemsGrid()
        {
            System.Diagnostics.Debug.WriteLine($"=== RefreshBatchItemsGrid START ===");
            System.Diagnostics.Debug.WriteLine($"Table rows: {batchItemsTable?.Rows.Count ?? 0}");

            try
            {
                // Suspend layout to prevent flickering
                dgvbatches.SuspendLayout();

                // Clear existing binding
                dgvbatches.DataSource = null;

                // Accept any pending changes to the table
                if (batchItemsTable != null)
                {
                    batchItemsTable.AcceptChanges();
                }

                // Re-bind to the table
                dgvbatches.DataSource = batchItemsTable;

                System.Diagnostics.Debug.WriteLine($"After binding - Grid rows: {dgvbatches.Rows.Count}");

                // Configure column visibility
                if (dgvbatches.Columns.Contains("BatchItemID"))
                    dgvbatches.Columns["BatchItemID"].Visible = false;
                if (dgvbatches.Columns.Contains("BatchID"))
                    dgvbatches.Columns["BatchID"].Visible = false;
                if (dgvbatches.Columns.Contains("MedicineID"))
                    dgvbatches.Columns["MedicineID"].Visible = false;

                // Format currency columns
                if (dgvbatches.Columns.Contains("PurchasePrice"))
                    dgvbatches.Columns["PurchasePrice"].DefaultCellStyle.Format = "C2";
                if (dgvbatches.Columns.Contains("SalePrice"))
                    dgvbatches.Columns["SalePrice"].DefaultCellStyle.Format = "C2";
                if (dgvbatches.Columns.Contains("TotalCost"))
                    dgvbatches.Columns["TotalCost"].DefaultCellStyle.Format = "C2";

                // Format date column
                if (dgvbatches.Columns.Contains("ExpiryDate"))
                    dgvbatches.Columns["ExpiryDate"].DefaultCellStyle.Format = "dd/MM/yyyy";

                // Resume layout
                dgvbatches.ResumeLayout();

                // Force refresh
                dgvbatches.Refresh();

                System.Diagnostics.Debug.WriteLine($"=== RefreshBatchItemsGrid END - Success ===");
            }
            catch (Exception ex)
            {
                dgvbatches.ResumeLayout();
                System.Diagnostics.Debug.WriteLine($"=== RefreshBatchItemsGrid ERROR: {ex.Message} ===");
                throw;
            }
        }
        private void SetBatchFormEnabled(bool enabled)
        {
            txtBnames.Enabled = enabled;
            txtcompany.Enabled = enabled;
            txttotalamont.Enabled = enabled;
            txtpaid.Enabled = enabled;
            iconButton1.Enabled = enabled;
        }

        private void SetupDataGridViews()
        {
            // Setup company datagridview
            dgvcompany.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvcompany.MultiSelect = false;
            dgvcompany.CellClick += DgvCompany_CellClick;

            // Setup medicines datagridview
            dgvmedicines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvmedicines.MultiSelect = false;
            dgvmedicines.CellClick += DgvMedicines_CellClick;

            // Setup batches datagridview (for showing added batch items)
            dgvbatches.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvbatches.MultiSelect = false;
            dgvbatches.CellDoubleClick += DgvBatches_CellDoubleClick;
            dgvbatches.KeyDown += DgvBatches_KeyDown;
        }

        private void LoadCompanies()
        {
            try
            {
                DataTable companies = dbHelper.GetCompany("");
                dgvcompany.DataSource = companies;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading companies: {ex.Message}");
            }
        }

        private void LoadMedicines()
        {
            try
            {
                var batchesDl = new BatchesDl();
                DataTable medicines = batchesDl.GetMedicines();
                dgvmedicines.DataSource = medicines;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading medicines: {ex.Message}");
            }
        }

        private void LoadBatchItemsFromDatabase(int batchId)
        {
            try
            {
                // Initialize the table first
                InitializeBatchItemsTable();

                // Get batch items from database - assuming it returns List<BatchItems>
                var batchItemsList = batchItemsBl.GetBatchItemsByBatchId(batchId);

                if (batchItemsList != null && batchItemsList.Count > 0)
                {
                    // Populate in-memory table with database data
                    foreach (var batchItem in batchItemsList)
                    {
                        DataRow newRow = batchItemsTable.NewRow();
                        newRow["BatchItemID"] = batchItem.BatchItemID;
                        newRow["BatchID"] = batchItem.BatchID;
                        newRow["MedicineID"] = batchItem.MedicineID;

                        // Get medicine name
                        newRow["MedicineName"] = GetMedicineName(batchItem.MedicineID);

                        newRow["Quantity"] = batchItem.Quantity;
                        newRow["PurchasePrice"] = batchItem.PurchasePrice;
                        newRow["SalePrice"] = batchItem.SalePrice;
                        newRow["ExpiryDate"] = batchItem.ExpiryDate;
                        newRow["TotalCost"] = batchItem.Quantity * batchItem.PurchasePrice;

                        batchItemsTable.Rows.Add(newRow);
                    }
                }

                // Refresh grid display
                RefreshBatchItemsGrid();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading batch items from database: {ex.Message}");
                // Initialize empty table on error
                InitializeBatchItemsTable();
                RefreshBatchItemsGrid();
            }
        }

        private void LoadBatchItems()
        {
            // This method is used for loading existing batch items when not restoring from session
            // Since we're now working with in-memory data until save,
            // this method just refreshes the grid with current in-memory data
            RefreshBatchItemsGrid();
        }

        private string GetMedicineName(int medicineId)
        {
            try
            {
                // Get medicine name from the medicines DataTable
                DataTable medicines = (DataTable)dgvmedicines.DataSource;
                if (medicines != null)
                {
                    DataRow[] foundRows = medicines.Select($"product_id = {medicineId}");
                    if (foundRows.Length > 0)
                    {
                        string companyName = foundRows[0]["company_name"].ToString();
                        string categoryName = foundRows[0]["category_name"].ToString();
                        string packingName = foundRows[0]["packing_name"].ToString();
                        return $"{companyName} - {categoryName} - {packingName}";
                    }
                }

                // Fallback: try to get from database
                return $"Medicine ID: {medicineId}"; // You can implement a database call here if needed
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting medicine name: {ex.Message}");
                return $"Medicine ID: {medicineId}";
            }
        }

        private void ClearProductForm()
        {
            txtproduct.Clear();
            txtquantity.Clear();
            txtcost.Clear();
            txtsaleprice.Clear();
            dateTimePicker1.Value = DateTime.Now.AddMonths(6); // Default 6 months from now
            selectedProductId = 0;
        }

        private void EnableControls(bool enabled)
        {
            txtproduct.Enabled = enabled;
            txtquantity.Enabled = enabled;
            txtcost.Enabled = enabled;
            txtsaleprice.Enabled = enabled;
            dateTimePicker1.Enabled = enabled;
            iconButton2.Enabled = enabled;
            iconButton3.Enabled = enabled;
        }

        // 4. Update ResetForm to work with unified panel
        private void ResetForm()
        {
            try
            {
                string tempFile = GetTempBatchFilePath();
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                    System.Diagnostics.Debug.WriteLine("Temp batch file deleted on reset");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting temp file: {ex.Message}");
            }

            // Reset all form fields
            txtBnames.Clear();
            txtcompany.Clear();
            txttotalamont.Clear();
            txtpaid.Clear();
            ClearProductForm();

            // Reset variables
            selectedCompanyId = 0;
            selectedProductId = 0;
            currentBatchName = "";
            editingBatchItemId = 0;
            isEditing = false;
            batchSavedToDatabase = false;

            // ✅ Enable batch form controls for new entry
            SetBatchFormEnabled(true);
            EnableControls(true);

            // ✅ Panel stays visible
            dgvcompany.Visible = false;
            dgvmedicines.Visible = false;

            LoadCompanies();
            LoadMedicines();

            InitializeBatchItemsTable();
            RefreshBatchItemsGrid();

            this.Text = "Add Batch Details";
            ResetEditingVisuals();

            // ✅ Focus on batch name for new entry
            txtBnames.Focus();
        }
        #endregion

        #region Event Handlers

        private void SetupKeyboardHandlers()
        {
            // Company textbox keyboard handling
            txtcompany.KeyDown += TxtCompany_KeyDown;
            txtcompany.TextChanged += TxtCompany_TextChanged;
            txtcompany.Leave += TxtCompany_Leave;

            // Product textbox keyboard handling
            txtproduct.KeyDown += TxtProduct_KeyDown;
            txtproduct.TextChanged += TxtProduct_TextChanged;
            txtproduct.Leave += TxtProduct_Leave;

            // DataGridView keyboard handling
            dgvcompany.KeyDown += DgvCompany_KeyDown;
            dgvmedicines.KeyDown += DgvMedicines_KeyDown;

            // Add form-level keyboard handler for shortcuts and escape key
            this.KeyPreview = true;
            this.KeyDown += AddBatchdetailsform_KeyDown;

            // Add tooltips for keyboard shortcuts
            SetupKeyboardShortcutTooltips();
        }
        private void SetupKeyboardShortcutTooltips()
        {
            try
            {
                ToolTip tooltip = new ToolTip();
                tooltip.SetToolTip(iconButton1, "Add Batch (Ctrl+A when batch form is active)");
                tooltip.SetToolTip(iconButton2, "Add Product (Ctrl+A when product form is active)");
                tooltip.SetToolTip(iconButton3, "Save Batch Items (Ctrl+S)");

                // Optional: Add tooltip to form itself
                tooltip.SetToolTip(this, "Shortcuts: Ctrl+A (Add), Ctrl+S (Save), Ctrl+N (New), Esc (Cancel Edit)");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting up tooltips: {ex.Message}");
            }
        }


        // Replace the AddBatchdetailsform_KeyDown method to avoid duplicate validation:

        private void AddBatchdetailsform_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.A:
                            // ✅ Ctrl+A: Add Batch if not saved, otherwise Add Batch Item
                            if (!batchSavedToDatabase)
                            {
                                AddBatch();
                            }
                            else
                            {
                                AddBatchItem();
                            }
                            e.Handled = true;
                            break;

                        case Keys.S:
                            // Ctrl+S: Save Batch Items to database
                            if (batchSavedToDatabase && batchItemsTable.Rows.Count > 0)
                            {
                                SaveBatchItems();
                            }
                            else if (!batchSavedToDatabase)
                            {
                                MessageBox.Show("Please create the batch first (Ctrl+A).", "Info",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Please add at least one item before saving.", "Info",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            e.Handled = true;
                            break;

                        case Keys.N:
                            // Ctrl+N: New/Reset Form
                            ResetForm();
                            e.Handled = true;
                            break;
                    }
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    if (isEditing)
                    {
                        CancelEdit();
                        ClearProductForm();
                        txtproduct.Focus();
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error handling keyboard shortcut: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddBatch()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtBnames.Text))
                {
                    MessageBox.Show("Please enter batch name.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtBnames.Focus();
                    return;
                }

                if (selectedCompanyId == 0)
                {
                    MessageBox.Show("Please select a company.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtcompany.Focus();
                    return;
                }

                if (!decimal.TryParse(txttotalamont.Text, out decimal totalAmount) || totalAmount <= 0)
                {
                    MessageBox.Show("Please enter valid total amount.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txttotalamont.Focus();
                    return;
                }

                decimal paidAmount = 0;
                if (!string.IsNullOrWhiteSpace(txtpaid.Text))
                {
                    if (!decimal.TryParse(txtpaid.Text, out paidAmount) || paidAmount < 0)
                    {
                        MessageBox.Show("Please enter valid paid amount (0 or more).", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtpaid.Focus();
                        return;
                    }
                }

                var batch = new Batches
                {
                    BatchName = txtBnames.Text.Trim(),
                    CompanyID = selectedCompanyId,
                    TotalPrice = totalAmount,
                    Paid = paidAmount,
                    PurchaseDate = DateTime.Now,
                    Status = "Active"
                };

                bool success = batchesBl.AddBatch(batch);

                if (success)
                {
                    currentBatchName = batch.BatchName;
                    batchSavedToDatabase = true;

                    // ✅ No need to show panel - it's already visible
                    // ✅ Disable batch info fields to prevent editing
                    SetBatchFormEnabled(false);

                    InitializeBatchItemsTable();
                    RefreshBatchItemsGrid();

                    SaveTempBatch();

                    MessageBox.Show("Batch created successfully! You can now add products.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // ✅ Focus on product entry
                    txtproduct.Focus();
                }
                else
                {
                    MessageBox.Show("Failed to add batch. Please try again.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding batch: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AddBatchItem()
        {
            try
            {
                // Validation
                if (selectedProductId == 0)
                {
                    MessageBox.Show("Please select a product.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtproduct.Focus();
                    return;
                }

                if (!int.TryParse(txtquantity.Text, out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("Please enter valid quantity.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtquantity.Focus();
                    return;
                }

                if (!decimal.TryParse(txtcost.Text, out decimal costPrice) || costPrice <= 0)
                {
                    MessageBox.Show("Please enter valid cost price.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtcost.Focus();
                    return;
                }

                if (!decimal.TryParse(txtsaleprice.Text, out decimal salePrice) || salePrice <= 0)
                {
                    MessageBox.Show("Please enter valid sale price.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtsaleprice.Focus();
                    return;
                }

                if (dateTimePicker1.Value <= DateTime.Now)
                {
                    MessageBox.Show("Expiry date must be in the future.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dateTimePicker1.Focus();
                    return;
                }

                // Get medicine name for display
                string medicineName = txtproduct.Text;

                if (isEditing)
                {
                    // Update existing item in grid
                    DataRow rowToEdit = batchItemsTable.Rows[editingBatchItemId];
                    rowToEdit["MedicineID"] = selectedProductId;
                    rowToEdit["MedicineName"] = medicineName;
                    rowToEdit["Quantity"] = quantity;
                    rowToEdit["PurchasePrice"] = costPrice;
                    rowToEdit["SalePrice"] = salePrice;
                    rowToEdit["ExpiryDate"] = dateTimePicker1.Value;
                    rowToEdit["TotalCost"] = quantity * costPrice;

                    // Reset editing mode
                    isEditing = false;
                    editingBatchItemId = 0;
                    iconButton2.Text = "Add Product";
                    iconButton2.IconChar = FontAwesome.Sharp.IconChar.Plus;
                    ResetEditingVisuals();
                }
                else
                {
                    // Add new item to grid
                    DataRow newRow = batchItemsTable.NewRow();
                    newRow["BatchItemID"] = batchItemsTable.Rows.Count + 1; // Temporary ID
                    newRow["BatchID"] = 0; // Will be set when saving to database
                    newRow["MedicineID"] = selectedProductId;
                    newRow["MedicineName"] = medicineName;
                    newRow["Quantity"] = quantity;
                    newRow["PurchasePrice"] = costPrice;
                    newRow["SalePrice"] = salePrice;
                    newRow["ExpiryDate"] = dateTimePicker1.Value;
                    newRow["TotalCost"] = quantity * costPrice;

                    batchItemsTable.Rows.Add(newRow);
                }

                // Refresh grid display
                RefreshBatchItemsGrid();

                // Clear form
                ClearProductForm();

                // ✅ Auto-save after adding item
                SaveTempBatch();

                txtproduct.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing product: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveBatchItems()
        {
            try
            {
                // Validate that there are items to save
                if (batchItemsTable == null || batchItemsTable.Rows.Count == 0)
                {
                    MessageBox.Show("Please add at least one product to the batch.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the batch ID from database
                int batchId = DatabaseHelper.Instance.getbatchid(currentBatchName);
                if (batchId == 0)
                {
                    MessageBox.Show("Batch not found. Please create batch first.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Save all batch items to database
                int savedCount = 0;
                foreach (DataRow row in batchItemsTable.Rows)
                {
                    var batchItem = new BatchItems
                    {
                        BatchID = batchId,
                        MedicineID = Convert.ToInt32(row["MedicineID"]),
                        Quantity = Convert.ToInt32(row["Quantity"]),
                        PurchasePrice = Convert.ToDecimal(row["PurchasePrice"]),
                        SalePrice = Convert.ToDecimal(row["SalePrice"]),
                        ExpiryDate = Convert.ToDateTime(row["ExpiryDate"])
                    };

                    bool success = batchItemsBl.AddBatchItem(batchItem);
                    if (success)
                    {
                        savedCount++;
                    }
                }

                if (savedCount == batchItemsTable.Rows.Count)
                {
                    MessageBox.Show($"Batch saved successfully! {savedCount} products added to database.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // ✅ Delete temp file after successful save
                    string tempFile = GetTempBatchFilePath();
                    if (File.Exists(tempFile))
                        File.Delete(tempFile);

                    // Reset for new batch
                    ResetForm();
                }
                else
                {
                    MessageBox.Show($"Partially saved: {savedCount} out of {batchItemsTable.Rows.Count} products were saved.",
                        "Partial Save", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving batch: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #region Company Search and Keyboard Handling

        private void TxtCompany_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchTerm = txtcompany.Text.Trim();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    DataTable companies = dbHelper.GetCompany(searchTerm);
                    dgvcompany.DataSource = companies;
                    dgvcompany.Columns["company_id"].Visible = false;

                    dgvcompany.Visible = true;

                    // Position the grid below the textbox
                }
                else
                {
                    dgvcompany.Visible = false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error searching companies: {ex.Message}");
            }
        }

        private void TxtCompany_KeyDown(object sender, KeyEventArgs e)
        {
            if (dgvcompany.Visible && dgvcompany.Rows.Count > 0)
            {
                int firstVisibleCol = GetFirstVisibleColumnIndex(dgvcompany);

                switch (e.KeyCode)
                {
                    case Keys.Down:
                        {
                            int currentRow = dgvcompany.CurrentCell?.RowIndex ?? -1;
                            int nextRow = currentRow + 1;

                            if (nextRow < dgvcompany.Rows.Count)
                            {
                                dgvcompany.ClearSelection();
                                dgvcompany.CurrentCell = dgvcompany.Rows[nextRow].Cells[firstVisibleCol];
                                dgvcompany.Rows[nextRow].Selected = true;
                            }
                            e.Handled = true;
                            e.SuppressKeyPress = true; // Prevent default behavior
                            break;
                        }

                    case Keys.Up:
                        {
                            int currentRow = dgvcompany.CurrentCell?.RowIndex ?? -1;
                            int prevRow = currentRow - 1;

                            if (prevRow >= 0)
                            {
                                dgvcompany.ClearSelection();
                                dgvcompany.CurrentCell = dgvcompany.Rows[prevRow].Cells[firstVisibleCol];
                                dgvcompany.Rows[prevRow].Selected = true;
                            }
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                            break;
                        }

                    case Keys.Enter:
                        if (dgvcompany.CurrentRow != null)
                        {
                            SelectCompanyFromGrid(dgvcompany.CurrentRow);
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                        }
                        break;

                    case Keys.Escape:
                        dgvcompany.Visible = false;
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        break;
                }
            }
        }
        private void TxtCompany_Leave(object sender, EventArgs e)
        {
            // Small delay to allow clicking on the grid
            Timer timer = new Timer();
            timer.Interval = 200;
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                if (!dgvcompany.Focused)
                {
                    dgvcompany.Visible = false;
                }
            };
            timer.Start();
        }

        private void DgvCompany_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dgvcompany.SelectedRows.Count > 0)
            {
                SelectCompanyFromGrid(dgvcompany.SelectedRows[0]);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                dgvcompany.Visible = false;
                txtcompany.Focus();
                e.Handled = true;
            }
        }

        private void DgvCompany_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    SelectCompanyFromGrid(dgvcompany.Rows[e.RowIndex]);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error selecting company: {ex.Message}");
            }
        }

        private void SelectCompanyFromGrid(DataGridViewRow row)
        {
            selectedCompanyId = Convert.ToInt32(row.Cells["company_id"].Value);
            string companyName = row.Cells["company_name"].Value.ToString();
            txtcompany.Text = companyName;
            dgvcompany.Visible = false;

            // Move focus to next control
            txttotalamont.Focus();
        }

        #endregion

        #region Product Search and Keyboard Handling

        private void TxtProduct_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Skip if suppressed (when programmatically setting text)
                if (suppressTextChanged) return;

                string searchTerm = txtproduct.Text.Trim();

                if (!string.IsNullOrEmpty(searchTerm) && paneldetails.Visible)
                {
                    // IMPORTANT: Reload the full medicines list first
                    var batchesDl = new BatchesDl();
                    DataTable medicines = batchesDl.GetMedicines();

                    // Now filter
                    DataView dv = medicines.DefaultView;
                    dv.RowFilter = $"company_name LIKE '%{searchTerm}%' OR category_name LIKE '%{searchTerm}%' OR packing_name LIKE '%{searchTerm}%' OR name LIKE '%{searchTerm}%'";

                    if (dv.Count > 0)
                    {
                        dgvmedicines.DataSource = dv.ToTable();
                        dgvmedicines.Columns["company_id"].Visible = false;
                        dgvmedicines.Columns["packing_id"].Visible = false;
                        dgvmedicines.Columns["category_id"].Visible = false;
                        dgvmedicines.Columns["product_id"].Visible = false;

                        dgvmedicines.Visible = true;
                    }
                    else
                    {
                        dgvmedicines.Visible = false;
                    }
                }
                else
                {
                    dgvmedicines.Visible = false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error searching medicines: {ex.Message}");
            }
        }
        private void TxtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (dgvmedicines.Visible && dgvmedicines.Rows.Count > 0)
            {
                int firstVisibleCol = GetFirstVisibleColumnIndex(dgvmedicines);

                switch (e.KeyCode)
                {
                    case Keys.Down:
                        {
                            int currentRow = dgvmedicines.CurrentCell?.RowIndex ?? -1;
                            int nextRow = currentRow + 1;

                            if (nextRow < dgvmedicines.Rows.Count)
                            {
                                dgvmedicines.ClearSelection();
                                dgvmedicines.CurrentCell = dgvmedicines.Rows[nextRow].Cells[firstVisibleCol];
                                dgvmedicines.Rows[nextRow].Selected = true;
                            }
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                            break;
                        }

                    case Keys.Up:
                        {
                            int currentRow = dgvmedicines.CurrentCell?.RowIndex ?? -1;
                            int prevRow = currentRow - 1;

                            if (prevRow >= 0)
                            {
                                dgvmedicines.ClearSelection();
                                dgvmedicines.CurrentCell = dgvmedicines.Rows[prevRow].Cells[firstVisibleCol];
                                dgvmedicines.Rows[prevRow].Selected = true;
                            }
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                            break;
                        }

                    case Keys.Enter:
                        if (dgvmedicines.CurrentRow != null)
                        {
                            SelectMedicineFromGrid(dgvmedicines.CurrentRow);
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                        }
                        break;

                    case Keys.Escape:
                        dgvmedicines.Visible = false;
                        txtproduct.Focus();
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        break;
                }
            }
        }


        private void TxtProduct_Leave(object sender, EventArgs e)
        {
            // Small delay to allow clicking on the grid
            Timer timer = new Timer();
            timer.Interval = 200;
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                if (!dgvmedicines.Focused)
                {
                    dgvmedicines.Visible = false;
                }
            };
            timer.Start();
        }

        private void DgvMedicines_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dgvmedicines.SelectedRows.Count > 0)
            {
                SelectMedicineFromGrid(dgvmedicines.SelectedRows[0]);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                dgvmedicines.Visible = false;
                txtproduct.Focus();
                e.Handled = true;
            }
        }

        private void DgvMedicines_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                SelectMedicineFromGrid(dgvmedicines.Rows[e.RowIndex]);
            }
        }

        private void SelectMedicineFromGrid(DataGridViewRow row)
        {
            try
            {
                selectedProductId = Convert.ToInt32(row.Cells["product_id"].Value);
                txtsaleprice.Text = row.Cells["sale_price"].Value.ToString();

                string companyName = row.Cells["company_name"].Value.ToString();
                string categoryName = row.Cells["category_name"].Value.ToString();
                string packingName = row.Cells["packing_name"].Value.ToString();
                string ProductName = row.Cells["name"].Value.ToString();

                suppressTextChanged = true;
                txtproduct.Text = $"{ProductName}-{companyName} - {categoryName} - {packingName}";
                suppressTextChanged = false;

                dgvmedicines.Visible = false;
                txtquantity.Focus();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error selecting medicine: {ex.Message}");
            }
        }


        #endregion
        private int GetFirstVisibleColumnIndex(DataGridView dgv)
        {
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.Visible)
                    return col.Index;
            }
            throw new InvalidOperationException("No visible columns found in DataGridView.");
        }

        #region Batch Items Grid Event Handlers

        private void DgvBatches_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    // Enter edit mode
                    isEditing = true;
                    editingBatchItemId = e.RowIndex;

                    // Load data into form for editing
                    DataRow row = batchItemsTable.Rows[e.RowIndex];

                    selectedProductId = Convert.ToInt32(row["MedicineID"]);
                    txtproduct.Text = row["MedicineName"].ToString();
                    txtquantity.Text = row["Quantity"].ToString();
                    txtcost.Text = row["PurchasePrice"].ToString();
                    txtsaleprice.Text = row["SalePrice"].ToString();
                    dateTimePicker1.Value = Convert.ToDateTime(row["ExpiryDate"]);

                    // Change button appearance to indicate editing mode
                    iconButton2.Text = "Update Product";
                    iconButton2.IconChar = FontAwesome.Sharp.IconChar.Edit;
                    iconButton2.BackColor = Color.Orange;
                    iconButton2.ForeColor = Color.White;

                    // Visual indicator for editing mode

                    // Update form title
                    this.Text = "Add Batch Details - Editing Item";

                    // Set focus to quantity for quick editing
                    txtquantity.Focus();
                    txtquantity.SelectAll();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing batch item: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvBatches_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && dgvbatches.SelectedRows.Count > 0)
            {
                // Confirm deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete the selected item?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int rowIndex = dgvbatches.SelectedRows[0].Index;
                        batchItemsTable.Rows.RemoveAt(rowIndex);
                        RefreshBatchItemsGrid();

                        // Auto-save session
                        if (sessionManager != null)
                        {
                            var sessionData = CreateCurrentSessionData();
                            if (sessionData != null)
                            {
                                sessionManager.SaveSession(sessionData);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting batch item: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (e.KeyCode == Keys.Enter && dgvbatches.SelectedRows.Count > 0)
            {
                // Double-click functionality on Enter key
                DgvBatches_CellDoubleClick(sender, new DataGridViewCellEventArgs(0, dgvbatches.SelectedRows[0].Index));
            }
        }

        #endregion

        #region Grid Helper Methods

    
        #endregion

        #endregion

        #region Additional Helper Methods

      

        #endregion

        private void iconButton4_Click(object sender, EventArgs e)
        {
            var f = Program.ServiceProvider.GetRequiredService<AddCompany>();
            f.ShowDialog(this);
        }

        private void iconButton5_Click(object sender, EventArgs e)
        {
            var f= Program.ServiceProvider.GetRequiredService<AddMedicine>();
            f.ShowDialog(this);
        }

       
    }
}