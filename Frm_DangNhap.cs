using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLySinhVien
{
    public partial class Frm_DangNhap : Form
    {
        private FrmMain frmMain1;
        public Frm_DangNhap(FrmMain frmMain)
        {
            InitializeComponent();
            frmMain1 = frmMain;
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            //kiểm tra dữ liệu nhập vào
            if (txtTaiKhoan.Text == "" || txtMatKhau.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //kiểm tra tài khoản và mật khẩu chính xác chưa
            if (CheckTaiKhoan(txtTaiKhoan.Text, txtMatKhau.Text) == 2)
            {
                MessageBox.Show("Đăng nhập thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //hiển thị FrmTimKiem
                Frm_TimKiem frmTimKiem = new Frm_TimKiem();
                frmTimKiem.Show();
                this.Hide();
                FrmMain frmMain = new FrmMain();
                frmMain.Hide();
            }
            else if(CheckTaiKhoan(txtTaiKhoan.Text, txtMatKhau.Text) == 1)
            {
                MessageBox.Show("Đăng nhập thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //hiển thị FrmQuanLy
                Frm_DanhSachSinhVien frmQuanLy = new Frm_DanhSachSinhVien();
                frmQuanLy.Show();
                this.Hide();
                FrmMain frmMain = new FrmMain();
                frmMain.Hide();
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private int CheckTaiKhoan(string taiKhoan, string matKhau)
        {
            //kiểm tra tài khoản và mật khẩu có chính xác không
            using (var acc = new Account_DB.AccountData())
            {
                var user = acc.Users.Where(u => u.Username == taiKhoan).FirstOrDefault();
                if (user != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(matKhau, user.PasswordHash) && user.UserID == 1)
                    {
                        return 1;
                    }
                    else if (BCrypt.Net.BCrypt.Verify(matKhau, user.PasswordHash))
                    {
                        return 2;
                    }
                }
                return 0;
            }
        }

        private void Frm_DangNhap_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmMain1.Show();
        }
    }
}
