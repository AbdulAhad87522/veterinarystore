using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fertilizesop.BL.Bl;
using fertilizesop.BL.Models;
using fertilizesop.BL.Models.persons;
using fertilizesop.DL;
using KIMS;
using Microsoft.Extensions.DependencyInjection;

namespace fertilizesop.UI
{
    public partial class OrderStatus : Form
    {
        DataTable dt;
        OrderBl o = new OrderBl();
        OrderDAL or=new OrderDAL();
        int orderId;
        string supp;
        int selectedRowIndex;
        public OrderStatus()
        {
            InitializeComponent();
            LoadOrderGrid();
            date.Value = DateTime.Now;
            ordersdata.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrderDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            cmbSuppliers.TextUpdate += cmbSuppliers_TextUpdate;
            cmbSuppliers.DropDownStyle = ComboBoxStyle.DropDown;
            //ordersdata.Columns["Order_id"].Visible = false;
            paneledit.Visible = false;
            UIHelper.StyleGridView(dgvOrderDetails);

        }


        private void LoadOrderGrid()
        {
            dt = o.GetOrders(); // replace with actual class
            ordersdata.DataSource = dt;
        }
        private void paneledit_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btncancle1_Click(object sender, EventArgs e)
        {
            paneledit.Visible = false;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().Replace("'", "''");

            if (dt != null)
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = $@"
            Convert(OrderID, 'System.String') LIKE '%{keyword}%' OR
            SupplierName LIKE '%{keyword}%' OR
            [Order Status] LIKE '%{keyword}%'";

                ordersdata.DataSource = dv;
            }
        }

        private void iconButton9_Click(object sender, EventArgs e)
        {
            paneledit.Visible = true;
        }
        private void cmbSuppliers_TextUpdate(object sender, EventArgs e)
        {
            //string searchText = cmbSuppliers.Text.Trim();

            //List<Suppliers> filteredSuppliers =o.GetSuppliers(searchText);

            //if (filteredSuppliers != null && filteredSuppliers.Count > 0)
            //{
            //    cmbSuppliers.BeginUpdate();
            //    cmbSuppliers.DataSource = null;
            //    cmbSuppliers.DataSource = filteredSuppliers;
            //    cmbSuppliers.DisplayMember = "first_Name"; // match your Suppliers class
            //    cmbSuppliers.ValueMember = "Id";
            //    cmbSuppliers.DroppedDown = true;

            //    cmbSuppliers.SelectionStart = searchText.Length;
            //    cmbSuppliers.SelectionLength = 0;
            //    cmbSuppliers.EndUpdate();
            //}
        }

