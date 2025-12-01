using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityStoreAPIWeb.Models
{
    public class LoadCity
    {
        public short District_Code { get; set; }
        public string District_Name { get; set; }
        public short State_Code { get; set; }
        public byte District_Priority { get; set; }
        public bool District_Active { get; set; }
        public int DisCode { get; set; }
        public Nullable<int> FID { get; set; }
    }
}