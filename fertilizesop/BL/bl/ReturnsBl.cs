using fertilizesop.BL.Models;
using fertilizesop.DL;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Bl
{
    public class ReturnsBl : IReturnsBl
    {
        private readonly IRetunrsDl _retunrsDl;
        public ReturnsBl(IRetunrsDl retunrsDl)
        {
            _retunrsDl = retunrsDl;
        }
        public bool AddReturn(returns r)
        {
            // 1️⃣ Basic null and range checks
            if (r == null) throw new ArgumentNullException(nameof(r));
            if (r.product_id <= 0) throw new ArgumentException("Invalid product ID.");
            if (r.bill_id <= 0) throw new ArgumentException("Invalid bill ID.");
            if (r.quantity_returned <= 0) throw new ArgumentException("Returned quantity must be greater than 0.");
            if (r.amount < 0) throw new ArgumentException("Refund amount cannot be negative.");




            // ✅ Passed all checks → send to DL
            bool result = _retunrsDl.addreturn(r);
            if (result)
            {
                MySqlBackupHelper.CreateBackup();
            }
            return result;
        }
        public List<Products> GetProducts(string name)
        {

            return _retunrsDl.getproducts(name);

        }
    }
}
