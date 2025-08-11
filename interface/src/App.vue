<template>
  <div id="app">
    <!-- Header -->
    <header class="header">
      <nav class="nav">
        <div class="logo">
          <router-link to="/" class="navbar-brand d-flex align-items-center">
            <img
              src="../src/assets/img/logotutaspa.png"
              alt="TutaSpa Logo"
              class="me-2"
              style="height: 50px; width: auto"
            />
            <span>TutaSpa 🌿</span>
          </router-link>
        </div>
        <ul class="nav-links">
          <li><router-link to="/">Trang chủ</router-link></li>
          <li><router-link to="/#services">Dịch vụ</router-link></li>
          <li><router-link to="/#booking">Đặt lịch</router-link></li>
          <li><router-link to="/#about">Về chúng tôi</router-link></li>
          <li><router-link to="/#contact">Liên hệ</router-link></li>
        </ul>
        <!-- <router-link to="/#booking" class="book-btn">Đặt lịch ngay</router-link> -->
        <div class="d-flex align-items-center gap-2">
          <router-link
            v-if="!state.isAuthenticated"
            to="/login"
            class="book-btn"
          >
            <i class="fa-solid fa-user"></i> Đăng nhập
          </router-link>
          <router-link
            v-if="state.isAuthenticated"
            to="/lich-hen"
            class="book-btn"
          >
            <i class="fa-regular fa-calendar-check"></i> Xem lịch
          </router-link>
          <button
            v-if="state.isAuthenticated"
            class="book-btn"
            @click="state.logout"
          >
            <i class="fa-regular fa-sign-out-alt"></i> Đăng Xuất
          </button>
        </div>
      </nav>
    </header>

    <!-- Content -->
    <router-view></router-view>
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
          <p v-if="assignedAdmin.name">
            Bạn đang trò chuyện với <strong>{{ assignedAdmin.name }}</strong>
          </p>
          <p v-else>Chúng tôi luôn sẵn sàng hỗ trợ bạn</p>
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
          <button @click="startChat" class="start-chat-btn">
            Bắt đầu chat
          </button>
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
              <div class="message-time">
                {{ formatTime(message.timestamp) }}
              </div>
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
        <div
          v-if="chatStarted && adminDisconnected"
          class="new-session-section"
        >
          <div class="disconnect-notice">
            <button @click="startNewSession" class="start-new-session-btn">
              Bắt đầu phiên hỗ trợ mới
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Footer -->
    <footer id="contact" class="footer">
      <div class="footer-content">
        <div class="footer-section">
          <h3>Liên hệ</h3>
          <p>📍 123 Đường Nguyễn Huệ, Quận 1, TP.HCM</p>
          <p>📞 Hotline: 0123 456 789</p>
          <p>✉️ Email: info@serenityspa.vn</p>
          <p>🌐 Website: www.serenityspa.vn</p>
        </div>
        <div class="footer-section">
          <h3>Giờ mở cửa</h3>
          <p>Thứ 2 - Thứ 6: 8:00 - 21:00</p>
          <p>Thứ 7 - Chủ nhật: 8:00 - 22:00</p>
          <p>Lễ Tết: 9:00 - 20:00</p>
          <p>🎯 Đặt lịch trước 24h để được phục vụ tốt nhất</p>
        </div>
        <div class="footer-section">
          <h3>Dịch vụ nổi bật</h3>
          <p><router-link to="/#services">Massage trị liệu</router-link></p>
          <p>
            <router-link to="/#services">Chăm sóc da chuyên sâu</router-link>
          </p>
          <p><router-link to="/#services">Detox toàn thân</router-link></p>
          <p><router-link to="/#services">Liệu pháp thư giãn</router-link></p>
        </div>
        <div class="footer-section">
          <h3>Theo dõi chúng tôi</h3>
          <p><a href="#">📘 Facebook: Serenity Spa VN</a></p>
          <p><a href="#">📷 Instagram: @serenityspa_vn</a></p>
          <p><a href="#">💬 Zalo: 0123 456 789</a></p>
          <p><a href="#">📺 YouTube: Serenity Spa Vietnam</a></p>
        </div>
      </div>
      <!-- <div class="footer-bottom">
        <p>
          &copy; 2024 Serenity Spa. Tất cả quyền được bảo lưu. | Thiết kế bởi
          Serenity Team
        </p>
      </div> -->
    </footer>
  </div>
</template>

