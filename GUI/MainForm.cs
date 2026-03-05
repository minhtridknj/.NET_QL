using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QuanLyAoQuan.BLL;
using QuanLyAoQuan.DAL;
using QuanLyAoQuan.DTO;

namespace QuanLyAoQuan.GUI
{
    /// <summary>
    /// Giao diện chính của chương trình.
    /// Minh họa cách nhận List<AoQuanDTO> từ BLL và hiển thị linh hoạt lên 2 View khác nhau.
    /// </summary>
    public partial class MainForm : Form
    {
        // Khai báo BLL. Tiêm phụ thuộc (Dependency Injection) IAoQuanDAL tại đây
        // (Hoặc có thể dùng Autofac, Ninject... nếu cần)
        private AoQuanBLL _bll;

        // Giả định form có các Controls sau (được vẽ bên Designer):
        // DataGridView dgvDanhSachAoQuan
        // ComboBox cboDanhSachAoQuan
        // TextBox txtTimKiem
        // Button btnTimKiemSQL, btnTimKiemLINQ, btnTaiDanhSach

        public MainForm()
        {
            InitializeComponent();

            // Thiết lập kết nối: Khởi tạo DAL (có thể đổi sang MySQL DAL ở đây) và gán cho BLL
            IAoQuanDAL dal = new SqlServerAoQuanDAL();
            _bll = new AoQuanBLL(dal);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Tải dữ liệu ban đầu
            TaiDanhSach();
        }

        /// <summary>
        /// Hàm tải danh sách sản phẩm hiển thị trên 2 View khác nhau
        /// </summary>
        private void TaiDanhSach()
        {
            // Tầng BLL trả về List<AoQuanDTO>
            List<AoQuanDTO> danhSach = _bll.GetAllAoQuan();
            HienThiLenViews(danhSach);
        }

        /// <summary>
        /// Yêu cầu 3: Hiển thị List<DTO> lên 2 View khác nhau.
        /// </summary>
        private void HienThiLenViews(List<AoQuanDTO> danhSach)
        {
            // View 1: DataGridView - Hiển thị toàn bộ thông tin chi tiết
            dgvDanhSachAoQuan.DataSource = null; // Reset binding
            dgvDanhSachAoQuan.DataSource = danhSach;

            // View 2: ComboBox - Chỉ hiển thị tên sản phẩm thông qua DisplayMember
            cboDanhSachAoQuan.DataSource = null; // Reset binding
            cboDanhSachAoQuan.DataSource = danhSach;
            cboDanhSachAoQuan.DisplayMember = "TenSanPham"; // Tên thuộc tính muốn hiển thị
            cboDanhSachAoQuan.ValueMember = "Id"; // Giá trị ẩn (thường là Primary Key)

            // Nếu không set DisplayMember, ComboBox sẽ tự gọi hàm ToString()
            // mà chúng ta đã Override bên trong class AoQuanDTO.
        }

        // Sự kiện tìm kiếm dùng SQL Thuần (Yêu cầu 2)
        private void btnTimKiemSQL_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();

            // Tốc độ truy vấn trên CSDL sẽ nhanh nhưng phụ thuộc đường truyền mạng
            List<AoQuanDTO> ketQua = _bll.TimKiemBangSqlThuan(keyword);

            // Cập nhật lên View
            HienThiLenViews(ketQua);
        }

        // Sự kiện tìm kiếm dùng LINQ (Yêu cầu 2)
        private void btnTimKiemLINQ_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();

            // Tốc độ phụ thuộc vào dung lượng dữ liệu trên bộ nhớ (RAM)
            List<AoQuanDTO> ketQua = _bll.TimKiemBangLinq(keyword);

            // Cập nhật lên View
            HienThiLenViews(ketQua);
        }

        // Nút nạp lại toàn bộ dữ liệu
        private void btnTaiDanhSach_Click(object sender, EventArgs e)
        {
            TaiDanhSach();
            txtTimKiem.Clear();
        }
    }
}

// Đoạn code giả lập InitializeComponent chỉ để minh họa form (bỏ qua khi đưa vào báo cáo)
// Code trên cung cấp toàn bộ Logic cần thiết của Form cho mục đích copy báo cáo Word.
