using System;
using System.Web;
using System.Web.Mvc;

namespace Web.Commons
{
    public static class ExtHelper
    {

        #region Session 操作，Key不区分大小写

        public static T Load<T>(this HttpSessionStateBase state)
        {
            var key = typeof(T).FullName;
            return (T)state[key];
        }

        public static T Load<T>(this HttpSessionStateBase state,string key)
        {
            return !string.IsNullOrEmpty(key) ? (T)state[key] : default(T);
        }

        public static Controller Save(this Controller ctrl, string key, object value)
        {
            if (!string.IsNullOrEmpty(key) && value != null)
            {
                ctrl.Session[key] = value;
                ctrl.Session.Timeout = 10;
            }
            return ctrl;
        }

        public static Controller Save<T>(this Controller ctrl,T value)
        {
            var key = typeof(T).FullName;
            return ctrl.Save(key, value);
        }

        /// <summary>
        /// 读出Session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctrl"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Load<T>(this Controller ctrl,string key)
        {
            return !string.IsNullOrEmpty(key) ? (T)ctrl.Session[key] : default(T);
        }

        public static T Load<T>(this Controller ctrl)
        {
            var key = typeof(T).FullName;
            return ctrl.Load<T>(key);
        }

        #endregion

        /// <summary>
        /// 返回以类型名称为开头的id表示
        /// <para>ResourcePublishController_Id</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctrl"></param>
        /// <param name="token">标识</param>
        /// <returns></returns>
        public static string GetTid<T>(this T ctrl,string token = "")
        {
            return $"{ctrl.GetType().Name}{token}_Id";
        }

        public static bool SameText(this string source, string dest)
        {
            return string.Equals(source,dest,StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 将需要跨action的变量存储到TempData里
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctrl"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Controller ActionSave<T>(this Controller ctrl,string key,T value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                ctrl.TempData[key] = value;
            }
            return ctrl;
        }

        public static T ActionLoad<T>(this Controller ctrl,string key)
        {
            return !string.IsNullOrEmpty(key) ? (T)ctrl.TempData[key] : default(T);
        }

        
    }
}