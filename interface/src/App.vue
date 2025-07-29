<template>
  <div id="app">
    <!-- ✅ Header Top -->
    <div
      class="header-top d-flex justify-content-between align-items-center px-4 py-2"
    >
      <!-- Logo -->
      <div class="logo d-flex align-items-center">
        <router-link to="/">
          <img
            src="/src/assets/img/logo.png"
            alt="Logo"
            class="me-2"
            style="height: 40px"
          />
        </router-link>
        <span class="brand-name ms-2"></span>
      </div>
      <!-- Giờ mở cửa -->
      <div class="open-hours text-end me-4">
        <small class="text-uppercase text-muted">GIỜ MỞ CỬA</small><br />
        <strong class="fs-6">8.00AM - 8.00PM</strong>
      </div>
      <!-- Hotline & Tài khoản -->
      <div class="d-flex align-items-center gap-4">
        <!-- Hotline -->
        <div class="hotline-box d-flex align-items-center gap-2">
          <i class="fa-solid fa-phone"></i>
          <div class="text-start">
            <span class="d-block" style="font-size: 12px; font-style: italic"
              >Hotline</span
            >
            <span style="font-size: 18px; font-weight: bold">0966193978</span>
          </div>
        </div>

        <!-- Đăng nhập và Xem lịch -->
        <div class="d-flex align-items-center gap-2">
          <router-link
            to="/login"
            class="btn btn-outline-dark btn-sm d-flex align-items-center gap-1"
          >
            <i class="fa-solid fa-user"></i> Đăng nhập
          </router-link>
          <router-link
            to="/lich-hen"
            class="btn btn-outline-primary btn-sm d-flex align-items-center gap-1"
          >
            <i class="fa-regular fa-calendar-check"></i> Xem lịch
          </router-link>
        </div>
      </div>
    </div>

    <!-- ✅ Navbar -->
    <nav class="navbar" :class="{ hidden: isNavbarHidden }" ref="navbar">
      <ul class="nav-links">
        <li>
          <router-link to="/"
            ><i class="fa-solid fa-house"></i> Trang Chủ</router-link
          >
        </li>
        <li>
          <router-link to="/DichVu"
            ><i class="fa-solid fa-briefcase"></i> Dịch Vụ</router-link
          >
        </li>
        <li>
          <router-link to="/DatLich"
            ><i class="fa-regular fa-calendar-check"></i> Đặt Lịch</router-link
          >
        </li>
        <li>
          <router-link to="/GioiThieu"
            ><i class="fa-solid fa-circle-info"></i> Giới Thiệu</router-link
          >
        </li>
        <li>
          <router-link to="/LienHe"
            ><i class="fa-solid fa-envelope"></i> Liên Hệ</router-link
          >
        </li>
      </ul>

      <!-- 🔍 Thanh tìm kiếm dịch vụ -->
      <div class="search-bar ms-4">
        <form @submit.prevent="handleSearch">
          <div class="input-group">
            <input
              v-model="searchQuery"
              type="text"
              class="form-control form-control-sm"
              placeholder="Nhập từ khóa..."
            />
            <button type="submit" class="btn btn-warning btn-sm">
              <i class="fa fa-search"></i>
            </button>
          </div>
        </form>
      </div>
    </nav>

    <main class="main-content">
      <!-- Carousel -->

      <!-- Router View -->
      <router-view />
    </main>
  </div>
  <!-- Floating Button -->
  <div class="chat-popup-wrapper">
    <!-- Chat Toggle Button -->
    <button
      v-if="!chatOpen"
      @click="toggleChat"
      class="chat-toggle-btn"
      title="Hỗ trợ trực tuyến"
    >
      <i class="fas fa-comments"></i>
      <span v-if="hasUnreadMessages" class="notification-badge">{{
        unreadCount
      }}</span>
    </button>

    <!-- Chat Window -->
    <div v-if="chatOpen" class="chat-window">
      <div class="chat-header">
        <h3>Hỗ trợ khách hàng</h3>
        <p>Chúng tôi luôn sẵn sàng hỗ trợ bạn</p>
        <div
          :class="['status-indicator', isConnected ? 'connected' : '']"
        ></div>
        <button @click="toggleChat" class="close-btn">
          <i class="fas fa-times"></i>
        </button>
      </div>

      <!-- Welcome Screen -->
      <div v-if="!chatStarted" class="welcome-screen">
        <h4>Chào mừng bạn!</h4>
        <p>Vui lòng nhập tên của bạn để bắt đầu trò chuyện</p>
        <input
          v-model="guestName"
          type="text"
          placeholder="Tên của bạn (tùy chọn)"
          maxlength="50"
          @keypress="handleWelcomeKeyPress"
        />
        <button @click="startChat" class="start-chat-btn">Bắt đầu chat</button>
      </div>

      <!-- Chat Messages -->
      <div v-if="chatStarted" class="chat-messages" ref="chatMessages">
        <div
          v-for="message in messages"
          :key="message.id"
          :class="[
            'message',
            message.isFromAdmin ? 'support' : '',
            message.isInlineNotice ? 'inline-notice' : '',
            message.isFromAdmin === false &&
            !message.isSystem &&
            !message.isInlineNotice
              ? 'user'
              : '',
          ]"
        >
          <div v-if="message.isInlineNotice" class="inline-system-notice">
            {{ message.message }}
          </div>
          <div v-else class="message-bubble">
            {{ message.message }}
            <div class="message-time">{{ formatTime(message.timestamp) }}</div>
          </div>
        </div>
      </div>

      <!-- Chat Input - chỉ hiện khi chat đang hoạt động và admin chưa disconnect -->
      <div v-if="chatStarted && !adminDisconnected" class="chat-input">
        <div class="input-group">
          <input
            v-model="currentMessage"
            type="text"
            class="message-input"
            placeholder="Nhập tin nhắn..."
            maxlength="500"
            @keypress="handleMessageKeyPress"
          />
          <button
            @click="sendMessage"
            class="send-btn"
            :disabled="!currentMessage.trim()"
          >
            <i class="fas fa-paper-plane"></i>
          </button>
        </div>
      </div>

      <!-- Nút bắt đầu phiên mới khi admin disconnect -->
      <div v-if="chatStarted && adminDisconnected" class="new-session-section">
        <div class="disconnect-notice">
          <p>Nhân viên hỗ trợ đã kết thúc phiên làm việc</p>
          <button @click="startNewSession" class="start-new-session-btn">
            Bắt đầu phiên hỗ trợ mới
          </button>
        </div>
      </div>
    </div>
  </div>

  <!-- Footer -->
  <footer style="background-color: #006641" class="text-white pt-5 pb-4">
    <div class="container">
      <div class="row gy-4">
        <!-- Cột 1: Logo & giới thiệu -->
        <div class="col-md-3">
          <img
            src="/src/assets/img/logo.png"
            alt="TutaSpa Logo"
            class="mb-3"
            width="120"
          />
          <p class="text-white-50">
            <strong>Tuta Spa</strong> – Nơi tôn vinh vẻ đẹp tự nhiên. Dịch vụ
            chăm sóc da chuyên nghiệp, hiện đại và tận tâm.
          </p>
        </div>

        <!-- Cột 2: Liên kết nhanh -->
        <div class="col-md-3">
          <h5 class="fw-semibold mb-3">Liên kết nhanh</h5>
          <ul class="list-unstyled">
            <li>
              <router-link
                to="/"
                class="text-white-50 text-decoration-none d-block mb-2"
                >Trang chủ</router-link
              >
            </li>
            <li>
              <router-link
                to="/DichVu"
                class="text-white-50 text-decoration-none d-block mb-2"
                >Dịch vụ</router-link
              >
            </li>
            <li>
              <router-link
                to="/dat-lich"
                class="text-white-50 text-decoration-none d-block mb-2"
                >Đặt lịch</router-link
              >
            </li>
            <li>
              <router-link
                to="/GioiThieu"
                class="text-white-50 text-decoration-none d-block mb-2"
                >Giới thiệu</router-link
              >
            </li>
            <li>
              <router-link
                to="/LienHe"
                class="text-white-50 text-decoration-none d-block"
                >Liên hệ</router-link
              >
            </li>
          </ul>
        </div>

        <!-- Cột 3: Liên hệ -->
        <div class="col-md-3">
          <h5 class="fw-semibold mb-3">Liên hệ</h5>
          <p class="mb-2">
            <i class="fa-solid fa-location-dot me-2"></i>31 Nguyễn Mộng Tuân, Q.
            Liên Chiểu, Đà Nẵng
          </p>
          <p class="mb-2"><i class="fa-solid fa-phone me-2"></i>0901 234 567</p>
          <p class="mb-2">
            <i class="fa-solid fa-envelope me-2"></i>info@tutaspa.vn
          </p>
          <p><i class="fa-solid fa-clock me-2"></i>Thứ 2 - CN: 8:00 - 20:00</p>
        </div>

        <!-- Cột 4: Mạng xã hội & thanh toán -->
        <div class="col-md-3">
          <h5 class="fw-semibold mb-3">Kết nối với chúng tôi</h5>
          <div class="d-flex gap-3 mb-3">
            <a
              href="https://facebook.com/tutaspa.vn"
              target="_blank"
              class="text-white fs-5"
              ><i class="fa-brands fa-facebook"></i
            ></a>
            <a
              href="https://instagram.com/tutaspa"
              target="_blank"
              class="text-white fs-5"
              ><i class="fa-brands fa-instagram"></i
            ></a>
            <a href="https://zalo.me/0901234567" target="_blank">
              <img
                src="/src/assets/img/zalo.png"
                alt="Zalo"
                width="20"
                height="20"
              />
            </a>
          </div>

          <h6 class="fw-semibold mb-2">Hỗ trợ thanh toán</h6>
          <div class="d-flex gap-2 align-items-center">
            <img
              src="/src/assets/img/Logo-Vietcombank.webp"
              alt="VCB"
              class="rounded"
              width="50"
              height="30"
            />
            <img
              src="/src/assets/img/Logo_MB_new.png.webp"
              alt="MBBank"
              class="rounded"
              width="50"
              height="30"
            />
            <img
              src="/src/assets/img/Techcombank_logo.png"
              alt="Techcombank"
              class="rounded"
              width="50"
              height="30"
            />
            <img
              src="/src/assets/img/LOGO-VIB-Blue.png"
              alt="VIB"
              class="rounded"
              width="50"
              height="30"
            />
          </div>
        </div>
      </div>

      <!-- Footer bottom -->
      <div class="text-center text-white-50 border-top mt-4 pt-3 small">
        &copy; 2025 Tuta Spa. Phát triển bởi đội ngũ yêu cái đẹp.
      </div>
    </div>
  </footer>
