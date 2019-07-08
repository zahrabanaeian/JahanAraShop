using JahanAraShop.Areas.Admin.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using JahanAraShop.Resourecs;

namespace JahanAraShop.Areas.Admin.Controllers
{
    
        // GET: Admin/signin
        [Authorize(Roles = "Admin, Manager, Editor, Social, Job")]

        public partial class SigninController : Controller
        {
            public SigninController()
            {

            }

            private ApplicationSignInManager _signInManager;
            private ApplicationUserManager _userManager;


            public SigninController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
            {
                UserManager = userManager;
                SignInManager = signInManager;
            }

            public ApplicationSignInManager SignInManager
            {
                get
                {
                    return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                }
                private set
                {
                    _signInManager = value;
                }
            }

            public ApplicationUserManager UserManager
            {
                get
                {
                    return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                }
                private set
                {
                    _userManager = value;
                }
            }
            // GET: Admin/Signin
            [AllowAnonymous]
            public virtual ActionResult Index()
            {
                return View();
            }
            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public virtual async Task<ActionResult> Index(LoginViewModel model, string returnUrl)
            {

                if (!ModelState.IsValid)
                {
                    return View(MVC.Admin.Default.Index());
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                var result = await SignInManager.PasswordSignInAsync(model.PhoneNumber, model.Password, false, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToAction(MVC.Admin.Default.Index());
                    default:
                        ModelState.AddModelError("", "شناسه کاربری یا رمز عبور وارد شده اشتباه می باشد.");
                        return View(model);
                }
            }


        public virtual ActionResult ChangePassword()
        {
            return View();

        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ChangePassword(JahanAraShop.Models.ChangePasswordViewModel model, string returnUrl)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = User.Identity.GetUserId();
            // This doen't count login failures towards lockout only two factor authentication
            // To enable password failures to trigger lockout, change to shouldLockout: true
            var result = await UserManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {

                const string description = "کاربر گرامی! تغییر رمز شما با موفقیت انجام گرفت.";
                ViewBag.Message = description;
                return View();
            }
            else
            {
                ModelState.AddModelError("", ErrorMessages.InCorrectPassword);
                
                return View(model);
            }

        }

    }
}
