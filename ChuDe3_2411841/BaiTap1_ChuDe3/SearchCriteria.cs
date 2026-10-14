using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BT1
{
    // Chứa điều kiện tìm kiếm, không phụ thuộc vào giao diện hoặc hộp thoại.
    public class SearchCriteria
    {
        public string MSSV { get; set; }
        public string HoVaTenLot { get; set; }
        public string Ten { get; set; }
        public string Lop { get; set; }
        public string CMND { get; set; }
        public string DienThoai { get; set; }
        public string DiaChi { get; set; }
        // 0: không lọc giới tính, 1: Nam, 2: Nữ.
        public int GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public List<string> MonHoc { get; set; }
        public bool GhepAND { get; set; }

        // Khởi tạo bộ lọc trống; không nhập điều kiện thì xem toàn bộ danh sách.
        public SearchCriteria()
        {
            MonHoc = new List<string>();
            GhepAND = true;
        }

        // Tạo bản sao để sửa điều kiện trên hộp thoại không làm thay đổi bộ lọc đang dùng.
        public SearchCriteria SaoChep()
        {
            SearchCriteria banSao = (SearchCriteria)MemberwiseClone();
            banSao.MonHoc = new List<string>(MonHoc ?? new List<string>());
            return banSao;
        }

        // Kiểm tra điều kiện tìm kiếm; ô số cho phép nhập một phần, không bắt nhập đủ.
        public void KiemTra()
        {
            MSSV = (MSSV ?? "").Trim();
            HoVaTenLot = (HoVaTenLot ?? "").Trim();
            Ten = (Ten ?? "").Trim();
            Lop = (Lop ?? "").Trim();
            CMND = (CMND ?? "").Trim();
            DienThoai = (DienThoai ?? "").Trim();
            DiaChi = (DiaChi ?? "").Trim();
            KiemTraOso(MSSV, 7, "MSSV", "MSSV");
            KiemTraOso(CMND, 9, "CMND", "Số CMND");
            if (DienThoai.Length > 0)
            {
                if (!Regex.IsMatch(DienThoai, @"^[0-9\s().-]+$"))
                    throw new ArgumentException("Điện thoại chỉ được nhập chữ số, khoảng trắng, dấu chấm, dấu gạch hoặc ngoặc.", "DienThoai");
                DienThoai = Regex.Replace(DienThoai, @"[^0-9]", "");
                if (DienThoai.Length == 0)
                    throw new ArgumentException("Hãy nhập ít nhất một chữ số điện thoại hoặc để trống ô này.", "DienThoai");
                KiemTraOso(DienThoai, 10, "DienThoai", "Điện thoại");
            }

            if (GioiTinh < 0 || GioiTinh > 2)
                throw new ArgumentException("Hãy chọn Không dùng, Nam hoặc Nữ.", "GioiTinh");
            if (NgaySinh.HasValue)
            {
                NgaySinh = NgaySinh.Value.Date;
                if (NgaySinh.Value < new DateTime(1900, 1, 1) || NgaySinh.Value > DateTime.Today)
                    throw new ArgumentException("Ngày sinh phải từ năm 1900 đến hôm nay.", "NgaySinh");
            }

            MonHoc = (MonHoc ?? new List<string>()).Where(m => !string.IsNullOrWhiteSpace(m)).Select(m => m.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        }

        // Chỉ kiểm tra ô đã nhập: MSSV tối đa 7 số, CMND 9 số, điện thoại 10 số.
        private static void KiemTraOso(string noiDung, int toiDa, string tenTruong, string nhan)
        {
            if (noiDung.Length == 0)
                return;
            if (noiDung.Length > toiDa || !Regex.IsMatch(noiDung, @"^[0-9]+$"))
                throw new ArgumentException(nhan + " tìm kiếm phải gồm từ 1 đến " + toiDa + " chữ số.", tenTruong);
        }

        // Kiểm tra một sinh viên với điều kiện đã được KiemTra ở lớp quản lý.
        public bool PhuHop(Student sinhVien)
        {
            if (sinhVien == null)
                return false;
            List<bool> ketQua = new List<bool>();
            ThemDieuKienChu(ketQua, sinhVien.MSSV, MSSV);
            ThemDieuKienChu(ketQua, sinhVien.HoVaTenLot, HoVaTenLot);
            ThemDieuKienChu(ketQua, sinhVien.Ten, Ten);
            ThemDieuKienChu(ketQua, sinhVien.Lop, Lop);
            ThemDieuKienChu(ketQua, sinhVien.CMND, CMND);
            ThemDieuKienChu(ketQua, sinhVien.DienThoai, DienThoai);
            ThemDieuKienChu(ketQua, sinhVien.DiaChi, DiaChi);
            if (GioiTinh != 0)
                ketQua.Add(sinhVien.Nam == (GioiTinh == 1));
            if (NgaySinh.HasValue)
                ketQua.Add(sinhVien.NgaySinh.Date == NgaySinh.Value.Date);
            foreach (string mon in MonHoc ?? new List<string>())
            {
                if (string.IsNullOrWhiteSpace(mon))
                    continue;
                ketQua.Add(sinhVien.MonHoc != null && sinhVien.MonHoc.Any(m => string.Equals((m ?? "").Trim(), mon.Trim(), StringComparison.OrdinalIgnoreCase)));
            }

            // Không có điều kiện phải trả toàn bộ danh sách, kể cả khi chọn OR.
            if (ketQua.Count == 0)
                return true;
            return GhepAND ? ketQua.All(k => k) : ketQua.Any(k => k);
        }

        // Bỏ qua ô trống; tìm chữ theo một phần và không phân biệt dấu hoặc hoa/thường.
        private static void ThemDieuKienChu(List<bool> ketQua, string giaTri, string canTim)
        {
            if (string.IsNullOrWhiteSpace(canTim))
                return;
            ketQua.Add(BoDau(giaTri).IndexOf(BoDau(canTim.Trim()), StringComparison.OrdinalIgnoreCase) >= 0);
        }

        // Chuẩn hóa tiếng Việt: 'nguyen' khớp 'Nguyễn', 'da lat' khớp 'Đà Lạt'.
        public static string BoDau(string chuoi)
        {
            StringBuilder ketQua = new StringBuilder();
            foreach (char c in (chuoi ?? "").Normalize(NormalizationForm.FormD))
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    ketQua.Append(c == 'đ' ? 'd' : c == 'Đ' ? 'D' : c);
            return ketQua.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
