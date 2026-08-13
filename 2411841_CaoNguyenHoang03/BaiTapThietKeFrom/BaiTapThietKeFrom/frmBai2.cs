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
    public partial class frmBai2 : Form
    {
        List<string> list = new List<string>();
        public frmBai2()
        {
            InitializeComponent();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var tu = txtTuMoi.Text;
            var nghia = txtNghia.Text;
            listBox1.Items.Add(tu);
            list.Add(nghia);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var stt =listBox1.SelectedIndex;
            txtHienThiNghia.Text = list[stt];
        }
    }
}
