
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Portal.Resources;
using Portal.Constant;
using Portal.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Portal.Models;
using System.Data;

namespace Microsoft.AspNetCore.Html
{
    public static class HtmlExtensions
    {
        #region Get Current Url
        public static string GetCurrentUrl(string CurrentArea, string CurrentController)
        {
            string CurrentUrl = "";
            if (!string.IsNullOrEmpty(CurrentArea))
            {
                CurrentUrl = string.Format("{0}/{1}", CurrentArea, CurrentController);
            }
            else
            {
                CurrentUrl = CurrentController;
            }
            return CurrentUrl;
        }
        private static String RenderHtml(TagBuilder aTag)
        {
            using (var writer = new StringWriter())
            {
                aTag.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                return writer.ToString();
            }
        }
        #endregion Get Current Url
        #region Search Button
        public static HtmlString SearchButton(object htmlAttributes = null)
        {
            TagBuilder aTag = new TagBuilder("a");
            aTag.Attributes.Add("id", "btn-search");
            aTag.Attributes.Add("class", "btn btn-primary btn-search");
            aTag.InnerHtml.AppendHtmlLine(string.Format("<i class='bi bi-search'></i> {0}", ""));
            aTag.InnerHtml.AppendHtmlLine(string.Format("<span class='spinner-border spinner-border-sm d-none' role='status' aria-hidden='true'></span> {0}", LanguageResource.Btn_Search));
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                aTag.MergeAttributes(attributes, true);
            }
            aTag.RenderSelfClosingTag();
            return new HtmlString(RenderHtml(aTag)); ;
        }
        #endregion Search Button
        #region Create Button
        public static HtmlString CreateButton(string areaName, string controlName, object htmlAttributes = null,string action = "Upsert") {
            string CurrentUrl = GetCurrentUrl(areaName, controlName);
            //string roles = controlName + ":" + ConstFunction.Create;
            //bool isHasPermission = Function.GetPermission(listRoles, roles);
            //IHtmlContent
            TagBuilder aTag = new TagBuilder("a");
            aTag.Attributes.Add("href", string.Format("/{0}/{1}", CurrentUrl, action));
            aTag.Attributes.Add("id", "btn-create");
            aTag.Attributes.Add("class", "btn btn-primary");
            aTag.InnerHtml.AppendHtmlLine(string.Format("<i class='bi bi-plus-circle'></i> {0}", LanguageResource.Btn_Create));
            if (htmlAttributes != null) {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                aTag.MergeAttributes(attributes, true);
            }
            aTag.RenderSelfClosingTag();


            return new HtmlString(RenderHtml(aTag));

        }

