using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hanashop.Models;
namespace Hanashop.Models
{
    public class Giohang
    {
        //Tao doi tuong data chua dữ liệu từ model dbBansach đã tạo. 
        QLBANQADataContext data = new QLBANQADataContext();
        public int iMasach { set; get; }
        public string sTensach { set; get; }
        public string sAnhbia { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }

        }
        //Khoi tao gio hàng theo Masach duoc truyen vao voi Soluong mac dinh la 1
        public Giohang(int Masach)
        {
            iMasach = Masach;
            QUANAO sach = data.QUANAOs.Single(n => n.MaQA == iMasach);
            sTensach = sach.TenQA;
            sAnhbia = sach.Anhbia;
            dDongia = double.Parse(sach.Giaban.ToString());
            iSoluong = 1;
        }
    }
}