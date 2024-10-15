using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CodeLearn.Core.Services.Interfaces;
using CodeLearn.Core.DTOs;
using CodeLearn.Core.Security;

namespace CodeLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(2)]
    public class IndexModel : PageModel
    {
        private IUserService _userService;

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        public UserForAdminViewModel UserForAdminViewModel { get; set; }

        public void OnGet(int pageId = 1, string filterUserName = "", string filterEmail = "")
        {
            UserForAdminViewModel = _userService.GetUsers(pageId, filterEmail, filterUserName);
        }


    }
}
