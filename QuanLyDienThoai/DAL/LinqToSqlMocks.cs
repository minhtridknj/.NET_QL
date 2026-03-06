using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace QuanLyDienThoai.DAL
{
    // LƯU Ý: Đây là một lớp giả lập (Mock class) để code biên dịch được và mô tả đúng cú pháp LINQ to SQL.
    // Trong báo cáo, phần này sẽ được sinh tự động bởi Visual Studio khi bạn tạo file .dbml (LINQ to SQL Classes).
    // Bạn chỉ cần copy cấu trúc này để làm dẫn chứng nếu cần, còn khi làm thật hãy tạo .dbml.

    public class QuanLyDienThoaiDataContext : DataContext
    {
        public QuanLyDienThoaiDataContext(string connection) : base(connection) { }

        public Table<DienThoaiEntity> DienThoais
        {
            get { return this.GetTable<DienThoaiEntity>(); }
        }
    }

    [Table(Name = "DienThoai")]
    public class DienThoaiEntity
    {
        [Column(IsPrimaryKey = true, Name = "MaDT")]
        public string MaDT { get; set; }

        [Column(Name = "TenDT")]
        public string TenDT { get; set; }

        [Column(Name = "HangSX")]
        public string HangSX { get; set; }

        [Column(Name = "Gia")]
        public decimal? Gia { get; set; }

        [Column(Name = "SoLuong")]
        public int? SoLuong { get; set; }
    }
}
