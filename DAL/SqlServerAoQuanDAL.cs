using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using QuanLyAoQuan.DTO;

namespace QuanLyAoQuan.DAL
{
    /// <summary>
    /// Thực thi giao tiếp dữ liệu cụ thể cho SQL Server sử dụng ADO.NET thuần.
    /// Kế thừa Interface IAoQuanDAL.
    /// </summary>
    public class SqlServerAoQuanDAL : IAoQuanDAL
    {
        // Chuỗi kết nối đến SQL Server. Thay thế "Tên_Server" bằng thông tin thực tế.
        private readonly string connectionString = @"Data Source=.;Initial Catalog=QuanLyAoQuan;Integrated Security=True";

        // Hàm tiện ích để đọc dữ liệu từ DataReader sang DTO
        private AoQuanDTO MapToDTO(SqlDataReader reader)
        {
            return new AoQuanDTO
            {
                Id = Convert.ToInt32(reader["Id"]),
                TenSanPham = reader["TenSanPham"].ToString(),
                Size = reader["Size"] != DBNull.Value ? reader["Size"].ToString() : null,
                GiaBan = Convert.ToDecimal(reader["GiaBan"]),
                SoLuong = Convert.ToInt32(reader["SoLuong"])
            };
        }

        public List<AoQuanDTO> GetAll()
        {
            var list = new List<AoQuanDTO>();
            string query = "SELECT Id, TenSanPham, Size, GiaBan, SoLuong FROM AoQuan ORDER BY Id DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(MapToDTO(reader));
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Lấy dữ liệu phân trang - Xử lý Big Data bằng SQL Server thuần (Yêu cầu 1)
        /// Sử dụng mệnh đề OFFSET và FETCH NEXT (từ SQL Server 2012 trở lên).
        /// </summary>
        public List<AoQuanDTO> GetPaged(int pageNumber, int pageSize)
        {
            var list = new List<AoQuanDTO>();

            // Tính toán vị trí bắt đầu
            int offset = (pageNumber - 1) * pageSize;

            // Câu lệnh SQL phân trang để tối ưu lấy dữ liệu lớn
            string query = @"
                SELECT Id, TenSanPham, Size, GiaBan, SoLuong
                FROM AoQuan
                ORDER BY Id
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Offset", offset);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(MapToDTO(reader));
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Phiên bản tìm kiếm bằng ADO.NET với SQL thuần (Yêu cầu 2)
        /// </summary>
        public List<AoQuanDTO> SearchByName(string keyword)
        {
            var list = new List<AoQuanDTO>();
            string query = "SELECT Id, TenSanPham, Size, GiaBan, SoLuong FROM AoQuan WHERE TenSanPham LIKE @Keyword ORDER BY Id DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Keyword", $"%{keyword}%");

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(MapToDTO(reader));
                        }
                    }
                }
            }
            return list;
        }
    }
}
