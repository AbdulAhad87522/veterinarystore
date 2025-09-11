using fertilizesop.BL.Bl;
using fertilizesop.BL.Models;
using fertilizesop.DL;
using fertilizesop.Interfaces.BLInterfaces;
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
    public partial class Addproductform : Form
    {
        private readonly IProductBl ibl;
        public Addproductform(IProductBl ibl)
        {
            InitializeComponent();
            this.ibl = ibl;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (txtname.Focused)
                {
                    txtprice.Focus();
                    return true;
                }
                else if (txtprice.Focused)
                {
                    txtquantity.Focus();
                    return true;
                }
                else if (txtquantity.Focused)
                {
                    txtdescription.Focus();
                    return true;
                }
                else if (txtdescription.Focused)
                {
                    btnsave.PerformClick();
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            string name=txtname.Text.Trim();
            decimal price = Convert.ToDecimal(txtprice.Text.Trim());
            int quantity=Convert.ToInt32(txtquantity.Text.Trim());
            string description= txtdescription.Text.Trim();
            try
            {
                var products = new Products(0, name, description, price, quantity);
                var result = ibl.Addproduct(products);
                if (result)
                {
                    MessageBox.Show("Product added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtname.Clear();
                    txtdescription.Clear();
                    txtquantity.Clear();
                    txtprice.Clear();
                }
                else
                {
                    MessageBox.Show("Failed to add Product. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void txtprice_TextChanged(object sender, EventArgs e)
        {

        }
    }
    }