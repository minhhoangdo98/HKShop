using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebVanPhongPhamHKShop.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace WebVanPhongPhamHKShop.Controllers
{
    public class AdminController : Controller
    {

        dbQLVanPhongPhamDataContext db = new dbQLVanPhongPhamDataContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sanpham(int ?page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.MatHangs.ToList().OrderBy(n=>n.MaMH).ToPagedList(pageNumber,pageSize));
        }

        public ActionResult Danhmuc(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.DanhMucs.ToList().OrderBy(n => n.MaDM).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Themsanpham()
        {
            ViewBag.MaDM = new SelectList(db.DanhMucs.ToList().OrderBy(n => n.TenDM), "MaDM", "TenDM");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themsanpham(MatHang mh, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaDM = new SelectList(db.DanhMucs.ToList().OrderBy(n => n.TenDM), "MaDM", "TenDM");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    mh.AnhBia = fileName;
                    db.MatHangs.InsertOnSubmit(mh);
                    db.SubmitChanges();
                }
                return RedirectToAction("Sanpham");
            }                   
        }

        public ActionResult Chitietsp(int id)
        {
            MatHang mh = db.MatHangs.SingleOrDefault(n => n.MaMH == id);
            ViewBag.MaMH = mh.MaMH;
            if (mh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(mh);
        }

        [HttpGet]
        public ActionResult Xoasp(int id)
        {
            MatHang mh = db.MatHangs.SingleOrDefault(n => n.MaMH == id);
            ViewBag.MaMH = mh.MaMH;
            if (mh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(mh);
        }

        [HttpPost,ActionName("Xoasp")]
        public ActionResult Xacnhanxoa(int id)
        {
            MatHang mh = db.MatHangs.SingleOrDefault(n => n.MaMH == id);
            ViewBag.MaMH = mh.MaMH;
            if (mh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.MatHangs.DeleteOnSubmit(mh);
            db.SubmitChanges();
            return RedirectToAction("Sanpham");
        }

        [HttpGet]
        public ActionResult Suasp(int id)
        {
            MatHang mh = db.MatHangs.SingleOrDefault(n => n.MaMH == id);
            ViewBag.MaMH = mh.MaMH;
            if (mh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaDM = new SelectList(db.DanhMucs.ToList().OrderBy(n => n.TenDM), "MaDM", "TenDM", mh.MaDM);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX", mh.MaNSX);
            return View(mh);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasp(MatHang mh, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaDM = new SelectList(db.DanhMucs.ToList().OrderBy(n => n.TenDM), "MaDM", "TenDM");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    mh.AnhBia = fileName;
                    UpdateModel(mh);
                    db.SubmitChanges();
                }
                return RedirectToAction("Sanpham");
            }
        }

        [HttpGet]
        public ActionResult Themdanhmuc()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Themdanhmuc(DanhMuc dm)
        {
            db.DanhMucs.InsertOnSubmit(dm);
            db.SubmitChanges();
            return RedirectToAction("Danhmuc");
        }

        [HttpGet]
        public ActionResult Xoadm(int id)
        {
            DanhMuc dm = db.DanhMucs.SingleOrDefault(n => n.MaDM == id);
            ViewBag.MaDM = dm.MaDM;
            if (dm == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dm);
        }

        [HttpPost, ActionName("Xoadm")]
        public ActionResult Xacnhanxoadm(int id)
        {
            DanhMuc dm = db.DanhMucs.SingleOrDefault(n => n.MaDM == id);
            ViewBag.MaMH = dm.MaDM;
            if (dm == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.DanhMucs.DeleteOnSubmit(dm);
            db.SubmitChanges();
            return RedirectToAction("Danhmuc");
        }

        [HttpGet]
        public ActionResult Suadm(int id)
        {
            DanhMuc dm = db.DanhMucs.SingleOrDefault(n => n.MaDM == id);
            ViewBag.MaDM = dm.MaDM;
            if (dm == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dm);
        }

        [HttpPost]
        public ActionResult Suadm(DanhMuc dm)
        {
            UpdateModel(dm);
            db.SubmitChanges();
            return RedirectToAction("Danhmuc");
        }
    }
}