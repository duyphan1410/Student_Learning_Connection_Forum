using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using Forum.Models;

namespace Forum.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        dbForumDataContext db = new dbForumDataContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            var sTenDN = f["TenDangNhap"];
            var sMatKhau = f["MatKhau"];
            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["Err1"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["Err2"] = "Phải nhập mật khẩu";
            }
            else
            {
                NguoiDung nd = db.NguoiDungs.SingleOrDefault(n => n.TenDangNhap == sTenDN && n.MatKhau == GetMD5(sMatKhau));
                if (nd != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["TaiKhoan"] = nd;
                    if (f["Remember"].Contains("true"))
                    {
                        Response.Cookies["TenDangNhap"].Value = sTenDN;
                        Response.Cookies["MatKhau"].Value = sMatKhau;
                        Response.Cookies["TenDangNhap"].Expires = DateTime.Now.AddDays(1);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(1);
                    }
                    else
                    {
                        Response.Cookies["TenDangNhap"].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(-1);
                    }
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection f, NguoiDung nd)
        {
            var sHoTen = f["TenND"];
            var sTenDangNhap = f["TenDangNhap"];
            var sMatKhau = f["MatKhau"];
            var sMatkhauNhapLai = f["MatKhauNL"];
            var sEmail = f["Email"];        
            var sDienThoai = f["DienThoai"];
            var sChucVu = f["ChucVu"];
            var sChuyenNganh = f["ChuyenNganh"];
            var dNgaySinh = String.Format("{0:MM/dd/yyyy}", f["NgaySinh"]);
            if (String.IsNullOrEmpty(sMatkhauNhapLai))
            {
                ViewData["err4"] = "Phải nhập lại mật khẩu";
            }
            else if (sMatKhau != sMatkhauNhapLai)
            {
                ViewData["err4"] = "Mật khẩu nhập lại không khớp";
            }
            else if (db.NguoiDungs.SingleOrDefault(n => n.TenDangNhap == sTenDangNhap) != null)
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
            }
            else if (db.NguoiDungs.SingleOrDefault(n => n.Email == sEmail) != null)
            {
                ViewBag.ThongBao = "Email đã được sử dụng";
            }
            else if (ModelState.IsValid)
            {
                nd.TenND = sHoTen;
                nd.TenDangNhap = sTenDangNhap;
                nd.MatKhau = GetMD5(sMatKhau);
                nd.Email = sEmail;
                nd.SDT = sDienThoai;
                nd.ChucVu = sChucVu;
                nd.ChuyenNganh= sChuyenNganh;
                if(dNgaySinh == null)
                {
                    nd.NgaySinh = null;
                }
                else
                {
                    nd.NgaySinh = DateTime.Parse(dNgaySinh);
                }
                db.NguoiDungs.InsertOnSubmit(nd);
                db.SubmitChanges();
                return RedirectToAction("DangNhap");
            }
            return this.DangKy();
        }

        public static string GetMD5(string str)
        {
            MD5 mD5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = mD5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String = targetData[i].ToString("x2");
            }
            return byte2String;
        }
    }
}