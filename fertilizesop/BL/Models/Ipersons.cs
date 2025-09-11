using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Models
{
    public interface Ipersons
    {
        string name { get; }
        string address { get; }
        string phone { get; }
        int id { get; }
    }
}
