using fertilizesop.BL.Models;
using System.Collections.Generic;

namespace fertilizesop.DL
{
    public interface IBatchesDl
    {
        bool addbatches(Batches b);
        List<Batches> GetAllBatches();
        List<Batches> SearchBatches(string keyword);
        bool UpdateBatch(Batches b);
        List<string> getsuppliernames(string text);
    }
}