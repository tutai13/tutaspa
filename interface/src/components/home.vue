<template>
  <!-- Hero Carousel with Enhanced Parallax Effect -->
  <div class="hero-section">
    <div id="bannerCarousel" class="carousel slide carousel-fade" data-bs-ride="carousel" data-bs-interval="6000">
      <div class="carousel-inner">
        <div class="carousel-item active">
          <div class="hero-slide" style="background-image: url('/src/assets/img/BNR1.jpg')">
            <div class="hero-overlay"></div>
            <div class="hero-particles"></div>
            <div class="hero-content">
              <div class="hero-badge animate-badge">🌟 SPA 5 SAO</div>
              <h1 class="hero-title animate-title">Chào Mừng Đến TUTA Spa</h1>
              <p class="hero-subtitle animate-subtitle">Nơi thư giãn và tái tạo năng lượng cho tâm hồn bạn</p>
              <button class="btn-cta primary animate-button" @click="goToBooking">
                <span>Đặt Lịch Ngay</span>
                <i class="fa-solid fa-calendar-plus"></i>
              </button>
            </div>
          </div>
        </div>
        
        <div class="carousel-item">
          <div class="hero-slide" style="background-image: url('/src/assets/img/BNR2.jpg')">
            <div class="hero-overlay"></div>
            <div class="hero-particles"></div>
            <div class="hero-content">
              <div class="hero-badge animate-badge">🔬 CÔNG NGHỆ</div>
              <h1 class="hero-title animate-title">Công Nghệ Hiện Đại</h1>
              <p class="hero-subtitle animate-subtitle">Thiết bị tối tân kết hợp kỹ thuật chuyên nghiệp hàng đầu</p>
              <button class="btn-cta secondary animate-button" @click="scrollToServices">
                <span>Khám Phá Dịch Vụ</span>
                <i class="fa-solid fa-magic"></i>
              </button>
            </div>
          </div>
        </div>
        
        <div class="carousel-item">
          <div class="hero-slide" style="background-image: url('/src/assets/img/spa2.jpg')">
            <div class="hero-overlay"></div>
            <div class="hero-particles"></div>
            <div class="hero-content">
              <div class="hero-badge animate-badge">💆‍♀️ THƯ GIÃN</div>
              <h1 class="hero-title animate-title">Thư Giãn Tuyệt Đối</h1>
              <p class="hero-subtitle animate-subtitle">Trải nghiệm không gian yên tĩnh và sang trọng nhất</p>
              <button class="btn-cta tertiary animate-button" @click="scrollToAbout">
                <span>Tìm Hiểu Thêm</span>
                <i class="fa-solid fa-leaf"></i>
              </button>
            </div>
          </div>
        </div>
        
        <div class="carousel-item">
          <div class="hero-slide" style="background-image: url('/src/assets/img/hinh2.jpg')">
            <div class="hero-overlay"></div>
            <div class="hero-particles"></div>
            <div class="hero-content">
              <div class="hero-badge animate-badge">⭐ UY TÍN</div>
              <h1 class="hero-title animate-title">Đẳng Cấp 5 Sao</h1>
              <p class="hero-subtitle animate-subtitle">Chất lượng dịch vụ được hàng nghìn khách hàng tin tưởng</p>
              <button class="btn-cta quaternary animate-button" @click="goToReview">
                <span>Xem Đánh Giá</span>
                <i class="fa-solid fa-star"></i>
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Enhanced Navigation -->
      <button class="carousel-control-prev custom-nav" type="button" data-bs-target="#bannerCarousel" data-bs-slide="prev">
        <div class="nav-icon">
          <i class="fa-solid fa-chevron-left"></i>
        </div>
      </button>
      <button class="carousel-control-next custom-nav" type="button" data-bs-target="#bannerCarousel" data-bs-slide="next">
        <div class="nav-icon">
          <i class="fa-solid fa-chevron-right"></i>
        </div>
      </button>

      <!-- Modern Indicators -->
      <div class="custom-indicators">
        <button type="button" data-bs-target="#bannerCarousel" data-bs-slide-to="0" class="active"></button>
        <button type="button" data-bs-target="#bannerCarousel" data-bs-slide-to="1"></button>
        <button type="button" data-bs-target="#bannerCarousel" data-bs-slide-to="2"></button>
        <button type="button" data-bs-target="#bannerCarousel" data-bs-slide-to="3"></button>
      </div>
    </div>

    <!-- Enhanced Floating Elements -->
    <div class="floating-elements">
      <div class="floating-element element-1">🌸</div>
      <div class="floating-element element-2">🍃</div>
      <div class="floating-element element-3">✨</div>
      <div class="floating-element element-4">🌿</div>
      <div class="floating-element element-5">💎</div>
      <div class="floating-element element-6">🌙</div>
    </div>

    <!-- Scroll Indicator -->
    <!-- <div class="scroll-indicator">
      <div class="scroll-text">Scroll để khám phá</div>
      <div class="scroll-arrow">
        <i class="fa-solid fa-chevron-down"></i>
      </div>
    </div> -->
  </div>

  <!-- About Section with Glass Morphism -->
  <section id="about" class="about-section" ref="aboutSection">
    <div class="container">
      <div class="section-header" data-aos="fade-up">
        <span class="section-badge glass-badge">Về Chúng Tôi</span>
        <h2 class="section-title gradient-text">Câu Chuyện TUTA Spa</h2>
        <div class="section-divider">
          <div class="divider-dot"></div>
          <div class="divider-line"></div>
          <i class="fa-solid fa-spa divider-icon pulse"></i>
          <div class="divider-line"></div>
          <div class="divider-dot"></div>
        </div>
      </div>
      
      <div class="about-container" data-aos="fade-up" data-aos-delay="200">
        <div class="about-content">
          <div class="about-text">
            <h3 class="about-heading">Nơi Khởi Nguồn Vẻ Đẹp</h3>
            <p class="about-description">
              Với đội ngũ kỹ thuật viên <span class="text-highlight">giàu kinh nghiệm</span> và tâm huyết, 
              luôn tiên phong ứng dụng <span class="text-highlight">công nghệ hiện đại</span> nhất trong ngành làm đẹp, 
              <span class="brand-highlight">TUTA Spa</span> cam kết đem lại trải nghiệm dịch vụ hoàn hảo nhất.
            </p>
            <p class="about-mission">
              Sự hài lòng và niềm tin của khách hàng chính là <span class="text-highlight">động lực thành công</span> 
              lớn nhất của chúng tôi trong suốt hành trình phát triển.
            </p>
            <div class="about-stats">
              <div class="stat-mini" data-aos="zoom-in" data-aos-delay="300">
                <span class="stat-number count-up">1000+</span>
                <span class="stat-label">Khách hàng</span>
              </div>
              <div class="stat-mini" data-aos="zoom-in" data-aos-delay="400">
                <span class="stat-number count-up">5+</span>
                <span class="stat-label">Năm kinh nghiệm</span>
              </div>
              <div class="stat-mini" data-aos="zoom-in" data-aos-delay="500">
                <span class="stat-number count-up">4.9★</span>
                <span class="stat-label">Đánh giá</span>
              </div>
            </div>
          </div>
          <div class="about-visual">
            <div class="visual-card glass-card">
              <div class="visual-image">
                <img src="/src/assets/img/spa2.jpg" alt="TUTA Spa Interior" />
                <div class="image-badge glass-badge">
                  <i class="fa-solid fa-award"></i>
                  <span>Spa Uy Tín</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>

  <!-- Enhanced Benefits Section -->
  <section class="benefits-section">
    <div class="container">
      <div class="section-header" data-aos="fade-up">
        <span class="section-badge glass-badge">Ưu Điểm</span>
        <h2 class="section-title gradient-text">Tại Sao Chọn TUTA Spa?</h2>
        <p class="section-subtitle">Những lý do khiến hàng nghìn khách hàng tin tưởng và lựa chọn</p>
      </div>

      <div class="benefits-grid">
        <div 
          v-for="(benefit, i) in benefits" 
          :key="i" 
          class="benefit-item"
          :data-aos="i % 2 === 0 ? 'fade-right' : 'fade-left'"
          :data-aos-delay="i * 150"
        >
          <div class="benefit-card glass-card hover-glow">
            <div class="benefit-icon">
              <i :class="benefit.icon"></i>
            </div>
            <div class="benefit-content">
              <h4 class="benefit-title">{{ benefit.title }}</h4>
              <p class="benefit-description">{{ benefit.desc }}</p>
            </div>
            <div class="benefit-image">
              <img :src="benefit.image" :alt="benefit.title" />
            </div>
            <div class="benefit-number">{{ String(i + 1).padStart(2, '0') }}</div>
          </div>
        </div>
      </div>
    </div>
  </section>

  <!-- Enhanced Services Section -->
  <section id="services" class="services-section" ref="servicesSection">
    <div class="container">
      <div class="section-header" data-aos="fade-up">
        <span class="section-badge glass-badge">Dịch Vụ</span>
        <h2 class="section-title gradient-text">Khám Phá Dịch Vụ Nổi Bật</h2>
        <p class="section-subtitle">Trải nghiệm làm đẹp toàn diện với nhiều lựa chọn phù hợp mọi nhu cầu</p>
      </div>

      <!-- Enhanced Service Filter -->
      <div class="filter-section" data-aos="fade-up" data-aos-delay="200">
        <div class="filter-wrapper glass-panel">
          <button 
            @click="selectedTypeID = null" 
            :class="['filter-button', { 'active': selectedTypeID === null }]"
          >
            <span>Tất Cả Dịch Vụ</span>
          </button>
          <button 
            v-for="type in serviceTypes" 
            :key="type.loaiDichVuID" 
            @click="selectedTypeID = type.loaiDichVuID"
            :class="['filter-button', { 'active': selectedTypeID === type.loaiDichVuID }]"
          >
            <span>{{ type.tenLoai }}</span>
          </button>
        </div>
      </div>

      <!-- Enhanced Services Grid -->
      <div class="services-container" data-aos="fade-up" data-aos-delay="400">
        <div class="services-grid">
          <div 
            v-for="(service, index) in filteredServices" 
            :key="service.id" 
            class="service-item"
            :data-aos="'zoom-in'"
            :data-aos-delay="index * 100"
          >
            <div class="service-card glass-card hover-lift">
              <div class="service-image">
                <img :src="'images/' + service.image" :alt="service.name" />
                <div class="service-badge pulse">
                  <i class="fa-solid fa-percentage"></i>
                  <span>-25%</span>
                </div>
                <div class="service-overlay" @mouseenter="loadPricesForService(service.id)">
                  <div class="overlay-content">
                    <h5 class="service-name">{{ service.name }}</h5>
                    <div v-if="servicePrices[service.id]" class="service-prices">
                      <div class="prices-header">
                        <i class="fa-solid fa-tags"></i>
                        <span>Bảng Giá Dịch Vụ</span>
                      </div>
                      <div class="price-list glass-panel">
                        <div v-for="gia in servicePrices[service.id]" :key="gia.thoiLuong" class="price-row">
                          <span class="duration">{{ gia.thoiLuong }} phút</span>
                          <span class="price">{{ gia.gia.toLocaleString('vi-VN') }}₫</span>
                        </div>
                      </div>
                    </div>
                    <router-link :to="`/ChiTietDichVu/${service.id}`" class="service-button glass-button">
                      <span>Xem Chi Tiết</span>
                      <i class="fa-solid fa-arrow-right"></i>
                    </router-link>
                  </div>
                </div>
              </div>
              <div class="service-info">
                <h4 class="service-title">{{ service.name }}</h4>
                <div class="service-meta">
                  <div class="rating">
                    <i class="fa-solid fa-star"></i>
                    <span>4.9</span>
                  </div>
                  <div class="popularity">
                    <i class="fa-solid fa-heart"></i>
                    <span>Yêu thích</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>

  <!-- Enhanced CTA Section with Interactive Elements -->
  <section class="cta-section">
    <div class="container">
      <div class="cta-wrapper">
        <div class="cta-main glass-panel" data-aos="fade-right">
          <div class="cta-content">
            <div class="cta-header">
              <div class="cta-icon pulse">
                <i class="fa-solid fa-calendar-check"></i>
              </div>
              <div class="cta-text">
                <h3 class="cta-title">Đặt Lịch Nhanh Chóng</h3>
                <p class="cta-subtitle">Chỉ 30 giây - Trải nghiệm xong trả tiền - Hủy lịch miễn phí</p>
              </div>
            </div>
            
            <form class="booking-form" @submit.prevent="handleBookingSubmit">
              <div class="form-container">
                <div class="input-group">
                  <i class="fa-solid fa-phone input-icon"></i>
                  <input 
                    type="tel" 
                    v-model="phoneNumber"
                    class="form-input glass-input" 
                    placeholder="Nhập số điện thoại của bạn"
                    required
                  />
                </div>
                <button type="submit" class="submit-button glass-button hover-glow">
                  <span>Đặt Lịch Tư Vấn</span>
                  <i class="fa-solid fa-paper-plane"></i>
                </button>
              </div>
            </form>

            <div class="trust-indicators">
              <div class="trust-item">
                <i class="fa-solid fa-shield-alt"></i>
                <span>Bảo mật thông tin</span>
              </div>
              <div class="trust-item">
                <i class="fa-solid fa-clock"></i>
                <span>Phản hồi nhanh 24/7</span>
              </div>
              <div class="trust-item">
                <i class="fa-solid fa-undo"></i>
                <span>Hủy lịch miễn phí</span>
              </div>
            </div>
          </div>
        </div>
        
        <div class="cta-side glass-panel" data-aos="fade-left">
          <div class="review-section">
            <div class="review-header">
              <div class="review-icon pulse">
                <i class="fa-solid fa-quote-left"></i>
              </div>
              <h4 class="review-title">Chia Sẻ Trải Nghiệm</h4>
              <p class="review-subtitle">Đánh giá của bạn giúp chúng tôi hoàn thiện dịch vụ</p>
            </div>
            
            <div class="review-interaction">
              <router-link to="/DanhGia" class="rating-stars interactive-stars">
                <span v-for="n in 5" :key="n" class="star" @mouseover="highlightStars(n)" @mouseleave="resetStars">★</span>
              </router-link>
              <p class="rating-text">Nhấn để đánh giá dịch vụ</p>
            </div>

            <div class="review-stats">
              <div class="review-stat">
                <span class="stat-value count-up">4.9/5</span>
                <span class="stat-desc">Điểm trung bình</span>
              </div>
              <div class="review-stat">
                <span class="stat-value count-up">500+</span>
                <span class="stat-desc">Đánh giá</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>

  <!-- Enhanced Stats Section with Counter Animation -->
  <section class="stats-section">
    <div class="container">
      <div class="stats-header" data-aos="fade-up">
        <h2 class="stats-title gradient-text">TUTA Spa Trong Số Liệu</h2>
        <p class="stats-subtitle">Những con số ấn tượng khẳng định vị thế</p>
      </div>
      
      <div class="stats-grid">
        <div class="stat-card glass-card hover-glow" data-aos="zoom-in" data-aos-delay="0">
          <div class="stat-icon pulse">
            <i class="fa-solid fa-users"></i>
          </div>
          <div class="stat-info">
            <div class="stat-number count-up" ref="customersCount">1000+</div>
            <div class="stat-label">Khách Hàng Tin Tưởng</div>
            <div class="stat-description">Khách hàng thường xuyên và hài lòng</div>
          </div>
        </div>
        
        <div class="stat-card glass-card hover-glow" data-aos="zoom-in" data-aos-delay="100">
          <div class="stat-icon pulse">
            <i class="fa-solid fa-calendar-alt"></i>
          </div>
          <div class="stat-info">
            <div class="stat-number count-up" ref="experienceCount">5+</div>
            <div class="stat-label">Năm Kinh Nghiệm</div>
            <div class="stat-description">Phục vụ chuyên nghiệp không ngừng</div>
          </div>
        </div>
        
        <div class="stat-card glass-card hover-glow" data-aos="zoom-in" data-aos-delay="200">
          <div class="stat-icon pulse">
            <i class="fa-solid fa-spa"></i>
          </div>
          <div class="stat-info">
            <div class="stat-number count-up" ref="servicesCount">50+</div>
            <div class="stat-label">Dịch Vụ Đa Dạng</div>
            <div class="stat-description">Đáp ứng mọi nhu cầu làm đẹp</div>
          </div>
        </div>
        
        <div class="stat-card glass-card hover-glow" data-aos="zoom-in" data-aos-delay="300">
          <div class="stat-icon pulse">
            <i class="fa-solid fa-award"></i>
          </div>
          <div class="stat-info">
            <div class="stat-number count-up" ref="ratingCount">4.9</div>
            <div class="stat-label">Đánh Giá Trung Bình</div>
            <div class="stat-description">Chất lượng được khẳng định</div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup>
