using MedicineShop.BL.Bl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TechStore.UI;

namespace MedicineShop.UI
{
    public partial class CompanyBillDetails : Form
    {
        private int billId;
        private readonly ICompanyBillBl ibl;
        public CompanyBillDetails(int billId,ICompanyBillBl ibl)
        {
            InitializeComponent();
            this.billId = billId;
            this.ibl = ibl;
            UIHelper.StyleGridView(dataGridView1);
            UIHelper.StyleGridView(dataGridView2);
        }

        private void CompanyBillDetails_Load(object sender, EventArgs e)
        {
            LoadBillDetails(billId);
            LoadHeaderInfo();
        }
        private void LoadBillDetails(int billId)
        {
            //Assuming you have a method in your BL layer to get bill details by billId
            var billDetails = ibl.GetPaymentRecords(billId);
            dataGridView1.DataSource = billDetails;
            dataGridView1.Columns["CompanyId"].Visible = false;
            dataGridView1.Columns["PaymentId"].Visible = false;
            dataGridView1.Columns["Status"].Visible = false;
            dataGridView1.Columns["TotalPrice"].Visible = false;
            dataGridView1.Columns["Paid"].Visible = false;
            dataGridView1.Columns["RemainingBalance"].Visible = false;
            dataGridView1.Columns["CompanyName"].Visible = false;
            dataGridView1.Columns["BatchName"].Visible = false;
            var batchesdetails=ibl.getrecord(billId);
            dataGridView2.DataSource = batchesdetails;
            dataGridView2.Columns["CompanyId"].Visible = false;
            dataGridView2.Columns["PaymentId"].Visible = false;
            dataGridView2.Columns["RemainingBalance"].Visible = false;
            dataGridView2.Columns["CompanyName"].Visible = false;

        }
        private void LoadHeaderInfo()
        {
            var billList = ibl.GetCompanyBillById(billId);
            if (billList != null && billList.Count > 0)
            {
                var bill = billList.First();
                lblname.Text = " " + bill.company_name;

                lbltotal.Text = " Rs. " + bill.total_price.ToString("N2");
                lblpaid.Text = " Rs. " + bill.paid.ToString("N2");
                lblpending.Text = " Rs. " + bill.remaining.ToString("N2");
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            //var f = new CompanyBill(SelectedId, ibl);
            //this.Close();
            //f.ShowDialog();
        }
    }
}
