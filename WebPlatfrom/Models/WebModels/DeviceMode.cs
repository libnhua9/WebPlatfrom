using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.WebModels
{
    public class DeviceMode
    {
            /// <summary>
            /// 验票设备是否启用
            /// </summary>

            public bool DeviceEnable { get; set; }
            /// <summary>
            /// 通道工作模式
            /// </summary>

            public int ChannelMode { get; set; }
            /// <summary>
            /// 指纹验证等级
            /// </summary>

            public int FingerprintLevel { get; set; }
            /// <summary>
            /// 是否需要拍照
            /// </summary>

            public bool Snapshot { get; set; }
            /// <summary>
            /// 过闸超时值（秒）
            /// </summary>

            public int LockageTimeout { get; set; }
            /// <summary>
            /// 打指纹超时值(秒)
            /// </summary>

            public int FingerprintTimeout { get; set; }
            /// <summary>
            /// 身份证采集超时值(秒)
            /// </summary>

            public int IDTimeout { get; set; }
            /// <summary>
            /// 启用出口模式
            /// </summary>

            public bool ExitMode { get; set; }
            public string Begin { get; set; }
            public string End { get; set; }
            public int Id { get; set;}
    }
}