<template>
  <div class="spa-booking-container">
    <!-- Hero Section -->
    <div class="hero-section">
      <div class="hero-content">
        <div class="hero-icon">✨</div>
        <h1 class="hero-title">Đặt lịch hẹn tại Spa</h1>
        <p class="hero-subtitle">Trải nghiệm thư giãn đẳng cấp</p>
      </div>
      <div class="hero-decoration"></div>
    </div>

    <div class="booking-content">
      <!-- Service Selection -->
      <div class="booking-card service-section">
        <div class="card-header">
          <div class="header-icon">💆‍♀️</div>
          <div class="header-content">
            <h3>Chọn dịch vụ yêu thích</h3>
            <p>Bạn có thể chọn nhiều dịch vụ hoặc để nhân viên tư vấn</p>
          </div>
        </div>

        <div class="card-body">
          <!-- Enhanced Search -->
          <div class="search-container">
            <div class="search-icon">🔍</div>
            <input
              type="text"
              v-model="tuKhoa"
              placeholder="Tìm kiếm dịch vụ spa..."
              class="search-input"
            />
            <div class="search-decoration"></div>
          </div>

          <!-- Service Grid -->
          <div class="services-grid">
            <div
              v-for="dv in dichVuLoc"
              :key="dv.dichVuID"
              class="service-card"
              :class="{ selected: dichVuIDs.includes(dv.dichVuID) }"
              @click="toggleDichVu(dv.dichVuID)"
            >
              <div class="service-image-container">
                <img
                  :src="
                    dv.hinhAnh.startsWith('http')
                      ? dv.hinhAnh
                      : '/images/' + dv.hinhAnh
                  "
                  class="service-image"
                  :alt="dv.tenDichVu"
                />
                <div class="service-overlay">
                  <div
                    class="service-check"
                    v-if="dichVuIDs.includes(dv.dichVuID)"
                  >
                    ✓
                  </div>
                </div>
              </div>
              <div class="service-content">
                <h4 class="service-title">{{ dv.tenDichVu }}</h4>
                <p class="service-description">{{ dv.moTa }}</p>
                <div class="service-details">
                  <span class="service-price"
                    >{{ dv.gia.toLocaleString() }}đ</span
                  >
                  <span class="service-duration">{{ dv.thoiGian }} phút</span>
                </div>
              </div>
            </div>
          </div>

          <!-- Consultation Option -->
          <div class="consultation-option">
            <button
              class="consultation-btn"
              :class="{ active: dichVuIDs.length === 0 }"
              @click="dichVuIDs = []"
            >
              <span class="consultation-icon">💬</span>
              <span class="consultation-text"
                >Để nhân viên tư vấn dịch vụ phù hợp</span
              >
            </button>
          </div>
        </div>
      </div>

      <!-- Date & Time Selection -->
      <div class="booking-card datetime-section">
        <div class="card-header">
          <div class="header-icon">📅</div>
          <div class="header-content">
            <h3>Chọn thời gian</h3>
            <p>Lựa chọn ngày và khung giờ phù hợp với bạn</p>
          </div>
        </div>

        <div class="card-body">
          <div class="datetime-container">
            <div class="date-section">
              <label class="datetime-label">Ngày đến spa</label>
              <div class="date-input-container">
                <input
                  type="date"
                  v-model="ngay"
                  class="date-input"
                  @change="layKhungGio"
                />
                <div class="date-icon">📅</div>
              </div>
            </div>

            <div class="time-section">
              <label class="datetime-label">Khung giờ</label>
              <div class="time-slots">
                <button
                  v-for="gio in danhSachKhungGio"
                  :key="gio.khungGio"
                  class="time-slot"
                  :class="{
                    selected: gio.khungGio === khungGioChon,
                    unavailable: gio.conLai === 0,
                  }"
                  :disabled="gio.conLai === 0"
                  @click="khungGioChon = gio.khungGio"
                >
                  <span class="time-text">{{ gio.khungGio }}</span>
                  <span v-if="gio.conLai === 0" class="unavailable-badge"
                    >Hết chỗ</span
                  >
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Phone Number -->
      <div class="booking-card phone-section">
        <div class="card-header">
          <div class="header-icon">📱</div>
          <div class="header-content">
            <h3>Thông tin liên hệ</h3>
            <p>Để chúng tôi có thể xác nhận lịch hẹn</p>
          </div>
        </div>

        <div class="card-body">
          <div class="phone-input-container">
            <div class="phone-icon">📞</div>
            <input
              v-model="soDienThoai"
              type="tel"
              class="phone-input"
              placeholder="Nhập số điện thoại của bạn"
            />
          </div>
        </div>
      </div>

      <!-- Booking Summary -->
      <div
        class="booking-summary"
        v-if="dichVuIDs.length > 0 || khungGioChon || soDienThoai"
      >
        <h3 class="summary-title">Tóm tắt đặt lịch</h3>
        <div class="summary-content">
          <div v-if="dichVuIDs.length > 0" class="summary-item">
            <span class="summary-label">Dịch vụ:</span>
            <div class="selected-services">
              <span
                v-for="id in dichVuIDs"
                :key="id"
                class="selected-service-tag"
              >
                {{ getDichVuName(id) }}
              </span>
            </div>
          </div>
          <div v-if="ngay && khungGioChon" class="summary-item">
            <span class="summary-label">Thời gian:</span>
            <span class="summary-value">{{ formatDateTime() }}</span>
          </div>
          <div v-if="soDienThoai" class="summary-item">
            <span class="summary-label">Số điện thoại:</span>
            <span class="summary-value">{{ soDienThoai }}</span>
          </div>
        </div>
      </div>

      <!-- Booking Button -->
      <div class="booking-action">
        <button
          class="booking-btn"
          :disabled="!khungGioChon || !soDienThoai"
          :class="{ disabled: !khungGioChon || !soDienThoai }"
          @click="datLich"
        >
          <span class="booking-btn-icon">✨</span>
          <span class="booking-btn-text">Xác nhận đặt lịch</span>
          <div class="booking-btn-decoration"></div>
        </button>
      </div>

      <!-- Notification -->
      <div v-if="thongBao" class="notification" :class="getNotificationClass()">
        <div class="notification-icon">
          {{ thongBao.startsWith("🎉") ? "🎉" : "⚠️" }}
        </div>
        <div class="notification-content">
          {{ thongBao.replace(/^[🎉❌]\s*/, "") }}
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import apiClient from "../utils/axiosClient";

