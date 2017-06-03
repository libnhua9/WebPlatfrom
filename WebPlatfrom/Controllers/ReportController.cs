using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.SqlServer.Server;
using Web.Commons;
using Web.EF;
using Web.Models.WebModels;

namespace Web.Controllers
{
    [WebAuth]
    public class ReportController : Controller
    {
        #region 设备
        // GET: Report
        #region TicketReport
        /// <summary>
        /// 安全检票闸机
        /// </summary>
        /// <returns></returns>
        public ActionResult TicketReport()
        {
            return View();
        }

        /// <summary>
        /// 安全出口闸机
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="devicetKey">设备序列号</param>
        /// <param name="datetimes">时间</param>
        [CheckSession]
        [HttpPost]
        public JsonResult GetTicketPeport(int type,string devicetKey,string datetimes)
        {
            var sum = new List<int>();
            var min = new List<int>();
            var max = new List<int>();
            var count = new List<int>();
            var avg = new List<int>();
            var saveDate = new List<string>();
            using (var  db=DbHelper.Create())
            {
                var  users=this.Load<Tb_Users>();
                var level =users.Level!=2?0:2;
                var userid = users.UserID;
                var list = db.Database.SqlQuery<P_Logs_Ticket_Report_Result>(" exec P_Logs_Ticket_Report "+type+" ,"+ userid + ","+level+"");
                //var year = DateTime.Now.Year;//年
                var month = DateTime.Now.Month;//月
                var day = DateTime.Now.Day;//日
                var hour = DateTime.Now.Hour;//小时
                var param = type == 1? day : hour;
                var m = 0;
                for (int i = 1; i <= param; i++)
                {
                    m++;
                    if (type == 1) { saveDate.Add(month + "月" + m + "日"); }
                    else { saveDate.Add(m + "小时"); }
                    count.Add(0);
                    foreach (var item in list)
                    {
                        if (i == item.SaveDate)
                        {
                            if (type == 1)
                            {
                                if (item.SaveDate != null) saveDate[i-1]= month + "月" + item.SaveDate + "日";
                                if (item.countElapsedTime != null) count[i - 1] = (int)item.countElapsedTime;
                            }
                            else
                            {
                                if (item.SaveDate != null) saveDate[i-1]= item.SaveDate+"小时";
                                if (item.countElapsedTime != null) count[i-1]=(int)item.countElapsedTime;
                            }
                        }
                    }
                }
                foreach (var item in list)
                {
                    if (item.avgElapsedTime != null) avg.Add((int)item.avgElapsedTime);
                    if (item.sumElapsedTime != null) sum.Add((int)item.sumElapsedTime);
                    if (item.maxElapsedTime != null) max.Add((int)item.maxElapsedTime);
                    if (item.minElapsedTime != null) min.Add((int)item.minElapsedTime);
                }
            }
          
            var o = new
            {
                sum = sum,
                min = min,
                max = max,
                count=count,
                saveDate = saveDate,
                avg = avg,

            };
            return Json(o, JsonRequestBehavior.AllowGet);
        }
      
        #region 安全出口闸机名称
        public ActionResult GetDeviceTicket()
        {
             IEnumerable<object> deviceName ;
            using (var db = DbHelper.Create())
            {
                deviceName = db.Tb_Logs_Ticket.Select(m=>m.FunctionName).Distinct().ToList();
            }
           return Json(deviceName,JsonRequestBehavior.AllowGet);

        }
        #endregion 

        #endregion

        #region M4ChannelReport
        /// <summary>
        /// M4出口闸机
        /// </summary>
        /// <returns></returns>
        public ActionResult M4ChannelReport()
        {
            return View();
        }

