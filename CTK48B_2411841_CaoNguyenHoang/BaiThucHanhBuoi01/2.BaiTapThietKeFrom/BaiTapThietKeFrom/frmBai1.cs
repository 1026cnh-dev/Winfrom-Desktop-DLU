using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTapThietKeFrom
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void rdXanh_CheckedChanged(object sender, EventArgs e)
        {
            txtDonGia.Text = "22000";
        }

        private void rdDo_CheckedChanged(object sender, EventArgs e)
        {
            txtDonGia.Text = "25000";
        }

        private void rdDen_CheckedChanged(object sender, EventArgs e)
        {
            txtDonGia.Text = "28000";
        }

        private void btnTinhTien_Click(object sender, EventArgs e)
        {
            int sotien =int.Parse(txtDonGia.Text) * int.Parse(txtSoluong.Text);
            lblSoTien.Text = sotien.ToString();
        }
    }
}
