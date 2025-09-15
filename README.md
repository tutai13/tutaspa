# 🌿 TutaSpa – Spa Management System

**TutaSpa** là hệ thống quản lý spa toàn diện được phát triển theo mô hình **Team project (7 thành viên)**.  
Mục tiêu là hỗ trợ **khách hàng đặt lịch online**, **nhân viên thu ngân xử lý thanh toán**, và **chủ spa quản lý hóa đơn & lịch hẹn**.  

---

## 🚀 Features
- **Đặt lịch trực tuyến**: khách hàng chọn dịch vụ, khung giờ, ghi chú, số điện thoại.
- **Quản lý lịch hẹn**: xác nhận, huỷ, cập nhật trạng thái realtime với **SignalR**.
- **Thanh toán tại quầy**: thêm dịch vụ/sản phẩm, chọn phương thức thanh toán.
- **Tích hợp PayOS API**: hỗ trợ thanh toán trực tuyến, chuyển khoản, hoặc tiền mặt.
- **Quản lý hóa đơn**: tạo, xem chi tiết, lọc theo thời gian, chỉnh sửa thông tin khách hàng.
- **Giao diện người dùng hiện đại**: xây dựng bằng Vue.js và Bootstrap.

---

## 🛠 Technologies
- **Backend:** ASP.NET Core Web API (.NET 8), Entity Framework Core, SignalR  
- **Frontend:** Vue.js (Composition API), Axios, Bootstrap  
- **Database:** SQL Server  
- **Payment:** PayOS API  
- **Others:** JWT Authentication, Gmail SMTP, SMS Provider (Abenla)  

---

## ⚙️ Installation Guide

### 1. Clone project
git clone https://github.com/tutai13/tutaspa
🔑 Tạo file appsettings.json
Vì file này được .gitignore, bạn cần tự tạo file appsettings.json trong thư mục backend với cấu trúc sau:
```bash
{
  "GmailSettings": {
    "GmailAddress": "your_gmail@gmail.com",
    "AppPassword": "your_app_password"
  },
  "PayOSConfig": {
    "ClientId": "your_client_id",
    "ApiKey": "your_api_key",
    "ChecksumKey": "your_checksum_key"
  },
  "SmsSettings": {
    "LoginName": "your_login_name",
    "Sign": "your_sign_key",
    "ServiceTypeId": "your_service_type_id",
    "BrandName": "your_brand_name",
    "ApiUrl": "https://api.abenla.com/api/SendSms"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TutaSpa;User Id=sa;Password=your_password;TrustServerCertificate=True"
  },
  "Jwt": {
    "Key": "your_secret_key",
    "Issuer": "TutaSpaApi",
    "Audience": "TutaSpaClient",
    "AccessTokenExpiryInMinutes": 15,
    "RefreshTokenExpiryInDays": 7
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

```

👉 Nhớ thay your_xxx bằng thông tin thật trong môi trường của bạn.

### 2.Frontend Setup (Vue.js)  
cd frontend  
npm install  
npm run dev

🗄 Database
Hệ thống sử dụng SQL Server.  
Connection string chỉnh trong appsettings.json.  
Hãy xóa folder migration tron api, và tạo lại bằng lệnh  
Add-migration init  
Update-database  
📷 Screenshots
có thể vào hai link này để xem chi tiết  
**nếu đăng ký k được thì hãy vào tk này**  
tk: 0327206727 // mk: Xloc2005@  
https://tutaspa.vercel.app/  
https://tutaspaadmin.vercel.app/

<img width="1920" height="1080" alt="image" src="https://github.com/user-attachments/assets/08234e41-eef7-4c14-b976-769ef20e9a20" />
<img width="1920" height="911" alt="image" src="https://github.com/user-attachments/assets/8f7e7f74-5cef-40b3-b96a-55d1a1225b79" />
<img width="1916" height="1075" alt="image" src="https://github.com/user-attachments/assets/62c2f007-ce07-4cc8-87a6-ca1366f88b5f" />
<img width="1791" height="819" alt="image" src="https://github.com/user-attachments/assets/9beae691-eb3d-46a5-95a7-0d9788a0eb7c" />
<img width="1920" height="1080" alt="image" src="https://github.com/user-attachments/assets/9ebe4c4e-22f8-4d89-8398-d21c824606df" />
<img width="1920" height="1080" alt="image" src="https://github.com/user-attachments/assets/5d7d0456-68c6-4af8-99cd-00c72c6c5f30" />
<img width="1920" height="1079" alt="image" src="https://github.com/user-attachments/assets/45ffcfab-8b9c-4409-baa8-fa92f9874f2b" />
<img width="1920" height="1080" alt="image" src="https://github.com/user-attachments/assets/082f6712-ba00-4403-9050-6f80923c65b7" />








