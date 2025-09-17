using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineShop.BL.Bl
{
    public class BatchesBl
    {
        public bool AddBatches(Batches b, List<Models.BatchItems> itemsList)
        {
            try
            {
                return new DL.BatchesDl().AddBatches(b, itemsList);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in BatchesBl->AddBatches: " + ex.Message);
            }
        }
        public decimal getsaleprice(int product_id)
        {
            try
            {
                return new DL.BatchesDl().SalePrice(product_id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in BatchesBl->getsaleprice: " + ex.Message);
            }
        }
    }
}
