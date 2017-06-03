using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Web.Commons;
using Web.EF;
using Web.Models.WebModels;

namespace Web.Controllers
{
    [WebAuth]
    public class AccManageController : Controller
    {
        [CheckSession]
        #region 修改个人资料
        public ActionResult UserProfile()
        {
            var user = this.Load<Tb_Users>();
            ViewBag.User = user;
            return View();
        }
        /// <summary>
        /// 编辑个人资料
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult SaveUserProfile(PasswordChange info)
        {
            var result = ActionJsonResult.Create();
            if (info == null)
            {
                result.Failed("参数错误");
            }
            else
            {
                var user = this.Load<Tb_Users>();
                if (user.Password != info.OldPsw)
                {
                    result.Failed("用户密码不匹配.");
                }
                else
                {
                    using (var db = DbHelper.Create())
                    {
                        user.Password = info.NewPsw;
                        db.Entry(user).State = EntityState.Modified;
                        try
                        {
                            db.SaveChanges();
                            this.Save(user);
                        }
                        catch (Exception e)
                        {
                            result.Failed(e.Message);
                        }
                    }
                }
            }

            return result.ToJson();
        }
        /// <summary>
        /// 回退
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult GoBackUserProfile()
        {
            return RedirectToAction("Administrator");
        }
        #endregion

        private string GetUserToken()
        {
            return Guid.NewGuid().ToString().ToUpper();
        }

        #region Admin

        /// <summary>
        /// 点击管理员，默认的页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Administrator()
        {
            //using (var db = DbHelper.Create())
            //{
            //    var list = db.Tb_Users.Where(u => u.Level == 0 || u.Level == 1).ToList();
            //    ViewBag.Users = list.Take(20).ToList();
            //    ViewBag.Count = list.Count;
            //}
            return View();
        }

        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddAdmin(int? id ,int ? type)
        {
            if (id.HasValue) //编辑管理员,在编辑的情况下，需要前台页面判断业务对象是否为null
            {
                using (var db = DbHelper.Create())
                {
                    ViewBag.User = db.Tb_Users.FirstOrDefault(u => u.UserID == id);
                }
            }
            ViewBag.Type = type != 1 ? "添加" : "修改";
            return View();
        }

