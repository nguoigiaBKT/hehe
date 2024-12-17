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

namespace QuanLySinhVien
{
    public partial class Frm_QLKhoa : Form
    {
        QuanLiModel context = new QuanLiModel();
        List<Faculty> faculties;

        public Frm_QLKhoa()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            //đóng form quản lí khoa

            DialogResult result = MessageBox.Show("Bạn có muốn đóng Quản Lí Khoa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
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

        private void Frm_QLKhoa_Load(object sender, EventArgs e)
        {
            faculties = context.Faculty.ToList();
            SetGridViewStyle(dgvFaculty);
            BindGrid(faculties);

        }

        private void BindGrid(List<Faculty> faculties)
        {
            dgvFaculty.Rows.Clear();
            foreach (Faculty facul in faculties)
            {
                int index = dgvFaculty.Rows.Add();
                dgvFaculty.Rows[index].Cells[0].Value = facul.FacultyID;
                dgvFaculty.Rows[index].Cells[1].Value = facul.FacultyName;
                dgvFaculty.Rows[index].Cells[2].Value = facul.TotalProfessor;
            }

            CountGS();
        }

        private void CountGS()
        {
            //tính tổng số giáo sư 
            int total = 0;
            foreach (Faculty facul in faculties)
            {
                total += facul.TotalProfessor.Value;
            }

            txtGSCount.Text = total.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //kiểm tra dữ liệu nhập vào
            if (txtID.Text == "" || txtName.Text == "" || txtTotalProfessor.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //mã khoa không có ký tự đặc biệt
            if (txtID.Text.Any(char.IsPunctuation))
            {
                MessageBox.Show("Mã khoa không được chứa ký tự đặc biệt", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //tổng số giáo sư phải từ 0 đến 15
            if (int.Parse(txtTotalProfessor.Text) < 0 || int.Parse(txtTotalProfessor.Text) > 15)
            {
                MessageBox.Show("Tổng số giáo sư không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //tên khoa phải là chữ không có ký tự đặc biệt
            if (txtName.Text.Any(char.IsDigit))
            {
                MessageBox.Show("Tên khoa không được chứa số", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //lấy dữ liệu khoa từ txt
            var khoa = new Faculty()
            {
                FacultyID = int.Parse(txtID.Text),
                FacultyName = txtName.Text,
                TotalProfessor = int.Parse(txtTotalProfessor.Text)
            };

            //thêm khoa vào danh sách
            context.Faculty.Add(khoa);
            context.SaveChanges();

            //hiển thị danh sách khoa
            BindGrid(context.Faculty.ToList());
            //thông báo thêm thành công
            MessageBox.Show("Thêm mới dữ liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //reset form
            ResetForm();

        }

        private void ResetForm()
        {
            txtID.Text = "";
            txtName.Text = "";
            txtTotalProfessor.Text = "";
        }

        private void dgvFaculty_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //lấy dữ liệu từ dgv
            int index = e.RowIndex;
            if (index >= 0)
            {
                txtID.Text = dgvFaculty.Rows[index].Cells[0].Value.ToString();
                txtName.Text = dgvFaculty.Rows[index].Cells[1].Value.ToString();
                txtTotalProfessor.Text = dgvFaculty.Rows[index].Cells[2].Value.ToString();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            faculties = context.Faculty.ToList();
            //kiểm tra dữ liệu nhập vào
            if (txtID.Text == "")
            {
                MessageBox.Show("Vui lòng nhập mã khoa cần xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //kiểm tra mã khoa có tồn tại không
            var khoa = faculties.FirstOrDefault(x => x.FacultyID == int.Parse(txtID.Text));
            if (khoa != null)
            {
                //hỏi người dùng có chắc chắn xóa không
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    return;
                }

                //xóa khoa
                context.Faculty.Remove(khoa);
                context.SaveChanges();
                //hiển thị danh sách khoa
                BindGrid(context.Faculty.ToList());
                //thông báo xóa thành công
                MessageBox.Show("Xóa dữ liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //reset form
                ResetForm();
            }
            else
            {
                MessageBox.Show("Mã khoa không tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            faculties = context.Faculty.ToList();
            //kiểm tra mã khoa có tồn tại không
            var khoa = faculties.FirstOrDefault(x => x.FacultyID == int.Parse(txtID.Text));
            if (khoa != null)
            {
                //kiểm tra dữ liệu nhập vào
                if (txtID.Text == "" || txtName.Text == "" || txtTotalProfessor.Text == "")
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //cập nhật dữ liệu khoa
                khoa.FacultyName = txtName.Text;
                khoa.TotalProfessor = int.Parse(txtTotalProfessor.Text);
                context.SaveChanges();
                //hiển thị danh sách khoa
                BindGrid(context.Faculty.ToList());
                //thông báo cập nhật thành công
                MessageBox.Show("Cập nhật dữ liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //reset form
                ResetForm();
            }
            else
            {
                MessageBox.Show("Mã khoa không tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cboSapXep_SelectedValueChanged(object sender, EventArgs e)
        {
            //sắp xếp danh sách khoa theo số giáo sư
            if (cboSapXep.SelectedIndex == 0)
            {
                faculties = context.Faculty.OrderBy(x => x.TotalProfessor).ToList();
                BindGrid(faculties);
            }
            if(cboSapXep.SelectedIndex == 1)
            {
                faculties = context.Faculty.OrderByDescending(x => x.TotalProfessor).ToList();
                BindGrid(faculties);
            }
        }
    }
}
