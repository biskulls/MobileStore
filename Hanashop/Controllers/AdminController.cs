using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;
using Hanashop.Models;

namespace Hanashop.Controllers
{
    public class AdminController : Controller
    {
        QLBANQADataContext db = new QLBANQADataContext();

        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Chude()
        {
            return View(db.CHUDEs.ToList());

        }
        public ActionResult Nhaxuatban()
        {
            return View(db.NHAXUATBANs.ToList());

        }

        public ActionResult Quanao(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.QUANAOs.ToList().OrderBy(n => n.MaQA).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult themcd()
        {

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult themcd(CHUDE chude)
        {
            db.CHUDEs.InsertOnSubmit(chude);
            db.SubmitChanges();
            return RedirectToAction("Chude");
        }

        [HttpGet]
        public ActionResult themnxb()
        {

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult themnxb(NHAXUATBAN nxb)
        {
            db.NHAXUATBANs.InsertOnSubmit(nxb);
            db.SubmitChanges();
            return RedirectToAction("Nhaxuatban");
        }




        [HttpGet]
        public ActionResult suacd(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            var quanao = db.CHUDEs.SingleOrDefault(x => x.MaCD == id);
            return View(quanao);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult suacd(int id, FormCollection collection)
        {
            try
            {
                //Lấy giá trị ở Form EditProduct
                string _TenSP = collection["txt_TenSP"];


                //Lấy ra thông tin Sản phẩm từ MaSP truyền vào
                var sp = db.CHUDEs.First(s => s.MaCD == id);

                //Gán giá trị để chỉnh sửa
                sp.TenChuDe = _TenSP;

                UpdateModel(sp);
                db.SubmitChanges();
                return Content("<script>alert('Chỉnh sửa thành công!');window.location='/Admin/Quanao';</script>");
            }
            catch
            {
                return Content("<script>alert('Lỗi hệ thống!');window.location='/Admin/Quanao';</script>");
            }
        }

        [HttpGet]
        public ActionResult suanxb(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            var quanao = db.NHAXUATBANs.SingleOrDefault(x => x.MaNXB == id);
            return View(quanao);

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult suanxb(int id, FormCollection collection)
        {
            try
            {
                //Lấy giá trị ở Form EditProduct
                string _TenSP = collection["txt_TenSP"];
                string _GiaCu = collection["txt_GiaCu"];
                string _MoTa = collection["txt_MoTa"];

                //Lấy ra thông tin Sản phẩm từ MaSP truyền vào
                var sp = db.NHAXUATBANs.First(s => s.MaNXB == id);

                //Gán giá trị để chỉnh sửa
                sp.TenNXB = _TenSP;
                sp.Diachi = _GiaCu;
                sp.DienThoai = _MoTa;
                UpdateModel(sp);
                db.SubmitChanges();
                return Content("<script>alert('Chỉnh sửa thành công!');window.location='/Admin/Nhaxuatban';</script>");
            }
            catch
            {
                return Content("<script>alert('Lỗi hệ thống!');window.location='/Admin/Nhaxuatban';</script>");
            }

        }

        [HttpGet]
        public ActionResult Themmoiquanao()
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.MaCD), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.MaNXB), "MaNXB", "TenNXB");

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoiquanao(QUANAO quanao, HttpPostedFileBase fileupload)
        {

            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.MaCD), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.MaNXB), "MaNXB", "TenNXB");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }

