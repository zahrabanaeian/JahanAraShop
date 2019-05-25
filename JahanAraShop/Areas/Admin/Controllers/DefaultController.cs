using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JahanAraShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Manager, Editor, Social, Job")]
    
    public partial class DefaultController : Controller
    {
        public readonly JahanAraShop.Models.ApplicationDbContext Context = new JahanAraShop.Models.ApplicationDbContext();
        // GET: Admin/Default
        public virtual ActionResult Index()
        {
            return View();
        }

        //Get RolesManager
        public virtual ActionResult RolesManager()
        {
            // prepopulat roles for the view dropdown
            var list = Context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View();
        }

        public virtual ActionResult Create(FormCollection collection)
        {
            try
            {
                Context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                {
                    Name = collection["RoleName"]
                });
                Context.SaveChanges();
                ViewBag.ResultMessage = "نقش با موفقیت اضافه شد!";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }


     
        public virtual ActionResult GetRoles(string userName)
        {
            
            var user = Context.Users.FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase));
            var account = new AccountController();


            if (user != null)
            {
                var list = account.UserManager.GetRoles(user.Id);
                return Json(list, JsonRequestBehavior.AllowGet);
            }




            return Json("نقشی به این یوزر اختصاص داده نشده است", JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult RoleAddToUser(string CellPhone, string roleName)
        {
            var user = Context.Users.FirstOrDefault(u => u.UserName.Equals(CellPhone, StringComparison.CurrentCultureIgnoreCase));
            var account = new AccountController();

            if (user != null) account.UserManager.AddToRole(user.Id, roleName);

            ViewBag.ResultMessage = "عملیات با موفقیت انجام شد!";

            // prepopulat roles for the view dropdown
            var list = Context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return RedirectToAction(MVC.Admin.Default.RolesManager());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult DeleteRoleForUser(string PhoneNumber, string roleName)
        {
            var account = new AccountController();
            var user = Context.Users.FirstOrDefault(u => u.UserName.Equals(PhoneNumber, StringComparison.CurrentCultureIgnoreCase));

            if (user != null && account.UserManager.IsInRole(user.Id, roleName))
            {
                account.UserManager.RemoveFromRole(user.Id, roleName);
                ViewBag.ResultMessage = "عملیات با موفقیت انجام شد!";
            }
            else
            {
                ViewBag.ResultMessage = "این کاربر به این نقش تخصص ندارد.";
            }
            // prepopulat roles for the view dropdown
            var list = Context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return RedirectToAction(MVC.Admin.Default.RolesManager());
        }
    }
}