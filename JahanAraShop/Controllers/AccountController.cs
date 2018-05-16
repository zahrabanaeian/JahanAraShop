﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using JahanAraShop.Resourecs;
using JahanAraShop.Models;
using System.Net.Http;
using JahanAraShop.Data.Context;
using JahanAraShop.Domain.DomainModel;
using JahanAraShop.Services;

namespace JahanAraShop.Controllers
{
    [Authorize]
    public partial class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public virtual ActionResult Login(string returnUrl)
        {

            if (Request.IsAuthenticated)
            {
                const string title = "ورود انجام گرفته";
                const string description = "کاربر گرامی شما قبلا وارد سایت شده اید.";
                if (Request.Url != null)
                    TempData = new TempDataDictionary() {
                        {"title", title},
                        {"description", description},
                        { "currentpage", Request.Url.ToString()},
                        { "type", MessageViewModel.MessageTypes.Success}
                    };
                // return RedirectToAction(MVC.Message.Index());
                return View();

            }


            ViewBag.ReturnUrl = returnUrl;
            return View();
        }



        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doen't count login failures towards lockout only two factor authentication
            // To enable password failures to trigger lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.PhoneNumber, model.Password, model.RememberMe, shouldLockout: false);
            var user = await UserManager.FindByNameAsync(model.PhoneNumber);
            switch (result)
            {

                case SignInStatus.Success:

                    {
                        using (var db = new DataBaseContext())
                        {

                           tblOrganization org = new Domain.DomainModel.tblOrganization();
                            var userId = user.Id;
                            // org = db.tblOrganization.Where(c => c.User_Id == userId).Single();
                            //Session["username"] = Nininazshop.Resources.Labels.Hello+" "+org.FirstName + " " + org.Name;
                            //Session.Timeout = 30;
                            return RedirectToLocal(returnUrl);
                        }

                    }


                //case SignInStatus.LockedOut:
                //    return View("Lockout");
                //case SignInStatus.RequiresVerification:
                //    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", ErrorMessages.InvalidPassOrUserName);
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public virtual async Task<ActionResult> VerifyCode(string provider, string returnUrl)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            var user = await UserManager.FindByIdAsync(await SignInManager.GetVerifiedUserIdAsync());
            if (user != null)
            {
                ViewBag.Status = "For DEMO purposes the current " + provider + " code is: " + await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: false, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public virtual ActionResult Register()
        {
            return View();
        }

       
       // POST: /Account/Register
       [HttpPost]
       [AllowAnonymous]
       [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Register(RegisterViewModel model)
        {


            if (!ModelState.IsValid) return View(model);
            if (ModelState.IsValid)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = new ApplicationUser { UserName = model.PhoneNumber, PhoneNumber = model.PhoneNumber };
                var result = UserManager.Create(user, model.Password);
                if (result.Succeeded)
                {

                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    var neworg = new OrganizationSiteModel
                    {
                        PhoneNumber = model.PhoneNumber,
                        FirstName = model.FirstName,
                        Name = model.Name,
                        User_Id = user.Id

                    };
                     var resulapit = await WebApi.PostOrganization(neworg);

                    //Organization اضافه کردن کاربر به جدول 
             
                    using (var client = new HttpClient())
                    {
                        //client.BaseAddress = new Uri(AppConstants.ApiAddress);
                        //const string url = "PostOrganization";
                        //dynamic organizationModel = new ExpandoObject();
                        //organizationModel.FirstName = model.FirstName;
                        //organizationModel.Name = model.Name;
                        //organizationModel.PhoneNumber = model.PhoneNumber;
                        //organizationModel.User_Id = user.Id;
                        //var jsonorganizationModel = JsonConvert.SerializeObject(organizationModel);
                        //var content = new StringContent(jsonorganizationModel, Encoding.UTF8, AppConstants.JsonType);
                        //var jsonResult = await client.PostAsync(url, content);
                        //var response = jsonResult.Content.ReadAsStringAsync().Result;
                        //var finalResult = JsonConvert.DeserializeObject<Services.Result>(response);

                        ////if (resulapit.ErrorStatus == 0)
                        //if (finalResult.ErrorStatus == Services.Result.ErrorStatusType.Ok)
                        //{

                        //    var title = Labels.Register;
                        //    var description = Labels.SuccessfulMessage;
                        //    if (Request.Url != null)
                        //        TempData = new TempDataDictionary() {
                        //    {"title", title},
                        //    {"description", description},
                        //    { "currentpage", Request.Url.ToString()},
                        //    { "type", ViewModels.MessageViewModel.MessageTypes.Success}
                        //};
                        //    return RedirectToAction(MVC.Message.Index());
                        //}
                        //else
                        //{
                        //    var title = Labels.Register;
                        //    var description = Labels.ErrorMessage;
                        //    if (Request.Url != null)
                        //        TempData = new TempDataDictionary() {
                        //    {"title", title},
                        //    {"description", description},
                        //    { "currentpage", Request.Url.ToString()},
                        //    { "type", ViewModels.MessageViewModel.MessageTypes.Error}
                        //};
                        //    return RedirectToAction(MVC.Message.Index());

                    }
                }



            }
            //else
            //    AddErrors(result);


            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public virtual async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public virtual ActionResult ForgotPassword()
        {
            return View();
        }

        //
        //// POST: /Account/ForgotPassword
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public virtual async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByNameAsync(model.PhoneNumber);
        //        if (user == null) return View(model);
        //        var errorMessage = "";
        //        var smscode = SMS.GetCode();
        //        var username = "n1125592";
        //        var password = "nini@1234";
        //        var smsnumber = "30001341125592";

        //        //if (!SMS.SendSms(model.PhoneNumber, AppConstants.MesCode + code, username, password, smsnumber, out errorMessage))
        //        //{
        //        //    ModelState.AddModelError("", AppConstants.MesSmsSendFailed);
        //        //    return View(model);
        //        //}
        //        Session.Timeout = Setting.SesExpireTime;
        //        //Session[AppConstants.SesCode] = Services.Utilities.Encrypt(code);
        //        Session[AppConstants.SesCode] = smscode;
        //        return RedirectToAction(MVC.Account.ResetPassword(model.PhoneNumber));
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public virtual ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public virtual ActionResult ResetPassword(string phonenumber)
        {
            //var x = Session[AppConstants.SesCode] as string;
            var md = new ResetPasswordViewModel { PhoneNumber = phonenumber };
            return View(md);
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.PhoneNumber);
            if (user == null)
            {
                const string title = "بازنشانی پسورد";
                const string description = "کاربر گرامی! بانشانی پسورد شما با مشکل مواجه شده است. لطفا دوباره تلاش کنید.";
                if (Request.Url != null)
                    TempData = new TempDataDictionary() {
                        {"title", title},
                        {"description", description},
                        { "currentpage",Request.Url.ToString()},
                        { "type", MessageViewModel.MessageTypes.Error}
                    };
                // return RedirectToAction(MVC.Message.Index());
                return View();
            }
            //  model.Smscode = Session[AppConstants.SesCode] as string;

            //string smscode = Session[AppConstants.SesCode] as string;
            string smscode = "";
            if (smscode == model.Code)
            {
                var result = UserManager.RemovePassword(user.Id);
                if (result == IdentityResult.Success)
                    result = UserManager.AddPassword(user.Id, model.Password);
                SignInUser(user, false);
                if (result == IdentityResult.Success)
                {
                    const string title = "بازنشانی پسورد";
                    const string description = "کاربر گرامی! تغییر رمز شما با موفقیت انجام گرفت. هم اکنون می توانید وارد سایت شوید.";
                    if (Request.Url != null)
                        TempData = new TempDataDictionary() {
                        {"title", title},
                        {"description", description},
                        { "currentpage", Request.Url.ToString()},
                        { "type", MessageViewModel.MessageTypes.Success}
                    };
                    //return RedirectToAction(MVC.Message.Index());
                    return View();
                }
                AddErrors(result);
            }

            else
            {
                ModelState.AddModelError("", ErrorMessages.IsNotSameCode);
                return View(model);
            }
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public virtual ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public virtual async Task<ActionResult> SendCode(string returnUrl)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public virtual async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult LogOff()
        {
            // AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Clear();
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }



        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public virtual ActionResult ExternalLoginFailure()
        {
            return View();
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        /// <summary>
        /// متد ورود کاربر
        /// </summary>
        private void SignInUser(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }


        /// <summary>
        /// موبایل چک
        /// </summary>
        /// <param name="phoneNumber">شماره موبایل</param>
        /// <returns></returns>
        public async Task<bool> IsUser(string phoneNumber)
        {
            var user = await UserManager.FindByNameAsync(phoneNumber);

            if (user == null)
                return false;
            return true;
        }




        // POST: /Shop/Login
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public virtual async Task<ActionResult> Authenticate(LoginViewModel model)
        //{

        //    //اعتبار سنجی شماره موبایل وار شده
        //    if (!ModelState.IsValid)
        //        return View(MVC.Shop.Views.Authenticate, model);
        //    //ذخیره موبایل وارد شده در نشست
        //    //   Session[AppConstants.SesMobile] = Services.Utilities.Encrypt(model.PhoneNumber);
        //    //   var cellphone = model.PhoneNumber.CellphonNormalize();
        //    var cellphone = model.PhoneNumber;
        //    //اگر کاربر باشد
        //    if (await IsUser(model.PhoneNumber))
        //    //پسوردش را بپرس
        //    {

        //        // if (Session[AppConstants.SesMobile] == null)
        //        if (cellphone == null)
        //            return View(MVC.Shop.Views.Authenticate, new LoginViewModel());
        //        //  model.PhoneNumber = Utilities.Decrypt(Session[AppConstants.SesMobile].ToString());


        //        //ورود با اطلاعات وارد شده
        //        // To enable password failures to trigger account lockout, change to shouldLockout: true
        //        var result = await SignInManager.PasswordSignInAsync(model.PhoneNumber, model.Password, model.RememberMe, shouldLockout: false);
        //        //بررسی نتیجه
        //        switch (result)
        //        {
        //            case SignInStatus.Success://موفقیت در ورود
        //                return RedirectToAction(MVC.Shop.Invoice());

        //            //case SignInStatus.LockedOut://کاربر قفل شده است
        //            //    break;
        //            //case SignInStatus.RequiresVerification://شماره موبایل کاربر تایید نشده است
        //            //    break;
        //            case SignInStatus.Failure://شماره موبایل یا رمز ورد اشتباه است.
        //                ModelState.AddModelError("", ErrorMessages.InvalidPassOrUserName);
        //                return View(MVC.Shop.Views.Authenticate, model);
        //        }
        //        return null;//هیچ زمانی رخ نمی دهد
        //    }
        //    //و الا
        //    //else
        //    //    using (var db = new DataBaseContext())
        //    //    {
        //    //        var person = db.tblOrganization
        //    //            .FirstOrDefault(o => o.CellPhone == cellphone || o.CellPhone == model.PhoneNumber);
        //    //        //اگر مشتری باشد
        //    //        if (person != null)
        //    //        {
        //    //            //یک پیامک برایش ارسال کن و سپس به صفحه اعتبارسنجی موبایل ارجاع شود
        //    //            var errorMessage = "";
        //    //            var code = SmsUtilities.GetCode();
        //    //            var username = await WebApi.GetParameter(SmsUserName);
        //    //            var password = await WebApi.GetParameter(SmsPassword);
        //    //            var number = await WebApi.GetParameter(SmsNumber);
        //    //            if (!SmsUtilities.SendSms(Utilities.Decrypt(Session[AppConstants.SesMobile].ToString()), AppConstants.MesCode + code, username, password, number, out errorMessage))
        //    //            {
        //    //                var obj = new { state = AjaxResultStates.Error, msg = errorMessage };
        //    //                return Json(obj);
        //    //            }
        //    //            Session.Timeout = Settings.SesExpireTime;
        //    //            Session[AppConstants.SesCode] = Services.Utilities.Encrypt(code);
        //    //            return PartialView(MVC.Shop.Views._SMSVerfication, new TowStepLoginSms());
        //    //        }
        //    //        //اگر مهمان بود
        //    //        //به صفحه فاکتور ارجاع شود
        //    //        var res = new { state = AjaxResultStates.Redirect, url = "/Shop/Invoice" };
        //    //        return Json(res);
        //    //    }
        //    return View(model);
        //}

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        #region GetUserName
        public virtual string GetUserName(string name)
        {
            using (var db = new DataBaseContext())
            {
                //var userid = User.Identity.GetUserId();


                var username = (from organization in db.tblOrganizations
                                where organization.CellPhone == name
                                select new { organization.FirstName, organization.Name }).SingleOrDefault();
                if (username != null)
                {
                    string v = string.Format("{0} {1} {2}", "سلام", username.FirstName, username.Name);
                    return v;
                }
                else
                    return " ";
            }

        }
        #endregion GetUserName

        public virtual ActionResult ChangePassword()
        {
            return View();

        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ChangePassword(ChangePasswordViewModel model, string returnUrl)
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
                const string title = "تغییر پسورد";
                const string description = "کاربر گرامی! تغییر رمز شما با موفقیت انجام گرفت.";
                if (Request.Url != null)
                    TempData = new TempDataDictionary() {
                        {"title", title},
                        {"description", description},
                        { "currentpage", Request.Url.ToString()},
                        { "type", MessageViewModel.MessageTypes.Success}
                    };
                //  return RedirectToAction(MVC.Message.Index());
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