using CodeLearn.Core.Security;
using CodeLearn.Core.Services.Interfaces;
using CodeLearn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeLearn.Web.Pages.Admin.Roles
{
    [PermissionChecker(6)] //baraye check kardane dastresi karbar be admin page ast
    public class IndexModel : PageModel
    {
        private IPermissionService _permissionService;

        public IndexModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public List<Role> RolesList { get; set; }     //saxte listi az role ha baraye estefade dar html file be surate model


        public void OnGet()
        {
            RolesList = _permissionService.GetRoles();
        }
    }
}
