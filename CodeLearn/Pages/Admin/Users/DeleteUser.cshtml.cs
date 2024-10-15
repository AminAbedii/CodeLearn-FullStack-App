using CodeLearn.Core.DTOs;
using CodeLearn.Core.Security;
using CodeLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(5)]
    public class DeleteUserModel : PageModel
    {
        private IUserService _userService;
        private IPermissionService _permissionService;
        public DeleteUserModel(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }
        public InformationUserViewModel informationUserViewModel { get; set; }
        public void OnGet(int id)
        {
            ViewData["UserId"] = id;
            informationUserViewModel = _userService.GetUserInformation(id);
        }

        public IActionResult OnPost(int UserId) 
        {
            _userService.DeleteUser(UserId);
            return RedirectToPage("Index");
        }
    }
}
