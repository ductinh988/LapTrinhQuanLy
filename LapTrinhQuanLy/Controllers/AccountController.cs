using LapTrinhQuanLy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LapTrinhQuanLy.Controllers
{
    public class AccountController : Controller
    {
        LTQLDbContext db = new LTQLDbContext();
        Encrytion ecry = new Encrytion();
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register(AccountModel acc)
        {
            if (ModelState.IsValid)
            {
                acc.Password = ecry.PasswordEncrytion(acc.Password);
                db.AccountModels.Add(acc);
                db.SaveChanges();
                return RedirectToAction("Login", "Account"  );
            }
            return View(acc);
        }

        public ViewResult Login(string returUrl)
        {
            ViewBag.returnUrl = returUrl;
            return View();       
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AccountModel acc, string returnUrl )
        {
            if (ModelState.IsValid)
            {
                if (acc.Username == "admin" && acc.Password == "123123")
                {
                    FormsAuthentication.SetAuthCookie(acc.Username, true);
                    return RedirectToLocal(returnUrl);
                }
            }
            return View(acc);
        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }    
    }
}