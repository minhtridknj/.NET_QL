using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using QuanLyDienThoai.DTO;

namespace QuanLyDienThoai.GUI
{
    /// <summary>
    /// Giao diện chính của ứng dụng Quản lý cửa hàng Điện thoại.
    /// Kế thừa Form và Implement IProductView để nhận dữ liệu từ Presenter.
    /// (Sử dụng kiến trúc MVP và 3-Tier như yêu cầu Đồ Án)
    /// Toàn bộ Control (Giao diện) được khởi tạo tự động bằng Code (Dynamic Controls) để copy/paste chạy ngay.
    /// </summary>
    public class MainForm : Form, IProductView
    {
        // -------------------------------------------------------------
        // Các Controls giao diện
        // -------------------------------------------------------------
        private DataGridView dgvDienThoai;
        private TextBox txtMaDT;
        private TextBox txtTenDT;
        private TextBox txtHangSX;
        private TextBox txtGia;
        private TextBox txtSoLuong;
        private TextBox txtTimKiem;

        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnTimKiem;

        // Phân trang
        private Button btnPrev;
        private Button btnNext;
        private Label lblPageInfo;

        // Presenter (Cầu nối với BLL & DAL)
        private DienThoaiPresenter _presenter;

        public MainForm()
        {
            // Thiết lập giao diện
            InitializeComponentDynamic();

            // Khởi tạo Presenter và truyền View (chính Form này) vào
            // Lúc này, Form không hề biết tới BLL hay DAL, mọi thao tác đều qua Presenter.
            _presenter = new DienThoaiPresenter(this);

            // Tự động load dữ liệu trang 1 khi Form bật lên
            _presenter.LoadData(1);
        }

        #region TẠO GIAO DIỆN BẰNG CODE (DYNAMIC CONTROLS)

        private void InitializeComponentDynamic()
        {
            this.Text = "Quản lý Cửa hàng Điện Thoại - MVP Pattern";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // 1. GroupBox Nhập liệu
            GroupBox grpInput = new GroupBox() { Text = "Thông tin Điện thoại", Location = new Point(10, 10), Size = new Size(760, 150) };

            grpInput.Controls.Add(new Label() { Text = "Mã ĐT:", Location = new Point(20, 30), AutoSize = true });
            txtMaDT = new TextBox() { Location = new Point(100, 27), Width = 150 };
            grpInput.Controls.Add(txtMaDT);

            grpInput.Controls.Add(new Label() { Text = "Tên ĐT:", Location = new Point(20, 60), AutoSize = true });
            txtTenDT = new TextBox() { Location = new Point(100, 57), Width = 250 };
            grpInput.Controls.Add(txtTenDT);

            grpInput.Controls.Add(new Label() { Text = "Hãng SX:", Location = new Point(20, 90), AutoSize = true });
            txtHangSX = new TextBox() { Location = new Point(100, 87), Width = 150 };
            grpInput.Controls.Add(txtHangSX);

            grpInput.Controls.Add(new Label() { Text = "Giá bán:", Location = new Point(400, 30), AutoSize = true });
            txtGia = new TextBox() { Location = new Point(480, 27), Width = 150 };
            grpInput.Controls.Add(txtGia);

            grpInput.Controls.Add(new Label() { Text = "Số lượng:", Location = new Point(400, 60), AutoSize = true });
            txtSoLuong = new TextBox() { Location = new Point(480, 57), Width = 150 };
            grpInput.Controls.Add(txtSoLuong);

            this.Controls.Add(grpInput);

            // 2. Các nút chức năng
            btnThem = new Button() { Text = "Thêm", Location = new Point(20, 180), Size = new Size(80, 30) };
            btnThem.Click += (s, e) => { _presenter.ThemMoi(); };
            this.Controls.Add(btnThem);

            btnSua = new Button() { Text = "Sửa", Location = new Point(110, 180), Size = new Size(80, 30) };
            btnSua.Click += (s, e) => { _presenter.Sua(); };
            this.Controls.Add(btnSua);

            btnXoa = new Button() { Text = "Xóa", Location = new Point(200, 180), Size = new Size(80, 30) };
            btnXoa.Click += (s, e) => { _presenter.Xoa(txtMaDT.Text.Trim()); };
            this.Controls.Add(btnXoa);

            txtTimKiem = new TextBox() { Location = new Point(350, 185), Width = 200 };
            this.Controls.Add(txtTimKiem);

            btnTimKiem = new Button() { Text = "Tìm kiếm", Location = new Point(560, 180), Size = new Size(80, 30) };
            btnTimKiem.Click += (s, e) => { _presenter.TimKiem(txtTimKiem.Text.Trim()); };
            this.Controls.Add(btnTimKiem);

            // 3. DataGridView hiển thị danh sách
            dgvDienThoai = new DataGridView();
            dgvDienThoai.Location = new Point(10, 230);
            dgvDienThoai.Size = new Size(760, 250);
            dgvDienThoai.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDienThoai.ReadOnly = true;
            dgvDienThoai.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDienThoai.CellClick += DgvDienThoai_CellClick;
            this.Controls.Add(dgvDienThoai);

            // 4. Các nút Phân trang (Pagination)
            btnPrev = new Button() { Text = "<< Previous", Location = new Point(250, 500), Size = new Size(90, 30) };
            btnPrev.Click += (s, e) => { _presenter.PreviousPage(); };
            this.Controls.Add(btnPrev);

            lblPageInfo = new Label() { Text = "Page 1 of 1", Location = new Point(350, 505), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) };
            this.Controls.Add(lblPageInfo);