export default {
  name: "DatLichCard",
  data() {
    return {
      danhSachDichVu: [],
      tuKhoa: "",
      dichVuIDs: [],
      ngay: new Date().toISOString().substring(0, 10),
      danhSachKhungGio: [],
      khungGioChon: "",
      soDienThoai: "",
      thongBao: "",
    };
  },
  computed: {
    dichVuLoc() {
      return this.danhSachDichVu.filter((dv) =>
        dv.tenDichVu.toLowerCase().includes(this.tuKhoa.toLowerCase())
      );
    },
  },
  methods: {
    async layDichVu() {
      const res = await apiClient.get("dichvu");
      this.danhSachDichVu = res;
    },
    async layKhungGio() {
      const res = await apiClient.get("datlich/slots", {
        params: { ngay: this.ngay },
      });
      this.danhSachKhungGio = res;
    },
    toggleDichVu(id) {
      const index = this.dichVuIDs.indexOf(id);
      if (index === -1) this.dichVuIDs.push(id);
      else this.dichVuIDs.splice(index, 1);
    },
    getDichVuName(id) {
      const dv = this.danhSachDichVu.find((d) => d.dichVuID === id);
      return dv ? dv.tenDichVu : "";
    },
    formatDateTime() {
      if (!this.ngay || !this.khungGioChon) return "";
      const date = new Date(this.ngay);
      const options = {
        weekday: "long",
        year: "numeric",
        month: "long",
        day: "numeric",
      };
      const dateStr = date.toLocaleDateString("vi-VN", options);
      return `${dateStr} lúc ${this.khungGioChon}`;
    },
    getNotificationClass() {
      return this.thongBao.startsWith("🎉") ? "success" : "error";
    },
    async datLich() {
      const thoiGian = `${this.ngay}T${this.khungGioChon}:00`;

      try {
        await apiClient.post("DatLich", {
          soDienThoai: this.soDienThoai,
          thoiGian,
          dichVuIDs: this.dichVuIDs,
        });

        this.thongBao =
          "🎉 Đặt lịch thành công! Nhân viên sẽ tư vấn nếu bạn chưa chọn dịch vụ.";
        this.khungGioChon = "";
        this.dichVuIDs = [];
        this.soDienThoai = "";
        await this.layKhungGio();
      } catch (err) {
        if (err.response?.status === 400) {
          this.thongBao = "❌ " + err.response.message;
        } else {
          this.thongBao = "❌ Đã có lỗi xảy ra. Vui lòng thử lại sau.";
        }
      }
    },
  },
  mounted() {
    this.layDichVu();
    this.layKhungGio();
  },
};
</script>

