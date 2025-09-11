using fertilizesop.DL;
using fertilizesop.Interfaces.BLInterfaces;
using System;
using System.Collections.Generic;

namespace fertilizesop.BL.Bl
{
    public class BatchDetailsBl : IbatchdetailsBl
    {
        private readonly IBatchdetailsDl idl;

        public BatchDetailsBl(IBatchdetailsDl idl)
        {
            this.idl = idl;
        }

        public bool adddetails(BatchDetails b)
        {
            try
            {
                // Validation
                if (b == null) throw new ArgumentNullException(nameof(b), "Batch details cannot be null.");
                if (b.product_id <= 0) throw new ArgumentException("Invalid Product ID.");
                if (b.cost_price <= 0) throw new ArgumentException("Cost price must be greater than 0.");
                if (b.quantity_received < 0) throw new ArgumentException("Quantity received cannot be negative.");
                if (b.sale_price <= 0) throw new ArgumentException("Sale price must be greater than 0.");

                bool result = idl.adddetails(b);

                if (result)
                {
                    // ✅ Automatically backup to .dat
                    MySqlBackupHelper.CreateBackup();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in BL while adding batch details: " + ex.Message, ex);
            }
        }

        public List<BatchDetails> GetAllBatchDetails()
        {
            try
            {
                return idl.GetAllBatchDetails();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in BL while fetching all batch details: " + ex.Message, ex);
            }
        }

        public int getsaleprice(int product_id)
        {
            if (product_id <= 0) throw new ArgumentException("Error in getting sale price.");
            return idl.getsaleprice(product_id);
        }

        public List<BatchDetails> SearchBatchDetails(string searchText)
        {
            try
            {
                return idl.SearchBatchDetails(searchText.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Error in BL while searching batch details: " + ex.Message, ex);
            }
        }
    }
}
