using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BT1
{
    public partial class SearchForm : Form
    {
        public SearchCriteria DieuKien { get; private set; }

        // Hàm khởi tạo mặc định để mở được form bằng Visual Studio Designer.
        public SearchForm() : this(new string[0], null)
        {
        }

        // Nạp danh mục môn và điền lại điều kiện đang dùng khi mở tìm kiếm lần tiếp theo.
        public SearchForm(string[] danhMucMon, SearchCriteria dieuKienCu = null)
        {
            InitializeComponent();
            dtNgaySinh.MinDate = new DateTime(1900, 1, 1);
            dtNgaySinh.MaxDate = DateTime.Today;
            dtNgaySinh.Value = DateTime.Today;
            SearchCriteria banSao = (dieuKienCu ?? new SearchCriteria()).SaoChep();
            banSao.KiemTra();
            List<string> cacMon = (danhMucMon ?? new string[0]).Concat(banSao.MonHoc).Where(m => !string.IsNullOrWhiteSpace(m)).Select(m => m.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            dsMon.Items.AddRange(cacMon.ToArray());
            DienDieuKien(banSao);
        }

        // Điền điều kiện đã lưu; không dùng trực tiếp đối tượng bộ lọc của form chính.
        private void DienDieuKien(SearchCriteria dieuKien)
        {
            txtMSSV.Text = dieuKien.MSSV;
            txtHo.Text = dieuKien.HoVaTenLot;
            txtTen.Text = dieuKien.Ten;
            txtLop.Text = dieuKien.Lop;
            txtCMND.Text = dieuKien.CMND;
            txtDienThoai.Text = dieuKien.DienThoai;
            txtDiaChi.Text = dieuKien.DiaChi;
            cboGioiTinh.SelectedIndex = dieuKien.GioiTinh;
            if (dieuKien.NgaySinh.HasValue)
                dtNgaySinh.Value = dieuKien.NgaySinh.Value;
            dtNgaySinh.Checked = dieuKien.NgaySinh.HasValue;
            radAND.Checked = dieuKien.GhepAND;
            radOR.Checked = !dieuKien.GhepAND;
            for (int i = 0; i < dsMon.Items.Count; i++)
                dsMon.SetItemChecked(i, dieuKien.MonHoc.Contains((string)dsMon.Items[i], StringComparer.OrdinalIgnoreCase));
        }

        // Tạo đối tượng mới từ các ô nhập; để trống nghĩa là không dùng điều kiện.
        private SearchCriteria LayDieuKien()
        {
            return new SearchCriteria
            {
                MSSV = txtMSSV.Text,
                HoVaTenLot = txtHo.Text,
                Ten = txtTen.Text,
                Lop = txtLop.Text,
                CMND = txtCMND.Text,
                DienThoai = txtDienThoai.Text,
                DiaChi = txtDiaChi.Text,
                GioiTinh = cboGioiTinh.SelectedIndex,
                NgaySinh = dtNgaySinh.Checked ? (DateTime? )dtNgaySinh.Value.Date : null,
                MonHoc = dsMon.CheckedItems.Cast<string>().ToList(),
                GhepAND = radAND.Checked
            };
        }

        // Chỉ đóng hộp thoại khi điều kiện hợp lệ; Enter thực hiện cùng thao tác này.
        private void Tim_Click(object sender, EventArgs e)
        {
            loiNhap.Clear();
            try
            {
                SearchCriteria moi = LayDieuKien();
                moi.KiemTra();
                DieuKien = moi;
                DialogResult = DialogResult.OK;
            }
            catch (ArgumentException loi)
            {
                Control oNhap = OBiLoi(loi.ParamName);
                if (oNhap != null)
                {
                    loiNhap.SetError(oNhap, loi.Message);
                    oNhap.Focus();
                }

                MessageBox.Show(this, loi.Message, "Điều kiện chưa hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Chọn đúng ô cần sửa theo tên trường báo lỗi của SearchCriteria.
        private Control OBiLoi(string tenTruong)
        {
            switch (tenTruong)
            {
                case "MSSV":
                    return txtMSSV;
                case "CMND":
                    return txtCMND;
                case "DienThoai":
                    return txtDienThoai;
                case "GioiTinh":
                    return cboGioiTinh;
                case "NgaySinh":
                    return dtNgaySinh;
                default:
                    return null;
            }
        }

        // Giữ hộp thoại trong màn hình nhỏ; phần dư có thể cuộn và ô MSSV nhận con trỏ.
        private void Form_DaHien(object sender, EventArgs e)
        {
            Rectangle vung = Screen.FromControl(this).WorkingArea;
            Size = new Size(Math.Min(Width, vung.Width - 20), Math.Min(Height, vung.Height - 20));
            Location = new Point(Math.Max(vung.Left, Math.Min(Left, vung.Right - Width)), Math.Max(vung.Top, Math.Min(Top, vung.Bottom - Height)));
            txtMSSV.Focus();
        }
    }
}
