using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Web.EF;
using Web.Models.WebModels;

namespace Web.Commons
{
    public class MenuReader
    {
        private readonly Tb_Users _currentUser;

        public MenuReader(Tb_Users user)
        {
            _currentUser = user;
        }

        private void Build<T>(List<T> items) where T : MenuItem
        {
            if (PermissionItems == null || items == null) return;
            var list = new List<T>();
            list.AddRange(items);
            foreach (var item in list)
            {
                if (!item.Level.Contains(_currentUser.Level.ToString()))
                {
                    items.Remove(item);
                    continue;
                }
                if (!string.IsNullOrEmpty(item.Link))
                {
                    var key = item.Link.ToUpper();
                    if (!PermissionItems.ContainsKey(key))
                    {
                        PermissionItems[key] = item;
                    }
                }
                if (item.Items != null)
                {
                    Build(item.Items);
                }
                if (item.Toolbar != null)
                {
                    Build(item.Toolbar);
                }
            }
        }


        public void LoadMenus(string json)
        {
            Menus = ((JArray)JsonConvert.DeserializeObject(json)).ToObject<List<MenuItem>>();
            if (Menus?.Count > 0)
            {
                PermissionItems = new Dictionary<string,UserPermissionItem>();
                Build(Menus);
            }
        }

        public List<MenuItem> Menus { get; private set; }

        public Dictionary<string,UserPermissionItem> PermissionItems { get; private set; }
    }
}