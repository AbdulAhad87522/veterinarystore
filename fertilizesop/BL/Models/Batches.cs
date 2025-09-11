using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Models
{
    public class Batches
    {
        public int batch_id { get;private set; }
        public string batch_name { get;private set; }   
        public DateTime received_date { get;private set; }
        public string supplier_name { get;private set; }
        public int supplier_id { get;private set; }
        public string phone { get;private set; }
        public Batches(int batch_id, string batch_name, DateTime received_date, string supplier_name, string phone, int supplier_id)
        {
            this.batch_id = batch_id;
            this.batch_name = batch_name;
            this.received_date = received_date;
            this.supplier_name = supplier_name;
            this.phone = phone;
            this.supplier_id = supplier_id;
        }
    }
}
