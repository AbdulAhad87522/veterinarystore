using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fertilizesop.BL.Models;
using fertilizesop.Interfaces.BLInterfaces;
using Microsoft.Extensions.DependencyInjection;

namespace fertilizesop.UI
{
    public partial class supplierform : Form
    {
        private readonly Isupplierbl _customerbl;
        private int customerid = -1;
        private int selectedRowIndex = -1;
        public supplierform(Isupplierbl customerbl)
        {
            InitializeComponent();
            _customerbl = customerbl;
            editpanel.Visible = false;
            UIHelper.StyleGridView(dataGridView1);
        }

        int selectedRowIndex1 = 0; // Declare at the class level

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (txtfirstname.Focused || txtcontact.Focused || txtaddress.Focused)
                {
                    btnsave.PerformClick();
                    return true;
                }

                else if (dataGridView1.Focused && dataGridView1.CurrentCell != null)
                {

                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    if (rowIndex >= 0)
                    {
                        DataGridViewRow row = dataGridView1.Rows[rowIndex];

                        customerid = Convert.ToInt32(row.Cells["Id"].Value);
                        txtfirstname.Text = row.Cells["first_name"].Value.ToString();
                        txtcontact.Text = row.Cells["phonenumber"].Value.ToString();
                        txtaddress.Text = row.Cells["address"].Value.ToString();

                        editpanel.Visible = true;
                        UIHelper.RoundPanelCorners(editpanel, 20);
                        UIHelper.ShowCenteredPanel(this, editpanel);

                        return true;
                    }
                }
            }

            else if (keyData == Keys.Up)
            {
                if (dataGridView1.Visible && dataGridView1.Rows.Count > 0)
                {
                    selectedRowIndex1 = Math.Max(0, selectedRowIndex1 - 1);
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[selectedRowIndex1].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1.Rows[selectedRowIndex1].Cells[0];
                    return true;
                }
            }

            else if (keyData == Keys.Down)
            {
                if (dataGridView1.Visible && dataGridView1.Rows.Count > 0)
                {
                    selectedRowIndex1 = Math.Min(dataGridView1.Rows.Count - 1, selectedRowIndex1 + 1);
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[selectedRowIndex1].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1.Rows[selectedRowIndex1].Cells[0];
                    return true;
                }
            }

            else if (keyData == (Keys.Control | Keys.A))
            {
                Addbutton.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }



        public void load()
        {
            var supplier = _customerbl.getsupplier();
            dataGridView1.Columns.Clear();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DataSource = supplier.OfType<Suppliers>().Select(c=> new { c.Id, c.first_Name, c.Address, c.phonenumber }).ToList();
            dataGridView1.Columns["Id"].Visible = false;
            UIHelper.AddButtonColumn(dataGridView1, "Edit", "Edit", "Edit");
            UIHelper.AddButtonColumn(dataGridView1, "Delete", "Delete", "Delete");
            
        }
        
        

        private void txtfirstname_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtlastname_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtcontact_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtaddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtfirstname.Text))
            {
                MessageBox.Show("Pease enter a name first");
                return;
            }
            string fname = txtfirstname.Text;
            string contact = txtcontact.Text;
            string address = txtaddress.Text;
            try
            {
                var supplier = new Suppliers(customerid, fname, contact, address);
                bool result = _customerbl.updatesupplier(supplier);
                if (result)
                {
                    MessageBox.Show("supplier updated successfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearfields();
                    editpanel.Visible = false;
                    load();
                    dataGridView1.Focus();
                    
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving the supplier " + ex.Message);
            }
        }

        private void editpanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Addbutton_Click(object sender, EventArgs e)
        {
            var form = Program.ServiceProvider.GetRequiredService<Addsupplier>();
            form.ShowDialog();
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            editpanel.Visible = false;
            dataGridView1.Focus();
            //dataGridView1.ClearSelection();
            //dataGridView1.Rows[selectedRowIndex].Selected = true;
            //dataGridView1.CurrentCell = dataGridView1.Rows[selectedRowIndex].Cells[0];
        }
        private void clearfields()
        {
            txtaddress.Clear();
            txtcontact.Clear();
            txtfirstname.Clear();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                return;
            }
            var columnname = dataGridView1.Columns[e.ColumnIndex].Name;
            var rowname = dataGridView1.Rows[e.RowIndex];
            customerid = Convert.ToInt32(rowname.Cells["Id"].Value);
            if (columnname == "Edit")
            {
                editpanel.Visible = true;
                txtaddress.Text = rowname.Cells["address"].Value.ToString();
                txtfirstname.Text = rowname.Cells["first_name"].Value.ToString();
                txtcontact.Text = rowname.Cells["phonenumber"].Value.ToString();
                UIHelper.RoundPanelCorners(editpanel, 20);
                UIHelper.ShowCenteredPanel(this, editpanel);
                txtfirstname.Focus();

            }

            else if (columnname == "Delete")
            {
                try
                {
                    var confirm = MessageBox.Show("Are you sure that you want to delete this supplier", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirm == DialogResult.Yes)
                    {
                        bool result = _customerbl.deletesupplier(customerid);
                        if (result)
                        {
                            MessageBox.Show("Supplier deleted successfully", "Deletion succesfull" , MessageBoxButtons.OK , MessageBoxIcon.Information);
                            load();
                        }
                        else
                        {
                            MessageBox.Show("Error in deleting the supplier");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("error in deleting the supplier" + ex.Message);
                }
            } 
        }

        private void supplierform_Load(object sender, EventArgs e)
        {
            load();
        }

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            string text = txtsearch.Text;
            if(string.IsNullOrEmpty(text))
            {
                load();
                return;
            }
            var sup = _customerbl.searchsupplier(text);
            dataGridView1.Columns.Clear();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; 
            dataGridView1.DataSource = sup.OfType<Suppliers>().Select(c => new { c.Id, c.first_Name, c.Address, c.phonenumber }).ToList();
            dataGridView1.Columns["Id"].Visible = false;
            UIHelper.AddButtonColumn(dataGridView1, "Edit", "Edit", "Edit");
            UIHelper.AddButtonColumn(dataGridView1, "Delete", "Delete", "Deleter");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            load();
        }
    }
}
