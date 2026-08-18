using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bai01
{
    internal class HangHoa
    {
        public string Mahang { get; set; }
        public string TenHang { get; set; }
        public string DVT {  get; set; }
        public int Soluong {  get; set; }
        public int Dongia { get; set; }
        //Ham khoi tao khong co tham so
        public HangHoa() { }
        // Hien thi tat ca thong tin
        public string HienThi()
        {
            return string.Format("{0},{1},{2},{3},{4}", Mahang, TenHang, DVT, Soluong, Dongia);
        }
    }
}
