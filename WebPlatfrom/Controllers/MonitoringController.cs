using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Diagnostics;
using System.Web.Mvc;
using Web.Commons;
using Web.EF;
using Web.Models.WebModels;
using DocomSDK.Ticket.Enum;

namespace Web.Controllers
{
    [WebAuth]
    public class MonitoringController : Controller
    {


        #region 设备状态
        /// <summary>
        /// 设备状态
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult EquipStatus()
        {
            using (var db = DbHelper.Create())
            {
                ViewBag.list = db.V_Status_Ticket.OrderBy(a => a.DeviceID).ToList();
            }
            return View();
        }

        /// <summary>
        /// 定时查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetStatusTicketList()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            var result = ActionJsonResult.Create();
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;


            using (var db = DbHelper.Create())
            {

                
                if (tbuser.Level > 1)
                {
                    List<V_FP_Status_Ticket> list1 = new List<V_FP_Status_Ticket>();
                    var list = db.V_FP_Status_Ticket.ToList();
                    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                    var psr = new PageSearchResult<List<V_FP_Status_Ticket>>(list.Count, list);
                    foreach (var item in list)
                    {
                        V_FP_Status_Ticket ticket = new V_FP_Status_Ticket();
                        ticket.DeviceName = item.DeviceName;
                        ticket.DeviceID = item.DeviceID;
                        ticket.DeviceSN = item.DeviceSN;
                        if (item.UpdateDate == null)
                        {
                            ticket.Remarks = "";
                        }
                        else
                        {
                            System_Type type = (System_Type)item.SystemType;
                            ticket.Remarks = type.ToString();
                        }
                        ticket.StartDate = item.StartDate;
                        ticket.UpdateDate = item.UpdateDate;
                        ticket.IPAddress = item.IPAddress;
                        ticket.PassCount = item.PassCount;
                        ticket.checkCount = item.checkCount;
                        ticket.Startup = item.Startup;
                        ticket.Lat = item.Lat;
                        ticket.Lng = item.Lng;
                        list1.Add(ticket);
                    }
                    result.Data = list1;
                }
                else
                {
                    List<V_Status_Ticket> list1 = new List<V_Status_Ticket>();
                    
                    var list = db.V_Status_Ticket.ToList();
                    var psr = new PageSearchResult<List<V_Status_Ticket>>(list.Count, list);
                    foreach (var item in list)
                    {
                        V_Status_Ticket ticket = new V_Status_Ticket();
                        ticket.DeviceName = item.DeviceName;
                        ticket.DeviceID = item.DeviceID;
                        ticket.DeviceSN = item.DeviceSN;
                        if (item.UpdateDate == null)
                        {
                            ticket.Remarks = "";
                        }
                        else {
                        System_Type type = (System_Type)item.SystemType;
                        ticket.Remarks= type.ToString();
                        }
                        
                        ticket.StartDate = item.StartDate;
                        ticket.UpdateDate = item.UpdateDate;
                        ticket.IPAddress = item.IPAddress;
                        ticket.PassCount = item.PassCount;
                        ticket.CheckCount = item.CheckCount;
                        ticket.Startup = item.Startup;
                        ticket.Lat = item.Lat;
                        ticket.Lng = item.Lng;
                        list1.Add(ticket);
                    }
                    //System_Type type = (System_Type)list[0].SystemType;
                    //type.ToString();
                    result.Data = list1;
                }



            }

            //sw.Stop();
            // Debug.WriteLine(sw.ElapsedMilliseconds);
            return result.ToJson();
        }


        /// <summary>
        /// 定时查询没有物理存在的设备
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetStatusTicketNullList()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            var result = ActionJsonResult.Create();
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;
            int map = 0;
            if (!string.IsNullOrEmpty(Request["map"]))
            {
                map = Convert.ToInt32(Request["map"]);
            }

