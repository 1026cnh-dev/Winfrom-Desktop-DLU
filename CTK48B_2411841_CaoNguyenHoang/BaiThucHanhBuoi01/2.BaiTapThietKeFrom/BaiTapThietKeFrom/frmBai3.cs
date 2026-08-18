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
    public partial class frmBai3 : Form
    {
        public frmBai3()
        {
            InitializeComponent();
        }

        private void frmBai3_Load(object sender, EventArgs e)
        {
            Random rand =new Random();
            int so;
            for (int i=1; i<= 10; i++)
            {
                so=rand.Next(1, 100);
                listBox1.Items.Add(so);
            }
               

        }

        private void btnTimSo_Click(object sender, EventArgs e)
        {
            int soCanTim = int.Parse(txtSo.Text);
            lblKetQua.Text = "Khong tim Thay";

            foreach (int so in listBox1.Items)
            {
                if (so == soCanTim)
                {
                    lblKetQua.Text = "Tim thay";
                    break;
                }
                   
                
                    
            }
        }
    }
}
