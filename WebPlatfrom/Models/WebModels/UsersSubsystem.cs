using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.WebModels
{
    public class UsersSubsystem
    {
        public string API_Url { get; set; }
        public string StartDate { get; set; }
        public string  EndDate { get; set; }
        public string Remarks { get; set; }
        public int Enabled { get; set; }

        public int ProductType { get; set; }
    }
}