using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityStoreAPIWeb.Models
{
    public class JoinTrnWishlistProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> Rate { get; set; }
        public int Discount { get; set; }
        public Decimal Cashback { get; set; }
    }
}