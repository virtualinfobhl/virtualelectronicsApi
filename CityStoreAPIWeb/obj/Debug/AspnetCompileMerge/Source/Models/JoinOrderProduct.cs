using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityStoreAPIWeb.Models
{
    public class JoinOrderProduct
    {
        public Nullable<int> OrderId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string Image { get; set; }
        public string ProductName { get; set; }
        public decimal Weight { get; set; }
        public string Unit { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string ColorName { get; set; }
        public string ColorImage { get; set; }
        public decimal Cashback { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> NetAmount { get; set; }
        public bool Dispatched { get; set; }
        public bool Cancelled { get; set; }
        public bool Delivered { get; set; }
        public bool Returned { get; set; }
        public System.DateTime RTS { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int AID { get; set; }
        public Nullable<bool> Returnable { get; set; }
        public Nullable<bool> Cancellable { get; set; }
        public Nullable<bool> DeliverStatus { get; set; }
    }
}