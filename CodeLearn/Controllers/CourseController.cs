﻿using CodeLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeLearn.Web.Controllers
{
    public class CourseController : Controller
    {
        private ICourseService _courseService;
        private IOrderService _orderService;
        public CourseController(ICourseService courseService, IOrderService orderService)
        {
            _courseService = courseService;
            _orderService = orderService;
        }
        public IActionResult Index(int pageId = 1, string filter = "", string getType = "all",
            string orderByType = "date", int startPrice = 0, int endPrice = 0, List<int> selectedGroups = null)
        {
            ViewBag.selectedGroups = selectedGroups;
            ViewBag.Groups = _courseService.GetAllGroup();
            ViewBag.pageId = pageId;
            return View(_courseService.GetCourse(pageId, filter, getType, orderByType, startPrice, endPrice, selectedGroups, 9));
        }

        [Route("ShowCourse/{id}")]
        public IActionResult ShowCourse(int id)
        {
            var course = _courseService.GetCourseForShow(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [Authorize]
        public ActionResult BuyCourse(int id)
        {
            int orderId = (int)_orderService.AddOrder(User.Identity.Name, id);
            return Redirect("/UserPanel/MyOrders/ShowOrder/" + orderId);
            
        }
    }

}