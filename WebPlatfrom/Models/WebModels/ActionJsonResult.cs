using System.Web.Mvc;

namespace Web.Models.WebModels
{
    /// <summary>
    /// ActionResult返回结果的Json形式
    /// </summary>
    public class ActionJsonResult
    {
        /// <summary>
        /// 返回的状态码
        /// <para> 0 = 失败 </para>
        /// <para> 1 = 成功 </para>
        /// </summary>
        public int State { get; private set; }

        /// <summary>
        /// 返回的信息或错误内容
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// 返回的数据
        /// </summary>
        public object Data { get; set; }

        #region 方法

        /// <summary>
        /// 创建一个新的结果类，默认成功。
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        public static ActionJsonResult Create(bool success = true)
        {
            return  new ActionJsonResult {State = success? 1:0 };
        }

        public void Failed(string message)
        {
            State = 0;
            Message = message;
        }

        public void Auto(bool express,string message = null,object data = null)
        {
            State = express ? 1 : 0;
            Message = message + (express ? "成功" : "失败");
            Data = data;
        }

        public void Success()
        {
            Success(null,null);
        }

        public void Success(string message)
        {
            Success(message,null);
        }

        public void Success(object data,string message = null)
        {
            State = 1;
            Message = message;
            Data = data;
        }

        #endregion

        public JsonResult ToJson(bool allowGet = true)
        {
            var result= new JsonResult
            {
                Data = this,
                ContentType = null,
                ContentEncoding = null
            };
            if (allowGet)
            {
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            }
            return result;
        }
    }
}