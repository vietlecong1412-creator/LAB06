using LAB06_DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace LAB06_BUS.Services
{
    public class LoaiSachService
    {
        // Lấy toàn bộ danh sách loại sách
        public List<LoaiSach> GetAll()
        {
            using (var db = new SachModel())
            {
                return db.LoaiSaches.ToList();
            }
        }

        // Lấy loại sách theo mã
        public LoaiSach GetById(int maLoai)
        {
            using (var db = new SachModel())
            {
                return db.LoaiSaches.Find(maLoai);
            }
        }

        // Thêm mới loại sách
        public bool Add(LoaiSach loai)
        {
            using (var db = new SachModel())
            {
                if (string.IsNullOrWhiteSpace(loai.TenLoai))
                    return false;

                db.LoaiSaches.Add(loai);
                db.SaveChanges();
                return true;
            }
        }

        // Cập nhật loại sách
        public bool Update(LoaiSach loai)
        {
            using (var db = new SachModel())
            {
                var old = db.LoaiSaches.Find(loai.MaLoai);
                if (old == null) return false;

                old.TenLoai = loai.TenLoai;
                db.SaveChanges();
                return true;
            }
        }

        // Xóa loại sách
        public bool Delete(int maLoai)
        {
            using (var db = new SachModel())
            {
                var loai = db.LoaiSaches.Find(maLoai);
                if (loai == null) return false;

                // Kiểm tra nếu có sách thuộc loại này thì không cho xóa
                bool hasBooks = db.Saches.Any(s => s.MaLoai == maLoai);
                if (hasBooks) return false;

                db.LoaiSaches.Remove(loai);
                db.SaveChanges();
                return true;
            }
        }
    }
}
