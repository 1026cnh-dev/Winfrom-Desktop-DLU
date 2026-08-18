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
    public partial class frmBai3 : Form
    {
        public frmBai3()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnXemkq_Click(object sender, EventArgs e)
        {
            int a = int.Parse(txtSoA.Text);
            int b = int.Parse(txtSoB.Text);
            int n = int.Parse(txtSoC.Text);
             int kq = 0;
            if (rdTongAB.Checked)
            {
                TinhToan.Conghaiso(a, b, ref kq);
            }
            else
            {
               kq =  TinhToan.Tongdayso(n);
            }
            lblKetqua.Text = kq.ToString();
        }
    }
}