        #endregion Create Button
        public static HtmlString SaveButton(string id, string btn_name, object htmlAttributes = null)
        {
            TagBuilder aTag = new TagBuilder("a");

            aTag.Attributes.Add("id", id);

            aTag.Attributes.Add("class", "btn btn-primary me-1");
            aTag.Attributes.Add("onclick", "$(this).button('loading')");
            aTag.InnerHtml.AppendHtmlLine(string.Format("<i class='fa fa-save'> {0}</i>", btn_name));
            //aTag.InnerHtml.AppendHtmlLine(string.Format("<span class='spinner-border spinner-border-sm d-none' role='status' aria-hidden='true'></span> {0}", btn_name));
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                aTag.MergeAttributes(attributes, true);
            }
            aTag.RenderSelfClosingTag();
            return new HtmlString(RenderHtml(aTag));
        }
        #region Save Button

        #endregion Save Button
        #region Back Button
        public static HtmlString BackButton(string areaName, string controllerName, object htmlAttributes = null)
        {
            //a Tag

            string CurrentUrl = GetCurrentUrl(areaName, controllerName);
            TagBuilder aTag = new TagBuilder("a");
            aTag.Attributes.Add("href", string.Format("/{0}/index", CurrentUrl));
            aTag.Attributes.Add("class", "btn btn-warning-light");
            aTag.InnerHtml.AppendHtmlLine(string.Format("<i class='text-white bi bi-reply-all-fill me-1'></i> {0}", LanguageResource.Btn_Back));
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                aTag.MergeAttributes(attributes, true);
            }
            aTag.RenderSelfClosingTag();
            return new HtmlString(RenderHtml(aTag));
        }
        #endregion Back Button

        #region Edit Button
        public static HtmlString UpsertButton(string areaName, string controllerName, Guid id, object htmlAttributes = null,String listRoles ="")
        {
            string CurrentUrl = GetCurrentUrl(areaName, controllerName);
            string roles = controllerName + ":" + ConstFunction.Upsert;
            bool isHasPermission = Function.GetPermission(listRoles, roles);
            //bool isHasPermission = true;
            if (isHasPermission)
            {
                TagBuilder aTag = new TagBuilder("a");
                aTag.Attributes.Add("class", "btn btn-icon  btn-info-light me-2 bradius");
                aTag.Attributes.Add("data-bs-toggle", "tooltip");
                aTag.Attributes.Add("title", LanguageResource.Btn_Edit);

                aTag.Attributes.Add("href", string.Format("/{0}/Upsert/{1}", CurrentUrl, id));
                aTag.Attributes.Add("onclick", "$(this).button('loading')");

                aTag.InnerHtml.AppendHtmlLine(string.Format("<i class='fe fe-edit'></i> {0}", "")) ;
                if (htmlAttributes != null)
                {
                    var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                    aTag.MergeAttributes(attributes, true);
                }
                aTag.RenderSelfClosingTag();
                return new HtmlString(RenderHtml(aTag));
            }
            return null;
        }
        public static HtmlString UpsertButton(string areaName, string controllerName, String id, object htmlAttributes = null, String listRoles = "")
        {
            string CurrentUrl = GetCurrentUrl(areaName, controllerName);
            string roles = controllerName + ":" + ConstFunction.Upsert;
            bool isHasPermission = Function.GetPermission(listRoles, roles);
            //bool isHasPermission = true;
            if (isHasPermission)
            {
                TagBuilder aTag = new TagBuilder("a");
                aTag.Attributes.Add("class", "btn btn-icon  btn-info-light me-2 bradius");
                aTag.Attributes.Add("data-bs-toggle", "tooltip");
                aTag.Attributes.Add("title", LanguageResource.Btn_Edit);

                aTag.Attributes.Add("href", string.Format("/{0}/Upsert/{1}", CurrentUrl, id));
                aTag.Attributes.Add("onclick", "$(this).button('loading')");

                aTag.InnerHtml.AppendHtmlLine(string.Format("<i class='fe fe-edit'></i> {0}", ""));
                if (htmlAttributes != null)
                {
                    var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                    aTag.MergeAttributes(attributes, true);
                }
                aTag.RenderSelfClosingTag();
                return new HtmlString(RenderHtml(aTag));
            }
            return null;
        }
        public static HtmlString EditButton(string areaName, string controllerName, String id, object htmlAttributes = null, String listRoles = "")
        {
            string CurrentUrl = GetCurrentUrl(areaName, controllerName);
            string roles = controllerName + ":" + ConstFunction.Edit;
            bool isHasPermission = Function.GetPermission(listRoles, roles);
            //bool isHasPermission = true;
            if (isHasPermission)
            {
                TagBuilder aTag = new TagBuilder("a");
                aTag.Attributes.Add("class", "btn btn-icon  btn-info-light me-2 bradius");
                aTag.Attributes.Add("data-bs-toggle", "tooltip");
                aTag.Attributes.Add("title", LanguageResource.Btn_Edit);

                aTag.Attributes.Add("href", string.Format("/{0}/Edit/{1}", CurrentUrl, id));
                aTag.Attributes.Add("onclick", "$(this).button('loading')");

                aTag.InnerHtml.AppendHtmlLine(string.Format("<i class='fe fe-edit'></i> {0}", ""));
                if (htmlAttributes != null)
                {
                    var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                    aTag.MergeAttributes(attributes, true);
                }
                aTag.RenderSelfClosingTag();
                return new HtmlString(RenderHtml(aTag));
            }
            return null;
        }
        public static HtmlString EditButton(string areaName, string controllerName, Guid id, object htmlAttributes = null, String listRoles = "")
        {
            string CurrentUrl = GetCurrentUrl(areaName, controllerName);
            string roles = controllerName + ":" + ConstFunction.Edit;
            bool isHasPermission = Function.GetPermission(listRoles, roles);
            //bool isHasPermission = true;
            if (isHasPermission)
            {
                TagBuilder aTag = new TagBuilder("a");
                aTag.Attributes.Add("class", "btn btn-icon  btn-info-light me-2 bradius");
                aTag.Attributes.Add("data-bs-toggle", "tooltip");
                aTag.Attributes.Add("title", LanguageResource.Btn_Edit);

                aTag.Attributes.Add("href", string.Format("/{0}/Edit/{1}", CurrentUrl, id));
                aTag.Attributes.Add("onclick", "$(this).button('loading')");

                aTag.InnerHtml.AppendHtmlLine(string.Format("<i class='fe fe-edit'></i> {0}", ""));
                if (htmlAttributes != null)
                {
                    var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                    aTag.MergeAttributes(attributes, true);
                }
                aTag.RenderSelfClosingTag();
                return new HtmlString(RenderHtml(aTag));
            }
            return null;
        }
        #endregion Edit Button

        #region Delete Button
        public static HtmlString DeleteButton(string areaName, string controllerName, string itemName, Guid id, object htmlAttributes = null,String listRoles = "")
        {
            string CurrentUrl = GetCurrentUrl(areaName, controllerName);
            string roles = controllerName + ":" + ConstFunction.Delete;
            bool isHasPermission = Function.GetPermission(listRoles, roles);
           // bool isHasPermission = true;
            if (isHasPermission)
            {
                TagBuilder aTag = new TagBuilder("a");
                //aTag.Attributes.Add("onclick", "$(this).button('loading')");
                aTag.Attributes.Add("class", "btn btn-icon btn-danger-light me-2 bradius");
                aTag.Attributes.Add("id", "btn-delete");
                aTag.Attributes.Add("data-bs-toggle", "tooltip");
                aTag.Attributes.Add("title", LanguageResource.Btn_Del);

                aTag.Attributes.Add("data-id", string.Format("{0}", id));
                aTag.Attributes.Add("data-current-url", string.Format("{0}", CurrentUrl));
                aTag.Attributes.Add("data-item-name", string.Format("{0}", itemName));
                aTag.InnerHtml.AppendHtmlLine(string.Format("<i class=\"fe fe-trash\"></i> {0}", ""));
                if (htmlAttributes != null)
                {
                    var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                    aTag.MergeAttributes(attributes, true);
                }
                aTag.RenderSelfClosingTag();
                return new HtmlString(RenderHtml(aTag));
            }
            return null;
        }
        public static HtmlString DeleteButton(string areaName, string controllerName, string itemName, string id, object htmlAttributes = null, String listRoles = "")
        {
            string CurrentUrl = GetCurrentUrl(areaName, controllerName);
            string roles = controllerName + ":" + ConstFunction.Delete;
            bool isHasPermission = Function.GetPermission(listRoles, roles);
            // bool isHasPermission = true;
            if (isHasPermission)
            {
                TagBuilder aTag = new TagBuilder("a");
                //aTag.Attributes.Add("onclick", "$(this).button('loading')");
                aTag.Attributes.Add("class", "btn btn-icon btn-danger-light me-2 bradius");
                aTag.Attributes.Add("id", "btn-delete");
                aTag.Attributes.Add("data-bs-toggle", "tooltip");
                aTag.Attributes.Add("title", LanguageResource.Btn_Del);

                aTag.Attributes.Add("data-id", string.Format("{0}", id));
                aTag.Attributes.Add("data-current-url", string.Format("{0}", CurrentUrl));
                aTag.Attributes.Add("data-item-name", string.Format("{0}", itemName));
                aTag.InnerHtml.AppendHtmlLine(string.Format("<i class=\"fe fe-trash\"></i> {0}", ""));
                if (htmlAttributes != null)
                {
                    var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                    aTag.MergeAttributes(attributes, true);
                }
                aTag.RenderSelfClosingTag();
                return new HtmlString(RenderHtml(aTag));
            }
            return null;
        }
        #endregion Delete Button

        #region Menu nhan su
        public static HtmlString MenuNhanSu(string areaName, string controllerName, string itemName, string id, String listRoles = "")
        {
            string CurrentUrl = GetCurrentUrl(areaName, controllerName);
            //string roles = controllerName + ":" + ConstFunction.Delete;
            //bool isHasPermission = Function.GetPermission(listRoles, roles);

            

            TagBuilder menu = new TagBuilder("div");
            menu.Attributes.Add("class", "d-flex");

            TagBuilder file_dropdown = new TagBuilder("div");
            file_dropdown.Attributes.Add("class", "ms-auto mt-1 file-dropdown");
            file_dropdown.InnerHtml.AppendHtmlLine("<a href=\"javascript:void(0)\" class=\"text-muted\" data-bs-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\"><i class=\"fa fa-align-justify fs-18\"></i></a>");

            TagBuilder dropdown_menu = new TagBuilder("div");
            dropdown_menu.Attributes.Add("class", "dropdown-menu dropdown-menu-start");

            /// check edit
            string roles = controllerName + ":" + ConstFunction.Edit;
            bool isHasPermission = Function.GetPermission(listRoles, roles);
            //bool isHasPermission = true;
            if (isHasPermission)
            {
                TagBuilder aTagEdit = new TagBuilder("a");
                aTagEdit.Attributes.Add("class", "btn dropdown-item");


                aTagEdit.Attributes.Add("href", string.Format("/{0}/Edit/{1}", CurrentUrl, id));

                aTagEdit.InnerHtml.AppendHtmlLine(string.Format("<i class='fe fe-edit bg-primary-transparent me-2'></i> {0}", LanguageResource.Edit));
                aTagEdit.RenderSelfClosingTag();
                dropdown_menu.InnerHtml.AppendHtmlLine(RenderHtml(aTagEdit));
            }
            roles = controllerName + ":" + ConstFunction.View;
            isHasPermission = Function.GetPermission(listRoles, roles);
            if (isHasPermission) {
                // xem thông tin
                TagBuilder aTagView = new TagBuilder("a");
                aTagView.Attributes.Add("class", "btn dropdown-item");


                aTagView.Attributes.Add("href", string.Format("/{0}/View/{1}", CurrentUrl, id));
                aTagView.InnerHtml.AppendHtmlLine(string.Format("<i class='fa fa-eye bg-info-transparent me-2'></i> {0}", LanguageResource.View));
                aTagView.RenderSelfClosingTag();
                dropdown_menu.InnerHtml.AppendHtmlLine(RenderHtml(aTagView));
            }
            roles = controllerName + ":" + ConstFunction.Print;
            isHasPermission = Function.GetPermission(listRoles, roles);
            if (isHasPermission)
            {
                // In lý lịch
                TagBuilder aTagPrint = new TagBuilder("a");
                aTagPrint.Attributes.Add("class", "btn dropdown-item");


                aTagPrint.Attributes.Add("href", string.Format("/{0}/Print/{1}", CurrentUrl, id));
                aTagPrint.InnerHtml.AppendHtmlLine(string.Format("<i class='fa fa-print bg-success-transparent me-2'></i> {0}", LanguageResource.Print));
                aTagPrint.RenderSelfClosingTag();
                dropdown_menu.InnerHtml.AppendHtmlLine(RenderHtml(aTagPrint));
            }
            // Xóa
            roles = controllerName + ":" + ConstFunction.Delete;
            isHasPermission = Function.GetPermission(listRoles, roles);
            if (isHasPermission)
            {
                TagBuilder aTagDel = new TagBuilder("a");
                //aTag.Attributes.Add("onclick", "$(this).button('loading')");
                aTagDel.Attributes.Add("class", "btn dropdown-item");
                aTagDel.Attributes.Add("id", "btn-delete");

                aTagDel.Attributes.Add("data-id", string.Format("{0}", id));
                aTagDel.Attributes.Add("data-current-url", string.Format("{0}", CurrentUrl));
                aTagDel.Attributes.Add("data-item-name", string.Format("{0}", itemName));
                aTagDel.InnerHtml.AppendHtmlLine(string.Format("<i class=\"fe fe-trash bg-danger-transparent me-2\"></i> {0}", LanguageResource.Btn_Del));
                aTagDel.RenderSelfClosingTag();
                dropdown_menu.InnerHtml.AppendHtmlLine(RenderHtml(aTagDel));
            }
            file_dropdown.InnerHtml.AppendHtmlLine(RenderHtml(dropdown_menu));

            menu.InnerHtml.AppendHtmlLine(RenderHtml(file_dropdown));
            return new HtmlString(RenderHtml(menu));
        }
        #endregion Menu nhan su

        #region Import Button
        public static HtmlString ImportButton(string areaName, string controlName, object htmlAttributes = null,string listRoles = null)
        {

           string roles = controlName + ":" + ConstFunction.Import;
           bool  isHasPermission = Function.GetPermission(listRoles, roles);
            if (!isHasPermission)
            {
                TagBuilder button = new TagBuilder("button");
                button.Attributes.Add("id", "btn-import");
                button.Attributes.Add("class", "btn bg-olive");
                button.Attributes.Add("data-toggle", "modal");
                button.Attributes.Add("data-target", "#importexcel-window");
                button.InnerHtml.AppendHtmlLine(string.Format("<i class='fa fa-upload'></i> {0}", LanguageResource.Btn_Import));
                button.Attributes.Add("disabled", "disabled");
                if (htmlAttributes != null)
                {
                    var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                    button.MergeAttributes(attributes, true);
                }
                new HtmlString(RenderHtml(button));
            }

           
            return null;
        }
        #endregion Import Button
    }
}
