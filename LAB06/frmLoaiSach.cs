using LAB06_BUS.Services;
using LAB06_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB06
{
    public partial class frmLoaiSach : Form
    {
        private readonly LoaiSachService loaiSachService = new LoaiSachService();

        public frmLoaiSach()
        {
            InitializeComponent();
        }

        private void frmLoaiSach_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            dgvLoaiSach.DataSource = loaiSachService.GetAll()
                .Select(ls => new { ls.MaLoai, ls.TenLoai })
                .ToList();
        }

        private void dgvLoaiSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvLoaiSach.Rows[e.RowIndex];
                txtMaLoai.Text = row.Cells["MaLoai"].Value.ToString();
                txtTenLoai.Text = row.Cells["TenLoai"].Value.ToString();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenLoai.Text))
            {
                MessageBox.Show("Vui lòng nhập tên loại sách!");
                return;
            }

            var loai = new LoaiSach
            {
                TenLoai = txtTenLoai.Text.Trim()
            };

            if (loaiSachService.Add(loai))
            {
                MessageBox.Show("Thêm loại sách thành công!");
                LoadData();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Thêm thất bại! Vui lòng kiểm tra dữ liệu.");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaLoai.Text))
            {
                MessageBox.Show("Vui lòng chọn loại sách cần sửa!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenLoai.Text))
            {
                MessageBox.Show("Tên loại sách không được để trống!");
                return;
            }

            int maLoai = int.Parse(txtMaLoai.Text);
            var loai = new LoaiSach
            {
                MaLoai = maLoai,
                TenLoai = txtTenLoai.Text.Trim()
            };

            if (loaiSachService.Update(loai))
            {
                MessageBox.Show("Cập nhật loại sách thành công!");
                LoadData();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Không tìm thấy loại sách cần sửa!");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaLoai.Text))
            {
                MessageBox.Show("Vui lòng chọn loại sách cần xóa!");
                return;
            }

            int maLoai = int.Parse(txtMaLoai.Text);

            var result = MessageBox.Show("Bạn có chắc muốn xóa loại sách này?",
                                         "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                bool ok = loaiSachService.Delete(maLoai);
                if (ok)
                {
                    MessageBox.Show("Xóa thành công!");
                    LoadData();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Không thể xóa loại sách (có thể đang được dùng trong bảng Sách).");
                }
            }
        }

        private void ResetForm()
        {
            txtMaLoai.Clear();
            txtTenLoai.Clear();
        }
    }
}
