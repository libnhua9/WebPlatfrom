using System.Collections.Generic;

namespace Web.Models.WebModels
{
    /// <summary>
    /// 用户权限的项
    /// </summary>
    public class UserPermissionItem
    {
        /// <summary>
        /// 权限项名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 权限项图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 权限项链接
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 权限项，角色列表
        /// </summary>
        public string Level { get; set; }
    }

    public class MenuItem : UserPermissionItem
    {
        public List<MenuItem> Items { get; set; }

        public List<MenuItem> Toolbar { get; set; }
    }
}