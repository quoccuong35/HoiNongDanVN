
using System.ComponentModel.DataAnnotations;


namespace Portal.Models
{
    public class AccountLoginViewModel
    {

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "UserName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string UserName { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RememberMe")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        public string errorMessage { get; set; }
    }
}
