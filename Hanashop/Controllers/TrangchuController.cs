using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hanashop.Models;

using PagedList;
using PagedList.Mvc;

namespace Hanashop.Controllers
{
    public class TrangchuController : Controller
    {
        //Tao doi tuong daya chưa dữ liệu từ model dbBansach đã tạo. 
        QLBANQADataContext data = new QLBANQADataContext();
        // Ham lay n quyen sach moi

       
        private List<QUANAO> Laysachmoi(int count)
        {
            //Sắp xếp sách theo ngày cập nhật, sau đó lấy top @count 
            return data.QUANAOs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        public ActionResult Index(int? page)
        {
            //Tao bien quy dinh so san pham tren moi trang
            int pageSize = 4;
            //Tao bien so trang
            int pageNum = (page ?? 1);


            //Lấy top 5 Album bán chạy nhất
            var sachmoi = Laysachmoi(15);
            return View(sachmoi.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Chude()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }
        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Chudemenu()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }

        public ActionResult Nhaxuatban()
        {
            var nhaxuatban = from nxb in data.NHAXUATBANs select nxb;
            return PartialView(nhaxuatban);
        }

        public ActionResult Nhasanxuatmenu()
        {
            var nhaxuatban = from nxb in data.NHAXUATBANs select nxb;
            return PartialView(nhaxuatban);
        }
        public ActionResult SPTheochude(int id)
        {
            var sach = from s in data.QUANAOs where s.MaCD == id select s;
            return View(sach);
        }
        public ActionResult SPTheoNXB(int id)
        {
            var sach = from s in data.QUANAOs where s.MaNXB == id select s;
            return View(sach);
        }

        public ActionResult Details(int id)
        {
            var sach = from s in data.QUANAOs
                       where s.MaQA == id
                       select s;
            return View(sach.Single());
        }
    }
}
