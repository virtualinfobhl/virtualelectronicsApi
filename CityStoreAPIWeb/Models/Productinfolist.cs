using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityStoreAPIWeb.Models
{
    public class Productinfolist
    {
        public Nullable<int> categoryid { get; set; }
        public int productid { get; set; }
        public string productname { get; set; }
       
        public Nullable<bool> sale { get; set; }
        public int price { get; set; }
        public int discount { get; set; }
        public string Image { get; set; }
        public string Quantity { get; set; }
        public string PColor { get; set; }
        public string PColorId { get; set; }
        public string Description { get; set; }
    }
}