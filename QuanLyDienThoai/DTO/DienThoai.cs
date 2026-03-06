using System;

namespace QuanLyDienThoai.DTO
{
    /// <summary>
    /// Lớp Data Transfer Object (DTO) chứa thông tin đối tượng Điện thoại.
    /// Đối tượng này được truyền giữa các lớp DAL, BLL và GUI.
    /// </summary>
    public class DienThoai
    {
        // Thuộc tính cơ bản của Điện thoại
        public string MaDT { get; set; }
        public string TenDT { get; set; }
        public string HangSX { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }

        public DienThoai() { }

        public DienThoai(string maDT, string tenDT, string hangSX, decimal gia, int soLuong)
        {
            MaDT = maDT;
            TenDT = tenDT;
            HangSX = hangSX;
            Gia = gia;
            SoLuong = soLuong;
        }
    }
}
