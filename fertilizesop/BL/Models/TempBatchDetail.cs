using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Models
{
    public class TempBatchDetail
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public decimal Sale { get; set; }
        public int Quantity { get; set; }
    }
}
