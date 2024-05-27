using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Forum.Models;
using PagedList;
using PagedList.Mvc;

namespace Forum.Controllers
{
    public class HomeController : Controller
    {
        dbForumDataContext db = new dbForumDataContext();
        
        public List<BaiDang> ListBaiDang(int count)
        {
            return  db.BaiDangs.OrderByDescending(n => n.NgayDang.Value.Year)
                                .ThenByDescending(n => n.NgayDang.Value.Month)
                                .ThenByDescending(n => n.NgayDang.Value.Day)
                                .ThenByDescending(n => n.NgayDang.Value.Hour)
                                .ThenByDescending(n => n.NgayDang.Value.Minute)
                                .ThenByDescending(n => n.NgayDang.Value.Second)
                                .ToList();
        }
        public ActionResult Index(int ? page)
        {
            int iSize = 10;
            int iPageNum = (page ?? 1);
            var lstBD = ListBaiDang(50);
            return View(lstBD.ToPagedList(iPageNum,iSize));
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var bd = db.BaiDangs.SingleOrDefault(n => n.MaBD == id);
            bd.LuotXem++;
            if (bd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var nguoidung = db.NguoiDungs.SingleOrDefault(n => n.MaND == bd.MaND);
            var comment = from tl in db.TraLois
                          join nd in db.NguoiDungs on tl.MaND equals nd.MaND
                          where tl.MaBD == id
                          select new UserComment
                          {
                              nguoidung = nd,
                              traloi = tl
                          };
            var feedback = from ph in db.PhanHois
                          join nd in db.NguoiDungs on ph.MaND equals nd.MaND
                          where ph.MaBD == id
                          select new UserFeedback
                          {
                              phanhoi = ph,
                              nguoidung = nd
                          };
            ViewBag.NguoiDung = nguoidung;
            ViewBag.TraLoi = comment.ToList();
            ViewBag.PhanHoi = feedback.ToList();
            return View(bd);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Details(FormCollection f,int id)
        {
            var bd = db.BaiDangs.SingleOrDefault(n => n.MaBD == id);
            bd.LuotXem = bd.LuotXem + 1;
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                //  return RedirectToAction("DangNhap", "User");
                return RedirectToAction("DangNhap", "User");
            }
            NguoiDung nd = Session["TaiKhoan"] as NguoiDung;
            if (f["btnTang"]!=null)
            {
                var btntang = int.Parse(f["btnTang"]);
                if (btntang == 1)
                {

                    if (ModelState.IsValid)
                    {
                        bd.LuotTuongTac = bd.LuotTuongTac + 1;
                        db.SubmitChanges();
                        return RedirectToAction("Details", new { id = id });
                    }
                }
                if (btntang == 2)
                {
                    var tl = db.TraLois.FirstOrDefault(n => n.MaBD == id);
                    if (ModelState.IsValid)
                    {
                        tl.LuotTTTL = tl.LuotTTTL + 1;
                        db.SubmitChanges();
                        return RedirectToAction("Details", new { id = id });
                    }
                }
            }
            if (f["btnGiam"]!=null)
            {
                var btngiam = int.Parse(f["btnGiam"]);
                if (btngiam == 1)
                {

                    if (ModelState.IsValid)
                    {
                        bd.LuotTuongTac = bd.LuotTuongTac - 1;
                        db.SubmitChanges();
                        return RedirectToAction("Details", new { id = id });
                    }
                }
                if (btngiam == 2)
                {
                    var tl = db.TraLois.SingleOrDefault(n => n.MaBD == id);
                    if (ModelState.IsValid)
                    {
                        tl.LuotTTTL = tl.LuotTTTL - 1;
                        db.SubmitChanges();
                        return RedirectToAction("Details", new { id = id });
                    }
                }
            }
            var flag = int.Parse(f["sFlag"]);
            if (flag == 0)
            {
                PhanHoi phbd = new PhanHoi();
                if (ModelState.IsValid)
                {
                    phbd.MaND = nd.MaND;
                    phbd.NoiDungPH = f["sNoiDungPHBD"].Replace("<p>", "").Replace("</p>", "");
                    phbd.NgayGioPH = DateTime.Now;
                    phbd.MaBD = id;
                    phbd.MaTL = null;
                    db.PhanHois.InsertOnSubmit(phbd);
                    db.SubmitChanges();
                    return RedirectToAction("Details", new { id = id });
                }
            }
            if (flag == 1)
            {
                TraLoi tl = new TraLoi();
                if (ModelState.IsValid)
                {
                    tl.MaND = nd.MaND;
                    tl.NoiDungTL = f["sNoidungTL"].Replace("<p>", "").Replace("</p>", "");
                    tl.LuotTTTL = 0;
                    tl.NgayGioTL = DateTime.Now;
                    tl.MaBD = id;
                    db.TraLois.InsertOnSubmit(tl);
                    bd.LuotPH++;
                    db.SubmitChanges();
                    return RedirectToAction("Details", new { id = id });
                }
            }    
            else
            {
                PhanHoi phtl = new PhanHoi();
                if (ModelState.IsValid)
                {
                    phtl.MaND = nd.MaND;
                    phtl.NoiDungPH = f["sNoiDungPHTL"].Replace("<p>", "").Replace("</p>", "");
                    phtl.NgayGioPH = DateTime.Now;
                    phtl.MaBD = id;
                    phtl.MaTL = flag;
                    db.PhanHois.InsertOnSubmit(phtl);
                    db.SubmitChanges();
                    return RedirectToAction("Details", new { id = id });
                }
            }
            return View("Details", new { id = id });

        }
    }
}