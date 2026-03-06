using System;
using System.Collections.Generic;
using QuanLyDienThoai.DAL;
using QuanLyDienThoai.DTO;

namespace QuanLyDienThoai.BLL
{
    /// <summary>
    /// Class thao tác với các quy tắc nghiệp vụ (Business Logic Layer) của Điện Thoại.
    /// Nó nhận yêu cầu từ Presenter (hoặc GUI trực tiếp), kiểm tra các quy tắc nghiệp vụ,
    /// rồi mới gọi xuống tầng DAL.
    /// </summary>
    public class DienThoaiBLL
    {
        // Interface cho phép linh hoạt, dễ đổi Data Access.
        // Đây là Dependency Injection: Truyền SqlDienThoaiDAL vào IDienThoaiDAL.
        private IDienThoaiDAL _dal;

        public DienThoaiBLL()
        {
            // Trong thực tế có thể dùng IoC Container, nhưng đồ án sinh viên thì new trực tiếp là đủ.
            _dal = new SqlDienThoaiDAL();
        }

        /// <summary>
        /// Lấy danh sách Điện Thoại với phân trang.
        /// Hàm này có thể chuyển đổi giữa ADO và LINQ (tùy vào cài đặt thực tế hoặc config).
        /// Ở đây minh họa bằng việc gọi phiên bản ADO, bạn có thể gọi bản LINQ tùy ý.
        /// </summary>
        public List<DienThoai> LayDanhSachDienThoai(int pageNumber, int pageSize, out int totalRecords)
        {
            // Có thể đổi thành _dal.GetDanhSachLinq(...)
            return _dal.GetDanhSachAdo(pageNumber, pageSize, out totalRecords);
        }

        public bool ThemDienThoai(DienThoai dt)
        {
            // KIỂM TRA QUY TẮC NGHIỆP VỤ:
            if (string.IsNullOrEmpty(dt.MaDT) || string.IsNullOrEmpty(dt.TenDT))
                throw new Exception("Mã và Tên điện thoại không được để trống!");
            if (dt.Gia <= 0)
                throw new Exception("Giá điện thoại phải lớn hơn 0!");
            if (dt.SoLuong < 0)
                throw new Exception("Số lượng không được âm!");

            // Nếu qua hết các rule thì gọi DAL
            return _dal.ThemAdo(dt);
        }

        public bool SuaDienThoai(DienThoai dt)
        {
            // Tương tự, kiểm tra nghiệp vụ trước khi sửa
            if (string.IsNullOrEmpty(dt.MaDT))
                throw new Exception("Phải chọn mã điện thoại cần sửa.");
            if (dt.Gia <= 0)
                throw new Exception("Giá bán không hợp lệ!");

            return _dal.SuaAdo(dt);
        }

        public bool XoaDienThoai(string maDT)
        {
            if (string.IsNullOrEmpty(maDT))
                throw new Exception("Mã điện thoại không hợp lệ.");

            return _dal.XoaAdo(maDT);
        }

        public List<DienThoai> TimKiem(string keyword)
        {
            if (keyword == null) keyword = "";
            return _dal.TimKiemAdo(keyword);
        }
    }
}
