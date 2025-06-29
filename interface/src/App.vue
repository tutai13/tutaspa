<template>
  <div id="app">
    <!-- Navbar -->
    <nav class="navbar" :class="{ hidden: isNavbarHidden }" ref="navbar">
      <div class="logo animated-logo">
        <router-link to="/">
          <img
            src="/src/assets/img/logo.png"
            alt="TutaSpa Logo"
            class="logo-image"
          />
        </router-link>
      </div>

      <ul class="nav-links">
        <li>
          <router-link to="/">
            <i class="fa-solid fa-house"></i> Trang Chủ
          </router-link>
        </li>
        <li>
          <router-link to="/DichVu">
            <i class="fa-solid fa-briefcase"></i> Dịch Vụ
          </router-link>
        </li>
        <li>
          <router-link to="/dat-lich">
            <i class="fa-regular fa-calendar-check"></i> Đặt Lịch
          </router-link>
        </li>
        <li>
          <router-link to="/GioiThieu">
            <i class="fa-solid fa-circle-info"></i> Giới Thiệu
          </router-link>
        </li>
        <li>
          <router-link to="/LienHe">
            <i class="fa-solid fa-envelope"></i> Liên Hệ
          </router-link>
        </li>
      </ul>

      <div class="user-icon">
        <router-link to="/tai-khoan" class="user-link">
          <i class="fa-solid fa-circle-user"></i>
          <span>Đăng nhập</span>
        </router-link>
        <router-link to="/lich-hen" class="calendar-link">
          <i class="fa-regular fa-calendar-check"></i>
          <span>Xem lịch</span>
        </router-link>
      </div>
    </nav>

    <main class="main-content">
      <!-- Carousel -->

      <!-- Router View -->
      <router-view />
    </main>
  </div>

  <!-- Floating Button -->
  <div class="floating-menu">
    <!-- Nút chính -->
    <button class="main-btn" @click="toggleMenu">
      <i class="fa-solid fa-phone"></i>
    </button>

    <!-- Menu các chức năng -->
    <div
      class="menu-items"
      :class="{ 'show-menu': isMenuVisible }"
      id="menuItems"
    >
      <div class="menu-item">
        <a href="https://zalo.me" target="_blank">
          <i class="fa-brands fa-zalo"></i>
          <span>Chat trên Zalo</span>
        </a>
      </div>
      <div class="menu-item">
        <a href="https://m.me/" target="_blank">
          <i class="fa-brands fa-facebook-messenger"></i>
          <span>Chat trên Facebook</span>
        </a>
      </div>
      <div class="menu-item">
        <a href="tel:0123456789">
          <i class="fa-solid fa-phone"></i>
          <span>Gọi hotline</span>
        </a>
      </div>
      <div class="menu-item">
        <a href="#">
          <i class="fa-regular fa-star"></i>
          <span>Đánh giá dịch vụ</span>
        </a>
      </div>
      <div class="menu-item">
        <a href="#">
          <i class="fa-solid fa-house"></i>
          <span>Liên hệ nhượng quyền</span>
        </a>
      </div>
      <!-- Nút đóng -->
      <button class="close-btn" @click="toggleMenu">
        <i class="fa-solid fa-xmark"></i>
      </button>
    </div>
  </div>

  <!-- footer -->

  <!-- Footer -->
  <footer class="footer bg-dark-red text-white py-5">
    <div
      class="footer-container container grid grid-cols-1 md:grid-cols-4 gap-8"
    >
      <!-- Cột 1: Logo & giới thiệu -->
      <div class="footer-section about">
        <img
          src="/src/assets/img/logo.png"
          alt="TutaSpa Logo"
          class="footer-logo mb-4 w-32"
        />
        <p>
          <strong>Tuta Spa</strong> – Nơi tôn vinh vẻ đẹp tự nhiên. Dịch vụ chăm
          sóc da chuyên nghiệp, hiện đại và tận tâm.
        </p>
      </div>

      <!-- Cột 2: Liên kết nhanh -->
      <div class="footer-section links">
        <h3 class="text-lg font-semibold mb-3">Liên kết nhanh</h3>
        <ul class="space-y-2">
          <li><router-link to="/">Trang chủ</router-link></li>
          <li><router-link to="/DichVu">Dịch vụ</router-link></li>
          <li><router-link to="/dat-lich">Đặt lịch</router-link></li>
          <li><router-link to="/GioiThieu">Giới thiệu</router-link></li>
          <li><router-link to="/LienHe">Liên hệ</router-link></li>
        </ul>
      </div>


    <!-- Cột 4: MXH & Fanpage -->
    <div class="footer-section social">
      <h3 class="text-lg font-semibold mb-3">Kết nối với chúng tôi</h3>
      <div class="flex space-x-4 mb-4">
        <a href="https://facebook.com/tutaspa.vn" target="_blank"><i class="fa-brands fa-facebook fa-lg"></i></a>
        <a href="https://instagram.com/tutaspa" target="_blank"><i class="fa-brands fa-instagram fa-lg"></i></a>
        <a href="https://zalo.me/0901234567" target="_blank"><img  alt="Zalo" class="w-5 h-5" width="15" height="15" /></a>
      </div>

      <h3 class="text-lg font-semibold mb-2">Hỗ trợ thanh toán</h3>
      <div class="flex space-x-3">
        <!-- <img src="D:\DATNNN\tutaspa\interface\src\assets\img\Logo-Vietcombank.webp" alt="VCB" class="w-10 h-6" width="50" height="35"/>
        <img src="D:\DATNNN\tutaspa\interface\src\assets\img\Logo_MB_new.png.webp" alt="MBBank" class="w-10 h-6" width="50" height="35" />
        <img src="D:\DATNNN\tutaspa\interface\src\assets\img\Techcombank_logo.png" alt="Techcombank" class="w-10 h-6" width="50" height="35" />
        <img src="D:\DATNNN\tutaspa\interface\src\assets\img\LOGO-VIB-Blue.png" alt="VIB" class="w-10 h-6" width="50" height="35" /> -->

      <!-- Cột 3: Thông tin liên hệ -->
      <div class="footer-section contact">
        <h3 class="text-lg font-semibold mb-3">Liên hệ</h3>
        <p>
          <i class="fa-solid fa-location-dot"></i> 31 Nguyễn Mộng Tuân, Q. Liên
          Chiểu, Đà Nẵng
        </p>
        <p><i class="fa-solid fa-phone"></i> 0901 234 567</p>
        <p><i class="fa-solid fa-envelope"></i> info@tutaspa.vn</p>
        <p><i class="fa-solid fa-clock"></i> Thứ 2 - CN: 8:00 - 20:00</p>
      </div>

      <!-- Cột 4: MXH & Fanpage -->
      <div class="footer-section social">
        <h3 class="text-lg font-semibold mb-3">Kết nối với chúng tôi</h3>
        <div class="flex space-x-4 mb-4">
          <a href="https://facebook.com/tutaspa.vn" target="_blank"
            ><i class="fa-brands fa-facebook fa-lg"></i
          ></a>
          <a href="https://instagram.com/tutaspa" target="_blank"
            ><i class="fa-brands fa-instagram fa-lg"></i
          ></a>
          <a href="https://zalo.me/0901234567" target="_blank"
            ><img
              src="\src\assets\img\zalo.png"
              alt="Zalo"
              class="w-5 h-5"
              width="15"
              height="15"
          /></a>
        </div>

        <h3 class="text-lg font-semibold mb-2">Hỗ trợ thanh toán</h3>
        <div class="flex space-x-3">
          <img
            src="\src\assets\img\Logo-Vietcombank.webp"
            alt="VCB"
            class="w-10 h-6"
            width="50"
            height="35"
          />
          <img
            src="\src\assets\img\Logo_MB_new.png.webp"
            alt="MBBank"
            class="w-10 h-6"
            width="50"
            height="35"
          />
          <img
            src="\src\assets\img\Techcombank_logo.png"
            alt="Techcombank"
            class="w-10 h-6"
            width="50"
            height="35"
          />
          <img
            src="\src\assets\img\LOGO-VIB-Blue.png"
            alt="VIB"
            class="w-10 h-6"
            width="50"
            height="35"
          />
        </div>

      </div>
    </div>

    <div
      class="footer-bottom mt-8 border-t border-white/20 pt-4 text-center text-sm"
    >
      &copy; 2025 Tuta Spa. Phát triển bởi đội ngũ yêu cái đẹp.
    </div>
  </footer>