<style scoped>
@import url("https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap");

/* CSS Root Variables - Dễ dàng thay đổi màu chủ đạo */
:root {
  /* Primary Colors - Xanh lá chủ đạo */
  --primary-color: #e6dcdc;
  --primary-dark: #059669;
  --primary-light: #34d399;
  --primary-lighter: #34d399;
  --primary-lightest: #34d399;

  /* Secondary Colors */
  --secondary-color: #065f46;
  --secondary-dark: #064e3b;
  --secondary-light: #047857;

  /* Accent Colors */
  --accent-color: #fbbf24;
  --accent-dark: #f59e0b;
  --accent-light: #fcd34d;

  /* Neutral Colors */
  --neutral-50: #94a3b8;
  --neutral-100: #94a3b8;
  --neutral-200: #94a3b8;
  --neutral-300: #94a3b8;
  --neutral-400: #94a3b8;
  --neutral-500: #64748b;
  --neutral-600: #475569;
  --neutral-700: #334155;
  --neutral-800: #1e293b;
  --neutral-900: #0f172a;

  /* Semantic Colors */
  --success-color: #10b981;
  --success-light: #d1fae5;
  --error-color: #ef4444;
  --error-light: #fef2f2;
  --warning-color: #f59e0b;
  --warning-light: #fef3c7;

  /* Gradients */
  --gradient-primary: linear-gradient(
    135deg,
    var(--primary-color) 0%,
    var(--primary-dark) 100%
  );
  --gradient-secondary: linear-gradient(
    135deg,
    var(--secondary-color) 0%,
    var(--secondary-dark) 100%
  );
  --gradient-accent: linear-gradient(
    135deg,
    var(--accent-color) 0%,
    var(--accent-dark) 100%
  );
  --gradient-background: linear-gradient(
    90deg,
    var(--primary-color) 100%,
    var(--secondary-color) 50%
  );
  --gradient-card: linear-gradient(
    135deg,
    var(--neutral-50) 0%,
    var(--neutral-100) 100%
  );

  /* Shadows */
  --shadow-sm: 0 4px 12px rgba(16, 185, 129, 0.05);
  --shadow-md: 0 8px 24px rgba(16, 185, 129, 0.1);
  --shadow-lg: 0 16px 48px rgba(16, 185, 129, 0.15);
  --shadow-primary: 0 8px 32px rgba(16, 185, 129, 0.3);
  --shadow-accent: 0 8px 24px rgba(251, 191, 36, 0.3);
}

/* Alternative Color Scheme - Uncomment để dùng màu xanh dương */
/*
:root {
  --primary-color: #3b82f6;
  --primary-dark: #2563eb;
  --primary-light: #60a5fa;
  --primary-lighter: #93c5fd;
  --primary-lightest: #dbeafe;
  
  --secondary-color: #1e40af;
  --secondary-dark: #1e3a8a;
  --secondary-light: #2563eb;
  
  --accent-color: #f59e0b;
  --accent-dark: #d97706;
  --accent-light: #fbbf24;
  
  --gradient-primary: linear-gradient(135deg, var(--primary-color) 0%, var(--primary-dark) 100%);
  --gradient-secondary: linear-gradient(135deg, var(--secondary-color) 0%, var(--secondary-dark) 100%);
  --gradient-background: linear-gradient(135deg, var(--primary-color) 0%, var(--secondary-color) 100%);
  
  --shadow-primary: 0 8px 32px rgba(59, 130, 246, 0.3);
}
*/

