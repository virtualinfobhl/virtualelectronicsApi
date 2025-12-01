using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityStoreAPIWeb.Models
{
    public class JoinTrnCartProduct
    {
        public Nullable<int> ProductId { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public Nullable<int> Stock { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> Rate { get; set; }
        public int Discount { get; set; }
        public string SchemeOffer { get; set; }
        public decimal Cashback { get; set; }
        public Nullable<int> Weight { get; set; }
        public string Unit { get; set; }
        public string Size { get; set; }
        public string ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorImage { get; set; }
        public string ActualUnit { get; set; }
        public Nullable<decimal> ActualWeight { get; set; }
        public int Id { get; set; }
        public Nullable<bool> Payment_Online { get; set; }
        public Nullable<bool> Payment_COD { get; set; }
           public int DeliveryFees { get; set; }
     
    }
}