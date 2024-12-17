using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLySinhVien.Entities;
using QuanLySinhVien.Entities_db;


namespace QuanLySinhVien
{
    public partial class Frm_DanhSachSinhVien : Form
    {
        QuanLiModel context = new QuanLiModel();
        List<Faculty> lstKhoa;
        List<Student> lstSinhVien;
        public Frm_DanhSachSinhVien()
        {
            InitializeComponent();
            cmbKhoa.DisplayMember = "TenKhoa";
            cmbKhoa.ValueMember = "MaKhoa";
        }

        public void SetGridViewStyle(DataGridView dgview)
        {
            // Loại bỏ viền của DataGridView để tạo cảm giác nhẹ nhàng hơn
            dgview.BorderStyle = BorderStyle.None;

            // Đặt màu nền khi chọn dòng là màu DarkTurquoise để nổi bật lựa chọn
            dgview.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;

            // Sử dụng viền đơn giữa các ô, tạo cảm giác thanh thoát và đơn giản
            dgview.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            // Đặt màu nền chính của DataGridView là màu trắng, tạo độ tương phản cao với các hàng dữ liệu
            dgview.BackgroundColor = Color.White;

            // Đặt chế độ chọn toàn bộ hàng khi người dùng nhấp vào một ô, cải thiện trải nghiệm sử dụng
            dgview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void Frm_DanhSachSinhVien_Load(object sender, EventArgs e)
        {
            SetGridViewStyle(dgvDSSV);
            lstSinhVien = context.Student.ToList();
            lstKhoa = context.Faculty.ToList();
            FillFalcultyComboBox(lstKhoa);
            BindGrid(lstSinhVien);

            //khoa QTKD và giới tính nữ được chọn mặc định  
            cmbKhoa.SelectedValue = 6;
            rbWomen.Checked = true;
            CountSV();


        }

        private void BindGrid(List<Student> lstSinhVien)
        {
            dgvDSSV.Rows.Clear();
            foreach (Student sv in lstSinhVien)
            {
                int index = dgvDSSV.Rows.Add();
                dgvDSSV.Rows[index].Cells[0].Value = sv.StudentID;
                dgvDSSV.Rows[index].Cells[1].Value = sv.FullName;
                dgvDSSV.Rows[index].Cells[2].Value = sv.Gender;
                dgvDSSV.Rows[index].Cells[4].Value = sv.Faculty.FacultyName;
                dgvDSSV.Rows[index].Cells[3].Value = sv.AverageScore;
            }
            CountSV();
        }

        private void FillFalcultyComboBox(List<Faculty> lstKhoa)
        {
            this.cmbKhoa.DataSource = lstKhoa;
            this.cmbKhoa.DisplayMember = "FacultyName";
            this.cmbKhoa.ValueMember = "FacultyID";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            List<Student> lstSinhVien = context.Student.ToList();

            //kiểm tra dữ liệu nhập vào có hợp lệ không
            if (string.IsNullOrEmpty(txtID.Text) ||
                string.IsNullOrEmpty(txtName.Text) ||
                string.IsNullOrEmpty(txtAVG.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtID.Focus();
                return;
            }

            //kiểm tra mã số sinh viên có bị trùng không
            if (lstSinhVien.Any(s => s.StudentID == txtID.Text))
            {
                MessageBox.Show("Mã số sinh viên đã tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtID.Focus();
                return;
            }

            //kiểm tra điểm trung bình có nằm trong khoảng 0-10 không
            if (double.Parse(txtAVG.Text) < 0 || double.Parse(txtAVG.Text) > 10)
            {
                MessageBox.Show("Điểm trung bình sinh viên không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAVG.Focus();
                return;
            }

            //lấy dữ liệu từ txt
            var student = new Student
            {
                StudentID = txtID.Text,
                FullName = txtName.Text,
                Gender = rbMen.Checked ? "Male" : "Female",
                AverageScore = double.Parse(txtAVG.Text),
                FacultyID = int.Parse(cmbKhoa.SelectedValue.ToString()),


            };

            //thêm sinh viên vào danh sách
            context.Student.Add(student);
            context.SaveChanges();

            //hiển thị danh sách sinh viên lên dgvQLSinhVIen
            BindGrid(context.Student.ToList());
            MessageBox.Show("Thêm sinh viên thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);    

            //reset form 
            txtID.Text = txtName.Text = txtAVG.Text = "";
            cmbKhoa.SelectedValue = "QTKD";
            rbWomen.Checked = true;
        }

        private void CountSV()
        {
            lstSinhVien = context.Student.ToList();
            txtMenCount.Text = lstSinhVien.Count(s => s.Gender == "Male").ToString();
            txtWomenCount.Text = lstSinhVien.Count(s => s.Gender == "Female").ToString();
        }

        private void dgvDSSV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            lstSinhVien = context.Student.ToList();
            //kiểm tra người dùng có click vào dòng header không
            if (e.RowIndex < 0) return;
            //lấy thông tin sinh viên từ dòng đc click
            string maSinhVien = dgvDSSV.Rows[e.RowIndex].Cells[0].Value.ToString();
            Student sv = lstSinhVien.FirstOrDefault(s => s.StudentID == maSinhVien);
            if (sv != null)
            {
                //hiển thị thông tin sinh viên lên form
                txtID.Text = sv.StudentID;
                txtName.Text = sv.FullName;
                txtAVG.Text = sv.AverageScore.ToString();
                cmbKhoa.SelectedValue = sv.Faculty.FacultyID;
                rbMen.Checked = sv.Gender == "Male";
                rbWomen.Checked = sv.Gender == "Female";
            }
        }


        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            //không cho nhập số và kí tự đặc biệt
            if (char.IsDigit(e.KeyChar) || char.IsPunctuation(e.KeyChar) || char.IsSymbol(e.KeyChar))
            {
                errName.SetError(txtName, "Tên sinh viên không được chứa số hoặc kí tự đặc biệt");
                e.Handled = true;
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            lstSinhVien = context.Student.ToList();
            //kiểm tra mssv có trong danh sách hay không
            var sv = lstSinhVien.FirstOrDefault(s => s.StudentID == txtID.Text);
            if (sv != null)
            {
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    return;
                }
                //xóa sinh viên khỏi danh sách
                context.Student.Remove(sv);
                context.SaveChanges();

                //hiển thị danh sách
                BindGrid(context.Student.ToList());
                
                MessageBox.Show("Xóa dữ liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //hiển thị lại thống kê số lượng sinh viên nam, nữ
                
                //reset form
                txtID.Text = txtName.Text = txtAVG.Text = "";
                cmbKhoa.SelectedValue = "QTKD";
                rbWomen.Checked = true;
            }
            else
            {
                MessageBox.Show("Mã số sinh viên không tồn tại trong hệ thống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            //hỏi người dùng có muốn thoát chương trình không
            DialogResult result = MessageBox.Show("Bạn có muốn thoát chương trình không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            //mở form tìm kiếm
            Frm_TimKiem frm = new Frm_TimKiem();
            frm.ShowDialog();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            lstSinhVien = context.Student.ToList();
            try
            {
                //kiểm tra mã số sinh viên có tồn tại trong danh sách hay không
                string maSinhVien = txtID.Text;
                Student sv = lstSinhVien.FirstOrDefault(s => s.StudentID == maSinhVien);
                if (sv != null)
                {
                    //cập nhật thông tin sinh viên
                    sv.FullName = txtName.Text;
                    sv.AverageScore = double.Parse(txtAVG.Text);
                    sv.FacultyID = int.Parse(cmbKhoa.SelectedValue.ToString());
                    sv.Gender = rbMen.Checked ? "Male" : "Female";

                    //lưu thay đổi vào database
                    context.SaveChanges();

                    //hiển thị lại danh sách sinh viên
                    BindGrid(context.Student.ToList());
                    MessageBox.Show("Cập nhật dữ liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Mã số sinh viên không tồn tại trong hệ thống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //reset form
            txtID.Text = txtName.Text = txtAVG.Text = "";
            cmbKhoa.SelectedValue = "QTKD";
            rbWomen.Checked = true;
        }

        private void btnAddKhoa_Click(object sender, EventArgs e)
        {
            //mở form quản lý khoa
            Frm_QLKhoa frm = new Frm_QLKhoa();
            frm.ShowDialog();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMain frmMain = new FrmMain();
            frmMain.Show();
            this.Close();
        }
    }
}
