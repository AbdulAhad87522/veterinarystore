using FontAwesome.Sharp;
using MedicineShop.BL;
using MedicineShop.BL.Models;
using MedicineShop.DL;
using MedicineShop.Models;
using System;
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

        public AddBatchdetailsform(IBatchItemsBl batchItemsBl, IBatchesBl batchesBl)
        {
            InitializeComponent();
            this.batchesBl = batchesBl;
            this.batchItemsBl = batchItemsBl;
            dbHelper = DatabaseHelper.Instance;

            // Initialize session manager with error handling
            try
            {
                sessionManager = new BatchSessionManager();
                sessionManager.UnsavedChangesChanged += OnUnsavedChangesChanged;
                sessionManager.AutoSaveRequested += OnAutoSaveRequested;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to initialize session manager: {ex.Message}");
                sessionManager = null;
            }

            // Initially hide the details panel
            paneldetails.Visible = false;

            // Initialize in-memory batch items table
            InitializeBatchItemsTable();

            // Setup form event handlers
            this.Load += AddBatchdetailsform_Load;
            this.FormClosing += AddBatchdetailsform_FormClosing;
        }

        private void OnUnsavedChangesChanged(object sender, bool hasChanges)
        {
            // Auto-save session when changes occur
            if (hasChanges && sessionManager != null)
            {
                var sessionData = CreateCurrentSessionData();
                if (sessionData != null)
                {
                    sessionManager.SaveSession(sessionData);
                }
            }
        }

        private void OnAutoSaveRequested(object sender, EventArgs e)
        {
            // Auto-save session
            var sessionData = CreateCurrentSessionData();
            if (sessionData != null && sessionManager != null)
            {
                sessionManager.SaveSession(sessionData);
            }
        }

        private BatchSessionData CreateCurrentSessionData()
        {
            if (string.IsNullOrWhiteSpace(txtBnames.Text))
                return null;

            return new BatchSessionData
            {
                BatchName = txtBnames.Text.Trim(),
                CompanyID = selectedCompanyId,
                CompanyName = txtcompany.Text.Trim(),
                TotalAmount = decimal.TryParse(txttotalamont.Text, out decimal total) ? total : 0,
                PaidAmount = decimal.TryParse(txtpaid.Text, out decimal paid) ? paid : 0,
                BatchSaved = batchSavedToDatabase,
                DetailsPanelVisible = paneldetails.Visible,
                // BatchItemsCount = batchItemsTable?.Rows.Count ?? 0  // Removed if not defined in BatchSessionData
            };
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

            // Setup change tracking
            SetupChangeTracking();

            // Try to restore session after form is fully loaded
            this.Shown += (s, ev) => RestoreSession();
        }

        private void SetupChangeTracking()
        {
            if (sessionManager == null) return;

            // Track changes in all input controls
            txtBnames.TextChanged += (s, e) => sessionManager.MarkUnsavedChanges();
            txtcompany.TextChanged += (s, e) => sessionManager.MarkUnsavedChanges();
            txttotalamont.TextChanged += (s, e) => sessionManager.MarkUnsavedChanges();
            txtpaid.TextChanged += (s, e) => sessionManager.MarkUnsavedChanges();
            txtproduct.TextChanged += (s, e) => sessionManager.MarkUnsavedChanges();
            txtquantity.TextChanged += (s, e) => sessionManager.MarkUnsavedChanges();
            txtcost.TextChanged += (s, e) => sessionManager.MarkUnsavedChanges();
            txtsaleprice.TextChanged += (s, e) => sessionManager.MarkUnsavedChanges();
            dateTimePicker1.ValueChanged += (s, e) => sessionManager.MarkUnsavedChanges();
        }

        private void RestoreSession()
        {
            if (sessionManager == null) return;

            var sessionData = sessionManager.RestoreSession(out bool shouldRestore);

            if (shouldRestore && sessionData != null)
            {
                RestoreSessionData(sessionData);
            }
        }

        private void RestoreSessionData(BatchSessionData sessionData)
        {
            try
            {
                // Restore basic batch info
                txtBnames.Text = sessionData.BatchName;
                txtcompany.Text = sessionData.CompanyName;
                txttotalamont.Text = sessionData.TotalAmount.ToString("F2");
                txtpaid.Text = sessionData.PaidAmount.ToString("F2");
                selectedCompanyId = sessionData.CompanyID;
                currentBatchName = sessionData.BatchName;

                // Check if batch exists in database
                int batchId = DatabaseHelper.Instance.getbatchid(sessionData.BatchName);

                if (batchId > 0)
                {
                    // Batch exists in database
                    batchSavedToDatabase = true;
                    paneldetails.Visible = true;

                    // Disable batch form fields since batch is saved
                    SetBatchFormEnabled(false);

                    // Load existing batch items from database
                    LoadBatchItemsFromDatabase(batchId);

                    // Update form title
                    this.Text = "Add Batch Details - Session Restored (Database)";

                    // Set focus to product search
                    txtproduct.Focus();
                }
                else
                {
                    // Batch doesn't exist in database but session has data
                    batchSavedToDatabase = false;
                    paneldetails.Visible = sessionData.DetailsPanelVisible;

                    if (paneldetails.Visible)
                    {
                        // Batch form was filled but not yet saved
                        SetBatchFormEnabled(true);

                        // Initialize empty batch items table for this session
                        InitializeBatchItemsTable();
                        RefreshBatchItemsGrid();

                        // Update form title
                        this.Text = "Add Batch Details - Session Restored (Unsaved)";
                    }
                    else
                    {
                        // Just basic form data restored
                        SetBatchFormEnabled(true);
                        this.Text = "Add Batch Details - Session Restored";
                    }
                }
            }
            catch (Exception ex)
            {
                // Silent error handling - just continue without session restore
                System.Diagnostics.Debug.WriteLine($"Error restoring session: {ex.Message}");

                // Reset to clean state on error
                ResetForm();
            }
        }

        private void AddBatchdetailsform_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Only auto-save session if there's meaningful data
            if (sessionManager != null && !string.IsNullOrWhiteSpace(txtBnames.Text))
            {
                var sessionData = CreateCurrentSessionData();
                if (sessionData != null)
                {
                    sessionManager.SaveSession(sessionData);
                }
            }

            // Dispose of session manager
            sessionManager?.Dispose();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            try
            {
                // Validation code
                if (string.IsNullOrWhiteSpace(txtBnames.Text))
                {
                    MessageBox.Show("Please enter batch name.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (selectedCompanyId == 0)
                {
                    MessageBox.Show("Please select a company.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txttotalamont.Text, out decimal totalAmount) || totalAmount <= 0)
                {
                    MessageBox.Show("Please enter valid total amount.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtpaid.Text, out decimal paidAmount) || paidAmount < 0)
                {
                    MessageBox.Show("Please enter valid paid amount.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create batch object
                var batch = new Batches
                {
                    BatchName = txtBnames.Text.Trim(),
                    CompanyID = selectedCompanyId,
                    TotalPrice = totalAmount,
                    Paid = paidAmount,
                    PurchaseDate = DateTime.Now,
                    Status = "Active"
                };

                // Add batch to database
                bool success = batchesBl.AddBatch(batch);

                if (success)
                {
                    currentBatchName = batch.BatchName;
                    // Mark as saved to database
                    batchSavedToDatabase = true;

                    // Make details panel visible
                    paneldetails.Visible = true;

                    // Disable batch form fields
                    SetBatchFormEnabled(false);

                    // Initialize fresh batch items table for this batch
                    InitializeBatchItemsTable();
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

                    // Set focus to product search
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

        // Add/Update Product - Only add to grid, not database
        private void iconButton2_Click(object sender, EventArgs e)
        {
            try
            {
                // Validation
                if (selectedProductId == 0)
                {
                    MessageBox.Show("Please select a product.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtquantity.Text, out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("Please enter valid quantity.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtcost.Text, out decimal costPrice) || costPrice <= 0)
                {
                    MessageBox.Show("Please enter valid cost price.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtsaleprice.Text, out decimal salePrice) || salePrice <= 0)
                {
                    MessageBox.Show("Please enter valid sale price.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dateTimePicker1.Value <= DateTime.Now)
                {
                    MessageBox.Show("Expiry date must be in the future.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                // Set focus back to product search
                txtproduct.Focus();

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
                MessageBox.Show($"Error processing product: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            iconButton2.BackColor = SystemColors.Control;
            iconButton2.ForeColor = SystemColors.ControlText;
            paneldetails.BackColor = SystemColors.Control;
        }

        // Save Button - Save all data to database at once
        private void iconButton3_Click(object sender, EventArgs e)
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

                    // Mark as saved to database
                    batchSavedToDatabase = true;

                    // Clear session since batch is now finalized
                    if (sessionManager != null)
                    {
                        sessionManager.ClearSession();
                    }

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
            dgvbatches.DataSource = batchItemsTable;

            // Hide unnecessary columns
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

        private void ResetForm()
        {
            // Clear session first
            if (sessionManager != null)
            {
                sessionManager.ClearSession();
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

            // Reset control states
            SetBatchFormEnabled(true);
            EnableControls(true);

            // Hide panels
            paneldetails.Visible = false;
            dgvcompany.Visible = false;
            dgvmedicines.Visible = false;

            // Reload data
            LoadCompanies();
            LoadMedicines();

            // Reset in-memory batch items table
            InitializeBatchItemsTable();
            RefreshBatchItemsGrid();

            // Reset form title
            this.Text = "Add Batch Details";

            // Reset visual elements
            ResetEditingVisuals();
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

            // Add form-level keyboard handler for escape key to cancel edits
            this.KeyPreview = true;
            this.KeyDown += AddBatchdetailsform_KeyDown;
        }

        private void AddBatchdetailsform_KeyDown(object sender, KeyEventArgs e)
        {
            // Press Escape to cancel edit mode
            if (e.KeyCode == Keys.Escape && isEditing)
            {
                CancelEdit();
                ClearProductForm();
                txtproduct.Focus();
                e.Handled = true;
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
                    PositionGridBelowControl(dgvcompany, txtcompany);
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
                switch (e.KeyCode)
                {
                    case Keys.Down:
                        if (dgvcompany.SelectedRows.Count == 0 || dgvcompany.CurrentCell.RowIndex < dgvcompany.Rows.Count - 1)
                        {
                            int nextRow = dgvcompany.SelectedRows.Count == 0 ? 0 : dgvcompany.CurrentCell.RowIndex + 1;
                            dgvcompany.ClearSelection();
                            dgvcompany.Rows[nextRow].Selected = true;
                            dgvcompany.CurrentCell = dgvcompany.Rows[nextRow].Cells[0];
                        }
                        e.Handled = true;
                        break;

                    case Keys.Up:
                        if (dgvcompany.SelectedRows.Count > 0 && dgvcompany.CurrentCell.RowIndex > 0)
                        {
                            int prevRow = dgvcompany.CurrentCell.RowIndex - 1;
                            dgvcompany.ClearSelection();
                            dgvcompany.Rows[prevRow].Selected = true;
                            dgvcompany.CurrentCell = dgvcompany.Rows[prevRow].Cells[0];
                        }
                        e.Handled = true;
                        break;

                    case Keys.Enter:
                        if (dgvcompany.SelectedRows.Count > 0)
                        {
                            SelectCompanyFromGrid(dgvcompany.SelectedRows[0]);
                            e.Handled = true;
                        }
                        break;

                    case Keys.Escape:
                        dgvcompany.Visible = false;
                        e.Handled = true;
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
                string searchTerm = txtproduct.Text.Trim();

                if (!string.IsNullOrEmpty(searchTerm) && paneldetails.Visible)
                {
                    // Filter medicines based on search term
                    DataTable medicines = ((DataTable)dgvmedicines.DataSource).Copy();
                    DataView dv = medicines.DefaultView;

                    // Search in multiple columns (adjust column names as per your data)
                    dv.RowFilter = $"company_name LIKE '%{searchTerm}%' OR category_name LIKE '%{searchTerm}%' OR packing_name LIKE '%{searchTerm}%' OR name LIKE '%{searchTerm}%' ";

                    if (dv.Count > 0)
                    {
                        dgvmedicines.DataSource = dv.ToTable();
                        dgvmedicines.Columns["company_id"].Visible = false;
                        dgvmedicines.Columns["packing_id"].Visible = false;
                        dgvmedicines.Columns["category_id"].Visible = false;
                        dgvmedicines.Columns["product_id"].Visible = false;

                        dgvmedicines.Visible = true;
                        PositionGridBelowControl(dgvmedicines, txtproduct);
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
                switch (e.KeyCode)
                {
                    case Keys.Down:
                        if (dgvmedicines.SelectedRows.Count == 0 || dgvmedicines.CurrentCell.RowIndex < dgvmedicines.Rows.Count - 1)
                        {
                            int nextRow = dgvmedicines.SelectedRows.Count == 0 ? 0 : dgvmedicines.CurrentCell.RowIndex + 1;
                            dgvmedicines.ClearSelection();
                            dgvmedicines.Rows[nextRow].Selected = true;
                            dgvmedicines.CurrentCell = dgvmedicines.Rows[nextRow].Cells[0];
                        }
                        e.Handled = true;
                        break;

                    case Keys.Up:
                        if (dgvmedicines.SelectedRows.Count > 0 && dgvmedicines.CurrentCell.RowIndex > 0)
                        {
                            int prevRow = dgvmedicines.CurrentCell.RowIndex - 1;
                            dgvmedicines.ClearSelection();
                            dgvmedicines.Rows[prevRow].Selected = true;
                            dgvmedicines.CurrentCell = dgvmedicines.Rows[prevRow].Cells[0];
                        }
                        e.Handled = true;
                        break;

                    case Keys.Enter:
                        if (dgvmedicines.SelectedRows.Count > 0)
                        {
                            SelectMedicineFromGrid(dgvmedicines.SelectedRows[0]);
                            e.Handled = true;
                        }
                        break;

                    case Keys.Escape:
                        dgvmedicines.Visible = false;
                        e.Handled = true;
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
                txtsaleprice.Text=row.Cells["sale_price"].Value.ToString();

                // Display product information
                string companyName = row.Cells["company_name"].Value.ToString();
                string categoryName = row.Cells["category_name"].Value.ToString();
                string packingName = row.Cells["packing_name"].Value.ToString();

                txtproduct.Text = $"{companyName} - {categoryName} - {packingName}";
                dgvmedicines.Visible = false;

                // Move focus to next control
                txtquantity.Focus();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error selecting medicine: {ex.Message}");
            }
        }

        #endregion

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
                    paneldetails.BackColor = Color.LightYellow;

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

        private void PositionGridBelowControl(DataGridView grid, Control control)
        {
            // Position the grid right below the control
            Point controlLocation = this.PointToClient(control.Parent.PointToScreen(control.Location));
            grid.Location = new Point(controlLocation.X, controlLocation.Y + control.Height + 2);
            grid.BringToFront();
        }

        #endregion

        #endregion

        #region Additional Helper Methods

        private void RecalculateTotalAmount()
        {
            try
            {
                if (batchItemsTable != null && batchItemsTable.Rows.Count > 0)
                {
                    decimal totalCost = 0;
                    foreach (DataRow row in batchItemsTable.Rows)
                    {
                        totalCost += Convert.ToDecimal(row["TotalCost"]);
                    }

                    // Update the total amount field
                    txttotalamont.Text = totalCost.ToString("F2");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error calculating total amount: {ex.Message}");
            }
        }

        private bool ValidateForm()
        {
            // Basic form validation
            if (string.IsNullOrWhiteSpace(txtBnames.Text))
            {
                MessageBox.Show("Please enter batch name.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBnames.Focus();
                return false;
            }

            if (selectedCompanyId == 0)
            {
                MessageBox.Show("Please select a company.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtcompany.Focus();
                return false;
            }

            if (!decimal.TryParse(txttotalamont.Text, out decimal totalAmount) || totalAmount <= 0)
            {
                MessageBox.Show("Please enter valid total amount.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txttotalamont.Focus();
                return false;
            }

            if (!decimal.TryParse(txtpaid.Text, out decimal paidAmount) || paidAmount < 0)
            {
                MessageBox.Show("Please enter valid paid amount.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtpaid.Focus();
                return false;
            }

            return true;
        }

        private void SetFormTitle(string additionalInfo = "")
        {
            if (string.IsNullOrEmpty(additionalInfo))
            {
                this.Text = "Add Batch Details";
            }
            else
            {
                this.Text = $"Add Batch Details - {additionalInfo}";
            }
        }

        // Note: Dispose method is handled in Designer.cs file
        // Session manager disposal is handled in FormClosing event

        #endregion

        private void dgvcompany_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}