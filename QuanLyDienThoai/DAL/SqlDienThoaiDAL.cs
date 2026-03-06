using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using QuanLyDienThoai.DTO;

namespace QuanLyDienThoai.DAL
{
    /// <summary>
    /// Class thao tác với dữ liệu (Data Access Layer) của Điện Thoại.
    /// Kế thừa Interface IDienThoaiDAL để BLL không phụ thuộc cứng vào Class này (Dependency Injection).
    /// </summary>
    public class SqlDienThoaiDAL : IDienThoaiDAL
    {
        // Chuỗi kết nối đến SQL Server (Vui lòng thay đổi chuỗi này cho phù hợp với máy tính chạy CSDL của bạn).
        private string connectionString = "Data Source=.;Initial Catalog=QuanLyDienThoaiDB;Integrated Security=True;";

        // Biến DataContext dùng cho LINQ to SQL
        // LƯU Ý CHO BÁO CÁO: Ở đây tôi tự sinh một lớp DataContext mô phỏng để đoạn code này chạy được mà không lỗi.
        // Trong dự án thực tế bạn sẽ add file .dbml (LINQ to SQL Classes) trong Visual Studio.
        private QuanLyDienThoaiDataContext db;

        public SqlDienThoaiDAL()
        {
            db = new QuanLyDienThoaiDataContext(connectionString);
        }

        #region PHIÊN BẢN 1: ADO.NET THUẦN (SqlCommand, SqlDataReader, Chuỗi truy vấn trực tiếp)

        /// <summary>
        /// Lấy danh sách Điện thoại bằng ADO.NET thuần. Phân trang bằng SQL OFFSET / FETCH.
        /// Lấy tổng số dòng dữ liệu trả ra tham số out totalRecords.
        /// </summary>
        public List<DienThoai> GetDanhSachAdo(int pageNumber, int pageSize, out int totalRecords)
        {
            List<DienThoai> list = new List<DienThoai>();
            totalRecords = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Truy vấn 1: Đếm tổng số lượng bản ghi (Total Records)
                string countQuery = "SELECT COUNT(*) FROM DienThoai";
                SqlCommand cmdCount = new SqlCommand(countQuery, conn);

                // Truy vấn 2: Lấy dữ liệu phân trang
                // Giải thích trong báo cáo Word: Dùng OFFSET (Bỏ qua số dòng của các trang trước) FETCH NEXT (Lấy số dòng của trang hiện tại).
                string pagedQuery = @"
                    SELECT MaDT, TenDT, HangSX, Gia, SoLuong
                    FROM DienThoai
                    ORDER BY MaDT
                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY;";

                SqlCommand cmd = new SqlCommand(pagedQuery, conn);
                int offset = (pageNumber - 1) * pageSize;
                cmd.Parameters.AddWithValue("@Offset", offset);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);

