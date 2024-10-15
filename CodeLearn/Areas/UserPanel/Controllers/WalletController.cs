using CodeLearn.Core.DTOs;
using CodeLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CodeLearn.Web.Areas.UserPanel.Controllers
{
    [Authorize]
    [Area("UserPanel")]
    public class WalletController : Controller
    {
        private IUserService _userService;
        public WalletController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("UserPanel/Wallet")]
        public IActionResult Index()
        {
            //chon xode index daraye modele charge ast pas be an nemidahim va view bag jodaganeyi dorost mikonim
            ViewBag.ListWallet = _userService.GetWalletsUser(User.Identity.Name); 

            return View();
        }

        [HttpPost]
        [Route("UserPanel/Wallet")]
        public IActionResult Index(ChargeWalletViewModel charge)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.ListWallet = _userService.GetWalletsUser(User.Identity.Name);
                return View(charge);
            }

            int walletId=_userService.ChargeWallet(User.Identity.Name, charge.Amount, "شارژ حساب");

            //TODO Online Payment
            #region OnlinePayment
            var payment= new ZarinpalSandbox.Payment(charge.Amount);
            var res = payment.PaymentRequest("شارژ کیف پول", "https://localhost:44317/OnlinePayment" + walletId);

            if(res.Result.Status == 100) 
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority);
            }
            #endregion
            return null;
        }
    }
}
