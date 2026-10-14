using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BT1
{
    // 3. QUẢN LÝ: lưu file thành công rồi mới thay danh sách đang sử dụng.
    public class StudentManager
    {
        private List<Student> danhSach = new List<Student>();
        // Trả bản sao để nơi hiển thị hoặc gọi hàm không thay đổi dữ liệu chưa lưu.
        public List<Student> DanhSach
        {
            get
            {
                return danhSach.Select(s => s.SaoChep()).ToList();
            }
        }

        public string DuongDan { get; private set; }

        // Bắt đầu với danh sách rỗng; form sẽ nạp dữ liệu lúc mở chương trình.
        public StudentManager()
        {
        }

        // Nạp và kiểm tra toàn bộ file trước khi thay danh sách hiện tại.
        public void TaiFile(string duongDan)
        {
            List<Student> moi = FileSinhVien.Doc(duongDan);
            if (moi == null)
                throw new FormatException("File không chứa danh sách sinh viên.");
            HashSet<string> maDaCo = new HashSet<string>();
            foreach (Student s in moi)
            {
                if (s == null)
                    throw new FormatException("File có sinh viên rỗng.");
                s.KiemTra();
                if (!maDaCo.Add(s.MSSV))
                    throw new FormatException("Trùng MSSV trong file: " + s.MSSV);
            }

            danhSach = moi.Select(s => s.SaoChep()).ToList();
            DuongDan = Path.GetFullPath(duongDan);
        }

        // Thêm sinh viên mới và chặn MSSV trùng.
        public void Them(Student sinhVien)
        {
            sinhVien.KiemTra();
            if (DanhSach.Any(s => s.MSSV == sinhVien.MSSV))
                throw new ArgumentException("MSSV đã tồn tại. Hãy dùng Cập nhật.");
            List<Student> moi = new List<Student>(DanhSach);
            moi.Add(sinhVien.SaoChep());
            LuuThayDoi(moi);
        }

        // Cập nhật đúng sinh viên đã chọn, kể cả khi người dùng sửa MSSV.
        public void CapNhat(string maCu, Student sinhVien)
        {
            sinhVien.KiemTra();
            int viTri = DanhSach.FindIndex(s => s.MSSV == maCu);
            if (viTri < 0)
                throw new ArgumentException("Hãy chọn sinh viên cần cập nhật.");
            if (DanhSach.Any(s => s.MSSV == sinhVien.MSSV && s.MSSV != maCu))
                throw new ArgumentException("MSSV mới đã tồn tại.");
            List<Student> moi = new List<Student>(DanhSach);
            moi[viTri] = sinhVien.SaoChep();
            LuuThayDoi(moi);
        }

        // Xóa một hoặc nhiều MSSV và ghi danh sách còn lại xuống file.
        public void Xoa(List<string> cacMa)
        {
            LuuThayDoi(DanhSach.Where(s => !cacMa.Contains(s.MSSV)).ToList());
        }

        // Tìm theo điều kiện được truyền vào; null nghĩa là lấy toàn bộ danh sách.
        public List<Student> TimKiem(SearchCriteria dieuKien)
        {
            if (dieuKien == null)
                return new List<Student>(DanhSach);
            SearchCriteria banSao = dieuKien.SaoChep();
            banSao.KiemTra();
            return DanhSach.Where(banSao.PhuHop).ToList();
        }

        // Ghi trước, đổi danh sách sau; lỗi ghi không làm mất dữ liệu đang có.
        private void LuuThayDoi(List<Student> moi)
        {
            if (string.IsNullOrEmpty(DuongDan))
                throw new InvalidOperationException("Hãy mở hoặc lưu danh sách vào file trước.");
            FileSinhVien.Ghi(DuongDan, moi);
            danhSach = moi.Select(s => s.SaoChep()).ToList();
        }

        // Xuất sang TXT/XML/JSON và chuyển sang sử dụng file mới.
        public void LuuThanh(string duongDan)
        {
            FileSinhVien.Ghi(duongDan, DanhSach);
            DuongDan = Path.GetFullPath(duongDan);
        }
    }
}
