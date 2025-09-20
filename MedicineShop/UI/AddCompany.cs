using System;
using System.Windows.Forms;
using System.Xml.Linq;
using MedicineShop.BL;
using MedicineShop.Models;

namespace MedicineShop.UI
{
    public partial class AddCompany : Form
    {
        private readonly CompanyBL companyBL = new CompanyBL();
        private Company company;

        public AddCompany()
        {
            InitializeComponent();
            editbtn.Visible = false;  // Add mode
        }

        public AddCompany(Company companyToEdit)
        {
            InitializeComponent();
            company = companyToEdit;
            txtName.Text = company.CompanyName;
            txtContact.Text = company.Contact;
            txtAddress.Text = company.Address;

            addbtn.Visible = false;   // Edit mode
            editbtn.Visible = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                company = new Company
                {
                    CompanyName = txtName.Text,
                    Contact = txtContact.Text,
                    Address = txtAddress.Text
                };

                companyBL.AddCompany(company);
                MessageBox.Show("Company added successfully.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                company.CompanyName = txtName.Text;
                company.Contact = txtContact.Text;
                company.Address = txtAddress.Text;

                companyBL.UpdateCompany(company);
                MessageBox.Show("Company updated successfully.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
