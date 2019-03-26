using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebVanPhongPhamHKShop.Models;

namespace WebVanPhongPhamHKShop.Controllers
{
    public class GioHangController : Controller
    {
        dbQLVanPhongPhamDataContext data = new dbQLVanPhongPhamDataContext();

        public List<Giohang> LayGioHang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }

        public ActionResult ThemGioHang(int iMaMH, string strURL)
        {
            List<Giohang> lstGiohang = LayGioHang();
            Giohang sanpham = lstGiohang.Find(n => n.iMaMH == iMaMH);
            if (sanpham == null)
            {
                sanpham = new Giohang(iMaMH);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }

        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);
            }
            return iTongSoLuong;
        }

        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhTien);
            }
            return iTongTien;
        }

        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = LayGioHang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "HKShop");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }

        public ActionResult XoaGioHang(int iMaSP)
        {
            List<Giohang> lstGiohang = LayGioHang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaMH == iMaSP);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iMaMH == iMaSP);
                return RedirectToAction("GioHang");

            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "HKShop");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapnhatGioHang(int iMaSP, FormCollection f)
        {
            List<Giohang> lstGiohang = LayGioHang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaMH == iMaSP);
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoLuong"].ToString());

            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaTatCaGioHang()
        {
            List<Giohang> lstGiohang = LayGioHang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "HKShop");
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            if(Session["Taikhoan"]==null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "HKShop");
            }
            List<Giohang> lstGiohang = LayGioHang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }

        public ActionResult DatHang(FormCollection collection)
        {
            DonDatHang ddh = new DonDatHang();
            KhachHang kh = (KhachHang)Session["Taikhoan"];
            List<Giohang> gh = LayGioHang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            var Ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.NgayGiao = DateTime.Parse(Ngaygiao);
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            data.DonDatHangs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaMH = item.iMaMH;
                ctdh.SoLuong = item.iSoluong;
                ctdh.DonGia = (decimal)item.dDongia;
                data.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }

        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}