        /// <summary>
        /// 编辑管理员
        /// </summary>
        /// <param name="id">管理员Id</param>
        /// <returns></returns>
        public ActionResult EditAdmin(int id)
        {
            return RedirectToAction("AddAdmin", new { id,type=1 });
        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="id">管理员Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteUser(int? id)
        {
            var result = ActionJsonResult.Create();
            if (id.HasValue) //编辑
            {
                using (var db = DbHelper.Create())
                {
                    var user = db.Tb_Users.FirstOrDefault(u => u.UserID == id);
                    if (user == null)
                    {
                        result.Failed("无法查询到指定的用户.");
                    }
                    else
                    {
                        try
                        {
                            db.Tb_Users.Remove(user);
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            result.Failed(e.Message);
                        }
                    }
                }
            }
            else
            {
                result.Failed("无效的用户Id.");
            }
            return result.ToJson();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetAdminList()
        {
            
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            //var deviceName = Request["DeviceName"];

            var X = Request["X"];
            //int Y = -1;
            //起始日期
            var starts = Request["starts"];
            //结束日期
            var end = Request["end"];
            //if (!string.IsNullOrEmpty(Request["Y"]))
            //{
            //    Y = Convert.ToInt32(Request["Y"]);
            //}
            
            var o = new object();
            using (var db = DbHelper.Create())
                {
                //var result = ActionJsonResult.Create();
                 var   result=(IEnumerable<Tb_Users>) db.Tb_Users;

                result = result.Where(m => m.Level == 0 || m.Level == 1);

               
                //var sql = new StringBuilder("select * from tb_users u where 1=1 ");
                    if (!string.IsNullOrEmpty(X))
                    {
                        //sql.AppendFormat(" and u.username = '{0}'", X);
                       result= result.Where(m => m.UserName.Contains(X));
                    }

                    //if (Y != -1)
                    //{
                    //    //sql.AppendFormat(" and u.level = {0}", Y);
                    //    result = result.Where(m => m.Level == Y);
                    //}
                    if (starts != "" && end != "")
                    {
                        result = result.Where(m => m.CreateDate >= Convert.ToDateTime(starts + " 00:00:00") && m.CreateDate <= Convert.ToDateTime((end + " 23:59:59")));
                    }
                    else if (starts != "")
                    {
                        result = result.Where(m => m.CreateDate >= Convert.ToDateTime(starts + " 00:00:00"));
                    }
                    else if (end != "")
                    {
                        result = result.Where(m => m.CreateDate <= Convert.ToDateTime((end + " 23:59:59")));
                    }
                   else
                    {
                        //sql.Append(" and u.level in (0,1)");
                        result = result.Where(m => m.Level == 0 || m.Level == 1);
                     }
                    
                    //sql.Append(" order by u.userId");


                //var users = db.Tb_Users.Where(u => (!string.IsNullOrEmpty(param.X) && u.UserName == param.X) || (param.Y != -1 &&  u.Level == param.Y)).ToList();
               
                //var users = db.Database.SqlQuery<Tb_Users>(sql.ToString()).ToList();
                var recordsTotal = result.Count();
                var data = result.OrderByDescending(m=>m.UserID).Skip(start).Take(rows).ToList();
                o = new
                {
                    draw = draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsTotal,
                    data = data
                };

                //var list = users.Skip(param.Offset * param.PageSize).Take(param.PageSize).ToList();
                //    var psr = new PageSearchResult<List<Tb_Users>>(users.Count, list);
                //    result.Data = psr;
                }

            return Json(o, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存或更新用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult SaveAdmin(Tb_Users user)
        {
            var result = ActionJsonResult.Create();
            if (user == null)
            {
                result.Failed("用户数据错误.");
            }
            else
            {
                using (var db = DbHelper.Create())
                {
                    try
                    {
                        //查询是否有相同的记录存在，存在，则更新；否则插入新的
                        var tmp = db.Tb_Users.FirstOrDefault((a => a.UserID == user.UserID));
                        if (tmp != null)
                        {
                            tmp.Level = user.Level;
                            tmp.Enabled = user.Enabled;
                            tmp.UserName = user.UserName;
                            db.Entry(tmp).State = EntityState.Modified;
                        }
                        else
                        {
                            user.Password = "000000";
                            user.CreateDate = DateTime.Now;
                            user.Token = GetUserToken();
                            db.Tb_Users.Add(user);
                        }
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        result.Failed(e.Message);
                    }
                }
            }
            return result.ToJson();
        }

        /// <summary>
        /// 重设账户密码
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        [CheckSession]
        [HttpGet]
        public ActionResult ResetPsw(int id)
        {
            var result = ActionJsonResult.Create();
            using (var db = DbHelper.Create())
            {
                var user = db.Tb_Users.FirstOrDefault(u => u.UserID == id);
                if (user == null)
                {
                    result.Failed("无法查询到指定的用户.");
                }
                else
                {
                    try
                    {
                        user.Password = "000000";
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        result.Failed(e.Message);
                    }
                }
            }

            return result.ToJson();
        }

        [CheckSession]
        public ActionResult GobackAdmin()
        {
            return RedirectToAction("Administrator");
        }


        #endregion

        #region Developer

        /// <summary>
        /// 点击开发者，默认的页面
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult Developer()
        {
            //using (var db = DbHelper.Create())
            //{
            //    var list = db.Tb_Users.Where(u => u.Level ==2).ToList();
            //    ViewBag.Users = list.Take(20).ToList();
            //    ViewBag.Count = list.Count;
            //}
            return View();
        }
        [CheckSession]

        public ActionResult AddDeveloper(int? id ,int ? type)
        {
            if (id.HasValue) //编辑开发者,在编辑的情况下，需要前台页面判断业务对象是否为null
            {
                using (var db = DbHelper.Create())
                {
                    ViewBag.User = db.Tb_Users.FirstOrDefault(u => u.UserID == id);
                }
            }
            ViewBag.Type = type != 1 ? "添加" : "修改";
            return View();
        }
        [CheckSession]
        public ActionResult EditDeveloper(int id)
        {
            return RedirectToAction("AddDeveloper", new { id,type=1 });
        }


        [CheckSession]
        [HttpPost]
        public ActionResult SaveDeveloper( Tb_Users user)
        {
            var result = ActionJsonResult.Create();
            if (user == null)
            {
                result.Failed("用户数据错误.");
            }
            else
            {
                using (var db = DbHelper.Create())
                {
                    try
                    {
                        //查询是否有相同的记录存在，存在，则更新；否则插入新的
                        var tmp = db.Tb_Users.FirstOrDefault((a => a.UserID == user.UserID));
                        if (tmp != null)
                        {
                            tmp.Enabled = user.Enabled;
                            tmp.UserName = user.UserName;
                            db.Entry(tmp).State = EntityState.Modified;
                        }
                        else
                        {
                            user.Level = 2;
                            user.Password = "000000";
                            user.CreateDate = DateTime.Now;
                            user.Token = GetUserToken();
                            db.Tb_Users.Add(user);
                        }
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        result.Failed(e.Message);
                    }
                }
            }
            return result.ToJson();
        }

        [CheckSession]
        [HttpGet]
        public ActionResult ResetToken(int id)
        {
            var result = ActionJsonResult.Create();
            using (var db = DbHelper.Create())
            {
                var user = db.Tb_Users.FirstOrDefault(u => u.UserID == id);
                if (user == null)
                {
                    result.Failed("无法查询到指定的用户.");
                }
                else
                {
                    try
                    {
                        user.Token = GetUserToken();
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        result.Failed(e.Message);
                    }
                }
            }

            return result.ToJson();
        }
        [CheckSession]
        [HttpPost]
        public ActionResult GetDeveloperList(SearchParam<string, int> param)
        {
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            //var deviceName = Request["DeviceName"];
            var X = Request["X"];
            //起始日期
            var starts = Request["starts"];
            //结束日期
            var end = Request["end"];
            var o = new object();
            using (var db = DbHelper.Create())
                {

                    var result = (IEnumerable<Tb_Users>)db.Tb_Users;
                    result = result.Where(m => m.Level == 2);
                    //var sql = new StringBuilder("select * from tb_users u where Level=2 ");
                    if (!string.IsNullOrEmpty(X))
                    {
                        //sql.AppendFormat(" and u.username ='{0}'", X);
                        result = result.Where(m => m.UserName.Contains(X));
                    }
                //sql.Append(" order by u.UserID");
                    if (starts != "" && end != "")
                    {
                        result = result.Where(m => m.CreateDate >= Convert.ToDateTime(starts + " 00:00:00") && m.CreateDate <= Convert.ToDateTime((end + " 23:59:59")));
                    }
                    else if (starts != "")
                    {
                        result = result.Where(m => m.CreateDate >= Convert.ToDateTime(starts + " 00:00:00"));
                    }
                    else if (end != "")
                    {
                        result = result.Where(m => m.CreateDate <= Convert.ToDateTime((end + " 23:59:59")));
                    }

                //var users = db.Tb_Users.Where(u => (!string.IsNullOrEmpty(param.X) && u.UserName == param.X) || (param.Y != -1 &&  u.Level == param.Y)).ToList();

                //var users = db.Database.SqlQuery<Tb_Users>(sql.ToString()).ToList();
                var recordsTotal = result.Count();
                var data = result.OrderByDescending(m=>m.UserID).Skip(start).Take(rows).ToList();
                o = new
                {
                    draw = draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsTotal,
                    data = data
                };
                //var list = users.Skip(param.Offset * param.PageSize).Take(param.PageSize).ToList();
                //var psr = new PageSearchResult<List<Tb_Users>>(users.Count, list);
                //result.Data = psr;
            }

            return Json(o, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 回退
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult GobackDeveloper()
        {
            return RedirectToAction("Developer");
        }
        #endregion


    }
}