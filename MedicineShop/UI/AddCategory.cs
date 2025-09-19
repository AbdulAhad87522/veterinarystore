using MedicineShop.BL;
using MedicineShop.Models;
using System;
using System.Windows.Forms;

namespace MedicineShop.UI
{
    public partial class AddCategory : Form
    {
        private readonly CategoryBL _categoryBL = new CategoryBL();

        public AddCategory()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Category category = new Category
                {
                    CategoryName = txtCategoryName.Text.Trim()
                };

                int result = _categoryBL.AddCategory(category);
                if (result > 0)
                {
                    MessageBox.Show("Category added successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Category not added.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