/* Alternative Color Scheme - Uncomment để dùng màu tím */
/*
:root {
  --primary-color: #8b5cf6;
  --primary-dark: #7c3aed;
  --primary-light: #a78bfa;
  --primary-lighter: #c4b5fd;
  --primary-lightest: #ede9fe;
  
  --secondary-color: #6d28d9;
  --secondary-dark: #5b21b6;
  --secondary-light: #7c3aed;
  
  --accent-color: #ec4899;
  --accent-dark: #db2777;
  --accent-light: #f472b6;
  
  --gradient-primary: linear-gradient(135deg, var(--primary-color) 0%, var(--primary-dark) 100%);
  --gradient-secondary: linear-gradient(135deg, var(--secondary-color) 0%, var(--secondary-dark) 100%);
  --gradient-background: linear-gradient(135deg, var(--primary-color) 0%, var(--secondary-color) 100%);
  
  --shadow-primary: 0 8px 32px rgba(139, 92, 246, 0.3);
}
*/

* {
  font-family: "Inter", sans-serif;
}

.spa-booking-container {
  min-height: 100vh;
  background: var(--gradient-background);
  padding: 0;
  position: relative;
  overflow-x: hidden;
}

.spa-booking-container::before {
  content: "";
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: radial-gradient(
      circle at 20% 50%,
      rgba(16, 185, 129, 0.3) 0%,
      transparent 50%
    ),
    radial-gradient(circle at 80% 20%, rgba(6, 95, 70, 0.3) 0%, transparent 50%),
    radial-gradient(
      circle at 40% 80%,
      rgba(52, 211, 153, 0.3) 0%,
      transparent 50%
    );
  pointer-events: none;
  z-index: 0;
}

/* Hero Section */
.hero-section {
  position: relative;
  padding: 4rem 2rem 3rem;
  text-align: center;
  z-index: 1;
}

.hero-content {
  max-width: 600px;
  margin: 0 auto;
}

.hero-icon {
  font-size: 4rem;
  margin-bottom: 1rem;
  animation: float 3s ease-in-out infinite;
}

.hero-title {
  font-size: 3rem;
  font-weight: 700;
  color: white;
  margin-bottom: 0.5rem;
  text-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
  letter-spacing: -0.02em;
}

.hero-subtitle {
  font-size: 1.2rem;
  color: rgba(255, 255, 255, 0.9);
  margin-bottom: 0;
  font-weight: 300;
}

.hero-decoration {
  position: absolute;
  bottom: -20px;
  left: 50%;
  transform: translateX(-50%);
  width: 100px;
  height: 4px;
  background: linear-gradient(
    90deg,
    transparent,
    rgba(255, 255, 255, 0.8),
    transparent
  );
  border-radius: 2px;
}

/* Main Content */
.booking-content {
  position: relative;
  z-index: 1;
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem;
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

/* Card Styles */
.booking-card {
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(20px);
  border-radius: 24px;
  box-shadow: var(--shadow-md), 0 0 0 1px rgba(255, 255, 255, 0.2);
  overflow: hidden;
  transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
}

.booking-card:hover {
  transform: translateY(-8px);
  box-shadow: var(--shadow-lg), 0 0 0 1px rgba(255, 255, 255, 0.3);
}

.card-header {
  display: flex;
  align-items: center;
  padding: 2rem;
  background: var(--gradient-card);
  border-bottom: 1px solid var(--neutral-200);
  gap: 1rem;
}

.header-icon {
  font-size: 2.5rem;
  flex-shrink: 0;
}

.header-content h3 {
  margin: 0 0 0.25rem 0;
  font-size: 1.5rem;
  font-weight: 600;
  color: var(--neutral-800);
}

.header-content p {
  margin: 0;
  color: var(--neutral-500);
  font-size: 0.95rem;
}

.card-body {
  padding: 2rem;
}

/* Search Styles */
.search-container {
  position: relative;
  margin-bottom: 2rem;
}

.search-input {
  width: 100%;
  padding: 1rem 1rem 1rem 3.5rem;
  border: 2px solid var(--neutral-200);
  border-radius: 16px;
  font-size: 1rem;
  background: white;
  transition: all 0.3s ease;
}

.search-input:focus {
  outline: none;
  border-color: var(--primary-color);
  box-shadow: 0 0 0 4px rgba(16, 185, 129, 0.1);
}

.search-icon {
  position: absolute;
  left: 1rem;
  top: 50%;
  transform: translateY(-50%);
  font-size: 1.2rem;
  color: var(--neutral-500);
  z-index: 2;
}

/* Services Grid */
.services-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 1.5rem;
  margin-bottom: 2rem;
  max-height: 500px;
  overflow-y: auto;
  padding-right: 0.5rem;
}

