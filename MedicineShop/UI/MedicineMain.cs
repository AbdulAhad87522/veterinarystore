using System;
using System.Data;
using System.Windows.Forms;
using MedicineShop.BL;
using MedicineShop.Models;
using MedicineShop.UI;

namespace MedicineShop
{
    public partial class MedicineMain : Form
    {
        private readonly MedicineBL _medicineBL = new MedicineBL();

        public MedicineMain()
        {
            InitializeComponent();
            LoadMedicines();
            CustomizeGrid();
        }

        private void LoadMedicines()
        {
            dataGridView1.DataSource = _medicineBL.GetMedicines();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddMedicine form = new AddMedicine();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadMedicines();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Select a medicine to edit.");
                return;
            }

            Medicine med = new Medicine
            {
                ProductId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["product_id"].Value),
                Name = dataGridView1.CurrentRow.Cells["name"].Value.ToString(),
                Description = dataGridView1.CurrentRow.Cells["description"].Value.ToString(),
                PackingId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["packing_id"].Value),
                SalePrice = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["sale_price"].Value),
                CategoryId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Category_id"].Value)

            };

            AddMedicine form = new AddMedicine(med);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadMedicines();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Select a medicine to delete.");
                return;
            }

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["product_id"].Value);
            DialogResult confirm = MessageBox.Show("Are you sure?", "Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                int rows = _medicineBL.DeleteMedicine(id);
                if (rows > 0) LoadMedicines();
            }
        }

        private void CustomizeGrid()
        {
            var grid = dataGridView1;
            grid.BorderStyle = BorderStyle.None;
            grid.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(238, 239, 249);
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.SeaGreen;
            grid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            grid.BackgroundColor = System.Drawing.Color.White;

            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(20, 25, 72);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);

            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10);
            grid.RowTemplate.Height = 35;
            grid.AllowUserToAddRows = false;
            grid.ReadOnly = true;

            if (grid.Columns.Contains("company_id")) grid.Columns["company_id"].Visible = false;
            if (grid.Columns.Contains("Category_id")) grid.Columns["Category_id"].Visible = false;
            if (grid.Columns.Contains("packing_id")) grid.Columns["packing_id"].Visible = false;
        }



            private void MedicineMain_Load(object sender, EventArgs e)
            {

            }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Live search as user types
            string keyword = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
                LoadMedicines();
            else
                dataGridView1.DataSource = _medicineBL.SearchMedicines(keyword);
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            AddCategory addCategory = new AddCategory();
            addCategory.ShowDialog();
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            AddPacking addPacking = new AddPacking();
            addPacking.ShowDialog();
        }
    } 
}
