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
    public class ApplicationRightController : Controller
    {

        #region 我的权限
        /// <summary>
        /// 我的权限
        /// </summary>
        /// <returns></returns>
        public ActionResult MyPermission()
        {
            var id = this.Load<Tb_Users>().UserID;
            using (var db = DbHelper.Create())
            {
                //功能API
                ViewBag.APIList = db.V_User_APIFunction.Where(a => a.UserID == id)?.ToList();
            }

            return View();
        }
        #endregion

        #region 权限管理
        /// <summary>
        /// 权限管理
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult AuthorityManage()
        {
            using (var db = DbHelper.Create())
            {
                var list = db.Tb_Users.Where(u => u.Level == 2).ToList();
                ViewBag.Users = list.Take(20).ToList();
                ViewBag.count = list.Count();
            }
            return View();
        }
        #endregion

        #region  分配权限
        /// <summary>
        /// 分配权限
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult Assignperm(int? id)
        {
            using (var db = DbHelper.Create())
            {
                //功能API
                ViewBag.User = Session["Web.EF.Tb_Users"];
                Tb_Users tbuser = ViewBag.User;
                if (id==null) {
                    id = tbuser.UserID;
                }
                ViewBag.selId = id;
                var APIList = db.Tb_APIFunction.ToList();
                var APIList1 = db.V_User_APIFunction.Where(a => a.UserID == id)?.ToList();
                List<UserAPI> list = new List<UserAPI>();
                List<EventShow> EventList = new List<EventShow>();
                foreach (var item in APIList)
                {
                    UserAPI UA = new UserAPI();
                    UA.APIName = item.APIName;
                    UA.API_ID = item.API_ID;
                    UA.ProviderUrl = item.ProviderUrl;
                    UA.Remarks = item.Remarks;
                    for (int i = 0; i < APIList1.Count; i++)
                    {
                        if (APIList1[i].API_ID == item.API_ID)
                        {
                            UA.UserId = Convert.ToInt32(id);
                        }
                    }
                    list.Add(UA);
                }
                ViewBag.APIList2 = list;
                ViewBag.APIList3 = APIList1;

                //事件提醒
                var eventUser = db.Tb_User_Notify.Where(u => u.UserID == id).ToList();
                var EventType = db.Tb_EventType.ToList();
                foreach (var item in EventType)
                {
                    EventShow evShow = new EventShow();
                    evShow.EventName = item.EventName;
                    evShow.EventTypeID = item.EventTypeID;
                    for (int i = 0; i < eventUser.Count; i++)
                    {
                        if (eventUser[i].EventTypeID == item.EventTypeID)
                        {
                            evShow.UserID = eventUser[i].UserID;
                            evShow.URL = eventUser[i].URL;
                        }
                    }
                    EventList.Add(evShow);
                }


                ViewBag.eventUser = eventUser;
                ViewBag.EventList = EventList;
            }
            return View();
        }


        //AddAndDelAPI
        /// <summary>
        /// 删除保存API
        /// </summary>
        /// <param name="Notify"></param>
        /// <returns></returns>
        [CheckSession]
        public ActionResult AddAndDelAPI()
        {
            var result = ActionJsonResult.Create();
            var arrayAPI = Request["Array"];
            int UserId = 0;
            if (!string.IsNullOrEmpty(Request["UserID"]))
            {
                UserId = Convert.ToInt32(Request["UserID"]);
            }

            using (var db = DbHelper.Create())
            {
                if (!string.IsNullOrEmpty(arrayAPI))
                {

                    var str = new string[] { };
                    str = arrayAPI.Split(';');
                    //判断删除还是添加
                    for (int i = 0; i < str.Length; i++)
                    {
                        var str2 = new string[] { };
                        str2 = str[i].Split(',');
                        if (str2[1] == "true")
                        {
                            int api_id = Convert.ToInt32(str2[0]);
                            if (UserId > 0)
                            {
                                try
                                {
                                    //查询是否有相同的记录存在，存在，则更新；否则插入新的
                                    var tmp = db.Tb_API_User.FirstOrDefault((a => a.UserID == UserId && a.API_ID == api_id));
                                    if (tmp != null)
                                    {
                                        tmp.UserID = UserId;
                                        tmp.API_ID = api_id;
                                       

                                        db.Entry(tmp).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        Tb_API_User UserAPI = new Tb_API_User();
                                        UserAPI.UserID = UserId;
                                        UserAPI.API_ID = api_id;
                                        UserAPI.StartDate = DateTime.Now;
                                        UserAPI.EndDate = DateTime.Now.AddDays(10);
                                        UserAPI.TotalCount = 50;
                                        UserAPI.Enabled = 0;
                                        db.Tb_API_User.Add(UserAPI);
                                    }
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    result.Failed(e.Message);
                                }
                            }
                            else
                            {
                                result.Failed("无法查询到指定的用户.");
                            }
                        }
                        else
                        {
                            ////删除用户
                            //查询是否有相同的记录存在，存在，则删除
                            int api_id = Convert.ToInt32(str2[0]);
                            var tmp = db.Tb_API_User.FirstOrDefault((a => a.UserID == UserId && a.API_ID == api_id));
                            if (tmp == null)
                            {
                                //result.Failed("无法查询到指定的用户.");
                            }
                            else
                            {
                                try
                                {
                                    db.Tb_API_User.Remove(tmp);
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    result.Failed(e.Message);
                                }
                            }
                        }
                    }
                }
                else
                {
                    result.Failed("无法查询到指定的用户.");
                }

            }
            return result.ToJson();
        }



        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="Notify"></param>
        /// <returns></returns>
        [CheckSession]
        public ActionResult AddEvent(Tb_User_Notify Notify)
        {
            var result = ActionJsonResult.Create();
            if (Notify == null)
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
                        var tmp = db.Tb_User_Notify.FirstOrDefault((a => a.UserID == Notify.UserID&&a.EventTypeID== Notify.EventTypeID));
                        if (tmp != null)
                        {
                            tmp.UserID = Notify.UserID;
                            tmp.EventTypeID = Notify.EventTypeID;
                            tmp.URL = Notify.URL;
                            tmp.LastUpdate = DateTime.Now;
                            db.Entry(tmp).State = EntityState.Modified;
                        }
                        else
                        {
                            Notify.LastUpdate = DateTime.Now;
                            db.Tb_User_Notify.Add(Notify);
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
        /// 删除事件
        /// </summary>
        /// <param name="Notify"></param>
        /// <returns></returns>
        [CheckSession]
        public ActionResult DelEvent(Tb_User_Notify Notify)
        {
            var result = ActionJsonResult.Create();
            if (Notify!=null) //编辑
            {
                using (var db = DbHelper.Create())
                {
                    //查找用户是否存在
                    var userNotify = db.Tb_User_Notify.FirstOrDefault(u => u.UserID == Notify.UserID&&u.EventTypeID==Notify.EventTypeID);
                    if (userNotify == null)
                    {
                        result.Failed("无法查询到指定的用户.");
                    }
                    else
                    {
                        try
                        {
                            db.Tb_User_Notify.Remove(userNotify);
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
        /// 分页查询 TVM(分配)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetTVMList()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            int UserType=0;
            int UserId = 0;
            var o = new object();
            List<V_TVM_FP> list=new List<V_TVM_FP>();
            using (var db = DbHelper.Create())
            {
                var result = ActionJsonResult.Create();

                if (!string.IsNullOrEmpty(Request["UserId"]))
                {
                    UserId = Convert.ToInt32(Request["UserId"]);
                }
                if (!string.IsNullOrEmpty(Request["UserType"]))
                {
                    UserType = Convert.ToInt32(Request["UserType"]);
                }

                if (UserType <= 1)//判断用户权限加载数据
                {
                    var list2 = db.Tb_Users_TVM.Where(a => a.UserID == UserId).ToList();//指定用户授权的设备
                    var list1 = db.Tb_Device_TVM.ToList();   ///全部设备
                    for (int i = 0; i < list1.Count; i++)
                    {
                        V_TVM_FP DTvm = new V_TVM_FP();
                        DTvm.DeviceID = list1[i].DeviceID;
                        //DTvm.DeviceName = list1[i].DeviceName;
                        DTvm.DeviceSN = list1[i].DeviceSN;
                        if (list2.Count > 0) {
                            foreach (var item in list2)
                            {
                                if (item.DeviceID == list1[i].DeviceID)
                                {
                                    // DTvm.UserID = Convert.ToInt32(item.UserID);
                                    DTvm.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID + "*" + item.UserID;
                                    break;
                                }
                                else
                                {
                                    DTvm.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID;
                                }
                            }
                        } else {
                            DTvm.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID;
                        }
                        
                        list.Add(DTvm);
                    }


                }
                else {
                    if (UserId > 0) {
                        var list1 = db.V_TVM_FP.Where(a => a.UserID == UserId).ToList();

                        for (int i = 0; i < list1.Count; i++)
                        {
                            V_TVM_FP TVM = new V_TVM_FP();
                            TVM.DeviceID = list1[i].DeviceID;
                            //DTvm.DeviceName = list1[i].DeviceName;
                            TVM.DeviceSN = list1[i].DeviceSN;
                            TVM.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID + "*" + list1[i].UserID;
                            list.Add(TVM);
                        }


                    } else {

                    }
                    
                }
                
                var recordsTotal = list.Count();
                //var psr = new PageSearchResult<List<DeviceLogs>>(list.Count, list);
                var data = list.OrderBy(a=>a.DeviceID).Skip(start).Take(rows).ToList();
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
        /// 删除保存TVM
        /// </summary>
        /// <param name="Notify"></param>
        /// <returns></returns>
        [CheckSession]
        public ActionResult AddAndDelTVM()
        {
            var result = ActionJsonResult.Create();
            var arrayTVM = Request["Array"];
            int UserId = 0;
            if (!string.IsNullOrEmpty(Request["UserID"])) {
                UserId = Convert.ToInt32(Request["UserID"]);
            }
            
                using (var db = DbHelper.Create())
                {
                if (!string.IsNullOrEmpty(arrayTVM))
                {

                    var str = new string[] { };
                    str = arrayTVM.Split(';');
                    //判断删除还是添加
                    for (int i = 0; i < str.Length; i++)
                    {
                        var str2 = new string[] { };
                        str2 = str[i].Split(',');
                        if (str2[1] == "true")
                        {
                            int deviceid = Convert.ToInt32(str2[0]);
                            if (UserId > 0)
                            {
                                try
                                {
                                    //查询是否有相同的记录存在，存在，则更新；否则插入新的
                                    var tmp = db.Tb_Users_TVM.FirstOrDefault((a => a.UserID == UserId && a.DeviceID == deviceid));
                                    if (tmp != null)
                                    {
                                        tmp.UserID = UserId;
                                        tmp.DeviceID = deviceid;

                                        db.Entry(tmp).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        Tb_Users_TVM UserTVM = new Tb_Users_TVM();
                                        UserTVM.UserID = UserId;
                                        UserTVM.DeviceID = deviceid;
                                        //Notify.LastUpdate = DateTime.Now;
                                        db.Tb_Users_TVM.Add(UserTVM);
                                    }
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    result.Failed(e.Message);
                                }
                            }
                            else
                            {
                                result.Failed("无法查询到指定的用户.");
                            }
                        }
                        else
                        {
                            ////删除用户
                            //查询是否有相同的记录存在，存在，则删除
                            int deviceid = Convert.ToInt32(str2[0]);
                            var tmp = db.Tb_Users_TVM.FirstOrDefault((a => a.UserID == UserId && a.DeviceID == deviceid));
                            if (tmp == null)
                            {
                                //result.Failed("无法查询到指定的用户.");
                            }
                            else
                            {
                                try
                                {
                                    db.Tb_Users_TVM.Remove(tmp);
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    result.Failed(e.Message);
                                }
                            }
                        }
                    }
                }
                else {
                    result.Failed("无法查询到指定的用户.");
                }
                
            }
            return result.ToJson();
        }


        /// <summary>
        /// 分页查询 M4Chennel(分配)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetM4Channellist()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            int UserType = 0;
            int UserId = 0;
            var o = new object();
            List<V_M4Chanel_FP> list = new List<V_M4Chanel_FP>();
            using (var db = DbHelper.Create())
            {
                var result = ActionJsonResult.Create();

                if (!string.IsNullOrEmpty(Request["UserId"]))
                {
                    UserId = Convert.ToInt32(Request["UserId"]);
                }
                if (!string.IsNullOrEmpty(Request["UserType"]))
                {
                    UserType = Convert.ToInt32(Request["UserType"]);
                }

                if (UserType <= 1)//判断用户权限加载数据
                {
                    var list2 = db.Tb_Users_M4Channel.Where(a => a.UserID == UserId).ToList();//指定用户授权的设备
                    var list1 = db.Tb_Device_M4Channel.ToList();   ///全部设备
                    for (int i = 0; i < list1.Count; i++)
                    {
                        V_M4Chanel_FP DM4Channel = new V_M4Chanel_FP();
                        DM4Channel.DeviceID = list1[i].DeviceID;
                        //DTvm.DeviceName = list1[i].DeviceName;
                        DM4Channel.DeviceSN = list1[i].DeviceSN;
                        if (list2.Count > 0)
                        {
                            foreach (var item in list2)
                            {
                                if (item.DeviceID == list1[i].DeviceID)
                                {
                                    // DTvm.UserID = Convert.ToInt32(item.UserID);
                                    DM4Channel.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID + "*" + item.UserID;
                                    break;
                                }
                                else
                                {
                                    DM4Channel.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID;
                                }
                            }
                        }
                        else
                        {
                            DM4Channel.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID;
                        }

                        list.Add(DM4Channel);
                    }


                }
                else
                {
                    if (UserId > 0)
                    {
                        var list1 = db.V_M4Chanel_FP.Where(a => a.UserID == UserId).ToList();
                        for (int i = 0; i < list1.Count; i++)
                        {
                            V_M4Chanel_FP M4 = new V_M4Chanel_FP();
                            M4.DeviceID = list1[i].DeviceID;
                            //DTvm.DeviceName = list1[i].DeviceName;
                            M4.DeviceSN = list1[i].DeviceSN;
                            M4.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID + "*" + list1[i].UserID;
                            list.Add(M4);
                        }


                    }
                    else
                    {

                    }

                }

                var recordsTotal = list.Count();
                //var psr = new PageSearchResult<List<DeviceLogs>>(list.Count, list);
                var data = list.OrderBy(a => a.DeviceID).Skip(start).Take(rows).ToList();
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
        /// 删除保存M4Channel
        /// </summary>
        /// <param name="Notify"></param>
        /// <returns></returns>
        [CheckSession]
        public ActionResult AddAndDelM4Channel()
        {
            var result = ActionJsonResult.Create();
            var arrayTVM = Request["Array"];
            int UserId = 0;
            if (!string.IsNullOrEmpty(Request["UserID"]))
            {
                UserId = Convert.ToInt32(Request["UserID"]);
            }

            using (var db = DbHelper.Create())
            {
                if (!string.IsNullOrEmpty(arrayTVM))
                {

                    var str = new string[] { };
                    str = arrayTVM.Split(';');
                    //判断删除还是添加
                    for (int i = 0; i < str.Length; i++)
                    {
                        var str2 = new string[] { };
                        str2 = str[i].Split(',');
                        if (str2[1] == "true")
                        {
                            int deviceid = Convert.ToInt32(str2[0]);
                            if (UserId > 0)
                            {
                                try
                                {
                                    //查询是否有相同的记录存在，存在，则更新；否则插入新的
                                    var tmp = db.Tb_Users_M4Channel.FirstOrDefault((a => a.UserID == UserId && a.DeviceID == deviceid));
                                    if (tmp != null)
                                    {
                                        tmp.UserID = UserId;
                                        tmp.DeviceID = deviceid;

                                        db.Entry(tmp).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        Tb_Users_M4Channel UserM4 = new Tb_Users_M4Channel();
                                        UserM4.UserID = UserId;
                                        UserM4.DeviceID = deviceid;
                                        //Notify.LastUpdate = DateTime.Now;
                                        db.Tb_Users_M4Channel.Add(UserM4);
                                    }
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    result.Failed(e.Message);
                                }
                            }
                            else
                            {
                                result.Failed("无法查询到指定的用户.");
                            }
                        }
                        else
                        {
                            ////删除用户
                            //查询是否有相同的记录存在，存在，则删除
                            int deviceid = Convert.ToInt32(str2[0]);
                            var tmp = db.Tb_Users_M4Channel.FirstOrDefault((a => a.UserID == UserId && a.DeviceID == deviceid));
                            if (tmp == null)
                            {
                                //result.Failed("无法查询到指定的用户.");
                            }
                            else
                            {
                                try
                                {
                                    db.Tb_Users_M4Channel.Remove(tmp);
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    result.Failed(e.Message);
                                }
                            }
                        }
                    }
                }
                else
                {
                    result.Failed("无法查询到指定的用户.");
                }

            }
            return result.ToJson();
        }

        /// <summary>
        /// 分页查询 Ticket安全检票闸机(分配)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetTicketlist()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            int UserType = 0;
            int UserId = 0;
            var o = new object();
            List<V_FP_Ticket> list = new List<V_FP_Ticket>();
            using (var db = DbHelper.Create())
            {
                var result = ActionJsonResult.Create();

                if (!string.IsNullOrEmpty(Request["UserId"]))
                {
                    UserId = Convert.ToInt32(Request["UserId"]);
                }
                if (!string.IsNullOrEmpty(Request["UserType"]))
                {
                    UserType = Convert.ToInt32(Request["UserType"]);
                }

                if (UserType <= 1)//判断用户权限加载数据
                {
                    var list2 = db.Tb_Users_Ticket.Where(a => a.UserID == UserId).ToList();//指定用户授权的设备
                    var list1 = db.Tb_Device_Ticket.ToList();   ///全部设备
                    for (int i = 0; i < list1.Count; i++)
                    {
                        V_FP_Ticket ticket = new V_FP_Ticket();
                        ticket.DeviceID = list1[i].DeviceID;
                        //DTvm.DeviceName = list1[i].DeviceName;
                        ticket.DeviceSN = list1[i].DeviceSN;
                        if (list2.Count > 0)
                        {
                            foreach (var item in list2)
                            {
                                if (item.DeviceID == list1[i].DeviceID)
                                {
                                    // DTvm.UserID = Convert.ToInt32(item.UserID);
                                    ticket.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID + "*" + item.UserID;
                                    break;
                                }
                                else
                                {
                                    ticket.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID;
                                }
                            }
                        }
                        else
                        {
                            ticket.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID;
                        }

                        list.Add(ticket);
                    }


                }
                else
                {
                    if (UserId > 0)
                    {
                        var list1 = db.V_FP_Ticket.Where(a => a.UserID == UserId).ToList();
                        for (int i = 0; i < list1.Count; i++)
                        {
                            V_FP_Ticket M4 = new V_FP_Ticket();
                            M4.DeviceID = list1[i].DeviceID;
                            //DTvm.DeviceName = list1[i].DeviceName;
                            M4.DeviceSN = list1[i].DeviceSN;
                            M4.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID + "*" + list1[i].UserID;
                            list.Add(M4);
                        }
                    }
                    else
                    {

                    }

                }

                var recordsTotal = list.Count();
                //var psr = new PageSearchResult<List<DeviceLogs>>(list.Count, list);
                var data = list.OrderBy(a => a.DeviceID).Skip(start).Take(rows).ToList();
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
        /// 删除保存Ticket安全检票闸机
        /// </summary>
        /// <param name="Notify"></param>
        /// <returns></returns>
        [CheckSession]
        public ActionResult AddAndDelTicket()
        {
            var result = ActionJsonResult.Create();
            var arrayTicket = Request["Array"];
            int UserId = 0;
            if (!string.IsNullOrEmpty(Request["UserID"]))
            {
                UserId = Convert.ToInt32(Request["UserID"]);
            }

            using (var db = DbHelper.Create())
            {
                if (!string.IsNullOrEmpty(arrayTicket))
                {

                    var str = new string[] { };
                    str = arrayTicket.Split(';');
                    //判断删除还是添加
                    for (int i = 0; i < str.Length; i++)
                    {
                        var str2 = new string[] { };
                        str2 = str[i].Split(',');
                        if (str2[1] == "true")
                        {
                            int deviceid = Convert.ToInt32(str2[0]);
                            if (UserId > 0)
                            {
                                try
                                {
                                    //查询是否有相同的记录存在，存在，则更新；否则插入新的
                                    var tmp = db.Tb_Users_Ticket.FirstOrDefault((a => a.UserID == UserId && a.DeviceID == deviceid));
                                    if (tmp != null)
                                    {
                                        tmp.UserID = UserId;
                                        tmp.DeviceID = deviceid;

                                        db.Entry(tmp).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        Tb_Users_Ticket UserTicket = new Tb_Users_Ticket();
                                        UserTicket.UserID = UserId;
                                        UserTicket.DeviceID = deviceid;
                                        //Notify.LastUpdate = DateTime.Now;
                                        db.Tb_Users_Ticket.Add(UserTicket);
                                    }
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    result.Failed(e.Message);
                                }
                            }
                            else
                            {
                                result.Failed("无法查询到指定的用户.");
                            }
                        }
                        else
                        {
                            ////删除用户
                            //查询是否有相同的记录存在，存在，则删除
                            int deviceid = Convert.ToInt32(str2[0]);
                            var tmp = db.Tb_Users_Ticket.FirstOrDefault((a => a.UserID == UserId && a.DeviceID == deviceid));
                            if (tmp == null)
                            {
                                //result.Failed("无法查询到指定的用户.");
                            }
                            else
                            {
                                try
                                {
                                    db.Tb_Users_Ticket.Remove(tmp);
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    result.Failed(e.Message);
                                }
                            }
                        }
                    }
                }
                else
                {
                    result.Failed("无法查询到指定的用户.");
                }

            }
            return result.ToJson();
        }
        /// <summary>
        /// 分页查询 水公园储物柜(分配)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetParkStorelist()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            int UserType = 0;
            int UserId = 0;
            var o = new object();
            List<V_FP_ParkStore> list = new List<V_FP_ParkStore>();
            using (var db = DbHelper.Create())
            {
                var result = ActionJsonResult.Create();

                if (!string.IsNullOrEmpty(Request["UserId"]))
                {
                    UserId = Convert.ToInt32(Request["UserId"]);
                }
                if (!string.IsNullOrEmpty(Request["UserType"]))
                {
                    UserType = Convert.ToInt32(Request["UserType"]);
                }

                if (UserType <= 1)//判断用户权限加载数据
                {
                    var list2 = db.Tb_Users_ParkStore.Where(a => a.UserID == UserId).ToList();//指定用户授权的设备
                    var list1 = db.Tb_Device_ParkStore.ToList();   ///全部设备
                    for (int i = 0; i < list1.Count; i++)
                    {
                        V_FP_ParkStore parkStore = new V_FP_ParkStore();
                        parkStore.DeviceID = list1[i].DeviceID;
                        //DTvm.DeviceName = list1[i].DeviceName;
                        parkStore.DeviceSN = list1[i].DeviceSN;
                        if (list2.Count > 0)
                        {
                            foreach (var item in list2)
                            {
                                if (item.DeviceID == list1[i].DeviceID)
                                {
                                    // DTvm.UserID = Convert.ToInt32(item.UserID);
                                    parkStore.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID + "*" + item.UserID;
                                    break;
                                }
                                else
                                {
                                    parkStore.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID;
                                }
                            }
                        }
                        else
                        {
                            parkStore.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID;
                        }

                        list.Add(parkStore);
                    }


                }
                else
                {
                    if (UserId > 0)
                    {
                        var list1 = db.V_FP_ParkStore.Where(a => a.UserID == UserId).ToList();
                        for (int i = 0; i < list1.Count; i++)
                        {
                            V_FP_ParkStore ParkStore = new V_FP_ParkStore();
                            ParkStore.DeviceID = list1[i].DeviceID;
                            //DTvm.DeviceName = list1[i].DeviceName;
                            ParkStore.DeviceSN = list1[i].DeviceSN;
                            ParkStore.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID + "*" + list1[i].UserID;
                            list.Add(ParkStore);
                        }

                    }
                    else
                    {

                    }

                }

                var recordsTotal = list.Count();
                //var psr = new PageSearchResult<List<DeviceLogs>>(list.Count, list);
                var data = list.OrderBy(a => a.DeviceID).Skip(start).Take(rows).ToList();
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
        /// 删除保存 水公园储物柜(分配)
        /// </summary>
        /// <param name="Notify"></param>
        /// <returns></returns>
        [CheckSession]
        public ActionResult AddAndDelParkStore()
        {
            var result = ActionJsonResult.Create();
            var arrayTicket = Request["Array"];
            int UserId = 0;
            if (!string.IsNullOrEmpty(Request["UserID"]))
            {
                UserId = Convert.ToInt32(Request["UserID"]);
            }

            using (var db = DbHelper.Create())
            {
                if (!string.IsNullOrEmpty(arrayTicket))
                {

                    var str = new string[] { };
                    str = arrayTicket.Split(';');
                    //判断删除还是添加
                    for (int i = 0; i < str.Length; i++)
                    {
                        var str2 = new string[] { };
                        str2 = str[i].Split(',');
                        if (str2[1] == "true")
                        {
                            int deviceid = Convert.ToInt32(str2[0]);
                            if (UserId > 0)
                            {
                                try
                                {
                                    //查询是否有相同的记录存在，存在，则更新；否则插入新的
                                    var tmp = db.Tb_Users_ParkStore.FirstOrDefault((a => a.UserID == UserId && a.DeviceID == deviceid));
                                    if (tmp != null)
                                    {
                                        tmp.UserID = UserId;
                                        tmp.DeviceID = deviceid;

                                        db.Entry(tmp).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        Tb_Users_ParkStore UserParkStore = new Tb_Users_ParkStore();
                                        UserParkStore.UserID = UserId;
                                        UserParkStore.DeviceID = deviceid;
                                        //Notify.LastUpdate = DateTime.Now;
                                        db.Tb_Users_ParkStore.Add(UserParkStore);
                                    }
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    result.Failed(e.Message);
                                }
                            }
                            else
                            {
                                result.Failed("无法查询到指定的用户.");
                            }
                        }
                        else
                        {
                            ////删除用户
                            //查询是否有相同的记录存在，存在，则删除
                            int deviceid = Convert.ToInt32(str2[0]);
                            var tmp = db.Tb_Users_ParkStore.FirstOrDefault((a => a.UserID == UserId && a.DeviceID == deviceid));
                            if (tmp == null)
                            {
                                //result.Failed("无法查询到指定的用户.");
                            }
                            else
                            {
                                try
                                {
                                    db.Tb_Users_ParkStore.Remove(tmp);
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    result.Failed(e.Message);
                                }
                            }
                        }
                    }
                }
                else
                {
                    result.Failed("无法查询到指定的用户.");
                }

            }
            return result.ToJson();
        }
        /// <summary>
        /// 分页查询 自助储物柜(分配)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetSelfCabinetlist()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            int UserType = 0;
            int UserId = 0;
            var o = new object();
            List<V_FP_SelfCabinet> list = new List<V_FP_SelfCabinet>();
            using (var db = DbHelper.Create())
            {
                var result = ActionJsonResult.Create();

                if (!string.IsNullOrEmpty(Request["UserId"]))
                {
                    UserId = Convert.ToInt32(Request["UserId"]);
                }
                if (!string.IsNullOrEmpty(Request["UserType"]))
                {
                    UserType = Convert.ToInt32(Request["UserType"]);
                }

                if (UserType <= 1)//判断用户权限加载数据
                {
                    var list2 = db.Tb_Users_SelfCabinet.Where(a => a.UserID == UserId).ToList();//指定用户授权的设备
                    var list1 = db.Tb_Device_SelfCabinet.ToList();   ///全部设备
                    for (int i = 0; i < list1.Count; i++)
                    {
                        V_FP_SelfCabinet SelfCabinet = new V_FP_SelfCabinet();
                        SelfCabinet.DeviceID = list1[i].DeviceID;
                        //DTvm.DeviceName = list1[i].DeviceName;
                        SelfCabinet.DeviceSN = list1[i].DeviceSN;
                        if (list2.Count > 0)
                        {
                            foreach (var item in list2)
                            {
                                if (item.DeviceID == list1[i].DeviceID)
                                {
                                    // DTvm.UserID = Convert.ToInt32(item.UserID);
                                    SelfCabinet.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID + "*" + item.UserID;
                                    break;
                                }
                                else
                                {
                                    SelfCabinet.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID;
                                }
                            }
                        }
                        else
                        {
                            SelfCabinet.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID;
                        }

                        list.Add(SelfCabinet);
                    }


                }
                else
                {
                    if (UserId > 0)
                    {
                        var list1 = db.V_FP_SelfCabinet.Where(a => a.UserID == UserId).ToList();
                        for (int i = 0; i < list1.Count; i++)
                        {
                            V_FP_SelfCabinet SelfCabinet = new V_FP_SelfCabinet();
                            SelfCabinet.DeviceID = list1[i].DeviceID;
                            //DTvm.DeviceName = list1[i].DeviceName;
                            SelfCabinet.DeviceSN = list1[i].DeviceSN;
                            SelfCabinet.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID + "*" + list1[i].UserID;
                            list.Add(SelfCabinet);
                        }
                    }
                    else
                    {

                    }

                }

                var recordsTotal = list.Count();
                //var psr = new PageSearchResult<List<DeviceLogs>>(list.Count, list);
                var data = list.OrderBy(a => a.DeviceID).Skip(start).Take(rows).ToList();
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
        /// 删除保存 自助储物柜(分配)
        /// </summary>
        /// <param name="Notify"></param>
        /// <returns></returns>
        [CheckSession]
        public ActionResult AddAndDelSelfCabinet()
        {
            var result = ActionJsonResult.Create();
            var arraySelfCab = Request["Array"];
            int UserId = 0;
            if (!string.IsNullOrEmpty(Request["UserID"]))
            {
                UserId = Convert.ToInt32(Request["UserID"]);
            }

            using (var db = DbHelper.Create())
            {
                if (!string.IsNullOrEmpty(arraySelfCab))
                {

                    var str = new string[] { };
                    str = arraySelfCab.Split(';');
                    //判断删除还是添加
                    for (int i = 0; i < str.Length; i++)
                    {
                        var str2 = new string[] { };
                        str2 = str[i].Split(',');
                        if (str2[1] == "true")
                        {
                            int deviceid = Convert.ToInt32(str2[0]);
                            if (UserId > 0)
                            {
                                try
                                {
                                    //查询是否有相同的记录存在，存在，则更新；否则插入新的
                                    var tmp = db.Tb_Users_SelfCabinet.FirstOrDefault((a => a.UserID == UserId && a.DeviceID == deviceid));
                                    if (tmp != null)
                                    {
                                        tmp.UserID = UserId;
                                        tmp.DeviceID = deviceid;

                                        db.Entry(tmp).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        Tb_Users_SelfCabinet UserSelfCab = new Tb_Users_SelfCabinet();
                                        UserSelfCab.UserID = UserId;
                                        UserSelfCab.DeviceID = deviceid;
                                        //Notify.LastUpdate = DateTime.Now;
                                        db.Tb_Users_SelfCabinet.Add(UserSelfCab);
                                    }
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    result.Failed(e.Message);
                                }
                            }
                            else
                            {
                                result.Failed("无法查询到指定的用户.");
                            }
                        }
                        else
                        {
                            ////删除用户
                            //查询是否有相同的记录存在，存在，则删除
                            int deviceid = Convert.ToInt32(str2[0]);
                            var tmp = db.Tb_Users_SelfCabinet.FirstOrDefault((a => a.UserID == UserId && a.DeviceID == deviceid));
                            if (tmp == null)
                            {
                                //result.Failed("无法查询到指定的用户.");
                            }
                            else
                            {
                                try
                                {
                                    db.Tb_Users_SelfCabinet.Remove(tmp);
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    result.Failed(e.Message);
                                }
                            }
                        }
                    }
                }
                else
                {
                    result.Failed("无法查询到指定的用户.");
                }

            }
            return result.ToJson();
        }

        /// <summary>
        /// 分页查询 DVR(分配)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetDVRlist()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            int UserType = 0;
            int UserId = 0;
            var o = new object();
            List<V_FP_DVR> list = new List<V_FP_DVR >();
            using (var db = DbHelper.Create())
            {
                var result = ActionJsonResult.Create();

                if (!string.IsNullOrEmpty(Request["UserId"]))
                {
                    UserId = Convert.ToInt32(Request["UserId"]);
                }

                if (!string.IsNullOrEmpty(Request["UserType"]))
                {
                    UserType = Convert.ToInt32(Request["UserType"]);
                }
                if (UserType <= 1)//判断用户权限加载数据
                {
                    var list2 = db.Tb_Users_DVR.Where(a => a.UserID == UserId).ToList();//指定用户授权的设备
                    var list1 = db.Tb_Device_DVR.ToList();   ///全部设备
                    for (int i = 0; i < list1.Count; i++)
                    {
                        V_FP_DVR vDVR = new V_FP_DVR();
                        vDVR.DeviceID = list1[i].DeviceID;
                        //DTvm.DeviceName = list1[i].DeviceName;
                        //vDVR.DeviceSN = list1[i].DeviceSN;
                        if (list2.Count > 0)
                        {
                            foreach (var item in list2)
                            {
                                if (item.DeviceID == list1[i].DeviceID)
                                {
                                    // DTvm.UserID = Convert.ToInt32(item.UserID);
                                    vDVR.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID + "*" + item.UserID;
                                    break;
                                }
                                else
                                {
                                    vDVR.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID;
                                }
                            }
                        }
                        else
                        {
                            vDVR.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID;
                        }

                        list.Add(vDVR);
                    }


                }
                else
                {
                    if (UserId > 0)
                    {
                        var list1 = db.V_FP_DVR.Where(a => a.UserID == UserId).ToList();
                        for (int i = 0; i < list1.Count; i++)
                        {
                            V_FP_DVR DVR = new V_FP_DVR();
                            DVR.DeviceID = list1[i].DeviceID;
                            DVR.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID + "*" + list1[i].UserID;
                            list.Add(DVR);
                        }

                    }
                    else
                    {

                    }

                }

                var recordsTotal = list.Count();
                //var psr = new PageSearchResult<List<DeviceLogs>>(list.Count, list);
                var data = list.OrderBy(a => a.DeviceID).Skip(start).Take(rows).ToList();
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
        /// 删除保存 DVR(分配)
        /// </summary>
        /// <param name="Notify"></param>
        /// <returns></returns>
        [CheckSession]
        public ActionResult AddAndDelDVR()
        {
            var result = ActionJsonResult.Create();
            var arrayDVR = Request["Array"];
            int UserId = 0;
            if (!string.IsNullOrEmpty(Request["UserID"]))
            {
                UserId = Convert.ToInt32(Request["UserID"]);
            }

            using (var db = DbHelper.Create())
            {
                if (!string.IsNullOrEmpty(arrayDVR))
                {

                    var str = new string[] { };
                    str = arrayDVR.Split(';');
                    //判断删除还是添加
                    for (int i = 0; i < str.Length; i++)
                    {
                        var str2 = new string[] { };
                        str2 = str[i].Split(',');
                        if (str2[1] == "true")
                        {
                            int deviceid = Convert.ToInt32(str2[0]);
                            if (UserId > 0)
                            {
                                try
                                {
                                    //查询是否有相同的记录存在，存在，则更新；否则插入新的
                                    var tmp = db.Tb_Users_DVR.FirstOrDefault((a => a.UserID == UserId && a.DeviceID == deviceid));
                                    if (tmp != null)
                                    {
                                        tmp.UserID = UserId;
                                        tmp.DeviceID = deviceid;

                                        db.Entry(tmp).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        Tb_Users_DVR UserDVR = new Tb_Users_DVR();
                                        UserDVR.UserID = UserId;
                                        UserDVR.DeviceID = deviceid;
                                        //Notify.LastUpdate = DateTime.Now;
                                        db.Tb_Users_DVR.Add(UserDVR);
                                    }
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    result.Failed(e.Message);
                                }
                            }
                            else
                            {
                                result.Failed("无法查询到指定的用户.");
                            }
                        }
                        else
                        {
                            ////删除用户
                            //查询是否有相同的记录存在，存在，则删除
                            int deviceid = Convert.ToInt32(str2[0]);
                            var tmp = db.Tb_Users_DVR.FirstOrDefault((a => a.UserID == UserId && a.DeviceID == deviceid));
                            if (tmp == null)
                            {
                                //result.Failed("无法查询到指定的用户.");
                            }
                            else
                            {
                                try
                                {
                                    db.Tb_Users_DVR.Remove(tmp);
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    result.Failed(e.Message);
                                }
                            }
                        }
                    }
                }
                else
                {
                    result.Failed("无法查询到指定的用户.");
                }

            }
            return result.ToJson();
        }

        /// <summary>
        /// 分页查询 LED(分配)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetLEDlist()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            int UserType = 0;
            int UserId = 0;
            var o = new object();
            List<V_FP_LED> list = new List<V_FP_LED>();
            using (var db = DbHelper.Create())
            {
                var result = ActionJsonResult.Create();

                if (!string.IsNullOrEmpty(Request["UserId"]))
                {
                    UserId = Convert.ToInt32(Request["UserId"]);
                }
                if (!string.IsNullOrEmpty(Request["UserType"]))
                {
                    UserType = Convert.ToInt32(Request["UserType"]);
                }

                if (UserType <= 1)//判断用户权限加载数据
                {
                    var list2 = db.Tb_Users_LED.Where(a => a.UserID == UserId).ToList();//指定用户授权的设备
                    var list1 = db.Tb_Device_LED.ToList();   ///全部设备
                    for (int i = 0; i < list1.Count; i++)
                    {
                        V_FP_LED vLED = new V_FP_LED();
                        vLED.DeviceID = list1[i].DeviceID;
                        //DTvm.DeviceName = list1[i].DeviceName;
                        //vLED.DeviceSN = list1[i].DeviceSN;
                        if (list2.Count > 0)
                        {
                            foreach (var item in list2)
                            {
                                if (item.DeviceID == list1[i].DeviceID)
                                {
                                    // DTvm.UserID = Convert.ToInt32(item.UserID);
                                    vLED.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID + "*" + item.UserID;
                                    break;
                                }
                                else
                                {
                                    vLED.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID;
                                }
                            }
                        }
                        else
                        {
                            vLED.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID;
                        }

                        list.Add(vLED);
                    }


                }
                else
                {
                    if (UserId > 0)
                    {
                        var list1 = db.V_FP_LED.Where(a => a.UserID == UserId).ToList();
                        for (int i = 0; i < list1.Count; i++)
                        {
                            V_FP_LED LED = new V_FP_LED();
                            LED.DeviceID = list1[i].DeviceID;
                            LED.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID + "*" + list1[i].UserID;
                            list.Add(LED);
                        }
                    }
                    else
                    {

                    }

                }

                var recordsTotal = list.Count();
                //var psr = new PageSearchResult<List<DeviceLogs>>(list.Count, list);
                var data = list.OrderBy(a => a.DeviceID).Skip(start).Take(rows).ToList();
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
        /// 删除保存 LED(分配)
        /// </summary>
        /// <param name="Notify"></param>
        /// <returns></returns>
        [CheckSession]
        public ActionResult AddAndDelLED()
        {
            var result = ActionJsonResult.Create();
            var arrayLED = Request["Array"];
            int UserId = 0;
            if (!string.IsNullOrEmpty(Request["UserID"]))
            {
                UserId = Convert.ToInt32(Request["UserID"]);
            }

            using (var db = DbHelper.Create())
            {
                if (!string.IsNullOrEmpty(arrayLED))
                {

                    var str = new string[] { };
                    str = arrayLED.Split(';');
                    //判断删除还是添加
                    for (int i = 0; i < str.Length; i++)
                    {
                        var str2 = new string[] { };
                        str2 = str[i].Split(',');
                        if (str2[1] == "true")
                        {
                            int deviceid = Convert.ToInt32(str2[0]);
                            if (UserId > 0)
                            {
                                try
                                {
                                    //查询是否有相同的记录存在，存在，则更新；否则插入新的
                                    var tmp = db.Tb_Users_LED.FirstOrDefault((a => a.UserID == UserId && a.DeviceID == deviceid));
                                    if (tmp != null)
                                    {
                                        tmp.UserID = UserId;
                                        tmp.DeviceID = deviceid;

                                        db.Entry(tmp).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        Tb_Users_LED UserLED = new Tb_Users_LED();
                                        UserLED.UserID = UserId;
                                        UserLED.DeviceID = deviceid;
                                        //Notify.LastUpdate = DateTime.Now;
                                        db.Tb_Users_LED.Add(UserLED);
                                    }
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    result.Failed(e.Message);
                                }
                            }
                            else
                            {
                                result.Failed("无法查询到指定的用户.");
                            }
                        }
                        else
                        {
                            ////删除用户
                            //查询是否有相同的记录存在，存在，则删除
                            int deviceid = Convert.ToInt32(str2[0]);
                            var tmp = db.Tb_Users_LED.FirstOrDefault((a => a.UserID == UserId && a.DeviceID == deviceid));
                            if (tmp == null)
                            {
                                //result.Failed("无法查询到指定的用户.");
                            }
                            else
                            {
                                try
                                {
                                    db.Tb_Users_LED.Remove(tmp);
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    result.Failed(e.Message);
                                }
                            }
                        }
                    }
                }
                else
                {
                    result.Failed("无法查询到指定的用户.");
                }

            }
            return result.ToJson();
        }

        /// <summary>
        /// 分页查询 eSensor客流计(分配)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GeteSensorlist()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            int UserType = 0;
            int UserId = 0;
            var o = new object();
            List<V_FP_eSensor> list = new List<V_FP_eSensor>();
            using (var db = DbHelper.Create())
            {
                var result = ActionJsonResult.Create();

                if (!string.IsNullOrEmpty(Request["UserId"]))
                {
                    UserId = Convert.ToInt32(Request["UserId"]);
                }
                  if (!string.IsNullOrEmpty(Request["UserType"]))
                {
                    UserType = Convert.ToInt32(Request["UserType"]);
                }

                if (UserType <= 1)//判断用户权限加载数据
                {
                    var list2 = db.Tb_Users_eSensor.Where(a => a.UserID == UserId).ToList();//指定用户授权的设备
                    var list1 = db.Tb_Device_eSensor.ToList();   ///全部设备
                    for (int i = 0; i < list1.Count; i++)
                    {
                        V_FP_eSensor eSensor = new V_FP_eSensor();
                        eSensor.DeviceID = list1[i].DeviceID;
                        //DTvm.DeviceName = list1[i].DeviceName;
                        //eSensor.DeviceSN = list1[i].DeviceSN;
                        if (list2.Count > 0)
                        {
                            foreach (var item in list2)
                            {
                                if (item.DeviceID == list1[i].DeviceID)
                                {
                                    // DTvm.UserID = Convert.ToInt32(item.UserID);
                                    eSensor.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID + "*" + item.UserID;
                                    break;
                                }
                                else
                                {
                                    eSensor.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID;
                                }
                            }
                        }
                        else
                        {
                            eSensor.DeviceName = list1[i].DeviceName + "*" + list1[i].DeviceID;
                        }

                        list.Add(eSensor);
                    }


                }
                else
                {
                    if (UserId > 0)
                    {
                        list = db.V_FP_eSensor.Where(a => a.UserID == UserId).ToList();
                        for (int i = 0; i < list.Count; i++)
                        {
                            V_FP_eSensor eSensor = new V_FP_eSensor();
                            eSensor.DeviceID = list[i].DeviceID;
                            //DTvm.DeviceName = list1[i].DeviceName;
                            //eSensor.DeviceSN = list[i].DeviceSN;
                            eSensor.DeviceName = list[i].DeviceName + "*" + list[i].DeviceID + "*" + list[i].UserID;
                            list.Add(eSensor);
                        }
                       
                    }
                    else
                    {

                    }

                }

                var recordsTotal = list.Count();
                //var psr = new PageSearchResult<List<DeviceLogs>>(list.Count, list);
                var data = list.OrderBy(a => a.DeviceID).Skip(start).Take(rows).ToList();
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
        /// 删除保存 eSensor客流计(分配)
        /// </summary>
        /// <param name="Notify"></param>
        /// <returns></returns>
        [CheckSession]
        public ActionResult AddAndDeleSensor()
        {
            var result = ActionJsonResult.Create();
            var arrayeSensor = Request["Array"];
            int UserId = 0;
            if (!string.IsNullOrEmpty(Request["UserID"]))
            {
                UserId = Convert.ToInt32(Request["UserID"]);
            }

            using (var db = DbHelper.Create())
            {
                if (!string.IsNullOrEmpty(arrayeSensor))
                {

                    var str = new string[] { };
                    str = arrayeSensor.Split(';');
                    //判断删除还是添加
                    for (int i = 0; i < str.Length; i++)
                    {
                        var str2 = new string[] { };
                        str2 = str[i].Split(',');
                        if (str2[1] == "true")
                        {
                            int deviceid = Convert.ToInt32(str2[0]);
                            if (UserId > 0)
                            {
                                try
                                {
                                    //查询是否有相同的记录存在，存在，则更新；否则插入新的
                                    var tmp = db.Tb_Users_eSensor.FirstOrDefault((a => a.UserID == UserId && a.DeviceID == deviceid));
                                    if (tmp != null)
                                    {
                                        tmp.UserID = UserId;
                                        tmp.DeviceID = deviceid;

                                        db.Entry(tmp).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        Tb_Users_eSensor UsereSensor = new Tb_Users_eSensor();
                                        UsereSensor.UserID = UserId;
                                        UsereSensor.DeviceID = deviceid;
                                        //Notify.LastUpdate = DateTime.Now;
                                        db.Tb_Users_eSensor.Add(UsereSensor);
                                    }
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    result.Failed(e.Message);
                                }
                            }
                            else
                            {
                                result.Failed("无法查询到指定的用户.");
                            }
                        }
                        else
                        {
                            ////删除用户
                            //查询是否有相同的记录存在，存在，则删除
                            int deviceid = Convert.ToInt32(str2[0]);
                            var tmp = db.Tb_Users_Ticket.FirstOrDefault((a => a.UserID == UserId && a.DeviceID == deviceid));
                            if (tmp == null)
                            {
                                //result.Failed("无法查询到指定的用户.");
                            }
                            else
                            {
                                try
                                {
                                    db.Tb_Users_Ticket.Remove(tmp);
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    result.Failed(e.Message);
                                }
                            }
                        }
                    }
                }
                else
                {
                    result.Failed("无法查询到指定的用户.");
                }

            }
            return result.ToJson();
        }

        #endregion
    }
}