.services-grid::-webkit-scrollbar {
  width: 6px;
}

.services-grid::-webkit-scrollbar-track {
  background: var(--neutral-100);
  border-radius: 3px;
}

.services-grid::-webkit-scrollbar-thumb {
  background: var(--neutral-300);
  border-radius: 3px;
}

.service-card {
  background: white;
  border-radius: 16px;
  overflow: hidden;
  cursor: pointer;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  border: 2px solid transparent;
  box-shadow: var(--shadow-sm);
}

.service-card:hover {
  transform: translateY(-4px);
  box-shadow: var(--shadow-md);
}

.service-card.selected {
  border-color: var(--primary-color);
  box-shadow: var(--shadow-primary);
}

.service-image-container {
  position: relative;
  height: 180px;
  overflow: hidden;
}

.service-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.3s ease;
}

.service-card:hover .service-image {
  transform: scale(1.05);
}

.service-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: var(--gradient-primary);
  opacity: 0;
  transition: opacity 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
}

.service-card.selected .service-overlay {
  opacity: 1;
}

.service-check {
  width: 50px;
  height: 50px;
  background: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
  font-weight: bold;
  color: var(--primary-color);
  transform: scale(0);
  transition: transform 0.3s cubic-bezier(0.68, -0.55, 0.265, 1.55);
}

.service-card.selected .service-check {
  transform: scale(1);
}

.service-content {
  padding: 1.5rem;
}

.service-title {
  font-size: 1.1rem;
  font-weight: 600;
  color: var(--neutral-800);
  margin-bottom: 0.5rem;
}

.service-description {
  color: var(--neutral-500);
  font-size: 0.9rem;
  margin-bottom: 1rem;
  line-height: 1.5;
}

.service-details {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.service-price {
  font-weight: 600;
  color: var(--primary-color);
  font-size: 1rem;
}

.service-duration {
  color: var(--neutral-500);
  font-size: 0.9rem;
  background: var(--neutral-100);
  padding: 0.25rem 0.75rem;
  border-radius: 12px;
}

/* Consultation Option */
.consultation-option {
  text-align: center;
}

.consultation-btn {
  display: inline-flex;
  align-items: center;
  gap: 0.75rem;
  padding: 1rem 2rem;
  background: var(--gradient-card);
  border: 2px solid var(--neutral-200);
  border-radius: 16px;
  color: var(--neutral-500);
  font-weight: 500;
  transition: all 0.3s ease;
  cursor: pointer;
}

.consultation-btn:hover,
.consultation-btn.active {
  background: var(--gradient-primary);
  border-color: var(--primary-color);
  color: white;
  transform: translateY(-2px);
  box-shadow: var(--shadow-primary);
}

.consultation-icon {
  font-size: 1.2rem;
}

/* DateTime Styles */
.datetime-container {
  display: grid;
  grid-template-columns: 1fr 2fr;
  gap: 2rem;
  align-items: start;
}

.datetime-label {
  display: block;
  font-weight: 600;
  color: var(--neutral-800);
  margin-bottom: 0.75rem;
  font-size: 0.95rem;
}

.date-input-container {
  position: relative;
}

.date-input {
  width: 100%;
  padding: 1rem;
  border: 2px solid var(--neutral-200);
  border-radius: 12px;
  font-size: 1rem;
  background: white;
  transition: all 0.3s ease;
}

.date-input:focus {
  outline: none;
  border-color: var(--primary-color);
  box-shadow: 0 0 0 4px rgba(16, 185, 129, 0.1);
}

.date-icon {
  position: absolute;
  right: 1rem;
  top: 50%;
  transform: translateY(-50%);
  pointer-events: none;
  font-size: 1.2rem;
}

.time-slots {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(120px, 1fr));
  gap: 0.75rem;
}

