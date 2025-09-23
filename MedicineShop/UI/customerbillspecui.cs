using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MedicineShop.BL.Bl;
using TechStore.UI;

namespace MedicineShop.UI
{
    public partial class customerbillspecui : Form
    {
        private int billId;
        private readonly Icustomerbillbl ibl;
        public customerbillspecui(int billId, Icustomerbillbl ibl)
        {
            InitializeComponent();
            this.ibl = ibl;
            this.billId = billId;
            UIHelper.StyleGridView(dataGridView1);
            UIHelper.StyleGridView(dataGridView2);
        }

        private void customerbillspecui_Load(object sender, EventArgs e)
        {
            LoadBillDetails(billId);
            LoadHeaderInfo();
        }

        private void LoadBillDetails(int billId)
        {
            //Assuming you have a method in your BL layer to get bill details by billId
            var billDetails = ibl.GetcustPaymentRecords(billId);
            dataGridView1.DataSource = billDetails;
            dataGridView1.Columns["customerId"].Visible = false;
            dataGridView1.Columns["PaymentId"].Visible = false;
            dataGridView1.Columns["Status"].Visible = false;
            dataGridView1.Columns["TotalPrice"].Visible = false;
            dataGridView1.Columns["Paid"].Visible = false;
            dataGridView1.Columns["RemainingBalance"].Visible = false;
            dataGridView1.Columns["CustomerName"].Visible = false;
            var batchesdetails = ibl.getcustrecord(billId);
            dataGridView2.DataSource = batchesdetails;
            dataGridView2.Columns["customerId"].Visible = false;
            dataGridView2.Columns["PaymentId"].Visible = false;
            dataGridView2.Columns["RemainingBalance"].Visible = false;
            dataGridView2.Columns["CustomerName"].Visible = false;
        }

        private void LoadHeaderInfo()
        {
            var billList = ibl.GetCustomerBillById(billId);
            if (billList != null && billList.Count > 0)
            {
                var bill = billList.First();
                lblname.Text = " " + bill.full_name;

                lbltotal.Text = " Rs. " + bill.total_amount.ToString("N2");
                lblpaid.Text = " Rs. " + bill.paid.ToString("N2");
                lblpending.Text = " Rs. " + bill.remaining.ToString("N2");
            }
        }
    }
}
