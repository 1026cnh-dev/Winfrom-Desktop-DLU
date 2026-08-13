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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void txtTen_TextChanged(object sender, EventArgs e)
        {
            txtsaochep.Text = txtTen.Text;
        }

        private void XacNhan_Click(object sender, EventArgs e)
        {
            var nhaptenban = txtTen.Text;
            MessageBox.Show($"Chao {nhaptenban} rất vui được gặp bạn");
        }

        private void saochep_Click(object sender, EventArgs e)
        {

        }
    }
}