</template>

<script setup>
import { onMounted, onUnmounted, ref, nextTick } from "vue";
import * as signalR from "@microsoft/signalr";

const chatClosed = ref(false);
const adminDisconnected = ref(false); // Thêm biến theo dõi trạng thái admin disconnect
const base_url = import.meta.env.VITE_CHAT_URL;
const chatOpen = ref(false);
const chatStarted = ref(false);
const isConnected = ref(false);
const guestName = ref("");
const currentMessage = ref("");
const messages = ref([]);
const hasUnreadMessages = ref(false);
const unreadCount = ref(0);
const sessionId = ref(null);
const userName = ref(null);
const connection = ref(null);
const chatMessages = ref(null);
const assignedAdmin = ref({ id: null, name: null });

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
  initializeChat();
});

// Enhanced cleanup function
onUnmounted(async () => {
  try {
    if (
      connection.value &&
      connection.value.state === signalR.HubConnectionState.Connected
    ) {
      await connection.value.stop();
    }
  } catch (error) {
    console.error("Lỗi khi đóng kết nối:", error);
  }
});

const isMenuVisible = ref(false);

const searchQuery = ref("");

function handleSearch() {
  if (searchQuery.value.trim()) {
    window.location.href = `/DichVu?search=${encodeURIComponent(
      searchQuery.value
    )}`;
  }
}