</template>

<script setup>
import { onMounted, ref } from "vue";

const isNavbarHidden = ref(false);
let lastScroll = 0;

onMounted(() => {
  window.addEventListener("scroll", () => {
    const currentScroll =
      window.pageYOffset || document.documentElement.scrollTop;
    if (currentScroll > lastScroll && currentScroll > 100) {
      isNavbarHidden.value = true;
    } else {
      isNavbarHidden.value = false;
    }
    lastScroll = currentScroll;
  });
});
const isMenuVisible = ref(false);

function toggleMenu() {
  isMenuVisible.value = !isMenuVisible.value;
}
</script>

<style scoped>
body {
  margin: 0;
  font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
  background: #fff;
}

/* Navbar */
.navbar {
  background-color: #fff;
  color: #f7c213;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 13px 23px;
  position: sticky;
  top: 0;
  z-index: 1000;
  transition: transform 0.4s ease;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
}
.navbar.hidden {
  transform: translateY(-100%);
}
.nav-links {
  list-style: none;
  display: flex;
  gap: 50px;
  margin: 0;
  padding: 0;
  font-size: 21px;
}
.nav-links li a {
  position: relative;
  color: #8e0d3c; /* Màu mặc định */
  text-decoration: none;
  transition: color 0.3s;
  font-size: 21px;
}

