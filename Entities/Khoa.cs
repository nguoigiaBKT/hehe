using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien.Entities
{
    internal class Khoa
    {
            public string MaKhoa { get; set; } // Mã khoa
            public string TenKhoa { get; set; } // Tên khoa

            // Constructor
            public Khoa(string maKhoa, string tenKhoa)
            {
                MaKhoa = maKhoa;
                TenKhoa = tenKhoa;
            }
        public Khoa()
        {
            
        }

        // Phương thức hiển thị thông tin khoa
        public override string ToString()
            {
                return$"Mã Khoa: {MaKhoa}, Tên Khoa: {TenKhoa}";
            }
        }
    
}
