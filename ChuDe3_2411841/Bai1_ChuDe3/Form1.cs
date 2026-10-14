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
using System.Windows.Forms.VisualStyles;

namespace ChuDe3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void WriteReadText(string filename, string[] text)
        {
            // Ghi dữ liệu text vào file filename
            File.WriteAllLines(filename, text);
            //Đọc dữ liệu từ file filename hiển thị ra màn hình
            foreach (string s in File.ReadAllLines(filename))
            {
                Console.WriteLine(s);
            }
            Console.ReadLine();
        }
        private void btnTxt_Click(object sender, EventArgs e)
        {
            string path = System.IO.Path.Combine(Application.StartupPath, "demo.txt");
            string[] text = { "Dong 1", "Dong 2", "Dong 3" };
            System.IO.File.WriteAllLines(path, text);

            string kq = "";
            foreach (string s in System.IO.File.ReadAllLines(path))
                kq += s + "\r\n";
            MessageBox.Show(kq, "Nội dung file");
        }
    }
}
