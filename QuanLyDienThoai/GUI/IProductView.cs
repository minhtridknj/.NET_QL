using System.Collections.Generic;
using QuanLyDienThoai.DTO;

namespace QuanLyDienThoai.GUI
{
    /// <summary>
    /// Design Pattern: MVP (Model-View-Presenter).
    /// Lớp trung gian Interface View để Form (GUI) giao tiếp với Presenter
    /// Mọi View (WinForms, WPF, Web) muốn hiển thị danh sách Điện Thoại đều phải implement Interface này.
    /// Nó giúp tách biệt hoàn toàn UI và Logic, giải quyết "Tight-coupling".
    /// </summary>
    public interface IProductView
    {
        // 1. Hàm nhận dữ liệu từ Presenter để vẽ lên DataGridView / ListView / Chart
        void RenderData(List<DienThoai> data);

        // 2. Hàm thông báo lỗi cho người dùng (MessageBox)
        void ShowMessage(string message);

        // 3. Hàm hiển thị tổng số dòng, số trang (Dùng để tính phân trang: Page X of Y)
        void UpdatePaginationState(int totalRecords, int currentPage, int totalPages);

        // Các Property để lấy dữ liệu nhập vào (Nếu cần) - Ví dụ khi Thêm/Sửa
        DienThoai GetInputData();

        // Cập nhật lại danh sách dữ liệu để hiện view sau khi chỉnh sửa
        void ClearInput();
    }
}
