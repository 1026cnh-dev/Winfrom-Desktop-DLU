using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bai01
{
    internal class TinhToan
    {
        public static void Conghaiso(int a,int b, ref int c   )
        {
            c = a + b;
        }
        public static int Tongdayso(int n)
        {
            int tong = 0 ;
            for (int i = 0; i < n; i++)
            {
                tong += i;
            }
            return tong;
        }
    }
}
