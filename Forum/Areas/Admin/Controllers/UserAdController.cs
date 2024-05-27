using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Forum.Models;

namespace Forum.Areas.Admin.Controllers
{
    public class UserAdController : Controller
    {
        private dbForumDataContext db = new dbForumDataContext();
        // GET: Admin/User
        public ActionResult Index()
        {
            return View(db.NguoiDungs.ToList());
        }
    }
}