using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Forum.Models;

namespace Forum.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        dbForumDataContext db = new dbForumDataContext();
        // GET: Dashboard/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            string UserName = f["sTenDN"].ToString();
            string Password = f["Password"].ToString();
            var admin = db.Admins.SingleOrDefault(n => n.TenDN == UserName && n.MatKhau == Password);
            if (admin != null)
            {
                Session["Admin"] = admin;
                return RedirectToAction("Index", "HomeAd");
            }
            TempData["err"] = "Đăng nhập không thành công";
            return RedirectToAction("Login");
        }
    }
}