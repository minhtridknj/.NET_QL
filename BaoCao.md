# BÁO CÁO PHÂN TÍCH: TRUY VẤN SQL THUẦN (ADO.NET) VÀ LINQ TO OBJECTS

Dưới đây là bảng đánh giá, so sánh ưu/nhược điểm, tốc độ và tính bảo trì giữa hai phương pháp thực hiện truy vấn nghiệp vụ (ví dụ: Tìm kiếm áo quần theo tên) nhằm giải quyết Yêu cầu số 2 trong Đồ án:

## 1. Phương pháp 1: Dùng ADO.NET với câu lệnh SQL thuần

Phương pháp này tạo các chuỗi truy vấn (ví dụ: `SELECT * FROM AoQuan WHERE TenSanPham LIKE @Keyword`) và gọi đến SQL Server thông qua `SqlCommand`.

### Ưu điểm
* **Tối ưu hóa từ hệ quản trị CSDL:** SQL Server (hoặc các RDBMS khác) được tối ưu hóa đặc biệt tốt cho việc xử lý truy vấn, lọc, và tìm kiếm trên dữ liệu lớn thông qua các cơ chế như Indexing (Chỉ mục).
* **Tiết kiệm băng thông:** Chỉ những dữ liệu thỏa mãn điều kiện lọc mới được truyền qua mạng từ Database Server về Application Server (Client).
* **Hiệu suất xử lý trên tập dữ liệu siêu lớn:** Khi dữ liệu lên đến hàng triệu dòng, việc bắt Database Server (nơi có tài nguyên mạnh) làm việc luôn là lựa chọn tốt nhất.

### Nhược điểm
* **Tính bảo trì kém:** Code C# và SQL bị trộn lẫn, dễ dẫn đến các lỗi cú pháp (Syntax Error) khó phát hiện khi biên dịch do chuỗi SQL chỉ là kiểu String.
* **Nguy cơ bảo mật:** Nếu không cẩn thận và không sử dụng Parameterized Query (như `@Keyword`), hệ thống rất dễ bị tấn công SQL Injection.
* **Phụ thuộc vào CSDL:** Câu lệnh SQL có thể cần thay đổi nếu ứng dụng chuyển đổi sang một hệ quản trị CSDL khác (ví dụ: từ T-SQL của SQL Server sang MySQL).

---

## 2. Phương pháp 2: Dùng LINQ to Objects trên C#

Phương pháp này tải toàn bộ tập dữ liệu (danh sách Áo quần) từ cơ sở dữ liệu lên bộ nhớ (RAM) của ứng dụng dưới dạng `List<AoQuanDTO>`, sau đó sử dụng thư viện LINQ của .NET (`System.Linq`) để lọc/tìm kiếm: `allData.Where(a => a.TenSanPham.Contains(keyword))`.

### Ưu điểm
* **Code ngắn gọn, dễ hiểu và dễ bảo trì:** Cú pháp của LINQ rất trong sáng, giống với ngôn ngữ tự nhiên, giúp lập trình viên viết ít dòng code hơn so với việc mở kết nối, tạo Command, và đọc DataReader.
* **An toàn kiểu (Type-Safety) và IntelliSense:** Trình biên dịch C# sẽ bắt lỗi ngay lập tức nếu viết sai tên thuộc tính (ví dụ `TenSanPham`). Không thể có lỗi cú pháp tiềm ẩn như chuỗi SQL.
* **Chống SQL Injection 100%:** Do việc lọc diễn ra trên bộ nhớ của ứng dụng bằng các toán tử .NET.
* **Độc lập CSDL:** Code LINQ sẽ chạy như nhau bất kể dữ liệu nguồn ban đầu được lấy từ SQL Server, XML, JSON hay bất kỳ Data Source nào.

### Nhược điểm
* **Ngốn nhiều tài nguyên bộ nhớ (RAM):** Để có thể lọc dữ liệu, ứng dụng phải tải TOÀN BỘ dữ liệu từ bảng lên RAM. Nếu bảng có 10 triệu bản ghi, ứng dụng sẽ bị tràn bộ nhớ (Out of Memory) hoặc chạy rất chậm.
* **Tốc độ chậm ở Client:** Khi dữ liệu lớn, việc lặp qua danh sách trên bộ nhớ sẽ chậm hơn nhiều so với việc tra cứu Index trong SQL Server.
* **Tốn băng thông mạng:** Cần chuyển tất cả dữ liệu qua mạng mặc dù cuối cùng người dùng chỉ cần một phần rất nhỏ trong số đó.

---

## 3. Tổng kết (Khi nào dùng cái nào?)

* **Tốc độ:** SQL thuần nhanh hơn đáng kể khi truy vấn trên dữ liệu cực lớn vì tận dụng được Index và giảm tải băng thông. Ngược lại, LINQ (to Objects) nhanh hơn khi dữ liệu đã sẵn có trên RAM (ví dụ: cần lọc lại nhiều lần danh sách khoảng vài trăm/nghìn sản phẩm trên Form).
* **Tính bảo trì:** LINQ vượt trội hoàn toàn với khả năng phát hiện lỗi lúc biên dịch và hỗ trợ nhắc lệnh (IntelliSense) của IDE.
* **Khuyến nghị:**
  * Nếu dữ liệu nhỏ hoặc cần lọc linh hoạt phía Client để làm giảm độ trễ hiển thị UI liên tục: **Dùng LINQ to Objects**.
  * Nếu dữ liệu quá lớn (Big Data) và chỉ cần lọc chính xác một lần để trả về kết quả: **Dùng ADO.NET SQL thuần**.
  * *(Mở rộng)* Trong thực tế, các ORM như Entity Framework kết hợp LINQ to Entities (dịch mã LINQ thành câu SQL tương ứng trước khi gửi xuống Server) chính là giải pháp lai hoàn hảo mang lại cả 2 lợi ích trên.