        /// <summary>
        ///  M4出口闸机
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>返回json</returns>
        [CheckSession]
        [HttpPost]
        public JsonResult GetM4ChannelList(int type)
        {
            var sum = new List<int>();
            var min = new List<int>();
            var max = new List<int>();
            var count = new List<int>();
            var saveDate = new List<string>();
            var avg = new List<int>();
            using (var db = DbHelper.Create())
            {
                var users = this.Load<Tb_Users>();
                var level = users.Level != 2 ? 0 : 2;
                var userid = users.UserID;

                var list = db.Database.SqlQuery<P_Logs_M4Channel_Report_Result>(" exec P_Logs_M4Channel_Report " + type + " ," + userid + "," + level + "");
                var month = DateTime.Now.Month;//月
                var day = DateTime.Now.Day;//日
                var hour = DateTime.Now.Hour;//小时
                var param = type == 1 ? day : hour;
                var m = 0;
                for (int i = 1; i <= param; i++)
                {
                    m++;
                    if (type == 1) { saveDate.Add(month + "月" + m + "日"); }
                    else { saveDate.Add(m + "小时"); }
                    count.Add(0);
                    foreach (var item in list)
                    {
                        if (i == item.SaveDate)
                        {
                            if (type == 1)
                            {
                                if (item.SaveDate != null) saveDate[i - 1] = month + "月" + item.SaveDate + "日";
                                if (item.countElapsedTime != null) count[i - 1] = (int)item.countElapsedTime;
                            }
                            else
                            {
                                if (item.SaveDate != null) saveDate[i - 1] = item.SaveDate + "小时";
                                if (item.countElapsedTime != null) count[i - 1] = (int)item.countElapsedTime;
                            }
                        }
                    }
                }
                foreach (var item in list)
                {
                    if (item.avgElapsedTime != null) avg.Add((int)item.avgElapsedTime);
                    if (item.sumElapsedTime != null) sum.Add((int)item.sumElapsedTime);
                    if (item.maxElapsedTime != null) max.Add((int)item.maxElapsedTime);
                    if (item.minElapsedTime != null) min.Add((int)item.minElapsedTime);
                }
            }

            var o = new
            {
                sum = sum,
                min = min,
                max = max,
                count = count,
                saveDate = saveDate,
                avg = avg,

            };
            return Json(o, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ParkStroeReport
        /// <summary>
        /// 水公园储物柜
        /// </summary>
        /// <returns></returns>
        public ActionResult ParkStroeReport()
        {
            return View();
        }
        /// <summary>
        /// 水公园储物柜 报表
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>返回json</returns>
        [CheckSession]
        [HttpPost]
        public JsonResult GetParkStroeList(int type)
        {
            var sum = new List<int>();
            var min = new List<int>();
            var max = new List<int>();
            var count = new List<int>();
            var saveDate = new List<string>();
            var avg = new List<int>();
            using (var db = DbHelper.Create())
            {
                var users = this.Load<Tb_Users>();
                var level = users.Level != 2 ? 0 : 2;
                var userid = users.UserID;

                var list = db.Database.SqlQuery<P_Logs_ParkStore_Report_Result>(" exec P_Logs_ParkStore_Report " + type + " ," + userid + "," + level + "");
                var month = DateTime.Now.Month;//月
                var day = DateTime.Now.Day;//日
                var hour = DateTime.Now.Hour;//小时
                var param = type == 1 ? day : hour;
                var m = 0;
                for (int i = 1; i <= param; i++)
                {
                    m++;
                    if (type == 1) { saveDate.Add(month + "月" + m + "日"); }
                    else { saveDate.Add(m + "小时"); }
                    count.Add(0);
                    foreach (var item in list)
                    {
                        if (i == item.SaveDate)
                        {
                            if (type == 1)
                            {
                                if (item.SaveDate != null) saveDate[i - 1] = month + "月" + item.SaveDate + "日";
                                if (item.countElapsedTime != null) count[i - 1] = (int)item.countElapsedTime;
                            }
                            else
                            {
                                if (item.SaveDate != null) saveDate[i - 1] = item.SaveDate + "小时";
                                if (item.countElapsedTime != null) count[i - 1] = (int)item.countElapsedTime;
                            }
                        }
                    }
                }
                foreach (var item in list)
                {
                    if (item.avgElapsedTime != null) avg.Add((int)item.avgElapsedTime);
                    if (item.sumElapsedTime != null) sum.Add((int)item.sumElapsedTime);
                    if (item.maxElapsedTime != null) max.Add((int)item.maxElapsedTime);
                    if (item.minElapsedTime != null) min.Add((int)item.minElapsedTime);
                }
            }

            var o = new
            {
                sum = sum,
                min = min,
                max = max,
                count = count,
                saveDate = saveDate,
                avg = avg,

            };
            return Json(o, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SelfCabineReport
        /// <summary>
        /// 自助储物柜
        /// </summary>
        /// <returns></returns>
        public ActionResult SelfCabineReport()
        {
            return View();
        }

        /// <summary>
        /// 自助储物柜报表
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>返回json</returns>
        [CheckSession]
        [HttpPost]
        public JsonResult GetSelfCabineList(int type)
        {
            var sum = new List<int>();
            var min = new List<int>();
            var max = new List<int>();
            var count = new List<int>();
            var saveDate = new List<string>();
            var avg = new List<int>();
            using (var db = DbHelper.Create())
            {
                var users = this.Load<Tb_Users>();
                var level = users.Level != 2 ? 0 : 2;
                var userid = users.UserID;

                var list = db.Database.SqlQuery<P_Logs_SelfCabinet_Report_Result>(" exec P_Logs_SelfCabinet_Report " + type + " ," + userid + "," + level + "");
                var month = DateTime.Now.Month;//月
                var day = DateTime.Now.Day;//日
                var hour = DateTime.Now.Hour;//小时
                var param = type == 1 ? day : hour;
                var m = 0;
                for (int i = 1; i <= param; i++)
                {
                    m++;
                    if (type == 1) { saveDate.Add(month + "月" + m + "日"); }
                    else { saveDate.Add(m + "小时"); }
                    count.Add(0);
                    foreach (var item in list)
                    {
                        if (i == item.SaveDate)
                        {
                            if (type == 1)
                            {
                                if (item.SaveDate != null) saveDate[i - 1] = month + "月" + item.SaveDate + "日";
                                if (item.countElapsedTime != null) count[i - 1] = (int)item.countElapsedTime;
                            }
                            else
                            {
                                if (item.SaveDate != null) saveDate[i - 1] = item.SaveDate + "小时";
                                if (item.countElapsedTime != null) count[i - 1] = (int)item.countElapsedTime;
                            }
                        }
                    }
                }
                foreach (var item in list)
                {
                    if (item.avgElapsedTime != null) avg.Add((int)item.avgElapsedTime);
                    if (item.sumElapsedTime != null) sum.Add((int)item.sumElapsedTime);
                    if (item.maxElapsedTime != null) max.Add((int)item.maxElapsedTime);
                    if (item.minElapsedTime != null) min.Add((int)item.minElapsedTime);
                }
            }

            var o = new
            {
                sum = sum,
                min = min,
                max = max,
                count = count,
                saveDate = saveDate,
                avg = avg,

            };
            return Json(o, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region TVMReport
        /// <summary>
        /// TVM设备报表
        /// </summary>
        /// <returns></returns>
        public ActionResult TVMReport()
        {
            return View();
        }
        /// <summary>
        /// TVM报表
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>返回json</returns>
        [CheckSession]
        [HttpPost]
        public JsonResult GetTVMReport(int type)
        {
            var sum = new List<int>();
            var min = new List<int>();
            var max = new List<int>();
            var count = new List<int>();
            var saveDate = new List<string>();
            var avg = new List<int>();
            using (var db = DbHelper.Create())
            {
                var users = this.Load<Tb_Users>();
                var level = users.Level != 2 ? 0 : 2;
                var userid = users.UserID;

                var list = db.Database.SqlQuery<P_Logs_TVM_Report_Result>(" exec P_Logs_TVM_Report " + type + " ," + userid + "," + level + "");
                var month = DateTime.Now.Month;//月
                var day = DateTime.Now.Day;//日
                var hour = DateTime.Now.Hour;//小时
                var param = type == 1 ? day : hour;
                var m = 0;
                for (int i = 1; i <= param; i++)
                {
                    m++;
                    if (type == 1) { saveDate.Add(month + "月" + m + "日"); }
                    else { saveDate.Add(m + "小时"); }
                    count.Add(0);
                    foreach (var item in list)
                    {
                        if (i == item.SaveDate)
                        {
                            if (type == 1)
                            {
                                if (item.SaveDate != null) saveDate[i - 1] = month + "月" + item.SaveDate + "日";
                                if (item.countElapsedTime != null) count[i - 1] = (int)item.countElapsedTime;
                            }
                            else
                            {
                                if (item.SaveDate != null) saveDate[i - 1] = item.SaveDate + "小时";
                                if (item.countElapsedTime != null) count[i - 1] = (int)item.countElapsedTime;
                            }
                        }
                    }
                }
                foreach (var item in list)
                {
                    if (item.avgElapsedTime != null) avg.Add((int)item.avgElapsedTime);
                    if (item.sumElapsedTime != null) sum.Add((int)item.sumElapsedTime);
                    if (item.maxElapsedTime != null) max.Add((int)item.maxElapsedTime);
                    if (item.minElapsedTime != null) min.Add((int)item.minElapsedTime);
                }
            }

            var o = new
            {
                sum = sum,
                min = min,
                max = max,
                count = count,
                saveDate = saveDate,
                avg = avg,

            };
            return Json(o, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region DVRReport
        /// <summary>
        /// 海康DVR
        /// </summary>
        /// <returns></returns>
        public ActionResult DVRReport()
        {
            return View();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pages">分页id</param>
        /// <returns>返回json</returns>
        //public JsonResult GetDVRList()
        //{
        //    var start = Convert.ToInt32(Request["start"]);
        //    var rows = Convert.ToInt32(Request["length"]);
        //    var draw = Convert.ToInt32(Request["draw"]);
        //    var type = Convert.ToInt32(Request["Type"]);
        //    var startSaveDate = Convert.ToInt32(Request["startSaveDate"]);//保存开始时间
        //    var endtSaveDate = Convert.ToInt32(Request["endtSaveDate"]);  //保存结束时间
        //    var deviceKey = Convert.ToInt32(Request["deviceKey"]);        //安全出口闸机KEY
        //    var functionName = Request["functionName"];                   //名称     
        //    var o = new object();
        //    using (var db = DbHelper.Create())
        //    {
        //        var result = (IEnumerable<Tb_Logs_TVM>)db.Tb_Logs_DocomPay;
        //        if (type != -1)
        //        {
        //            result = result.Where(m => m.Result == type);
        //        }
        //        if (!string.IsNullOrEmpty(functionName))
        //        {
        //            result = result.Where(m => m.FunctionName.Contains(functionName));
        //        }
        //        var recordsTotal = result.Count();
        //        var data = result.Skip(start).Take(rows).ToList();
        //        o = new
        //        {
        //            draw = draw,
        //            recordsTotal = recordsTotal,
        //            recordsFiltered = recordsTotal,
        //            data = data
        //        };

        //    }
        //    return Json(o, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region LEDReport
        /// <summary>
        /// LED显示屏
        /// </summary>
        /// <returns></returns>
        public ActionResult LEDReport()
        {
            return View();
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>返回json</returns>
        [CheckSession]
        [HttpPost]
        public JsonResult GetLEDReport(int type)
        {
            var sum = new List<int>();
            var min = new List<int>();
            var max = new List<int>();
            var count = new List<int>();
            var saveDate = new List<string>();
            var avg = new List<int>();
            using (var db = DbHelper.Create())
            {
                var users = this.Load<Tb_Users>();
                var level = users.Level != 2 ? 0 : 2;
                var userid = users.UserID;

                var list = db.Database.SqlQuery<P_Logs_Ticket_Report_Result>(" exec P_Logs_LED_Report " + type + " ," + userid + "," + level + "");
                var month = DateTime.Now.Month;//月
                var day = DateTime.Now.Day;//日
                var hour = DateTime.Now.Hour;//小时
                var param = type == 1 ? day : hour;
                var m = 0;
                for (int i = 1; i <= param; i++)
                {
                    m++;
                    if (type == 1) { saveDate.Add(month + "月" + m + "日"); }
                    else { saveDate.Add(m + "小时"); }
                    count.Add(0);
                    foreach (var item in list)
                    {
                        if (i == item.SaveDate)
                        {
                            if (type == 1)
                            {
                                if (item.SaveDate != null) saveDate[i - 1] = month + "月" + item.SaveDate + "日";
                                if (item.countElapsedTime != null) count[i - 1] = (int)item.countElapsedTime;
                            }
                            else
                            {
                                if (item.SaveDate != null) saveDate[i - 1] = item.SaveDate + "小时";
                                if (item.countElapsedTime != null) count[i - 1] = (int)item.countElapsedTime;
                            }
                        }
                    }
                }
                foreach (var item in list)
                {
                    if (item.avgElapsedTime != null) avg.Add((int)item.avgElapsedTime);
                    if (item.sumElapsedTime != null) sum.Add((int)item.sumElapsedTime);
                    if (item.maxElapsedTime != null) max.Add((int)item.maxElapsedTime);
                    if (item.minElapsedTime != null) min.Add((int)item.minElapsedTime);
                }
            }

            var o = new
            {
                sum = sum,
                min = min,
                max = max,
                count = count,
                saveDate = saveDate,
                avg = avg,

            };
            return Json(o, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ESensorReport
        /// <summary>
        /// 客流计
        /// </summary>
        /// <returns></returns>
        public ActionResult ESensorReport()
        {
            return View();
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pages">分页id</param>
        /// <returns>返回json</returns>
        [CheckSession]
        [HttpPost]
        public JsonResult GeteSensorList()
        {
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            var type = Convert.ToInt32(Request["Type"]);
            var startSaveDate = Convert.ToInt32(Request["startSaveDate"]);//保存开始时间
            var endtSaveDate = Convert.ToInt32(Request["endtSaveDate"]);  //保存结束时间
            var deviceKey = Convert.ToInt32(Request["deviceKey"]);        //安全出口闸机KEY
            var functionName = Request["functionName"];                   //名称     
            var o = new object();
            using (var db = DbHelper.Create())
            {
                var result = (IEnumerable<Tb_Logs_eSensor>)db.Tb_Logs_eSensor;
                //if (type != -1)
                //{
                //    result = result.Where(m => m.Result == type);
                //}
                //if (!string.IsNullOrEmpty(functionName))
                //{
                //    result = result.Where(m => m.FunctionName.Contains(functionName));
                //}
                var recordsTotal = result.Count();
                var data = result.Skip(start).Take(rows).ToList();
                o = new
                {
                    draw = draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsTotal,
                    data = data
                };

            }
            return Json(o, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region 接口
        /// <summary>
        ///性能--人脸识别
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public ActionResult PerformanceFace()
        {
            return View();
        }

        /// <summary>
        ///性能--道控支付接口
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public ActionResult PerformancePay()
        {
            return View();
        }

        /// <summary>
        /// 统计
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public ActionResult Statistics()
        {
            return View();
        }
        
        /// <summary>
        ///明细--人脸识别
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public ActionResult DetailFace()
        {
            return View();
        }
        /// <summary>
        ///明细--道控支付接口
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public ActionResult DetailPay()
        {
            return View();
        }

        /// <summary>
        /// 接口支付明显列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPayList()
        {
            return null;
        }
        /// <summary>
        /// 接口人脸识别明显列表
        /// </summary>
        /// <returns></returns>
        public ActionResult FaceAPI()
        {
            return null;
        }
        /// <summary>
        /// 支付接口明细
        /// </summary>
        /// <param name="pages">分页id</param>
        /// <returns>返回json</returns>
        public JsonResult GetDocomPayList()
        {
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            var type = Convert.ToInt32(Request["Type"]);
            var startSaveDate = Convert.ToInt32(Request["startSaveDate"]);//保存开始时间
            var endtSaveDate = Convert.ToInt32(Request["endtSaveDate"]);  //保存结束时间
            var deviceKey = Convert.ToInt32(Request["deviceKey"]);        //安全出口闸机KEY
            var functionName = Request["functionName"];                   //名称     
            var o = new object();
            using (var db = DbHelper.Create())
            {
                var result = (IEnumerable<Tb_Logs_DocomPay>)db.Tb_Logs_DocomPay;
                if (type != -1)
                {
                    result = result.Where(m => m.Result == type);
                }
                if (!string.IsNullOrEmpty(functionName))
                {
                    result = result.Where(m => m.Customer.Contains(functionName));
                }
                var recordsTotal = result.Count();
                var data = result.Skip(start).Take(rows).ToList();
                o = new
                {
                    draw = draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsTotal,
                    data = data
                };

            }
            return Json(o, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 人脸识别接口明细
        /// </summary>
        /// <param name="pages">分页id</param>
        /// <returns>返回json</returns>
        public JsonResult GetFaceAPIList()
        {
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            var type = Convert.ToInt32(Request["Type"]);
            var startSaveDate = Convert.ToInt32(Request["startSaveDate"]);//保存开始时间
            var endtSaveDate = Convert.ToInt32(Request["endtSaveDate"]);  //保存结束时间
            var deviceKey = Convert.ToInt32(Request["deviceKey"]);        //安全出口闸机KEY
            var functionName = Request["functionName"];                   //名称     
            var o = new object();
            using (var db = DbHelper.Create())
            {
                var result = (IEnumerable<Tb_Logs_FaceAPI>)db.Tb_Logs_FaceAPI;
                if (type != -1)
                {
                    result = result.Where(m => m.Result == type);
                }
                if (!string.IsNullOrEmpty(functionName))
                {
                    result = result.Where(m => m.FunctionName.Contains(functionName));
                }
                var recordsTotal = result.Count();
                var data = result.Skip(start).Take(rows).ToList();
                o = new
                {
                    draw = draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsTotal,
                    data = data
                };

            }
            return Json(o, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 道控支付API 性能
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetFaceAPIReport(int type)
        {
            //var sw = new Stopwatch();
            //sw.Start();

            var sum = new List<int>();
            var min = new List<int>();
            var max = new List<int>();
            var count = new List<int>();
            var saveDate = new List<string>();
            var avg = new List<int>();
            var result = ActionJsonResult.Create();
            using (var db = DbHelper.Create())
            {

                var users = this.Load<Tb_Users>();
                var level = users.Level;
                var userid = users.UserID;
                string StrSql = "";
                string sqlWhere = "";
                string UserId = "";
                if (type == 1)
                {
                    
                    if (level > 1)
                    {
                        sqlWhere = " and UserID=" + userid + " ";
                        UserId = " UserID, ";
                    }
                    StrSql = " select sum(UseTime) as sumElapsedTime ,max(UseTime) as maxElapsedTime,count(UseTime) as countElapsedTime,avg(UseTime) as avgElapsedTime, min(UseTime) as minElapsedTime," + UserId + " day(Tb_Logs_FaceAPI.CallTime) as SaveDate from Tb_Logs_FaceAPI " +
                                 " where year(Tb_Logs_FaceAPI.CallTime) = year(getdate())  and month(Tb_Logs_FaceAPI.CallTime) = month(getdate()) "+sqlWhere+" " +
                                 " group by "+ UserId + " day(Tb_Logs_FaceAPI.CallTime)";
                }
                else {
                    if (level > 1)
                    {
                        sqlWhere = " and UserID=" + userid + " ";
                        UserId = " UserID, ";
                    }
                    StrSql = "select sum(UseTime) as sumElapsedTime ,max(UseTime) as maxElapsedTime,count(UseTime) as countElapsedTime,avg(UseTime) as avgElapsedTime, min(UseTime) as minElapsedTime," + UserId + " DATEPART(hh,Tb_Logs_FaceAPI.CallTime) as SaveDate from Tb_Logs_FaceAPI " +
                                " where year(Tb_Logs_FaceAPI.CallTime) = year(getdate())  and month(Tb_Logs_FaceAPI.CallTime) = month(getdate()) "+
                                " and  day(Tb_Logs_FaceAPI.CallTime) = day(getdate()) "+sqlWhere+" group by "+ UserId + " DATEPART(hh, Tb_Logs_FaceAPI.CallTime)";
                }

                var sql = new StringBuilder(StrSql);

                var list = db.Database.SqlQuery<FaceApiData>(sql.ToString()).ToList();
                //var psr = new PageSearchResult<List<FaceApiData>>(list.Count, list);
                //result.Data = list;
                var month = DateTime.Now.Month;//月
                var day = DateTime.Now.Day;//日
                var hour = DateTime.Now.Hour;//小时
                var param = type == 1 ? day : hour;
                var m = 0;
                for (int i = 1; i <= param; i++)
                {
                    m++;
                    if (type == 1) { saveDate.Add(month + "月" + m + "日"); }
                    else { saveDate.Add(m + "小时"); }
                    count.Add(0);
                    foreach (var item in list)
                    {
                        if (i == item.SaveDate)
                        {
                            if (type == 1)
                            {
                                if (item.SaveDate != null) saveDate[i - 1] = month + "月" + item.SaveDate + "日";
                                if (item.countElapsedTime != null) count[i - 1] = (int)item.countElapsedTime;
                            }
                            else
                            {
                                if (item.SaveDate != null) saveDate[i - 1] = item.SaveDate + "小时";
                                if (item.countElapsedTime != null) count[i - 1] = (int)item.countElapsedTime;
                            }
                        }
                    }
                }
                foreach (var item in list)
                {
                    if (item.avgElapsedTime != null) avg.Add((int)item.avgElapsedTime);
                    if (item.sumElapsedTime != null) sum.Add((int)item.sumElapsedTime);
                    if (item.maxElapsedTime != null) max.Add((int)item.maxElapsedTime);
                    if (item.minElapsedTime != null) min.Add((int)item.minElapsedTime);
                }

            }
            var o = new
            {
                sum = sum,
                min = min,
                max = max,
                count = count,
                saveDate = saveDate,
                avg = avg,

            };
            return Json(o, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 人脸识别API 性能
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetDocomPayAPIReport(int type)
        {
            //var sw = new Stopwatch();
            //sw.Start();

            var sum = new List<int>();
            var min = new List<int>();
            var max = new List<int>();
            var count = new List<int>();
            var saveDate = new List<string>();
            var avg = new List<int>();
            var result = ActionJsonResult.Create();
            using (var db = DbHelper.Create())
            {
                var users = this.Load<Tb_Users>();
                var level = users.Level;
                var userid = users.UserID;

                string sqlWhere = "";
                string StrSql = "";
                string UserId = "";
                if (type == 2)
                {
                    
                    if (level>1) {
                        sqlWhere = " and Customer=" + userid + " ";
                        UserId = " Customer, ";
                    }
                    StrSql = "select sum(UseTime) as sumElapsedTime, max(UseTime) as maxElapsedTime, count(UseTime) as countElapsedTime, avg(UseTime) as avgElapsedTime, min(UseTime) as minElapsedTime, "+ UserId + " DATEPART(hh, Tb_Logs_DocomPay.CallTime) as SaveDate from Tb_Logs_DocomPay " +
                                 " where year(Tb_Logs_DocomPay.CallTime) = year(getdate())  and month(Tb_Logs_DocomPay.CallTime) = month(getdate()) "+
                                 " and  day(Tb_Logs_DocomPay.CallTime) = day(getdate()) "+sqlWhere+" group by "+ UserId + " DATEPART(hh, Tb_Logs_DocomPay.CallTime)";
                }
                else
                {
                    if (level > 1)
                    {
                        sqlWhere = " and Customer=" + userid + " ";
                        UserId = " Customer, ";
                    }
                    StrSql = "select sum(UseTime) as sumElapsedTime, max(UseTime) as maxElapsedTime, count(UseTime) as countElapsedTime, avg(UseTime) as avgElapsedTime, min(UseTime) as minElapsedTime, "+ UserId + " day(Tb_Logs_DocomPay.CallTime) as SaveDate from Tb_Logs_DocomPay " +
                                " where year(Tb_Logs_DocomPay.CallTime) = year(getdate())  and month(Tb_Logs_DocomPay.CallTime) = month(getdate()) "+sqlWhere+" "+
                                " group by "+ UserId + " day(Tb_Logs_DocomPay.CallTime)";
                }


                var sql = new StringBuilder(StrSql);
                var list = db.Database.SqlQuery<FaceApiData>(sql.ToString()).ToList();
                //var psr = new PageSearchResult<List<FaceApiData>>(list.Count, list);
                //result.Data = list;
                var month = DateTime.Now.Month;//月
                var day = DateTime.Now.Day;//日
                var hour = DateTime.Now.Hour;//小时
                var param = type == 1 ? day : hour;
                var m = 0;
                for (int i = 1; i <= param; i++)
                {
                    m++;
                    if (type == 1) { saveDate.Add(month + "月" + m + "日"); }
                    else { saveDate.Add(m + "小时"); }
                    count.Add(0);
                    foreach (var item in list)
                    {
                        if (i == item.SaveDate)
                        {
                            if (type == 1)
                            {
                                if (item.SaveDate != null) saveDate[i - 1] = month + "月" + item.SaveDate + "日";
                                if (item.countElapsedTime != null) count[i - 1] = (int)item.countElapsedTime;
                            }
                            else
                            {
                                if (item.SaveDate != null) saveDate[i - 1] = item.SaveDate + "小时";
                                if (item.countElapsedTime != null) count[i - 1] = (int)item.countElapsedTime;
                            }
                        }
                    }
                }
                foreach (var item in list)
                {
                    if (item.avgElapsedTime != null) avg.Add((int)item.avgElapsedTime);
                    if (item.sumElapsedTime != null) sum.Add((int)item.sumElapsedTime);
                    if (item.maxElapsedTime != null) max.Add((int)item.maxElapsedTime);
                    if (item.minElapsedTime != null) min.Add((int)item.minElapsedTime);
                }
            }
            var o = new
            {
                sum = sum,
                min = min,
                max = max,
                count = count,
                saveDate = saveDate,
                avg = avg,

            };
            return Json(o, JsonRequestBehavior.AllowGet);

        }
        #endregion
    }



}