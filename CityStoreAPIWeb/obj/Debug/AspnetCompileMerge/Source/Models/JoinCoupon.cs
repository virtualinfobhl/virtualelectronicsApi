using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityStoreAPIWeb.Models
{
    public class JoinCoupon
    {
        public int ServiceId { get; set; }
        public string CouponCode { get; set; }
        public decimal Discount { get; set; }
        public string Title { get; set; }
        public DateTime ValidityDate { get; set; }
        public string Description { get; set; }
        public string ShopName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public bool IsUsed { get; set; }
        public int OrderNo { get; set; }
        public DateTime UsedDate { get; set; }
        public string OrderType { get; set; }
        public bool ExpiryCoupon { get; set; }
        public int AID { get; set; }
    }
}