using fertilizesop.Interfaces.BLInterfaces;
using Microsoft.Extensions.DependencyInjection;
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
    public partial class BatchDetailsform : Form
    {
        private readonly IbatchdetailsBl ibl;
        public BatchDetailsform(IbatchdetailsBl ibl)
        {
            InitializeComponent();
            this.ibl = ibl;
            UIHelper.StyleGridView(dataGridView2);
            this.KeyPreview = true;
            this.KeyDown += CustomerForm_KeyDown;
        }
        private void CustomerForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.R)
            {
                pictureBox1_Click(sender, e);
                e.Handled = true;
            }
         
            //else if (e.Control && e.KeyCode == Keys.A)
            //{


            //    iconButton9.PerformClick();
            //    e.Handled = true;

            //}

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            load();
        }
        private void load()
        {
            var list = ibl.GetAllBatchDetails();
            dataGridView2.DataSource = list;
            dataGridView2.Columns["details_id"].Visible = false;
            dataGridView2.Columns["product_id"].Visible = false;
            dataGridView2.Columns["sale_price"].Visible = false;



        }
        private void BatchDetailsform_Load(object sender, EventArgs e)
        {
            load();
            dataGridView2.Focus();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string text = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                load();
                return;
            }
            var list = ibl.SearchBatchDetails(text);
            dataGridView2.Columns.Clear();
            dataGridView2.DataSource = list;
            dataGridView2.Columns["details_id"].Visible = false;
            dataGridView2.Columns["product_id"].Visible = false;
            dataGridView2.Columns["sale_price"].Visible = false;
        }

        private void iconButton9_Click(object sender, EventArgs e)
        {
            //var f = Program.ServiceProvider.GetRequiredService<Addbatchdetailsform>();
            //f.ShowDialog(this);
        }
    }
}
