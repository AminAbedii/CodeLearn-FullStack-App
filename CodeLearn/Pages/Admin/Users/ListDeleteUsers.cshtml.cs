using CodeLearn.Core.DTOs;
using CodeLearn.Core.Security;
using CodeLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(10)]
    public class ListDeleteUsersModel : PageModel
    {
        private IUserService _userService;

        public ListDeleteUsersModel(IUserService userService)
        {
            _userService = userService;
        }

        public UserForAdminViewModel UserForAdminViewModel { get; set; }

        public void OnGet(int pageId = 1, string filterUserName = "", string filterEmail = "")
        {
            UserForAdminViewModel = _userService.GetDeleteUsers(pageId, filterEmail, filterUserName);
        }

    }
}
