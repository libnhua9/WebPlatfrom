using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.WebModels
{
    public class UserAPI
    {
        public int API_ID { get; set; }
        public string APIName { get; set; }
        public string ProviderUrl { get; set; }
        public string Remarks { get; set; }

        public int UserId { get; set; }
    }
}