            using (var db = DbHelper.Create())
            {
                if (tbuser.Level > 1)
                {
                    List<V_FP_Status_TicketNull> list1 = new List<V_FP_Status_TicketNull>();
                    var list = db.V_FP_Status_TicketNull.ToList();
                    if (map == 0)
                    {
                        list = db.V_FP_Status_TicketNull.Where(a => a.UpdateDate == null).ToList();
                    }
                    
                    
                    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                    var psr = new PageSearchResult<List<V_FP_Status_TicketNull>>(list.Count, list);
                    foreach (var item in list)
                    {
                        V_FP_Status_TicketNull ticket = new V_FP_Status_TicketNull();
                        ticket.DeviceName = item.DeviceName;
                        ticket.DeviceID = item.DeviceID;
                        ticket.DeviceSN = item.DeviceSN;
                        if (item.UpdateDate == null)
                        {
                            ticket.Remarks = "";
                        }
                        else
                        {
                            System_Type type = (System_Type)item.SystemType;
                            ticket.Remarks = type.ToString();
                        }
                        ticket.StartDate = item.StartDate;
                        //ticket.UpdateDate = ;
                        ticket.IPAddress = item.IPAddress;
                        //ticket.PassCount = 0;
                        //ticket.checkCount = item.checkCount;
                        //ticket.Startup = item.Startup;
                        ticket.Lat = item.Lat;
                        ticket.Lng = item.Lng;
                        list1.Add(ticket);
                    }
                    result.Data = list1;
                }
                else
                {
                    List<V_Status_TicketNull> list1 = new List<V_Status_TicketNull>();
                    var list = db.V_Status_TicketNull.ToList();
                    if (map == 0)
                    {
                        list = db.V_Status_TicketNull.Where(a => a.UpdateDate == null).ToList();
                    }
                        
                    var psr = new PageSearchResult<List<V_Status_TicketNull>>(list.Count, list);
                    foreach (var item in list)
                    {
                        V_Status_TicketNull ticket = new V_Status_TicketNull();
                        ticket.DeviceName = item.DeviceName;
                        ticket.DeviceID = item.DeviceID;
                        ticket.DeviceSN = item.DeviceSN;
                        if (item.UpdateDate == null)
                        {
                            ticket.Remarks = "";
                        }
                        else
                        {
                            System_Type type = (System_Type)item.SystemType;
                            ticket.Remarks = type.ToString();
                        }

                        ticket.StartDate = item.StartDate;
                        ticket.UpdateDate = item.UpdateDate;
                        ticket.IPAddress = item.IPAddress;
                        ticket.PassCount = item.PassCount;
                        ticket.CheckCount = item.CheckCount;
                        ticket.Startup = item.Startup;
                        ticket.Lat = item.Lat;
                        ticket.Lng = item.Lng;
                        list1.Add(ticket);
                    }
                    //System_Type type = (System_Type)list[0].SystemType;
                    //type.ToString();
                    result.Data = list1;
                }

            }
            return result.ToJson();
        }


       

        #region M4出口闸机
        /// <summary>
        /// 设备状态
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult M4ChannelStatus()
        {
            using (var db = DbHelper.Create())
            {
                var sql = new StringBuilder("select S.DeviceSN, S.LastUpdate, S.IPAddress, S.M4WorkMode, S.TotalCount,D.DeviceName,D.DeviceID, D.StartDate, D.EndDate, D.Enabled, D.Version, D.Lat, D.Lng from Tb_Status_M4Channel S inner join Tb_Device_M4Channel D on S.DeviceSN = D.DeviceSN");
                ViewBag.list = db.Database.SqlQuery<M4Channel>(sql.ToString()).ToList();
            }
            return View();
        }



        /// <summary>
        /// 定时查询M4出口闸机
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetStatusList()
        {
            
            var result = ActionJsonResult.Create();
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;


            using (var db = DbHelper.Create())
            {


                if (tbuser.Level > 1)
                {
                    var list = db.V_FP_Status_M4Channel.ToList();
                    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                    var psr = new PageSearchResult<List<V_FP_Status_M4Channel>>(list.Count, list);
                    result.Data = list;
                }
                else
                {
                    var list = db.V_Status_M4Channel.ToList();
                    var psr = new PageSearchResult<List<V_Status_M4Channel>>(list.Count, list);
                    result.Data = list;
                }



            }


            return result.ToJson();
        }

        /// <summary>
        /// 定时查询M4出口闸机（没有物理存在的设备）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetStatusNullList()
        {

            var result = ActionJsonResult.Create();
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;
            int map = 0;
            if (!string.IsNullOrEmpty(Request["map"]))
            {
                map = Convert.ToInt32(Request["map"]);
            }

            using (var db = DbHelper.Create())
            {


                if (tbuser.Level > 1)
                {
                    var list = db.V_FP_Status_M4ChannelNull.ToList();
                    if (map==0) {
                         list = db.V_FP_Status_M4ChannelNull.Where(a => a.LastUpdate == null).ToList();
                    }
                    
                    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                    var psr = new PageSearchResult<List<V_FP_Status_M4ChannelNull>>(list.Count, list);
                    result.Data = list;
                }
                else
                {
                    var list = db.V_Status_M4ChannelNull.ToList();
                    if (map==0) {
                        list = db.V_Status_M4ChannelNull.Where(a => a.LastUpdate == null).ToList();
                    }
                   
                    var psr = new PageSearchResult<List<V_Status_M4ChannelNull>>(list.Count, list);
                    result.Data = list;
                }



            }


            return result.ToJson();
        }





        #endregion

        #region TVM
        /// <summary>
        /// 设备状态
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult TVMStatus()
        {

            return View();
        }


        /// <summary>
        /// 定时查询 TVM
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetStatusTVMList()
        {
          

            var result = ActionJsonResult.Create();
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;


            using (var db = DbHelper.Create())
            {


                if (tbuser.Level > 1)
                {
                    var list = db.V_FP_Status_TVM.ToList();
                    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                    var psr = new PageSearchResult<List<V_FP_Status_TVM>>(list.Count, list);
                    result.Data = list;
                }
                else
                {
                    var list = db.V_Status_TVM.ToList();
                    var psr = new PageSearchResult<List<V_Status_TVM>>(list.Count, list);
                    result.Data = list;
                }



            }



            //sw.Stop();
            // Debug.WriteLine(sw.ElapsedMilliseconds);
            return result.ToJson();
        }

        /// <summary>
        /// 定时查询 TVM(没有物理存在)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetStatusTVMNullList()
        {


            var result = ActionJsonResult.Create();
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;
            int map = 0;
            if (!string.IsNullOrEmpty(Request["UserId"]))
            {
                map = Convert.ToInt32(Request["map"]);
            }

            using (var db = DbHelper.Create())
            {


                if (tbuser.Level > 1)
                {
                    var list = db.V_FP_Status_TVMNull.ToList();
                    if (map==0) {
                        list = db.V_FP_Status_TVMNull.Where(a => a.LastUpdate == null).ToList();
                    }
                    
                    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                    var psr = new PageSearchResult<List<V_FP_Status_TVMNull>>(list.Count, list);
                    result.Data = list;
                }
                else
                {
                    var list = db.V_Status_TVMNull.ToList();
                    if (map == 0) {
                        list = db.V_Status_TVMNull.Where(a => a.LastUpdate == null).ToList();
                    }
                   
                    var psr = new PageSearchResult<List<V_Status_TVMNull>>(list.Count, list);
                    result.Data = list;
                }



            }



            //sw.Stop();
            // Debug.WriteLine(sw.ElapsedMilliseconds);
            return result.ToJson();
        }

        #endregion

        #region 水公园储物柜
        /// <summary>
        /// 水公园储物柜
        /// </summary>
        /// <returns></returns>
        public ActionResult ParkStoreStatus()
        {

            return View();
        }


        //ParkStore
        /// <summary>
        /// 定时查询 水公园储物柜
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetStatusParkList()
        {
            
            var result = ActionJsonResult.Create();
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;


            using (var db = DbHelper.Create())
            {


                if (tbuser.Level > 1)
                {
                    var list = db.V_FP_Status_ParkStore.ToList();
                    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                    var psr = new PageSearchResult<List<V_FP_Status_ParkStore>>(list.Count, list);
                    result.Data = list;
                }
                else
                {
                    var list = db.V_Status_ParkStore.ToList();
                    var psr = new PageSearchResult<List<V_Status_ParkStore>>(list.Count, list);
                    result.Data = list;
                }



            }


            return result.ToJson();
        }
        /// <summary>
        /// 定时查询 水公园储物柜(没有物理存在)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetStatusParkNullList()
        {

            var result = ActionJsonResult.Create();
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;

            int map = 0;
            if (!string.IsNullOrEmpty(Request["map"]))
            {
                map = Convert.ToInt32(Request["map"]);
            }
            using (var db = DbHelper.Create())
            {


                if (tbuser.Level > 1)
                {
                    var list = db.V_FP_Status_ParkStoreNull.ToList();
                    if (map == 0) {
                        list = db.V_FP_Status_ParkStoreNull.Where(a => a.LastUpdate == null).ToList();
                    }
                    
                    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                    var psr = new PageSearchResult<List<V_FP_Status_ParkStoreNull>>(list.Count, list);
                    result.Data = list;
                }
                else
                {
                    var list = db.V_Status_ParkStoreNull.ToList();
                    if (map == 0) {
                        list = db.V_Status_ParkStoreNull.Where(a => a.LastUpdate == null).ToList();
                    }
                    
                    var psr = new PageSearchResult<List<V_Status_ParkStoreNull>>(list.Count, list);
                    result.Data = list;
                }



            }


            return result.ToJson();
        }

        #endregion

        #region 自助储物柜
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SelfCabinet()
        {
            return View();
        }



        /// <summary>
        /// 定时查询 自助储物柜
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetStatusSelfCabinetList()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            //var result = ActionJsonResult.Create();

            //using (var db = DbHelper.Create())
            //{
            //    //todo 多条件查询
            //    var sql = new StringBuilder("select  D.DeviceID, D.DeviceName,S.DeviceSN, S.LastUpdate, S.IPAddress, S.DoorStatus, S.Config, D.StartDate, D.EndDate, "+
            //            "D.Enabled, D.Lat, D.Lng from Tb_Status_SelfCabinet S inner "+
            //            "join Tb_Device_SelfCabinet D on D.DeviceSN = S.DeviceSN");

            //    //var users = db.Tb_Users.Where(u => (!string.IsNullOrEmpty(param.X) && u.UserName == param.X) || (param.Y != -1 &&  u.Level == param.Y)).ToList();

            //    var list = db.Database.SqlQuery<SelfCabinet>(sql.ToString()).ToList();
            //    var psr = new PageSearchResult<List<SelfCabinet>>(list.Count, list);
            //    result.Data = list;
            //}

            //sw.Stop();
            // Debug.WriteLine(sw.ElapsedMilliseconds);


            var result = ActionJsonResult.Create();
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;


            using (var db = DbHelper.Create())
            {


                if (tbuser.Level > 1)
                {
                    var list = db.V_FP_Status_SelfCabinet.ToList();
                    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                    var psr = new PageSearchResult<List<V_FP_Status_SelfCabinet>>(list.Count, list);
                    result.Data = list;
                }
                else
                {
                    var list = db.V_Status_SelfCabinet.ToList();
                    var psr = new PageSearchResult<List<V_Status_SelfCabinet>>(list.Count, list);
                    result.Data = list;
                }



            }
            return result.ToJson();
        }
        /// <summary>
        /// 定时查询 自助储物柜(没有物理存在)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetStatusSelfCabinetNullList()
        {
           
            var result = ActionJsonResult.Create();
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;
            int map = 0;
            if (!string.IsNullOrEmpty(Request["UserId"]))
            {
                map = Convert.ToInt32(Request["map"]);
            }

            using (var db = DbHelper.Create())
            {


                if (tbuser.Level > 1)
                {
                    var list = db.V_FP_Status_SelfCabinetNull.ToList();
                    if (map==0) {
                        list = db.V_FP_Status_SelfCabinetNull.Where(a => a.LastUpdate == null).ToList();
                    }
                    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                    var psr = new PageSearchResult<List<V_FP_Status_SelfCabinetNull>>(list.Count, list);
                    result.Data = list;
                }
                else
                {
                    var list = db.V_Status_SelfCabinetNull.Where(a => a.LastUpdate == null).ToList();
                    var psr = new PageSearchResult<List<V_Status_SelfCabinetNull>>(list.Count, list);
                    result.Data = list;
                }



            }
            return result.ToJson();
        }
        #endregion

        #region 海康DVR
        /// <summary>
        /// 海康DVR
        /// </summary>
        /// <returns></returns>
        public ActionResult DVRStatus()
        {
            return View();
        }
        /// <summary>
        /// 定时查询 海康DVR
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetStatusDVRList()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            //var result = ActionJsonResult.Create();

            //using (var db = DbHelper.Create())
            //{
            //    //todo 多条件查询
            //    var sql = new StringBuilder("select S.DeviceID, S.LastUpdate, S.IPAddress,D.DeviceName, D.StartDate, D.EndDate, D.Enabled, D.Port, D.Account, D.PWD, D.ChannelID, D.Remarks, D.Lat, D.Lng from "+
            //                                "Tb_Status_DVR S inner join Tb_Device_DVR D on S.DeviceID = D.DeviceID");

            //    //var users = db.Tb_Users.Where(u => (!string.IsNullOrEmpty(param.X) && u.UserName == param.X) || (param.Y != -1 &&  u.Level == param.Y)).ToList();

            //    var list = db.Database.SqlQuery<DVR>(sql.ToString()).ToList();
            //    var psr = new PageSearchResult<List<DVR>>(list.Count, list);
            //    result.Data = list;
            //}

            //sw.Stop();
            // Debug.WriteLine(sw.ElapsedMilliseconds);

            var result = ActionJsonResult.Create();
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;


            using (var db = DbHelper.Create())
            {


                if (tbuser.Level > 1)
                {
                    var list = db.V_FP_Status_DVR.ToList();
                    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                    var psr = new PageSearchResult<List<V_FP_Status_DVR>>(list.Count, list);
                    result.Data = list;
                }
                else
                {
                    var list = db.V_Status_DVR.ToList();
                    var psr = new PageSearchResult<List<V_Status_DVR>>(list.Count, list);
                    result.Data = list;
                }



            }

            return result.ToJson();
        }



        /// <summary>
        /// 定时查询 海康DVR(没有物理存在)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetStatusDVRNullList()
        {
           
            var result = ActionJsonResult.Create();
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;
            int map = 0;
            if (!string.IsNullOrEmpty(Request["UserId"]))
            {
                map = Convert.ToInt32(Request["map"]);
            }

            using (var db = DbHelper.Create())
            {


                if (tbuser.Level > 1)
                {
                    var list = db.V_FP_Status_DVRNull.ToList();
                    if (map == 0) {
                        list = db.V_FP_Status_DVRNull.Where(a => a.LastUpdate == null).ToList();
                    }
                    
                    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                    var psr = new PageSearchResult<List<V_FP_Status_DVRNull>>(list.Count, list);
                    result.Data = list;
                }
                else
                {
                    var list = db.V_Status_DVRNull.ToList();
                    if (map == 0) {
                        list = db.V_Status_DVRNull.Where(a => a.LastUpdate == null).ToList();
                    }
                    var psr = new PageSearchResult<List<V_Status_DVRNull>>(list.Count, list);
                    result.Data = list;
                }



            }

            return result.ToJson();
        }


        #endregion

        #region LED

        /// <summary>
        /// LED
        /// </summary>
        /// <returns></returns>
        public ActionResult LEDStatus()
        {
            return View();
        }
        /// <summary>
        /// 定时查询 LED
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetStatusLEDList()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            //var result = ActionJsonResult.Create();

            //using (var db = DbHelper.Create())
            //{
            //    //todo 多条件查询
            //    var sql = new StringBuilder("select S.DeviceID, S.LastUpdate, S.IPAddress,D.DeviceName, D.StartDate, D.EndDate, D.Enabled, D.Port, D.Lat, D.Lng from "+
            //                                "Tb_Device_LED D inner join Tb_Status_Led S on D.DeviceID = S.DeviceID");

            //    //var users = db.Tb_Users.Where(u => (!string.IsNullOrEmpty(param.X) && u.UserName == param.X) || (param.Y != -1 &&  u.Level == param.Y)).ToList();

            //    var list = db.Database.SqlQuery<LED>(sql.ToString()).ToList();
            //    var psr = new PageSearchResult<List<LED>>(list.Count, list);
            //    result.Data = list;
            //}

            //sw.Stop();
            // Debug.WriteLine(sw.ElapsedMilliseconds);

            var result = ActionJsonResult.Create();
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;


            using (var db = DbHelper.Create())
            {


                if (tbuser.Level > 1)
                {
                    var list = db.V_FP_Status_LED.ToList();
                    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                    var psr = new PageSearchResult<List<V_FP_Status_LED>>(list.Count, list);
                    result.Data = list;
                }
                else
                {
                    var list = db.V_Status_LED.ToList();
                    var psr = new PageSearchResult<List<V_Status_LED>>(list.Count, list);
                    result.Data = list;
                }



            }

            return result.ToJson();
        }
        /// <summary>
        /// 定时查询 LED(没有物理存在)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetStatusLEDNullList()
        {
          
            var result = ActionJsonResult.Create();
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;
            int map = 0;
            if (!string.IsNullOrEmpty(Request["UserId"]))
            {
                map = Convert.ToInt32(Request["map"]);
            }

            using (var db = DbHelper.Create())
            {


                if (tbuser.Level > 1)
                {
                    var list = db.V_FP_Status_LEDNull.ToList();
                    if (map==0) {
                        list = db.V_FP_Status_LEDNull.Where(a => a.LastUpdate == null).ToList();
                    }
                    
                    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                    var psr = new PageSearchResult<List<V_FP_Status_LEDNull>>(list.Count, list);
                    result.Data = list;
                }
                else
                {
                    var list = db.V_Status_LEDNull.ToList();

                    if (map==0) {
                        list = db.V_Status_LEDNull.Where(a => a.LastUpdate == null).ToList();

                    }
                    var psr = new PageSearchResult<List<V_Status_LEDNull>>(list.Count, list);
                    result.Data = list;
                }



            }

            return result.ToJson();
        }


        #endregion

        #region 客流计
        /// <summary>
        /// 客流计
        /// </summary>
        /// <returns></returns>
        public ActionResult eSensorStatus()
        {
            return View();
        }

        /// <summary>
        /// 定时查询 客流计
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetStatuseSensorList()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            //var result = ActionJsonResult.Create();

            //using (var db = DbHelper.Create())
            //{
            //    //todo 多条件查询
            //    var sql = new StringBuilder("select D.DeviceSN, D.DeviceName, D.StartDate, D.EndDate, D.Enabled, D.Port, D.Lat, D.Lng, "+
            //                                "S.DeviceID, S.LastUpdate, S.IPAddress from Tb_Device_eSensor D inner "+
            //                                "join Tb_Status_eSensor S on D.DeviceID = S.DeviceID");

            //    //var users = db.Tb_Users.Where(u => (!string.IsNullOrEmpty(param.X) && u.UserName == param.X) || (param.Y != -1 &&  u.Level == param.Y)).ToList();

            //    var list = db.Database.SqlQuery<eSensor>(sql.ToString()).ToList();
            //    var psr = new PageSearchResult<List<eSensor>>(list.Count, list);
            //    result.Data = list;
            //}

            //sw.Stop();
            // Debug.WriteLine(sw.ElapsedMilliseconds);


            var result = ActionJsonResult.Create();
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;


            using (var db = DbHelper.Create())
            {


                //if (tbuser.Level > 1)
                //{
                //    var list = db.V_FP_Status_eSensor.ToList();
                //    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                //    var psr = new PageSearchResult<List<V_FP_Status_eSensor>>(list.Count, list);
                //    result.Data = list;
                //}
                //else
                //{
                //    var list = db.V_Status_ESensor.ToList();
                //    var psr = new PageSearchResult<List<V_Status_ESensor>>(list.Count, list);
                //    result.Data = list;
                //}



            }
            return result.ToJson();
        }
        /// <summary>
        /// 定时查询 客流计(没有物理存在)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetStatuseSensorNullList()
        {

            var result = ActionJsonResult.Create();
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;
            int map = 0;
            if (!string.IsNullOrEmpty(Request["UserId"]))
            {
                map = Convert.ToInt32(Request["map"]);
            }

            using (var db = DbHelper.Create())
            {


                //if (tbuser.Level > 1)
                //{
                //    var list = db.V_FP_Status_ESensorNull.ToList();
                //    if (map==0) {
                //        list = db.V_FP_Status_ESensorNull.Where(a => a.LastUpdate == null).ToList();
                //    }
                    
                //    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                //    var psr = new PageSearchResult<List<V_FP_Status_ESensorNull>>(list.Count, list);
                //    result.Data = list;
                //}
                //else
                //{
                //    var list = db.V_Status_ESonsorNull.ToList();
                //    if (map==0) {
                //        list = db.V_Status_ESonsorNull.Where(a => a.LastUpdate == null).ToList();
                //    }
                    
                //    var psr = new PageSearchResult<List<V_Status_ESonsorNull>>(list.Count, list);
                //    result.Data = list;
                //}



            }
            return result.ToJson();
        }






        #endregion

        #endregion

        #region  设备交互
        /// <summary>
        /// 检票闸机
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult EquipInteraction()
        {
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;
            using (var db = DbHelper.Create())
            {
                if (tbuser.Level > 1)
                {
                    ViewBag.DeviceName = db.V_FP_Ticket.Where(a => a.UserID == tbuser.UserID).ToList();
                }
                else
                {
                    ViewBag.DeviceName = db.Tb_Device_Ticket.OrderByDescending(a => a.DeviceID).ToList();
                }


            }

            return View();
        }


        /// <summary>
        /// 分页查询 检票闸机
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetLogsTicketList()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            var deviceName = Request["DeviceName"];
            //起始日期
            var starts = Request["starts"];
            //结束日期
            var end = Request["end"];

            var o = new object();
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;

            var result = ActionJsonResult.Create();
            using (var db = DbHelper.Create())
            {

                //var sql = new StringBuilder("select * from V_Logs_Ticket u where 1=1 ");
                //if (!string.IsNullOrEmpty(deviceName))
                //{
                //    sql.AppendFormat(" and u.DeviceName = '{0}'", deviceName);

                //}
                var list = (IEnumerable<V_FP_Logs_Ticket>)db.V_FP_Logs_Ticket;
                if (!string.IsNullOrEmpty(deviceName))
                {
                    list = list.Where(a => a.DeviceName.Contains(deviceName));

                }
                if (starts != "" && end != "")
                {
                    list = list.Where(m => m.SaveDate >= Convert.ToDateTime(starts+ " 00:00:00" )&& m.SaveDate <= Convert.ToDateTime((end +" 23:59:59")));
                }
                else if (starts != "")
                {
                    list = list.Where(m => m.SaveDate >= Convert.ToDateTime(starts + " 00:00:00"));
                }
                else if (end != "")
                {
                    list = list.Where(m => m.SaveDate <= Convert.ToDateTime((end + " 23:59:59")));
                }
                if (tbuser.Level > 1)
                {
                    list = list.Where(a => a.UserID == tbuser.UserID);
                }
                o = new
                {
                    draw = draw,
                    recordsTotal = list.Count(),
                    recordsFiltered = list.Count(),
                    data = list.Skip(start).Take(rows).ToList()
                };



            }
            return Json(o, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// M4检票闸机
        /// </summary>
        /// <returns></returns>
        public ActionResult M4ChannelLogs()
        {

            using (var db = DbHelper.Create())
            {
                ViewBag.User = Session["Web.EF.Tb_Users"];
                Tb_Users tbuser = ViewBag.User;
                if (tbuser.Level > 1)
                {
                    ViewBag.DeviceName = db.V_M4Chanel_FP.Where(a => a.UserID == tbuser.UserID).ToList();
                }
                else
                {
                    ViewBag.DeviceName = db.Tb_Device_M4Channel.ToList();
                }

                //ViewBag.list = db.V_Logs_Ticket.OrderBy(a => a.SaveDate).Skip(0).Take(10).Select(s => s).ToList();
                //ViewBag.Count = db.V_Logs_Ticket.ToList().Count / 10;
            }

            return View();
        }


        /// <summary>
        /// 分页查询 M4检票闸机
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetM4ChannelLogsList()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            var deviceName = Request["DeviceName"];
            //起始日期
            var starts = Request["starts"];
            //结束日期
            var end = Request["end"];
            var o = new object();
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;
            using (var db = DbHelper.Create())
            {
                //var result = (IEnumerable<Tb_Device_M4Channel>)db.Tb_Device_M4Channel;

                var result = ActionJsonResult.Create();

                var list = (IEnumerable<V_FP_Logs_M4Channel>)db.V_FP_Logs_M4Channel;
                if (!string.IsNullOrEmpty(deviceName))
                {
                    list = list.Where(a => a.DeviceName.Contains(deviceName));

                }
                if (starts != "" && end != "")
                {
                    list = list.Where(m => m.SaveDate >= Convert.ToDateTime(starts + " 00:00:00") && m.SaveDate <= Convert.ToDateTime((end + " 23:59:59")));
                }
                else if (starts != "")
                {
                    list = list.Where(m => m.SaveDate >= Convert.ToDateTime(starts+" 00:00:00"));
                }
                else if (end != "")
                {
                    list = list.Where(m => m.SaveDate <= Convert.ToDateTime((end+" 23:59:59")));
                }
                if (tbuser.Level > 1)
                {
                    list = list.Where(a => a.UserID == tbuser.UserID);
                }

                o = new
                {
                    draw = draw,
                    recordsTotal = list.Count(),
                    recordsFiltered = list.Count(),
                    data = list.Skip(start).Take(rows).ToList()
                };
            }
            return Json(o, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 自助储物柜
        /// </summary>
        /// <returns></returns>
        public ActionResult SelfCabinetLogs()
        {
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;
            using (var db = DbHelper.Create())
            {
                if (tbuser.Level > 1)
                {
                    ViewBag.DeviceName = db.V_FP_SelfCabinet.Where(a => a.UserID == tbuser.UserID).ToList();
                }
                else
                {
                    ViewBag.DeviceName = db.Tb_Device_SelfCabinet.ToList();
                }

                //ViewBag.list = db.V_Logs_Ticket.OrderBy(a => a.SaveDate).Skip(0).Take(10).Select(s => s).ToList();
                //ViewBag.Count = db.V_Logs_Ticket.ToList().Count / 10;
            }

            return View();
        }


        /// <summary>
        /// 分页查询 自助储物柜
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetSelfCabinetLogsList()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            var deviceName = Request["DeviceName"];
            //起始日期
            var starts = Request["starts"];
            //结束日期
            var end = Request["end"];
            var o = new object();
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;

            var result = ActionJsonResult.Create();
            using (var db = DbHelper.Create())
            {
                //var sql = new StringBuilder("select L.ID, L.DeviceKey, L.FunctionID, L.FunctionName, L.ElapsedTime, L.SaveDate, L.Request, L.Response, L.Result, D.DeviceID, D.DeviceName " +
                //            "from Tb_Logs_M4Channel L inner " +
                //            "join Tb_Device_M4Channel D on L.DeviceKey = D.DeviceSN ");
                ////todo 多条件查询
                //if (!string.IsNullOrEmpty(deviceName))
                //{
                //    sql.AppendFormat(" and D.DeviceName = '{0}'", deviceName);

                //}
                var list = (IEnumerable<V_FP_Logs_M4Channel>)db.V_FP_Logs_M4Channel;
                if (!string.IsNullOrEmpty(deviceName))
                {
                    list = list.Where(a => a.DeviceName.Contains(deviceName));

                }
                if (starts != "" && end != "")
                {
                    list = list.Where(m => m.SaveDate >= Convert.ToDateTime(starts+" 00:00:00") && m.SaveDate <= Convert.ToDateTime((end+" 23:59:59")));
                }
                else if (starts != "")
                {
                    list = list.Where(m => m.SaveDate >= Convert.ToDateTime(starts + " 00:00:00"));
                }
                else if (end != "")
                {
                    list = list.Where(m => m.SaveDate <= Convert.ToDateTime((end + " 23:59:59")));
                }
                if (tbuser.Level > 1)
                {
                    list = list.Where(a => a.UserID == tbuser.UserID);
                }
                o = new
                {
                    draw = draw,
                    recordsTotal = list.Count(),
                    recordsFiltered = list.Count(),
                    data = list.Skip(start).Take(rows).ToList()
                };
            }
            return Json(o, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// TVM
        /// </summary>
        /// <returns></returns>
        public ActionResult TVMLogs()
        {

            using (var db = DbHelper.Create())
            {
                ViewBag.User = Session["Web.EF.Tb_Users"];
                Tb_Users tbuser = ViewBag.User;
                if (tbuser.Level > 1)
                {
                    ViewBag.DeviceName = db.V_TVM_FP.Where(a => a.UserID == tbuser.UserID).ToList();
                }
                else
                {
                    ViewBag.DeviceName = db.Tb_Device_TVM.ToList();
                }

                //ViewBag.list = db.V_Logs_Ticket.OrderBy(a => a.SaveDate).Skip(0).Take(10).Select(s => s).ToList();
                //ViewBag.Count = db.V_Logs_Ticket.ToList().Count / 10;
            }

            return View();
        }


        /// <summary>
        /// 分页查询 TVM
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetTVMLogsList()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            var deviceName = Request["DeviceName"];
            //起始日期
            var starts = Request["starts"];
            //结束日期
            var end = Request["end"];
            var o = new object();
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;
            using (var db = DbHelper.Create())
            {
                var result = ActionJsonResult.Create();

                var list = (IEnumerable<V_FP_Logs_TVM>)db.V_FP_Logs_TVM;
                if (!string.IsNullOrEmpty(deviceName))
                {
                    list = list.Where(a => a.DeviceName.Contains(deviceName) );

                }
                if (starts != "" && end != "")
                {
                    list = list.Where(m => m.SaveDate >= Convert.ToDateTime(starts +" 00:00:00") && m.SaveDate <= Convert.ToDateTime((end+" 23:59:59")));
                }
                else if (starts != "")
                {
                    list = list.Where(m => m.SaveDate >= Convert.ToDateTime(starts + " 00:00:00"));
                }
                else if (end != "")
                {
                    list = list.Where(m => m.SaveDate <= Convert.ToDateTime((end + " 23:59:59")));
                }
                if (tbuser.Level > 1)
                {
                    list = list.Where(a => a.UserID == tbuser.UserID);
                }

                o = new
                {
                    draw = draw,
                    recordsTotal = list.Count(),
                    recordsFiltered = list.Count(),
                    data = list.Skip(start).Take(rows).ToList()
                };
            }
            return Json(o, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 水公园交互
        /// </summary>
        /// <returns></returns>
        public ActionResult ParkStoreLogs()
        {

            using (var db = DbHelper.Create())
            {
                ViewBag.User = Session["Web.EF.Tb_Users"];
                Tb_Users tbuser = ViewBag.User;
                if (tbuser.Level > 1)
                {
                    ViewBag.DeviceName = db.V_FP_ParkStore.Where(a => a.UserID == tbuser.UserID).ToList();
                }
                else
                {
                    ViewBag.DeviceName = db.Tb_Device_ParkStore.ToList();
                }

                //ViewBag.list = db.V_Logs_Ticket.OrderBy(a => a.SaveDate).Skip(0).Take(10).Select(s => s).ToList();
                //ViewBag.Count = db.V_Logs_Ticket.ToList().Count / 10;
            }

            return View();
        }


        /// <summary>
        /// 分页查询 水公园交互
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetParkStoreLogsList()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            var deviceName = Request["DeviceName"];
            //起始日期
            var starts = Request["starts"];
            //结束日期
            var end = Request["end"];
            var o = new object();
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;

            var result = ActionJsonResult.Create();
            using (var db = DbHelper.Create())
            {

                //var sql = new StringBuilder("select L.ID, L.DeviceKey, L.FunctionID, L.FunctionName, L.ElapsedTime, L.SaveDate, L.Request, L.Response, L.Result, D.DeviceID, D.DeviceName " +
                //                            "from Tb_Logs_ParkStore L inner " +
                //                            "join Tb_Device_ParkStore D on L.DeviceKey = D.DeviceSN");
                ////todo 多条件查询
                //if (!string.IsNullOrEmpty(deviceName))
                //{
                //    sql.AppendFormat(" and D.DeviceName = '{0}'", deviceName);
                //}
                var list =(IEnumerable< V_FP_Logs_ParkStore>)db.V_FP_Logs_ParkStore;

                if (!string.IsNullOrEmpty(deviceName))
                {
                    list = list.Where(a => a.DeviceName.Contains(deviceName));

                }
                if (starts != "" && end != "")
                {
                    list = list.Where(m => m.SaveDate >= Convert.ToDateTime(starts+" 00:00:00") && m.SaveDate <= Convert.ToDateTime((end +" 23:59:59")));
                }
                else if (starts != "")
                {
                    list = list.Where(m => m.SaveDate >= Convert.ToDateTime(starts + " 00:00:00"));
                }
                else if (end != "")
                {
                    list = list.Where(m => m.SaveDate <= Convert.ToDateTime((end + " 23:59:59")));
                }
                if (tbuser.Level > 1)
                {
                    list = list.Where(a => a.UserID == tbuser.UserID);
                }

                o = new
                {
                    draw = draw,
                    recordsTotal = list.Count(),
                    recordsFiltered = list.Count(),
                    data = list.Skip(start).Take(rows).ToList()
                };
            }
            return Json(o, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// LED显示屏调用日志
        /// </summary>
        /// <returns></returns>
        public ActionResult LEDLogs()
        {

            using (var db = DbHelper.Create())
            {
                ViewBag.User = Session["Web.EF.Tb_Users"];
                Tb_Users tbuser = ViewBag.User;
                if (tbuser.Level > 1)
                {
                    ViewBag.DeviceName = db.V_FP_LED.Where(a => a.UserID == tbuser.UserID).ToList();
                }
                else
                {
                    ViewBag.DeviceName = db.Tb_Device_LED.ToList();
                }

                //ViewBag.list = db.V_Logs_Ticket.OrderBy(a => a.SaveDate).Skip(0).Take(10).Select(s => s).ToList();
                //ViewBag.Count = db.V_Logs_Ticket.ToList().Count / 10;
            }

            return View();
        }


        /// <summary>
        /// 分页查询 LED显示屏调用日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetLEDLogsList()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            var deviceName = Request["DeviceName"];
            var o = new object();
            using (var db = DbHelper.Create())
            {
                var result = ActionJsonResult.Create();



                var sql = new StringBuilder("select D.DeviceName, D.IPAddress, D.Port, D.Lat, D.Lng,L.ID, L.UserID, L.DeviceID, L.FunctionName, L.Request, L.Response, L.Result, L.SaveDate " +
                                            "from Tb_Logs_LedCall L inner " +
                                            "join Tb_Device_LED D on L.DeviceID = D.DeviceID");
                //todo 多条件查询
                if (!string.IsNullOrEmpty(deviceName))
                {
                    sql.AppendFormat(" and D.DeviceName LIKE '%{0}%'", deviceName);

                }
                var list = db.Database.SqlQuery<LEDLogs>(sql.ToString()).ToList();
                var recordsTotal = list.Count();
                //var psr = new PageSearchResult<List<DeviceLogs>>(list.Count, list);
                var data = list.Skip(start).Take(rows).ToList(); o = new
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
        /// 客流计调用日志
        /// </summary>
        /// <returns></returns>
        public ActionResult eSensorLogs()
        {

            using (var db = DbHelper.Create())
            {
                ViewBag.User = Session["Web.EF.Tb_Users"];
                Tb_Users tbuser = ViewBag.User;
                if (tbuser.Level > 1)
                {
                    ViewBag.DeviceName = db.V_FP_eSensor.Where(a => a.UserID == tbuser.UserID).ToList();
                }
                else
                {
                    ViewBag.DeviceName = db.Tb_Device_eSensor.ToList();
                }

                //ViewBag.list = db.V_Logs_Ticket.OrderBy(a => a.SaveDate).Skip(0).Take(10).Select(s => s).ToList();
                //ViewBag.Count = db.V_Logs_Ticket.ToList().Count / 10;
            }

            return View();
        }


        /// <summary>
        /// 分页查询 LED显示屏调用日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GeteSensorLogsList()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            var deviceName = Request["DeviceName"];
            var o = new object();
            using (var db = DbHelper.Create())
            {
                var result = ActionJsonResult.Create();



                var sql = new StringBuilder("select D.DeviceSN, D.DeviceName, D.IPAddress, D.Port, D.Lat, D.Lng,L.ID, L.DeviceID, L.EventID, L.In_Count, L.Out_Count, L.SaveDate " +
                                            "from Tb_Logs_eSensor L inner " +
                                            "join Tb_Device_eSensor D on L.DeviceID = D.DeviceID");
                //todo 多条件查询
                if (!string.IsNullOrEmpty(deviceName))
                {
                    sql.AppendFormat(" and D.DeviceName LIKE '%{0}%'", deviceName);

                }
                var list = db.Database.SqlQuery<eSensorLogs>(sql.ToString()).ToList();
                var recordsTotal = list.Count();
                //var psr = new PageSearchResult<List<DeviceLogs>>(list.Count, list);
                var data = list.Skip(start).Take(rows).ToList(); o = new
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

        #region 人脸检测接口交互
        /// <summary>
        /// 人脸检测接口交互
        /// </summary>
        /// <returns></returns>
        public ActionResult Interaction()
        {

            using (var db = DbHelper.Create())
            {
                //ViewBag.list = db.V_Logs_Ticket.OrderBy(a => a.SaveDate).Skip(0).Take(10).Select(s => s).ToList();
                //ViewBag.Count = db.V_Logs_Ticket.ToList().Count / 10;
            }

            return View();
        }
        /// <summary>
        /// 支付接口交互
        /// </summary>
        /// <returns></returns>
        public ActionResult InteractionPay()
        {

            using (var db = DbHelper.Create())
            {
                //ViewBag.list = db.V_Logs_Ticket.OrderBy(a => a.SaveDate).Skip(0).Take(10).Select(s => s).ToList();
                //ViewBag.Count = db.V_Logs_Ticket.ToList().Count / 10;
            }

            return View();
        }



        /// <summary>
        /// 分页查询 人脸API调用日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetFaceAPILogsList()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            var UserName = Request["UserName"];
            //起始日期
            var starts = Request["starts"];
            //结束日期
            var end = Request["end"];
            var o = new object();

            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;


            using (var db = DbHelper.Create())
            {
                var result = ActionJsonResult.Create();

                //var sql = new StringBuilder("select L.ID, L.API_ID, L.UserID, L.FunctionName, L.Request, L.Response, L.Result, L.UseTime, L.CallTime,U.UserName,A.ProviderUrl " +
                //                            "from Tb_Logs_FaceAPI L inner " +
                //                            "join Tb_Users U on L.UserID = U.UserID inner " +
                //                            "join Tb_APIFunction A on L.API_ID = A.API_ID");
                ////todo 多条件查询
                //if (!string.IsNullOrEmpty(deviceName))
                //{
                //    sql.AppendFormat(" and D.DeviceName = '{0}'", deviceName);

                //}

                //var list = db.Database.SqlQuery<FaceAPILogs>(sql.ToString()).ToList();
                //var recordsTotal = list.Count();
                ////var psr = new PageSearchResult<List<DeviceLogs>>(list.Count, list);
                //var data = list.Skip(start).Take(rows).ToList(); o = new
                //{
                //    draw = draw,
                //    recordsTotal = recordsTotal,
                //    recordsFiltered = recordsTotal,
                //    data = data
                //};
                var list =(IEnumerable<V_Logs_FaceAPI>)db.V_Logs_FaceAPI;
                if (!string.IsNullOrEmpty(UserName))
                {
                    list = list.Where(a => a.UserName.Contains(UserName) );

                }
                if (starts != "" && end != "")
                {
                    list = list.Where(m => m.CallTime >= Convert.ToDateTime(starts + " 00:00:00") && m.CallTime <= Convert.ToDateTime((end + " 23:59:59")));
                }
                else if (starts != "")
                {
                    list = list.Where(m => m.CallTime >= Convert.ToDateTime(starts + " 00:00:00"));
                }
                else if (end != "")
                {
                    list = list.Where(m => m.CallTime <= Convert.ToDateTime((end + " 23:59:59")));
                }
                if (tbuser.Level > 1) 
                {
                    list = list.Where(a => a.UserID == tbuser.UserID);
                   
                }
                o = new
                {
                    draw = draw,
                    recordsTotal = list.Count(),
                    recordsFiltered = list.Count(),
                    data = list.Skip(start).Take(rows).ToList()
                };
            }
            return Json(o, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 分页查询 支付API调用日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetPayAPILogsList()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            var UserName = Request["UserName"];
            //起始日期
            var starts = Request["starts"];
            //结束日期
            var end = Request["end"];
            var o = new object();

            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;

            using (var db = DbHelper.Create())
            {
                var result = ActionJsonResult.Create();

                //var sql = new StringBuilder("select L.ID, L.API_ID, L.Customer, L.CustomerType, L.Request, L.Response, L.Result, L.UseTime, L.CallTime,U.UserName,A.APIName,A.ProviderUrl "+
                //                            "from Tb_Logs_DocomPay L inner "+
                //                            "join Tb_Users U on L.Customer = U.UserID inner "+
                //                            "join Tb_APIFunction A on L.API_ID = A.API_ID");
                ////todo 多条件查询
                //if (!string.IsNullOrEmpty(deviceName))
                //{
                //    sql.AppendFormat(" and D.DeviceName = '{0}'", deviceName);

                //}
                //var list = db.Database.SqlQuery<PayAPILogs>(sql.ToString()).ToList();
                //var recordsTotal = list.Count();
                ////var psr = new PageSearchResult<List<DeviceLogs>>(list.Count, list);
                //var data = list.Skip(start).Take(rows).ToList(); o = new
                //{
                //    draw = draw,
                //    recordsTotal = recordsTotal,
                //    recordsFiltered = recordsTotal,
                //    data = data
                //};
                var list =(IEnumerable<V_Logs_DocomPay>)db.V_Logs_DocomPay;
                if (!string.IsNullOrEmpty(UserName))
                {
                    list = list.Where(a => a.UserName.Contains(UserName));

                }
                if (starts != "" && end != "")
                {
                    list = list.Where(m => m.CallTime >= Convert.ToDateTime(starts+" 00:00:00") && m.CallTime <= Convert.ToDateTime((end+" 23:59:59")));
                }
                else if (starts != "")
                {
                    list = list.Where(m => m.CallTime >= Convert.ToDateTime(starts + " 00:00:00"));
                }
                else if (end != "")
                {
                    list = list.Where(m => m.CallTime <= Convert.ToDateTime((end + " 23:59:59")));
                }
                if (tbuser.Level > 1)
                {
                    //list = list.Where(a => a.UserID == tbuser.UserID);
                }
                o = new
                {
                    draw = draw,
                    recordsTotal = list.Count(),
                    recordsFiltered = list.Count(),
                    data = list.Skip(start).Take(rows).ToList()
                };
            }

            return Json(o, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 地图
        /// <summary>
        /// M4出口闸机
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult ShowMap()
        {

            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;
            using (var db = DbHelper.Create())
            {
                if (tbuser.Level > 1)
                {
                    var list = db.V_FP_Status_M4Channel.ToList();
                    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                    if (list.Count > 0)
                    {
                        ViewBag.lat = list[0].Lat;
                        ViewBag.lng = list[0].Lng;
                    }
                }
                else
                {
                    var list = db.V_Status_M4Channel.ToList();
                    if (list.Count > 0)
                    {
                        ViewBag.lat = list[0].Lat;
                        ViewBag.lng = list[0].Lng;
                    }
                }

            }


            return View();
        }

        /// <summary>
        /// 安全检票闸机
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult ShowMapTicket()
        {
            //using (var db = DbHelper.Create())
            //{
            //    var list = db.V_Status_Ticket.ToList();
            //    if (list.Count > 0)
            //    {
            //        ViewBag.lat = list[0].Lat;
            //        ViewBag.lng = list[0].Lng;
            //    }
            //}
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;
            using (var db = DbHelper.Create())
            {
                if (tbuser.Level > 1)
                {
                    var list = db.V_FP_Status_Ticket.ToList();
                    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                    if (list.Count > 0)
                    {
                        ViewBag.lat = list[0].Lat;
                        ViewBag.lng = list[0].Lng;
                    }
                }
                else
                {
                    var list = db.V_Status_Ticket.ToList();
                    if (list.Count > 0)
                    {
                        ViewBag.lat = list[0].Lat;
                        ViewBag.lng = list[0].Lng;
                    }
                }

            }

            return View();
        }
        /// <summary>
        /// TVM
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult ShowMapTVM()
        {
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;
            using (var db = DbHelper.Create())
            {
                if (tbuser.Level > 1)
                {
                    var list = db.V_FP_Status_TVM.ToList();
                    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                    if (list.Count > 0)
                    {
                        ViewBag.lat = list[0].Lat;
                        ViewBag.lng = list[0].Lng;
                    }
                }
                else
                {
                    var list = db.V_Status_TVM.ToList();
                    if (list.Count > 0)
                    {
                        ViewBag.lat = list[0].Lat;
                        ViewBag.lng = list[0].Lng;
                    }
                }

            }


            return View();
        }
        /// <summary>
        /// 水公园储物柜
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult ShowMapParkSrore()
        {
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;
            using (var db = DbHelper.Create())
            {
                if (tbuser.Level > 1)
                {
                    var list = db.V_FP_Status_ParkStore.ToList();
                    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                    if (list.Count > 0)
                    {
                        ViewBag.lat = list[0].Lat;
                        ViewBag.lng = list[0].Lng;
                    }
                }
                else
                {
                    var list = db.V_Status_ParkStore.ToList();
                    if (list.Count > 0)
                    {
                        ViewBag.lat = list[0].Lat;
                        ViewBag.lng = list[0].Lng;
                    }
                }

            }


            return View();
        }
        /// <summary>
        /// 自助储物柜
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult ShowMapSelfCabinet()
        {
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;
            using (var db = DbHelper.Create())
            {
                if (tbuser.Level > 1)
                {
                    var list = db.V_FP_Status_SelfCabinet.ToList();
                    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                    if (list.Count > 0)
                    {
                        ViewBag.lat = list[0].Lat;
                        ViewBag.lng = list[0].Lng;
                    }
                }
                else
                {
                    var list = db.V_Status_SelfCabinet.ToList();
                    if (list.Count > 0)
                    {
                        ViewBag.lat = list[0].Lat;
                        ViewBag.lng = list[0].Lng;
                    }
                }

            }


            return View();
        }
        /// <summary>
        /// 海康DVR
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult ShowMapDVR()
        {
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;
            using (var db = DbHelper.Create())
            {
                if (tbuser.Level > 1)
                {
                    var list = db.V_FP_Status_DVR.ToList();
                    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                    if (list.Count > 0)
                    {
                        ViewBag.lat = list[0].Lat;
                        ViewBag.lng = list[0].Lng;
                    }
                }
                else
                {
                    var list = db.V_Status_DVR.ToList();
                    if (list.Count > 0)
                    {
                        ViewBag.lat = list[0].Lat;
                        ViewBag.lng = list[0].Lng;
                    }
                }

            }


            return View();
        }
        /// <summary>
        /// LED显示屏
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult ShowMapLED()
        {
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;
            using (var db = DbHelper.Create())
            {
                if (tbuser.Level > 1)
                {
                    var list = db.V_FP_Status_LED.ToList();
                    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                    if (list.Count > 0)
                    {
                        ViewBag.lat = list[0].Lat;
                        ViewBag.lng = list[0].Lng;
                    }
                }
                else
                {
                    var list = db.V_Status_LED.ToList();
                    if (list.Count > 0)
                    {
                        ViewBag.lat = list[0].Lat;
                        ViewBag.lng = list[0].Lng;
                    }
                }

            }


            return View();
        }
        /// <summary>
        /// 客流计
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult ShowMapEsensor()
        {
            ViewBag.User = Session["Web.EF.Tb_Users"];
            Tb_Users tbuser = ViewBag.User;
            using (var db = DbHelper.Create())
            {
                //if (tbuser.Level > 1)
                //{
                //    var list = db.V_FP_Status_eSensor.ToList();
                //    list = list.Where(a => a.UserID == tbuser.UserID).ToList();
                //    if (list.Count > 0)
                //    {
                //        ViewBag.lat = list[0].Lat;
                //        ViewBag.lng = list[0].Lng;
                //    }
                //}
                //else
                //{
                //    var list = db.V_Status_ESensor.ToList();
                //    if (list.Count > 0)
                //    {
                //        ViewBag.lat = list[0].Lat;
                //        ViewBag.lng = list[0].Lng;
                //    }
                //}

            }


            return View();
        }


        /// <summary>
        /// M4出口闸机详情页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CheckSession]
        public ActionResult M4ChannelDetail(int id)
        {
            using (var db = DbHelper.Create())
            {
                var list = db.V_Status_M4ChannelNull.Where(a => a.DeviceID == id).ToList();
                ViewBag.list = list;
            }

            return View();
        }
        [CheckSession]
        [HttpPost]
        public ActionResult GetM4ChannelDetail()
        {
            var result = ActionJsonResult.Create();
            using (var db = DbHelper.Create())
            {

                if (!string.IsNullOrEmpty(Request["id"]))
                {
                    int id = Convert.ToInt32(Request["id"]);
                    var list = db.V_Status_M4ChannelNull.Where(a => a.DeviceID == id).ToList();

                    result.Data = list;
                }
                else
                {
                    result.Failed("无法查询到指定的设备.");
                }



            }
            return result.ToJson();
        }

        /// <summary>
        /// 安全检票闸机详情页
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult TicketDetail(int id)
        {
            using (var db = DbHelper.Create())
            {
                var list = db.V_Status_TicketNull.Where(a => a.DeviceID == id).ToList();
                ViewBag.list = list;
            }

            return View();
        }

        [CheckSession]
        [HttpPost]
        public ActionResult GetTicketDetail()
        {
            var result = ActionJsonResult.Create();
            using (var db = DbHelper.Create())
            {

                if (!string.IsNullOrEmpty(Request["id"]))
                {
                    int id = Convert.ToInt32(Request["id"]);
                    var list = db.V_Status_TicketNull.Where(a => a.DeviceID == id).ToList();

                    result.Data = list;
                }
                else
                {
                    result.Failed("无法查询到指定的设备.");
                }



            }
            return result.ToJson();
        }

        /// <summary>
        /// TVM详情页
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult TVMDetail(int id)
        {
            using (var db = DbHelper.Create())
            {
                var list = db.V_Status_TVMNull.Where(a => a.DeviceID == id).ToList();
                ViewBag.list = list;
            }

            return View();
        }
        [CheckSession]
        [HttpPost]
        public ActionResult GetTVMDetail()
        {
            var result = ActionJsonResult.Create();
            using (var db = DbHelper.Create())
            {

                if (!string.IsNullOrEmpty(Request["id"]))
                {
                    int id = Convert.ToInt32(Request["id"]);
                    var list = db.V_Status_TVMNull.Where(a => a.DeviceID == id).ToList();

                    result.Data = list;
                }
                else
                {
                    result.Failed("无法查询到指定的设备.");
                }



            }
            return result.ToJson();
        }

        /// <summary>
        /// 水公园储物柜详情页
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult ParkStoreDetail(int id)
        {
            using (var db = DbHelper.Create())
            {
                var list = db.V_Status_ParkStoreNull.Where(a => a.DeviceID == id).ToList();
                ViewBag.list = list;
            }

            return View();
        }
        [CheckSession]
        [HttpPost]
        public ActionResult GetParkStoreDetail()
        {
            var result = ActionJsonResult.Create();
            using (var db = DbHelper.Create())
            {

                if (!string.IsNullOrEmpty(Request["id"]))
                {
                    int id = Convert.ToInt32(Request["id"]);
                    var list = db.V_Status_ParkStoreNull.Where(a => a.DeviceID == id).ToList();

                    result.Data = list;
                }
                else
                {
                    result.Failed("无法查询到指定的设备.");
                }



            }
            return result.ToJson();
        }
        /// <summary>
        /// 自助储物柜详情页
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult SelfCabinetDetail(int id)
        {
            using (var db = DbHelper.Create())
            {
                var list = db.V_Status_SelfCabinetNull.Where(a => a.DeviceID == id).ToList();
                ViewBag.list = list;
            }

            return View();
        }
        [CheckSession]
        [HttpPost]
        public ActionResult GetSelfCabinetDetail()
        {
            var result = ActionJsonResult.Create();
            using (var db = DbHelper.Create())
            {

                if (!string.IsNullOrEmpty(Request["id"]))
                {
                    int id = Convert.ToInt32(Request["id"]);
                    var list = db.V_Status_SelfCabinetNull.Where(a => a.DeviceID == id).ToList();

                    result.Data = list;
                }
                else
                {
                    result.Failed("无法查询到指定的设备.");
                }



            }
            return result.ToJson();
        }
        /// <summary>
        /// 海康DVR详情页
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult DVRDetail(int id)
        {
            using (var db = DbHelper.Create())
            {
                var list = db.V_Status_DVRNull.Where(a => a.DeviceID == id).ToList();
                ViewBag.list = list;
            }

            return View();
        }
        [CheckSession]
        [HttpPost]
        public ActionResult GetDVRDetail()
        {
            var result = ActionJsonResult.Create();
            using (var db = DbHelper.Create())
            {

                if (!string.IsNullOrEmpty(Request["id"]))
                {
                    int id = Convert.ToInt32(Request["id"]);
                    var list = db.V_Status_DVRNull.Where(a => a.DeviceID == id).ToList();

                    result.Data = list;
                }
                else
                {
                    result.Failed("无法查询到指定的设备.");
                }



            }
            return result.ToJson();
        }
        /// <summary>
        /// LED显示屏详情页
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult LEDDetail(int id)
        {
            using (var db = DbHelper.Create())
            {
                var list = db.V_Status_LEDNull.Where(a => a.DeviceID == id).ToList();
                ViewBag.list = list;
            }

            return View();
        }
        [CheckSession]
        [HttpPost]
        public ActionResult GetLEDDetail()
        {
            var result = ActionJsonResult.Create();
            using (var db = DbHelper.Create())
            {

                if (!string.IsNullOrEmpty(Request["id"]))
                {
                    int id = Convert.ToInt32(Request["id"]);
                    var list = db.V_Status_LEDNull.Where(a => a.DeviceID == id).ToList();

                    result.Data = list;
                }
                else
                {
                    result.Failed("无法查询到指定的设备.");
                }



            }
            return result.ToJson();
        }
        /// <summary>
        /// 客流计详情页
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult ESensorDetail(int id)
        {
            using (var db = DbHelper.Create())
            {
                //var list = db.V_Status_ESonsorNull.Where(a => a.DeviceID == id).ToList();
                //ViewBag.list = list;
            }

            return View();
        }
        [CheckSession]
        [HttpPost]
        public ActionResult GetESensorDetail()
        {
            var result = ActionJsonResult.Create();
            using (var db = DbHelper.Create())
            {

                if (!string.IsNullOrEmpty(Request["id"]))
                {
                    int id = Convert.ToInt32(Request["id"]);
                    //var list = db.V_Status_ESonsorNull.Where(a => a.DeviceID == id).ToList();

                    //result.Data = list;
                }
                else
                {
                    result.Failed("无法查询到指定的设备.");
                }



            }
            return result.ToJson();
        }



        #endregion





    }
}