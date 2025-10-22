using LAB06_DAL.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LAB06_BUS.Services
{
    public class SachService
    {
        // Lấy toàn bộ sách + thông tin loại sách
        public List<Sach> GetAll()
        {
            using (var db = new SachModel())
            {
                return db.Saches.Include(s => s.LoaiSach).ToList();
            }
        }

        // Tìm kiếm theo mã, tên, hoặc năm xuất bản
        public List<Sach> Search(string keyword)
        {
            using (var db = new SachModel())
            {
                return db.Saches
                    .Include(s => s.LoaiSach)
                    .Where(s => s.MaSach.Contains(keyword)
                             || s.TenSach.Contains(keyword)
                             || s.NamXB.ToString().Contains(keyword))
                    .ToList();
            }
        }

        // Lấy sách theo mã
        public Sach GetById(string maSach)
        {
            using (var db = new SachModel())
            {
                return db.Saches.Include(s => s.LoaiSach).FirstOrDefault(s => s.MaSach == maSach);
            }
        }

        // Thêm hoặc cập nhật sách (AddOrUpdate)
        public bool AddOrUpdate(Sach sach)
        {
            using (var db = new SachModel())
            {
                if (string.IsNullOrWhiteSpace(sach.MaSach) || sach.MaSach.Length != 6)
                    return false;
                if (string.IsNullOrWhiteSpace(sach.TenSach))
                    return false;

                var existing = db.Saches.Find(sach.MaSach);
                if (existing == null)
                {
                    db.Saches.Add(sach);
                }
                else
                {
                    existing.TenSach = sach.TenSach;
                    existing.NamXB = sach.NamXB;
                    existing.MaLoai = sach.MaLoai;
                    existing.HinhAnh = sach.HinhAnh;
                }

                db.SaveChanges();
                return true;
            }
        }

        // Xóa sách theo mã
        public bool Delete(string maSach)
        {
            using (var db = new SachModel())
            {
                var sach = db.Saches.Find(maSach);
                if (sach == null)
                    return false;

                db.Saches.Remove(sach);
                db.SaveChanges();
                return true;
            }
        }
    }
}
