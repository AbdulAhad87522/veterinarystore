using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Models
{
    public interface Isupplier
    {
        string Address { get; set; }
        string first_Name { get; set; }
        int Id { get; set; }
        string phonenumber { get; set; }
    }
}