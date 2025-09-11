using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Models
{
    public class Dashboard
    {
        public int totalproducts { get; set; }
        public int total_stock { get; set; }
        public int totalcustomers { get; set; }
        public int salestodays { get; set; }
        public int totalsuppliers { get; set; }
        public int outproduct { get; set; }
        public int pendingbills { get; set; }
        public int total_stock_value { get; set; }

        public Dashboard() { }
    }
}
