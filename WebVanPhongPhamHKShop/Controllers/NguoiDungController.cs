using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebVanPhongPhamHKShop.Models;

namespace WebVanPhongPhamHKShop.Controllers
{
    public class NguoiDungController : Controller
    {
        dbQLVanPhongPhamDataContext db = new dbQLVanPhongPhamDataContext();//Tao databsase de dua thong tin va lay thong tin tu cac bang
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]//hien view
        public ActionResult DangKy()
        {
            return View();
        }
        
        [HttpPost]//dua du lieu vao
        public ActionResult DangKy(FormCollection collection, KhachHang kh)
        {
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var email = collection["Email"];
            var diachi = collection["Diachi"];
            var dienthoai = collection["Dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            if(String.IsNullOrEmpty(hoten))//neu de trong thi hien thi loi
            {
                ViewData["Loi1"] = "*Họ tên khách hàng không được để trống";
            }
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "*Phải nhập tên đăng nhập";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "*Phải nhập mật khẩu";
            }
            if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi4"] = "*Phải nhập lại mật khẩu";
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "*Email không được bỏ trống";
            }
            if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi6"] = "*Vui lòng nhập địa chỉ";
            }
            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi7"] = "*Phải nhập điện thoại";
            }
            else
            {
                kh.HoTen = hoten;
                kh.TaiKhoan = tendn;
                kh.MatKhau = matkhau;
                kh.Email = email;
                kh.DiaChiKH = diachi;
                kh.DienThoaiKH = dienthoai;
                kh.NgaySinh = DateTime.Parse(ngaysinh);
                db.KhachHangs.InsertOnSubmit(kh);
                db.SubmitChanges();
            }
            return RedirectToAction("Index", "HKShop");
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {  
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];

            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "*Phải nhập tên đăng nhập";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "*Phải nhập mật khẩu";
            }
            else
            {
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan == tendn && n.MatKhau == matkhau);
                if (kh != null)
                {
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index", "HKShop");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
    }
}