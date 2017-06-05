using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.EF;
using Web.Commons;
using Web.Models.WebModels;

namespace Web.Controllers
{
    public class AdController : Controller
    {
        // GET: Ad
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LedAd()
        {
            return View();
        }

        public ActionResult LedAdDetailed()
        {
            return View();
        }

        public ActionResult AddLedAd()
        {
            return View();
        }

        public ActionResult LedAdEdit()
        {
            return View();
        }
        public ActionResult LED(string idstr)
        {
            ViewBag.Array = idstr;
            return View();
        }

        #region 保存数据
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSave()
        {
            return null;
        }
        #endregion

        #region 删除
        public ActionResult GetDelete(string arry)
        {
            var result = 0;
            using (var db = DbHelper.Create())
            {
                var number = db.Database.ExecuteSqlCommand("delete tb_ledad where id in(" + arry + ")");
                if (number == 0)
                {
                    result = 1;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 列表绑定数据
        /// <summary>
        /// 列表绑定数据
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        [HttpPost]
        public ActionResult GetLedAdList(AdModel ad)
        {
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            try
            {
                var user = this.Load<Tb_Users>();
                using (var db = DbHelper.Create())
                {
                    IQueryable<Tb_LedAd> ledad = db.Tb_LedAd;
                    if (user.Level > 1)
                    {
                        ledad = ledad.Where(m => m.UserID == user.UserID);
                    }
                    var data = ledad.OrderByDescending(m => m.ID).Skip(start).Take(rows).ToList();
                    var count = ledad.Count();
                    var o = new
                    {
                        draw = draw,
                        recordsTotal = count,
                        recordsFiltered = count,
                        data = data
                    };
                    return Json(o, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                var o = new
                {
                    draw = 0,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = 0
                };
                return Json(o, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region LED

        public ActionResult GetLedList()
        {
            return View();
        }
        #endregion

        #region  设备广告分配
        /// <summary>
        /// 设备广告分配
        /// </summary>
        /// <param name="array">广告分配id</param>
        /// <param name="array2">设备分配id</param>
        /// <returns></returns>
        public ActionResult GetDeviceAdAllot(string array, string array2)
        {


            return null;
        }

        #endregion




        public ActionResult GetSelfCabinet(AdModel ad)
        {
            var start = Convert.ToInt32(Request["start"]);
            var rows = Convert.ToInt32(Request["length"]);
            var draw = Convert.ToInt32(Request["draw"]);
            var user = this.Load<Tb_Users>();
            using (var db = DbHelper.Create())
            {
                var being = DateTime.Parse(ad.Begin + " 00:00:00");
                var end = DateTime.Parse(ad.End + " 23:59:59");
                if (user.Level > 1)
                {
                    var data = from d in db.Tb_Device_SelfCabinet
                               join u in db.Tb_Device_SelfCabinet on d.DeviceID equals u.DeviceID
                               select new
                               {
                                   d.DeviceID,
                                   d.DeviceName,
                                   d.DeviceSN,
                                   d.Enabled,
                                   d.EndDate,
                                   d.StartDate,
                                   d.SaveDate,
                                   d.LastUpdate
                               };

                    if (ad.Begin != null && ad.End != null)
                    {
                        data = data.Where(m => m.SaveDate >= being && m.SaveDate <= end);
                    }
                    if (ad.Name != null)
                    {
                        data = data.Where(m => m.DeviceName.Contains(ad.Name));
                    }
                    data = data.OrderByDescending(m => m.DeviceID).Skip(start).Take(rows);
                    var count = data.Count();
                    var o = new
                    {
                        draw = draw,
                        recordsTotal = count,
                        recordsFiltered = count,
                        data = data
                    };
                    return Json(o, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data = from d in db.Tb_Device_SelfCabinet select d;
                    if (ad.Begin != null && ad.End != null)
                    {
                        data = data.Where(m => m.SaveDate >= being && m.SaveDate <= end);
                    }
                    if (ad.Name != null)
                    {
                        data = data.Where(m => m.DeviceName.Contains(ad.Name));
                    }
                    data = data.OrderByDescending(m => m.DeviceID).Skip(start).Take(rows);
                    var count = data.Count();
                    var o = new
                    {
                        draw = draw,
                        recordsTotal = count,
                        recordsFiltered = count,
                        data = data
                    };
                    return Json(o, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}