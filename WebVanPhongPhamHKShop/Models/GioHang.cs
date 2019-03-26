using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebVanPhongPhamHKShop.Models
{
    public class Giohang
    {
        dbQLVanPhongPhamDataContext data = new dbQLVanPhongPhamDataContext();
        public int iMaMH { set; get; }
        public string sTenMH { set; get; }
        public string sAnhbia { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhTien
        {
            get { return iSoluong * dDongia; }
        }

        public Giohang(int MaMH)
        {
            iMaMH = MaMH;
            MatHang mh = data.MatHangs.Single(n => n.MaMH == iMaMH);
            sTenMH = mh.TenMH;
            sAnhbia = mh.AnhBia;
            dDongia = double.Parse(mh.GiaBan.ToString());
            iSoluong = 1;
        }

    }
}