                try
                {
                    conn.Open();
                    // Lấy tổng số dòng để phân trang trên View (tính số lượng nút Next, Prev)
                    totalRecords = (int)cmdCount.ExecuteScalar();

                    // Lấy dữ liệu của trang hiện tại
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DienThoai dt = new DienThoai();
                        dt.MaDT = reader["MaDT"].ToString();
                        dt.TenDT = reader["TenDT"].ToString();
                        dt.HangSX = reader["HangSX"].ToString();
                        dt.Gia = Convert.ToDecimal(reader["Gia"]);
                        dt.SoLuong = Convert.ToInt32(reader["SoLuong"]);
                        list.Add(dt);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi vào file hoặc xử lý Exception (để trống hoặc quăng ra tùy framework)
                    throw new Exception("Lỗi khi lấy dữ liệu phân trang bằng ADO.NET: " + ex.Message);
                }
            }
            return list;
        }

        public bool ThemAdo(DienThoai dt)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO DienThoai (MaDT, TenDT, HangSX, Gia, SoLuong) VALUES (@MaDT, @TenDT, @HangSX, @Gia, @SoLuong)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaDT", dt.MaDT);
                cmd.Parameters.AddWithValue("@TenDT", dt.TenDT);
                cmd.Parameters.AddWithValue("@HangSX", dt.HangSX);
                cmd.Parameters.AddWithValue("@Gia", dt.Gia);
                cmd.Parameters.AddWithValue("@SoLuong", dt.SoLuong);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool SuaAdo(DienThoai dt)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE DienThoai SET TenDT=@TenDT, HangSX=@HangSX, Gia=@Gia, SoLuong=@SoLuong WHERE MaDT=@MaDT";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaDT", dt.MaDT);
                cmd.Parameters.AddWithValue("@TenDT", dt.TenDT);
                cmd.Parameters.AddWithValue("@HangSX", dt.HangSX);
                cmd.Parameters.AddWithValue("@Gia", dt.Gia);
                cmd.Parameters.AddWithValue("@SoLuong", dt.SoLuong);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool XoaAdo(string maDT)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM DienThoai WHERE MaDT=@MaDT";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaDT", maDT);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public List<DienThoai> TimKiemAdo(string keyword)
        {
            List<DienThoai> list = new List<DienThoai>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM DienThoai WHERE TenDT LIKE @Keyword OR MaDT LIKE @Keyword";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DienThoai dt = new DienThoai();
                    dt.MaDT = reader["MaDT"].ToString();
                    dt.TenDT = reader["TenDT"].ToString();
                    dt.HangSX = reader["HangSX"].ToString();
                    dt.Gia = Convert.ToDecimal(reader["Gia"]);
                    dt.SoLuong = Convert.ToInt32(reader["SoLuong"]);
                    list.Add(dt);
                }
            }
            return list;
        }

        #endregion

        #region PHIÊN BẢN 2: LINQ TO SQL (DataContext, Cú pháp LINQ, Skip/Take)

        /// <summary>
        /// Lấy danh sách Điện thoại bằng LINQ. Phân trang bằng Skip() và Take().
        /// Code dễ đọc hơn ADO.NET thuần, tự động sinh truy vấn SQL khi Execute.
        /// </summary>
        public List<DienThoai> GetDanhSachLinq(int pageNumber, int pageSize, out int totalRecords)
        {
            // Trả về tổng số dòng bằng LINQ Count()
            totalRecords = db.DienThoais.Count();

            // Tính toán vị trí bắt đầu
            int offset = (pageNumber - 1) * pageSize;

            // Sử dụng Skip() để bỏ qua số dòng trang trước, Take() để lấy số dòng trang hiện tại
            var query = db.DienThoais
                          .OrderBy(d => d.MaDT)
                          .Skip(offset)
                          .Take(pageSize)
                          .Select(d => new DienThoai
                          {
                              MaDT = d.MaDT,
                              TenDT = d.TenDT,
                              HangSX = d.HangSX,
                              Gia = d.Gia ?? 0,
                              SoLuong = d.SoLuong ?? 0
                          }).ToList();

            return query;
        }

        public bool ThemLinq(DienThoai dt)
        {
            try
            {
                // Ánh xạ từ DTO DienThoai sang đối tượng Entity sinh bởi LINQ (DienThoaiEntity)
                var entity = new DienThoaiEntity
                {
                    MaDT = dt.MaDT,
                    TenDT = dt.TenDT,
                    HangSX = dt.HangSX,
                    Gia = dt.Gia,
                    SoLuong = dt.SoLuong
                };

                db.DienThoais.InsertOnSubmit(entity);
                db.SubmitChanges(); // LINQ to SQL tự động sinh câu lệnh INSERT và thực thi
                return true;
            }
            catch (Exception ex)
            {
                // Trong thực tế cần log lại lỗi (ex.Message)
                return false;
            }
        }

        public bool SuaLinq(DienThoai dt)
        {
            try
            {
                // Tìm kiếm điện thoại cần sửa trong DataContext
                var entity = db.DienThoais.SingleOrDefault(d => d.MaDT == dt.MaDT);
                if (entity != null)
                {
                    // Cập nhật các trường
                    entity.TenDT = dt.TenDT;
                    entity.HangSX = dt.HangSX;
                    entity.Gia = dt.Gia;
                    entity.SoLuong = dt.SoLuong;

                    db.SubmitChanges(); // LINQ sinh câu lệnh UPDATE
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool XoaLinq(string maDT)
        {
            try
            {
                var entity = db.DienThoais.SingleOrDefault(d => d.MaDT == maDT);
                if (entity != null)
                {
                    db.DienThoais.DeleteOnSubmit(entity);
                    db.SubmitChanges(); // LINQ sinh câu lệnh DELETE
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public List<DienThoai> TimKiemLinq(string keyword)
        {
            var query = db.DienThoais
                          .Where(d => d.TenDT.Contains(keyword) || d.MaDT.Contains(keyword))
                          .Select(d => new DienThoai
                          {
                              MaDT = d.MaDT,
                              TenDT = d.TenDT,
                              HangSX = d.HangSX,
                              Gia = d.Gia ?? 0,
                              SoLuong = d.SoLuong ?? 0
                          }).ToList();
            return query;
        }

        #endregion
    }
}
