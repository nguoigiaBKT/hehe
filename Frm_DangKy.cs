using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLySinhVien.Account_DB;
using BCrypt.Net;

namespace QuanLySinhVien
{
    public partial class Frm_DangKy : Form
    {
        AccountData acc = new AccountData();
        private FrmMain frmMain1;

        public Frm_DangKy(FrmMain frmMain)
        {
            InitializeComponent();
            frmMain1 = frmMain;
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            //kiểm tra dữ liệu nhập vào
            if (txtTaiKhoan.Text == "" || txtMatKhau.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //kiểm tra tài khoản đã tồn tại chưa
            if (CheckTaiKhoan(txtTaiKhoan.Text))
            {
                MessageBox.Show("Tài khoản đã tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //lưu tài khoản vào database
            Users user = new Users();
            user.Username= txtTaiKhoan.Text;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(txtMatKhau.Text);
            acc.Users.Add(user);
            acc.SaveChanges();
            MessageBox.Show("Đăng ký thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //hiển thị FrmMain
            FrmMain frmMain = new FrmMain();
            frmMain.Show();

            this.Hide();

        }
        
        private bool CheckTaiKhoan(string taiKhoan)
        {
            var user = acc.Users.Where(u => u.Username == taiKhoan).FirstOrDefault();
            if (user != null)
            {
                return true;
            }
            return false;
        }

        private void Frm_DangKy_FormClosing(object sender, FormClosingEventArgs e)
        {
                frmMain1.Show();
        }
    }

}
