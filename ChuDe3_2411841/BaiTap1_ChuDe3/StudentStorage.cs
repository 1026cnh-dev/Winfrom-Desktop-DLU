using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace BT1
{
    // 2. ĐỌC/GHI FILE: mỗi định dạng được xử lý bằng một hàm rõ ràng.
    public static class FileSinhVien
    {
        // Chọn cách đọc theo phần mở rộng; không bỏ qua file sai định dạng.
        public static List<Student> Doc(string duongDan)
        {
            switch (Path.GetExtension(duongDan).ToLowerInvariant())
            {
                case ".txt":
                    return DocTXT(duongDan);
                case ".xml":
                    return DocXML(duongDan);
                case ".json":
                    return DocJSON(duongDan);
                default:
                    throw new ArgumentException("Chỉ hỗ trợ file TXT, XML hoặc JSON.");
            }
        }

        // Đọc TXT: mỗi dòng gồm 10 trường cách nhau bằng tab; các môn cách bằng |.
        public static List<Student> DocTXT(string duongDan)
        {
            List<Student> danhSach = new List<Student>();
            int soDong = 0;
            foreach (string dong in File.ReadAllLines(duongDan, Encoding.UTF8))
            {
                soDong++;
                if (string.IsNullOrWhiteSpace(dong))
                    continue;
                string[] cot = dong.Split('\t');
                if (cot.Length != 10 || (cot[3] != "1" && cot[3] != "0"))
                    throw new FormatException("Dòng " + soDong + " của TXT không hợp lệ.");
                danhSach.Add(new Student { MSSV = GiaiMa(cot[0]), HoVaTenLot = GiaiMa(cot[1]), Ten = GiaiMa(cot[2]), Nam = cot[3] == "1", NgaySinh = DateTime.ParseExact(cot[4], "dd/MM/yyyy", CultureInfo.InvariantCulture), Lop = GiaiMa(cot[5]), CMND = GiaiMa(cot[6]), DienThoai = GiaiMa(cot[7]), DiaChi = GiaiMa(cot[8]), MonHoc = cot[9].Split('|').Where(m => m.Length > 0).Select(GiaiMa).ToList() });
            }

            return danhSach;
        }

        // Đọc XML bằng XmlSerializer; không cho phép nạp DTD từ file ngoài.
        public static List<Student> DocXML(string duongDan)
        {
            XmlReaderSettings caiDat = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null
            };
            using (XmlReader doc = XmlReader.Create(duongDan, caiDat))
                return (List<Student>)new XmlSerializer(typeof(List<Student>)).Deserialize(doc);
        }

        // Tạo bộ đọc/ghi JSON; bản mới lưu ngày sinh rõ ràng theo yyyy-MM-dd.
        private static DataContractJsonSerializer TaoBoJSON(bool dinhDangCu)
        {
            DataContractJsonSerializerSettings caiDat = new DataContractJsonSerializerSettings();
            caiDat.MaxItemsInObjectGraph = int.MaxValue;
            if (!dinhDangCu)
                caiDat.DateTimeFormat = new DateTimeFormat("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return new DataContractJsonSerializer(typeof(List<Student>), caiDat);
        }

        // Đọc JSON mới; vẫn hỗ trợ file cũ dùng ngày dạng /Date(...)/.
        public static List<Student> DocJSON(string duongDan)
        {
            string noiDung = File.ReadAllText(duongDan, Encoding.UTF8);
            bool dinhDangCu = Regex.IsMatch(noiDung, @"""NgaySinh""\s*:\s*""\\?/Date\(");
            using (MemoryStream doc = new MemoryStream(Encoding.UTF8.GetBytes(noiDung)))
            {
                List<Student> danhSach = (List<Student>)TaoBoJSON(dinhDangCu).ReadObject(doc);
                if (dinhDangCu && danhSach != null)
                    foreach (Student s in danhSach)
                        if (s != null)
                            s.NgaySinh = s.NgaySinh.ToLocalTime().Date;
                return danhSach;
            }
        }

        // Ghi toàn bộ danh sách theo định dạng của file đang dùng.
        public static void Ghi(string duongDan, List<Student> danhSach)
        {
            string duoiFile = Path.GetExtension(duongDan).ToLowerInvariant();
            GhiAnToan(duongDan, delegate (string fileTam)
            {
                switch (duoiFile)
                {
                    case ".txt":
                        GhiTXT(fileTam, danhSach);
                        break;
                    case ".xml":
                        // Entitize giữ nguyên ký tự xuống dòng trong dữ liệu khi đọc lại XML.
                        XmlWriterSettings caiDat = new XmlWriterSettings
                        {
                            Encoding = Encoding.UTF8,
                            Indent = true,
                            NewLineHandling = NewLineHandling.Entitize
                        };
                        using (XmlWriter ghi = XmlWriter.Create(fileTam, caiDat))
                            new XmlSerializer(typeof(List<Student>)).Serialize(ghi, danhSach);
                        break;
                    case ".json":
                        using (FileStream ghi = File.Create(fileTam))
                            TaoBoJSON(false).WriteObject(ghi, danhSach);
                        break;
                    default:
                        throw new ArgumentException("Chỉ hỗ trợ file TXT, XML hoặc JSON.");
                }
            });
        }

        // Ghi mỗi sinh viên thành một dòng TXT, các cột ngăn bằng tab.
        private static void GhiTXT(string duongDan, List<Student> danhSach)
        {
            List<string> cacDong = new List<string>();
            foreach (Student sinhVien in danhSach)
                cacDong.Add(TaoDongTXT(sinhVien));
            File.WriteAllLines(duongDan, cacDong, Encoding.UTF8);
        }

        // Chuyển một sinh viên thành đúng 10 cột của định dạng TXT.
        private static string TaoDongTXT(Student sinhVien)
        {
            string[] cot =
            {
                MaHoa(sinhVien.MSSV),
                MaHoa(sinhVien.HoVaTenLot),
                MaHoa(sinhVien.Ten),
                sinhVien.Nam ? "1" : "0",
                sinhVien.NgaySinh.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                MaHoa(sinhVien.Lop),
                MaHoa(sinhVien.CMND),
                MaHoa(sinhVien.DienThoai),
                MaHoa(sinhVien.DiaChi),
                string.Join("|", sinhVien.MonHoc.Select(MaHoa))
            };
            return string.Join("\t", cot);
        }

        // Ghi file tạm trước; chỉ thay file cũ khi đã ghi thành công toàn bộ dữ liệu.
        public static void GhiAnToan(string duongDan, Action<string> ghiFileTam)
        {
            string fileTam = duongDan + "." + Guid.NewGuid().ToString("N") + ".tmp";
            try
            {
                ghiFileTam(fileTam);
                if (File.Exists(duongDan))
                    File.Replace(fileTam, duongDan, null);
                else
                    File.Move(fileTam, duongDan);
            }
            finally
            {
                if (File.Exists(fileTam))
                    try
                    {
                        File.Delete(fileTam);
                    }
                    catch (IOException)
                    {
                    }
                    catch (UnauthorizedAccessException)
                    {
                    }
            }
        }

        // Mã hóa ký tự đặc biệt để tab, xuống dòng và | không làm hỏng cấu trúc TXT.
        public static string MaHoa(string chuoi)
        {
            return (chuoi ?? "").Replace("\\", "\\\\").Replace("\t", "\\t").Replace("\r", "\\r").Replace("\n", "\\n").Replace("|", "\\p");
        }

        // Khôi phục chuỗi đã mã hóa khi đọc TXT.
        public static string GiaiMa(string chuoi)
        {
            StringBuilder ketQua = new StringBuilder();
            for (int i = 0; i < chuoi.Length; i++)
            {
                if (chuoi[i] != '\\')
                {
                    ketQua.Append(chuoi[i]);
                    continue;
                }

                if (++i == chuoi.Length)
                    throw new FormatException("Ký tự escape không hợp lệ.");
                switch (chuoi[i])
                {
                    case '\\':
                        ketQua.Append('\\');
                        break;
                    case 't':
                        ketQua.Append('\t');
                        break;
                    case 'r':
                        ketQua.Append('\r');
                        break;
                    case 'n':
                        ketQua.Append('\n');
                        break;
                    case 'p':
                        ketQua.Append('|');
                        break;
                    default:
                        throw new FormatException("Ký tự escape không hợp lệ.");
                }
            }

            return ketQua.ToString();
        }
    }
}
