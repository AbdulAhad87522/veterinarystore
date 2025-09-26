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
using Org.BouncyCastle.Pqc.Crypto.Lms;

namespace MedicineShop.UI
{
    public partial class AddMedicine : Form
    {
        private readonly MedicineBL _medicineBL = new MedicineBL();
        private readonly Medicine _medicine;
        private readonly bool _isEdit;
        private List<string> _allProductNames;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (keyData == Keys.Enter)
                {
                    if (txtName.Focused)
                    {
                        pckcmb.Focus();
                        return true;
                    }

                    else if (pckcmb.Focused)
                    {
                        txtPrice.Focus();
                        return true;

                    }

                    else if (txtPrice.Focused)
                    {
                        cmbCategory.Focus();
                        return true;

                    }

                    else if (cmbCategory.Focused)
                    {
                        cmbCompany.Focus();
                        return true;

                    }
                    else if (cmbCompany.Focused)
                    {
                        txtDesc.Focus();
                        return true;

                    }
                    else if (txtDesc.Focused)
                    {
                        txtDesc.Focus();
                        return true;

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error in event listener", ex.Message);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public AddMedicine(Medicine med = null)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            BindCompanies();
            BindCategories();
            BindPackings();

            if (med != null)
            {
                _medicine = med;
                _isEdit = true;
                btnAdd.Visible = false;
                btnUpdate.Visible = true;

                // Fill form immediately after data binding
                FillForm();
            }
            else
            {
                _medicine = new Medicine();
                _isEdit = false;
                btnAdd.Visible = true;
                btnUpdate.Visible = false;
            }
        }

        // Remove the FillForm call from Load event
        private void AddMedicine_Load(object sender, EventArgs e)
        {
            // Remove the if (_isEdit) block from here
        }


        private void BindCompanies()
        {
            var list = _medicineBL.GetCompanyList(""); // full list
            cmbCompany.DisplayMember = "CompanyName";
            cmbCompany.ValueMember = "CompanyId";
            cmbCompany.DataSource = list;
        }

        private void BindCategories()
        {
            var list = _medicineBL.GetCategoryList("");
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "CategoryId";
            cmbCategory.DataSource = list;
        }

        private void BindPackings()
        {
            var list = _medicineBL.GetPackingList("");
            pckcmb.DisplayMember = "PackingName";
            pckcmb.ValueMember = "PackingId";
            pckcmb.DataSource = list;
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

        //private void BindSearchableCombo<T>(
        //ComboBox combo,
        //List<T> list,
        //string displayMember,
        //string valueMember,
        //string typedText)
        //{
        //    string oldText = typedText;

        //    combo.BeginUpdate();
        //    combo.DataSource = null;

        //    if (list.Count > 0)
        //    {
        //        combo.DisplayMember = displayMember;
        //        combo.ValueMember = valueMember;
        //        combo.DataSource = list;

        //        combo.DroppedDown = true;

        //        // Restore typed text + cursor
        //        combo.Text = oldText;
        //        combo.SelectionStart = oldText.Length;
        //        combo.SelectionLength = 0;

               
        //    }
        //    else
        //    {
        //        combo.DroppedDown = false;
        //    }

        //    combo.EndUpdate();
        //}

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

            if (cmbCompany.SelectedItem is Company selectedCompany)
                _medicine.CompanyId = selectedCompany.CompanyId;

            if (cmbCategory.SelectedItem is Category selectedCategory)
                _medicine.CategoryId = selectedCategory.CategoryId;

            if (pckcmb.SelectedItem is Packing selectedPacking)
                _medicine.PackingId = selectedPacking.PackingId;
        }


        // For searchable combobox (optional)
        private void cmbCompany_TextChanged(object sender, EventArgs e)
        {
            
        }


        private void comboCategory_TextChanged(object sender, EventArgs e)
        {
            
        }

        //private void cmbCategory_TextUpdate(object sender, EventArgs e)
        //{
        //    string text = cmbCategory.Text.Trim();
        //    var list = _medicineBL.GetCategoryList(text); // List<Category>

        //    BindSearchableCombo(cmbCategory, list, "CategoryName", "CategoryId", text);
        //}


        private void comboPacking_TextChanged(object sender, EventArgs e)
        {
           
        }


        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        //private void AddMedicine_Load(object sender, EventArgs e)
        //{
        //    if (_isEdit)
        //    {
        //        FillForm();
        //    }
        //}
        

        //private void cmbCompany_TextUpdate(object sender, EventArgs e)
        //{
        //    string text = cmbCompany.Text.Trim();
        //    var list = _medicineBL.GetCompanyList(text); // List<Company>

        //    BindSearchableCombo(cmbCompany, list, "CompanyName", "CompanyId", text);
        //}

        //private void pckcmb_TextUpdate(object sender, EventArgs e)
        //{
        //    string text = pckcmb.Text.Trim();
        //    var list = _medicineBL.GetPackingList(text); // List<Packing>

        //    BindSearchableCombo(pckcmb, list, "PackingName", "PackingId", text);
        //}

        private void pckcmb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}
