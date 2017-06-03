using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Web.EF;

namespace Web.Commons
{
    public class DbHelper
    {
        public static DocomPlatformEntities Create()
        {
            return new DocomPlatformEntities();
        }

    }
}