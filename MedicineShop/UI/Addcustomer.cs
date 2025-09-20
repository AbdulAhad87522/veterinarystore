using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MedicineShop.BL;
using MedicineShop.BL.Bl;
using MedicineShop.DL;
using MedicineShop.Models;

namespace MedicineShop.UI
{
    public partial class Addcustomer : Form
    {
        private readonly CustomerBl companyBL = new CustomerBl();
        private Customer customer;
        public Addcustomer()
        {
            InitializeComponent();
        }

        public Addcustomer(Customer customerToEdit)
        {
            InitializeComponent();
        }

        private void editbtn_Click(object sender, EventArgs e)
        {
            try
            {
                customer.full_name = txtName.Text;
                customer.Contact = txtContact.Text;
                customer.Address = txtAddress.Text;

                companyBL.UpdateCompany(customer);
                MessageBox.Show("Company updated successfully.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                customer = new Customer
                {
                    full_name = txtName.Text,
                    Contact = txtContact.Text,
                    Address = txtAddress.Text
                };

                companyBL.AddCompany(customer);
                MessageBox.Show("Company added successfully.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
