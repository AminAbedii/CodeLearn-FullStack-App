
using CodeLearn.Core.Convertors;
using CodeLearn.Core.DTOs;
using CodeLearn.Core.Generator;
using CodeLearn.Core.Security;
using CodeLearn.Core.Senders;
using CodeLearn.Core.Services.Interfaces;
using CodeLearn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CodeLearn.Web.Controllers
{
    public class AccountController : Controller
    {
        private IUserService _userService;
        private IViewRenderService _viewRender;
        public AccountController(IUserService userService , IViewRenderService viewRender)
        {
            _userService = userService;   
            _viewRender = viewRender;
        }

        #region Register

        [Route("Register")]    //sefate Route baraye neveshtane fgt Register dar url baraye dastrasi
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [Route("Register")]    
        public IActionResult Register(RegisterViewModel register)   //daryafte method action post az register
        {
            if (!ModelState.IsValid)            //agar magadire dade shode motabar nabood be View bargardande shavad
            {
                return View(register);
            }
            //agar username gablan mojud bud error bede va view bargardun
            if (_userService.IsExistUserName(register.UserName))
            {
                ModelState.AddModelError("UserName","نام کاربری معتبز نمی باشد");
                return View(register);  
            }
            //agar email gablan mojud bud error bede va view bargardun
            if (_userService.IsExistUserEmail(FixedText.FixEmail(register.Email)))
            {
                ModelState.AddModelError("Email", "ایمیل معتبز نمی باشد");
                return View(register);
            }

            //add user to database
            DataLayer.Entities.User.User user = new User()
            {
                ActiveCode=NameGenerator.GeneratorUniqCode(),
                Email=FixedText.FixEmail(register.Email),
                IsActive=false,
                Password=PasswordHelper.EncodePasswordMd5(register.Password),
                RegisterDate=DateTime.Now,
                UserAvatar="Default.jpg",
                UserName=register.UserName,
            };
            _userService.AddUser(user);

            #region SendActivationEmail
            //Send Activation Email
            string body = _viewRender.RenderToStringAsync("_ActiveEmail", user);
            SendEmail.Send(user.Email,"فعال سازی",body);
            #endregion

            return View("SuccessRegister",user);
        }
        #endregion

        #region Login
        [Route("Login")]
        public ActionResult Login(bool EditProfile=false)
        {
            ViewBag.EditProfile = EditProfile;
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            var user = _userService.LoginUser(login);
            if (user != null) 
            {
                if (user.IsActive)
                {
                    //Loging in User
                    var claims = new List<Claim>()    //saxt list claim baraye etelaate user dar Login kardan
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                        new Claim(ClaimTypes.Name,user.UserName)
                    };
                    var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = login.RememberMe
                    };
                    HttpContext.SignInAsync(principal, properties);     //dasture login kardan in ast

                    ViewBag.IsSuccess = true;
                    return Redirect("/");
                }
                else
                {
                    ModelState.AddModelError("Email", "حساب کاربری شما فعال نمی باشد.");
                }
            }
            ModelState.AddModelError("Email", "کاربری با مشخصات وارده موجود نمی باشد.");
            return View(login);
        }
        #endregion

        #region Active Account

        public IActionResult ActiveAccount(string id)       //baraye active kardane kaarbar & in id marbut be haman active code mishavad
        {
            ViewBag.IsActive = _userService.ActiveAccount(id);
            return View();
        }

        #endregion

        #region Logout
        [Route("Logout")]
            public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
        #endregion

        #region ForgotPassword
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if (!ModelState.IsValid)
                return View(forgot);

            string fixedEmail=FixedText.FixEmail(forgot.Email);
            DataLayer.Entities.User.User user = _userService.GetUserByEmail(fixedEmail);
            if (user == null)
            {
                ModelState.AddModelError("Email", "کاربری یافت نشد !");
                return View(forgot);
            }
            string bodyEmail = _viewRender.RenderToStringAsync("_ForgotPassword", user);
            SendEmail.Send(user.Email, "بازیابی حساب کاربری", bodyEmail);
            ViewBag.IsSuccess=true;

            return View();
        }

        #endregion

        #region ResetPassword   

        public IActionResult ResetPassword(string id)
        {
            return View(new ResetPasswordViewModel()
            {
                ActiveCode = id
            });
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel reset)
        {
            if (!ModelState.IsValid)
                return View(reset);

            DataLayer.Entities.User.User user = _userService.GetUserByActiveCode(reset.ActiveCode);

            if (user == null)
                return NotFound();
            string hashNewPassword = PasswordHelper.EncodePasswordMd5(reset.Password);
            user.Password = hashNewPassword;
            _userService.UpdateUser(user);

            return RedirectToAction("Login");
        }
        #endregion
    }
}
