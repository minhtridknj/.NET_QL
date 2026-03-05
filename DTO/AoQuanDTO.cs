using System;

namespace QuanLyAoQuan.DTO
{
    /// <summary>
    /// Đối tượng truyền tải dữ liệu (Data Transfer Object) giữa các tầng.
    /// Đại diện cho một mẫu quần áo.
    /// </summary>
    public class AoQuanDTO
    {
        public int Id { get; set; }

        public string TenSanPham { get; set; }

        public string Size { get; set; }

        public decimal GiaBan { get; set; }

        public int SoLuong { get; set; }

        // Override ToString để hiển thị trên ListBox/ComboBox
        public override string ToString()
        {
            return $"{TenSanPham} (Size: {Size}) - Giá: {GiaBan:N0} VNĐ";
        }
    }
}
