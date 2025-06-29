<template>
  <div class="layout">
    <!-- Sidebar -->
    <aside class="sidebar">
      <div class="logo">
        <img src="/src/assets/img/logo.png" alt="Logo" />
        <span class="brand">TutaSpa</span>
      </div>
      <div class="menu">
        <div class="menu-title">HOME</div>
        <router-link to="/" class="menu-item" exact-active-class="active">
          <i class="fas fa-th-large"></i> Dashboard
        </router-link>
        <router-link to="/QlDichVu" class="menu-item" exact-active-class="active">
          <i class="fas fa-globe"></i> Quản lý dịch vụ
        </router-link>
        <router-link
          to="/khuyenMai"
          class="menu-item"
          exact-active-class="active"
        >
          <i class="fas fa-shopping-cart"></i> Quản lý Khuyến mãi
        </router-link>
        <router-link to="/kho" class="menu-item" exact-active-class="active">
          <i class="fas fa-columns"></i> Quản lý kho
        </router-link>
        <router-link to="/employees" class="menu-item" exact-active-class="active">
          <i class="fas fa-columns"></i> Quản lý nhân viên
        </router-link>

        <div class="menu-title">APPS</div>
        <router-link
          to="/apps/ecommerce"
          class="menu-item"
          exact-active-class="active"
        >
          <i class="fas fa-store"></i> Thống kê
        </router-link>
        <router-link
          to="/ThuNgan"
          class="menu-item"
          exact-active-class="active"
        >
          <i class="fa-solid fa-wallet"></i> Thu Ngân
        </router-link>
        <router-link
          to="/apps/profile"
          class="menu-item"
          exact-active-class="active"
        >
          <i class="fas fa-user-circle"></i> User Profile
        </router-link>
      </div>
    </aside>

    <!-- Main Area -->
    <div class="main">
      <header class="top-nav">
        <i class="fas fa-bell notification-icon"></i>
        <div class="dropdown">
          <button
            class="btn btn-light dropdown-toggle"
            id="dropdownUser"
            data-bs-toggle="dropdown"
          >
            <img
              src="/src/assets/img/user-1.jpg"
              alt="User"
              class="avatar-img"
            />
          </button>
          <ul
            class="dropdown-menu dropdown-menu-end mt-2 shadow rounded-3 p-3"
            aria-labelledby="dropdownUser"
          >
            <li>
              <a
                class="dropdown-item d-flex align-items-center gap-2 text-secondary"
                href="/profile"
              >
                <i class="fas fa-user fs-6"></i> My Profile
              </a>
            </li>
            <li>
              <a
                class="dropdown-item d-flex align-items-center gap-2 text-secondary"
                href="#"
              >
                <i class="fas fa-envelope fs-6"></i> My Account
              </a>
            </li>
            <li>
              <a
                class="dropdown-item d-flex align-items-center gap-2 text-secondary"
                href="/ve-da-dat"
              >
                <i class="fas fa-tasks fs-6"></i> My Task
              </a>
            </li>
            <li>
              <a class="btn btn-outline-primary w-100" href="#" @click="Logout">
                Logout
              </a>
            </li>
          </ul>
        </div>
      </header>

      <main class="main-content">
        <router-view />
      </main>
    </div>
  </div>
</template>

<script setup>
function Logout() {
  // Xử lý đăng xuất tại đây (ví dụ: xóa token, redirect...)
  console.log("Logout clicked");
}
</script>
<style scoped>
.layout {
  display: flex;
  height: 100vh;
  background-color: #f8f9fc;
}

/* Sidebar */
.sidebar {
  width: 300px;
  background-color: #fff;
  border-right: 1px solid #e2e8f0;
  padding: 20px 0;
  display: flex;
  flex-direction: column;
}
.logo {
  display: flex;
  align-items: center;
  padding: 0 20px;
  margin-bottom: 30px;
}
.logo img {
  width: 60px;
  margin-right: 10px;
}
.brand {
  font-weight: bold;
  font-size: 30px;
  color: #c16fcd;
}
.menu {
  flex: 1;
  display: flex;
  flex-direction: column;
}
.menu-title {
  font-size: 12px;
  font-weight: bold;
  text-transform: uppercase;
  color: #64748b;
  padding: 10px 20px;
}
.menu-item {
  font-size: 20px;
  padding: 10px 20px;
  display: flex;
  align-items: center;
  color: #1e293b;
  text-decoration: none;
  transition: background 0.2s;
}
.menu-item i {
  margin-right: 10px;
}
.menu-item:hover {
  background-color: #f1f5f9;
}
.menu-item.active {
  background-color: #3b82f6;
  color: #fff;
  border-radius: 8px;
}

/* Main Area */
.main {
  flex: 1;
  display: flex;
  flex-direction: column;
}

/* Top Navbar */
.top-nav {
  height: 80px;
  background-color: #fff;
  border-bottom: 1px solid #e2e8f0;
  display: flex;
  justify-content: flex-end;
  align-items: center;
  padding: 0 20px;
}
.notification-icon {
  margin-right: 20px;
  color: #475569;
  font-size: 18px;
}

/* Avatar & Dropdown */
.avatar-img {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  object-fit: cover;
}
.dropdown-menu {
  min-width: 220px;
}
.dropdown-item {
  font-size: 14px;
  padding: 6px 12px;
}
.dropdown-item i {
  margin-right: 10px;
}
.dropdown-item:hover {
  background-color: #f1f5f9;
}
.btn-outline-primary {
  border-radius: 8px;
  font-weight: 500;
}

/* Main Content */
.main-content {
  height: 100%;;
  float: right;
  padding: 20px;
  flex: 1;
  overflow-y: auto;
}
</style>
