using fertilizesop.BL.Models;
using System.Collections.Generic;

namespace fertilizesop.BL.Bl
{
    public interface IInventorylogBl
    {
        List<inventorylog> getlog();
        List<inventorylog> getlog(string searchTerm);
    }
}