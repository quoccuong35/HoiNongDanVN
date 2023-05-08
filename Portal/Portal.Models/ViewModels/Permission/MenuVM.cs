
using Portal.Models.Entitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Portal.Models
{
    public class Menu
    {
        public Guid? MenuId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MenuCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public String MenuCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MenuName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public String MenuName { get; set; }

        [RegularExpression("([0-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        public int OrderIndex { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Icon")]
        public String? Icon { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; } = true;

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Href")]
        public String? Href { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MenuIdParent")]
        public Guid? MenuIdParent { get; set; }
        public String? Error { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MenuType")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public MenuType MenuType { get; set; } = MenuType.Menu;

    }
    public class MenuVM : Menu
    {
        public List<MenuFunction> FunctionList { get; set; }
        public MenuModel Add(Guid accountId)
        {
            return new MenuModel
            {
                MenuId = Guid.NewGuid(),
                MenuCode = this.MenuCode,
                MenuName = this.MenuName,
                Icon = this.Icon,
                Href = this.Href == null ? "#" : this.Href,
                OrderIndex = this.OrderIndex,
                Actived = true,
                CreatedTime = DateTime.Now,
                CreatedAccountId = accountId,
                MenuType = this.MenuType,
                MenuIdParent = this.MenuIdParent,
                PageFunctionModels = pageFunctionModels()

            };
        }
        public MenuModel Update(Guid accountId, MenuModel edit)
        {
            edit.MenuCode = this.MenuCode;
            edit.MenuName = this.MenuName;
            edit.Icon = this.Icon;
            edit.Href = this.Href == null ? "#" : this.Href;
            edit.MenuIdParent = this.MenuIdParent;
            edit.Actived = this.Actived != null ? this.Actived.Value : true;
            edit.OrderIndex = this.OrderIndex;
            edit.LastModifiedAccountId = accountId;
            edit.LastModifiedTime = DateTime.Now;
            edit.MenuType = this.MenuType;
            edit.PageFunctionModels = pageFunctionModels();
            return edit;
        }

        public List<PageFunctionModel> pageFunctionModels() {
            List<PageFunctionModel> listFunction = new List<PageFunctionModel>();
            var list = FunctionList.Where(it => it.Selected == true);
            foreach (var item in list)
            {
                listFunction.Add(new PageFunctionModel { FunctionId = item.FunctionId });
            }
            return listFunction;
        }

    }
    public class MenuFunction{
        public string FunctionId { get; set; }
        public string? FunctionName { get; set; }
        public bool Selected { get; set; }
    }
}
