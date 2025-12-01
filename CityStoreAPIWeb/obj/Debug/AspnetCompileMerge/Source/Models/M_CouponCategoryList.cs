using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityStoreAPIWeb.Models
{
    public class M_CouponCategoryList
    {
        public int AID { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool ActiveStatus { get; set; }      
        public int Id { get; set; }
    }
}