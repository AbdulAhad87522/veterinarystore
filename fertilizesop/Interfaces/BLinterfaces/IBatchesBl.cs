using fertilizesop.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.Interfaces.BLInterfaces
{
    public interface IBatchesBl
    {
        bool addbatches(Batches b);
        List<Batches> GetAllBatches();
        List<Batches> SearchBatches(string keyword);
        bool UpdateBatch(Batches b);
        List<string> getsuppliernames(string text);
    }
}
