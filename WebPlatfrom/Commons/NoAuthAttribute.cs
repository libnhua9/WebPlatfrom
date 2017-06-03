using System;

namespace Web.Commons
{
    /// <summary>
    /// 不检测权限
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class NoCheckAttribute : Attribute
    {

    }

    /// <summary>
    /// 只检测session权限
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CheckSessionAttribute : Attribute
    {

    }
}