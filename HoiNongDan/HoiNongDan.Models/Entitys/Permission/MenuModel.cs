using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HoiNongDan.Models.Entitys
{
    public enum MenuType
    {
        Menu = 0,
        Page = 1,
    };
    public class MenuModel
    {
        public Guid MenuId { get; set; }
        public string MenuCode { get; set; }
        public string MenuName { get; set; }
        public int OrderIndex { get; set; }
        public string? Icon { get; set; }
        public bool Actived { get; set; } = true;
        public string Href { get; set; } = "#";
        public Guid? MenuIdParent { get; set; }

        public string? Description { get; set; }
        public Guid CreatedAccountId { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public Guid? LastModifiedAccountId { get; set; }

        public DateTime? LastModifiedTime { get; set; }
        public MenuType MenuType { get; set; } = MenuType.Menu;

        [ForeignKey("MenuIdParent")]
        public ICollection<MenuModel> Children { get; set; }
        public ICollection<PageFunctionModel> PageFunctionModels { get; set; }
        public ICollection<PagePermissionModel> PagePermissionModels { get; set; }
        public MenuModel()
        {
            PageFunctionModels = new List<PageFunctionModel>();
            PagePermissionModels = new List<PagePermissionModel>();
        }
    }
}
