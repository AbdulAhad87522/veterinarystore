using fertilizesop.BL.Bl;
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
    public partial class Inventorylogform : Form
    {
        private readonly IInventorylogBl ibl;
        public Inventorylogform(IInventorylogBl ibl)
        {
            InitializeComponent(); UIHelper.StyleGridView(dataGridView2);
            UIHelper.StyleGridView(dataGridView2);
            this.ibl = ibl;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (textBox1.Focused)
                {
                    button9.PerformClick();
                    return true;
                }

            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Inventorylogform_Load(object sender, EventArgs e)
        {
            load();
            dataGridView2.Focus();
        }
        private void load()
        {
            var list = ibl.getlog("");
            dataGridView2.DataSource = list;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            string searchTerm = textBox1.Text.Trim();
            string text = textBox1.Text;
            if (string.IsNullOrEmpty(searchTerm))
            {


                load();
            }
            var list = ibl.getlog(searchTerm);
            dataGridView2.DataSource = list;
        }



    }
}
