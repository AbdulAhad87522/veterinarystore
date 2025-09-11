using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Models
{
    public class Supplierpayment
    {
        public int bill_id { get; private set; }
        public int bill_detail_id { get; private set; }
        public int quantity { get; private set; }
        public string product_name { get; private set; }
        public string descp { get; private set; }
        public Supplierbill Supplierbill { get; private set; }

        public Supplierpayment(int bill_id, int bill_details_id, int quantity, string product_name, string descp)
        {
            this.bill_id = bill_id;
            this.bill_detail_id = bill_details_id;
            this.quantity = quantity;
            this.product_name = product_name;
            this.descp = descp;
        }

    }
}
