using CodeLearn.Core.Security;
using CodeLearn.Core.Services.Interfaces;
using CodeLearn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeLearn.Web.Pages.Admin.Roles
{
    [PermissionChecker(7)]
    public class CreateRoleModel : PageModel
    {
        private IPermissionService _permissionService;
        public CreateRoleModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [BindProperty]
        public Role Role { get; set; }
        public void OnGet()
        {
            ViewData["Permissions"] = _permissionService.GetAllPermission();
        }

        public IActionResult OnPost(List<int> SelectedPermission)
        {
            ModelState.Clear();

            if (!ModelState.IsValid)
                return Page();

            //ToDo Add Role
            Role.IsDelete = false;
            int roleId = _permissionService.AddRole(Role);

            //TODO Add Permission
            _permissionService.AddPermissionToRole(roleId, SelectedPermission);

            return RedirectToPage("Index");
        }
    }
}
