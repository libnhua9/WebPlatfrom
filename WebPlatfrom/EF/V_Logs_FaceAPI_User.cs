//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class V_Logs_FaceAPI_User
    {
        public int ID { get; set; }
        public Nullable<int> API_ID { get; set; }
        public string Response { get; set; }
        public string Request { get; set; }
        public Nullable<int> Result { get; set; }
        public Nullable<int> UseTime { get; set; }
        public Nullable<System.DateTime> CallTime { get; set; }
        public string FunctionName { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string APIName { get; set; }
        public string ProviderUrl { get; set; }
    }
}