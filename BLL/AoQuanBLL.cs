using System;
using System.Collections.Generic;
using System.Linq;
using QuanLyAoQuan.DAL;
using QuanLyAoQuan.DTO;

namespace QuanLyAoQuan.BLL
{
    /// <summary>
    /// Tầng Nghiệp vụ (Business Logic Layer) xử lý logic ứng dụng và kết nối GUI với DAL.
    /// Có chức năng trả về danh sách đối tượng List<AoQuanDTO>.
    /// </summary>
    public class AoQuanBLL
    {
        // Sử dụng Interface để dễ dàng đổi database (MySQL, Access...)
        // Đây là điểm ứng dụng Dependency Injection cơ bản
        private readonly IAoQuanDAL _aoQuanDal;

        public AoQuanBLL(IAoQuanDAL aoQuanDal)
        {
            _aoQuanDal = aoQuanDal;
        }

        // --- HÀM CƠ BẢN ---
        public List<AoQuanDTO> GetAllAoQuan()
        {
            return _aoQuanDal.GetAll();
        }

        /// <summary>
        /// Lấy dữ liệu phân trang, xử lý Big Data.
        /// </summary>
        public List<AoQuanDTO> GetAoQuanPaged(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;
            return _aoQuanDal.GetPaged(pageNumber, pageSize);
        }

        // --- YÊU CẦU 2: PHÂN TÍCH LINQ VÀ SQL THUẦN ---
        // Nghiệp vụ: TÌM KIẾM ÁO QUẦN THEO TÊN SẢN PHẨM

        /// <summary>
        /// Phiên bản 1: Gọi tầng DAL dùng câu lệnh SQL thuần (SqlCommand).
        /// </summary>
        public List<AoQuanDTO> TimKiemBangSqlThuan(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return _aoQuanDal.GetAll(); // Hoặc trả về rỗng tùy nghiệp vụ
            }

            // Gọi hàm tìm kiếm đã triển khai dưới DAL
            return _aoQuanDal.SearchByName(keyword);
        }

        /// <summary>
        /// Phiên bản 2: Dùng LINQ to Objects để lọc dữ liệu trực tiếp trên RAM.
        /// </summary>
        public List<AoQuanDTO> TimKiemBangLinq(string keyword)
        {
            // Lấy toàn bộ dữ liệu lên List (từ DAL)
            List<AoQuanDTO> allData = _aoQuanDal.GetAll();

            if (string.IsNullOrEmpty(keyword))
            {
                return allData;
            }

            // Dùng LINQ phân tích, lọc chuỗi không phân biệt hoa thường
            // (Yêu cầu 2 - Demo truy vấn LINQ)
            var result = allData
                .Where(a => a.TenSanPham.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderByDescending(a => a.Id)
                .ToList();

            return result;
        }
    }
}
