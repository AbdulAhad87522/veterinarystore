using fertilizesop.DL;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace fertilizesop.UI
{
    public partial class Customerreturnform : Form
    {
        private readonly Customerbilldl idl;

        public Customerreturnform(Customerbilldl idl)
        {
            InitializeComponent();
            this.idl = idl;

            // Attach event once
            dataGridView2.CellContentClick += dataGridView2_CellContentClick;
            UIHelper.StyleGridView(dataGridView2);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string name = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(name))
                return;

            var list = idl.searchbill(name);
            dataGridView2.DataSource = list;

            // Hide unwanted columns
            if (dataGridView2.Columns.Contains("customer_id"))
                dataGridView2.Columns["customer_id"].Visible = false;
            if (dataGridView2.Columns.Contains("pending"))
                dataGridView2.Columns["pending"].Visible = false;
            if (dataGridView2.Columns.Contains("batch_name"))
                dataGridView2.Columns["batch_name"].Visible = false;

            // ✅ Remove existing select column to prevent duplicates
            if (dataGridView2.Columns.Contains("select"))
                dataGridView2.Columns.Remove("select");

            // ✅ Add fresh button column after binding
            var btnCol = new DataGridViewButtonColumn();
            btnCol.Name = "select";
            btnCol.HeaderText = "";
            btnCol.Text = "Select";
            btnCol.UseColumnTextForButtonValue = true;
            btnCol.Width = 70;
            dataGridView2.Columns.Add(btnCol);
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (dataGridView2.Columns[e.ColumnIndex].Name == "select")
            {
                int billId = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["bill_id"].Value);

                var addReturnForm = Program.ServiceProvider.GetRequiredService<Addreturmform>();
                addReturnForm.SetBillId(billId);
                addReturnForm.ShowDialog();
            }
        }

        private void Customerreturnform_Load(object sender, EventArgs e)
        {

        }
    }
}
