using System.Collections.Generic;
using QuanLyDienThoai.DTO;

namespace QuanLyDienThoai.DAL
{
    /// <summary>
    /// Design Pattern: Interface định nghĩa các phương thức chuẩn để thao tác với Data Source.
    /// Giúp BLL không bị phụ thuộc vào SQL Server hay MySQL (Dependency Inversion).
    /// </summary>
    public interface IDienThoaiDAL
    {
        // -------------------------------------------------------------
        // Phiên bản 1: ADO.NET Thuần
        // -------------------------------------------------------------

        /// <summary>
        /// Lấy danh sách Điện thoại có phân trang (Pagination) bằng ADO.NET thuần.
        /// Sử dụng OFFSET FETCH và trả về tổng số dòng qua tham số out.
        /// </summary>
        List<DienThoai> GetDanhSachAdo(int pageNumber, int pageSize, out int totalRecords);

        /// <summary>
        /// Thêm Điện thoại bằng ADO.NET thuần (SqlCommand, SqlDataReader).
        /// </summary>
        bool ThemAdo(DienThoai dt);

        /// <summary>
        /// Sửa Điện thoại bằng ADO.NET thuần (SqlCommand).
        /// </summary>
        bool SuaAdo(DienThoai dt);

        /// <summary>
        /// Xóa Điện thoại bằng ADO.NET thuần (SqlCommand).
        /// </summary>
        bool XoaAdo(string maDT);

        /// <summary>
        /// Tìm kiếm Điện thoại bằng ADO.NET thuần.
        /// </summary>
        List<DienThoai> TimKiemAdo(string keyword);

        // -------------------------------------------------------------
        // Phiên bản 2: LINQ to SQL
        // -------------------------------------------------------------

        /// <summary>
        /// Lấy danh sách Điện thoại có phân trang bằng LINQ to SQL (DataContext, Skip/Take).
        /// Trả về tổng số dòng qua tham số out.
        /// </summary>
        List<DienThoai> GetDanhSachLinq(int pageNumber, int pageSize, out int totalRecords);

        /// <summary>
        /// Thêm Điện thoại bằng LINQ to SQL.
        /// </summary>
        bool ThemLinq(DienThoai dt);

        /// <summary>
        /// Sửa Điện thoại bằng LINQ to SQL.
        /// </summary>
        bool SuaLinq(DienThoai dt);

        /// <summary>
        /// Xóa Điện thoại bằng LINQ to SQL.
        /// </summary>
        bool XoaLinq(string maDT);

        /// <summary>
        /// Tìm kiếm Điện thoại bằng LINQ to SQL.
        /// </summary>
        List<DienThoai> TimKiemLinq(string keyword);
    }
}
