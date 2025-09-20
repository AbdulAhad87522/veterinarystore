using MedicineShop.BL;
using MedicineShop.DL;
using System;
using System.Collections.Generic;

namespace MedicineShop.BL
{
    public class BatchesBl
    {
        private readonly BatchesDl _batchesDl;

        public BatchesBl()
        {
            _batchesDl = new BatchesDl();
        }

        // ✅ Add
        public bool AddBatch(Batches batch)
        {
            if (string.IsNullOrWhiteSpace(batch.BatchName))
                throw new ArgumentException("Batch name cannot be empty.");
            if (batch.TotalPrice < 0 || batch.Paid < 0)
                throw new ArgumentException("Price values cannot be negative.");

            return _batchesDl.AddBatch(batch);
        }

        // ✅ Get All
        public List<Batches> GetAllBatches()
        {
            return _batchesDl.GetAllBatches();
        }

        // ✅ Get By ID
        public Batches GetBatchById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid batch ID.");
            return _batchesDl.GetBatchById(id);
        }

        // ✅ Update
        public bool UpdateBatch(Batches batch)
        {
            if (batch.PurchaseBatchID <= 0)
                throw new ArgumentException("Invalid batch ID.");
            return _batchesDl.UpdateBatch(batch);
        }

        // ✅ Delete
        public bool DeleteBatch(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid batch ID.");
            return _batchesDl.DeleteBatch(id);
        }

        // ✅ Search
        public List<Batches> SearchBatches(string searchTerm)
        {
         
            return _batchesDl.SearchBatches(searchTerm);
        }
    }
}
