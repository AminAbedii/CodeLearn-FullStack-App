using CodeLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class MyOrdersController : Controller
    {

        private IOrderService _orderService;
        

        public MyOrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowOrder(int orderId) 
        {
            var order = _orderService.GetOrderForUserPanel(User.Identity.Name, orderId);

            if(order == null)
            {
                return NotFound();
            }

            return View();
        }
    }
}
