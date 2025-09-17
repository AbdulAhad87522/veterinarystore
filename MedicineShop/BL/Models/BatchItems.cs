using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineShop.BL.Models
{
    public  class BatchItems
    {
        public int BatchItemID { get; set; }
        public int BatchID { get; set; }
       
        public int MedicineID { get; set; }
        public string MedicineName { get; set; }
        public int Quantity { get; set; }
        public decimal Purcahseprice { get; set; }
        public decimal SalePrice { get; set; }
        public DateTime ExpiryDate { get; set; }

    }
}
