
using DocomSDK.Ticket.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using Web.Commons;
using Web.EF;
using Web.Models.WebModels;

namespace Web.Controllers
{
    [WebAuth]
    public class DevManageController : Controller
    {
        #region Ticket
        /// <summary>
        /// 添加标注 lbh 1313216464646497
        /// </summary>SaveConfig
        /// <returns></returns>
        [CheckSession]
        public ActionResult AddMark(int id, string device)
        {
            switch (device)
            {
                case "Ticket":
                    using (var db = DbHelper.Create())
                    {
                        var result = db.Tb_Device_Ticket.FirstOrDefault(m => m.DeviceID == id);
                        if (result != null)
                        {
                            ViewBag.Lat = (decimal?)result.Lat;
                            ViewBag.Lng = (decimal?)result.Lng;

                        }
                    }
                    break;
                case "DVR":
                    using (var db = DbHelper.Create())
                    {
                        var dvr = db.Tb_Device_DVR.FirstOrDefault(m => m.DeviceID == id);
                        if (dvr != null)
                        {
                            ViewBag.Lat = (decimal?)dvr.Lat;
                            ViewBag.Lng = (decimal?)dvr.Lng;
                        }
                    }
                    break;
                case "eSensor":
                    using (var db = DbHelper.Create())
                    {
                        var eSensor = db.Tb_Device_eSensor.FirstOrDefault(m => m.DeviceID == id);
                        if (eSensor != null)
                        {
                            ViewBag.Lat = (decimal?)eSensor.Lat;
                            ViewBag.Lng = (decimal?)eSensor.Lng;
                        }
                    }
                    break;
                case "LED":
                    using (var db = DbHelper.Create())
                    {
                        var led = db.Tb_Device_LED.FirstOrDefault(m => m.DeviceID == id);
                        if (led != null)
                        {
                            ViewBag.Lat = (decimal?)led.Lat;
                            ViewBag.Lng = (decimal?)led.Lng;
                        }
                    }
                    break;
                case "M4Channel":
                    using (var db = DbHelper.Create())
                    {
                        var m4Channel = db.Tb_Device_M4Channel.FirstOrDefault(m => m.DeviceID == id);
                        if (m4Channel != null)
                        {
                            ViewBag.Lat = (decimal?)m4Channel.Lat;
                            ViewBag.Lng = (decimal?)m4Channel.Lng;
                        }
                    }
                    break;
                case "ParkStore":
                    using (var db = DbHelper.Create())
                    {
                        var parkStore = db.Tb_Device_ParkStore.FirstOrDefault(m => m.DeviceID == id);
                        if (parkStore != null)
                        {
                            ViewBag.Lat = (decimal?)parkStore.Lat;
                            ViewBag.Lng = (decimal?)parkStore.Lng;
                        }
                    }
                    break;
                case "SelfCabinet":
                    using (var db = DbHelper.Create())
                    {
                        var selfCabinet = db.Tb_Device_SelfCabinet.FirstOrDefault(m => m.DeviceID == id);
                        if (selfCabinet != null)
                        {
                            ViewBag.Lat = (decimal?)selfCabinet.Lat;
                            ViewBag.Lng = (decimal?)selfCabinet.Lng;
                        }
                    }
                    break;
                case "TVM":
                    using (var db = DbHelper.Create())
                    {
                        var tvm = db.Tb_Device_TVM.FirstOrDefault(m => m.DeviceID == id);
                        if (tvm != null)
                        {
                            ViewBag.Lat = (decimal?)tvm.Lat;
                            ViewBag.Lng = (decimal?)tvm.Lng;
                        }
                    }
                    break;
                default:
                    break;
            }
            ViewBag.device = device;
            ViewBag.id = id;
            return View();
        }
        #endregion

