using QuanLySinhVien.Entities_db;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace QuanLySinhVien
{
    public partial class Frm_TimKiem : Form
    {
        QuanLiModel context = new QuanLiModel();
        List<Student> students;
        public Frm_TimKiem()
        {
            InitializeComponent();
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
        private void FillFalcultyComboBox(List<Faculty> lstKhoa)
        {
            this.cboKhoa.DataSource = lstKhoa;
            this.cboKhoa.DisplayMember = "FacultyName";
            this.cboKhoa.ValueMember = "FacultyID";
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Frm_TimKiem_Load(object sender, EventArgs e)
        {
            SetGridViewStyle(dgvSearch);
            students = context.Student.ToList();
            txtID.Text = "";
            txtName.Text = "";
            txtSearchCount.Text = "0";
            BindGrid(students);
            FillFalcultyComboBox(context.Faculty.ToList());
            cboKhoa.SelectedValue= "";

        }

        private void BindGrid(List<Student> lstSinhVien)
        {
            dgvSearch.Rows.Clear();
            foreach (Student sv in lstSinhVien)
            {
                int index = dgvSearch.Rows.Add();
                dgvSearch.Rows[index].Cells[0].Value = sv.StudentID;
                dgvSearch.Rows[index].Cells[1].Value = sv.FullName;
                dgvSearch.Rows[index].Cells[2].Value = sv.Gender;
                dgvSearch.Rows[index].Cells[4].Value = sv.Faculty.FacultyName;
                dgvSearch.Rows[index].Cells[3].Value = sv.AverageScore;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" && cboKhoa.SelectedIndex == -1 && txtID.Text == "0")
            {
                MessageBox.Show("Vui lòng nhập thông tin tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //tìm kiếm sinh viên theo tên
                students = context.Student.Where(sv => sv.FullName.Contains(txtName.Text) && 
                sv.StudentID.Contains(txtID.Text) && sv.Faculty.FacultyName.Contains(cboKhoa.Text)).ToList();
            if (rbMen.Checked)
            {
                students = students.Where(sv => sv.Gender == "Male").ToList();
            }
            if (rbWomen.Checked)
            {
                students = students.Where(sv => sv.Gender == "Female").ToList();
            }
                BindGrid(students);
            txtSearchCount.Text = students.Count.ToString();
            if(students.Count == 0)
            {
                MessageBox.Show("Không tìm thấy sinh viên nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        private void btnDelete_Click(object sender, EventArgs e)
        {
            Frm_TimKiem_Load(sender, e);
        }

        // Biến lưu trạng thái đã chọn của RadioButton
        private RadioButton lastCheckedRadioButton = null;

        // Xử lý sự kiện Click của RadioButton Nam
        private void rbtnMale_Click(object sender, EventArgs e)
        {
            ToggleRadioButton(rbMen);
        }

        // Xử lý sự kiện Click của RadioButton Nữ
        private void rbtnFemale_Click(object sender, EventArgs e)
        {
            ToggleRadioButton(rbWomen);
        }

        // Hàm xử lý logic hủy chọn RadioButton khi click lại
        private void ToggleRadioButton(RadioButton radioButton)
        {
            if (radioButton.Checked && lastCheckedRadioButton == radioButton)
            {
                // Hủy chọn RadioButton nếu nó đã được chọn trước đó
                radioButton.Checked = false;
                lastCheckedRadioButton = null;
            }
            else
            {
                // Gán RadioButton hiện tại là đã chọn
                radioButton.Checked = true;
                lastCheckedRadioButton = radioButton;
            }
        }


    }
}
