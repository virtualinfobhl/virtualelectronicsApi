using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityStoreAPIWeb.Models
{
    public class JoinStoreProduct
    {

        public int ProductId { get; set; }
        public Nullable<int> StoreId { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public string BrandLogo { get; set; }
        public Nullable<int> SubCategoryId { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> Rate { get; set; }
        public string Image { get; set; }
        public Nullable<int> Weight { get; set; }
        public int Discount { get; set; }
        public bool Active { get; set; }
        public string Type { get; set; }
        public string SchemeOffer { get; set; }
        public string ProductCode { get; set; }
        public Nullable<int> Brand_Id { get; set; }
        public Nullable<int> DeliveryFees { get; set; }
        public decimal Cashback { get; set; }
        public bool DealActive { get; set; }
        public bool ComboActive { get; set; }
        public string PColor { get; set; }
        public string PColorQty { get; set; }
        public string PColorId { get; set; }
        public string PColorImage { get; set; }
        public bool IsNewlyArrived { get; set; }
        public bool IsWishlisted { get; set; }
         //public string IsWishlisted { get; set; }
    }

}