import { ref, onMounted, computed, nextTick } from 'vue';
import { useRouter } from 'vue-router';
import apiClient from "../../utils/axiosClient";

const router = useRouter();
const services = ref([]);
const serviceTypes = ref([]);
const selectedTypeID = ref(null);
const servicePrices = ref({});
const phoneNumber = ref("");
const aboutSection = ref(null);
const servicesSection = ref(null);

const benefits = [
  { 
    title: 'Đội Ngũ Chuyên Gia', 
    desc: 'Kỹ thuật viên giàu kinh nghiệm, được đào tạo bài bản và cập nhật công nghệ mới nhất', 
    image: '/src/assets/img/nhanvien.jpg',
    icon: 'fa-solid fa-user-md'
  },
  { 
    title: 'Thương Hiệu Ưu Việt', 
    desc: 'Được hàng nghìn khách hàng tin tưởng và đánh giá cao về chất lượng dịch vụ', 
    image: '/src/assets/img/logo.png',
    icon: 'fa-solid fa-medal'
  },
  { 
    title: 'Thiết Bị Hiện Đại', 
    desc: 'Đầu tư máy móc, thiết bị tối tân nhập khẩu từ các nước phát triển', 
    image: '/src/assets/img/maymoc.jpg',
    icon: 'fa-solid fa-cogs'
  },
  { 
    title: 'Dịch Vụ Hoàn Hảo', 
    desc: 'Phục vụ tận tâm, chu đáo với tiêu chuẩn 5 sao, mang đến sự hài lòng tuyệt đối', 
    image: '/src/assets/img/phucvu.png',
    icon: 'fa-solid fa-handshake'
  }
];

