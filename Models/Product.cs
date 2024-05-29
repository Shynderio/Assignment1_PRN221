using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Estore.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        [JsonIgnore]
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int CategoryId { get; set; }
        public int UnitPrice { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