function toggleMenu() {
  isMenuVisible.value = !isMenuVisible.value;
}

// Chat functions
function initializeChat() {
  try {
    // Only create connection if it doesn't exist or is disconnected
    if (
      !connection.value ||
      connection.value.state === signalR.HubConnectionState.Disconnected
    ) {
      connection.value = new signalR.HubConnectionBuilder()
        .withUrl(`${base_url}`)
        .withAutomaticReconnect()
        .build();

      setupChatEventHandlers();
    }
  } catch (error) {
    console.error("Lỗi khởi tạo kết nối:", error);
    isConnected.value = false;
    addSystemMessage(
      "Không thể khởi tạo kết nối với server. Vui lòng thử lại.",
      "error"
    );
  }
}

function setupChatEventHandlers() {
  connection.value.onreconnected(() => {
    isConnected.value = true;
    addSystemMessage("Đã kết nối lại với server");
  });

  connection.value.onclose(() => {
    isConnected.value = false;
    addSystemMessage("Mất kết nối với server");
  });

  connection.value.on("AdminAssigned", (data) => {
    console.log("Admin assigned:", data);
    // Lưu admin vào biến để hiện ở header
    assignedAdmin.value = { id: data.AdminId, name: data.AdminName };
    // Reset trạng thái admin disconnect khi có admin mới
    adminDisconnected.value = false;
    // Hiện thông báo inline
    addInlineSystemNotice(data.message);
    //addMessage(data.chatMessage);
  });

  connection.value.on("ChatSessionCreated", (data) => {
    console.log("Chat session created:", data);
    sessionId.value = data.sessionId;
    userName.value = data.userName;
    chatStarted.value = true;
    // Reset trạng thái khi tạo session mới
    adminDisconnected.value = false;
    chatClosed.value = false;
    addSystemMessage1(data.message);
  });

  connection.value.on("ReceiveMessage", (message) => {
    addMessage(message);
    if (!chatOpen.value) {
      hasUnreadMessages.value = true;
      unreadCount.value++;
    }
  });

  connection.value.on("ChatClosed", (msg) => {
    chatStarted.value = false;
    // Ẩn phần nhập tin nhắn
    chatClosed.value = true;
    adminDisconnected.value = false; // Reset trạng thái admin disconnect
    // Hiện lại màn hình bắt đầu chat
    addInlineSystemNotice(
      typeof msg === "string"
        ? msg
        : msg?.message ||
            "Phiên chat đã kết thúc. Vui lòng bắt đầu cuộc trò chuyện mới."
    );
  });

  connection.value.on("MessageSent", (message) => {
    console.log("Message sent successfully:", message.messageId);
  });

  connection.value.on("Error", (error) => {
    addSystemMessage(`Lỗi: ${error}`, "error");
  });

  // Sửa lại logic xử lý AdminDisconnected
  connection.value.on("AdminDisconnected", (data) => {
    // Đánh dấu admin đã disconnect nhưng giữ chatStarted = true
    adminDisconnected.value = true;

    // // Hiện thông báo
    // addInlineSystemNotice(data.Message || "Nhân viên hỗ trợ đã kết thúc phiên làm việc.");
  });
}

