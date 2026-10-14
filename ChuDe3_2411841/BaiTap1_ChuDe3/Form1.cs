using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BT1
{
    // 4. FORM CHÍNH: giao diện và các thao tác của người dùng.
    public partial class Form1 : Form
    {
        private readonly StudentManager quanLy = new StudentManager();
        private string thuMucDuLieu, maDangSua;
        private SearchCriteria boLoc;
        private readonly string[] monMacDinh =
        {
            "Mạng máy tính",
            "Hệ điều hành",
            "Lập trình CSDL",
            "Lập trình mạng",
            "Đồ án cơ sở",
            "Phương pháp NCKH",
            "Lập trình trên thiết bị di động",
            "An toàn và bảo mật hệ thống"
        };
        private const string LocFile = "Văn bản (*.txt)|*.txt|XML (*.xml)|*.xml|JSON (*.json)|*.json";
        // Khởi tạo giao diện từ Designer và đặt giới hạn ngày sinh.
        public Form1()
        {
            InitializeComponent();
            dtNgaySinh.MaxDate = DateTime.Today;
            dtNgaySinh.Value = DateTime.Today.AddYears(-18);
        }

        // Tải danh mục môn và danh sách sinh viên khi mở chương trình.
        private void Form_Load(object sender, EventArgs e)
        {
            string thuMucGoc = Path.GetFullPath(Path.Combine(Application.StartupPath, "..", ".."));
            thuMucDuLieu = File.Exists(Path.Combine(thuMucGoc, "BT1.csproj")) ? thuMucGoc : Application.StartupPath;
            TaiDanhMucMon();
            try
            {
                string fileSV = FileLucKhoiDong();
                if (File.Exists(fileSV))
                    quanLy.TaiFile(fileSV);
                else
                    quanLy.LuuThanh(fileSV);
                HienDanhSach();
            }
            catch (Exception loi)
            {
                BaoLoi(loi);
                btnThem.Enabled = false;
                btnCapNhat.Enabled = false;
                menuXoa.Enabled = false;
                lblThongBao.Text = "Chưa tải được dữ liệu. Hãy mở file khác hoặc dùng Lưu thành.";
            }
        }

        // Đọc danh mục môn; lỗi danh mục không ngăn tải danh sách sinh viên.
        private void TaiDanhMucMon()
        {
            string[] cacMon = monMacDinh;
            try
            {
                string duongDan = Path.Combine(thuMucDuLieu, "courses.txt");
                if (File.Exists(duongDan))
                    cacMon = File.ReadAllLines(duongDan, Encoding.UTF8);
            }
            catch (Exception loi)
            {
                BaoLoi(loi);
            }

            dsMon.Items.Clear();
            dsMon.Items.AddRange(cacMon.Where(m => !string.IsNullOrWhiteSpace(m)).Select(m => m.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToArray());
        }

        // Dùng file đã chọn lần trước nếu còn tồn tại; lỗi tùy chọn thì dùng students.txt.
        private string FileLucKhoiDong()
        {
            string macDinh = Path.Combine(thuMucDuLieu, "students.txt");
            try
            {
                string tuyChon = Path.Combine(thuMucDuLieu, "lastFile.txt");
                if (File.Exists(tuyChon))
                {
                    string daChon = File.ReadAllText(tuyChon, Encoding.UTF8).Trim();
                    if (File.Exists(daChon))
                        return daChon;
                }
            }
            catch (Exception loi)
            {
                BaoLoi(loi);
            }

            return macDinh;
        }

        // Lấy dữ liệu trên form và kiểm tra trước khi thêm hoặc cập nhật.
        private Student LayTuForm()
        {
            if (!txtMSSV.MaskCompleted || !txtCMND.MaskCompleted || !txtDienThoai.MaskCompleted)
                throw new ArgumentException("Nhập đủ MSSV 7 số, CMND 9 số và điện thoại 10 số.");
            if (!radNam.Checked && !radNu.Checked)
                throw new ArgumentException("Hãy chọn giới tính.");
            Student s = new Student
            {
                MSSV = txtMSSV.Text,
                HoVaTenLot = txtHo.Text.Trim(),
                Ten = txtTen.Text.Trim(),
                Nam = radNam.Checked,
                NgaySinh = dtNgaySinh.Value.Date,
                Lop = cboLop.Text.Trim(),
                CMND = txtCMND.Text,
                DienThoai = txtDienThoai.Text,
                DiaChi = txtDiaChi.Text.Trim(),
                MonHoc = dsMon.CheckedItems.Cast<string>().ToList()
            };
            s.KiemTra();
            return s;
        }

        // Đưa danh sách lên bảng; bộ lọc chỉ đổi hiển thị, không thay dữ liệu gốc.
        private void HienDanhSach()
        {
            List<Student> danhSach = quanLy.DanhSach;
            List<Student> ketQua = quanLy.TimKiem(boLoc);
            maDangSua = null;
            bangSinhVien.BeginUpdate();
            try
            {
                bangSinhVien.Items.Clear();
                foreach (string mon in danhSach.SelectMany(s => s.MonHoc))
                    if (!dsMon.Items.Cast<string>().Contains(mon, StringComparer.OrdinalIgnoreCase))
                        dsMon.Items.Add(mon);
                foreach (Student sinhVien in ketQua)
                {
                    ListViewItem dong = new ListViewItem(sinhVien.MSSV);
                    dong.Tag = sinhVien;
                    dong.SubItems.Add(sinhVien.HoVaTenLot);
                    dong.SubItems.Add(sinhVien.Ten);
                    dong.SubItems.Add(sinhVien.NgaySinh.ToString("dd/MM/yyyy"));
                    dong.SubItems.Add(sinhVien.Lop);
                    dong.SubItems.Add(sinhVien.CMND);
                    dong.SubItems.Add(sinhVien.DienThoai);
                    dong.SubItems.Add(sinhVien.DiaChi);
                    bangSinhVien.Items.Add(dong);
                }
            }
            finally
            {
                bangSinhVien.EndUpdate();
            }

            lblThongBao.Text = "Hiển thị " + ketQua.Count + "/" + danhSach.Count + " sinh viên | " + quanLy.DuongDan;
            bool daCoFile = !string.IsNullOrEmpty(quanLy.DuongDan);
            btnThem.Enabled = daCoFile;
            btnCapNhat.Enabled = daCoFile;
            menuXoa.Enabled = daCoFile;
        }

        // Khi chọn một dòng, điền toàn bộ thông tin và môn đăng ký lên form.
        private void ChonSinhVien(object sender, EventArgs e)
        {
            maDangSua = null;
            if (bangSinhVien.SelectedItems.Count != 1)
                return;
            Student s = (Student)bangSinhVien.SelectedItems[0].Tag;
            maDangSua = s.MSSV;
            txtMSSV.Text = s.MSSV;
            txtHo.Text = s.HoVaTenLot;
            txtTen.Text = s.Ten;
            radNam.Checked = s.Nam;
            radNu.Checked = !s.Nam;
            dtNgaySinh.Value = s.NgaySinh.Date;
            cboLop.Text = s.Lop;
            txtCMND.Text = s.CMND;
            txtDienThoai.Text = s.DienThoai;
            txtDiaChi.Text = s.DiaChi;
            for (int i = 0; i < dsMon.Items.Count; i++)
                dsMon.SetItemChecked(i, s.MonHoc.Contains((string)dsMon.Items[i], StringComparer.OrdinalIgnoreCase));
        }

        // Xóa các ô nhập sau khi thêm/xóa hoặc đổi danh sách để nhập người tiếp theo.
        private void XoaO()
        {
            maDangSua = null;
            txtMSSV.Clear();
            txtHo.Clear();
            txtTen.Clear();
            txtCMND.Clear();
            txtDienThoai.Clear();
            txtDiaChi.Clear();
            radNam.Checked = radNu.Checked = false;
            cboLop.SelectedIndex = -1;
            cboLop.Text = "";
            dtNgaySinh.Value = DateTime.Today.AddYears(-18);
            for (int i = 0; i < dsMon.Items.Count; i++)
                dsMon.SetItemChecked(i, false);
        }

        // Thêm mới, ghi file ngay rồi cập nhật bảng.
        private void Them_Click(object sender, EventArgs e)
        {
            try
            {
                quanLy.Them(LayTuForm());
                boLoc = null;
                HienDanhSach();
                XoaO();
                txtMSSV.Focus();
            }
            catch (Exception loi)
            {
                BaoLoi(loi);
            }
        }

        // Cập nhật sinh viên đã chọn; vẫn giữ thông tin vừa sửa trên form.
        private void CapNhat_Click(object sender, EventArgs e)
        {
            try
            {
                if (maDangSua == null)
                    throw new ArgumentException("Hãy chọn một dòng sinh viên để cập nhật.");
                Student s = LayTuForm();
                quanLy.CapNhat(maDangSua, s);
                boLoc = null;
                HienDanhSach();
                foreach (ListViewItem dong in bangSinhVien.Items)
                    if (dong.Text == s.MSSV)
                    {
                        dong.Selected = true;
                        dong.EnsureVisible();
                        break;
                    }
            }
            catch (Exception loi)
            {
                BaoLoi(loi);
            }
        }

        // Chọn dòng dưới con trỏ khi nhấp chuột phải; không thay các checkbox đã đánh dấu.
        private void Bang_ChuotPhai(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;
            ListViewItem dong = bangSinhVien.GetItemAt(e.X, e.Y);
            if (dong == null || dong.Selected)
                return;
            foreach (ListViewItem cu in bangSinhVien.SelectedItems.Cast<ListViewItem>().ToArray())
                cu.Selected = false;
            dong.Selected = true;
        }

        // Ưu tiên xóa các checkbox đã đánh dấu; nếu không có thì xóa các dòng đang chọn.
        private void Xoa_Click(object sender, EventArgs e)
        {
            List<string> cacMa = bangSinhVien.CheckedItems.Cast<ListViewItem>().Select(d => d.Text).ToList();
            if (cacMa.Count == 0)
                cacMa = bangSinhVien.SelectedItems.Cast<ListViewItem>().Select(d => d.Text).ToList();
            if (cacMa.Count == 0)
            {
                MessageBox.Show("Hãy đánh dấu hoặc chọn sinh viên cần xóa.");
                return;
            }

            if (MessageBox.Show("Xóa " + cacMa.Count + " sinh viên?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            try
            {
                quanLy.Xoa(cacMa);
                HienDanhSach();
                XoaO();
            }
            catch (Exception loi)
            {
                BaoLoi(loi);
            }
        }

        // Mở điều kiện cũ để sửa; Hủy không đổi bộ lọc hoặc dữ liệu đang dùng.
        private void TimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                using (SearchForm form = new SearchForm(dsMon.Items.Cast<string>().ToArray(), boLoc))
                {
                    if (form.ShowDialog(this) != DialogResult.OK)
                        return;
                    boLoc = form.DieuKien.SaoChep();
                    HienDanhSach();
                    XoaO();
                    if (bangSinhVien.Items.Count == 0)
                        MessageBox.Show(this, "Không tìm thấy sinh viên phù hợp. Hãy mở Tìm kiếm và sửa hoặc bỏ các điều kiện.", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception loi)
            {
                BaoLoi(loi);
            }
        }

        // Xác định môn nằm dưới con trỏ để context menu xóa đúng môn.
        private void Mon_ChuotPhai(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                dsMon.SelectedIndex = dsMon.IndexFromPoint(e.Location);
        }

        // Hiện hộp thoại nhập tên môn, kiểm tra trùng và lưu danh mục.
        private void ThemMon_Click(object sender, EventArgs e)
        {
            using (Form form = new Form
            {
                Text = "Thêm môn",
                ClientSize = new Size(380, 130),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            }

            )
            {
                Label nhan = new Label
                {
                    Text = "Tên môn học:",
                    Location = new Point(15, 12),
                    Size = new Size(350, 20)
                };
                TextBox o = new TextBox
                {
                    Location = new Point(15, 38),
                    Size = new Size(350, 25)
                };
                Button dongY = new Button
                {
                    Text = "Thêm",
                    Location = new Point(155, 82),
                    DialogResult = DialogResult.OK
                };
                Button huy = new Button
                {
                    Text = "Hủy",
                    Location = new Point(265, 82),
                    DialogResult = DialogResult.Cancel
                };
                form.Controls.AddRange(new Control[] { nhan, o, dongY, huy });
                form.AcceptButton = dongY;
                form.CancelButton = huy;
                if (form.ShowDialog(this) != DialogResult.OK)
                    return;
                try
                {
                    string mon = o.Text.Trim();
                    if (mon.Length == 0)
                        throw new ArgumentException("Tên môn không được để trống.");
                    if (dsMon.Items.Cast<string>().Contains(mon, StringComparer.OrdinalIgnoreCase))
                        throw new ArgumentException("Môn đã tồn tại.");
                    List<string> cacMon = dsMon.Items.Cast<string>().ToList();
                    cacMon.Add(mon);
                    LuuMon(cacMon);
                    dsMon.Items.Add(mon, true);
                }
                catch (Exception loi)
                {
                    BaoLoi(loi);
                }
            }
        }

        // Xóa môn chưa được đăng ký; môn đã dùng cần được bỏ khỏi thông tin sinh viên trước.
        private void XoaMon_Click(object sender, EventArgs e)
        {
            if (dsMon.SelectedIndex < 0)
            {
                MessageBox.Show("Nhấp chuột phải vào môn cần xóa.");
                return;
            }

            string mon = (string)dsMon.SelectedItem;
            if (quanLy.DanhSach.Any(s => s.MonHoc.Contains(mon, StringComparer.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Môn đang được sinh viên đăng ký. Hãy cập nhật bỏ môn đó trước khi xóa danh mục.");
                return;
            }

            if (MessageBox.Show("Xóa môn " + mon + "?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            try
            {
                LuuMon(dsMon.Items.Cast<string>().Where(m => m != mon).ToList());
                dsMon.Items.Remove(mon);
            }
            catch (Exception loi)
            {
                BaoLoi(loi);
            }
        }

        // Lưu danh mục môn để lần chạy sau vẫn có các môn người dùng thêm.
        private void LuuMon(List<string> cacMon)
        {
            FileSinhVien.GhiAnToan(Path.Combine(thuMucDuLieu, "courses.txt"), delegate (string fileTam)
            {
                File.WriteAllLines(fileTam, cacMon, Encoding.UTF8);
            });
        }

        // Mở danh sách từ file người dùng chọn; file hỏng không thay danh sách đang dùng.
        private void MoFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog hopThoai = new OpenFileDialog
            {
                Filter = LocFile,
                InitialDirectory = thuMucDuLieu
            }

            )
            {
                if (hopThoai.ShowDialog(this) != DialogResult.OK)
                    return;
                try
                {
                    quanLy.TaiFile(hopThoai.FileName);
                    boLoc = null;
                    HienDanhSach();
                    XoaO();
                    GhiNhoFile();
                }
                catch (Exception loi)
                {
                    BaoLoi(loi);
                }
            }
        }

        // Lưu toàn bộ danh sách sang định dạng khác và chọn file đó làm file đang dùng.
        private void LuuThanh_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog hopThoai = new SaveFileDialog
            {
                Filter = LocFile,
                InitialDirectory = thuMucDuLieu,
                FileName = "students",
                AddExtension = true,
                OverwritePrompt = true
            }

            )
            {
                if (hopThoai.ShowDialog(this) != DialogResult.OK)
                    return;
                try
                {
                    quanLy.LuuThanh(hopThoai.FileName);
                    HienDanhSach();
                    XoaO();
                    GhiNhoFile();
                }
                catch (Exception loi)
                {
                    BaoLoi(loi);
                }
            }
        }

        // Nhớ file đã chọn để tải đúng danh sách trong lần chạy tiếp theo.
        private void GhiNhoFile()
        {
            try
            {
                File.WriteAllText(Path.Combine(thuMucDuLieu, "lastFile.txt"), quanLy.DuongDan, Encoding.UTF8);
            }
            catch (Exception loi)
            {
                MessageBox.Show("Dữ liệu đã được xử lý, nhưng không ghi nhớ được đường dẫn file: " + loi.Message);
            }
        }

        // Gọi đóng form; việc xác nhận thoát được xử lý tập trung bên dưới.
        private void Thoat_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Hỏi xác nhận cho cả nút Thoát và nút X của cửa sổ.
        private void Form_DangDong(object sender, FormClosingEventArgs e)
        {
            e.Cancel = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận thoát", MessageBoxButtons.YesNo) != DialogResult.Yes;
        }

        // Hiển thị lỗi nhập liệu hoặc đọc/ghi file bằng thông báo dễ hiểu.
        private void BaoLoi(Exception loi)
        {
            MessageBox.Show(this, loi.Message, "Không thể thực hiện", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
