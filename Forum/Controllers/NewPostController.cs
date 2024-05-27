using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Forum.Models;

namespace Forum.Controllers
{
    public class NewPostController : Controller
    {
        // GET: NewPost
        dbForumDataContext db = new dbForumDataContext();
        public ActionResult Index()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                //  return RedirectToAction("DangNhap", "User");
                return RedirectToAction("DangNhap","User");
            }
            else return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            //tenform["Name bien input"]
            NguoiDung nd = Session["TaiKhoan"] as NguoiDung;
            var bd = new BaiDang();
            if (ModelState.IsValid)
            {
                if (fFileUpload != null)
                {
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    bd.HinhAnh = sFileName;
                }
                bd.TieuDe = f["sTieuDe"];
                bd.NoiDung = f["sNoidung"].Replace("<p>", "").Replace("</p>", "");
                bd.NgayDang = DateTime.Now;
                bd.LuotPH = 0;
                bd.LuotTuongTac= 0;
                bd.MaND = nd.MaND;
                db.BaiDangs.InsertOnSubmit(bd);
                db.SubmitChanges();
                return RedirectToAction("Index","Home");
            }
            return View(bd);
        }
    }
}