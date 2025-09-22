using MedicineShop.BL;
using MedicineShop.BL.Models;
using MedicineShop.DL;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MedicineShop.UI
{
    public partial class AddBatchdetailsform : Form
    {
        private BatchesBl batchesBl;
        private BatchItemsBl batchItemsBl;
        private DatabaseHelper dbHelper;
        private BatchSessionManager sessionManager;
        private int selectedCompanyId = 0;
        private int selectedProductId = 0;
        private string currentBatchName = "";
        private int editingBatchItemId = 0;
        private bool isEditing = false;

        public AddBatchdetailsform()
        {
            InitializeComponent();
            batchesBl = new BatchesBl();
            batchItemsBl = new BatchItemsBl();
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
                // If session manager fails, continue without it
                System.Diagnostics.Debug.WriteLine($"Failed to initialize session manager: {ex.Message}");
                sessionManager = null;
            }

            // Initially hide the details panel
            paneldetails.Visible = false;

            // Setup form event handlers
            this.Load += AddBatchdetailsform_Load;
            this.FormClosing += AddBatchdetailsform_FormClosing;
        }

        private void OnUnsavedChangesChanged(object sender, bool hasChanges)
        {
            // Update form title to indicate unsaved changes
            string baseTitle = "Add Batch Details";
            if (hasChanges && !this.Text.Contains("*"))
            {
                this.Text = baseTitle + " *";
            }
            else if (!hasChanges && this.Text.Contains("*"))
            {
                this.Text = baseTitle;
            }
        }

        private void OnAutoSaveRequested(object sender, EventArgs e)
        {
            // Create session data and save it
            var sessionData = CreateCurrentSessionData();
            if (sessionData != null)
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
                BatchSaved = !string.IsNullOrEmpty(currentBatchName),
                DetailsPanelVisible = paneldetails.Visible
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

                if (sessionData.BatchSaved)
                {
                    // Batch was already saved to database
                    currentBatchName = sessionData.BatchName;
                    paneldetails.Visible = sessionData.DetailsPanelVisible;

                    // Disable batch form fields since batch is saved
                    SetBatchFormEnabled(false);

                    // Load existing batch items
                    LoadBatchItems();

                    // Set focus to product search
                    if (paneldetails.Visible)
                        txtproduct.Focus();
                }

                // Update form title to indicate restored session
                this.Text = "Add Batch Details - Session Restored";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error restoring session: {ex.Message}", "Session Restore Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AddBatchdetailsform_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = sessionManager.HandleFormClosing();

            switch (result)
            {
                case DialogResult.Yes:
                    var sessionData = CreateCurrentSessionData();
                    if (sessionData != null)
                    {
                        sessionManager.SaveSession(sessionData);
                    }
                    break;
                case DialogResult.No:
                    sessionManager.ClearSession();
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    return;
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
                    MessageBox.Show("Batch added successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Make details panel visible
                    paneldetails.Visible = true;

                    // Disable batch form fields
                    SetBatchFormEnabled(false);

                    // Mark as having changes and save session
                    sessionManager.MarkUnsavedChanges();
                    var sessionData = CreateCurrentSessionData();
                    if (sessionData != null)
                    {
                        sessionManager.SaveSession(sessionData);
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

        // FIXED: Missing iconButton2_Click method - Add/Update Product
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

                if (isEditing)
                {
                    // Update existing batch item
                    var batchItem = new BatchItems
                    {
                        BatchItemID = editingBatchItemId,
                        BatchID=DatabaseHelper.Instance.getbatchid(currentBatchName),
                        MedicineID = selectedProductId,
                        Quantity = quantity,
                        PurchasePrice = costPrice,
                        SalePrice = salePrice,
                        ExpiryDate = dateTimePicker1.Value
                    };

                    bool success = batchItemsBl.UpdateBatchItem(batchItem);

                    if (success)
                    {
                        MessageBox.Show("Product updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Reset editing mode
                        isEditing = false;
                        editingBatchItemId = 0;
                        iconButton2.Text = "Add Product";
                        iconButton2.IconChar = FontAwesome.Sharp.IconChar.Plus;

                        // Clear form and refresh grid
                        ClearProductForm();
                        LoadBatchItems();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update product.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Add new batch item
                    var batchItem = new BatchItems
                    {
                        BatchID=DatabaseHelper.Instance.getbatchid(currentBatchName),
                        MedicineID = selectedProductId,
                        Quantity = quantity,
                        PurchasePrice = costPrice,
                        SalePrice = salePrice,
                        ExpiryDate = dateTimePicker1.Value,
                    };

                    bool success = batchItemsBl.AddBatchItem(batchItem);

                    if (success)
                    {
                        MessageBox.Show("Product added to batch successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Clear form and refresh grid
                        ClearProductForm();
                        LoadBatchItems();

                        // Set focus back to product search
                        txtproduct.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add product to batch.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                // Mark changes and save session
                sessionManager.MarkUnsavedChanges();
                var sessionData = CreateCurrentSessionData();
                if (sessionData != null)
                {
                    sessionManager.SaveSession(sessionData);
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
            iconButton2.BackColor = SystemColors.Control;
            iconButton2.ForeColor = SystemColors.ControlText;

            // Reset form appearance
            paneldetails.BackColor = SystemColors.Control;

            // Reset form title
            this.Text = "Add Batch Details";

            // Hide cancel button if you have one
            // iconButtonCancel.Visible = false;
        }
        private void iconButton3_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to finalize this batch? You won't be able to add more products after this.",
                    "Confirm Save",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    MessageBox.Show("Batch saved and finalized successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear session since batch is now finalized
                    sessionManager.ClearSession();

                    // Disable all controls
                    EnableControls(false);

                    // Optionally close the form or reset for new batch
                    DialogResult newBatch = MessageBox.Show(
                        "Would you like to add another batch?",
                        "New Batch",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (newBatch == DialogResult.Yes)
                    {
                        ResetForm();
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving batch: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Helper Methods

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
                MessageBox.Show($"Error loading companies: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show($"Error loading medicines: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBatchItems()
        {
            try
            {
                var batchItemsDl = new BatchItemsDl();
                var allBatchItems = batchItemsDl.GetAllBatchItems();

                // Filter items for current batch
                var currentBatchItems = allBatchItems.Where(x => x.batchname == currentBatchName).ToList();

                dgvbatches.DataSource = currentBatchItems;
                dgvbatches.Columns["BatchItemID"].Visible = false;
                dgvbatches.Columns["BatchID"].Visible = false;
                dgvbatches.Columns["MedicineID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading batch items: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            sessionManager.ClearSession();

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
            dgvbatches.DataSource = null;

            // Reset form title
            this.Text = "Add Batch Details";
        }

        #endregion

        #region Event Handlers (Add all your existing keyboard and click event handlers here)

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
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to cancel editing? All changes will be lost.",
                    "Cancel Edit",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    CancelEdit();
                    ClearProductForm();
                    txtproduct.Focus();
                }
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
                MessageBox.Show($"Error searching companies: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            try {
                if (e.RowIndex >= 0)
                {
                    SelectCompanyFromGrid(dgvcompany.Rows[e.RowIndex]);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                    dv.RowFilter = $"company_name LIKE '%{searchTerm}%' OR category_name LIKE '%{searchTerm}%' OR packing_name LIKE '%{searchTerm}%'";

                    if (dv.Count > 0)
                    {
                        dgvmedicines.DataSource = dv.ToTable();
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
                MessageBox.Show($"Error searching medicines: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            selectedProductId = Convert.ToInt32(row.Cells["product_id"].Value);

            // Display product information
            string companyName = row.Cells["company_name"].Value.ToString();
            string categoryName = row.Cells["category_name"].Value.ToString();
            string packingName = row.Cells["packing_name"].Value.ToString();

            txtproduct.Text = $"{companyName} - {categoryName} - {packingName}";

            // Get current sale price
            decimal currentSalePrice = Convert.ToDecimal(row.Cells["sale_price"].Value);
            txtsaleprice.Text = currentSalePrice.ToString("F2");

            dgvmedicines.Visible = false;

            // Move focus to quantity
            txtquantity.Focus();
        }

        #endregion

        #region Batch Items Edit/Delete Functionality

        private void DgvBatches_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                EditBatchItem(e.RowIndex);
            }
        }

        private void DgvBatches_KeyDown(object sender, KeyEventArgs e)
        {
            if (dgvbatches.SelectedRows.Count > 0)
            {
                switch (e.KeyCode)
                {
                    case Keys.F2:
                        EditBatchItem(dgvbatches.SelectedRows[0].Index);
                        e.Handled = true;
                        break;
                    case Keys.Delete:
                        DeleteBatchItem(dgvbatches.SelectedRows[0].Index);
                        e.Handled = true;
                        break;
                }
            }
        }

        private void EditBatchItem(int rowIndex)
        {
            try
            {
                if (rowIndex >= 0 && dgvbatches.Rows.Count > rowIndex)
                {
                    DataGridViewRow row = dgvbatches.Rows[rowIndex];

                    // Get batch item details
                    editingBatchItemId = Convert.ToInt32(row.Cells["BatchItemID"].Value);
                    selectedProductId = Convert.ToInt32(row.Cells["MedicineID"].Value);

                    // Fill the form with existing data
                    txtproduct.Text = row.Cells["MedicineName"].Value?.ToString() ?? "";
                    txtquantity.Text = row.Cells["Quantity"].Value?.ToString() ?? "";
                    txtcost.Text = Convert.ToDecimal(row.Cells["PurchasePrice"].Value).ToString("F2");
                    txtsaleprice.Text = Convert.ToDecimal(row.Cells["SalePrice"].Value).ToString("F2");
                    dateTimePicker1.Value = Convert.ToDateTime(row.Cells["ExpiryDate"].Value);

                    // Set editing mode
                    isEditing = true;

                    // Update button text and icon
                    iconButton2.Text = "Update Product";
                    iconButton2.IconChar = FontAwesome.Sharp.IconChar.Edit;

                    // Change button color to indicate edit mode
                    iconButton2.BackColor = Color.Orange;
                    iconButton2.ForeColor = Color.White;

                    // Show cancel option (if you have a cancel button, uncomment these lines):
                    // iconButtonCancel.Visible = true;
                    // iconButtonCancel.BackColor = Color.Gray;

                    // Add visual feedback to form
                    paneldetails.BackColor = Color.FromArgb(255, 248, 220); // Light orange background

                    // Update form title to show edit mode
                    this.Text = "Add Batch Details - Editing Product *";

                    // Focus on quantity for quick editing
                    txtquantity.Focus();
                    txtquantity.SelectAll();

                    // Show a status message
                    MessageBox.Show($"Now editing: {txtproduct.Text}\n\nMake your changes and click 'Update Product' to save, or press Escape to cancel.",
                        "Edit Mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing batch item: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteBatchItem(int rowIndex)
        {
            try
            {
                if (rowIndex >= 0 && dgvbatches.Rows.Count > rowIndex)
                {
                    DataGridViewRow row = dgvbatches.Rows[rowIndex];
                    int batchItemId = Convert.ToInt32(row.Cells["BatchItemID"].Value);
                    string medicineName = row.Cells["MedicineName"].Value?.ToString() ?? "Unknown";

                    DialogResult result = MessageBox.Show(
                        $"Are you sure you want to delete '{medicineName}' from this batch?",
                        "Confirm Delete",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        var batchItemsDl = new BatchItemsDl();
                        bool success = batchItemsDl.DeleteBatchItem(batchItemId);

                        if (success)
                        {
                            MessageBox.Show("Product removed from batch successfully!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Refresh the grid
                            LoadBatchItems();
                        }
                        else
                        {
                            MessageBox.Show("Failed to remove product from batch.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting batch item: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}