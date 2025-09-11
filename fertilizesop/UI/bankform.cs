using fertilizesop.BL.Bl;
using fertilizesop.DL;
using fertilizesop.Interfaces.BLInterfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fertilizesop.UI
{
    public partial class bankform : Form
    {
        private readonly IBankBl bankBl;
        private readonly ItransactionBL transactionBl;
        private bool showingHistory = false;
        private int currentBankId = -1;
        public bankform(ItransactionBL ibl, IBankBl idl)
        {
            InitializeComponent();
            this.bankBl = idl ?? throw new ArgumentNullException(nameof(idl));
            this.transactionBl = ibl ?? throw new ArgumentNullException(nameof(ibl));
            panelbank.Visible = false;
            PanelBill.Visible = false;
            UIHelper.StyleGridView(dataGridView2);
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            string bankname = txtname.Text.Trim();
            string remaining = txtamount.Text.Trim();
            try
            {
                bool result = bankBl.AddBank(bankname, Convert.ToDecimal(remaining));
                if (result)
                {
                    MessageBox.Show("Bank added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    panelbank.Visible = false;
                }
                else
                {
                    MessageBox.Show("Failed to add bank. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("Missing required data: " + ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Invalid input: " + ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void iconButton9_Click(object sender, EventArgs e)
        {
            UIHelper.RoundPanelCorners(panelbank, 20);
            UIHelper.ShowCenteredPanel(this, panelbank);

        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            if (panelbank.Visible)
            {
                panelbank.Visible = false;
            }

            UIHelper.RoundPanelCorners(PanelBill, 20);
            UIHelper.ShowCenteredPanel(this, PanelBill);

            // Load bank names into combo box
            LoadBankNames();
        }


        private void bankform_Load(object sender, EventArgs e)
        {
            LoadBankList();
            dataGridView2.KeyDown += DataGridView2_KeyDown;
            this.KeyPreview = true; // Enable form-level shortcut handling
            this.KeyDown += Bankform_KeyDown;
        }
        private void Bankform_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (panelbank.Visible)
                    btnsave.PerformClick();
                else if (PanelBill.Visible)
                    btnsavedetail.PerformClick();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                if (panelbank.Visible)
                    panelbank.Visible = false;
                if (PanelBill.Visible)
                    PanelBill.Visible = false;
            }
            else if (e.Control&& e.KeyCode==Keys.R)
            {
                LoadBankList();
                textBox1.Clear();// Refresh bank list
            }
        }

        private void DataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true; // Prevent default editing mode

                if (dataGridView2.SelectedRows.Count > 0)
                {
                    if (!showingHistory) // If in bank list view
                    {
                        currentBankId = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["BankId"].Value);
                        string bankName = dataGridView2.SelectedRows[0].Cells["BankName"].Value.ToString();
                        LoadTransactionHistory(currentBankId, bankName);
                        showingHistory = true;
                    }
                    else
                    {
                        // If in history view - here you could show transaction details if needed
                        MessageBox.Show("Transaction selected: " +
                            dataGridView2.SelectedRows[0].Cells["TransactionType"].Value.ToString());
                    }
                }
            }
        }

        private void LoadBankNames()
        {
            try
            {
                var bankNames = bankBl.GetAllBankNames();
                cmbbank.DataSource = bankNames;
                cmbbank.SelectedIndex = -1; // No selection initially
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load bank names: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnsavedetail_Click(object sender, EventArgs e)
        {
            if (cmbbank.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a bank.");
                return;
            }

            if (cmbtype.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a transaction type.");
                return;
            }

            if (!decimal.TryParse(txtbamount.Text.Trim(), out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid amount.");
                return;
            }

            string bankName = cmbbank.SelectedItem.ToString();
            string type = cmbtype.SelectedItem.ToString();
            DateTime date = txtDate.Value;

            try
            {
                bool result = transactionBl.AddTransaction(bankName, type, amount, date);
                if (result)
                {
                    MessageBox.Show("Transaction saved successfully.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh remaining balance after transaction
                    decimal newBalance = bankBl.GetRemainingBalance(bankName);
                    txtremaining.Text = newBalance.ToString("N2");

                    // Clear amount box
                    txtbamount.Clear();
                }
                else
                {
                    MessageBox.Show("Failed to save transaction.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btncanceldetail_Click(object sender, EventArgs e)
        {
            PanelBill.Visible = false;
        }

        private void cmbbank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbbank.SelectedIndex >= 0)
            {
                string selectedBank = cmbbank.SelectedItem.ToString();
                try
                {
                    decimal balance = bankBl.GetRemainingBalance(selectedBank);
                    txtremaining.Text = balance.ToString("N2");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load balance: " + ex.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void LoadBankList()
        {
            try
            {
                var banks = bankBl.GetAllBanks();
                dataGridView2.DataSource = banks;

                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView2.Columns["BankId"].Visible = false; // Hide ID
                dataGridView2.Columns["BankName"].HeaderText = "Bank Name";
                dataGridView2.Columns["RemainingBalance"].HeaderText = "Remaining Balance";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load bank list: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTransactionHistory(int bankId, string bankName)
        {
            try
            {
                var history = transactionBl.GetBankTransactionHistory(bankId);

                if (history.Count == 0)
                {
                    MessageBox.Show($"No transactions found for bank: {bankName}",
                        "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dataGridView2.DataSource = history;

                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView2.Columns["TransactionId"].Visible = false; // Hide ID
                dataGridView2.Columns["BankId"].Visible = false;         // Hide bankId
                dataGridView2.Columns["TransactionType"].HeaderText = "Type";
                dataGridView2.Columns["Amount"].HeaderText = "Amount";
                dataGridView2.Columns["TransactionDate"].HeaderText = "Date";
                dataGridView2.Columns["BankName"].HeaderText = "Bank Name";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load transaction history: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnbill_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                currentBankId = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["BankId"].Value);
                string bankName = dataGridView2.SelectedRows[0].Cells["BankName"].Value.ToString();
                LoadTransactionHistory(currentBankId, bankName);
                showingHistory = true;
            }
            else
            {
                MessageBox.Show("Please select a bank first.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            LoadBankList();
            textBox1.Clear(); // Clear search box
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string keyword = textBox1.Text.Trim();

            // If keyword is empty, reload full data
            if (string.IsNullOrEmpty(keyword))
            {
                // If grid is showing banks
                if (dataGridView2.DataSource is List<fertilizesop.BL.Models.Bank>)
                {
                    LoadBankList();
                }
                else // grid is showing transactions
                {
                    if (dataGridView2.SelectedRows.Count > 0)
                    {
                        int bankId = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["BankId"].Value);
                        var history = transactionBl.GetBankTransactionHistory(bankId);
                        dataGridView2.DataSource = history;
                    }
                }
                return;
            }

            try
            {
                // If grid currently showing banks
                if (dataGridView2.DataSource is List<fertilizesop.BL.Models.Bank>)
                {
                    var result = BankDAL.SearchBanks(keyword);
                    dataGridView2.DataSource = result;
                }
                else // grid showing transactions
                {
                    var result = TransactionDAL.SearchTransactions(keyword);
                    dataGridView2.DataSource = result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}