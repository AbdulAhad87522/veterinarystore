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
using MedicineShop.DL;
using MedicineShop.Models;

namespace MedicineShop.UI
{
    public partial class AddMedicine : Form
    {
        private readonly MedicineBL _medicineBL = new MedicineBL();
        private readonly Medicine _medicine;
        private readonly bool _isEdit;
        private DataTable _allCompanies;
        private DataTable _allCategories;


        public AddMedicine(Medicine med = null)
        {
            InitializeComponent();
            


            if (med != null)
            {
                _medicine = med;
                _isEdit = true;
                FillForm();
                btnAdd.Visible = false;
                btnUpdate.Visible = true;
            }
            else
            {
                _medicine = new Medicine();
                _isEdit = false;
                btnAdd.Visible = true;
                btnUpdate.Visible = false;
            }
        }


        private void FillForm()
        {
            txtName.Text = _medicine.Name;
            txtDesc.Text = _medicine.Description;
            txtPacking.Text = _medicine.Packing;
            txtPrice.Text = _medicine.SalePrice.ToString();

            cmbCompany.SelectedValue = _medicine.CompanyId;
            cmbCategory.SelectedValue = _medicine.CategoryId;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SetMedicineFromForm();
            if (_medicineBL.AddMedicine(_medicine) > 0)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SetMedicineFromForm();
            if (_medicineBL.UpdateMedicine(_medicine) > 0)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void SetMedicineFromForm()
        {
            _medicine.Name = txtName.Text.Trim();
            _medicine.Description = txtDesc.Text.Trim();
            _medicine.Packing = txtPacking.Text.Trim();
            _medicine.SalePrice = decimal.TryParse(txtPrice.Text, out decimal price) ? price : 0;
            _medicine.CompanyId = Convert.ToInt32(cmbCompany.SelectedValue);
            _medicine.CategoryId = Convert.ToInt32(cmbCategory.SelectedValue);
        }

        // For searchable combobox (optional)
        private void cmbCompany_TextChanged(object sender, EventArgs e)
        {
           
        }


        private void comboCategory_TextChanged(object sender, EventArgs e)
        {
            
        }
    

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void AddMedicine_Load(object sender, EventArgs e)
        {
            _allCompanies = _medicineBL.GetCompanies("");
            _allCategories = _medicineBL.GetCategories("");

            cmbCompany.DropDownStyle = ComboBoxStyle.DropDown;
            cmbCompany.AutoCompleteMode = AutoCompleteMode.None;

            cmbCategory.DropDownStyle = ComboBoxStyle.DropDown;
            cmbCategory.AutoCompleteMode = AutoCompleteMode.None;

            // Start with full list
            cmbCompany.DataSource = _allCompanies.Copy();
            cmbCompany.DisplayMember = "company_name";
            cmbCompany.ValueMember = "company_id";
            cmbCompany.SelectedIndex = -1;

            cmbCategory.DataSource = _allCategories.Copy();
            cmbCategory.DisplayMember = "category_name";
            cmbCategory.ValueMember = "category_id";
            cmbCategory.SelectedIndex = -1;

            //cmbCompany.KeyUp += cmbCompany_KeyUp;
            //cmbCategory.KeyUp += cmbCategory_KeyUp;

        }
    }
}
