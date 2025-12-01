using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityStoreAPIWeb.Models
{
    public class JoinCouponProvider
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CouponCode { get; set; }
        public string Description { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Discount { get; set; }
        public Nullable<int> CID { get; set; }
        public bool OnRegister { get; set; }
        public bool OnPayment { get; set; }
        public bool Both { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public System.DateTime RTS { get; set; }
      

    }
}