using fertilizesop.DL;
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
    public partial class Sbilldetailform : Form
    {
        private int billid;
        private readonly SbilldetailsDl sbillDetailsDl = new SbilldetailsDl();
        private readonly SupplierbillDl billDl = new SupplierbillDl();
        public Sbilldetailform(int billid)
        {
            InitializeComponent();
            this.billid = billid;
        }


        private void LoadPriceRecords(int billId)
        {
            try
            {
                var list = sbillDetailsDl.getrecord(billId); // or your injected DL call
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = list;

                // Hide sensitive/internal columns
                dataGridView1.Columns["name"].Visible = false;
                dataGridView1.Columns["suppid"].Visible = false;
                dataGridView1.Columns["bill_id"].Visible = false;
                dataGridView1.Columns["id"].Visible = false;
                dataGridView1.Columns["date"].HeaderText = "Date";
                dataGridView1.Columns["payement"].HeaderText = "Payment";
                dataGridView1.Columns["remarks"].HeaderText = "Remarks";

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading records: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Sbilldetailform_Load(object sender, EventArgs e)
        {
            LoadDetails();
            LoadHeaderInfo();
            LoadPriceRecords(billid);
        }
        private void LoadDetails()
        {
            var details = sbillDetailsDl.getdetails(billid);
            dataGridView2.DataSource = details;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.Columns["bill_detail_id"].Visible = false;
            dataGridView2.Columns["Supplierbill"].Visible = false;

        }
        private void LoadHeaderInfo()
        {
            var billList = billDl.getbills(billid);
            if (billList != null && billList.Count > 0)
            {
                var bill = billList.First();
                lblname.Text = " " + bill.supplier_name;

                lbltotal.Text = " Rs. " + bill.total_price.ToString("N2");
                lblpaid.Text = " Rs. " + bill.paid_price.ToString("N2");
                lblpending.Text = " Rs. " + bill.pending.ToString("N2");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lblname_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toplbl_Click(object sender, EventArgs e)
        {

        }

        private void lblpending_Click(object sender, EventArgs e)
        {

        }

        private void lblpaid_Click(object sender, EventArgs e)
        {

        }

        private void lbltotal_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
