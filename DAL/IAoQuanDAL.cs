using System.Collections.Generic;
using QuanLyAoQuan.DTO;

namespace QuanLyAoQuan.DAL
{
    /// <summary>
    /// Interface định nghĩa các phương thức giao tiếp với CSDL cho đối tượng Áo quần.
    /// Giúp dễ dàng chuyển đổi hệ quản trị CSDL (SQL Server, MySQL...) mà không ảnh hưởng BLL.
    /// </summary>
    public interface IAoQuanDAL
    {
        /// <summary>
        /// Lấy toàn bộ danh sách áo quần.
        /// </summary>
        List<AoQuanDTO> GetAll();

        /// <summary>
        /// Lấy danh sách áo quần có phân trang (Pagination) - Xử lý Big Data.
        /// </summary>
        /// <param name="pageNumber">Số trang hiện tại (bắt đầu từ 1).</param>
        /// <param name="pageSize">Số lượng bản ghi trên mỗi trang.</param>
        List<AoQuanDTO> GetPaged(int pageNumber, int pageSize);

        /// <summary>
        /// Tìm kiếm áo quần theo tên bằng SQL thuần.
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm.</param>
        List<AoQuanDTO> SearchByName(string keyword);
    }
}
