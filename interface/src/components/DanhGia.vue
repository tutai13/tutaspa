<template>
  <div class="review-wrapper">
    <!-- Floating background elements -->
    <div class="floating-elements">
      <i
        class="fas fa-star floating-star"
        style="top: 10%; left: 10%; font-size: 2rem; animation-delay: 0s"
      ></i>
      <i
        class="fas fa-star floating-star"
        style="top: 20%; right: 15%; font-size: 1.5rem; animation-delay: 1s"
      ></i>
      <i
        class="fas fa-star floating-star"
        style="bottom: 30%; left: 20%; font-size: 1.8rem; animation-delay: 2s"
      ></i>
      <i
        class="fas fa-star floating-star"
        style="bottom: 15%; right: 10%; font-size: 2.2rem; animation-delay: 3s"
      ></i>
    </div>

    <div class="main-container">
      <!-- Title Section -->
      <div class="title-section">
        <h1 class="pulse-effect">✨ Đánh Giá Dịch Vụ</h1>
        <p>Chia sẻ trải nghiệm của bạn với chúng tôi</p>
      </div>

      <!-- Main Form -->
      <div class="form-container glass-container">
        <form @submit.prevent="submitDanhGia" class="premium-form">
          <!-- Dịch vụ -->
          <div class="form-group">
            <label class="form-label">
              <i class="fas fa-concierge-bell"></i>
              Dịch vụ
            </label>
            <div v-if="route.params.id" class="service-display">
              <i class="fas fa-star service-icon"></i>
              {{
                dichVus.find((d) => d.dichVuID === danhGia.maDichVu)
                  ?.tenDichVu || "Đang tải..."
              }}
            </div>
            <select
              v-else
              class="form-select premium-select"
              v-model="danhGia.maDichVu"
              required
            >
              <option value="" disabled>Chọn dịch vụ</option>
              <option
                v-for="d in dichVus"
                :key="d.dichVuID"
                :value="d.dichVuID"
              >
                {{ d.tenDichVu }}
              </option>
            </select>
          </div>

          <!-- Đánh giá sao -->
          <div class="form-group">
            <label class="form-label">
              <i class="fas fa-star"></i>
              Đánh giá của bạn
            </label>
            <div class="star-rating">
              <span
                v-for="i in 5"
                :key="i"
                class="star"
                :class="{
                  active: i <= danhGia.soSao,
                  inactive: i > danhGia.soSao,
                }"
                @click="setRating(i)"
                @mouseenter="hoverRating(i)"
                @mouseleave="resetHover"
                >★</span
              >
            </div>
            <div class="rating-text">
              {{ getRatingText(danhGia.soSao) }}
            </div>
          </div>

          <!-- Nội dung -->
          <div class="form-group">
            <label class="form-label">
              <i class="fas fa-comment-alt"></i>
              Nội dung đánh giá
            </label>
            <textarea
              class="form-control review-textarea"
              rows="6"
              v-model="danhGia.noiDung"
              placeholder="Hãy chia sẻ trải nghiệm của bạn về dịch vụ này...&#10;&#10;Điều gì khiến bạn hài lòng?&#10;Có điều gì cần cải thiện không?"
              @focus="onTextareaFocus"
              @blur="onTextareaBlur"
            ></textarea>
            <div class="character-count">
              {{ danhGia.noiDung.length }}/500 ký tự
            </div>
          </div>

          <!-- Toggle ẩn danh -->
          <div class="toggle-container">
            <div class="form-switch">
              <label class="switch-label">
                <i class="fas fa-user"></i>
                Hiện tên
              </label>
              <div class="toggle-wrapper">
                <input
                  type="checkbox"
                  class="toggle-input"
                  id="anDanhSwitch"
                  v-model="danhGia.anDanh"
                />
                <label for="anDanhSwitch" class="toggle-slider"></label>
              </div>
              <label class="switch-label">
                <i class="fas fa-user-secret"></i>
                Ẩn danh
              </label>
            </div>
          </div>

          <!-- Submit button -->
          <button
            type="submit"
            class="submit-btn"
            :class="{ loading: isSubmitting, success: showSuccess }"
            :disabled="isSubmitting"
          >
            <i v-if="isSubmitting" class="fas fa-spinner fa-spin"></i>
            <i v-else-if="showSuccess" class="fas fa-check"></i>
            <i v-else class="fas fa-paper-plane"></i>
            <span v-if="isSubmitting">Đang gửi...</span>
            <span v-else-if="showSuccess">Gửi thành công!</span>
            <span v-else>Gửi đánh giá</span>
          </button>
        </form>
      </div>

      <!-- Đánh giá gần đây -->
      <div class="recent-reviews" v-if="danhSach.length > 0">
        <h3>📜 Đánh giá gần đây</h3>
        <div
          v-for="(dg, index) in danhSach"
          :key="dg.id"
          class="review-item glass-container"
          :style="{ animationDelay: `${index * 0.1}s` }"
        >
          <div class="review-stars">
            <span
              v-for="i in 5"
              :key="i"
              class="star"
              :class="{
                active: i <= dg.soSao,
                inactive: i > dg.soSao,
              }"
              >★</span
            >
            <span class="review-date">{{ formatDate(dg.ngayTao) }}</span>
          </div>
          <div class="review-content">
            {{ dg.noiDung }}
          </div>
          <div class="review-author">
            <i class="fas fa-user-circle"></i>
            Người đánh giá:
            <span v-if="dg.anDanh" class="anonymous">Ẩn danh</span>
            <span v-else class="author-name">{{
              dg.user?.name || "Không rõ"
            }}</span>
          </div>
        </div>
      </div>

      <!-- Empty state -->
      <!-- <div v-else-if="danhGia.maDichVu && !isLoading" class="empty-state glass-container">
        <i class="fas fa-star-half-alt empty-icon"></i>
        <h4>Chưa có đánh giá nào</h4>
        <p>Hãy là người đầu tiên đánh giá dịch vụ này!</p>
      </div> -->
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch, nextTick } from "vue";
import { useRoute } from "vue-router";
import axiosClient from "../utils/axiosClient";

