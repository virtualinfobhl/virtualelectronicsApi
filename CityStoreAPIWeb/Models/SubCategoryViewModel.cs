using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityStoreAPIWeb.Models
{
    public class SubCategoryViewModel
    {
        public int MainCategoryId { get; set; }
        public string  MainCategoryName { get; set; }
        public IEnumerable<SubCat> SubCategory { get; set; }
        public Nullable<short> Priority { get; set; }

    }
    public class SubCat
    {
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }      
        public string Image { get; set; }
        public Nullable<short> Priority { get; set; }
    }
}