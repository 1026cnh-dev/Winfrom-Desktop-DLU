using System;
using System.Windows.Forms;

namespace BT1
{
    internal static class Program
    {
        // Điểm khởi đầu: bật kiểu hiển thị Windows và chạy form chính.
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
