using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien.Entities
{
    internal class SinhVien
    {
        public string MaSV { get; set; } // Mã sinh viên
        public string Ten { get; set; } // Tên sinh viên
        public double Diem { get; set; } // Điểm
        public string MaKhoa { get; set; } // Mã khoa
        public string GioiTinh { get; set; } // Giới tính
        public int rank { get; set; } // Xếp loại

        // Constructor
        public SinhVien()
        {
            
        }
        public SinhVien(string maSV, string ten, double diem, string maKhoa, string gioiTinh)
        {
            MaSV = maSV;
            Ten = ten;
            Diem = diem;
            MaKhoa = maKhoa;
            GioiTinh = gioiTinh;
            rank = 0;
        }

        // Phương thức hiển thị thông tin sinh viên
        public override string ToString()
        {
            return $"Mã SV: {MaSV}, Tên: {Ten}, Điểm: {Diem}, Mã Khoa: {MaKhoa}, Giới tính: {GioiTinh}";
        }

        // phuongthuc kiem tra diem
        public bool KiemTraDiem()
        {
                        return Diem >= 0 && Diem <= 10;
        }
    }
}
