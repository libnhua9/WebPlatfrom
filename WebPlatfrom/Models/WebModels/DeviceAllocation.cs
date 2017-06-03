using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.WebModels
{
    public class DeviceAllocation
    {
    }
    public class Device_TVM {
        public int DeviceID { get; set; }
        public string DeviceSN { get; set; }
        public string DeviceName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Enabled{get;set;}
        public DateTime LastUpdate { get; set; }
        public Nullable<decimal> Lat { get; set; }
        public Nullable<decimal> Lng { get; set; }
        public int UserID { get; set; }
        public int ID { get; set; }//关联表Id

    }


}