        #region Ticket
        /// <summary>
        /// 安全检票闸机
        /// </summary>
        /// <returns></returns>
        public ActionResult Ticket()
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
        public JsonResult GetTicketList()
        {
            //GetDeviceTikcetData();
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            //var type = Convert.ToInt32(Request["Type"]);
            var deviceName = Request["DeviceName"];
            //起始日期
            var starts = Request["starts"];
            //结束日期
            var end = Request["end"];
            var o = new object();
            using (var db = DbHelper.Create())
            {
                var result = (IEnumerable<Tb_Device_Ticket>)db.Tb_Device_Ticket;
                //if (type != -1)
                //{
                //    result = result.Where(m => m.Enabled == type);
                //}
                if (!string.IsNullOrEmpty(deviceName))
                {
                    result = result.Where(m => m.DeviceName.Contains(deviceName));

                }
                if (starts != "" && end != "")
                {
                    result = result.Where(m => m.SaveDate >= Convert.ToDateTime(starts+" 00:00:00") && m.SaveDate <= Convert.ToDateTime((end+" 23:59:59")));
                }
                else if (starts != "")
                {
                    result = result.Where(m => m.SaveDate >= Convert.ToDateTime(starts + " 00:00:00"));
                }
                else if (end != "")
                {
                    result = result.Where(m => m.SaveDate <= Convert.ToDateTime((end+" 23:59:59")));
                }
                var recordsTotal = result.Count();
                var data = result.OrderByDescending(m => m.DeviceID).Skip(start).Take(rows).ToList();
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
        /// 添加安全检票闸机
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult AddTicket(int? id ,int ? type)
        {
            if (id.HasValue) //在编辑的情况下，需要前台页面判断业务对象是否为null
            {
                using (var db = DbHelper.Create())
                {
                    ViewBag.ticket = db.Tb_Device_Ticket.FirstOrDefault(u => u.DeviceID == id);

                }
            }
            ViewBag.Type = type != 1 ? "添加" : "修改";
            return View();
        }
        /// <summary>
        /// 获取DVR设备名称
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        [HttpGet]
        public JsonResult GetDvrOrLed()
        {
            using (var db = DbHelper.Create())
            {
                var dvr = db.Tb_Device_DVR.Select(s => new { s.DeviceID, s.DeviceName }).ToList();
                var led = db.Tb_Device_LED.Select(s => new { s.DeviceID, s.DeviceName }).ToList();
                var data = new
                {
                    dvr = dvr,
                    led = led
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// 获取led设备名称
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        [HttpGet]
        public JsonResult GetLed()
        {
            using (var  db=DbHelper.Create())
            {
                var list = db.Tb_Device_LED.Select(s => new {s.DeviceID,s.DeviceName}).ToList();
                return Json(list,JsonRequestBehavior.AllowGet);
            }
            
        }

        /// <summary>
        /// 编辑安全检票闸机
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CheckSession]
        public ActionResult EditTicket(int id)
        {
            return RedirectToAction("AddTicket", new { id,type=1 });
        }
        /// <summary>
        /// 删除安全检票闸机
        /// </summary>
        /// <param name="id">删除id</param>
        /// <returns></returns>
        [CheckSession]
        [HttpGet]
        public ActionResult DeleteTicket(int? id, string arry)
        {
            var result = ActionJsonResult.Create();

            try
            {
                using (var db = DbHelper.Create())
                {

                    var number = db.Database.ExecuteSqlCommand("delete tb_device_ticket where deviceid in(" + arry + ")");
                    if (number == 0)
                    {
                        result.Failed("");
                    }
                }
            }
            catch (Exception e)
            {

                result.Failed("异常");
            }
            return result.ToJson();
        }

        /// <summary>
        /// 保存更新安全检票闸机_设备
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult SaveTicket(Tb_Device_Ticket ticket)
        {

            var result = ActionJsonResult.Create();
            if (ticket == null)
            {
                result.Failed("数据异常.");
            }
            else
            {
                using (var db = DbHelper.Create())
                {

                    try
                    {
                        var tmp = db.Tb_Device_Ticket.FirstOrDefault((a => a.DeviceID == ticket.DeviceID));
                        if (tmp != null)
                        {
                            tmp.DeviceSN = ticket.DeviceSN;
                            tmp.DeviceName = ticket.DeviceName;
                            tmp.StartDate = ticket.StartDate;
                            tmp.EndDate = ticket.EndDate;
                            tmp.Enabled = ticket.Enabled;
                            tmp.LastUpdate = DateTime.Now;
                            tmp.Remarks = ticket.Remarks;
                            tmp.SystemType = ticket.SystemType;
                            tmp.DvrID = ticket.DvrID;
                            tmp.LedID = ticket.LedID;
                            tmp.Version += 1;
                            tmp.UIID = ticket.UIID;
                            tmp.UICount = ticket.UICount;
                            db.Entry(tmp).State = EntityState.Modified;
                        }
                        else
                        {
                            if (db.Tb_Device_Ticket.FirstOrDefault((a => a.DeviceSN == ticket.DeviceSN)) != null)
                            {
                                result.Failed("设备序列号(" + ticket.DeviceSN + ")名称已存在，请从新填写！");
                            }
                            else
                            {
                                ticket.Version = 1;
                                ticket.SaveDate = DateTime.Now;
                                db.Tb_Device_Ticket.Add(ticket);
                            }
                        }
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        result.Failed(ex.Message);
                    }
                }

            }
            return result.ToJson();
        }



        /// <summary>
        /// 14646464
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="config"></param>
        [CheckSession]
        [HttpPost]
        public ActionResult SaveDvrConfig(int id, string type, DeviceMode deviceMode)
        {
            var o = new object();
            try
            {
                using (var db = DbHelper.Create())
                {
                    var deviceTicket = db.Tb_Device_Ticket.FirstOrDefault(s => s.DeviceID == id);
                    var deviceWorkModeUpdate = new DeviceWorkModeUpdate() { ModeList = new List<DeviceWorkMode>() };
                    List<DeviceWorkMode> DeviceModeList = new List<DeviceWorkMode>();

                    if (deviceTicket.Config != null && !string.IsNullOrEmpty(deviceTicket.Config))
                    {
                        deviceWorkModeUpdate = DocomSDK.Function.DeserializeJSON<DeviceWorkModeUpdate>(deviceTicket.Config);
                    }
                    DeviceModeList = deviceWorkModeUpdate.ModeList;

                    DeviceWorkMode deviceWorkMode = new DeviceWorkMode();
                    deviceWorkMode.DeviceEnable = deviceMode.DeviceEnable;
                    deviceWorkMode.ChannelMode = deviceMode.ChannelMode == 0 ? DocomSDK.Ticket.Enum.Channel_Mode.Channel_A : DocomSDK.Ticket.Enum.Channel_Mode.Channel_B;
                    deviceWorkMode.FingerprintLevel = deviceMode.FingerprintLevel;
                    deviceWorkMode.Snapshot = deviceMode.Snapshot;
                    deviceWorkMode.LockageTimeout = deviceMode.LockageTimeout;
                    deviceWorkMode.FingerprintTimeout = deviceMode.FingerprintTimeout;
                    deviceWorkMode.IDTimeout = deviceMode.IDTimeout;
                    deviceWorkMode.ExitMode = deviceMode.ExitMode;
                    var beginArr = deviceMode.Begin.Split(':');
                    var endArr = deviceMode.End.Split(':');
                    Time begin = new Time(Convert.ToInt32(beginArr[0]), Convert.ToInt32(beginArr[1]), Convert.ToInt32(beginArr[2]));
                    Time end = new Time(Convert.ToInt32(endArr[0]), Convert.ToInt32(endArr[1]), Convert.ToInt32(endArr[2]));
                    TimeBlock timeBlock = new TimeBlock(begin, end);
                    deviceWorkMode.CheckTicketTimeBlock = timeBlock;

                    var timeBlockList = DeviceModeList.Select(s => s.CheckTicketTimeBlock);
                    foreach (var item in timeBlockList)
                    {
                        bool retBegin = item.Contains(begin);
                        bool retEnd = item.Contains(end);
                        if (retBegin || retEnd)
                        {
                            throw new Exception("时间段冲突");
                        }
                    }
                    if (type == "add")
                    {
                        int max = 0;
                        if (DeviceModeList.Count() > 0)
                        {
                            max = DeviceModeList.Max(s => s.ID);
                        }
                        deviceWorkMode.ID = max + 1;
                        DeviceModeList.Add(deviceWorkMode);
                    }
                    else if (type == "edit")
                    {
                        var tempMode = DeviceModeList.FirstOrDefault(s => s.ID == deviceMode.Id);
                        DeviceModeList.Remove(tempMode);
                        DeviceModeList.Add(deviceWorkMode);
                    }
                    else
                    {
                        var tempMode = DeviceModeList.FirstOrDefault(s => s.ID == deviceMode.Id);
                        DeviceModeList.Remove(tempMode);
                    }

                    var version = deviceTicket.Version + 1;
                    deviceWorkModeUpdate.WorkMode_Version = version;
                    deviceWorkModeUpdate.ModeList = DeviceModeList;
                    deviceTicket.Version = version;
                    deviceTicket.Config = DocomSDK.Function.SerializeJSON<DeviceWorkModeUpdate>(deviceWorkModeUpdate);
                    db.Entry<Tb_Device_Ticket>(deviceTicket).State = EntityState.Modified;
                    db.SaveChanges();

                    o = new
                    {
                        State = 1,
                        Message = "保存成功"
                    };
                }
            }
            catch (Exception ex)
            {
                o = new
                {
                    State = 0,
                    Message = ex.Message
                };
            }

            return Json(o, JsonRequestBehavior.AllowGet);
        }

        [CheckSession]
        [HttpGet]
        public ActionResult DelDvrConfig(int id, int idx)
        {
            var o = new object();
            try
            {
                using (var db = DbHelper.Create())
                {
                    var deviceTicket = db.Tb_Device_Ticket.FirstOrDefault(s => s.DeviceID == id);
                    var deviceWorkModeUpdate = DocomSDK.Function.DeserializeJSON<DeviceWorkModeUpdate>(deviceTicket.Config);
                    List<DeviceWorkMode> DeviceModeList = deviceWorkModeUpdate.ModeList;

                    var tempMode = DeviceModeList.FirstOrDefault(s => s.ID == idx);
                    DeviceModeList.Remove(tempMode);

                    var version = deviceTicket.Version + 1;
                    deviceWorkModeUpdate.WorkMode_Version = version;
                    deviceWorkModeUpdate.ModeList = DeviceModeList;
                    deviceTicket.Version = version;
                    deviceTicket.Config = DocomSDK.Function.SerializeJSON<DeviceWorkModeUpdate>(deviceWorkModeUpdate);
                    db.Entry<Tb_Device_Ticket>(deviceTicket).State = EntityState.Modified;
                    db.SaveChanges();

                    o = new
                    {
                        State = 1,
                        Message = "删除成功"
                    };
                }
            }
            catch (Exception ex)
            {
                o = new
                {
                    State = 0,
                    Message = "删除失败"
                };
            }

            return Json(o, JsonRequestBehavior.AllowGet);
        }
        [CheckSession]
        public ActionResult GetDeviceConfig(int DvrId, int idx)
        {
            ViewBag.id = DvrId;
            using (var db = DbHelper.Create())
            {
                var deviceTicket = db.Tb_Device_Ticket.FirstOrDefault(s => s.DeviceID == DvrId);
                var deviceWorkModeUpdate = DocomSDK.Function.DeserializeJSON<DeviceWorkModeUpdate>(deviceTicket.Config);

                var result = deviceWorkModeUpdate.ModeList.Select(s => new
                {
                    Id = s.ID,
                    DeviceEnable = s.DeviceEnable,
                    ChannelMode = Convert.ToInt32(s.ChannelMode),
                    FingerprintLevel = s.FingerprintLevel,
                    Snapshot = s.Snapshot,
                    LockageTimeout = s.LockageTimeout,
                    FingerprintTimeout = s.FingerprintTimeout,
                    IDTimeout = s.IDTimeout,
                    ExitMode = s.ExitMode,
                    Begin = s.CheckTicketTimeBlock.Begin.ToString(),
                    End = s.CheckTicketTimeBlock.End.ToString()
                });
                var deviceMode = result.FirstOrDefault(s => s.Id == idx);
                return Json(deviceMode, JsonRequestBehavior.AllowGet);
            }
        }
        [CheckSession]
        public ActionResult DeviceConfig(int id)
        {
            ViewBag.id = id;
            return View();
        }


        [CheckSession]
        [HttpPost]
        public JsonResult GetDeviceConfigList()
        {
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            var id = Convert.ToInt32(Request["id"]);

            var o = new object();
            using (var db = DbHelper.Create())
            {
                var deviceTicket = db.Tb_Device_Ticket.FirstOrDefault(s => s.DeviceID == id);
                DeviceWorkModeUpdate deviceWorkModeUpdate = new DeviceWorkModeUpdate() { ModeList = new List<DeviceWorkMode>() };
                IEnumerable<DeviceMode> result = new List<DeviceMode>();
                if (deviceTicket.Config != null)
                {
                    deviceWorkModeUpdate = DocomSDK.Function.DeserializeJSON<DeviceWorkModeUpdate>(deviceTicket.Config);
                    if (deviceWorkModeUpdate != null)
                    {
                        result = deviceWorkModeUpdate.ModeList.Select(s => new DeviceMode
                        {
                            Id = s.ID,
                            DeviceEnable = s.DeviceEnable,
                            ChannelMode = Convert.ToInt32(s.ChannelMode),
                            FingerprintLevel = s.FingerprintLevel,
                            Snapshot = s.Snapshot,
                            LockageTimeout = s.LockageTimeout,
                            FingerprintTimeout = s.FingerprintTimeout,
                            IDTimeout = s.IDTimeout,
                            ExitMode = s.ExitMode,
                            Begin = s.CheckTicketTimeBlock.Begin.ToString(),
                            End = s.CheckTicketTimeBlock.End.ToString()
                        });
                    }
                }

                ViewBag.json = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
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
        #region 
        /// <summary>
        /// 地图标记保存 lbh 
        /// </summary>
        /// <param name="id">设备id</param>
        /// <param name="lat">坐标lat</param>
        /// <param name="lng">坐标lng</param>
        /// <param name="device">设备名称</param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult Save(int id, decimal? lat, decimal? lng, string device)
        {

            lat = lat == 0 ? null : lat;
            lng = lng == 0 ? null : lng;

            var resultreturn = true;
            using (var db = DbHelper.Create())
            {
                switch (device)
                {
                    case "Ticket":

                        var ticket = db.Tb_Device_Ticket.FirstOrDefault(t => t.DeviceID == id);
                        if (ticket != null)
                        {
                            try
                            {
                                ticket.Lat = lat;
                                ticket.Lng = lng;
                                db.Entry(ticket).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            catch (Exception)
                            {
                                resultreturn = false;
                                throw;
                            }
                        }
                        else
                        {
                            resultreturn = false;
                        }
                
                break;
                case "DVR":
               
                    var dvr = db.Tb_Device_DVR.FirstOrDefault(t => t.DeviceID == id);
                    if (dvr != null)
                    {
                        try
                        {
                            dvr.Lat = lat;
                            dvr.Lng = lng;
                            db.Entry(dvr).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        catch (Exception)
                        {
                            resultreturn = false;
                            throw;
                        }
                    }
                    else
                    {
                        resultreturn = false;
                    }
               
                break;
                case "eSensor":
               
                    var eSensor = db.Tb_Device_eSensor.FirstOrDefault(t => t.DeviceID == id);
                    if (eSensor != null)
                    {
                        try
                        {
                            eSensor.Lat = lat;
                            eSensor.Lng = lng;
                            db.Entry(eSensor).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        catch (Exception)
                        {
                            resultreturn = false;
                            throw;
                        }
                    }
                    else
                    {
                        resultreturn = false;
                    }
               
                break;
                case "LED":
                var led = db.Tb_Device_LED.FirstOrDefault(t => t.DeviceID == id);
                    if (led != null)
                    {
                        try
                        {
                            led.Lat = lat;
                            led.Lng = lng;
                            db.Entry(led).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        catch (Exception)
                        {
                            resultreturn = false;
                            throw;
                        }
                    }
                    else
                    {
                        resultreturn = false;
                    }
               
                break;
                case "M4Channel":
               
                    var m4Channel = db.Tb_Device_M4Channel.FirstOrDefault(t => t.DeviceID == id);
                    if (m4Channel != null)
                    {
                        try
                        {
                            m4Channel.Lat = lat;
                            m4Channel.Lng = lng;
                            db.Entry(m4Channel).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        catch (Exception)
                        {
                            resultreturn = false;
                            throw;
                        }
                    }
                    else
                    {
                        resultreturn = false;
                    }
               
                break;
                case "ParkStore":
               
                    var parkStore = db.Tb_Device_ParkStore.FirstOrDefault(t => t.DeviceID == id);
                    if (parkStore != null)
                    {
                        try
                        {
                            parkStore.Lat = lat;
                            parkStore.Lng = lng;
                            db.Entry(parkStore).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        catch (Exception)
                        {
                            resultreturn = false;
                            throw;
                        }
                    }
                    else
                    {
                        resultreturn = false;
                    }
              
                break;
                case "SelfCabinet":
               
                    var selfCabinet = db.Tb_Device_SelfCabinet.FirstOrDefault(t => t.DeviceID == id);
                    if (selfCabinet != null)
                    {
                        try
                        {
                            selfCabinet.Lat = lat;
                            selfCabinet.Lng = lng;
                            db.Entry(selfCabinet).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        catch (Exception)
                        {
                            resultreturn = false;
                            throw;
                        }
                    }
                    else
                    {
                        resultreturn = false;
                    }
              
                break;
                case "TVM":
              
                    var tvm = db.Tb_Device_TVM.FirstOrDefault(t => t.DeviceID == id);
                    if (tvm != null)
                    {
                        try
                        {
                            tvm.Lat = lat;
                            tvm.Lng = lng;
                            db.Entry(tvm).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        catch (Exception)
                        {
                            resultreturn = false;
                            throw;
                        }
                    }
                    else
                    {
                        resultreturn = false;
                    }
              
                break;
                default:
                break;
            }
        }
       
            return Json(resultreturn, JsonRequestBehavior.AllowGet);
        }
        #endregion
        /// <summary>
        /// 初始化设备表已添加坐标显示
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetDeviceData(int id, string device)
        {
            #region

            try
            {
                using (var db = DbHelper.Create())
                {
                    switch (device)
                    {
                        case "Ticket":
                            var tickets = db.Tb_Device_Ticket.Where(m => m.Lat != null && m.Lng != null).ToList();
                            return Json(tickets, JsonRequestBehavior.AllowGet);
                        case "DVR":
                            var dvrs = db.Tb_Device_DVR.Where(m => m.Lat != null && m.Lng != null).ToList();
                            return Json(dvrs, JsonRequestBehavior.AllowGet);
                        case "eSensor":
                            var eSensors = db.Tb_Device_eSensor.Where(m => m.Lat != null && m.Lng != null).ToList();
                            return Json(eSensors, JsonRequestBehavior.AllowGet);
                        case "LED":
                            var leds = db.Tb_Device_LED.Where(m => m.Lat != null && m.Lng != null).ToList();
                            return Json(leds, JsonRequestBehavior.AllowGet);
                        case "M4Channel":
                            var m4Channels = db.Tb_Device_M4Channel.Where(m => m.Lat != null && m.Lng != null).ToList();
                            return Json(m4Channels, JsonRequestBehavior.AllowGet);
                        case "ParkStore":
                            var parkStores = db.Tb_Device_ParkStore.Where(m => m.Lat != null && m.Lng != null).ToList();
                            return Json(parkStores, JsonRequestBehavior.AllowGet);
                        case "SelfCabinet":
                            var selfCabinets = db.Tb_Device_SelfCabinet.Where(m => m.Lat != null && m.Lng != null).ToList();
                            return Json(selfCabinets, JsonRequestBehavior.AllowGet);
                        case "TVM":
                            var tvms = db.Tb_Device_TVM.Where(m => m.Lat != null && m.Lng != null).ToList();
                            return Json(tvms, JsonRequestBehavior.AllowGet);
                        default:
                            return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            #endregion
        }

        #endregion

        #region M4Channel
        /// <summary>
        /// M4出口闸机
        /// </summary>
        /// <returns></returns>
        public ActionResult M4Channel()
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
        public JsonResult GetM4ChannelList()
        {
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            //var type = Convert.ToInt32(Request["Type"]);
            var deviceName = Request["DeviceName"];
            //起始日期
            var starts = Request["starts"];
            //结束日期
            var end = Request["end"];
            var o = new object();
            using (var db = DbHelper.Create())
            {
                var result = (IEnumerable<Tb_Device_M4Channel>)db.Tb_Device_M4Channel;
                //if (type != -1)
                //{
                //    result = result.Where(m => m.Enabled == type);
                //}
                if (!string.IsNullOrEmpty(deviceName))
                {
                    result = result.Where(m => m.DeviceName.Contains(deviceName));
                }
                if (starts != "" && end != "")
                {
                    result = result.Where(m => m.SaveDate >= Convert.ToDateTime(starts + " 00:00:00") && m.SaveDate <= Convert.ToDateTime((end + " 23:59:59")));
                }
                else if (starts != "")
                {
                    result = result.Where(m => m.SaveDate >= Convert.ToDateTime(starts + " 00:00:00"));
                }
                else if (end != "")
                {
                    result = result.Where(m => m.SaveDate <= Convert.ToDateTime((end + " 23:59:59")));
                }
                var recordsTotal = result.Count();
                var data = result.OrderByDescending(m => m.DeviceID).Skip(start).Take(rows).ToList();
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
        ///添加M4出口闸机
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult AddM4Channel(int? id ,int ? type)
        {
            if (id.HasValue) //在编辑的情况下，需要前台页面判断业务对象是否为null
            {
                using (var db = DbHelper.Create())
                {
                    ViewBag.m4channel = db.Tb_Device_M4Channel.FirstOrDefault(u => u.DeviceID == id);
                }
            }
            ViewBag.Type = type != 1 ? "添加" : "修改";
            return View();
        }

        /// <summary>
        /// 编辑M4出口闸机
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CheckSession]
        public ActionResult EditM4Channel(int id)
        {
            return RedirectToAction("AddM4Channel", new { id, type=1});
        }
        /// <summary>
        /// 删除M4出口闸机
        /// </summary>
        /// <param name="id">删除id</param>
        /// <returns></returns>
        [CheckSession]
        [HttpGet]
        public ActionResult DeleteM4Channel(int? id, string arry)
        {
            var result = ActionJsonResult.Create();

            try
            {
                using (var db = DbHelper.Create())
                {

                    var number = db.Database.ExecuteSqlCommand("delete tb_device_M4Channel where deviceid in(" + arry + ")");
                    if (number == 0)
                    {
                        result.Failed("");
                    }
                }
            }
            catch (Exception e)
            {

                result.Failed("异常");
            }
            return result.ToJson();
        }

        /// <summary>
        /// 保存更新M4出口闸机_设备
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult SaveM4Channel(Tb_Device_M4Channel m4channel)
        {

            var result = ActionJsonResult.Create();
            if (m4channel == null)
            {
                result.Failed("数据异常.");
            }
            else
            {
                using (var db = DbHelper.Create())
                {
                    try
                    {

                        var tmp = db.Tb_Device_M4Channel.FirstOrDefault((a => a.DeviceID == m4channel.DeviceID));
                        if (tmp != null)
                        {
                            tmp.DeviceSN = m4channel.DeviceSN;
                            tmp.DeviceName = m4channel.DeviceName;
                            tmp.StartDate = m4channel.StartDate;
                            tmp.EndDate = m4channel.EndDate;
                            tmp.Enabled = m4channel.Enabled;
                            tmp.LastUpdate = DateTime.Now; ;
                            tmp.M4WorkMode = m4channel.M4WorkMode;
                            tmp.TotalCount = m4channel.TotalCount;
                            tmp.Version += 1;
                            db.Entry(tmp).State = EntityState.Modified;
                        }
                        else
                        {
                            if (db.Tb_Device_M4Channel.FirstOrDefault((a => a.DeviceSN == m4channel.DeviceSN)) != null)
                            {
                                result.Failed("设备序列号(" + m4channel.DeviceSN + ")名称已存在，请从新填写！");
                            }
                            else
                            {
                                m4channel.Version = 1;
                                m4channel.SaveDate=DateTime.Now;
                                db.Tb_Device_M4Channel.Add(m4channel);
                            }
                        }
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        result.Failed(ex.Message);
                    }
                }

            }
            return result.ToJson();
        }
        #endregion

        #region ParkStroe
        /// <summary>
        ///水公园储物柜
        /// </summary>
        /// <returns></returns>
         [CheckSession]
        public ActionResult ParkStroe()
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
        public JsonResult GetParkStoreList()
        {
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            //var type = Convert.ToInt32(Request["Type"]);
            var deviceName = Request["DeviceName"];
            //起始日期
            var starts = Request["starts"];
            //结束日期
            var end = Request["end"];
            var o = new object();
            using (var db = DbHelper.Create())
            {
                var result = (IEnumerable<Tb_Device_ParkStore>)db.Tb_Device_ParkStore;
                //if (type != -1)
                //{
                //    result = result.Where(m => m.Enabled == type);
                //}
                if (!string.IsNullOrEmpty(deviceName))
                {
                    result = result.Where(m => m.DeviceName.Contains(deviceName));
                }
                if (starts != "" && end != "")
                {
                    result = result.Where(m => m.SaveDate >= Convert.ToDateTime(starts + " 00:00:00") && m.SaveDate <= Convert.ToDateTime((end + " 23:59:59")));
                }
                else if (starts != "")
                {
                    result = result.Where(m => m.SaveDate >= Convert.ToDateTime(starts + " 00:00:00"));
                }
                else if (end != "")
                {
                    result = result.Where(m => m.SaveDate <= Convert.ToDateTime((end + " 23:59:59")));
                }
                var recordsTotal = result.Count();
                var data = result.OrderByDescending(m => m.DeviceID).Skip(start).Take(rows).ToList();
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
        ///添加水公园储物柜
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult AddParkStroe(int? id,int ? type)
        {
            if (id.HasValue) //在编辑的情况下，需要前台页面判断业务对象是否为null
            {
                using (var db = DbHelper.Create())
                {
                    ViewBag.parkstore = db.Tb_Device_ParkStore.FirstOrDefault(u => u.DeviceID == id);
                }
            }
            ViewBag.Type = type != 1 ? "添加" : "修改";
            return View();
        }

        /// <summary>
        /// 编辑水公园储物柜
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CheckSession]
        public ActionResult EditParkStroe(int id)
        {
            return RedirectToAction("AddParkStroe", new { id,type=1 });
        }
        /// <summary>
        /// 删除水公园储物柜
        /// </summary>
        /// <param name="id">删除id</param>
        /// <returns></returns>
        [CheckSession]
        [HttpGet]
        public ActionResult DeleteParkStroe(int? id, string arry)
        {
            var result = ActionJsonResult.Create();

            try
            {
                using (var db = DbHelper.Create())
                {

                    var number = db.Database.ExecuteSqlCommand("delete Tb_Device_ParkStore where deviceid in(" + arry + ")");
                    if (number == 0)
                    {
                        result.Failed("");
                    }
                }
            }
            catch (Exception e)
            {

                result.Failed("异常");
            }
            return result.ToJson();
        }

        /// <summary>
        /// 保存更新水公园储物柜_设备
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult SaveParkStore(Tb_Device_ParkStore parkstore)
        {
            var result = ActionJsonResult.Create();
            if (parkstore == null)
            {
                result.Failed("数据异常.");
            }
            else
            {
                using (var db = DbHelper.Create())
                {
                    try
                    {

                        var tmp = db.Tb_Device_ParkStore.FirstOrDefault((a => a.DeviceID == parkstore.DeviceID));
                        if (tmp != null)
                        {
                            tmp.DeviceSN = parkstore.DeviceSN;
                            tmp.DeviceName = parkstore.DeviceName;
                            tmp.StartDate = parkstore.StartDate;
                            tmp.EndDate = parkstore.EndDate;
                            tmp.Enabled = parkstore.Enabled;
                            tmp.LastUpdate = DateTime.Now; ;
                            db.Entry(tmp).State = EntityState.Modified;
                        }
                        else
                        {
                            if (db.Tb_Device_ParkStore.FirstOrDefault((a => a.DeviceSN == parkstore.DeviceSN)) !=
                                null)
                            {
                                result.Failed("设备序列号(" + parkstore.DeviceSN + ")名称已存在，请从新填写！");
                            }
                            else
                            {
                                parkstore.SaveDate=DateTime.Now;
                                db.Tb_Device_ParkStore.Add(parkstore);
                            }
                        }
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        result.Failed(ex.Message);
                    }
                }

            }
            return result.ToJson();
        }
        #endregion

        #region SelfCabine
        /// <summary>
        ///自助储物柜
        /// </summary>
        /// <returns></returns>
        public ActionResult SelfCabine()
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
        public JsonResult GetSelfCabinetList()
        {
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            //var type = Convert.ToInt32(Request["Type"]);
            var deviceName = Request["DeviceName"];
            //起始日期
            var starts = Request["starts"];
            //结束日期
            var end = Request["end"];
            var o = new object();
            using (var db = DbHelper.Create())
            {
                var result = (IEnumerable<Tb_Device_SelfCabinet>)db.Tb_Device_SelfCabinet;
                //if (type != -1)
                //{
                //    result = result.Where(m => m.Enabled == type);
                //}
                if (!string.IsNullOrEmpty(deviceName))
                {
                    result = result.Where(m => m.DeviceName.Contains(deviceName));
                }
                if (starts != "" && end != "")
                {
                    result = result.Where(m => m.SaveDate >= Convert.ToDateTime(starts + " 00:00:00") && m.SaveDate <= Convert.ToDateTime((end + " 23:59:59")));
                }
                else if (starts != "")
                {
                    result = result.Where(m => m.SaveDate >= Convert.ToDateTime(starts + " 00:00:00"));
                }
                else if (end != "")
                {
                    result = result.Where(m => m.SaveDate <= Convert.ToDateTime((end + " 23:59:59")));
                }
                var recordsTotal = result.Count();
                var data = result.OrderByDescending(m => m.DeviceID).Skip(start).Take(rows).ToList();
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
        ///添加自助储物柜
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult AddSelfCabine(int? id, int ?type)
        {
            if (id.HasValue) //在编辑的情况下，需要前台页面判断业务对象是否为null
            {
                using (var db = DbHelper.Create())
                {
                    ViewBag.selfcabinet = db.Tb_Device_SelfCabinet.FirstOrDefault(u => u.DeviceID == id);
                }
            }
            ViewBag.Type = type != 1 ? "添加" : "修改";
            return View();
        }
        /// <summary>
        /// 编辑自助储物柜
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CheckSession]
        public ActionResult EditSelfCabine(int id)
        {
            return RedirectToAction("AddSelfCabine", new { id,type=1 });
        }
        /// <summary>
        /// 删除自助储物柜信息
        /// </summary>
        /// <param name="id">删除id</param>
        /// <returns></returns>
        [CheckSession]
        [HttpGet]
        public ActionResult DeleteSelfCabine(int? id,string arry)
        {
            var result = ActionJsonResult.Create();

            try
            {
                using (var db = DbHelper.Create())
                {

                    var number = db.Database.ExecuteSqlCommand("delete Tb_Device_SelfCabinet where deviceid in(" + arry + ")");
                    if (number == 0)
                    {
                        result.Failed("");
                    }
                }
            }
            catch (Exception e)
            {

                result.Failed("异常");
            }
            return result.ToJson();
        }
        /// <summary>
        /// 保存更新自助储物柜_设备
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult SaveSelfCabinet(Tb_Device_SelfCabinet selfcabinet)
        {
            var result = ActionJsonResult.Create();
            if (selfcabinet == null)
            {
                result.Failed("数据异常.");
            }
            else
            {
                using (var db = DbHelper.Create())
                {
                    try
                    {

                        var tmp = db.Tb_Device_SelfCabinet.FirstOrDefault((a => a.DeviceID == selfcabinet.DeviceID));
                        if (tmp != null)
                        {
                            tmp.DeviceSN = selfcabinet.DeviceSN;
                            tmp.DeviceName = selfcabinet.DeviceName;
                            tmp.StartDate = selfcabinet.StartDate;
                            tmp.EndDate = selfcabinet.EndDate;
                            tmp.Enabled = selfcabinet.Enabled;
                            tmp.LastUpdate = DateTime.Now;
                            db.Entry(tmp).State = EntityState.Modified;
                        }
                        else
                        {
                            if (db.Tb_Device_SelfCabinet.FirstOrDefault((a => a.DeviceSN == selfcabinet.DeviceSN)) != null)
                            {
                                result.Failed("设备序列号(" + selfcabinet.DeviceSN + ")名称已存在，请从新填写！");
                            }
                            else
                            {
                                selfcabinet.SaveDate=DateTime.Now;
                                db.Tb_Device_SelfCabinet.Add(selfcabinet);
                            }
                        }
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        result.Failed(ex.Message);
                    }
                }

            }
            return result.ToJson();
        }
        #endregion

        #region TVM
        /// <summary>
        ///TVM
        /// </summary>
        /// <returns></returns>
        public ActionResult TVM()
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
        public JsonResult GetTVMList()
        {
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            //var type = Convert.ToInt32(Request["Type"]);
            var deviceName = Request["DeviceName"];
            //起始日期
            var starts = Request["starts"];
            //结束日期
            var end = Request["end"];
            var o = new object();
            using (var db = DbHelper.Create())
            {
                var result = (IEnumerable<Tb_Device_TVM>)db.Tb_Device_TVM;
                //if (type != -1)
                //{
                //    result = result.Where(m => m.Enabled == type);
                //}
                if (!string.IsNullOrEmpty(deviceName))
                {
                    result = result.Where(m => m.DeviceName.Contains(deviceName));
                }
                if (starts != "" && end != "")
                {
                    result = result.Where(m => m.SaveDate >= Convert.ToDateTime(starts + " 00:00:00") && m.SaveDate <= Convert.ToDateTime((end + " 23:59:59")));
                }
                else if (starts != "")
                {
                    result = result.Where(m => m.SaveDate >= Convert.ToDateTime(starts + " 00:00:00"));
                }
                else if (end != "")
                {
                    result = result.Where(m => m.SaveDate <= Convert.ToDateTime((end + " 23:59:59")));
                }
                var recordsTotal = result.Count();
                var data = result.OrderByDescending(m => m.DeviceID).Skip(start).Take(rows).ToList();
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
        ///添加TVM
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult AddTVM(int? id,int ? type)
        {
            if (id.HasValue) //在编辑的情况下，需要前台页面判断业务对象是否为null
            {
                using (var db = DbHelper.Create())
                {
                    ViewBag.tvm = db.Tb_Device_TVM.FirstOrDefault(u => u.DeviceID == id);
                }
            }
            ViewBag.Type = type != 1 ? "添加" : "修改";
            return View();
        }

        /// <summary>
        /// 编辑TVM
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CheckSession]
        public ActionResult EditTVM(int id)
        {
            return RedirectToAction("AddTVM", new { id,type=1 });
        }
        /// <summary>
        /// 删除TVM信息
        /// </summary>
        /// <param name="id">删除id</param>
        /// <returns></returns>
        [CheckSession]
        [HttpGet]
        public ActionResult DeleteTVM(int? id,string arry)
        {
            var result = ActionJsonResult.Create();

            try
            {
                using (var db = DbHelper.Create())
                {

                    var number = db.Database.ExecuteSqlCommand("delete tb_device_TVM where deviceid in(" + arry + ")");
                    if (number == 0)
                    {
                        result.Failed("");
                    }
                }
            }
            catch (Exception e)
            {

                result.Failed("异常");
            }
            return result.ToJson();
        }

        /// <summary>
        /// 保存更新TVM_设备
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult SaveTVM(Tb_Device_TVM tvm)
        {
            var result = ActionJsonResult.Create();
            if (tvm == null)
            {
                result.Failed("数据异常.");
            }
            else
            {
                using (var db = DbHelper.Create())
                {
                    try
                    {

                        var tmp = db.Tb_Device_TVM.FirstOrDefault((a => a.DeviceID == tvm.DeviceID));
                        if (tmp != null)
                        {
                            tmp.DeviceName = tvm.DeviceName;
                            tmp.DeviceSN = tvm.DeviceSN;
                            tmp.StartDate = tvm.StartDate;
                            tmp.EndDate = tvm.EndDate;
                            tmp.Enabled = tvm.Enabled;
                            tmp.LastUpdate = DateTime.Now;
                            db.Entry(tmp).State = EntityState.Modified;
                        }
                        else
                        {
                            if (db.Tb_Device_TVM.FirstOrDefault((a => a.DeviceSN == tvm.DeviceSN)) != null)
                            {
                                result.Failed("设备序列号(" + tvm.DeviceSN + ")名称已存在，请从新填写！");
                            }
                            else
                            {
                                tvm.SaveDate=DateTime.Now;
                                db.Tb_Device_TVM.Add(tvm);
                            }
                        }
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        result.Failed(ex.Message);
                    }
                }

            }
            return result.ToJson();
        }
        #endregion

        #region DVR
        /// <summary>
        ///海康DVR
        /// </summary>
        /// <returns></returns>
        public ActionResult DVR()
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
        public JsonResult GetList()
        {
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            //var type = Convert.ToInt32(Request["Type"]);
            var deviceName = Request["DeviceName"];
            //起始日期
            var starts = Request["starts"];
            //结束日期
            var end = Request["end"];

            var o = new object();
            using (var db = DbHelper.Create())
            {
                var result = (IEnumerable<Tb_Device_DVR>)db.Tb_Device_DVR;
                //if (type != -1)
                //{
                //    result = result.Where(m => m.Enabled == type);
                //}
                if (!string.IsNullOrEmpty(deviceName))
                {
                    result = result.Where(m => m.DeviceName.Contains(deviceName));
                }
                if (starts != "" && end != "")
                {
                    result = result.Where(m => m.SaveDate >= Convert.ToDateTime(starts + " 00:00:00") && m.SaveDate <= Convert.ToDateTime((end + " 23:59:59")));
                }
                else if (starts != "")
                {
                    result = result.Where(m => m.SaveDate >= Convert.ToDateTime(starts + " 00:00:00"));
                }
                else if (end != "")
                {
                    result = result.Where(m => m.SaveDate <= Convert.ToDateTime((end + " 23:59:59")));
                }
                var recordsTotal = result.Count();
                var data = result.OrderByDescending(m => m.DeviceID).Skip(start).Take(rows).ToList();
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
        ///添加海康DVR
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult AddDVR(int? id,int? type)
        {
            if (id.HasValue) //在编辑的情况下，需要前台页面判断业务对象是否为null
            {
                using (var db = DbHelper.Create())
                {
                    ViewBag.dvr = db.Tb_Device_DVR.FirstOrDefault(u => u.DeviceID == id);
                }
            }
            ViewBag.Type = type != 1 ? "添加" : "修改";
            return View();
        }
        /// <summary>
        /// 编辑海康DVR
        /// </summary>
        /// <param name="id">编辑id</param>
        /// <returns></returns>
        [CheckSession]
        public ActionResult EditDVR(int id)
        {
            return RedirectToAction("AddDVR", new { id ,type=1});
        }
        /// <summary>
        /// 删除海康DVR
        /// </summary>
        /// <param name="id">DVR Id</param>
        /// <returns></returns>
        [CheckSession]
        [HttpGet]
        public ActionResult DeleteDVR(int? id,string arry)
        {
            var result = ActionJsonResult.Create();

            try
            {
                using (var db = DbHelper.Create())
                {

                    var number = db.Database.ExecuteSqlCommand("delete tb_device_DVR where deviceid in(" + arry + ")");
                    if (number == 0)
                    {
                        result.Failed("");
                    }
                }
            }
            catch (Exception e)
            {

                result.Failed("异常");
            }
            return result.ToJson();
        }

        /// <summary>
        /// 保存更新海康DVR_设备
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult SaveDVR(Tb_Device_DVR dvr)
        {
            var result = ActionJsonResult.Create();


            if (dvr == null)
            {
                result.Failed("数据异常.");
            }
            else
            {
                using (var db = DbHelper.Create())
                {

                    try
                    {
                        var tmp = db.Tb_Device_DVR.FirstOrDefault((a => a.DeviceID == dvr.DeviceID));
                        if (tmp != null)
                        {
                            tmp.DeviceName = dvr.DeviceName;
                            tmp.StartDate = dvr.StartDate;
                            tmp.EndDate = dvr.EndDate;
                            tmp.Enabled = dvr.Enabled;
                            tmp.LastUpdate = DateTime.Now;
                            tmp.IPAddress = dvr.IPAddress;
                            tmp.Port = dvr.Port;
                            tmp.Account = dvr.Account;
                            tmp.PWD = dvr.PWD;
                            tmp.ChannelID = dvr.ChannelID;
                            tmp.Remarks = dvr.Remarks;
                            db.Entry(tmp).State = EntityState.Modified;
                        }
                        else
                        {
                            if (db.Tb_Device_DVR.FirstOrDefault((a => a.DeviceName == dvr.DeviceName)) != null)
                            {
                                result.Failed("设备(" + dvr.DeviceName + ")名称已存在，请从新填写！");
                            }
                            else
                            {
                                dvr.SaveDate=DateTime.Now;
                                db.Tb_Device_DVR.Add(dvr);
                            }
                        }
                        db.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        result.Failed(ex.Message);
                    }
                }

            }
            return result.ToJson();
        }
        #endregion

        #region LED
        /// <summary>
        ///LED显示屏
        /// </summary>
        /// <returns></returns>
        public ActionResult LED()
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
        public JsonResult GetLEDList()
        {
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            //var type = Convert.ToInt32(Request["Type"]);
            var deviceName = Request["DeviceName"];
            //起始日期
            var starts = Request["starts"];
            //结束日期
            var end = Request["end"];
            var o = new object();
            using (var db = DbHelper.Create())
            {
                var result = (IEnumerable<Tb_Device_LED>)db.Tb_Device_LED;
                //if (type != -1)
                //{
                //    result = result.Where(m => m.Enabled == type);
                //}
                if (!string.IsNullOrEmpty(deviceName))
                {
                    result = result.Where(m => m.DeviceName.Contains(deviceName));
                }
                if (starts != "" && end != "")
                {
                    result = result.Where(m => m.SaveDate >= Convert.ToDateTime(starts + " 00:00:00") && m.SaveDate <= Convert.ToDateTime((end + " 23:59:59")));
                }
                else if (starts != "")
                {
                    result = result.Where(m => m.SaveDate >= Convert.ToDateTime(starts + " 00:00:00"));
                }
                else if (end != "")
                {
                    result = result.Where(m => m.SaveDate <= Convert.ToDateTime((end + " 23:59:59")));
                }
                var recordsTotal = result.Count();
                var data = result.OrderByDescending(m => m.DeviceID).Skip(start).Take(rows).ToList();
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
        ///添加LED显示屏
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult AddLED(int? id,int ? type)
        {
            if (id.HasValue) //在编辑的情况下，需要前台页面判断业务对象是否为null
            {
                using (var db = DbHelper.Create())
                {
                    ViewBag.led = db.Tb_Device_LED.FirstOrDefault(u => u.DeviceID == id);
                }
            }
            ViewBag.Type = type != 1 ? "添加" : "修改";
            return View();
        }
        /// <summary>
        /// 编辑LED
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CheckSession]
        public ActionResult EditLED(int id)
        {
            return RedirectToAction("AddLED", new { id,type=1 });
        }
        /// <summary>
        /// 删除LED信息
        /// </summary>
        /// <param name="id">删除id</param>
        /// <returns></returns>
        [CheckSession]
        [HttpGet]
        public ActionResult DeleteLED(int? id,string arry)
        {
            var result = ActionJsonResult.Create();

            try
            {
                using (var db = DbHelper.Create())
                {

                    var number = db.Database.ExecuteSqlCommand("delete tb_device_LED where deviceid in(" + arry + ")");
                    if (number == 0)
                    {
                        result.Failed("");
                    }
                }
            }
            catch (Exception e)
            {

                result.Failed("异常");
            }
            return result.ToJson();
        }

        /// <summary>
        /// 保存更新LED_设备
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult SaveLED(Tb_Device_LED led)
        {
            var result = ActionJsonResult.Create();
            if (led == null)
            {
                result.Failed("数据异常.");
            }
            else
            {
                using (var db = DbHelper.Create())
                {
                    try
                    {

                        var tmp = db.Tb_Device_LED.FirstOrDefault((a => a.DeviceID == led.DeviceID));
                        if (tmp != null)
                        {
                            tmp.DeviceName = led.DeviceName;
                            tmp.StartDate = led.StartDate;
                            tmp.EndDate = led.EndDate;
                            tmp.Enabled = led.Enabled;
                            tmp.LastUpdate = DateTime.Now;
                            tmp.IPAddress = led.IPAddress;
                            tmp.Port = led.Port;
                            tmp.Width = led.Width;
                            tmp.Height = led.Height;
                            tmp.Remarks = led.Remarks;
                            db.Entry(tmp).State = EntityState.Modified;
                        }
                        else
                        {
                            if (db.Tb_Device_LED.FirstOrDefault((a => a.DeviceName == led.DeviceName)) != null)
                            {
                                result.Failed("设备(" + led.DeviceName + ")名称已存在，请从新填写！");
                            }
                            else
                            {
                                led.SaveDate=DateTime.Now;
                                db.Tb_Device_LED.Add(led);
                            }
                        }
                        db.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        result.Failed(ex.Message);
                    }
                }

            }
            return result.ToJson();
        }

        #region  led配置

        #region  区域定义
        /// <summary>
        /// 区域定义
        /// </summary>
        public class RECT
        {
            public int ID { get; set; }
            /// <summary>
            /// 左上角坐标
            /// </summary>
            public int X { get; set; }
            /// <summary>
            /// 右上角坐标
            /// </summary>
            public int Y { get; set; }
            /// <summary>
            /// 宽
            /// </summary>
            public int Width { get; set; }
            /// <summary>
            /// 高
            /// </summary>
            public int Height { get; set; }
        }
        /// <summary>
        /// Json帮助类
        /// </summary>
        public class JsonHelper
        {
            /// <summary>
            /// 将对象序列化为JSON格式
            /// </summary>
            /// <param name="o">对象</param>
            /// <returns>json字符串</returns>
            public static string SerializeObject(object o)
            {
                string json = JsonConvert.SerializeObject(o);
                return json;
            }

            /// <summary>
            /// 解析JSON字符串生成对象实体
            /// </summary>
            /// <typeparam name="T">对象类型</typeparam>
            /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
            /// <returns>对象实体</returns>
            public static T DeserializeJsonToObject<T>(string json) where T : class
            {
                JsonSerializer serializer = new JsonSerializer();
                StringReader sr = new StringReader(json);
                object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
                T t = o as T;
                return t;
            }

            /// <summary>
            /// 解析JSON数组生成对象实体集合
            /// </summary>
            /// <typeparam name="T">对象类型</typeparam>
            /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
            /// <returns>对象实体集合</returns>
            public static List<T> DeserializeJsonToList<T>(string json) where T : class
            {
                JsonSerializer serializer = new JsonSerializer();
                StringReader sr = new StringReader(json);
                object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
                List<T> list = o as List<T>;
                return list;
            }

            /// <summary>
            /// 反序列化JSON到给定的匿名对象.
            /// </summary>
            /// <typeparam name="T">匿名对象类型</typeparam>
            /// <param name="json">json字符串</param>
            /// <param name="anonymousTypeObject">匿名对象</param>
            /// <returns>匿名对象</returns>
            public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject)
            {
                T t = JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
                return t;
            }
        }
        #endregion

        /// <summary>
        /// led配置保存
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="config"></param>
        [CheckSession]
        [HttpPost]
        public ActionResult SaveLedConfig(int id, string type, RECT deviceMode)
        {
            var o = new object();
            try
            {
                using (var db = DbHelper.Create())
                {
                    var deviceLed = db.Tb_Device_LED.FirstOrDefault(s => s.DeviceID == id);
                    var deviceWorkModeUpdate = new List<RECT>();
                    var DeviceModeList = new List<RECT>();

                    if (deviceLed.AreaConfig != null && !string.IsNullOrEmpty(deviceLed.AreaConfig))
                    {
                        deviceWorkModeUpdate = JsonHelper.DeserializeJsonToList<RECT>(deviceLed.AreaConfig);
                    }
                    DeviceModeList = deviceWorkModeUpdate;

                    RECT deviceRect = new RECT();
                    deviceRect.X = deviceMode.X;
                    deviceRect.Y = deviceMode.Y;
                    deviceRect.Width = deviceMode.Width;
                    deviceRect.Height = deviceMode.Height;


                    if (type == "add")
                    {
                        int max = 0;
                        if (DeviceModeList.Count() > 0)
                        {
                            max = DeviceModeList.Max(s => s.ID);
                        }
                        deviceRect.ID = max + 1;
                        DeviceModeList.Add(deviceRect);
                    }
                    else if (type == "edit")
                    {
                        var tempMode = DeviceModeList.FirstOrDefault(s => s.ID == deviceMode.ID);
                        DeviceModeList.Remove(tempMode);
                        DeviceModeList.Add(deviceRect);
                    }
                    else
                    {
                        var tempMode = DeviceModeList.FirstOrDefault(s => s.ID == deviceMode.ID);
                        DeviceModeList.Remove(tempMode);
                    }

                    //var version = deviceTicket.Version + 1;
                    //deviceWorkModeUpdate.WorkMode_Version = version;
                    //deviceWorkModeUpdate= DeviceModeList;
                    //deviceTicket.Version = version;
                    deviceLed.AreaConfig = JsonHelper.SerializeObject(DeviceModeList);
                    db.Entry<Tb_Device_LED>(deviceLed).State = EntityState.Modified;
                    db.SaveChanges();

                    o = new
                    {
                        State = 1,
                        Message = "保存成功"
                    };
                }
            }
            catch (Exception ex)
            {
                o = new
                {
                    State = 0,
                    Message = ex.Message
                };
            }

            return Json(o, JsonRequestBehavior.AllowGet);
        }

        [CheckSession]
        [HttpGet]
        public ActionResult DelLedConfig(int id, int idx)
        {
            var o = new object();
            try
            {
                using (var db = DbHelper.Create())
                {
                    var deviceLed = db.Tb_Device_LED.FirstOrDefault(s => s.DeviceID == id);
                    var deviceWorkModeUpdate = JsonHelper.DeserializeJsonToList<RECT>(deviceLed.AreaConfig);
                    List<RECT> DeviceModeList = deviceWorkModeUpdate;
                    var tempMode = DeviceModeList.FirstOrDefault(s => s.ID == idx);
                    DeviceModeList.Remove(tempMode);

                    string json = string.Empty;
                    if (DeviceModeList.Count() > 0)
                    {
                        json = JsonHelper.SerializeObject(DeviceModeList);
                    }
                    deviceLed.AreaConfig = json;
                    db.Entry<Tb_Device_LED>(deviceLed).State = EntityState.Modified;
                    db.SaveChanges();

                    o = new
                    {
                        State = 1,
                        Message = "删除成功"
                    };
                }
            }
            catch (Exception ex)
            {
                o = new
                {
                    State = 0,
                    Message = "删除失败"
                };
            }

            return Json(o, JsonRequestBehavior.AllowGet);
        }
        [CheckSession]
        [HttpPost]
        public ActionResult GetLedConfig(int DvrId, int idx)
        {
            ViewBag.id = DvrId;
            using (var db = DbHelper.Create())
            {
                var deviceLed = db.Tb_Device_LED.FirstOrDefault(s => s.DeviceID == DvrId);

                var deviceWorkModeUpdate = JsonHelper.DeserializeJsonToList<RECT>(deviceLed.AreaConfig);

                var result = deviceWorkModeUpdate.Select(s => new
                {
                    Id = s.ID,
                    X = s.X,
                    Y = s.Y,
                    Width = s.Width,
                    Height = s.Height
                });
                var deviceMode = result.FirstOrDefault(s => s.Id == idx);
                return Json(deviceMode, JsonRequestBehavior.AllowGet);
            }
        }
        [CheckSession]
        public ActionResult LEDConfig(int id)
        {
            ViewBag.id = id;
            return View();
        }


        [CheckSession]
        [HttpPost]
        public JsonResult GetLedConfigList()
        {
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            var id = Convert.ToInt32(Request["id"]);

            var o = new object();
            using (var db = DbHelper.Create())
            {
                var deviceLed = db.Tb_Device_LED.FirstOrDefault(s => s.DeviceID == id);

                IEnumerable<RECT> result = new List<RECT>();
                if (deviceLed.AreaConfig != null)
                {
                    var deviceWorkModeUpdate = JsonHelper.DeserializeJsonToList<RECT>(deviceLed.AreaConfig);
                    // deviceWorkModeUpdate = DocomSDK.Function.DeserializeJSON<DeviceWorkModeUpdate>(deviceLed.AreaConfig);
                    if (deviceWorkModeUpdate != null)
                    {
                        result = deviceWorkModeUpdate.Select(s => new RECT()
                        {
                            ID = s.ID,
                            X = s.X,
                            Y = s.Y,
                            Width = s.Width,
                            Height = s.Height
                        });
                    }
                }

                ViewBag.json = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
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

        #region ESensor
        /// <summary>
        ///客流计
        /// </summary>
        /// <returns></returns>
        public ActionResult ESensor()
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
        public JsonResult GetESensorList()
        {
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            //var type = Convert.ToInt32(Request["Type"]);
            var deviceName = Request["DeviceName"];
            //起始日期
            var starts = Request["starts"];
            //结束日期
            var end = Request["end"];
            var o = new object();
            using (var db = DbHelper.Create())
            {
                var result = (IEnumerable<Tb_Device_eSensor>)db.Tb_Device_eSensor;
                //if (type != -1)
                //{
                //    result = result.Where(m => m.Enabled == type);
                //}
                if (!string.IsNullOrEmpty(deviceName))
                {
                    result = result.Where(m => m.DeviceName.Contains(deviceName));
                }
                if (starts != "" && end != "")
                {
                    result = result.Where(m => m.SaveDate >= Convert.ToDateTime(starts + " 00:00:00") && m.SaveDate <= Convert.ToDateTime((end + " 23:59:59")));
                }
                else if (starts != "")
                {
                    result = result.Where(m => m.SaveDate >= Convert.ToDateTime(starts + " 00:00:00"));
                }
                else if (end != "")
                {
                    result = result.Where(m => m.SaveDate <= Convert.ToDateTime((end + " 23:59:59")));
                }
                var recordsTotal = result.Count();
                var data = result.OrderByDescending(m => m.DeviceID).Skip(start).Take(rows).ToList();
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
        ///添加客流计
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult AddESensor(int? id,int ? type)
        {
            if (id.HasValue) //在编辑的情况下，需要前台页面判断业务对象是否为null
            {
                using (var db = DbHelper.Create())
                {
                    ViewBag.esensor = db.Tb_Device_eSensor.FirstOrDefault(u => u.DeviceID == id);
                }
            }
            ViewBag.Type = type != 1 ? "添加" : "修改";
            return View();
        }


        /// <summary>
        /// 客流计LED
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CheckSession]
        public ActionResult EditESensor(int id)
        {
            return RedirectToAction("AddESensor", new { id,type=1 });
        }
        /// <summary>
        /// 删除客流计信息
        /// </summary>
        /// <param name="id">删除id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteESensor(int? id,string arry)
        {
            var result = ActionJsonResult.Create();

            try
            {
                using (var db = DbHelper.Create())
                {

                    var number = db.Database.ExecuteSqlCommand("delete tb_device_eSensor where deviceid in(" + arry + ")");
                    if (number == 0)
                    {
                        result.Failed("");
                    }
                }
            }
            catch (Exception e)
            {

                result.Failed("异常");
            }
            return result.ToJson();
        }

        /// <summary>
        /// 保存更新客流计_设备
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult SaveESensor(Tb_Device_eSensor seensor)
        {
            var result = ActionJsonResult.Create();
            if (seensor == null)
            {
                result.Failed("数据异常.");
            }
            else
            {
                using (var db = DbHelper.Create())
                {
                    try
                    {

                        var tmp = db.Tb_Device_eSensor.FirstOrDefault((a => a.DeviceID == seensor.DeviceID));
                        if (tmp != null)
                        {
                            //tmp.DeviceSN = seensor.DeviceSN;
                            tmp.DeviceName = seensor.DeviceName;
                            tmp.StartDate = seensor.StartDate;
                            tmp.EndDate = seensor.EndDate;
                            tmp.Enabled = seensor.Enabled;
                            tmp.LastUpdate = DateTime.Now;
                            tmp.IPAddress = seensor.IPAddress;
                            //tmp.Port = seensor.Port;
                            db.Entry(tmp).State = EntityState.Modified;
                        }
                        else
                        {
                            if (db.Tb_Device_eSensor.FirstOrDefault((a => a.DeviceName == seensor.DeviceName)) != null)
                            {
                                result.Failed("设备序列号(" + seensor.DeviceName + ")名称已存在，请从新填写！");
                            }
                            else
                            {
                                seensor.SaveDate=DateTime.Now;
                                db.Tb_Device_eSensor.Add(seensor);
                            }
                        }
                        db.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        result.Failed(ex.Message);
                    }
                }

            }
            return result.ToJson();
        }
        #endregion
    }

}