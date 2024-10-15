using CodeLearn.Core.DTOs;
using CodeLearn.Core.Security;
using CodeLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(3)]
    public class CreateUserModel : PageModel
    {
        private IUserService _userService;
        private IPermissionService _permissionService;
        public CreateUserModel(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }


        [BindProperty]  //sefati baraye update kardane etelaat dar saniye
        public CreateUserViewModel CreateUserViewModel { get; set; }
        public void OnGet()
        {
            ViewData["Roles"]=_permissionService.GetRoles();    //view data baraye ferestadan etelaat be view va estefade az anha (mishod az ViewBag ham estefade kard)
        }

        public IActionResult OnPost(List<int> SelectedRoles)
        {
            if(!ModelState.IsValid)
                return Page();
            int userId = _userService.AddUserFromAdmin(CreateUserViewModel);
            
            //AddRoles
            _permissionService.AddRolesToUser(SelectedRoles, userId);
            
            return Redirect("/Admin/Users");
        }
    }
}