async function startConnection() {
  try {
    // Check connection state before starting
    if (!connection.value) {
      initializeChat();
    }

    // Only start if connection is in Disconnected state
    if (connection.value.state === signalR.HubConnectionState.Disconnected) {
      await connection.value.start();
      isConnected.value = true;
      console.log("SignalR connection started successfully");
      return true;
    } else if (
      connection.value.state === signalR.HubConnectionState.Connected
    ) {
      // Already connected
      isConnected.value = true;
      return true;
    } else {
      // Connection is in Connecting or Reconnecting state, wait a bit
      console.log("Connection is in state:", connection.value.state);
      await new Promise((resolve) => setTimeout(resolve, 1000));
      return await startConnection(); // Retry
    }
  } catch (error) {
    console.error("Lỗi kết nối:", error);
    isConnected.value = false;
    addSystemMessage(
      "Không thể kết nối với server. Vui lòng thử lại.",
      "error"
    );
    return false;
  }
}

function toggleChat() {
  chatOpen.value = !chatOpen.value;
  if (chatOpen.value) {
    hasUnreadMessages.value = false;
    unreadCount.value = 0;
  }
}

async function startChat() {
  try {
    // Nếu đang kết nối thì disconnect trước
    if (
      connection.value &&
      connection.value.state === signalR.HubConnectionState.Connected
    ) {
      await connection.value.stop();
      // Reset trạng thái
      chatStarted.value = false;
      chatClosed.value = false;
      adminDisconnected.value = false; // Reset trạng thái admin disconnect
      sessionId.value = null;
      userName.value = null;
      messages.value = [];
      assignedAdmin.value = { id: null, name: null };
    }
    // Kết nối lại và bắt đầu phiên mới
    const connected = await startConnection();
    if (
      connected &&
      connection.value.state === signalR.HubConnectionState.Connected
    ) {
      await connection.value.invoke("JoinAsGuest", guestName.value || "");
    } else {
      addSystemMessage(
        "Không thể kết nối với server. Vui lòng thử lại.",
        "error"
      );
    }
  } catch (error) {
    console.error("Lỗi tham gia chat:", error);
    addSystemMessage("Không thể tham gia chat. Vui lòng thử lại.", "error");
  }
}

