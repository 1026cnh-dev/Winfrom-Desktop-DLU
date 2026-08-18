using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai01
{
    public partial class frmBai2 : Form
    {
        public frmBai2()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmBai2_Load(object sender, EventArgs e)
        {

        }

        private void cbbTenHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            var stt = cbbTenHang.SelectedIndex;
            switch (stt)
            {
                case 0:
                    txtDongia.Text = "100000";
                    break;
                case 1:
                    txtDongia.Text = "200000";
                    break;
                case 2:
                    txtDongia.Text = "300000";
                    break;

            }
        }

        private void btnTinhTien_Click(object sender, EventArgs e)
        {
            int dongia = int.Parse(txtDongia.Text);
            int soluong= int.Parse(txtSoluong.Text);
            double thanhtien = dongia*soluong;
            if (rdChuyenKhoan.Checked)
            {
                thanhtien = dongia * soluong * 0.95;
            }
           
            lblSoTien.Text= thanhtien.ToString();
        }

        private void rdTienmat_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