const route = useRoute();

// Reactive data
const danhGia = ref({
  maDichVu: "",
  soSao: 5,
  noiDung: "",
  anDanh: false,
});

const dichVus = ref([]);
const danhSach = ref([]);
const isSubmitting = ref(false);
const showSuccess = ref(false);
const isLoading = ref(false);
const hoveredRating = ref(0);

const userId = localStorage.getItem("userId");

// Rating text mapping
const ratingTexts = {
  1: "😞 Rất không hài lòng",
  2: "😐 Không hài lòng",
  3: "🙂 Bình thường",
  4: "😊 Hài lòng",
  5: "🤩 Rất hài lòng",
};

// Methods
const getRatingText = (rating) => ratingTexts[rating] || "";

const setRating = (rating) => {
  danhGia.value.soSao = rating;
  // Add click animation
  nextTick(() => {
    const stars = document.querySelectorAll(".star-rating .star");
    stars[rating - 1].style.animation = "star-click 0.3s ease";
    setTimeout(() => {
      stars[rating - 1].style.animation = "";
    }, 300);
  });
};

const hoverRating = (rating) => {
  hoveredRating.value = rating;
};

const resetHover = () => {
  hoveredRating.value = 0;
};

const onTextareaFocus = (e) => {
  e.target.parentElement.classList.add("focused");
};

const onTextareaBlur = (e) => {
  e.target.parentElement.classList.remove("focused");
};

const formatDate = (dateString) => {
  if (!dateString) return "";
  const date = new Date(dateString);
  return date.toLocaleDateString("vi-VN");
};

// Load data
onMounted(async () => {
  try {
    isLoading.value = true;
    const res = await axiosClient.get("DichVu");
    dichVus.value = res;

    const idFromRoute = route.params.id;
    if (idFromRoute) {
      danhGia.value.maDichVu = parseInt(idFromRoute);
      await loadDanhGia(idFromRoute);
    }
  } catch (err) {
    console.error("❌ Lỗi khi tải dịch vụ:", err);
  } finally {
    isLoading.value = false;
  }
});

watch(
  () => danhGia.value.maDichVu,
  async (newVal) => {
    if (newVal) {
      await loadDanhGia(newVal);
    }
  }
);

async function loadDanhGia(maDichVu) {
  try {
    isLoading.value = true;
    const res = await axiosClient.get(`DanhGia/dichvu/${maDichVu}`);
    danhSach.value = res;
  } catch (err) {
    console.error("❌ Lỗi khi tải đánh giá:", err);
  } finally {
    isLoading.value = false;
  }
}

async function submitDanhGia() {
  try {
    isSubmitting.value = true;

    console.log("🚀 Submit payload:", { ...danhGia.value, userId });

    await axiosClient.post("DanhGia", {
      ...danhGia.value,
      userId,
    });

    // Success animation
    showSuccess.value = true;
    setTimeout(() => {
      showSuccess.value = false;
    }, 2000);

    // Reset form
    const currentDichVuID = danhGia.value.maDichVu;
    danhGia.value = {
      maDichVu: currentDichVuID,
      soSao: 5,
      noiDung: "",
      anDanh: false,
    };

    await loadDanhGia(currentDichVuID);
  } catch (err) {
    console.error(err);
    alert("❌ Đánh giá thất bại.");
  } finally {
    isSubmitting.value = false;
  }
}
</script>

