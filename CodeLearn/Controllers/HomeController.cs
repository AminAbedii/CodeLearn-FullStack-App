﻿using CodeLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CodeLearn.Web.Controllers
{
    public class HomeController : Controller
    {
        private IUserService _userService;
        private ICourseService _courseService;
        public HomeController(IUserService userService,ICourseService courseService)
        {
            _userService = userService;
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            return View(_courseService.GetCourse());
        }

        [Route("OnlinePayment/{id}")]
        public IActionResult OnlinePayment(int id) 
        {
            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok"
                && HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"];

                var wallet = _userService.GetWalletByWalletId(id);

                var payment = new ZarinpalSandbox.Payment(wallet.Amount);
                var res = payment.Verification(authority).Result;
                if (res.Status == 100)
                {
                    ViewBag.code = res.RefId;
                    ViewBag.IsSuccess = true;
                    wallet.IsPay=true;
                    _userService.UpdateWallet(wallet);
                }

            }
            return View();
        }

        public IActionResult GetSubGroups(int id)  //baraye gereftan group asli va pass dadan group haye farii dar create course jadid
        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "انتخاب کنید",Value = ""}
            };
            list.AddRange(_courseService.GetSubGroupForManageCourse(id));
            return Json(new SelectList(list, "Value", "Text"));
        }

        [HttpPost]
        [Route("file-upload")]
        //this is for upload image for CourseDisciription
        public IActionResult UploadImage(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();



            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/MyImages",
                fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);

            }



            var url = $"{"/MyImages/"}{fileName}";


            return Json(new { uploaded = true, url });
        }
    }
}