            btnNext = new Button() { Text = "Next >>", Location = new Point(450, 500), Size = new Size(90, 30) };
            btnNext.Click += (s, e) => { _presenter.NextPage(); };
            this.Controls.Add(btnNext);
        }

        private void DgvDienThoai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Binding dữ liệu từ Grid lên TextBox khi click
            if (e.RowIndex >= 0 && e.RowIndex < dgvDienThoai.Rows.Count)
            {
                DataGridViewRow row = dgvDienThoai.Rows[e.RowIndex];
                if (row.Cells["MaDT"].Value != null)
                {
                    txtMaDT.Text = row.Cells["MaDT"].Value.ToString();
                    txtTenDT.Text = row.Cells["TenDT"].Value.ToString();
                    txtHangSX.Text = row.Cells["HangSX"].Value.ToString();
                    txtGia.Text = row.Cells["Gia"].Value.ToString();
                    txtSoLuong.Text = row.Cells["SoLuong"].Value.ToString();
                }
            }
        }

        #endregion

        #region IMPLEMENT CÁC HÀM CỦA INTERFACE IPRODUCTVIEW

        // Hiển thị danh sách lên DataGridView
        public void RenderData(List<DienThoai> data)
        {
            dgvDienThoai.DataSource = null; // Xóa sạch dữ liệu cũ
            dgvDienThoai.DataSource = data; // Binding List DTO mới
        }

        // Lấy dữ liệu Input từ người dùng trên các TextBox
        public DienThoai GetInputData()
        {
            DienThoai dt = new DienThoai();
            dt.MaDT = txtMaDT.Text.Trim();
            dt.TenDT = txtTenDT.Text.Trim();
            dt.HangSX = txtHangSX.Text.Trim();

            // Xử lý bắt lỗi số khi nhập (nếu cần an toàn có thể dùng try-parse, ở đây demo dùng Convert)
            try { dt.Gia = Convert.ToDecimal(txtGia.Text); } catch { dt.Gia = 0; }
            try { dt.SoLuong = Convert.ToInt32(txtSoLuong.Text); } catch { dt.SoLuong = 0; }

            return dt;
        }

        // Hiển thị thông báo (Thành công/Lỗi/Validate)
        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Cập nhật trạng thái hiển thị số Trang (Page 1 of 5)
        public void UpdatePaginationState(int totalRecords, int currentPage, int totalPages)
        {
            lblPageInfo.Text = $"Page {currentPage} of {totalPages}";

            // Tắt/Bật nút Next, Prev tùy theo trang hiện tại
            btnPrev.Enabled = (currentPage > 1);
            btnNext.Enabled = (currentPage < totalPages);
        }

        // Làm mới ô nhập liệu
        public void ClearInput()
        {
            txtMaDT.Clear();
            txtTenDT.Clear();
            txtHangSX.Clear();
            txtGia.Clear();
            txtSoLuong.Clear();
            txtMaDT.Focus();
        }

        #endregion
    }
}
