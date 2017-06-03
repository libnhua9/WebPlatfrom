using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Commons;
using Web.EF;
using Web.Models.WebModels;

namespace Web.Controllers
{
    [WebAuth]
    public class HomeController : Controller
    {
        #region Views

        [NoCheck]
        public ActionResult Index()
        {
            //if (this.Save(tb_users)
            //{
            //    this.Load(Tb_Users users);
            //}
            //if (this.Load<Tb_Users>() != null)
            //{
            //    ViewBag.UserName = this.Load<Tb_Users>().UserName;
            //    ViewBag.Password = this.Load<Tb_Users>().Password;
            //}

            HttpCookie cookies=Request.Cookies["Tb_Users"];
            //判断是否有cookie值，有的话就读取出来
            string userName = "";
            string passPwd = "";
            string isBer = "0";
            if (cookies != null && cookies.HasKeys)
            {
               
                if (cookies["IsBer"] == "1")
                {
                    passPwd = cookies["PassWord"];
                    userName = HttpUtility.UrlDecode(cookies["UserName"]); //cookies["UserName"];
                    isBer = cookies["IsBer"];
                }
                //else
                //{
                //    isBer = "0";
                //}
            }
            ViewBag.UserName = userName;
            ViewBag.Password = passPwd;
            //ViewBag.IsBer = isBer;

            return View();
        }
        [NoCheck]
        public ActionResult UpdateIE()
        {
            return View();
        }
        [CheckSession]
        public ActionResult Main()
        {
            ViewBag.CurrentUser = this.Load<Tb_Users>();
            return View();
        }

        [NoCheck]
        public ActionResult NoPermission()
        {
            return View();
        }

        #endregion

        #region Actions

        [HttpPost]
        [NoCheck]
        public ActionResult Login(LoginParam param,string isBer)
        {
            var result = ActionJsonResult.Create();

            using (var db = DbHelper.Create())
            {
                var user = db.Tb_Users.FirstOrDefault(u => u.UserName == param.UserName);
                if (user?.Password != param.Psw)
                {
                    result.Failed("用户或密码错误");
                }
                else if (user.Enabled == 0)
                {
                    result.Failed("用户已被禁用");
                }
                else
                {
                    //保存Cookie
                    HttpCookie cookie=new HttpCookie("Tb_Users");
                    cookie.Values.Add("UserName",HttpUtility.UrlEncode( param.UserName));
                    cookie.Values.Add("PassWord",param.Psw);
                    cookie.Values.Add("IsBer", isBer);
                    Response.AppendCookie(cookie);
                    //保存当前登录用户
                    this.Save(user);
                    //?获取用户菜单
                    var menuReader = new MenuReader(user);
                    menuReader.LoadMenus(GetMenuJson());
                    this.Save(SessionKeys.UserPermissionDic,menuReader.PermissionItems);
                    this.Save(SessionKeys.Menus, menuReader.Menus);
                    result.Success("/home/main");
                }
            }
            return result.ToJson();
        }

        [CheckSession]
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        [CheckSession]
        [HttpGet]
        public ActionResult GetAllMenu()
        {
            var result = ActionJsonResult.Create();
            var menus = this.Load<List<MenuItem>>(SessionKeys.Menus);
            result.Success(menus);
            return result.ToJson();
        }
        [NoCheck]
        [HttpPost]
        public ActionResult GetSession() {
            var result = ActionJsonResult.Create();
            if (Session["Web.EF.Tb_Users"]==null)
            {
                //Tb_Users user = Session["Web.EF.Tb_Users"] as Tb_Users;
                ////保存当前登录用户
                //this.Save(user);
                ////?获取用户菜单
                //var menuReader = new MenuReader(user);
                //menuReader.LoadMenus(GetMenuJson());
                //this.Save(SessionKeys.UserPermissionDic, menuReader.PermissionItems);
                //this.Save(SessionKeys.Menus, menuReader.Menus);
                result.Failed("过期");
            }
            return result.ToJson();
        }


        private string GetMenuJson()
        {
            var path = Server.MapPath("~\\files\\menus.json");
            return System.IO.File.ReadAllText(path);
        }

        #endregion
    }
}