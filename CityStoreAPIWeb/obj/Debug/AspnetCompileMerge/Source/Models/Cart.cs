using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityStoreAPIWeb.Models
{
    public class Cart
    {
        
     
       
      
        public int ProductId { get; set; }   
        public string productname { get; set; }
        public string size { get; set; }
        public string sizename { get; set; } 
        public string color { get; set; }
        public string colorid { get; set; }
        public Nullable<int> qty { get; set; }
        public int rate { get; set; }
        public int discount { get; set; }
        public int stock { get; set; }
     public string image { get; set; }
    }
}