.time-slot {
  padding: 0.75rem 1rem;
  border: 2px solid var(--neutral-200);
  border-radius: 12px;
  background: white;
  color: var(--neutral-500);
  font-weight: 500;
  cursor: pointer;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
}

.time-slot:hover:not(:disabled) {
  border-color: var(--primary-color);
  color: var(--primary-color);
  transform: translateY(-2px);
}

.time-slot.selected {
  background: var(--gradient-primary);
  border-color: var(--primary-color);
  color: white;
  box-shadow: var(--shadow-primary);
}

.time-slot.unavailable {
  background: var(--neutral-50);
  color: var(--neutral-300);
  cursor: not-allowed;
  border-color: var(--neutral-100);
}

.unavailable-badge {
  position: absolute;
  top: -4px;
  right: -8px;
  background: var(--error-color);
  color: white;
  font-size: 0.7rem;
  padding: 0.125rem 0.375rem;
  border-radius: 6px;
  font-weight: 600;
}

/* Phone Input */
.phone-input-container {
  position: relative;
  max-width: 400px;
}

.phone-input {
  width: 100%;
  padding: 1rem 1rem 1rem 3.5rem;
  border: 2px solid var(--neutral-200);
  border-radius: 16px;
  font-size: 1rem;
  background: white;
  transition: all 0.3s ease;
}

.phone-input:focus {
  outline: none;
  border-color: var(--primary-color);
  box-shadow: 0 0 0 4px rgba(16, 185, 129, 0.1);
}

.phone-icon {
  position: absolute;
  left: 1rem;
  top: 50%;
  transform: translateY(-50%);
  font-size: 1.2rem;
  color: var(--neutral-500);
}

/* Booking Summary */
.booking-summary {
  background: var(--gradient-card);
  border-radius: 20px;
  padding: 2rem;
  border: 1px solid var(--neutral-200);
}

.summary-title {
  font-size: 1.3rem;
  font-weight: 600;
  color: var(--neutral-800);
  margin-bottom: 1.5rem;
  text-align: center;
}

.summary-content {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.summary-item {
  display: flex;
  align-items: flex-start;
  gap: 1rem;
}

.summary-label {
  font-weight: 600;
  color: var(--neutral-700);
  min-width: 120px;
  flex-shrink: 0;
}

.summary-value {
  color: var(--neutral-500);
  font-weight: 500;
}

.selected-services {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
}

.selected-service-tag {
  background: var(--gradient-primary);
  color: white;
  padding: 0.375rem 0.75rem;
  border-radius: 12px;
  font-size: 0.85rem;
  font-weight: 500;
}

/* Booking Button */
.booking-action {
  text-align: center;
  margin-top: 1rem;
}

.booking-btn {
  position: relative;
  display: inline-flex;
  align-items: center;
  gap: 0.75rem;
  padding: 1.25rem 3rem;
  background: var(--gradient-primary);
  border: none;
  border-radius: 20px;
  color: white;
  font-size: 1.1rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
  overflow: hidden;
  box-shadow: var(--shadow-primary);
}

.booking-btn:hover:not(.disabled) {
  transform: translateY(-4px);
  box-shadow: var(--shadow-lg);
}

.booking-btn.disabled {
  background: var(--neutral-300);
  cursor: not-allowed;
  box-shadow: none;
}

.booking-btn-decoration {
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(
    90deg,
    transparent,
    rgba(255, 255, 255, 0.2),
    transparent
  );
  transition: left 0.6s ease;
}

.booking-btn:hover:not(.disabled) .booking-btn-decoration {
  left: 100%;
}

.booking-btn-icon {
  font-size: 1.3rem;
}

/* Notification */
.notification {
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 1.5rem;
  border-radius: 16px;
  margin-top: 1rem;
  animation: slideIn 0.5s ease;
}

.notification.success {
  background: var(--success-light);
  border: 1px solid var(--primary-light);
  color: var(--secondary-color);
}

.notification.error {
  background: var(--error-light);
  border: 1px solid #fca5a5;
  color: #991b1b;
}

.notification-icon {
  font-size: 1.5rem;
  flex-shrink: 0;
}

.notification-content {
  font-weight: 500;
  line-height: 1.5;
}

/* Animations */
@keyframes float {
  0%,
  100% {
    transform: translateY(0px);
  }
  50% {
    transform: translateY(-10px);
  }
}

@keyframes slideIn {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes pulse {
  0%,
  100% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.05);
  }
}