        private void SaveOrder()
        {
            //if (string.IsNullOrWhiteSpace(cmbSuppliers.Text) || cmbSuppliers.SelectedIndex == -1)
            //{
            //    MessageBox.Show("Please select a supplier before proceeding.", "Missing Supplier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    cmbSuppliers.Focus();
            //    return;
            //}

            //string sts=status.Text;
            //supp = cmbSuppliers.Text;
            //int supplierId = Convert.ToInt32(cmbSuppliers.SelectedValue);
            //DateTime datePicker = date.Value;
            //Order or = new Order(supplierId, datePicker,sts);
            //int orderIdd = o.InsertOrder(or);
            //MessageBox.Show($"Order with orderId {orderId} created! Now add products.");
            //paneledit.Visible = false;


            //if (cmbSuppliers.SelectedItem == null || string.IsNullOrWhiteSpace(cmbSuppliers.Text))
            //{
            //    MessageBox.Show("Please select a supplier before proceeding.", "Missing Supplier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    cmbSuppliers.Focus();
            //    return;
            //}

            //// Get selected supplier info
            //var selectedSupplier = cmbSuppliers.SelectedItem as Suppliers;
            //if (selectedSupplier == null)
            //{
            //    MessageBox.Show("Invalid supplier selection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            string supplier = cmbSuppliers.Text.Trim();

            //supp = selectedSupplier.first_Name;
            string sts = status.Text.Trim();
            DateTime datePicker = date.Value;

            Order or = new Order(0, datePicker, sts,supplier);
            int orderIdd = o.InsertOrder(or);

            MessageBox.Show($"Order with orderId {orderIdd} created! Now add products.");
            paneledit.Visible = false;

        }
        private void cmbSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void loadprevious()
        {
            LoadOrderGrid();

            // Restore original view
            ordersdata.Visible = true;
            dgvOrderDetails.Visible = false;
            lblSupplierInfo.Text = "";
            lblSupplierInfo.Visible = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            LoadOrderGrid();

            // Restore original view
            ordersdata.Visible = true;
            dgvOrderDetails.Visible = false;
            lblSupplierInfo.Text = "";
            lblSupplierInfo.Visible = false;
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            if (ordersdata.SelectedRows.Count > 0 || ordersdata.SelectedRows.Count ==1)
            {
                orderId = Convert.ToInt32(ordersdata.SelectedRows[0].Cells["OrderID"].Value);

               
                PlacingOrder placingOrder = new PlacingOrder(orderId, supp);
                placingOrder.Show();
            }
            else
            {
                MessageBox.Show("Please select a row first.");
            }
            
        }
        private void txtBname_TextUpdate(object sender, EventArgs e)
        {
            string searchText = cmbSuppliers.Text.Trim();

            var filteredBatches = DatabaseHelper.Instance.GetSuppliers(searchText);

            if (filteredBatches != null)
            {
                cmbSuppliers.Items.Clear();
                cmbSuppliers.Items.AddRange(filteredBatches.ToArray());
                cmbSuppliers.SelectionStart = searchText.Length;
                cmbSuppliers.SelectionLength = 0;
                cmbSuppliers.DroppedDown = true;
            }
        }
        //keys logic 
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter &&
                (!(ActiveControl is DataGridView) || ActiveControl == ordersdata || ordersdata.Focused))
            {
                btnsearch.PerformClick(); // Simulate button click
                return true; // Mark event as handled
            }

            else if (keyData == (Keys.Control | Keys.S))
            {
                btnsave.PerformClick();
                return true;
            }

            else if (keyData == (Keys.Control | Keys.A))
            {
                iconButton9.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.R))
            {
                loadprevious();
                return true;
            }

            else if (keyData ==Keys.Escape)
            {
                btncancle1.PerformClick();
                return true;
            }

            else if (keyData == Keys.Up)
            {
                if (ordersdata.Visible && selectedRowIndex > 0)
                {
                    selectedRowIndex--;
                    ordersdata.ClearSelection();
                    ordersdata.Rows[selectedRowIndex].Selected = true;
                    return true;
                }
            }
            else if (keyData == Keys.Down)
            {
                if (ordersdata.Visible && selectedRowIndex < ordersdata.Rows.Count - 1)
                {
                    selectedRowIndex++;
                    ordersdata.ClearSelection();
                    ordersdata.Rows[selectedRowIndex].Selected = true;
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData); // Allow default behavior
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            SaveOrder();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ordersdata.SelectedRows.Count > 0)
            {
                int selectedOrderId = Convert.ToInt32(ordersdata.SelectedRows[0].Cells["OrderID"].Value);
                string supplierName = ordersdata.SelectedRows[0].Cells["SupplierName"].Value?.ToString();

                lblSupplierInfo.Text = $"Supplier: {supplierName}";
                lblSupplierInfo.Visible = true;

                // Call BLL to get order details
                DataTable details = or.GetOrderDetailsByOrderId(selectedOrderId); // use OrderBl

                if (details != null && details.Rows.Count > 0)
                {
                    dgvOrderDetails.AutoGenerateColumns = true;
                    dgvOrderDetails.Visible = true;
                    dgvOrderDetails.DataSource = details;
                }
                else
                {
                    // Show empty grid with columns
                    dgvOrderDetails.DataSource = null;
                    dgvOrderDetails.Rows.Clear();
                    dgvOrderDetails.Columns.Clear();

                    dgvOrderDetails.Columns.Add("ProductName", "Product Name");
                    dgvOrderDetails.Columns.Add("Description", "Description");
                    dgvOrderDetails.Columns.Add("Price", "Price");
                    dgvOrderDetails.Columns.Add("Quantity", "Quantity");

                    MessageBox.Show("No products found for this order.");
                }
                // Show details, hide orders grid
                ordersdata.Visible = false;
                dgvOrderDetails.Visible = true;
                dgvOrderDetails.BringToFront();
            }
            else
            {
                MessageBox.Show("Please select an order from the list first.");
            }
        }

        private void OrderStatus_Load(object sender, EventArgs e)
        {
            dgvOrderDetails.Visible = false; // hide details grid initially
            ordersdata.Visible = true;       // show orders grid
           
        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void iconPictureBox3_Click(object sender, EventArgs e)
        {
            var f=Program.ServiceProvider.GetRequiredService<Addsupplier>();
            f.ShowDialog(this);
        }
    }
}
