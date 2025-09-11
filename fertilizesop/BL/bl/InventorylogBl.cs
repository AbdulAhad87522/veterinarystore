using fertilizesop.BL.Models;
using fertilizesop.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Bl
{
    public class InventorylogBl : IInventorylogBl
    {
        private readonly IInventorylogDl ibl;
        public InventorylogBl(IInventorylogDl ibl)
        {
            this.ibl = ibl;
        }
        public List<inventorylog> getlog()
        {
            try
            {
                return ibl.getlog();
            }
            catch
            {
                throw new Exception("error in retreving ");
            }
        }

        public List<inventorylog> getlog(string searchTerm)
        {
            return ibl.getlog(searchTerm);
        }
    }
}
