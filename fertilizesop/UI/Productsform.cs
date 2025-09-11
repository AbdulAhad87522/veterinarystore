using fertilizesop.BL.Models;
using fertilizesop.Interfaces.BLInterfaces;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fertilizesop.UI
{
    public partial class Productsform : Form
    {
        private int selectedproductid;
        private readonly IProductBl ibl;
        public Productsform(IProductBl ibl)
        {
            InitializeComponent();
            this.ibl = ibl;
            UIHelper.StyleGridView(dataGridView1);
            editpanel.Visible = false;
            this.KeyPreview = true;
            this.KeyDown += CustomerForm_KeyDown;
        }
        private void CustomerForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.R)
            {
                // Ctrl+R → Refresh
                pictureBox1_Click(sender, e);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                // Ctrl+S → Save
                if (editpanel.Visible)
                {
                    btnsave.PerformClick();
                    e.Handled = true;
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                // Esc → Close panel
                if (editpanel.Visible)
                {
                    editpanel.Visible = false;
                    e.Handled = true;
                }
            }
            else if (e.KeyCode == Keys.Enter)
            {
                // Enter → Edit selected row
                if (!editpanel.Visible && dataGridView1.Focused)
                {
                    editSelectedCustomer();
                    e.Handled = true;
                }
            }
        }
        private void editSelectedCustomer()
        {
            if (dataGridView1.CurrentRow != null)
            {
                var row = dataGridView1.CurrentRow;
                selectedproductid = Convert.ToInt32(row.Cells["id"].Value);
                txtname.Text = row.Cells["Name"].Value?.ToString();
                txtquantity.Text = row.Cells["quantity"].Value?.ToString();
                txtdescription.Text = row.Cells["Description"].Value?.ToString();
                txtprice.Text = row.Cells["Price"].Value?.ToString();


                UIHelper.RoundPanelCorners(editpanel, 20);
                UIHelper.ShowCenteredPanel(this, editpanel);
            }
        }
        private void load()
        {
            var list = ibl.GetProducts();
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = list.Select(p => new { p.Name, p.Price, p.quantity, p.Description, p.Id }).ToList();
            dataGridView1.Columns["Id"].Visible = false;
            UIHelper.AddButtonColumn(dataGridView1, "Edit", "Edit", "Edit");
        }
        private void Productsform_Load(object sender, EventArgs e)
        {
            load();
            dataGridView1.Focus();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var columnName = dataGridView1.Columns[e.ColumnIndex].Name;
            var row = dataGridView1.Rows[e.RowIndex];
            selectedproductid = Convert.ToInt32(row.Cells["id"].Value);

            if (columnName == "Edit")
            {
                txtname.Text = row.Cells["Name"].Value?.ToString();
                txtquantity.Text = row.Cells["quantity"].Value?.ToString();
                txtdescription.Text = row.Cells["Description"].Value?.ToString();
                txtprice.Text = row.Cells["Price"].Value?.ToString();


                UIHelper.RoundPanelCorners(editpanel, 20);
                UIHelper.ShowCenteredPanel(this, editpanel);
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            string name= txtname.Text.Trim();
            string description = txtdescription.Text.Trim();
            int quantity=Convert.ToInt32(txtquantity.Text.Trim());
            decimal price=Convert.ToDecimal(txtprice.Text.Trim());
            try
            {
                var p = new Products(selectedproductid, name, description, price, quantity);
                var result = ibl.update(p);
                if (result)
                {
                    MessageBox.Show("Product Updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtname.Clear();
                    txtdescription.Clear();
                    txtquantity.Clear();
                    txtprice.Clear();
                    editpanel.Visible=false;
                    load();
                }
                else
                {
                    MessageBox.Show("Failed to Update Product. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        

        private void btncancel_Click(object sender, EventArgs e)
        {
            editpanel.Visible = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            load();
            dataGridView1.Focus();
        }

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            string text=txtsearch.Text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                load();
                dataGridView1.Focus();
                return;
            }
            var list = ibl.searchproducts(text);
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = list.Select(p => new { p.Name, p.Price, p.quantity, p.Description, p.Id }).ToList();
            dataGridView1.Columns["Id"].Visible = false;
            UIHelper.AddButtonColumn(dataGridView1, "Edit", "Edit", "Edit");
        }

        private void Addbutton_Click(object sender, EventArgs e)
        {
            var f = Program.ServiceProvider.GetRequiredService<Addproductform>();
            f.ShowDialog(this);
        }
    }
}