<style scoped>
* {
  font-family: "Inter", -apple-system, BlinkMacSystemFont, sans-serif;
}

.review-wrapper {
  min-height: 100vh;
  background: linear-gradient(135deg, #22c55e 0%, #16a34a 50%, #15803d 100%);
  position: relative;
  overflow-x: hidden;
}

.floating-elements {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  pointer-events: none;
  z-index: -1;
}

.floating-star {
  position: absolute;
  color: rgb(255, 255, 255);
  animation: float 6s ease-in-out infinite;
}

@keyframes float {
  0%,
  100% {
    transform: translateY(0px) rotate(0deg);
  }
  50% {
    transform: translateY(-20px) rotate(180deg);
  }
}

.main-container {
  max-width: 800px;
  margin: 0 auto;
  padding: 2rem;
}

.title-section {
  text-align: center;
  margin-bottom: 3rem;
  color: white;
}

.title-section h1 {
  font-size: 3rem;
  font-weight: 700;
  background: linear-gradient(45deg, #eefe5b, #fdc453, #22c55e);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  margin-bottom: 0.5rem;
  text-shadow: 0 2px 10px rgba(34, 197, 94, 0.3);
}

.title-section p {
  font-size: 1.2rem;
  opacity: 0.9;
  font-weight: 300;
}

.pulse-effect {
  animation: pulse 2s ease-in-out infinite;
}

@keyframes pulse {
  0%,
  100% {
    opacity: 1;
  }
  50% {
    opacity: 0.7;
  }
}

.glass-container {
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(20px);
  border: 1px solid rgba(255, 255, 255, 0.2);
  border-radius: 20px;
  box-shadow: 0 25px 50px rgba(0, 0, 0, 0.1);
  transition: all 0.3s ease;
}

.glass-container:hover {
  transform: translateY(-2px);
  box-shadow: 0 30px 60px rgba(0, 0, 0, 0.15);
}

.form-container {
  padding: 2.5rem;
  position: relative;
  overflow: hidden;
}

.form-container::before {
  content: "";
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 4px;
  background: linear-gradient(90deg, #22c55e, #16a34a, #22c55e);
  background-size: 200% 100%;
  animation: shimmer 3s ease-in-out infinite;
}

@keyframes shimmer {
  0%,
  100% {
    background-position: 0% 50%;
  }
  50% {
    background-position: 100% 50%;
  }
}

.form-group {
  margin-bottom: 2rem;
  position: relative;
  transition: all 0.3s ease;
}

.form-group.focused {
  transform: translateY(-2px);
}

.form-label {
  font-weight: 600;
  color: #2d3748;
  margin-bottom: 0.75rem;
  font-size: 1rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.form-control,
.premium-select {
  border: 2px solid #e2e8f0;
  border-radius: 12px;
  padding: 1rem;
  font-size: 1rem;
  transition: all 0.3s ease;
  background: #f8fafc;
  width: 100%;
}

.form-control:focus,
.premium-select:focus {
  border-color: #22c55e;
  box-shadow: 0 0 0 4px rgba(34, 197, 94, 0.1);
  background: white;
  transform: translateY(-2px);
  outline: none;
}

.service-display {
  background: linear-gradient(135deg, #22c55e, #16a34a);
  color: white;
  border: none;
  font-weight: 600;
  padding: 1.25rem;
  border-radius: 12px;
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.service-icon {
  font-size: 1.2rem;
  animation: rotate 2s linear infinite;
}

@keyframes rotate {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
}

.star-rating {
  display: flex;
  gap: 0.5rem;
  justify-content: center;
  padding: 1.5rem;
  background: linear-gradient(135deg, #6cbf85, #c4dd7c);
  border-radius: 16px;
  margin: 1rem 0;
  position: relative;
  overflow: hidden;
  border: 1px solid rgba(34, 197, 94, 0.1);
}

.star-rating::before {
  content: "";
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(
    90deg,
    transparent,
    rgba(34, 197, 94, 0.1),
    transparent
  );
  animation: star-glow 2s ease-in-out infinite;
}

@keyframes star-glow {
  0% {
    left: -100%;
  }
  100% {
    left: 100%;
  }
}

.star {
  font-size: 2.5rem;
  cursor: pointer;
  transition: all 0.3s ease;
  filter: drop-shadow(0 2px 4px rgba(0, 0, 0, 0.1));
  position: relative;
  z-index: 2;
  font-family: Arial, sans-serif;
  user-select: none;
}

.star:hover {
  transform: scale(1.2) rotate(10deg);
  filter: drop-shadow(0 4px 8px rgba(0, 255, 94, 0.3));
}

.star.active {
  color: #eab308;
  text-shadow: 0 0 10px rgba(234, 179, 8, 0.5);
}

.star.inactive {
  color: #e9ecef;
}

@keyframes star-click {
  0%,
  100% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.4);
  }
}

.rating-text {
  text-align: center;
  font-weight: 600;
  color: #4a5568;
  margin-top: 0.5rem;
  font-size: 1.1rem;
}

.review-textarea {
  min-height: 150px;
  resize: vertical;
  background: linear-gradient(135deg, #f8fafc, #f1f5f9);
  font-family: inherit;
}

.character-count {
  text-align: right;
  font-size: 0.85rem;
  color: #718096;
  margin-top: 0.5rem;
}

.toggle-container {
  background: linear-gradient(135deg, #f0fdf4, #dcfce7);
  padding: 1.5rem;
  border-radius: 16px;
  margin: 1.5rem 0;
  border: 1px solid rgba(34, 197, 94, 0.1);
}

.form-switch {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 1rem;
}

.switch-label {
  font-weight: 600;
  color: #4a5568;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin: 0;
}

.toggle-wrapper {
  position: relative;
}

.toggle-input {
  opacity: 0;
  position: absolute;
}

.toggle-slider {
  width: 60px;
  height: 30px;
  background: #cbd5e0;
  border-radius: 30px;
  position: relative;
  cursor: pointer;
  transition: all 0.3s ease;
  display: block;
}

.toggle-slider::before {
  content: "";
  position: absolute;
  width: 26px;
  height: 26px;
  border-radius: 50%;
  background: white;
  top: 2px;
  left: 2px;
  transition: all 0.3s ease;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.2);
}

.toggle-input:checked + .toggle-slider {
  background: linear-gradient(135deg, #22c55e, #16a34a);
  box-shadow: 0 4px 12px rgba(34, 197, 94, 0.4);
}

.toggle-input:checked + .toggle-slider::before {
  transform: translateX(30px);
}

.submit-btn {
  background: linear-gradient(135deg, #22c55e, #16a34a);
  border: none;
  padding: 1.25rem 2rem;
  border-radius: 50px;
  color: white;
  font-weight: 600;
  font-size: 1.1rem;
  width: 100%;
  cursor: pointer;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
  text-transform: uppercase;
  letter-spacing: 1px;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  box-shadow: 0 4px 15px rgba(34, 197, 94, 0.2);
}

.submit-btn:hover:not(:disabled) {
  transform: translateY(-3px);
  box-shadow: 0 15px 30px rgba(34, 197, 94, 0.4);
}

.submit-btn:active {
  transform: translateY(-1px);
}

.submit-btn.loading {
  background: linear-gradient(135deg, #a0aec0, #718096);
  cursor: not-allowed;
}

.submit-btn.success {
  background: linear-gradient(135deg, #48bb78, #38a169);
}

.submit-btn::before {
  content: "";
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

.submit-btn:hover::before {
  left: 100%;
}

.recent-reviews {
  margin-top: 3rem;
}

.recent-reviews h3 {
  color: white;
  font-weight: 700;
  margin-bottom: 2rem;
  text-align: center;
  font-size: 2rem;
}

.review-item {
  padding: 1.5rem;
  margin-bottom: 1.5rem;
  animation: slideInUp 0.6s ease forwards;
  opacity: 0;
  transform: translateY(20px);
}

@keyframes slideInUp {
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.review-stars {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 1rem;
}

.review-stars .star {
  font-size: 1.5rem;
  margin-right: 0.25rem;
  font-family: Arial, sans-serif;
}

.review-date {
  font-size: 0.85rem;
  color: #718096;
  font-weight: 500;
}

.review-content {
  color: #4a5568;
  line-height: 1.6;
  margin-bottom: 1rem;
  font-size: 1rem;
}

.review-author {
  color: #718096;
  font-size: 0.9rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.anonymous {
  font-style: italic;
  color: #a0aec0;
}

.author-name {
  font-weight: 600;
  color: #4a5568;
}

.empty-state {
  text-align: center;
  padding: 3rem 2rem;
  color: #718096;
}

.empty-icon {
  font-size: 4rem;
  margin-bottom: 1rem;
  opacity: 0.5;
}

.empty-state h4 {
  margin-bottom: 0.5rem;
  color: #4a5568;
}

@media (max-width: 768px) {
  .main-container {
    padding: 1rem;
  }

  .title-section h1 {
    font-size: 2rem;
  }

  .form-container {
    padding: 1.5rem;
  }

  .star {
    font-size: 2rem;
  }

  .form-switch {
    flex-direction: column;
    gap: 1rem;
  }
}
</style>
