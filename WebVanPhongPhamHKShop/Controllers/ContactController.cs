using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebVanPhongPhamHKShop.Models;
using PagedList;
using PagedList.Mvc;
using WebVanPhongPhamHKShop.DAO;

namespace WebVanPhongPhamHKShop.Controllers
{
    public class ContactController : Controller
    {
        public ActionResult Index()
        {
            var model = new ContactDAO().GetContact();
            return View(model);
        }  

        
    }
}