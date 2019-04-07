using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebVanPhongPhamHKShop.Models;
using PagedList;
using PagedList.Mvc;

namespace WebVanPhongPhamHKShop.Controllers
{
    public class HKShopController : Controller
    {
        dbQLVanPhongPhamDataContext data = new dbQLVanPhongPhamDataContext();//Tao database de goi table

        private List<MatHang> LayMatHangMoi(int count)
        {
            return data.MatHangs.OrderByDescending(a => a.NgaySX).Take(count).ToList();//lay cac mat hang moi, so luong la count
            
        }

        public ActionResult Index(int ? page)
        {
            int pageSize = 6;//Tao bien quy dinh so san pham tren 1 trang
            int pageNum = (page ?? 1);//tao bien so trang
            var MHMoi = LayMatHangMoi(10);//Lay 10 mat hang moi
            return View(MHMoi.ToPagedList(pageNum,pageSize));//tao view hien thi 5 mat hang moi
        }

        public ActionResult DanhMuc()
        {
            var danhMuc = from dm in data.DanhMucs select dm;
            return PartialView(danhMuc);//PartialView hien thi danh muc tai menu, can them code vao _LayoutUser o cho menu de hien thi dung
        }

        public ActionResult SPTheoDanhMuc(int id)
        {
            var mH = from s in data.MatHangs where s.MaDM == id select s;//Hien thi san pham theo danh muc
            return View(mH);
        }

        public ActionResult Detail(int id)
        {
            var mH = from s in data.MatHangs where s.MaMH == id select s;//Hien thi thong tin chi tiet san pham
            return View(mH.Single());
        }

        public ActionResult BaoHanh()
        {
            return View();
        }
    }
}