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
            txtPrice.Text = _medicine.SalePrice.ToString();

            cmbCompany.SelectedValue = _medicine.CompanyId;
            cmbCategory.SelectedValue = _medicine.CategoryId;
            pckcmb.SelectedValue = _medicine.PackingId;
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
            _medicine.SalePrice = decimal.TryParse(txtPrice.Text, out decimal price) ? price : 0;
            if (cmbCompany.SelectedItem is ComboItem selectedCompany)
                _medicine.CompanyId = selectedCompany.Id;

            if (cmbCategory.SelectedItem is ComboItem selectedCategory)
                _medicine.CategoryId = selectedCategory.Id;

            if (pckcmb.SelectedItem is ComboItem selectedPacking)
                _medicine.PackingId = selectedPacking.Id;
        }

        // For searchable combobox (optional)
        private void cmbCompany_TextChanged(object sender, EventArgs e)
        {
            string searchText = cmbCompany.Text.Trim();

            var companies = _medicineBL.GetCompanyList(searchText);

            if (companies != null && companies.Count > 0)
            {
                cmbCompany.Items.Clear();
                foreach (var c in companies)
                    cmbCompany.Items.Add(c);

                cmbCompany.SelectionStart = cmbCompany.Text.Length;
                cmbCompany.DroppedDown = true;
                Cursor.Current = Cursors.Default;
            }
        }


        private void comboCategory_TextChanged(object sender, EventArgs e)
        {
            string searchText = cmbCategory.Text.Trim();

            var categories = _medicineBL.GetCategoryList(searchText);

            if (categories != null && categories.Count > 0)
            {
                cmbCategory.Items.Clear();
                foreach (var c in categories)
                    cmbCategory.Items.Add(c);

                cmbCategory.SelectionStart = cmbCategory.Text.Length;
                cmbCategory.DroppedDown = true;
                Cursor.Current = Cursors.Default;
            }
        }

        private void comboPacking_TextChanged(object sender, EventArgs e)
        {
            string searchText = pckcmb.Text.Trim();

            var packing = _medicineBL.GetPackingList(searchText);

            if (packing != null && packing.Count > 0)
            {
                pckcmb.Items.Clear();
                foreach (var c in packing)
                    pckcmb.Items.Add(c);

                pckcmb.SelectionStart = cmbCategory.Text.Length;
                pckcmb.DroppedDown = true;
                Cursor.Current = Cursors.Default;
            }
        }


        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void AddMedicine_Load(object sender, EventArgs e)
        {
           
        }
    }
}
