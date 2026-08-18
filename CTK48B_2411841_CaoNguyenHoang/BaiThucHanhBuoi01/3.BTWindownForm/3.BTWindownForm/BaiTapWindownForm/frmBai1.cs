using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai01
{
    public partial class frmBai1 : Form
    {
        public frmBai1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmBai1_Load(object sender, EventArgs e)
        {
            HangHoa hh = new HangHoa();
            hh.Mahang = "HH01";
            hh.TenHang = "Chuot";
            hh.DVT = "Cai";
            hh.Soluong = 4;
            hh.Dongia = 200000;
            //Hien thi thong bao
            lblThongbao.Text = hh.HienThi();

        }
    }
}