/* Responsive Design */
@media (max-width: 768px) {
  .hero-title {
    font-size: 2rem;
  }

  .hero-subtitle {
    font-size: 1rem;
  }

  .booking-content {
    padding: 1rem;
    gap: 1.5rem;
  }

  .card-header {
    padding: 1.5rem;
    flex-direction: column;
    text-align: center;
    gap: 0.5rem;
  }

  .card-body {
    padding: 1.5rem;
  }

  .services-grid {
    grid-template-columns: 1fr;
    max-height: 400px;
  }

  .datetime-container {
    grid-template-columns: 1fr;
    gap: 1.5rem;
  }

  .time-slots {
    grid-template-columns: repeat(2, 1fr);
  }

  .booking-btn {
    padding: 1rem 2rem;
    font-size: 1rem;
  }

  .summary-item {
    flex-direction: column;
    gap: 0.5rem;
  }

  .summary-label {
    min-width: auto;
  }
}

@media (max-width: 480px) {
  .hero-section {
    padding: 2rem 1rem 2rem;
  }

  .hero-title {
    font-size: 1.75rem;
  }

  .services-grid {
    max-height: 300px;
  }

  .time-slots {
    grid-template-columns: 1fr;
  }

  .booking-btn {
    width: 100%;
    padding: 1.25rem 1rem;
  }
}

/* Enhanced Focus States */
.service-card:focus-visible {
  outline: 3px solid var(--primary-color);
  outline-offset: 2px;
}

.time-slot:focus-visible {
  outline: 3px solid var(--primary-color);
  outline-offset: 2px;
}

.booking-btn:focus-visible {
  outline: 3px solid #ffffff;
  outline-offset: 2px;
}

/* Loading States */
.booking-btn.loading {
  pointer-events: none;
}

.booking-btn.loading .booking-btn-text::after {
  content: "...";
  animation: loading 1.5s infinite;
}

@keyframes loading {
  0% {
    content: "";
  }
  33% {
    content: ".";
  }
  66% {
    content: "..";
  }
  100% {
    content: "...";
  }
}

/* Smooth Scrolling */
html {
  scroll-behavior: smooth;
}

/* Custom Scrollbar for Service Grid */
.services-grid {
  scrollbar-width: thin;
  scrollbar-color: var(--neutral-300) var(--neutral-100);
}

/* Enhanced Hover Effects */
.service-card::before {
  content: "";
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: linear-gradient(
    45deg,
    rgba(16, 185, 129, 0.05),
    rgba(6, 95, 70, 0.05)
  );
  opacity: 0;
  transition: opacity 0.3s ease;
  z-index: -1;
}

.service-card:hover::before {
  opacity: 1;
}

/* Glassmorphism Effects */
.booking-card {
  border: 1px solid rgba(255, 255, 255, 0.2);
}

.booking-card::before {
  content: "";
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: linear-gradient(
    135deg,
    rgba(255, 255, 255, 0.1) 0%,
    rgba(255, 255, 255, 0.05) 100%
  );
  pointer-events: none;
  z-index: -1;
}

/* Micro-interactions */
.search-input:focus + .search-decoration {
  transform: scaleX(1);
}

.search-decoration {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  height: 2px;
  background: var(--gradient-primary);
  transform: scaleX(0);
  transition: transform 0.3s ease;
}
</style>
