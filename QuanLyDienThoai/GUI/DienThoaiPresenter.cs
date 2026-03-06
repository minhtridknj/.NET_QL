using System;
using System.Collections.Generic;
using QuanLyDienThoai.BLL;
using QuanLyDienThoai.DTO;

namespace QuanLyDienThoai.GUI
{
    /// <summary>
    /// Design Pattern: MVP - Presenter làm cầu nối giữa BLL và View (Interface IProductView).
    /// Đây là nơi xử lý các sự kiện click nút từ giao diện, gọi nghiệp vụ BLL,
    /// và thông báo dữ liệu mới về cho View hiển thị.
    /// Nó nhận Interface IProductView từ bất kì Form nào (DataGrid, ListView, Report,...).
    /// </summary>
    public class DienThoaiPresenter
    {
        // View hiện tại mà nó đang điều khiển
        private IProductView _view;
        // BLL để xử lý nghiệp vụ
        private DienThoaiBLL _bll;

        // Trạng thái Phân trang
        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalRecords = 0;
        private int _totalPages = 0;

        public DienThoaiPresenter(IProductView view)
        {
            _view = view;
            _bll = new DienThoaiBLL();
        }

        /// <summary>
        /// Khởi tạo và Load dữ liệu ban đầu lên View, có sử dụng Pagination.
        /// </summary>
        public void LoadData(int page = 1)
        {
            _currentPage = page;

            try
            {
                // Gọi BLL, đồng thời nhận totalRecords thông qua tham số out
                List<DienThoai> data = _bll.LayDanhSachDienThoai(_currentPage, _pageSize, out _totalRecords);

                // Tính toán tổng số trang
                _totalPages = (int)Math.Ceiling((double)_totalRecords / _pageSize);

                // Nếu danh sách rỗng thì _totalPages là 0, cập nhật thành 1 cho đẹp giao diện
                if (_totalPages == 0) _totalPages = 1;

                // 1. Trả dữ liệu về View (Ví dụ: Đổ vào DataGridView)
                _view.RenderData(data);

                // 2. Cập nhật lại trạng thái nút phân trang trên View (Trang hiện tại / Tổng số trang)
                _view.UpdatePaginationState(_totalRecords, _currentPage, _totalPages);
            }
            catch (Exception ex)
            {
                _view.ShowMessage("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        // --- CÁC SỰ KIỆN NÚT BẤM (BUTTON CLICK) TỪ GIAO DIỆN ---

        public void NextPage()
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                LoadData(_currentPage);
            }
        }

        public void PreviousPage()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadData(_currentPage);
            }
        }

        public void ThemMoi()
        {
            try
            {
                // 1. Lấy dữ liệu người dùng nhập từ TextBoxes trên View
                DienThoai dt = _view.GetInputData();

                // 2. Gửi dữ liệu này sang BLL để lưu vào DB
                bool success = _bll.ThemDienThoai(dt);
                if (success)
                {
                    _view.ShowMessage("Thêm mới thành công!");
                    _view.ClearInput(); // Làm sạch ô nhập liệu
                    LoadData(_currentPage); // Load lại dữ liệu trang hiện tại
                }
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        public void Sua()
        {
            try
            {
                DienThoai dt = _view.GetInputData();
                bool success = _bll.SuaDienThoai(dt);
                if (success)
                {
                    _view.ShowMessage("Cập nhật thành công!");
                    LoadData(_currentPage); // Load lại dữ liệu
                }
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        public void Xoa(string maDT)
        {
            try
            {
                bool success = _bll.XoaDienThoai(maDT);
                if (success)
                {
                    _view.ShowMessage("Xóa thành công!");
                    _view.ClearInput();

                    // Xóa xong thì load lại dữ liệu từ trang 1
                    LoadData(1);
                }
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        public void TimKiem(string keyword)
        {
            try
            {
                // Do yêu cầu lấy trực tiếp không phân trang, gọi BLL tìm kiếm thường
                List<DienThoai> data = _bll.TimKiem(keyword);

                // Trả về View
                _view.RenderData(data);

                // Cập nhật lại pagination (Tìm kiếm tạm bỏ qua phân trang)
                _view.UpdatePaginationState(data.Count, 1, 1);
            }
            catch (Exception ex)
            {
                _view.ShowMessage("Lỗi tìm kiếm: " + ex.Message);
            }
        }
    }
}
