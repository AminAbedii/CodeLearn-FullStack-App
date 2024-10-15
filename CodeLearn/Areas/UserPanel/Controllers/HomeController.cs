using CodeLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using CodeLearn.Core.DTOs;

namespace CodeLearn.Web.Areas.UserPanel.Controllers
{
    public class HomeController : Controller
    {
        private IUserService _userService;
        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        [Area("UserPanel")]
        [Authorize]
        public IActionResult Index()
        {
            return View(_userService.GetUserInformation(User.Identity.Name));
        }

        #region EditProfile

        [Area("UserPanel")]
        [Route("UserPanel/EditProfile")]
        public IActionResult EditProfile() 
        { 
            return View(_userService.GetDataForEditProfileUser(User.Identity.Name));  //baraye inke etelaate gabli user ra dar form neshan dahad
        }

        [Area("UserPanel")]
        [Route("UserPanel/EditProfile")]
        [HttpPost]
        public IActionResult EditProfile(EditProfileModelView profile)
        {
            if (!ModelState.IsValid)
                return View(profile);
            _userService.EditProfile(User.Identity.Name, profile);
            //Log Out User
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/Login?EditProfile=true");
        }
        #endregion

        #region ChangePassword

        [Area("UserPanel")]
        [Route("UserPanel/ChangePassword")]
        [Authorize]
        public IActionResult ChangePassword() 
        {
            return View();
        }

        [Area("UserPanel")]
        [Route("UserPanel/ChangePassword")]
        [HttpPost]
        [Authorize]
        public IActionResult ChangePassword(ChangePasswordViewModel change)
        {
            if (!ModelState.IsValid)
                return View(change);
            if (!_userService.CompareOldPassword(change.OldPassword, User.Identity.Name))
            {
                ModelState.AddModelError("OldPassword", "کلمه عبور فعلی صحیح نمی باشد!");
                return View(change);
            }
            _userService.ChangeUserPassword(User.Identity.Name, change.Password);
            ViewBag.IsSuccess = true;

            return View();
        }

        #endregion
    }
}
