using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityStoreAPIWeb.Models
{
    public class JoinWalletTransaction
    {
        public int AID { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> OrderId { get; set; }
        public string TrnType { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<bool> IsDeducted { get; set; }
        public System.DateTime RTS { get; set; }
        public string OrderType { get; set; }
        public string Type { get; set; }
        public Nullable<decimal> PAmount { get; set; }

    }
}