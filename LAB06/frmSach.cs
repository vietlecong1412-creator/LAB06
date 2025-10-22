using LAB06_BUS.Services;
using LAB06_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB06
{
    public partial class frmSach : Form
    {
        private readonly SachService sachService = new SachService();
        private readonly LoaiSachService loaiSachService = new LoaiSachService();

        private string imageFolder = Application.StartupPath + "\\Images\\";
        public frmSach()
        {
            InitializeComponent();
        }

        private void frmSach_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(imageFolder))
                Directory.CreateDirectory(imageFolder);

            LoadLoaiSach();
            LoadDanhSachSach();
        }

        private void LoadLoaiSach()
        {
            var loaiList = loaiSachService.GetAll();
            cboLoaiSach.DataSource = loaiList;
            cboLoaiSach.DisplayMember = "TenLoai";
            cboLoaiSach.ValueMember = "MaLoai";
        }

        private void LoadDanhSachSach()
        {
            dgvSach.DataSource = sachService.GetAll()
                .Select(s => new
                {
                    s.MaSach,
                    s.TenSach,
                    s.NamXB,
                    TenLoai = s.LoaiSach != null ? s.LoaiSach.TenLoai : "",
                    s.HinhAnh
                })
                .ToList();
        }

        private void dgvSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvSach.Rows[e.RowIndex];
                txtMaSach.Text = row.Cells["MaSach"].Value?.ToString();
                txtTenSach.Text = row.Cells["TenSach"].Value?.ToString();
                txtNamXB.Text = row.Cells["NamXB"].Value?.ToString();
                cboLoaiSach.Text = row.Cells["TenLoai"].Value?.ToString();

                string hinh = row.Cells["HinhAnh"].Value?.ToString();
                if (!string.IsNullOrEmpty(hinh))
                {
                    string path = Path.Combine(imageFolder, hinh);
                    if (File.Exists(path))
                        picAnhBia.Image = Image.FromFile(path);
                    else
                        picAnhBia.Image = null;
                }
                else
                    picAnhBia.Image = null;
            }
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.jpg;*.png)|*.jpg;*.png";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName = Path.GetFileName(dlg.FileName);
                string destPath = Path.Combine(imageFolder, fileName);

                // Copy ảnh vào thư mục Images
                File.Copy(dlg.FileName, destPath, true);
                picAnhBia.Image = Image.FromFile(destPath);
                picAnhBia.Tag = fileName;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!KiemTraNhapLieu()) return;

            if (sachService.GetById(txtMaSach.Text.Trim()) != null)
            {
                MessageBox.Show("Mã sách đã tồn tại, không thể thêm mới!", "Thông báo");
                return;
            }

            int nam = int.Parse(txtNamXB.Text);
            var sach = new Sach
            {
                MaSach = txtMaSach.Text.Trim(),
                TenSach = txtTenSach.Text.Trim(),
                NamXB = nam,
                MaLoai = (int)cboLoaiSach.SelectedValue,
                HinhAnh = picAnhBia.Tag?.ToString()
            };

            if (sachService.AddOrUpdate(sach))
            {
                MessageBox.Show("Thêm sách thành công!");
                LoadDanhSachSach();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Thêm sách thất bại!");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!KiemTraNhapLieu()) return;

            var sach = sachService.GetById(txtMaSach.Text.Trim());
            if (sach == null)
            {
                MessageBox.Show("Không tìm thấy sách cần sửa!", "Thông báo");
                return;
            }

            sach.TenSach = txtTenSach.Text.Trim();
            sach.NamXB = int.Parse(txtNamXB.Text);
            sach.MaLoai = (int)cboLoaiSach.SelectedValue;
            sach.HinhAnh = picAnhBia.Tag?.ToString();

            if (sachService.AddOrUpdate(sach))
            {
                MessageBox.Show("Cập nhật sách thành công!");
                LoadDanhSachSach();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại!");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string ma = txtMaSach.Text.Trim();
            if (string.IsNullOrEmpty(ma))
            {
                MessageBox.Show("Vui lòng chọn sách cần xóa!");
                return;
            }

            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận",
                                         MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (sachService.Delete(ma))
                {
                    MessageBox.Show("Xóa thành công!");
                    LoadDanhSachSach();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Sách cần xóa không tồn tại!");
                }
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string keyword = txtMaSach.Text.Trim();
            dgvSach.DataSource = sachService.Search(keyword)
                .Select(s => new
                {
                    s.MaSach,
                    s.TenSach,
                    s.NamXB,
                    TenLoai = s.LoaiSach != null ? s.LoaiSach.TenLoai : "",
                    s.HinhAnh
                }).ToList();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetForm();
            LoadDanhSachSach();
        }

        private bool KiemTraNhapLieu()
        {
            if (string.IsNullOrWhiteSpace(txtMaSach.Text) ||
                string.IsNullOrWhiteSpace(txtTenSach.Text) ||
                string.IsNullOrWhiteSpace(txtNamXB.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sách!");
                return false;
            }

            if (txtMaSach.Text.Length != 6)
            {
                MessageBox.Show("Mã sách phải đúng 6 ký tự!");
                return false;
            }

            if (!int.TryParse(txtNamXB.Text, out _))
            {
                MessageBox.Show("Năm xuất bản phải là số!");
                return false;
            }

            return true;
        }

        private void ResetForm()
        {
            txtMaSach.Clear();
            txtTenSach.Clear();
            txtNamXB.Clear();
            cboLoaiSach.SelectedIndex = 0;
            picAnhBia.Image = null;
            picAnhBia.Tag = null;
        }

        private void quảnLýLoạiSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLoaiSach frm = new frmLoaiSach();
            frm.ShowDialog();

            LoadLoaiSach();
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
