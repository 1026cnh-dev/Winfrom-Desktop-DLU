using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BT1
{
    // 1. LỚP SINH VIÊN: lưu thông tin và kiểm tra dữ liệu.
    public class Student
    {
        public string MSSV { get; set; }
        public string HoVaTenLot { get; set; }
        public string Ten { get; set; }
        public bool Nam { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Lop { get; set; }
        public string CMND { get; set; }
        public string DienThoai { get; set; }
        public string DiaChi { get; set; }
        public List<string> MonHoc { get; set; }

        // Khởi tạo danh sách môn rỗng cho mỗi sinh viên mới.
        public Student()
        {
            MonHoc = new List<string>();
        }

        // Sao chép cả danh sách môn để dữ liệu bên ngoài không sửa trực tiếp dữ liệu quản lý.
        public Student SaoChep()
        {
            Student banSao = (Student)MemberwiseClone();
            banSao.MonHoc = new List<string>(MonHoc ?? new List<string>());
            return banSao;
        }

        // Suy ra năm nhập học: CTK50 -> 2026, CTK49 -> 2025; hỗ trợ CNTT2026.
        public static int NamNhapHoc(string lop)
        {
            Match ketQua = Regex.Match(lop ?? "", @"^CTK([0-9]{2})[A-Za-z0-9]*$", RegexOptions.IgnoreCase);
            if (ketQua.Success)
                return 1976 + int.Parse(ketQua.Groups[1].Value);
            ketQua = Regex.Match(lop ?? "", @"(?:^|[^0-9])(20[0-9]{2})(?:[^0-9]|$)");
            if (ketQua.Success)
                return int.Parse(ketQua.Groups[1].Value);
            throw new ArgumentException("Không xác định được năm nhập học. Nhập lớp như CTK50 hoặc CNTT2026.");
        }

        // Kiểm tra đủ thông tin, số chữ số và MSSV đúng năm nhập học của lớp.
        public void KiemTra()
        {
            if (!Regex.IsMatch(MSSV ?? "", @"^[0-9]{7}$"))
                throw new ArgumentException("MSSV phải có đúng 7 chữ số.");
            if (string.IsNullOrWhiteSpace(HoVaTenLot) || string.IsNullOrWhiteSpace(Ten) || string.IsNullOrWhiteSpace(Lop) || string.IsNullOrWhiteSpace(DiaChi))
                throw new ArgumentException("Hãy nhập đầy đủ họ và tên lót, tên, lớp và địa chỉ.");
            if (!Regex.IsMatch(CMND ?? "", @"^[0-9]{9}$"))
                throw new ArgumentException("CMND phải có đúng 9 chữ số.");
            if (!Regex.IsMatch(DienThoai ?? "", @"^[0-9]{10}$"))
                throw new ArgumentException("Điện thoại phải có đúng 10 chữ số.");
            if (NgaySinh.Date < new DateTime(1900, 1, 1) || NgaySinh.Date > DateTime.Today)
                throw new ArgumentException("Ngày sinh phải từ năm 1900 đến hôm nay.");
            if (MonHoc == null || MonHoc.Count == 0 || MonHoc.Any(string.IsNullOrWhiteSpace))
                throw new ArgumentException("Hãy chọn ít nhất một môn học.");
            string dauMSSV = (NamNhapHoc(Lop) % 100).ToString("00") + "10";
            if (!MSSV.StartsWith(dauMSSV))
                throw new ArgumentException("MSSV của lớp " + Lop + " phải có dạng " + dauMSSV + "CCC.");
        }
    }
}
