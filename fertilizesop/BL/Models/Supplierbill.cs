using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Models
{
    public class Supplierbill
    {
        public Supplierbill(int v1, string v2, string v3, decimal v4, DateTime dateTime, decimal v5)
        {
            bill_id = v1;
            supplier_name = v2;
            batch_name = v3;
            date = dateTime;
            paid_price = v5;
            total_price = v4;
        }
        public Supplierbill(string batch_name, decimal paid, string supplier_name, decimal total_price)
        {
            this.batch_name = batch_name;
            paid_price = paid;
            this.supplier_name = supplier_name;
            this.total_price = total_price;
        }
        public Supplierbill(int bill_id, string supplier_name, int supplier_id, DateTime date, decimal total_price, decimal paid_price, string batch_name, int batch_id)
        {
            this.bill_id = bill_id;
            this.supplier_name = supplier_name;
            this.supplier_id = supplier_id;
            this.date = date;
            this.total_price = total_price;
            this.paid_price = paid_price;
            this.batch_name = batch_name;
            this.batch_id = batch_id;
        }
        public Supplierbill(int bill_id, string supplier_name, DateTime date, decimal total_price, decimal paid_price, string batch_name, decimal pending, int batch_id, int supplier_id, string status)
        {
            this.bill_id = bill_id;
            this.supplier_name = supplier_name;
            this.date = date;
            this.total_price = total_price;
            this.paid_price = paid_price;
            this.batch_name = batch_name;
            this.pending = pending;
            this.batch_id = batch_id;
            this.supplier_id = supplier_id;
            this.status = status;
        }

        public int bill_id { get; private set; }
        public string supplier_name { get; private set; }
        public int supplier_id { get; private set; }
        public DateTime date { get; private set; }
        public decimal total_price { get; private set; }
        public decimal paid_price { get; private set; }
        public string batch_name { get; private set; }
        public int batch_id { get; private set; }
        public decimal pending { get; private set; }
        public string status { get; private set; }

    }
}
