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

namespace fertilizesop.UI
{
    public partial class Addsupplier : Form
    {
        private readonly Isupplierbl _supplierbl;
        private readonly supplierform _sup;
        public Addsupplier(Isupplierbl supplierbl, supplierform sup)
        {
            InitializeComponent();
            _supplierbl = supplierbl;
            _sup = sup;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if(txtfirstname.Focused)
                {
                    txtcontact.Focus();
                    return true;
                }
                else if(txtcontact.Focused)
                {
                    txtaddress.Focus();
                    return true;
                }
                else if (txtaddress.Focused)
                {
                    btnsave.PerformClick();
                    return true;
                }
            }
                return base.ProcessCmdKey(ref msg, keyData);
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
                var supplier = new Suppliers(fname, contact, address);
                bool result = _supplierbl.addsupplier(supplier);
                if (result)
                {
                    MessageBox.Show("supplier saved successfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _sup.load();
                    clearfields();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid data entered");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving the supplier " + ex.Message);
            }
        }

        private void clearfields()
        {
            txtaddress.Clear();
            txtcontact.Clear();
            txtfirstname.Clear();
        }

        private void editpanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}