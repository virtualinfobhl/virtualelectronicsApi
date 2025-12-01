using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityStoreAPIWeb.Models
{
    public class Userregister
    {
        public int UserCode { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string MobileNumber { get; set; }
        public bool active { get; set; }
        public string ip { get; set; }
        public string ShippingName { get; set; }
        public string ShippingEmailId { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingState { get; set; }
        public string ShippingDistrict { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingZipCode { get; set; }
        public Nullable<long> ShippingMobileNumber { get; set; }
        public decimal Wallet { get; set; }
        public string HouseNo { get; set; }
        public System.DateTime PostDate { get; set; }
        public string Password { get; set; }
    }
}