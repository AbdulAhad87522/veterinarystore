using fertilizesop.BL.Models;
using fertilizesop.DL;
using fertilizesop.Interfaces.BLInterfaces;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Bl
{
    internal class BatchesBl : IBatchesBl
    {
        private readonly IBatchesDl idl;
        public BatchesBl(IBatchesDl idl)
        {
            this.idl = idl;
        }

        public bool addbatches(Batches b)
        {
            ValidateBatch(b);
           bool result= idl.addbatches(b);
            if (result)
            {
                MySqlBackupHelper.CreateBackup();
            }
            return result;
        }

        public List<Batches> GetAllBatches()
        {
            return idl.GetAllBatches();
        }

        public List<string> getsuppliernames(string text)
        {
            return idl.getsuppliernames(text);
        }

        public List<Batches> SearchBatches(string keyword)
        {
            return idl.SearchBatches(keyword);
        }

        public bool UpdateBatch(Batches b)
        {
            if (b.batch_id <= 0)
                throw new ArgumentException("Invalid batch ID for update.");

            ValidateBatch(b);
            bool result = idl.UpdateBatch(b);
            if (result)
            {
                MySqlBackupHelper.CreateBackup();
            }
            return result;
        }

        /// <summary>
        /// Private helper method to validate batch fields
        /// </summary>
        private void ValidateBatch(Batches b)
        {
            if (string.IsNullOrWhiteSpace(b.batch_name))
                throw new ArgumentException("Batch name cannot be empty.");

            if (string.IsNullOrWhiteSpace(b.supplier_name))
                throw new ArgumentException("Supplier name cannot be empty.");

            if (b.received_date == default)
                throw new ArgumentException("Received date is required.");


        }
    }
}
