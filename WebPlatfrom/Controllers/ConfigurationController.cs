using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.ServiceProcess;
using System.Web;
using System.Web.Mvc;
using DocomSDK;
using Web.Commons;
using Web.EF;
using Web.Models.WebModels;

namespace Web.Controllers
{
    [WebAuth]
    public class ConfigurationController : Controller
    {
        #region 系统配置--配置参数

        // GET: Configuration
        /// <summary>
        /// 系统配置--配置参数
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult ConfigParameter()
        {
            ServiceController serviceController1 = new ServiceController();
            //var serviceName = "MySQL";
            //var machineName = ".";
            var serviceName = System.Web.Configuration.WebConfigurationManager.AppSettings["SevicesName"];
            var machineName = System.Web.Configuration.WebConfigurationManager.AppSettings["SevicesIP"];
            serviceController1.ServiceName = serviceName;
            serviceController1.MachineName = machineName;
            try
            {
                //serviceController1.Status == ServiceControllerStatus.Paused;
                ViewBag.Status = serviceController1.Status.ToString() == "Running" ? "服务正在运行" : serviceController1.Status.ToString() == "StartPending" ? "服务器正在启动" : serviceController1.Status.ToString() == "StopPending" ? "服务器正在停止" : serviceController1.Status.ToString() == "Stopped" ? "服务未运行" : serviceController1.Status.ToString() == "Paused" ? "服务器暂停" : "未知状态";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // throw;
            }

            using (var db = DbHelper.Create())
            {
                var data = db.TB_Config.ToList();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        if (item.ID == 0)
                        {
                            ViewBag.Binary = item.Value;
                        }
                        if (item.ID == 1)
                        {
                            ViewBag.Json = item.Value;
                        }
                        if (item.ID == 2)
                        {
                            ViewBag.Msg = item.Value;
                        }
                    }
                }

            }
            return View();
        }
        [CheckSession]
        [HttpPost]
        public string SaveConfig(int type, int value)
        {

            var msg = "";
            var count = 0;
            using (var db = DbHelper.Create())
            {
                count = db.TB_Config.Where(m => m.Value == value && m.ID != type).Count();
            }
            if (count >= 1)
            {
                msg = "该端口已添加有（" + value + "）";
            }
            else
            {

                switch (type)
                {
                    case 0:
                        using (var db = DbHelper.Create())
                        {
                            try
                            {
                                var result = db.TB_Config.SingleOrDefault(m => m.ID == type);
                                if (result != null)
                                {
                                    result.Value = value;
                                    db.Entry(result).State = EntityState.Modified;
                                    msg = "更新成功";
                                }
                                else
                                {
                                    var config = new TB_Config();
                                    config.ID = (int)SystemConfig.BinaryPort;
                                    config.Value = value;
                                    config.Name = SystemConfig.BinaryPort.ToString();
                                    db.TB_Config.Add(config);
                                    msg = "保存成功";
                                }
                            }
                            catch (Exception e)
                            {
                                msg = "保存失败" + e;
                                throw;
                            }

                            db.SaveChanges();
                        }
                        break;
                    case 1:
                        using (var db = DbHelper.Create())
                        {
                            try
                            {
                                var result = db.TB_Config.SingleOrDefault(m => m.ID == type);
                                if (result != null)
                                {
                                    result.Value = value;
                                    db.Entry(result).State = EntityState.Modified;
                                    msg = "更新成功";
                                }
                                else
                                {
                                    var config = new TB_Config();
                                    config.ID = (int)SystemConfig.JSONPort;
                                    config.Value = value;
                                    config.Name = SystemConfig.JSONPort.ToString();
                                    db.TB_Config.Add(config);
                                    msg = "保存成功";
                                }
                            }
                            catch (Exception e)
                            {
                                msg = "保存失败" + e;
                                //throw;
                            }

                            db.SaveChanges();
                        }
                        break;
                    case 2:
                        using (var db = DbHelper.Create())
                        {
                            try
                            {
                                var result = db.TB_Config.SingleOrDefault(m => m.ID == type);
                                if (result != null)
                                {
                                    result.Value = value;
                                    db.Entry(result).State = EntityState.Modified;
                                    msg = "更新成功";
                                }
                                else
                                {
                                    var config = new TB_Config();
                                    config.ID = (int)SystemConfig.MessagePort;
                                    config.Value = value;
                                    config.Name = SystemConfig.MessagePort.ToString();
                                    db.TB_Config.Add(config);
                                    msg = "保存成功";
                                }
                            }
                            catch (Exception e)
                            {
                                msg = "保存失败" + e;
                                //throw;
                            }

                            db.SaveChanges();
                        }
                        break;
                    default:
                        break;
                }
            }
            return msg;
        }
        #endregion

        #region 服务启动、关闭
        /// <summary>
        /// 服务启动、关闭
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpGet]
        public ActionResult Service(int type)
        {
            var result = "";
            var status = "";

            ServiceController serviceController1 = new ServiceController();
            var serviceName = System.Web.Configuration.WebConfigurationManager.AppSettings["SevicesName"];
            var machineName = System.Web.Configuration.WebConfigurationManager.AppSettings["SevicesIP"];
            //var serviceName = "MySQL";
            //var machineName = ".";
            serviceController1.ServiceName = serviceName;
            serviceController1.MachineName = machineName;

            if (type == 2)
            {
                try
                {
                    if (serviceController1.Status == ServiceControllerStatus.Running)
                        serviceController1.Stop();

                    result = "停止服务成功";
                    status = "服务未运行";
                }
                catch (Exception e)
                {
                    result = "停止服务失败" + e;
                    status = "停止服务失败";

                }
            }
            else
            {
                try
                {
                    if (serviceController1.Status != ServiceControllerStatus.Running)
                        serviceController1.Start();
                    result = "启动服务成功";
                    status = "服务正在运行";

                }
                catch (Exception e)
                {
                    status = "启动服务失败";
                    result = "启动服务失败" + e;
                    //throw;
                }
            }
            return Json(new { result = result, status = status }, JsonRequestBehavior.AllowGet);
        }


        public enum SystemConfig : int
        {
            /// <summary>
            /// 二进制
            /// </summary>
            BinaryPort = 0,
            JSONPort = 1,
            MessagePort = 2
        }
        #endregion

        #region 子系统配置
        public ActionResult SubSystemConfig()
        {
            using (var db = DbHelper.Create())
            {

                var data = db.Tb_Users_Subsystem.Select(s => new UsersSubsystem { API_Url = s.API_Url, StartDate = s.StartDate.ToString(), EndDate = s.EndDate.ToString(), Remarks = s.Remarks, Enabled = (int)s.Enabled, ProductType = s.SubSystemID });
                ViewBag.Ticket = data.FirstOrDefault(s => s.ProductType == 0);
                ViewBag.TVM = data.FirstOrDefault(s => s.ProductType == 2);
                ViewBag.M4_ExitChannel = data.FirstOrDefault(s => s.ProductType == 3);

                return View();
            }
        }

        /// <summary>
        /// 保存子系统配置
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="url">url</param> 
        /// <param name="starts"> 开始时间</param> 
        /// <param name="end">结束时间</param>
        /// <param name="system"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public string GetSaveSystemConfig(int type, Tb_Users_Subsystem system)
        {
            var users = this.Load<Tb_Users>();
            var msg = "";
            try
            {
                var subSystemID = type == 0
                                ? (int)DocomSDK.ProductType.Ticket : type == 2
                                ? (int)DocomSDK.ProductType.TVM : type == 3
                                ? (int)DocomSDK.ProductType.M4_ExitChannel : 0;
                var subName = type == 0
                                ? DocomSDK.ProductType.Ticket.ToString() : type == 2
                                ? DocomSDK.ProductType.TVM.ToString() : type == 3
                                ? DocomSDK.ProductType.M4_ExitChannel.ToString() : "";
                using (var db = DbHelper.Create())
                {
                    var result = db.Tb_Users_Subsystem.SingleOrDefault(m => m.SubSystemID == type);
                    if (result != null)
                    {
                        result.API_Url = system.API_Url;
                        result.EndDate = system.StartDate;
                        result.EndDate = system.EndDate;
                        result.UserID = users.UserID;
                        result.Remarks = system.Remarks;
                        result.Enabled = system.Enabled;
                        db.Entry(result).State = EntityState.Modified;
                        msg = "更新成功";
                    }
                    else
                    {
                        system.UserID = users.UserID;
                        system.SubSystemID = subSystemID;
                        system.SubName = subName;
                        db.Tb_Users_Subsystem.Add(system);
                        msg = "保存成功";
                    }
                    db.SaveChanges();
                }

            }
            catch (Exception)
            {
                msg = "保存失败";
            }

            return msg;
        }

        #endregion
    }

}