using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityStoreAPIWeb.Models
{
    public class tUsedCouponDetails
    {
        public decimal Amount { get; set; }     
        public decimal DiscountAmt { get; set; }
        public decimal NetAmt { get; set; }
    }
}