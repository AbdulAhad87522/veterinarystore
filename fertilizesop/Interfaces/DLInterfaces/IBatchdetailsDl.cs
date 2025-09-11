using System.Collections.Generic;

namespace fertilizesop.DL
{
    public interface IBatchdetailsDl
    {
        bool adddetails(BatchDetails b);
        List<BatchDetails> GetAllBatchDetails();
        List<BatchDetails> SearchBatchDetails(string searchText);
        int getsaleprice(int product_id);
    }
}