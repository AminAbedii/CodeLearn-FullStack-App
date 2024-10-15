using CodeLearn.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeLearn.Web.Pages.Admin
{

    public class IndexModel : PageModel
    {
        [PermissionChecker(1)]
        public void OnGet()
        {
        }
    }
}
