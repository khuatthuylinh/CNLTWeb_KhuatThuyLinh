Hướng dẫn chạy chương trình
Mở project bằng Visual Studio 2022.
Nhấn Ctrl + F5 hoặc F5 để chạy ứng dụng.
Ghi nhớ địa chỉ API được hiển thị trong Terminal.

Ví dụ:

https://localhost:7079
http://localhost:5181
Hướng dẫn kiểm tra API bằng Postman
Tạo sản phẩm
POST http://localhost:5181/api/products

Body → Raw → JSON

{
  "name": "Laptop Dell",
  "price": 15000
}
Lấy sản phẩm theo ID
GET http://localhost:5181/api/products/1
Hướng dẫn Front-end kết nối API
JavaScript (Web)
fetch("http://localhost:5181/api/products/1")
  .then(response => response.json())
  .then(data => console.log(data));
React
const response = await fetch(
  "http://localhost:5181/api/products/1"
);

const data = await response.json();

console.log(data);
Flutter
final response = await http.get(
  Uri.parse(
    'http://localhost:5181/api/products/1'
  ),
);

print(response.body);