const filteredServices = computed(() => {
  return services.value.filter(s => !selectedTypeID.value || s.loaiDichVuID === selectedTypeID.value);
});

onMounted(async () => {
  try {
    const [dvRes, ldvRes] = await Promise.all([
      apiClient.get('api/DichVu'),
      apiClient.get('api/LoaiDichVu')
    ]);
    services.value = dvRes.data.map(s => ({
      id: s.dichVuID,
      name: s.tenDichVu,
      description: s.moTa,
      image: s.hinhAnh || "default-service.jpg",
      loaiDichVuID: s.loaiDichVuID
    }));
    serviceTypes.value = ldvRes.data;
  } catch (err) {
    console.error('Lỗi tải dữ liệu:', err);
  }

  // Initialize AOS with enhanced settings
  if (window.AOS) {
    window.AOS.init({
      duration: 1200,
      once: true,
      offset: 120,
      easing: 'ease-out-cubic',
      delay: 100
    });
  }

  // Initialize counter animations
  initCounterAnimations();
});

async function loadPricesForService(serviceId) {
  if (!servicePrices.value[serviceId]) {
    try {
      const res = await apiClient.get(`api/BangGiaDichVu/GetGiaTheoThoiGian/${serviceId}`);
      servicePrices.value[serviceId] = res.data;
    } catch (err) {
      console.error('Lỗi tải giá:', err);
      servicePrices.value[serviceId] = [];
    }
  }
}

function goToBooking() {
  router.push('/DatLich');
}

function goToReview() {
  router.push('/DanhGia');
}

function scrollToServices() {
  servicesSection.value?.scrollIntoView({ behavior: 'smooth' });
}

function scrollToAbout() {
  aboutSection.value?.scrollIntoView({ behavior: 'smooth' });
}

function handleBookingSubmit() {
  if (phoneNumber.value) {
    goToBooking();
  }
}

function highlightStars(n) {
  // Star highlighting logic
}

function resetStars() {
  // Reset star highlighting
}

function initCounterAnimations() {
  // Initialize counter animations for stats
}
</script>