/* Gạch dưới ẩn ban đầu */
.nav-links li a::after {
  content: "";
  position: absolute;
  bottom: -4px;
  left: 0;
  width: 0%;
  height: 2px;
  background-color: #8e0d3c; /* Màu gạch dưới */
  transition: width 0.3s ease;
}

/* Khi hover: đổi màu và hiện gạch dưới */
.nav-links li a:hover {
  color: #a70043;
}

.nav-links li a:hover::after {
  width: 100%;
}

.nav-links i {
  margin-right: 6px;
}
.user-icon i {
  font-size: 24px;
  color: #8e0d3c; /* màu mận đỏ như ảnh */
  margin-left: 10px;
  cursor: pointer;
  transition: transform 0.3s ease;
}
.user-icon i:hover {
  transform: scale(1.1);
}

/* Logo Animation */
.logo-image {
  height: 75px; /* hoặc kích thước bạn muốn */
  animation: fadeInDown 1s ease-in-out;
}

/* Nếu bạn có hiệu ứng logo đang dùng */
@keyframes fadeInDown {
  from {
    opacity: 0;
    transform: translateY(-20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

/* Content */
.main-content {
  padding: 20px;
}
.carousel-inner img {
  object-fit: cover;
  height: 400px;
}

.input-group-custom {
  display: flex;
  flex-wrap: nowrap;
  gap: 12px;
  width: 100%;
  max-width: 600px;
  margin: auto;
}

.input-custom {
  flex: 1;
  padding: 14px 16px;
  border-radius: 12px;
  border: 1px solid #ccc;
  transition: all 0.3s ease;
  font-size: 16px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
}

.input-custom:focus {
  border-color: #2a5298;
  box-shadow: 0 0 0 4px rgba(42, 82, 152, 0.2);
  outline: none;
}

.btn-submit {
  padding: 12px 24px;
  border: none;
  border-radius: 12px;
  background: linear-gradient(135deg, #1e3c72, #2a5298);
  color: white;
  font-weight: bold;
  font-size: 16px;
  white-space: nowrap;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
  box-shadow: 0 4px 14px rgba(42, 82, 152, 0.4);
}

.btn-submit:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 16px rgba(42, 82, 152, 0.6);
}

@media (max-width: 576px) {
  .input-group-custom {
    flex-direction: column;
    gap: 8px;
  }

  .btn-submit {
    width: 100%;
  }
}

.form-control-custom {
  flex: 1;
  padding: 12px 16px;
  border: none;
  border-radius: 8px 0 0 8px;
  outline: none;
  font-size: 16px;
}
.btn-custom {
  padding: 12px 20px;
  background-color: #fff;
  color: #000;
  font-weight: bold;
  border: none;
  border-radius: 0 8px 8px 0;
  cursor: pointer;
  transition: background-color 0.3s ease;
}
.btn-custom:hover {
  background-color: #f1f1f1;
}

/* Responsive input group */
@media (max-width: 768px) {
  .input-group-custom {
    flex-direction: column;
  }
  .form-control-custom {
    border-radius: 8px 8px 0 0;
  }
  .btn-custom {
    border-radius: 0 0 8px 8px;
  }
}

/* ⭐ Hiệu ứng sao động */
.custom-star {
  font-size: 36px; /* giống ảnh */
  color: white;
  transition: color 0.6s ease;
}

.star-yellow {
  color: #ffd700; /* vàng gold */
}

.animated-stars .star-animate {
  color: white;
  animation: glowStar 1.5s ease-in-out infinite;
}
.animated-stars .delay-1 {
  animation-delay: 0.2s;
}
.animated-stars .delay-2 {
  animation-delay: 0.4s;
}
.animated-stars .delay-3 {
  animation-delay: 0.6s;
}
.animated-stars .delay-4 {
  animation-delay: 0.8s;
}

@keyframes glowStar {
  0% {
    color: white;
    transform: scale(1);
  }
  50% {
    color: #ffd700;
    transform: scale(1.1);
  }
  100% {
    color: white;
    transform: scale(1);
  }
}
/* phần thêm bên phải */
.floating-menu {
  position: fixed;
  bottom: 20px;
  right: 20px;
  z-index: 1001;
}

.main-btn,
.close-btn {
  width: 60px;
  height: 60px;
  background: linear-gradient(135deg, #0d47a1, #1976d2);
  border: none;
  border-radius: 50%;
  color: white;
  font-size: 24px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
  cursor: pointer;
  transition: transform 0.3s ease;
}

.main-btn:hover,
.close-btn:hover {
  transform: scale(1.05);
}

.menu-items {
  display: none;
  flex-direction: column;
  align-items: flex-end;
  margin-bottom: 10px;
}

.menu-item {
  background: #fff;
  color: #333;
  border-radius: 30px;
  padding: 8px 12px;
  margin-bottom: 10px;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.15);
  display: flex;
  align-items: center;
  gap: 10px;
  transition: all 0.3s ease;
}

.menu-item i {
  color: #0d47a1;
  font-size: 20px;
}

.menu-item span {
  white-space: nowrap;
  font-size: 14px;
}

.menu-item:hover {
  background: #f5f5f5;
}

.show-menu {
  display: flex !important;
}
/* footer */

.footer {
  background-color: #a80032; /* Đỏ mận sang trọng */
  color: #fff;
  padding: 40px 20px 20px;
  font-family: "Segoe UI", sans-serif;
}

.footer-container {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  max-width: 1200px;
  margin: auto;
  gap: 20px;
}

.footer-section {
  flex: 1 1 220px;
  min-width: 220px;
}

.footer-logo {
  width: 140px;
  margin-bottom: 10px;
}

.footer-section h3 {
  font-size: 18px;
  margin-bottom: 15px;
  border-bottom: 2px solid #fff;
  padding-bottom: 5px;
}

.footer-section ul {
  list-style: none;
  padding: 0;
}

.footer-section ul li {
  margin-bottom: 8px;
}

.footer-section ul li a {
  color: #fff;
  text-decoration: none;
  transition: color 0.3s;
}

.footer-section ul li a:hover {
  color: #ffd4e1; /* Hồng nhạt nổi bật */
}

.footer-bottom {
  text-align: center;
  border-top: 1px solid #eee;
  margin-top: 30px;
  padding-top: 15px;
  font-size: 14px;
  color: #f4cdd6;
}

/* Responsive */
@media (max-width: 768px) {
  .footer-container {
    flex-direction: column;
    align-items: center;
  }

  .footer-section {
    text-align: center;
  }

  .footer-section h3 {
    border: none;
  }
}
.bg-dark-red {
  background-color: #7d1d1d; /* Đỏ mận sang trọng */
}

.footer-logo {
  max-width: 120px;
}

.user-icon {
  display: flex;
  gap: 16px;
  align-items: center;
}

.user-link,
.calendar-link {
  display: flex;
  align-items: center;
  gap: 4px;
  text-decoration: none;
  color: #333;
  font-weight: 500;
}

.user-link:hover,
.calendar-link:hover {
  color: #007bff;
}
</style>
