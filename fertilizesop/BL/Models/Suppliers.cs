using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Models
{
    public class Suppliers : Isupplier
    {
        public string Address { get; set; }
        public string first_Name { get; set; }
        public int Id { get; set; }
        public string phonenumber { get; set; }

        public Suppliers() { }  
        public Suppliers(int id, string first_name, string phonenumber, string Address)
        {
            Id = id;
            this.first_Name = first_name;
            this.phonenumber = phonenumber;
            this.Address = Address;
        }

        public Suppliers(string first_name, string phonenumber, string Address)
        {
            this.first_Name = first_name;
            this.phonenumber = phonenumber;
            this.Address = Address;
        }

    }
}