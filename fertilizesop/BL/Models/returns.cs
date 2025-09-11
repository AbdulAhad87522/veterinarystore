using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Models
{
    public class returns
    {
        public int return_id { get;set; }  
        public int product_id { get;         set; }
        public int quantity_returned { get; set; }
        public decimal amount { get;  set; }
        public DateTime return_date { get;  set; }
        public int bill_id { get;  set; }
    }
}