<style scoped>
/* Enhanced Global Styles with Modern Variables */
:root {
  --primary-color: #00695c;
  --primary-light: #4db6ac;
  --primary-dark: #004d40;
  --secondary-color: #22c55e;
  --secondary-light: #86efac;
  --accent-color: #f59e0b;
  --accent-light: #fbbf24;
  --background-light: #f8fafc;
  --background-dark: #1e293b;
  --text-primary: #1f2937;
  --text-secondary: #64748b;
  --white: #ffffff;
  --glass-bg: rgba(255, 255, 255, 0.1);
  --glass-border: rgba(255, 255, 255, 0.2);
  --shadow-light: 0 4px 20px rgba(0, 0, 0, 0.08);
  --shadow-medium: 0 8px 30px rgba(0, 0, 0, 0.12);
  --shadow-heavy: 0 20px 60px rgba(0, 0, 0, 0.15);
  --shadow-glow: 0 0 50px rgba(34, 197, 94, 0.3);
  --border-radius: 16px;
  --border-radius-lg: 24px;
  --transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
  --transition-fast: all 0.2s ease;
  --gradient-primary: linear-gradient(135deg, var(--secondary-color), var(--primary-color));
  --gradient-accent: linear-gradient(135deg, var(--accent-color), #d97706);
  --gradient-text: linear-gradient(135deg, var(--primary-color), var(--secondary-color));
}

/* Glass Morphism Components */
.glass-panel {
  background: var(--glass-bg);
  backdrop-filter: blur(20px);
  border: 1px solid var(--glass-border);
  border-radius: var(--border-radius-lg);
}

.glass-card {
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(30px);
  border: 1px solid rgba(255, 255, 255, 0.3);
  border-radius: var(--border-radius-lg);
  box-shadow: var(--shadow-medium);
}

.glass-badge {
  background: var(--glass-bg);
  backdrop-filter: blur(20px);
  border: 1px solid var(--glass-border);
}

.glass-input {
  background: var(--glass-bg);
  backdrop-filter: blur(15px);
  border: 1px solid var(--glass-border);
}

.glass-button {
  background: var(--glass-bg);
  backdrop-filter: blur(15px);
  border: 1px solid var(--glass-border);
}

/* Enhanced Animations */
@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(50px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes slideInLeft {
  from {
    opacity: 0;
    transform: translateX(-50px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

@keyframes slideInRight {
  from {
    opacity: 0;
    transform: translateX(50px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

@keyframes pulse {
  0%, 100% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.05);
  }
}

@keyframes glow {
  0%, 100% {
    box-shadow: 0 0 20px rgba(34, 197, 94, 0.3);
  }
  50% {
    box-shadow: 0 0 40px rgba(34, 197, 94, 0.6);
  }
}

@keyframes floatAnimation {
  0%, 100% { 
    transform: translateY(0px) rotate(0deg); 
    opacity: 0.4; 
  }
  25% { 
    transform: translateY(-30px) rotate(5deg); 
    opacity: 0.7; 
  }
  50% { 
    transform: translateY(-20px) rotate(-5deg); 
    opacity: 0.5; 
  }
  75% { 
    transform: translateY(-25px) rotate(3deg); 
    opacity: 0.6; 
  }
}

@keyframes particleFloat {
  0% {
    transform: translateY(100vh) rotate(0deg);
    opacity: 0;
  }
  10% {
    opacity: 1;
  }
  90% {
    opacity: 1;
  }
  100% {
    transform: translateY(-100px) rotate(360deg);
    opacity: 0;
  }
}

@keyframes bounceArrow {
  0%, 20%, 50%, 80%, 100% {
    transform: translateY(0);
  }
  40% {
    transform: translateY(-10px);
  }
  60% {
    transform: translateY(-5px);
  }
}

/* Animation Classes */
.animate-badge {
  animation: fadeInUp 1s ease-out 0.3s both;
}

.animate-title {
  animation: slideInLeft 1s ease-out 0.6s both;
}

.animate-subtitle {
  animation: slideInRight 1s ease-out 0.9s both;
}

.animate-button {
  animation: fadeInUp 1s ease-out 1.2s both;
}

.pulse {
  animation: pulse 2s infinite;
}

.hover-glow:hover {
  animation: glow 2s infinite;
}

.hover-lift {
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.hover-lift:hover {
  transform: translateY(-10px) scale(1.02);
  box-shadow: var(--shadow-heavy);
}

.count-up {
  font-variant-numeric: tabular-nums;
}

/* Enhanced Hero Section */
.hero-section {
  position: relative;
  height: 100vh;
  overflow: hidden;
}

.hero-slide {
  height: 100vh;
  background-size: cover;
  background-position: center;
  background-attachment: fixed;
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
}

.hero-overlay {
  position: absolute;
  inset: 0;
  background: linear-gradient(135deg, 
    rgba(0, 105, 92, 0.8) 0%, 
    rgba(34, 197, 94, 0.6) 50%, 
    rgba(245, 158, 11, 0.4) 100%);
  backdrop-filter: blur(2px);
}

.hero-particles {
  position: absolute;
  inset: 0;
  overflow: hidden;
  pointer-events: none;
}

.hero-particles::before,
.hero-particles::after {
  content: '';
  position: absolute;
  width: 4px;
  height: 4px;
  background: rgba(255, 255, 255, 0.6);
  border-radius: 50%;
  animation: particleFloat 15s infinite linear;
}

.hero-particles::before {
  left: 20%;
  animation-delay: 0s;
}

.hero-particles::after {
  left: 80%;
  animation-delay: 7s;
}

.hero-content {
  position: relative;
  z-index: 3;
  text-align: center;
  color: var(--white);
  max-width: 900px;
  padding: 2rem;
}

.hero-badge {
  display: inline-block;
  background: var(--glass-bg);
  backdrop-filter: blur(25px);
  border: 1px solid var(--glass-border);
  padding: 0.75rem 1.5rem;
  border-radius: 50px;
  font-size: 0.9rem;
  font-weight: 600;
  letter-spacing: 1px;
  margin-bottom: 2rem;
  text-transform: uppercase;
  box-shadow: var(--shadow-light);
}

.hero-title {
  font-size: clamp(2.5rem, 5vw, 4.5rem);
  font-weight: 800;
  margin-bottom: 1.5rem;
  line-height: 1.1;
  background: linear-gradient(135deg, #ffffff 0%, #84cc16 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  text-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
}

.hero-subtitle {
  font-size: clamp(1.1rem, 2.5vw, 1.4rem);
  margin-bottom: 3rem;
  opacity: 0.95;
  font-weight: 400;
  line-height: 1.6;
  max-width: 700px;
  margin-left: auto;
  margin-right: auto;
}

/* Enhanced CTA Buttons */
.btn-cta {
  display: inline-flex;
  align-items: center;
  gap: 0.75rem;
  padding: 1.25rem 2.5rem;
  border: none;
  border-radius: 50px;
  font-weight: 600;
  font-size: 1rem;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  cursor: pointer;
  transition: var(--transition);
  position: relative;
  overflow: hidden;
  box-shadow: var(--shadow-medium);
}

.btn-cta::before {
  content: '';
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.2), transparent);
  transition: left 0.6s ease;
}

.btn-cta:hover::before {
  left: 100%;
}

.btn-cta.primary {
  background: var(--gradient-primary);
  color: var(--white);
}

.btn-cta.secondary {
  background: linear-gradient(135deg, var(--primary-color), var(--primary-dark));
  color: var(--white);
}

.btn-cta.tertiary {
  background: var(--gradient-accent);
  color: var(--white);
}

.btn-cta.quaternary {
  background: linear-gradient(135deg, #8b5cf6, #7c3aed);
  color: var(--white);
}

.btn-cta:hover {
  transform: translateY(-3px) scale(1.02);
  box-shadow: 0 15px 40px rgba(0, 0, 0, 0.2);
}

.btn-cta i {
  font-size: 1.1rem;
  transition: transform 0.3s ease;
}

.btn-cta:hover i {
  transform: translateX(3px);
}

/* Enhanced Navigation */
.custom-nav {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  background: none;
  border: none;
  z-index: 10;
  transition: var(--transition);
}

.nav-icon {
  width: 70px;
  height: 70px;
  background: var(--glass-bg);
  backdrop-filter: blur(25px);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--white);
  font-size: 1.5rem;
  transition: var(--transition);
  border: 1px solid var(--glass-border);
  box-shadow: var(--shadow-light);
}

.nav-icon:hover {
  background: rgba(34, 197, 94, 0.3);
  transform: scale(1.1);
  border-color: rgba(34, 197, 94, 0.5);
  box-shadow: var(--shadow-glow);
}

.carousel-control-prev { left: 2rem; }
.carousel-control-next { right: 2rem; }

.custom-indicators {
  position: absolute;
  bottom: 2rem;
  left: 50%;
  transform: translateX(-50%);
  display: flex;
  gap: 1rem;
  z-index: 10;
}

.custom-indicators button {
  width: 15px;
  height: 15px;
  border-radius: 50%;
  border: 2px solid rgba(255, 255, 255, 0.5);
  background: transparent;
  transition: var(--transition);
  cursor: pointer;
  position: relative;
}

.custom-indicators button::after {
  content: '';
  position: absolute;
  inset: -3px;
  border-radius: 50%;
  background: var(--gradient-primary);
  opacity: 0;
  transition: var(--transition);
  z-index: -1;
}

.custom-indicators button.active,
.custom-indicators button:hover {
  background: var(--secondary-color);
  border-color: var(--secondary-color);
  transform: scale(1.3);
}

.custom-indicators button.active::after {
  opacity: 0.3;
}

/* Enhanced Floating Elements */
.floating-elements {
  position: absolute;
  inset: 0;
  pointer-events: none;
  z-index: 1;
}

.floating-element {
  position: absolute;
  font-size: 2.5rem;
  opacity: 0.4;
  animation: floatAnimation 10s ease-in-out infinite;
  filter: drop-shadow(0 4px 8px rgba(0, 0, 0, 0.2));
}

.element-1 { top: 15%; left: 10%; animation-delay: 0s; }
.element-2 { top: 35%; right: 15%; animation-delay: 2s; }
.element-3 { bottom: 25%; left: 20%; animation-delay: 4s; }
.element-4 { top: 60%; right: 25%; animation-delay: 6s; }
.element-5 { top: 20%; right: 40%; animation-delay: 1s; font-size: 2rem; }
.element-6 { bottom: 40%; left: 45%; animation-delay: 5s; font-size: 2rem; }

/* Scroll Indicator */
.scroll-indicator {
  position: absolute;
  bottom: 1rem;
  left: 50%;
  transform: translateX(-50%);
  z-index: 10;
  text-align: center;
  color: var(--white);
}

.scroll-text {
  font-size: 0.85rem;
  margin-bottom: 0.5rem;
  opacity: 0.8;
}

.scroll-arrow {
  font-size: 1.2rem;
  animation: bounceArrow 2s infinite;
}

/* Enhanced Sections */
section {
  padding: 8rem 0;
  position: relative;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1rem;
}

.section-header {
  text-align: center;
  margin-bottom: 5rem;
}

.section-badge {
  display: inline-block;
  background: var(--gradient-primary);
  color: var(--white);
  padding: 0.5rem 1.5rem;
  border-radius: 25px;
  font-size: 0.85rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 1px;
  margin-bottom: 1rem;
  box-shadow: var(--shadow-light);
}

.section-title {
  font-size: clamp(2rem, 4vw, 3.5rem);
  font-weight: 800;
  color: var(--text-primary);
  margin-bottom: 1rem;
  line-height: 1.2;
}

.gradient-text {
  background: var(--gradient-text);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.section-subtitle {
  font-size: 1.2rem;
  color: var(--text-secondary);
  max-width: 600px;
  margin: 0 auto 2rem;
  line-height: 1.6;
}

.section-divider {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 1rem;
  margin-top: 1.5rem;
}

.divider-dot {
  width: 10px;
  height: 10px;
  background: var(--secondary-color);
  border-radius: 50%;
  box-shadow: 0 0 10px rgba(34, 197, 94, 0.5);
}

.divider-line {
  width: 80px;
  height: 3px;
  background: linear-gradient(90deg, transparent, var(--secondary-color), transparent);
  border-radius: 2px;
}

.divider-icon {
  color: var(--secondary-color);
  font-size: 1.8rem;
  filter: drop-shadow(0 2px 4px rgba(34, 197, 94, 0.3));
}

/* Enhanced About Section */
.about-section {
  background: linear-gradient(135deg, #f0fdf4 0%, #dcfce7 100%);
  position: relative;
}

.about-section::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 200px;
  background: linear-gradient(180deg, rgba(248, 250, 252, 0.8), transparent);
}

.about-container {
  max-width: 1100px;
  margin: 0 auto;
  position: relative;
  z-index: 2;
}

.about-content {
  display: grid;
  grid-template-columns: 1.2fr 1fr;
  gap: 5rem;
  align-items: center;
}

.about-text {
  padding-right: 2rem;
}

.about-heading {
  font-size: 2.2rem;
  font-weight: 700;
  color: var(--primary-color);
  margin-bottom: 1.5rem;
  line-height: 1.3;
}

.about-description {
  font-size: 1.15rem;
  line-height: 1.8;
  color: var(--text-primary);
  margin-bottom: 1.5rem;
}

.about-mission {
  font-size: 1.1rem;
  color: var(--text-secondary);
  font-style: italic;
  margin-bottom: 2rem;
  line-height: 1.7;
  padding: 1.5rem;
  background: var(--glass-bg);
  backdrop-filter: blur(10px);
  border-radius: var(--border-radius);
  border-left: 4px solid var(--secondary-color);
}

.text-highlight {
  color: var(--primary-color);
  font-weight: 600;
  position: relative;
}

.text-highlight::after {
  content: '';
  position: absolute;
  bottom: -2px;
  left: 0;
  right: 0;
  height: 2px;
  background: var(--gradient-primary);
  border-radius: 1px;
  opacity: 0.3;
}

.brand-highlight {
  background: var(--gradient-primary);
  -webkit-background-clip: text;
  -webkit-text-fill-color: black;
  background-clip: text;
  font-weight: 700;
  font-size: 1.1em;
}

.about-stats {
  display: flex;
  gap: 2.5rem;
  margin-top: 2rem;
}

.stat-mini {
  text-align: center;
  padding: 1rem;
  background: var(--glass-bg);
  backdrop-filter: blur(15px);
  border-radius: var(--border-radius);
  border: 1px solid var(--glass-border);
  transition: var(--transition);
}

.stat-mini:hover {
  transform: translateY(-5px);
  box-shadow: var(--shadow-medium);
}

.stat-mini .stat-number {
  display: block;
  font-size: 2rem;
  font-weight: 700;
  color: var(--secondary-color);
  margin-bottom: 0.25rem;
}

.stat-mini .stat-label {
  font-size: 0.9rem;
  color: var(--text-secondary);
  font-weight: 500;
}

.about-visual {
  position: relative;
}

.visual-card {
  position: relative;
  border-radius: var(--border-radius-lg);
  overflow: hidden;
  box-shadow: var(--shadow-heavy);
  transform: rotate(2deg);
  transition: var(--transition);
}

.visual-card:hover {
  transform: rotate(0deg) scale(1.02);
  box-shadow: var(--shadow-glow);
}

.visual-image {
  position: relative;
  height: 450px;
  overflow: hidden;
}

.visual-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.6s ease;
}

.visual-card:hover .visual-image img {
  transform: scale(1.1);
}

.image-badge {
  position: absolute;
  top: 1.5rem;
  right: 1.5rem;
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(20px);
  padding: 0.75rem 1.25rem;
  border-radius: 25px;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-weight: 600;
  color: var(--primary-color);
  font-size: 0.9rem;
  box-shadow: var(--shadow-light);
  transition: var(--transition);
}

.image-badge:hover {
  transform: scale(1.05);
  box-shadow: var(--shadow-medium);
}

/* Enhanced Benefits Section */
.benefits-section {
  background: linear-gradient(135deg, #e6f5ef 0%, #ccf2dd 100%);
  position: relative;
  overflow: hidden;
}

.benefits-section::before {
  content: '';
  position: absolute;
  top: -50%;
  left: -50%;
  width: 200%;
  height: 200%;
  background: radial-gradient(circle, rgba(34, 197, 94, 0.1) 0%, transparent 70%);
  animation: pulse 10s infinite;
}

.benefits-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 2.5rem;
  max-width: 1000px;
  margin: 0 auto;
  position: relative;
  z-index: 2;
}

.benefit-item {
  height: 100%;
}

.benefit-card {
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(30px);
  border-radius: var(--border-radius-lg);
  padding: 2.5rem;
  border: 1px solid rgba(255, 255, 255, 0.5);
  transition: var(--transition);
  position: relative;
  overflow: hidden;
  height: 100%;
  display: flex;
  flex-direction: column;
}

.benefit-card::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 4px;
  background: var(--gradient-primary);
  transform: scaleX(0);
  transition: transform 0.4s ease;
  border-radius: var(--border-radius-lg) var(--border-radius-lg) 0 0;
}

.benefit-card:hover {
  transform: translateY(-10px);
  box-shadow: var(--shadow-heavy);
}

.benefit-card:hover::before {
  transform: scaleX(1);
}

.benefit-icon {
  width: 80px;
  height: 80px;
  background: var(--gradient-primary);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--white);
  font-size: 2rem;
  margin-bottom: 1.5rem;
  transition: var(--transition);
  box-shadow: var(--shadow-medium);
}

.benefit-card:hover .benefit-icon {
  transform: scale(1.1) rotate(5deg);
  box-shadow: var(--shadow-glow);
}

.benefit-content {
  flex: 1;
}

.benefit-title {
  font-size: 1.5rem;
  font-weight: 700;
  color: var(--text-primary);
  margin-bottom: 1rem;
  line-height: 1.3;
}

.benefit-description {
  color: var(--text-secondary);
  line-height: 1.7;
  font-size: 1rem;
}

.benefit-image {
  position: absolute;
  top: 1rem;
  right: 1rem;
  width: 70px;
  height: 70px;
  border-radius: 50%;
  overflow: hidden;
  opacity: 0.1;
  transition: var(--transition);
  border: 3px solid rgba(255, 255, 255, 0.5);
}

.benefit-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.benefit-card:hover .benefit-image {
  opacity: 0.3;
  transform: scale(1.1);
}

.benefit-number {
  position: absolute;
  bottom: 1rem;
  right: 1rem;
  width: 50px;
  height: 50px;
  background: var(--gradient-accent);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--white);
  font-weight: 700;
  font-size: 1.2rem;
  box-shadow: var(--shadow-light);
  transition: var(--transition);
}

.benefit-card:hover .benefit-number {
  transform: scale(1.1);
  box-shadow: var(--shadow-medium);
}

/* Enhanced Services Section */
.services-section {
  background: linear-gradient(135deg, var(--background-light) 0%, #f1f5f9 100%);
  position: relative;
}

.filter-section {
  text-align: center;
  margin-bottom: 4rem;
}

.filter-wrapper {
  display: inline-flex;
  background: rgba(255, 255, 255, 0.9);
  backdrop-filter: blur(25px);
  border-radius: 50px;
  padding: 0.75rem;
  gap: 0.5rem;
  border: 1px solid rgba(0, 0, 0, 0.05);
  box-shadow: var(--shadow-medium);
}

.filter-button {
  padding: 0.75rem 1.5rem;
  border: none;
  background: transparent;
  border-radius: 25px;
  font-weight: 500;
  color: var(--text-secondary);
  transition: var(--transition);
  cursor: pointer;
  position: relative;
  white-space: nowrap;
}

.filter-button::before {
  content: '';
  position: absolute;
  inset: 0;
  border-radius: 25px;
  background: var(--gradient-primary);
  opacity: 0;
  transition: var(--transition);
  z-index: -1;
}

.filter-button:hover {
  color: var(--primary-color);
  transform: translateY(-2px);
}

.filter-button:hover::before {
  opacity: 0.1;
}

.filter-button.active {
  background: var(--gradient-primary);
  color: var(--white);
  box-shadow: var(--shadow-light);
  transform: translateY(-2px);
}

.services-container {
  max-width: 900px;
  margin: 0 auto;
}

.services-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(350px, 1fr));
  gap: 2.5rem;
}

.service-item {
  height: 100%;
}

.service-card {
  background: var(--white);
  border-radius: var(--border-radius-lg);
  overflow: hidden;
  border: 1px solid rgba(0, 0, 0, 0.05);
  transition: var(--transition);
  height: 100%;
  display: flex;
  flex-direction: column;
  box-shadow: var(--shadow-light);
  position: relative;
}

.service-card::before {
  content: '';
  position: absolute;
  inset: 0;
  border-radius: var(--border-radius-lg);
  padding: 2px;
  background: var(--gradient-primary);
  mask: linear-gradient(#fff 0 0) content-box, linear-gradient(#fff 0 0);
  mask-composite: xor;
  opacity: 0;
  transition: var(--transition);
}

.service-card:hover::before {
  opacity: 1;
}

.service-image {
  position: relative;
  height: 280px;
  overflow: hidden;
}

.service-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.6s ease;
}

.service-card:hover .service-image img {
  transform: scale(1.1);
}

.service-badge {
  position: absolute;
  top: 1rem;
  left: 1rem;
  background: linear-gradient(135deg, #ef4444, #dc2626);
  color: var(--white);
  padding: 0.5rem 1rem;
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 0.25rem;
  z-index: 2;
  animation: pulse 2s infinite;
}

.service-overlay {
  position: absolute;
  inset: 0;
  background: linear-gradient(135deg, 
    rgba(0, 105, 92, 0.95) 0%, 
    rgba(34, 197, 94, 0.9) 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  opacity: 0;
  transition: var(--transition);
  padding: 2rem;
  backdrop-filter: blur(5px);
}

.service-card:hover .service-overlay {
  opacity: 1;
}

.overlay-content {
  text-align: center;
  color: var(--white);
  transform: translateY(30px);
  transition: var(--transition);
}

.service-card:hover .overlay-content {
  transform: translateY(0);
}

.service-name {
  font-size: 1.6rem;
  font-weight: 700;
  margin-bottom: 1rem;
  line-height: 1.3;
}

.service-prices {
  margin-bottom: 1.5rem;
}

.prices-header {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  margin-bottom: 1rem;
  font-weight: 600;
  font-size: 1.1rem;
}

.price-list {
  background: rgba(255, 255, 255, 0.15);
  backdrop-filter: blur(15px);
  border-radius: var(--border-radius);
  padding: 1rem;
  border: 1px solid rgba(255, 255, 255, 0.2);
}

.price-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.5rem 0;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

.price-row:last-child {
  border-bottom: none;
}

.duration {
  font-weight: 500;
  font-size: 0.95rem;
}

.price {
  font-weight: 700;
  color: var(--accent-light);
  font-size: 1rem;
}

.service-button {
  background: rgba(255, 255, 255, 0.2);
  border: 1px solid rgba(255, 255, 255, 0.3);
  color: var(--white);
  padding: 0.75rem 1.5rem;
  border-radius: 25px;
  text-decoration: none;
  font-weight: 600;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  transition: var(--transition);
  backdrop-filter: blur(15px);
}

.service-button:hover {
  background: rgba(255, 255, 255, 0.3);
  color: var(--white);
  transform: translateY(-2px);
  box-shadow: var(--shadow-light);
}

.service-info {
  padding: 2rem;
  flex: 1;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.service-title {
  font-size: 1.4rem;
  font-weight: 700;
  color: var(--text-primary);
  margin-bottom: 1rem;
  line-height: 1.3;
}

.service-meta {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.rating, .popularity {
  display: flex;
  align-items: center;
  gap: 0.3rem;
  font-size: 0.9rem;
  color: var(--text-secondary);
  padding: 0.5rem 1rem;
  background: var(--glass-bg);
  border-radius: 20px;
  border: 1px solid var(--glass-border);
}

.rating i {
  color: var(--accent-color);
}

.popularity i {
  color: #ef4444;
}

/* Enhanced CTA Section */
.cta-section {
  background: linear-gradient(135deg, 
    var(--primary-dark) 0%, 
    var(--primary-color) 50%, 
    var(--secondary-color) 100%);
  color: var(--white);
  position: relative;
  overflow: hidden;
}

.cta-section::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: url('data:image/svg+xml,<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 20"><circle cx="10" cy="10" r="1" fill="rgba(255,255,255,0.1)"/><circle cx="30" cy="15" r="1.5" fill="rgba(255,255,255,0.08)"/><circle cx="50" cy="5" r="1" fill="rgba(255,255,255,0.1)"/><circle cx="70" cy="12" r="1.2" fill="rgba(255,255,255,0.06)"/><circle cx="90" cy="8" r="1" fill="rgba(255,255,255,0.09)"/></svg>');
  animation: floatAnimation 20s linear infinite;
}

.cta-wrapper {
  display: grid;
  grid-template-columns: 1.5fr 1fr;
  gap: 4rem;
  align-items: center;
  position: relative;
  z-index: 2;
}

.cta-main {
  background: rgba(255, 255, 255, 0.1);
  backdrop-filter: blur(25px);
  border-radius: var(--border-radius-lg);
  padding: 3.5rem;
  border: 1px solid rgba(255, 255, 255, 0.2);
  box-shadow: var(--shadow-heavy);
}

.cta-header {
  display: flex;
  align-items: center;
  gap: 1.5rem;
  margin-bottom: 2.5rem;
}

.cta-icon {
  width: 70px;
  height: 70px;
  background: var(--gradient-accent);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 2rem;
  color: var(--white);
  box-shadow: var(--shadow-medium);
  animation: pulse 3s infinite;
}

.cta-text h3 {
  font-size: 2rem;
  font-weight: 700;
  margin-bottom: 0.5rem;
  background: linear-gradient(135deg, #d1eb71 0%, #fbbf24 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.cta-text p {
  font-size: 1.1rem;
  opacity: 0.9;
  line-height: 1.6;
}

.booking-form {
  margin-bottom: 2rem;
}

.form-container {
  display: flex;
  gap: 1rem;
  align-items: stretch;
  flex-wrap: wrap;
}

.input-group {
  position: relative;
  flex: 1;
  min-width: 250px;
}

.input-icon {
  position: absolute;
  left: 1rem;
  top: 50%;
  transform: translateY(-50%);
  color: rgba(255, 255, 255, 0.7);
  font-size: 1.1rem;
  z-index: 2;
}

.form-input {
  width: 100%;
  padding: 1rem 1rem 1rem 3rem;
  border: none;
  border-radius: 25px;
  background: rgba(255, 255, 255, 0.15);
  backdrop-filter: blur(15px);
  color: var(--white);
  font-size: 1rem;
  border: 1px solid rgba(255, 255, 255, 0.2);
  transition: var(--transition);
}

.form-input::placeholder {
  color: rgba(255, 255, 255, 0.7);
}

.form-input:focus {
  outline: none;
  background: rgba(255, 255, 255, 0.2);
  border-color: rgba(255, 255, 255, 0.4);
  box-shadow: 0 0 20px rgba(255, 255, 255, 0.1);
  transform: translateY(-2px);
}

.submit-button {
  padding: 1rem 2rem;
  border: none;
  border-radius: 25px;
  background: var(--gradient-accent);
  color: var(--white);
  font-weight: 600;
  font-size: 1rem;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  transition: var(--transition);
  border: 1px solid rgba(255, 255, 255, 0.2);
  min-width: 180px;
  justify-content: center;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.submit-button:hover {
  transform: translateY(-3px);
  box-shadow: 0 10px 30px rgba(245, 158, 11, 0.4);
  background: linear-gradient(135deg, #f59e0b, #ea580c);
}

.trust-indicators {
  display: flex;
  justify-content: space-between;
  flex-wrap: wrap;
  gap: 1rem;
}

.trust-item {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 0.9rem;
  opacity: 0.9;
  padding: 0.75rem;
  background: rgba(255, 255, 255, 0.1);
  border-radius: 15px;
  border: 1px solid rgba(255, 255, 255, 0.1);
  flex: 1;
  min-width: 150px;
  justify-content: center;
  transition: var(--transition);
}

.trust-item:hover {
  background: rgba(255, 255, 255, 0.15);
  transform: translateY(-2px);
}

.trust-item i {
  color: var(--accent-light);
  font-size: 1rem;
}

.cta-side {
  background: rgba(255, 255, 255, 0.08);
  backdrop-filter: blur(25px);
  border-radius: var(--border-radius-lg);
  padding: 3rem;
  border: 1px solid rgba(255, 255, 255, 0.15);
  text-align: center;
}

.review-section {
  height: 100%;
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.review-header {
  margin-bottom: 2rem;
}

.review-icon {
  width: 60px;
  height: 60px;
  background: var(--gradient-primary);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
  color: var(--white);
  margin: 0 auto 1rem;
  box-shadow: var(--shadow-light);
}

.review-title {
  font-size: 1.5rem;
  font-weight: 700;
  margin-bottom: 0.5rem;
  color: var(--white);
}

.review-subtitle {
  font-size: 1rem;
  opacity: 0.8;
  line-height: 1.5;
}

.review-interaction {
  margin-bottom: 2rem;
}

.interactive-stars {
  display: flex;
  justify-content: center;
  gap: 0.5rem;
  margin-bottom: 1rem;
  text-decoration: none;
}

.star {
  font-size: 2rem;
  color: rgba(255, 255, 255, 0.3);
  transition: var(--transition);
  cursor: pointer;
}

.star:hover,
.star.highlighted {
  color: var(--accent-light);
  transform: scale(1.2);
  text-shadow: 0 0 10px rgba(251, 191, 36, 0.5);
}

.rating-text {
  font-size: 0.9rem;
  opacity: 0.8;
  margin: 0;
}

.review-stats {
  display: flex;
  justify-content: space-around;
  gap: 1rem;
}

.review-stat {
  text-align: center;
  padding: 1rem;
  background: rgba(255, 255, 255, 0.1);
  border-radius: var(--border-radius);
  border: 1px solid rgba(255, 255, 255, 0.1);
  flex: 1;
  transition: var(--transition);
}

.review-stat:hover {
  background: rgba(255, 255, 255, 0.15);
  transform: translateY(-3px);
  box-shadow: var(--shadow-light);
}

.stat-value {
  display: block;
  font-size: 1.8rem;
  font-weight: 700;
  color: var(--accent-light);
  margin-bottom: 0.25rem;
}

.stat-desc {
  font-size: 0.8rem;
  opacity: 0.8;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

/* Enhanced Stats Section */
.stats-section {
  background: linear-gradient(135deg, var(--background-dark) 0%, #334155 100%);
  color: var(--white);
  position: relative;
  overflow: hidden;
}

.stats-section::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: radial-gradient(ellipse at center, rgba(34, 197, 94, 0.1) 0%, transparent 70%);
}

.stats-header {
  text-align: center;
  margin-bottom: 5rem;
  position: relative;
  z-index: 2;
}

.stats-title {
  font-size: clamp(2.5rem, 5vw, 4rem);
  font-weight: 800;
  margin-bottom: 1rem;
  background: linear-gradient(135deg, #ffffff 0%, #22c55e 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.stats-subtitle {
  font-size: 1.3rem;
  opacity: 0.8;
  max-width: 600px;
  margin: 0 auto;
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 2.5rem;
  max-width: 1000px;
  margin: 0 auto;
  position: relative;
  z-index: 2;
}

.stat-card {
  background: rgba(255, 255, 255, 0.05);
  backdrop-filter: blur(25px);
  border-radius: var(--border-radius-lg);
  padding: 3rem 2rem;
  text-align: center;
  border: 1px solid rgba(255, 255, 255, 0.1);
  transition: var(--transition);
  position: relative;
  overflow: hidden;
}

.stat-card::before {
  content: '';
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.1), transparent);
  transition: left 0.8s ease;
}

.stat-card:hover::before {
  left: 100%;
}

.stat-card:hover {
  transform: translateY(-10px) scale(1.02);
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
  border-color: rgba(34, 197, 94, 0.3);
}

.stat-icon {
  width: 100px;
  height: 100px;
  background: var(--gradient-primary);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 2.5rem;
  color: var(--white);
  margin: 0 auto 2rem;
  box-shadow: var(--shadow-heavy);
  position: relative;
  z-index: 2;
}

.stat-card:hover .stat-icon {
  animation: pulse 2s infinite;
  box-shadow: var(--shadow-glow);
}

.stat-info {
  position: relative;
  z-index: 2;
}

.stat-number {
  font-size: clamp(2.5rem, 4vw, 4rem);
  font-weight: 800;
  color: var(--secondary-color);
  margin-bottom: 0.5rem;
  display: block;
  text-shadow: 0 4px 8px rgba(0, 0, 0, 0.3);
}

.stat-label {
  font-size: 1.3rem;
  font-weight: 600;
  margin-bottom: 1rem;
  color: var(--white);
  text-transform: uppercase;
  letter-spacing: 1px;
}

.stat-description {
  font-size: 1rem;
  opacity: 0.8;
  line-height: 1.5;
  color: rgba(255, 255, 255, 0.9);
}

/* Enhanced Responsive Design */
@media (max-width: 1200px) {
  .container {
    max-width: 95%;
  }
  
  .about-content {
    grid-template-columns: 1fr;
    gap: 3rem;
    text-align: center;
  }
  
  .about-text {
    padding-right: 0;
  }
  
  .cta-wrapper {
    grid-template-columns: 1fr;
    gap: 3rem;
  }
}

@media (max-width: 768px) {
  section {
    padding: 5rem 0;
  }
  
  .hero-content {
    padding: 1rem;
  }
  
  .hero-title {
    font-size: 2.5rem;
  }
  
  .hero-subtitle {
    font-size: 1.1rem;
  }
  
  .btn-cta {
    padding: 1rem 2rem;
    font-size: 0.9rem;
  }
  
  .about-stats {
    flex-direction: column;
    gap: 1rem;
  }
  
  .benefits-grid {
    grid-template-columns: 1fr;
    gap: 2rem;
  }
  
  .services-grid {
    grid-template-columns: 1fr;
    gap: 2rem;
  }
  
  .filter-wrapper {
    flex-wrap: wrap;
    justify-content: center;
  }
  
  .filter-button {
    font-size: 0.85rem;
    padding: 0.6rem 1.2rem;
  }
  
  .stats-grid {
    grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
    gap: 2rem;
  }
  
  .stat-card {
    padding: 2rem 1.5rem;
  }
  
  .cta-main,
  .cta-side {
    padding: 2rem;
  }
  
  .form-container {
    flex-direction: column;
    gap: 1rem;
  }
  
  .submit-button {
    width: 100%;
  }
  
  .trust-indicators {
    flex-direction: column;
  }
  
  .trust-item {
    min-width: auto;
  }
  
  .nav-icon {
    width: 50px;
    height: 50px;
    font-size: 1.2rem;
  }
  
  .carousel-control-prev { left: 1rem; }
  .carousel-control-next { right: 1rem; }
  
  .floating-element {
    font-size: 1.5rem;
  }
}

@media (max-width: 480px) {
  .hero-content {
    padding: 0.5rem;
  }
  
  .hero-title {
    font-size: 2rem;
  }
  
  .hero-subtitle {
    font-size: 1rem;
  }
  
  .btn-cta {
    padding: 0.8rem 1.5rem;
    font-size: 0.8rem;
  }
  
  .section-title {
    font-size: 2rem;
  }
  
  .benefit-card {
    padding: 2rem 1.5rem;
  }
  
  .benefit-icon {
    width: 60px;
    height: 60px;
    font-size: 1.5rem;
  }
  
  .stat-card {
    padding: 1.5rem 1rem;
  }
  
  .stat-icon {
    width: 80px;
    height: 80px;
    font-size: 2rem;
  }
  
  .cta-main,
  .cta-side {
    padding: 1.5rem;
  }
  
  .cta-header {
    flex-direction: column;
    text-align: center;
  }
  
  .cta-icon {
    width: 60px;
    height: 60px;
    font-size: 1.5rem;
  }
  
  .review-icon {
    width: 50px;
    height: 50px;
    font-size: 1.2rem;
  }
  
  .star {
    font-size: 1.5rem;
  }
}

/* Enhanced Loading States */
.loading-shimmer {
  background: linear-gradient(90deg, 
    rgba(255, 255, 255, 0.1) 25%, 
    rgba(255, 255, 255, 0.3) 50%, 
    rgba(255, 255, 255, 0.1) 75%);
  background-size: 200% 100%;
  animation: shimmer 2s infinite;
}

@keyframes shimmer {
  0% {
    background-position: -200% 0;
  }
  100% {
    background-position: 200% 0;
  }
}

/* Enhanced Scroll Animations */
.reveal {
  opacity: 0;
  transform: translateY(50px);
  transition: all 0.6s ease;
}

.reveal.active {
  opacity: 1;
  transform: translateY(0);
}

.reveal-left {
  opacity: 0;
  transform: translateX(-50px);
  transition: all 0.6s ease;
}

.reveal-left.active {
  opacity: 1;
  transform: translateX(0);
}

.reveal-right {
  opacity: 0;
  transform: translateX(50px);
  transition: all 0.6s ease;
}

.reveal-right.active {
  opacity: 1;
  transform: translateX(0);
}

/* Enhanced Performance Optimizations */
.gpu-accelerated {
  transform: translateZ(0);
  will-change: transform;
}

.smooth-scroll {
  scroll-behavior: smooth;
}

/* Enhanced Accessibility */
@media (prefers-reduced-motion: reduce) {
  *,
  *::before,
  *::after {
    animation-duration: 0.01ms !important;
    animation-iteration-count: 1 !important;
    transition-duration: 0.01ms !important;
  }
  
  .floating-element,
  .hero-particles::before,
  .hero-particles::after {
    animation: none;
  }
  
  .pulse {
    animation: none;
  }
}

/* Enhanced Focus States for Accessibility */
.filter-button:focus,
.btn-cta:focus,
.form-input:focus,
.submit-button:focus {
  outline: 2px solid var(--secondary-color);
  outline-offset: 2px;
}

/* Enhanced Print Styles */
@media print {
  .hero-section,
  .floating-elements,
  .scroll-indicator,
  .custom-nav,
  .custom-indicators {
    display: none;
  }
  
  section {
    page-break-inside: avoid;
    padding: 2rem 0;
  }
  
  .glass-card,
  .glass-panel {
    background: var(--white) !important;
    border: 1px solid #ddd !important;
    box-shadow: none !important;
  }
}

/* Dark Mode Support */
@media (prefers-color-scheme: dark) {
  :root {
    --background-light: #0f172a;
    --text-primary: #f1f5f9;
    --text-secondary: #94a3b8;
    --white: #1e293b;
  }
  
  .glass-card {
    background: rgba(30, 41, 59, 0.95);
    border-color: rgba(148, 163, 184, 0.2);
  }
  
  .about-section {
    background: linear-gradient(135deg, #0f172a 0%, #1e293b 100%);
  }
  
  .benefits-section {
    background: linear-gradient(135deg, #1e293b 0%, #334155 100%);
  }
  
  .services-section {
    background: linear-gradient(135deg, #0f172a 0%, #1e293b 100%);
  }
}
</style>
