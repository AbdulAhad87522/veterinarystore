using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.Interfaces.BLInterfaces
{
    public  interface IbatchdetailsBl
    {
        bool adddetails(BatchDetails b);
        List<BatchDetails> GetAllBatchDetails();
        List<BatchDetails> SearchBatchDetails(string searchText);
        int getsaleprice(int product_id);
    }
}
