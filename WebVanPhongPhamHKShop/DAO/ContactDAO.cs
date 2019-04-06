using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebVanPhongPhamHKShop.Models;

namespace WebVanPhongPhamHKShop.DAO
{
    public class ContactDAO
    {
        dbQLVanPhongPhamDataContext data = null;
        public ContactDAO()
        {
            data = new dbQLVanPhongPhamDataContext();
        }
        public Contact GetContact()
        {
            return data.Contacts.Single(x => x.status == "trues");
        }
        
        public int InsertFeedBack(Feedback fb)
        {
            
            return fb.ID;

        }
    }
}