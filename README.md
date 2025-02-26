# Website Bán Hàng & Hệ Thống Quản Lý The Coffee House

## Mô tả dự án

Dự án này phát triển một website thương mại điện tử cho The Coffee House, cung cấp các chức năng:

- Xem thông tin sản phẩm chi tiết, kèm hình ảnh.
- Đặt hàng trực tuyến với giao diện thân thiện.
- Xem lịch sử đơn hàng đã đặt.
- Quản lý sản phẩm, đơn hàng, người dùng thông qua trang Admin.
- Cập nhật, chỉnh sửa thông tin sản phẩm, đơn hàng, tài khoản bằng AJAX giúp thao tác nhanh chóng mà không cần tải lại trang.
- Gửi thông tin đơn đặt hàng cho khách hàng qua gmail.
## Công nghệ sử dụng

- **Backend**: ASP.NET MVC, Entity Framework
- **Database**: SQL Server
- **Frontend**: HTML, CSS (Bootstrap), JavaScript
- **AJAX**: Sử dụng AJAX để xử lý cập nhật dữ liệu mà không cần refresh trang, giúp cải thiện trải nghiệm người dùng.
- **SMTP**: Gửi email thông báo.
## Giao diện

### Trang chủ
![Trang chủ](images/homepage.png)

### Trang sản phẩm
![Trang sản phẩm](images/products.png)

### Trang quản lý Admin
![Trang Admin](images/admin_dashboard.png)

## Hướng dẫn cài đặt
1. Clone repository:
   ```sh
   git clone https://github.com/your-repo-url.git
   ```
2. Mở project bằng Visual Studio.
3. Cấu hình chuỗi kết nối trong `appsettings.json`.
4. Chạy lệnh để tạo và cập nhật database:
   ```sh
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
5. Chạy project và trải nghiệm!

## Đóng góp
Mọi đóng góp để cải thiện dự án đều được hoan nghênh! Hãy tạo pull request hoặc mở issue để thảo luận.

## Giấy phép
Dự án này được phát hành dưới giấy phép MIT.
