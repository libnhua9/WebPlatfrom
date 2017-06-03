using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.WebModels
{
    public class StatusTicket
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }
        ///启动时间
        public DateTime Startup { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress { get; set; }  
        /// <summary>
        /// 过闸人数
        /// </summary>
        public int PassCount { get; set; }
        /// <summary>
        /// 系统类型
        /// </summary>
        public int SystemType { get; set;}
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// 通道模式
        /// </summary>
        public int ChannelMode { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public int DeviceID { get; set; }
        /// <summary>
        /// 坐标
        /// </summary>
        public decimal Lat { get; set; }
        /// <summary>
        /// 坐标
        /// </summary>
        public decimal Lng { get; set; }
    }
}