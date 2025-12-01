using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityStoreAPIWeb.Models
{
    public class JoinOrderMain
    {
        public int AID { get; set; }
        public int OrderId { get; set; }
        public Nullable<int> Usercode { get; set; }
        public Nullable<decimal> OrderAmount { get; set; }
        public Nullable<decimal> TotalQuantity { get; set; }
        public Nullable<decimal> DeliveryFees { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Paymode { get; set; }
        public string TrnId { get; set; }
        public string TrnStatus { get; set; }
        public string OrderThrough { get; set; }
        public string ShippingName { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingMobileNo { get; set; }
        public string ShippingEmailId { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingZipcode { get; set; }
        public string ShippingStats { get; set; }
        public bool Delivered { get; set; }
        public bool Cancelled { get; set; }
        public bool Returned { get; set; }
        public bool Dispatched { get; set; }
        public bool Settlement { get; set; }
        public Nullable<System.DateTime> RTS { get; set; }
        public decimal WalletAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public bool IsWalletDeduct { get; set; }
        public decimal AddAmount { get; set; }
        public bool IsCouponAvailed { get; set; }
        public bool IsReturnable { get; set; }
        public bool IsCancellable { get; set; }
        public bool DeliverStatus { get; set; }
    }
}