# LUỒNG HOẠT ĐỘNG CỦA CHƯƠNG TRÌNH

## 1. 
Người dùng truy cập đường dẫn `/Book/Create`.

Request GET được gửi đến action `Create()` trong `BookController`.

Controller trả về View chứa form nhập thông tin sách gồm:

* Tên sách (Name)
* Giá sách (Price)

## 2. 
Người dùng nhập thông tin sách và nhấn nút Submit.

Dữ liệu từ form được gửi bằng phương thức POST đến action `Create(Book book)`.

ASP.NET MVC sử dụng Model Binding để tự động gán dữ liệu từ form vào đối tượng `Book`.

## 3.
Khi nhận được dữ liệu, hệ thống thực hiện kiểm tra các điều kiện đã khai báo trong lớp `Book`.

* Thuộc tính `Name` sử dụng `[Required]` để kiểm tra không được để trống.
* Thuộc tính `Price` sử dụng `[Range]` để kiểm tra giá phải lớn hơn 0.

Nếu người dùng:

* Để trống tên sách → hiển thị thông báo "Không được để trống".
* Nhập giá nhỏ hơn hoặc bằng 0 → hiển thị thông báo "Giá phải lớn hơn 0".

## 4.
Sau khi validation hoàn tất, Controller sử dụng: (ModelState.IsValid) để kiểm tra dữ liệu có hợp lệ hay không.

Nếu tất cả điều kiện đều đúng:

* Tên sách không rỗng.
* Giá sách lớn hơn 0.

Thì `ModelState.IsValid` trả về `true`.

Controller hiển thị thông báo:
 "Thêm sách thành công!";

Nếu có bất kỳ lỗi nào:

* Tên sách rỗng.
* Giá nhỏ hơn hoặc bằng 0.

Thì `ModelState.IsValid` trả về `false`.

Hệ thống giữ nguyên form và hiển thị các thông báo lỗi tương ứng để người dùng sửa lại dữ liệu.
