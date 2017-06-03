using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using Web.EF;
using Web.Models.WebModels;

namespace Web.Commons
{
    public class WebAuthAttribute : FilterAttribute, IAuthenticationFilter
    {
        /// <summary>
        /// 判断是否有某个资源或操作项的权限
        /// </summary>
        /// <param name="action"></param>
        /// <param name="dic"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        private bool HasPermission(string action,Dictionary<string,UserPermissionItem> dic,int level)
        {
            if (string.IsNullOrEmpty(action) || dic == null ) return false;
            var item = dic.ContainsKey(action) ? dic[action] : null;
            if (!string.IsNullOrEmpty(item?.Level))
            {
                return item.Level.Contains(level.ToString());
            }
            return false;
        }

        private void NoPermission(AuthenticationContext context,int type)
        {
            var helper = new UrlHelper(context.RequestContext);
            var StrUrl = "";
            if (type==2) {
                StrUrl = "Index";
            }
            else {
                StrUrl = "NoPermission";
            }
            var url = helper.Action(StrUrl, "Home");
            context.Result = new RedirectResult(url);
        }

        public void OnAuthentication(AuthenticationContext context)
        {
            var noCheckAttrs = context.ActionDescriptor.GetCustomAttributes(typeof(NoCheckAttribute), false);
            if (noCheckAttrs.Length > 0) return;

            //检查用户对当前action的操作权限
            var action = context.ActionDescriptor;
            var act = $"/{action.ControllerDescriptor.ControllerName}/{action.ActionName}".ToUpper();
            var session = context.HttpContext.Session;
            var user = session.Load<Tb_Users>();
            if (user?.Level != null)//判断权限
            {
                var sessionCheckAttr = context.ActionDescriptor.GetCustomAttributes(typeof(CheckSessionAttribute),false);
                if (sessionCheckAttr.Length > 0) return;

                var dic = session.Load<Dictionary<string,UserPermissionItem>>(SessionKeys.UserPermissionDic);
                if (!HasPermission(act,dic,user.Level.Value))
                {
                    NoPermission(context,1);
                }
            }
            else if (user == null)///失去Session，跳到登录页面
            {
                NoPermission(context, 2);
            }
            else
            {
                NoPermission(context,1);
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            //这个方法是在Action执行之后调用
        }
    }
}