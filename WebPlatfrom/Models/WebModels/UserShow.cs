using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.WebModels
{
    public class UserShow
    {

    }
    public class EventShow {

        public int ID { get; set; }
        public int UserID { get; set; }
        public Nullable<int> EventTypeID { get; set; }
        public string URL { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }

        public string EventName { get; set; }
    }

    public class M4Channel {
        public string DeviceSN { get; set; }
        public DateTime LastUpdate { get; set; }
        public string IPAddress { get; set; }
        public int M4WorkMode { get; set; }
        public int TotalCount { get; set; }
        public string DeviceName { get; set; }
        public int DeviceID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Enabled { get; set; }
        public int Version { get; set; }
        public Nullable<decimal> Lat { get; set; }
        public Nullable<decimal> Lng { get; set; }
    }


    public class TVM {
        public int DeviceID{ get; set; }
        public string DeviceName{ get; set; }
        public DateTime StartDate{ get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public int Enabled{get; set;}
        public Nullable<decimal> Lat {get; set;}
        public Nullable<decimal> Lng {get; set;}
        public string DeviceSN{get; set;}
        public DateTime LastUpdate{get; set;}
        public string IPAddress{get; set;}
    }
    /// <summary>
    /// 水公园储物柜
    /// </summary>
    public class ParkStore
    {
        public int DeviceID { get; set; }
        public string DeviceName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Enabled { get; set; }
        public Nullable<decimal> Lat { get; set; }
        public Nullable<decimal> Lng { get; set; }
        public string DeviceSN { get; set; }
        public DateTime LastUpdate { get; set; }
        public string IPAddress { get; set; }
        public string DoorStatus { get; set; }
    }

    //D.DeviceID, D.DeviceName,S.DeviceSN, S.LastUpdate, S.IPAddress, S.DoorStatus, S.Config, D.StartDate, D.EndDate, 
    //D.Enabled, D.Lat, D.Lng
    /// <summary>
    /// 自助储物柜
    /// </summary>
    public class SelfCabinet
    {
        public int DeviceID { get; set; }
        public string DeviceName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Enabled { get; set; }
        public Nullable<decimal> Lat { get; set; }
        public Nullable<decimal> Lng { get; set; }
        public string DeviceSN { get; set; }
        public DateTime LastUpdate { get; set; }
        public string IPAddress { get; set; }
        public string DoorStatus { get; set; }
        public string Config { get; set; }
    }


    //S.DeviceID, S.LastUpdate, S.IPAddress,D.DeviceName, D.StartDate, D.EndDate, D.Enabled, D.Port, D.Account, D.PWD, D.ChannelID, D.Remarks, D.Lat, D.Lng
    /// <summary>
    /// 海康DVR
    /// </summary>
    public class DVR
    {
        public int DeviceID { get; set; }
        public string DeviceName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Enabled { get; set; }
        public int Port { get; set; }//端口
        public string Account { get; set; }//账号
        public string PWD { get; set; }//密码
        public int ChannelID { get; set; }//通道号
        public string Remarks { get; set; }
        public Nullable<decimal> Lat { get; set; }
        public Nullable<decimal> Lng { get; set; }
        public DateTime LastUpdate { get; set; }
        public string IPAddress { get; set; }
       
    }

    //S.DeviceID, S.LastUpdate, S.IPAddress,D.DeviceName, D.StartDate, D.EndDate, D.Enabled, D.Port, D.Lat, D.Lng
    /// <summary>
    /// LED显示屏
    /// </summary>
    public class LED
    {
        public int DeviceID { get; set; }
        public string DeviceName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Enabled { get; set; }
        public int Port { get; set; }//端口
        public Nullable<decimal> Lat { get; set; }
        public Nullable<decimal> Lng { get; set; }
        public DateTime LastUpdate { get; set; }
        public string IPAddress { get; set; }

    }


    /// <summary>
    /// 客流量统计
    /// </summary>
    public class eSensor {
        ///D.DeviceSN, D.DeviceName, D.StartDate, D.EndDate, D.Enabled, D.Port, D.Lat, D.Lng,
        //S.DeviceID, S.LastUpdate, S.IPAddress

        public int DeviceID { get; set; }
        public string DeviceSN { get; set; }
        public string DeviceName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Enabled { get; set; }
        public int Port { get; set; }//端口
        public Nullable<decimal> Lat { get; set; }
        public Nullable<decimal> Lng { get; set; }
        public DateTime LastUpdate { get; set; }
        public string IPAddress { get; set; }


    }

    /// <summary>
    /// 设备交互
    /// </summary>
    public class DeviceLogs {
        public int DeviceID { get; set; }
        public string DeviceName { get; set; }
        public int ID { get; set; }
        public string DeviceKey { get; set; }
        public int FunctionID { get; set; }
        public string FunctionName { get; set; }
        public int ElapsedTime { get; set; }
        public DateTime SaveDate { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public int Result { get; set; }
    }

    /// <summary>
    /// LED显示屏交互
    /// </summary>
    public class LEDLogs {
        //D.DeviceName, D.IPAddress, D.Port, D.Lat, D.Lng,L.ID, L.UserID, L.DeviceID, L.FunctionName, L.Request, L.Response, L.Result, L.SaveDate
        public string DeviceName { get; set; }
        public string IPAddress { get; set; }
        public int Port { get; set; }
        public Nullable<decimal> Lat { get; set; }
        public Nullable<decimal> Lng { get; set; }
        public int ID { get; set; }
        public int UserID { get; set; }//开发者Id
        public int DeviceID { get; set; }
        public string FunctionName { get; set; }//调用功能
        public string Request { get; set; }
        public string Response { get; set; }
        public int Result { get; set; }
        public DateTime SaveDate { get; set; }

    }
    /// <summary>
    /// 客流计交互
    /// </summary>
    public class eSensorLogs {
        //D.DeviceSN, D.DeviceName, D.IPAddress, D.Port, D.Lat, D.Lng,L.ID, L.DeviceID, L.EventID, L.In_Count, L.Out_Count, L.SaveDate
        public int ID { get; set; }
        public int DeviceID { get; set; }//客流计ID
        public string DeviceSN { get; set; }//设备序列号
        public string DeviceName { get; set; }
        public string IPAddress { get; set; }
        public int Port { get; set; }
        public Nullable<decimal> Lat { get; set; }
        public Nullable<decimal> Lng { get; set; }
        public int EventID { get; set; }//事件Id
        public int In_Count { get; set; }
        public int Out_Count { get; set; }
        public DateTime SaveDate { get; set; }


    }
        public class FaceAPILogs {
           // L.ID, L.API_ID, L.UserID, L.FunctionName, L.Request, L.Response, L.Result, L.UseTime, L.CallTime,U.UserName
            public int ID { get; set; }
            public int API_ID { get; set; }
            public int UserID { get; set; }
            public string FunctionName { get; set; }
            public string Request { get; set; }
            public string Response { get; set; }
            public int Result { get; set; }
            public int UseTime { get; set; }
            public DateTime CallTime { get; set; }
            public string UserName { get; set; }
            public string ProviderUrl { get; set; }
        }

        public class PayAPILogs
        {
        //L.ID, L.API_ID, L.Customer, L.CustomerType, L.Request, L.Response, L.Result, L.UseTime, L.CallTime,U.UserName,A.APIName,A.ProviderUrl
            public int ID { get; set; }
            public int API_ID { get; set; }
            public string Customer { get; set; }
            public int CustomerType { get; set; }
            public string Request { get; set; }
            public string Response { get; set; }
            public int Result { get; set; }
            public int UseTime { get; set; }
            public DateTime CallTime { get; set; }
            public string UserName { get; set; }
            public string ProviderUrl { get; set; }
            public string APIName { get; set; }
    }

    public class FaceApiData
    {
        public Nullable<int> countElapsedTime { get; set; }
        public Nullable<int> sumElapsedTime { get; set; }
        public Nullable<int> maxElapsedTime { get; set; }
        public Nullable<int> minElapsedTime { get; set; }
        public Nullable<int> avgElapsedTime { get; set; }
        public Nullable<int> SaveDate { get; set; }
        public Nullable<int> UserID { get; set; }
    }


}