// Thêm function mới để xử lý khi nhấn nút bắt đầu phiên mới
async function startNewSession() {
  try {
    // Reset các trạng thái về ban đầu
    chatStarted.value = false;
    adminDisconnected.value = false;
    chatClosed.value = false;
    sessionId.value = null;
    userName.value = null;
    messages.value = [];
    assignedAdmin.value = { id: null, name: null };

    // Disconnect connection hiện tại nếu có
    if (
      connection.value &&
      connection.value.state === signalR.HubConnectionState.Connected
    ) {
      await connection.value.stop();
    }

    // Mở lại welcome screen để user có thể nhập tên mới nếu muốn
    // chatStarted.value đã được set = false ở trên nên welcome screen sẽ hiện
  } catch (error) {
    console.error("Lỗi khi bắt đầu phiên mới:", error);
    addSystemMessage("Có lỗi xảy ra. Vui lòng thử lại.", "error");
  }
}

async function sendMessage() {
  if (!currentMessage.value.trim()) return;

  try {
    const message = {
      fromUserId: sessionId.value,
      fromUserName: userName.value,
      message: currentMessage.value,
      timestamp: new Date().toISOString(),
      isFromAdmin: false,
      id: Date.now(),
    };

    addMessage(message);
    await connection.value.invoke("SendMessageToSupport", currentMessage.value);
    currentMessage.value = "";
  } catch (error) {
    console.error("Lỗi gửi tin nhắn:", error);
    addSystemMessage("Không thể gửi tin nhắn. Vui lòng thử lại.", "error");
  }
}

function addMessage(message) {
  messages.value.push({
    ...message,
    id: message.id || Date.now() + Math.random(),
  });
  scrollToBottom();
}
function addSystemMessage1(message, type = "info") {
  messages.value.push({
    id: Date.now() + Math.random(),
    message: message,
    isFromAdmin: true,
    isSystem: false,
    type: type,
    timestamp: new Date().toISOString(),
  });
  scrollToBottom();
}

function addSystemMessage(message, type = "info") {
  messages.value.push({
    id: Date.now() + Math.random(),
    message: message,
    isFromAdmin: false,
    isSystem: true,
    type: type,
    timestamp: new Date().toISOString(),
  });
  scrollToBottom();
}

function addInlineSystemNotice(message) {
  messages.value.push({
    id: Date.now() + Math.random(),
    message: message,
    isInlineNotice: true,
    timestamp: new Date().toISOString(),
  });
}

function formatTime(timestamp) {
  return new Date(timestamp).toLocaleTimeString("vi-VN", {
    hour: "2-digit",
    minute: "2-digit",
  });
}

function scrollToBottom() {
  nextTick(() => {
    if (chatMessages.value) {
      chatMessages.value.scrollTop = chatMessages.value.scrollHeight;
    }
  });
}

function handleWelcomeKeyPress(e) {
  if (e.key === "Enter") {
    startChat();
  }
}

function handleMessageKeyPress(e) {
  if (e.key === "Enter") {
    sendMessage();
  }
}

// Helper function to check connection status
function checkConnectionStatus() {
  if (connection.value) {
    console.log("Connection state:", connection.value.state);
    return connection.value.state === signalR.HubConnectionState.Connected;
  }
  return false;
}

// Optional: Add connection status monitoring
setInterval(() => {
  if (connection.value && chatStarted.value) {
    const wasConnected = isConnected.value;
    const nowConnected =
      connection.value.state === signalR.HubConnectionState.Connected;

    if (wasConnected !== nowConnected) {
      isConnected.value = nowConnected;
      if (!nowConnected) {
        addSystemMessage("Mất kết nối với server", "warning");
      }
    }
  }
}, 5000);
</script>

<style scoped>
body {
  margin: 0;
  font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
  background: #fff;
}

/* Navbar */
.navbar {
  background-color: #57e6ad;
  color: #8c7851;
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
  font-size: 30px;
}
.nav-links li a {
  position: relative;
  color: rgb(255, 255, 255); /* Màu mặc định */
  text-decoration: none;
  transition: color 0.3s;
  font-size: 21px;
  font-weight: bold;
}

/* Gạch dưới ẩn ban đầu */
.nav-links li a::after {
  content: "";
  position: absolute;
  bottom: -4px;
  left: 0;
  width: 0%;
  height: 2px;
  background-color: #ffd700; /* Màu gạch dưới */
  transition: width 0.3s ease;
}

/* Khi hover: đổi màu và hiện gạch dưới */
.nav-links li a:hover {
  color: #ffd700;
}

.nav-links li a:hover::after {
  width: 100%;
}

.nav-links i {
  margin-right: 6px;
}
.user-icon i {
  font-size: 24px;
  color: #ffffff;
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
  box-shadow: 0 6px 16px #8c7851;
}

@media (max-width: 400px) {
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
