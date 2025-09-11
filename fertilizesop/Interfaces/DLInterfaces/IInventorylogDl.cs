using fertilizesop.BL.Models;
using System.Collections.Generic;

namespace fertilizesop.DL
{
    public interface IInventorylogDl
    {
        List<inventorylog> getlog();
        List<inventorylog> getlog(string searchTerm);
    }
}