<script setup>
import { onMounted, onUnmounted, ref, nextTick } from "vue";
import * as signalR from "@microsoft/signalr";
import { useAuthState } from "./services/authstate";

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
const state = useAuthState();
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
    assignedAdmin.value = { id: data.adminId, name: data.adminName };
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

  connection.value.on("ChatClosed", handleSessionEnded);
  connection.value.on("AdminDisconnected", handleSessionEnded);

  function handleSessionEnded(data) {
    // Ẩn phần nhập tin nhắn, hiện new-session-section
    chatStarted.value = true; // Để giữ khung chat đang mở
    adminDisconnected.value = true; // Hiện new-session-section
    chatClosed.value = true;
    addInlineSystemNotice(
      (data?.Message || data?.message) ??
        "Phiên chat đã kết thúc. Vui lòng bắt đầu phiên hỗ trợ mới."
    );
  }

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
  // Reset trạng thái về ban đầu
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
  // Welcome screen sẽ hiện lại cho nhập tên
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

new-session-section {
  padding: 16px;
}
.start-new-session-btn {
  background: linear-gradient(135deg, #ff6b6b, #ff5252);
  color: white;
  border: none;
  padding: 12px 24px;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
  width: 100%;
}

.start-new-session-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(255, 107, 107, 0.3);
  background: linear-gradient(135deg, #ff5252, #ff4444);
}

.start-new-session-btn:active {
  transform: translateY(0);
}
/* Chat Popup Styles */
.chat-popup-wrapper {
  position: fixed;
  bottom: 20px;
  right: 20px;
  z-index: 1000;
  font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
}

.chat-toggle-btn {
  width: 60px;
  height: 60px;
  border-radius: 50%;
  background: linear-gradient(135deg, #4caf50, #45a049);
  color: white;
  border: none;
  font-size: 24px;
  cursor: pointer;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
  transition: all 0.3s ease;
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
}

.chat-toggle-btn:hover {
  transform: scale(1.1);
  box-shadow: 0 6px 25px rgba(0, 0, 0, 0.4);
}

.notification-badge {
  position: absolute;
  top: -5px;
  right: -5px;
  background: #ff4444;
  color: white;
  border-radius: 50%;
  width: 20px;
  height: 20px;
  font-size: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
}

.chat-window {
  width: 350px;
  height: 500px;
  background: white;
  border-radius: 20px;
  box-shadow: 0 15px 35px rgba(0, 0, 0, 0.2);
  overflow: hidden;
  display: flex;
  flex-direction: column;
  animation: slideUp 0.3s ease;
}

@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.chat-header {
  background: linear-gradient(135deg, #4caf50, #45a049);
  color: white;
  padding: 20px;
  text-align: center;
  position: relative;
}

.chat-header h3 {
  margin: 0 0 5px 0;
  font-size: 1.2em;
}

.chat-header p {
  margin: 0;
  font-size: 0.8em;
  opacity: 0.9;
}

.status-indicator {
  position: absolute;
  top: 15px;
  right: 40px;
  width: 10px;
  height: 10px;
  border-radius: 50%;
  background: #ff4444;
  animation: pulse 2s infinite;
}

.status-indicator.connected {
  background: #4caf50;
}

@keyframes pulse {
  0% {
    transform: scale(1);
    opacity: 1;
  }
  50% {
    transform: scale(1.1);
    opacity: 0.7;
  }
  100% {
    transform: scale(1);
    opacity: 1;
  }
}

.close-btn {
  position: absolute;
  top: 15px;
  right: 15px;
  background: none;
  border: none;
  color: white;
  font-size: 16px;
  cursor: pointer;
  width: 20px;
  height: 20px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.welcome-screen {
  padding: 30px 20px;
  text-align: center;
  flex: 1;
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.welcome-screen h4 {
  color: #333;
  margin-bottom: 15px;
}

.welcome-screen p {
  color: #666;
  margin-bottom: 20px;
  font-size: 0.9em;
}

.welcome-screen input {
  width: 100%;
  padding: 10px;
  border: 2px solid #ddd;
  border-radius: 20px;
  font-size: 14px;
  margin-bottom: 15px;
  outline: none;
  transition: border-color 0.3s;
}

.welcome-screen input:focus {
  border-color: #4caf50;
}

.start-chat-btn {
  background: linear-gradient(135deg, #4caf50, #45a049);
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 20px;
  font-size: 14px;
  cursor: pointer;
  transition: transform 0.2s;
}

.start-chat-btn:hover {
  transform: translateY(-2px);
}

.chat-messages {
  flex: 1;
  overflow-y: auto;
  padding: 15px;
  background: #f8f9fa;
}

.message {
  margin-bottom: 15px;
  display: flex;
  align-items: flex-end;
}

.message.user {
  justify-content: flex-end;
}

.message.support {
  justify-content: flex-start;
}

.message-bubble {
  max-width: 70%;
  padding: 10px 14px;
  border-radius: 18px;
  font-size: 13px;
  line-height: 1.4;
  position: relative;
  animation: slideIn 0.3s ease;
}

.message.user .message-bubble {
  background: linear-gradient(135deg, #667eea, #764ba2);
  color: white;
  border-bottom-right-radius: 4px;
}

.message.support .message-bubble {
  background: white;
  color: #333;
  border: 1px solid #e0e0e0;
  border-bottom-left-radius: 4px;
}

.message-time {
  font-size: 10px;
  opacity: 0.7;
  margin-top: 5px;
}

.message.user .message-time {
  text-align: right;
  color: rgba(255, 255, 255, 0.7);
}

.message.support .message-time {
  color: #666;
}

@keyframes slideIn {
  from {
    opacity: 0;
    transform: translateY(10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.chat-input {
  padding: 15px;
  background: white;
  border-top: 1px solid #e0e0e0;
}

.input-group {
  display: flex;
  gap: 8px;
  align-items: center;
}

.message-input {
  flex: 1;
  padding: 10px 14px;
  border: 2px solid #ddd;
  border-radius: 20px;
  font-size: 13px;
  outline: none;
  transition: border-color 0.3s;
}

.message-input:focus {
  border-color: #4caf50;
}

.send-btn {
  background: linear-gradient(135deg, #4caf50, #45a049);
  color: white;
  border: none;
  padding: 10px;
  border-radius: 50%;
  cursor: pointer;
  transition: transform 0.2s;
  width: 40px;
  height: 40px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.send-btn:hover {
  transform: scale(1.05);
}

.send-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

@media (max-width: 768px) {
  .chat-popup-wrapper {
    bottom: 10px;
    right: 10px;
  }

  .chat-window {
    width: 320px;
    height: 450px;
  }
}

.inline-system-notice {
  width: 100%;
  text-align: center;
  background: #e5e7eb;
  color: #444;
  border-radius: 8px;
  padding: 7px 12px;
  margin: 10px 0;
  font-size: 0.95em;
  font-style: italic;
  opacity: 0.85;
}
/* Reset và Base */
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: "Georgia", "Times New Roman", serif;
  line-height: 1.7;
  color: #2d4a2d;
  overflow-x: hidden;
  background-color: #f8fcf8;
}

/* Header với màu xanh lá tươi */
.header {
  background: linear-gradient(135deg, #78ba7e 0%, #6ba371 50%, #5e8c64 100%);
  color: white;
  padding: 1rem 0;
  position: fixed;
  width: 100%;
  top: 0;
  z-index: 1000;
  backdrop-filter: blur(10px);
  box-shadow: 0 4px 20px rgba(120, 186, 126, 0.3);
}

.nav {
  display: flex;
  justify-content: space-between;
  align-items: center;
  max-width: 1400px;
  margin: 0 auto;
  padding: 0 2rem;
}

.logo {
  font-size: 1.8rem;
  font-weight: bold;
  color: white;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.nav-links {
  display: flex;
  list-style: none;
  gap: 2rem;
}

.nav-links a {
  color: white;
  text-decoration: none;
  font-weight: 500;
  transition: all 0.3s ease;
  padding: 0.5rem 1rem;
  border-radius: 25px;
}

.nav-links a:hover {
  background: rgba(255, 255, 255, 0.2);
  transform: translateY(-2px);
}

.book-btn {
  background: linear-gradient(45deg, #f59e0b, #f97316);
  padding: 0.8rem 2rem;
  border-radius: 30px;
  text-decoration: none;
  color: white;
  font-weight: bold;
  transition: all 0.3s ease;
  box-shadow: 0 4px 15px rgba(245, 158, 11, 0.3);
}

.book-btn:hover {
  transform: translateY(-3px);
  box-shadow: 0 6px 20px rgba(245, 158, 11, 0.4);
}

/* Footer */
.footer {
  background: #1f2937;
  color: white;
  padding: 4rem 2rem 2rem;
}

.footer-content {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 3rem;
  margin-bottom: 3rem;
  max-width: 1200px;
  margin-left: auto;
  margin-right: auto;
}

.footer-section h3 {
  margin-bottom: 1.5rem;
  color: #8bc792;
  font-family: "Georgia", serif;
}

.footer-section p,
.footer-section a {
  color: #d1d5db;
  text-decoration: none;
  line-height: 2;
  transition: color 0.3s ease;
}

.footer-section a:hover {
  color: #8bc792;
}

/* Responsive */
@media (max-width: 768px) {
  .nav-links {
    display: none;
  }
}
</style>
