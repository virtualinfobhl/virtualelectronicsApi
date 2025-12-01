using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityStoreAPIWeb.Models
{
    public class ProductListing
    {
        public Nullable<int> Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int mid { get; set; }

    }
}