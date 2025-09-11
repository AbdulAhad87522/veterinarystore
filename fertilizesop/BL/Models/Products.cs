using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Models
{
    public class Products
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int quantity { get; private set; }
        public Products(int id, string name, string description, decimal price, int quantity)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            this.quantity = quantity;
        }
        public Products(int id, string name, string description)
        {
            {
                Id = id;
                Name = name;
                Description = description;
            }
        }      

    }
}