                    quanao.Anhbia = fileName;
                    db.QUANAOs.InsertOnSubmit(quanao);
                    db.SubmitChanges();
                }
                return RedirectToAction("Quanao");
            }
        }

        public ActionResult Chitietquanao(int id)
        {
            QUANAO quanao = db.QUANAOs.SingleOrDefault(n => n.MaQA == id);
            ViewBag.MaQA = quanao.MaQA;
            if (quanao == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            return View(quanao);
        }

        public ActionResult Chitietnxb(int id)
        {
            NHAXUATBAN nxb = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            ViewBag.MaNXB = nxb.MaNXB;
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            return View(nxb);
        }


        public ActionResult Xoa(int id)
        {
            QUANAO quanao = db.QUANAOs.SingleOrDefault(n => n.MaQA == id);
            ViewBag.MaQA = quanao.MaQA;
            if (quanao == null)
            {
                Response.StatusCode = 404;
                return null;

            }

            return View(quanao);
        }

        [HttpPost, ActionName("Xoa")]

        public ActionResult Xacnhanxoa(int id)
        {
            QUANAO quanao = db.QUANAOs.SingleOrDefault(n => n.MaQA == id);
            ViewBag.MaQA = quanao.MaQA;
            if (quanao == null)
            {
                Response.StatusCode = 404;
                return null;

            }

            db.QUANAOs.DeleteOnSubmit(quanao);
            db.SubmitChanges();
            return RedirectToAction("Quanao");
        }


        public ActionResult xoacd(int id)
        {
            CHUDE chude = db.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            ViewBag.MaCD = chude.MaCD;
            if (chude == null)
            {
                Response.StatusCode = 404;
                return null;

            }

            return View(chude);
        }

        [HttpPost, ActionName("xoacd")]

        public ActionResult xacnhanxoacd(int id)
        {
            CHUDE chude = db.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            ViewBag.MaCD = chude.MaCD;
            if (chude == null)
            {
                Response.StatusCode = 404;
                return null;

            }

            db.CHUDEs.DeleteOnSubmit(chude);
            db.SubmitChanges();
            return RedirectToAction("Chude");
        }



        public ActionResult xoanxb(int id)
        {
            NHAXUATBAN nxb = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            ViewBag.MaNXB = nxb.MaNXB;
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;

            }

            return View(nxb);
        }

        [HttpPost, ActionName("xoanxb")]

        public ActionResult xacnhanxoanxb(int id)
        {
            NHAXUATBAN nxb = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            ViewBag.MaNXB = nxb.MaNXB;
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;

            }

            db.NHAXUATBANs.DeleteOnSubmit(nxb);
            db.SubmitChanges();
            return RedirectToAction("Nhaxuatban");
        }




        [HttpGet]
        public ActionResult Sua(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            var quanao = db.QUANAOs.SingleOrDefault(x => x.MaQA == id);
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View(quanao);
        }
        [HttpPost]
        [ValidateInput(false)]
        //public ActionResult Sua(QUANAO quanao, FormCollection collection)
        //public ActionResult Sua(int id, HttpPostedFileBase fileUpload)
        public ActionResult Sua(int id, FormCollection collection)
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            try
            {
                //Lấy giá trị ở Form EditProduct
                string _TenSP = collection["txt_TenSP"];
                string _UrlHinh = collection["txt_UrlHinh"];
                decimal _GiaCu = decimal.Parse(collection["txt_GiaCu"]);
                string _MoTa = collection["txt_MoTa"];
                int _MaNSX = int.Parse(collection["MaNXB"]);
                int _MaCD = int.Parse(collection["MaCD"]);
                int _SoLuongTon = int.Parse(collection["txt_SLTon"]);
                DateTime _NgayCapNhat = Convert.ToDateTime(collection["txt_NgayCapNhat"]);

                //Lấy ra thông tin Sản phẩm từ MaSP truyền vào
                var sp = db.QUANAOs.First(s => s.MaQA == id);

                //Gán giá trị để chỉnh sửa
                sp.TenQA = _TenSP;
                sp.Anhbia = _UrlHinh;
                sp.Giaban = _GiaCu;
                sp.Mota = _MoTa;
                sp.MaNXB = _MaNSX;
                sp.MaCD = _MaCD;
                sp.Soluongton = _SoLuongTon;
                sp.Ngaycapnhat = _NgayCapNhat;
                UpdateModel(sp);
                db.SubmitChanges();
                return Content("<script>alert('Chỉnh sửa thành công!');window.location='/Admin/Quanao';</script>");
            }
            catch
            {
                return Content("<script>alert('Lỗi hệ thống!');window.location='/Admin/Quanao';</script>");
            }



            //  var qa = new QUANAO();
            //  qa = db.QUANAOs.First(x => x.MaQA == id);
            //  if (Session["Taikhoanadmin"] == null)
            //      return RedirectToAction("Login", "Admin");
            //  if (ModelState.IsValid)
            //  {
            //      try
            //      {
            //          foreach (string upload in Request.Files)
            //          {
            //              if (Request.Files[upload].ContentLength == 0) continue;
            //              string pathToSave = Server.MapPath("~/images/");//Phần vị trí lưu File .
            //              string filename = DateTime.Now.ToFileTime() + Path.GetFileName(Request.Files[upload].FileName);
            //              var allowedExtensions = new[] { ".png", ".gif", ".bmp", ".jpeg", ".jpg" };
            //              var extension = Path.GetExtension(filename);
            //              if (allowedExtensions.Contains(extension))
            //              {
            //                  Request.Files[upload].SaveAs(Path.Combine(pathToSave, filename));
            //                  qa.Anhbia = filename;
            //              }
            //          }
            //      }
            //      catch { }
            //      UpdateModel(qa);
            //      db.SubmitChanges();
            //      return RedirectToAction("Quanao", "Admin");
            //  }
            //  ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            //  ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            ////  ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(x => x.TenNSX), "MaNSX", "TenNSX", dt.MaNSX);
            //  return View(qa);





            ////Dua du lieu vao dropdownload
            //ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            //ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            ////Kiem tra duong dan file
            //if (fileUpload == null)
            //{
            //    ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
            //    return View();
            //}
            ////Them vao CSDL
            //else
            //{
            //    if (ModelState.IsValid)
            //    {
            //        //Luu ten fie, luu y bo sung thu vien using System.IO;
            //        var fileName = Path.GetFileName(fileUpload.FileName);
            //        //Luu duong dan cua file
            //        var path = Path.Combine(Server.MapPath("~/images/"), fileName);
            //        //Kiem tra hình anh ton tai chua?
            //        if (System.IO.File.Exists(path))
            //            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
            //        else
            //        {
            //            //Luu hinh anh vao duong dan
            //            fileUpload.SaveAs(path);
            //        }
            //        qa.Anhbia = fileName;
            //        //Luu vao CSDL   
            //        UpdateModel(qa);
            //        db.SubmitChanges();

            //    }
            //    return RedirectToAction("Quanao");
            //}

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
                admin ad = db.admins.SingleOrDefault(n => n.Useadmin == tendn && n.Passadmin == matkhau);
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
    }
}
