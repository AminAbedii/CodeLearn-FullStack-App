using CodeLearn.Core.Security;
using CodeLearn.Core.Services.Interfaces;
using CodeLearn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeLearn.Web.Pages.Admin.Roles
{
    [PermissionChecker(8)]
    public class EditRoleModel : PageModel
    {
        private IPermissionService _permissionService;
        public EditRoleModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [BindProperty]
        public Role Role { get; set; }
        public void OnGet(int id)
        {
            Role = _permissionService.GetRoleById(id);
            ViewData["Permissions"] = _permissionService.GetAllPermission();
            ViewData["SelectedPermission"] = _permissionService.ActivePermissionsRole(id);
        }

        public IActionResult OnPost(List<int> SelectedPermission)
        {
            ModelState.Clear();
            if (!ModelState.IsValid)
                return Page();

            _permissionService.UpdateRole(Role);


            _permissionService.UpdatePermissionsRole(Role.RoleId, SelectedPermission);

            return RedirectToPage("Index");
        }
    }
}
