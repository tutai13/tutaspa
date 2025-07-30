<template>
  <!-- Hero Carousel with Parallax Effect -->
  <div class="hero-section">
    <div id="bannerCarousel" class="carousel slide carousel-fade" data-bs-ride="carousel" data-bs-interval="5000">
      <div class="carousel-inner">
        <div class="carousel-item active">
          <div class="hero-slide" style="background-image: url('/src/assets/img/BNR1.jpg')">
            <div class="hero-overlay"></div>
            <div class="hero-content">
              <div class="hero-badge">🌟 SPA 5 SAO</div>
              <h1 class="hero-title">Chào Mừng Đến TUTA Spa</h1>
              <p class="hero-subtitle">Nơi thư giãn và tái tạo năng lượng cho tâm hồn bạn</p>
              <button class="btn-cta primary" @click="goToBooking">
                <span>Đặt Lịch Ngay</span>
                <i class="fa-solid fa-calendar-plus"></i>
              </button>
            </div>
          </div>
        </div>
        <div class="carousel-item">
          <div class="hero-slide" style="background-image: url('/src/assets/img/BNR2.jpg')">
            <div class="hero-overlay"></div>
            <div class="hero-content">
              <div class="hero-badge">🔬 CÔNG NGHỆ</div>
              <h1 class="hero-title">Công Nghệ Hiện Đại</h1>
              <p class="hero-subtitle">Thiết bị tối tân kết hợp kỹ thuật chuyên nghiệp hàng đầu</p>
              <button class="btn-cta secondary" @click="scrollToServices">
                <span>Khám Phá Dịch Vụ</span>
                <i class="fa-solid fa-magic"></i>
              </button>
            </div>
          </div>
        </div>
        <div class="carousel-item">
          <div class="hero-slide" style="background-image: url('/src/assets/img/spa2.jpg')">
            <div class="hero-overlay"></div>
            <div class="hero-content">
              <div class="hero-badge">💆‍♀️ THƯ GIÃN</div>
              <h1 class="hero-title">Thư Giãn Tuyệt Đối</h1>
              <p class="hero-subtitle">Trải nghiệm không gian yên tĩnh và sang trọng nhất</p>
              <button class="btn-cta tertiary" @click="scrollToAbout">
                <span>Tìm Hiểu Thêm</span>
                <i class="fa-solid fa-leaf"></i>
              </button>
            </div>
          </div>
        </div>
        <div class="carousel-item">
          <div class="hero-slide" style="background-image: url('/src/assets/img/hinh2.jpg')">
            <div class="hero-overlay"></div>
            <div class="hero-content">
              <div class="hero-badge">⭐ UY TÍN</div>
              <h1 class="hero-title">Đẳng Cấp 5 Sao</h1>
              <p class="hero-subtitle">Chất lượng dịch vụ được hàng nghìn khách hàng tin tưởng</p>
              <button class="btn-cta quaternary" @click="goToReview">
                <span>Xem Đánh Giá</span>
                <i class="fa-solid fa-star"></i>
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Custom Navigation -->
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

      <!-- Custom Indicators -->
      <div class="custom-indicators">
        <button type="button" data-bs-target="#bannerCarousel" data-bs-slide-to="0" class="active" aria-label="Slide 1"></button>
        <button type="button" data-bs-target="#bannerCarousel" data-bs-slide-to="1" aria-label="Slide 2"></button>
        <button type="button" data-bs-target="#bannerCarousel" data-bs-slide-to="2" aria-label="Slide 3"></button>
        <button type="button" data-bs-target="#bannerCarousel" data-bs-slide-to="3" aria-label="Slide 4"></button>
      </div>
    </div>

    <!-- Floating Elements -->
    <div class="floating-elements">
      <div class="floating-element element-1">🌸</div>
      <div class="floating-element element-2">🍃</div>
      <div class="floating-element element-3">✨</div>
      <div class="floating-element element-4">🌿</div>
    </div>
  </div>


  <!-- About Section -->
  <section id="about" class="about-section" ref="aboutSection">
    <div class="container">
      <div class="section-header" data-aos="fade-up">
        <span class="section-badge">Về Chúng Tôi</span>
        <h2 class="section-title">Câu Chuyện TUTA Spa</h2>
        <div class="section-divider">
          <div class="divider-dot"></div>
          <div class="divider-line"></div>
          <i class="fa-solid fa-spa divider-icon"></i>
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
              <div class="stat-mini">
                <span class="stat-number">1000+</span>
                <span class="stat-label">Khách hàng</span>
              </div>
              <div class="stat-mini">
                <span class="stat-number">5+</span>
                <span class="stat-label">Năm kinh nghiệm</span>
              </div>
              <div class="stat-mini">
                <span class="stat-number">4.9★</span>
                <span class="stat-label">Đánh giá</span>
              </div>
            </div>
          </div>
          <div class="about-visual">
            <div class="visual-card">
              <div class="visual-image">
                <img src="/src/assets/img/spa2.jpg" alt="TUTA Spa Interior" />
                <div class="image-badge">
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

  <!-- Benefits Section -->
  <section class="benefits-section">
    <div class="container">
      <div class="section-header" data-aos="fade-up">
        <span class="section-badge">Ưu Điểm</span>
        <h2 class="section-title">Tại Sao Chọn TUTA Spa?</h2>
        <p class="section-subtitle">Những lý do khiến hàng nghìn khách hàng tin tưởng và lựa chọn</p>
      </div>

  <!-- Floating Elements -->
  <div class="floating-elements">
    <div class="floating-element element-1">🌸</div>
    <div class="floating-element element-2">🍃</div>
    <div class="floating-element element-3">✨</div>
    <div class="floating-element element-4">🌿</div>
  </div>
  <h2 class="fw-bold text-green mb-3 text-uppercase">ĐÔI NÉT VỀ TUTA SPA</h2>
  <div class="d-flex justify-content-center align-items-center mb-4">
    <div class="border-top border-2 border-success w-25"></div>
    <i class="fas fa-leaf text-green mx-3"></i>
    <div class="border-top border-2 border-success w-25"></div>
  </div>
  <div class="mx-auto px-3 px-md-5 col-lg-8">
    <p class="text-muted fs-5 lh-lg">
      Với đội ngũ kỹ thuật viên <strong>giàu kinh nghiệm</strong>, luôn tiên
      phong ứng dụng <em>công nghệ hiện đại</em> và phục vụ chuyên nghiệp,
      <span class="fw-semibold text-green">TUTA Spa</span>
      <u> hứa hẹn đem lại trải nghiệm dịch vụ tốt nhất </u> sự hài lòng của
      khách hàng <strong> là sự thành công </strong> của chúng tôi.
    </p>
  </div>


      <div class="benefits-grid">
        <div 
          v-for="(benefit, i) in benefits" 
          :key="i" 
          class="benefit-item"
          :data-aos="i % 2 === 0 ? 'fade-right' : 'fade-left'"
          :data-aos-delay="i * 150"
        >
          <div class="benefit-card">
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

  <!-- Services Section -->
  <section id="services" class="services-section" ref="servicesSection">
    <div class="container">
      <div class="section-header" data-aos="fade-up">
        <span class="section-badge">Dịch Vụ</span>
        <h2 class="section-title">Khám Phá Dịch Vụ Nổi Bật</h2>
        <p class="section-subtitle">Trải nghiệm làm đẹp toàn diện với nhiều lựa chọn phù hợp mọi nhu cầu</p>
      </div>

      <!-- Service Filter -->
      <div class="filter-section" data-aos="fade-up" data-aos-delay="200">
        <div class="filter-wrapper">
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

      <!-- Services Grid -->
      <div class="services-container" data-aos="fade-up" data-aos-delay="400">
        <div class="services-grid">
          <div 
            v-for="(service, index) in filteredServices" 
            :key="service.id" 
            class="service-item"
            :data-aos="'zoom-in'"
            :data-aos-delay="index * 100"
          >
            <div class="service-card">
              <div class="service-image">
                <img :src="'http://localhost:5055/images/' + service.image" :alt="service.name" />
                <div class="service-badge">
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
                      <div class="price-list">
                        <div v-for="gia in servicePrices[service.id]" :key="gia.thoiLuong" class="price-row">
                          <span class="duration">{{ gia.thoiLuong }} phút</span>
                          <span class="price">{{ gia.gia.toLocaleString('vi-VN') }}₫</span>
                        </div>
                      </div>
                    </div>
                    <router-link :to="`/ChiTietDichVu/${service.id}`" class="service-button">
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

  <!-- CTA Section -->
  <section class="cta-section">
    <div class="container">
      <div class="cta-wrapper">
        <div class="cta-main" data-aos="fade-right">
          <div class="cta-content">
            <div class="cta-header">
              <div class="cta-icon">
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
                    class="form-input" 
                    placeholder="Nhập số điện thoại của bạn"
                    required
                  />
                </div>
                <button type="submit" class="submit-button">
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
        
        <div class="cta-side" data-aos="fade-left">
          <div class="review-section">
            <div class="review-header">
              <div class="review-icon">
                <i class="fa-solid fa-quote-left"></i>
              </div>
              <h4 class="review-title">Chia Sẻ Trải Nghiệm</h4>
              <p class="review-subtitle">Đánh giá của bạn giúp chúng tôi hoàn thiện dịch vụ</p>
            </div>
            
            <div class="review-interaction">
              <router-link to="/DanhGia" class="rating-stars">
                <span v-for="n in 5" :key="n" class="star">★</span>
              </router-link>
              <p class="rating-text">Nhấn để đánh giá dịch vụ</p>
            </div>

            <div class="review-stats">
              <div class="review-stat">
                <span class="stat-value">4.9/5</span>
                <span class="stat-desc">Điểm trung bình</span>
              </div>
              <div class="review-stat">
                <span class="stat-value">500+</span>
                <span class="stat-desc">Đánh giá</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>

  <!-- Stats Section -->
  <section class="stats-section">
    <div class="container">
      <div class="stats-header" data-aos="fade-up">
        <h2 class="stats-title">TUTA Spa Trong Số Liệu</h2>
        <p class="stats-subtitle">Những con số ấn tượng khẳng định vị thế</p>
      </div>
      
      <div class="stats-grid">
        <div class="stat-card" data-aos="zoom-in" data-aos-delay="0">
          <div class="stat-icon">
            <i class="fa-solid fa-users"></i>
          </div>
          <div class="stat-info">
            <div class="stat-number" ref="customersCount">1000+</div>
            <div class="stat-label">Khách Hàng Tin Tưởng</div>
            <div class="stat-description">Khách hàng thường xuyên và hài lòng</div>
          </div>
        </div>
        
        <div class="stat-card" data-aos="zoom-in" data-aos-delay="100">
          <div class="stat-icon">
            <i class="fa-solid fa-calendar-alt"></i>
          </div>
          <div class="stat-info">
            <div class="stat-number" ref="experienceCount">5+</div>
            <div class="stat-label">Năm Kinh Nghiệm</div>
            <div class="stat-description">Phục vụ chuyên nghiệp không ngừng</div>
          </div>
        </div>
        
        <div class="stat-card" data-aos="zoom-in" data-aos-delay="200">
          <div class="stat-icon">
            <i class="fa-solid fa-spa"></i>
          </div>
          <div class="stat-info">
            <div class="stat-number" ref="servicesCount">50+</div>
            <div class="stat-label">Dịch Vụ Đa Dạng</div>
            <div class="stat-description">Đáp ứng mọi nhu cầu làm đẹp</div>
          </div>
        </div>
        
        <div class="stat-card" data-aos="zoom-in" data-aos-delay="300">
          <div class="stat-icon">
            <i class="fa-solid fa-award"></i>
          </div>
          <div class="stat-info">
            <div class="stat-number" ref="ratingCount">4.9</div>
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
import axios from 'axios';

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

//  return services.value.filter(
 //   (s) => !selectedTypeID.value || s.loaiDichVuID === selectedTypeID.value
 // );

});

onMounted(async () => {
  try {
    const [dvRes, ldvRes] = await Promise.all([
      axios.get('http://localhost:5055/api/DichVu'),
      axios.get('http://localhost:5055/api/LoaiDichVu')
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

  // Initialize AOS
  if (window.AOS) {
    window.AOS.init({
      duration: 1000,
      once: true,
      offset: 120,
      easing: 'ease-out-cubic'
    });
  }
});

async function loadPricesForService(serviceId) {
  if (!servicePrices.value[serviceId]) {
    try {
      const res = await axios.get(`http://localhost:5055/api/BangGiaDichVu/GetGiaTheoThoiGian/${serviceId}`);
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
    // Store phone number and redirect
    localStorage.setItem('bookingPhone', phoneNumber.value);
    goToBooking();
  }
}
</script>

<style scoped>
/* Global Styles */
/* * {
  font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
} */

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
  --shadow-light: 0 4px 20px rgba(0, 0, 0, 0.08);
  --shadow-medium: 0 8px 30px rgba(0, 0, 0, 0.12);
  --shadow-heavy: 0 20px 60px rgba(0, 0, 0, 0.15);
  --border-radius: 16px;
  --border-radius-lg: 24px;
  --transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}

/* Hero Section */
.hero-section {
  position: relative;
  height: 100vh;
  overflow: hidden;
}

.carousel-fade .carousel-item {
  transition: opacity 2s ease-in-out;
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
    rgba(0, 105, 92, 0.85) 0%, 
    rgba(34, 197, 94, 0.7) 50%, 
    rgba(245, 158, 11, 0.6) 100%);
  backdrop-filter: blur(1px);
}

.hero-content {
  position: relative;
  z-index: 2;
  text-align: center;
  color: var(--white);
  max-width: 900px;
  padding: 2rem;
}

.hero-badge {
  display: inline-block;
  background: rgba(255, 255, 255, 0.15);
  backdrop-filter: blur(20px);
  border: 1px solid rgba(255, 255, 255, 0.2);
  padding: 0.75rem 1.5rem;
  border-radius: 50px;
  font-size: 0.9rem;
  font-weight: 600;
  letter-spacing: 1px;
  margin-bottom: 2rem;
  text-transform: uppercase;
  animation: fadeInUp 1s ease-out 0.3s both;
}
.btn-primary {
  background-color: #04ee69;
  color: #fff;
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
  animation: fadeInUp 1s ease-out 1.2s both;
  box-shadow: var(--shadow-medium);
}

.btn-cta.primary {
  background: linear-gradient(
    135deg,
    var(--secondary-color),
    var(--primary-color)
  );
  color: var(--white);
}

.btn-cta.secondary {
  background: linear-gradient(
    135deg,
    var(--primary-color),
    var(--primary-dark)
  );
  color: var(--white);
}

.btn-cta.tertiary {
  background: linear-gradient(135deg, var(--accent-color), #d97706);
  color: var(--white);
}

.btn-cta.quaternary {
  background: linear-gradient(135deg, #8b5cf6, #7c3aed);
  color: var(--white);
}

.btn-cta::before {
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
  animation: fadeInUp 1s ease-out 0.6s both;
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
  animation: fadeInUp 1s ease-out 0.9s both;
}

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
  animation: fadeInUp 1s ease-out 1.2s both;
  box-shadow: var(--shadow-medium);
}
.text-highlight {
  color: var(--primary-color);
  font-weight: 600;
}

.brand-highlight {
  background: linear-gradient(
    135deg,
    var(--secondary-color),
    var(--primary-color)
  );
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  font-weight: 700;
  font-size: 1.1em;
}

.about-stats {
  display: flex;
  gap: 2rem;
  margin-top: 2rem;
}

.stat-mini {
  text-align: center;
}

.stat-mini .stat-number {
  display: block;
  font-size: 1.8rem;
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
}

.visual-image {
  position: relative;
  height: 400px;
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
}

/* Benefits Section */
.benefits-section {
  background: linear-gradient(135deg, #e6f5ef 0%, #ccf2dd 100%);
}

.benefits-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 2rem;
  max-width: 1000px;
  margin: 0 auto;
}

.benefit-item {
  height: 100%;
}

.benefit-card {
  background: rgba(255, 255, 255, 0.9);
  backdrop-filter: blur(20px);
  border-radius: var(--border-radius-lg);
  padding: 2rem;
  border: 1px solid rgba(255, 255, 255, 0.5);
  transition: var(--transition);
  position: relative;
  overflow: hidden;
  height: 100%;
  display: flex;
  flex-direction: column;
}

.benefit-card::before {
  content: "";
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 4px;
  background: linear-gradient(
    90deg,
    var(--secondary-color),
    var(--primary-color)
  );
  transform: scaleX(0);
  transition: transform 0.4s ease;
  border-radius: var(--border-radius-lg) var(--border-radius-lg) 0 0;
}

.benefit-card:hover {
  transform: translateY(-8px);
  box-shadow: var(--shadow-heavy);
}

.benefit-card:hover::before {
  transform: scaleX(1);
}

.benefit-icon {
  width: 70px;
  height: 70px;
  background: linear-gradient(
    135deg,
    var(--secondary-color),
    var(--primary-color)
  );
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--white);
  font-size: 1.8rem;
  margin-bottom: 1.5rem;
  transition: var(--transition);
}

.benefit-card:hover .benefit-icon {
  transform: scale(1.1) rotate(5deg);
}

.benefit-content {
  flex: 1;
}

.benefit-title {
  font-size: 1.4rem;
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
  width: 60px;
  height: 60px;
  border-radius: 50%;
  overflow: hidden;
  opacity: 0.1;
  transition: var(--transition);
}

.benefit-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.benefit-card:hover .benefit-image {
  opacity: 0.2;
  transform: scale(1.1);
}

.benefit-number {
  position: absolute;
  bottom: 1rem;
  right: 1rem;
  width: 40px;
  height: 40px;
  background: linear-gradient(135deg, var(--accent-color), #d97706);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--white);
  font-weight: 700;
  font-size: 1.1rem;
  box-shadow: var(--shadow-light);
}

/* Services Section */
.services-section {
  background: linear-gradient(135deg, var(--background-light) 0%, #f1f5f9 100%);
}

.filter-section {
  text-align: center;
  margin-bottom: 3rem;
}

.filter-wrapper {
  display: inline-flex;
  background: rgba(255, 255, 255, 0.8);
  backdrop-filter: blur(20px);
  border-radius: 50px;
  padding: 0.5rem;
  gap: 0.5rem;
  border: 1px solid rgba(0, 0, 0, 0.05);
  box-shadow: var(--shadow-light);
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
  justify-content: space-between;
  transition: transform 0.3s;

}

.btn-cta.primary {
  background: linear-gradient(135deg, var(--secondary-color), var(--primary-color));
  color: var(--white);
}

.btn-cta.secondary {
  background: linear-gradient(135deg, var(--primary-color), var(--primary-dark));
  color: var(--white);
}

.btn-cta.tertiary {
  background: linear-gradient(135deg, var(--accent-color), #d97706);
  color: var(--white);
}

.btn-cta.quaternary {
  background: linear-gradient(135deg, #8b5cf6, #7c3aed);
  color: var(--white);
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

  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 105, 92, 0.85);
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.service-title {
  font-size: 1.3rem;
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

.rating,
.popularity {
  display: flex;
  align-items: center;
  gap: 0.25rem;
  font-size: 0.9rem;
  color: var(--text-secondary);
}

.rating i {
  color: var(--accent-color);
}

.popularity i {
  color: #ef4444;
}

/* CTA Section */
.cta-section {
  background: linear-gradient(
    135deg,
    var(--primary-dark) 0%,
    var(--primary-color) 50%,
    var(--secondary-color) 100%
  );
  color: var(--white);
}

.cta-wrapper {
  display: grid;
  grid-template-columns: 1.5fr 1fr;
  gap: 3rem;
  align-items: center;
}

.cta-main {
  background: rgba(255, 255, 255, 0.1);
  backdrop-filter: blur(20px);
  border-radius: var(--border-radius-lg);
  padding: 3rem;
  border: 1px solid rgba(255, 255, 255, 0.2);
}

.cta-header {
  display: flex;
  align-items: flex-start;
  gap: 1.5rem;
  margin-bottom: 2rem;
}

.cta-icon {
  width: 70px;
  height: 70px;
  background: rgba(255, 255, 255, 0.2);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.8rem;
  flex-shrink: 0;
}

.cta-text {
  flex: 1;
}

.cta-title {
  font-size: 2.2rem;
  font-weight: 800;
  margin-bottom: 0.5rem;
  line-height: 1.2;
}

.cta-subtitle {
  font-size: 1.1rem;
  opacity: 0.9;
  line-height: 1.5;
}

.booking-form {
  margin-bottom: 2rem;
}

.form-container {
  display: flex;
  gap: 1rem;
  margin-bottom: 1.5rem;
}

.input-group {
  position: relative;
  flex: 1;
}

.input-icon {
  position: absolute;
  left: 1.25rem;
  top: 50%;
  transform: translateY(-50%);
  color: rgba(255, 255, 255, 0.7);
  z-index: 2;
}

.form-input {
  width: 100%;
  padding: 1rem 1rem 1rem 3rem;
  border: 1px solid rgba(255, 255, 255, 0.3);
  border-radius: 25px;
  background: rgba(255, 255, 255, 0.1);
  backdrop-filter: blur(10px);
  color: var(--white);
  font-size: 1rem;
  transition: var(--transition);
}

.form-input::placeholder {
  color: rgba(255, 255, 255, 0.7);
}

.form-input:focus {
  outline: none;
  border-color: rgba(255, 255, 255, 0.6);
  background: rgba(255, 255, 255, 0.15);
  box-shadow: 0 0 0 3px rgba(255, 255, 255, 0.1);
}

.submit-button {
  background: linear-gradient(135deg, var(--accent-color), #d97706);
  border: none;
  padding: 1rem 2rem;
  border-radius: 25px;
  color: var(--white);
  font-weight: 600;
  cursor: pointer;
  transition: var(--transition);
  display: flex;
  align-items: center;
  gap: 0.5rem;
  white-space: nowrap;
  box-shadow: var(--shadow-medium);
}

.submit-button:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 25px rgba(245, 158, 11, 0.4);
}

.trust-indicators {
  display: flex;
  gap: 2rem;
  flex-wrap: wrap;
}

.trust-item {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 0.9rem;
  opacity: 0.8;
}

.trust-item i {
  color: var(--secondary-light);
}

.cta-side {
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(20px);
  border-radius: var(--border-radius-lg);
  padding: 2.5rem;
  border: 1px solid rgba(255, 255, 255, 0.3);
  color: var(--text-primary);
}

.review-section {
  text-align: center;
}

.review-header {
  margin-bottom: 2rem;
}

.review-icon {
  width: 60px;
  height: 60px;
  background: linear-gradient(135deg, var(--accent-color), #d97706);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--white);
  font-size: 1.5rem;
  margin: 0 auto 1rem;

}

.btn-cta:hover {
  transform: translateY(-3px) scale(1.02);
  box-shadow: 0 15px 40px rgba(0, 0, 0, 0.2);
}

.btn-cta:hover::before {
  left: 100%;
}

.btn-cta i {
  font-size: 1.1rem;
  transition: transform 0.3s ease;
}

.btn-cta:hover i {
  transform: translateX(3px);
}

/* Navigation Controls */
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
  width: 60px;
  height: 60px;
  background: rgba(255, 255, 255, 0.1);
  backdrop-filter: blur(20px);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--white);
  font-size: 1.5rem;
  transition: var(--transition);
  border: 1px solid rgba(255, 255, 255, 0.2);
}

.nav-icon:hover {
  background: rgba(34, 197, 94, 0.3);
  transform: scale(1.1);
  border-color: rgba(34, 197, 94, 0.5);
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
  width: 12px;
  height: 12px;
  border-radius: 50%;
  border: 2px solid rgba(255, 255, 255, 0.5);
  background: transparent;
  transition: var(--transition);
  cursor: pointer;
}

.custom-indicators button.active,
.custom-indicators button:hover {
  background: var(--secondary-color);
  border-color: var(--secondary-color);
  transform: scale(1.3);
}

/* Floating Elements */
.floating-elements {
  position: absolute;
  inset: 0;
  pointer-events: none;
  z-index: 1;
}
.chat-window {
  width: 350px;
  height: 500px;
  background: white;
  border-radius: 20px;
  box-shadow: 0 15px 35px rgba(0, 0, 0, 0.2);
  overflow: hidden;
}

.stat-card::before {
  content: "";
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 3px;
  background: linear-gradient(
    90deg,
    var(--secondary-color),
    var(--primary-color)
  );
  transform: scaleX(0);
  transition: transform 0.4s ease;
}

.stat-card:hover {
  transform: translateY(-5px);
  background: rgba(255, 255, 255, 0.15);
}

.stat-card:hover::before {
  transform: scaleX(1);
}

.stat-icon {
  width: 70px;
  height: 70px;
  background: linear-gradient(
    135deg,
    var(--secondary-color),
    var(--primary-color)
  );
  border-radius: 50%;
  display: flex;
  flex-direction: column;
  animation: slideUp 0.3s ease;

}

.floating-element {
  position: absolute;
  font-size: 2rem;
  opacity: 0.3;
  animation: floatAnimation 8s ease-in-out infinite;
}

.element-1 { top: 15%; left: 10%; animation-delay: 0s; }
.element-2 { top: 35%; right: 15%; animation-delay: 2s; }
.element-3 { bottom: 25%; left: 20%; animation-delay: 4s; }
.element-4 { top: 60%; right: 25%; animation-delay: 6s; }

@keyframes floatAnimation {
  0%, 100% { transform: translateY(0px) rotate(0deg); opacity: 0.3; }
  25% { transform: translateY(-25px) rotate(5deg); opacity: 0.6; }
  50% { transform: translateY(-15px) rotate(-5deg); opacity: 0.4; }
  75% { transform: translateY(-20px) rotate(3deg); opacity: 0.5; }
}

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

/* Common Section Styles */
section {
  padding: 6rem 0;
  position: relative;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1rem;
}

.section-header {
  text-align: center;
  margin-bottom: 4rem;
}

.section-badge {
  display: inline-block;
  background: linear-gradient(135deg, var(--secondary-color), var(--primary-color));
  color: var(--white);
  padding: 0.5rem 1.5rem;
  border-radius: 25px;
  font-size: 0.85rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 1px;
  margin-bottom: 1rem;
}

.section-title {
  font-size: clamp(2rem, 4vw, 3.5rem);
  font-weight: 800;
  color: var(--text-primary);
  margin-bottom: 1rem;
  line-height: 1.2;
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
  width: 8px;
  height: 8px;
  background: var(--secondary-color);
  border-radius: 50%;
}

.divider-line {
  width: 60px;
  height: 2px;
  background: linear-gradient(90deg, transparent, var(--secondary-color), transparent);
}

.divider-icon {
  color: var(--secondary-color);
  font-size: 1.5rem;
}

/* About Section */
.about-section {
  background: linear-gradient(135deg, #f0fdf4 0%, #dcfce7 100%);
}

.about-container {
  max-width: 1100px;
  margin: 0 auto;
}

.about-content {
  display: grid;
  grid-template-columns: 1.2fr 1fr;
  gap: 4rem;
  align-items: center;
}

.about-text {
  padding-right: 2rem;
}

.about-heading {
  font-size: 2rem;
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
}

.text-highlight {
  color: var(--primary-color);
  font-weight: 600;
}

.brand-highlight {
  background: linear-gradient(135deg, var(--secondary-color), var(--primary-color));
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  font-weight: 700;
  font-size: 1.1em;
}

.about-stats {
  display: flex;
  gap: 2rem;
  margin-top: 2rem;
}

.stat-mini {
  text-align: center;
}

.stat-mini .stat-number {
  display: block;
  font-size: 1.8rem;
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
}

.visual-image {
  position: relative;
  height: 400px;
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
}

/* Benefits Section */
.benefits-section {
  background: linear-gradient(135deg, #e6f5ef 0%, #ccf2dd 100%);
}

.benefits-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 2rem;
  max-width: 1000px;
  margin: 0 auto;
}

.benefit-item {
  height: 100%;
}

.benefit-card {
  background: rgba(255, 255, 255, 0.9);
  backdrop-filter: blur(20px);
  border-radius: var(--border-radius-lg);
  padding: 2rem;
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
  background: linear-gradient(90deg, var(--secondary-color), var(--primary-color));
  transform: scaleX(0);
  transition: transform 0.4s ease;
  border-radius: var(--border-radius-lg) var(--border-radius-lg) 0 0;
}

.benefit-card:hover {
  transform: translateY(-8px);
  box-shadow: var(--shadow-heavy);
}

.benefit-card:hover::before {
  transform: scaleX(1);
}

.benefit-icon {
  width: 70px;
  height: 70px;
  background: linear-gradient(135deg, var(--secondary-color), var(--primary-color));
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--white);
  font-size: 1.8rem;
  margin-bottom: 1.5rem;
  transition: var(--transition);
}

.benefit-card:hover .benefit-icon {
  transform: scale(1.1) rotate(5deg);
}

.benefit-content {
  flex: 1;
}

.benefit-title {
  font-size: 1.4rem;
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
  width: 60px;
  height: 60px;
  border-radius: 50%;
  overflow: hidden;
  opacity: 0.1;
  transition: var(--transition);
}

.benefit-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.benefit-card:hover .benefit-image {
  opacity: 0.2;
  transform: scale(1.1);
}

.benefit-number {
  position: absolute;
  bottom: 1rem;
  right: 1rem;
  width: 40px;
  height: 40px;
  background: linear-gradient(135deg, var(--accent-color), #d97706);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--white);
  font-weight: 700;
  font-size: 1.1rem;
  box-shadow: var(--shadow-light);
}

/* Services Section */
.services-section {
  background: linear-gradient(135deg, var(--background-light) 0%, #f1f5f9 100%);
}

.filter-section {
  text-align: center;
  margin-bottom: 3rem;
}

.filter-wrapper {
  display: inline-flex;
  background: rgba(255, 255, 255, 0.8);
  backdrop-filter: blur(20px);
  border-radius: 50px;
  padding: 0.5rem;
  gap: 0.5rem;
  border: 1px solid rgba(0, 0, 0, 0.05);
  box-shadow: var(--shadow-light);
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

.filter-button:hover {
  color: var(--primary-color);
  background: rgba(0, 105, 92, 0.05);
}

.filter-button.active {
  background: linear-gradient(135deg, var(--secondary-color), var(--primary-color));
  color: var(--white);
  box-shadow: var(--shadow-light);
}

.services-container {
  max-width: 900px;
  margin: 0 auto;
}

.services-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(320px, 1fr));
  gap: 2rem;
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
}

.service-card:hover {
  transform: translateY(-5px);
  box-shadow: var(--shadow-heavy);
}

.service-image {
  position: relative;
  height: 250px;
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
}

.service-card:hover .service-overlay {
  opacity: 1;
}

.overlay-content {
  text-align: center;
  color: var(--white);
  transform: translateY(20px);
  transition: var(--transition);
}

.service-card:hover .overlay-content {
  transform: translateY(0);
}

.service-name {
  font-size: 1.5rem;
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
  background: rgba(255, 255, 255, 0.1);
  backdrop-filter: blur(10px);
  border-radius: var(--border-radius);
  padding: 1rem;
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
  backdrop-filter: blur(10px);
}

.service-button:hover {
  background: rgba(255, 255, 255, 0.3);
  color: var(--white);
  transform: translateX(3px);
}

.service-info {
  padding: 1.5rem;
  flex: 1;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.service-title {
  font-size: 1.3rem;
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
  gap: 0.25rem;
  font-size: 0.9rem;
  color: var(--text-secondary);
}

.rating i {
  color: var(--accent-color);
}

.popularity i {
  color: #ef4444;
}

/* CTA Section */
.cta-section {
  background: linear-gradient(135deg, 
    var(--primary-dark) 0%, 
    var(--primary-color) 50%, 
    var(--secondary-color) 100%);
  color: var(--white);
}

.cta-wrapper {
  display: grid;
  grid-template-columns: 1.5fr 1fr;
  gap: 3rem;
  align-items: center;
}

.cta-main {
  background: rgba(255, 255, 255, 0.1);
  backdrop-filter: blur(20px);
  border-radius: var(--border-radius-lg);
  padding: 3rem;
  border: 1px solid rgba(255, 255, 255, 0.2);
}

.cta-header {
  display: flex;
  align-items: flex-start;
  gap: 1.5rem;
  margin-bottom: 2rem;
}

.cta-icon {
  width: 70px;
  height: 70px;
  background: rgba(255, 255, 255, 0.2);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.8rem;
  flex-shrink: 0;
}

.cta-text {
  flex: 1;
}

.cta-title {
  font-size: 2.2rem;
  font-weight: 800;
  margin-bottom: 0.5rem;
  line-height: 1.2;
}

.cta-subtitle {
  font-size: 1.1rem;
  opacity: 0.9;
  line-height: 1.5;
}

.booking-form {
  margin-bottom: 2rem;
}

.form-container {
  display: flex;
  gap: 1rem;
  margin-bottom: 1.5rem;
}

.input-group {
  position: relative;
  flex: 1;
}

.input-icon {
  position: absolute;
  left: 1.25rem;
  top: 50%;
  transform: translateY(-50%);
  color: rgba(255, 255, 255, 0.7);
  z-index: 2;
}

.form-input {
  width: 100%;
  padding: 1rem 1rem 1rem 3rem;
  border: 1px solid rgba(255, 255, 255, 0.3);
  border-radius: 25px;
  background: rgba(255, 255, 255, 0.1);
  backdrop-filter: blur(10px);
  color: var(--white);
  font-size: 1rem;
  transition: var(--transition);
}

.form-input::placeholder {
  color: rgba(255, 255, 255, 0.7);
}

.form-input:focus {
  outline: none;
  border-color: rgba(255, 255, 255, 0.6);
  background: rgba(255, 255, 255, 0.15);
  box-shadow: 0 0 0 3px rgba(255, 255, 255, 0.1);
}

.submit-button {
  background: linear-gradient(135deg, var(--accent-color), #d97706);
  border: none;
  padding: 1rem 2rem;
  border-radius: 25px;
  color: var(--white);
  font-weight: 600;
  cursor: pointer;
  transition: var(--transition);
  display: flex;
  align-items: center;
  gap: 0.5rem;
  white-space: nowrap;
  box-shadow: var(--shadow-medium);
}

.submit-button:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 25px rgba(245, 158, 11, 0.4);
}

.trust-indicators {
  display: flex;
  gap: 2rem;
  flex-wrap: wrap;
}

.trust-item {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 0.9rem;
  opacity: 0.8;
}

.trust-item i {
  color: var(--secondary-light);
}

.cta-side {
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(20px);
  border-radius: var(--border-radius-lg);
  padding: 2.5rem;
  border: 1px solid rgba(255, 255, 255, 0.3);
  color: var(--text-primary);
}

.review-section {
  text-align: center;
}

.review-header {
  margin-bottom: 2rem;
}

.review-icon {
  width: 60px;
  height: 60px;
  background: linear-gradient(135deg, var(--accent-color), #d97706);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--white);
  font-size: 1.5rem;
  margin: 0 auto 1rem;
}

.review-title {
  font-size: 1.4rem;
  font-weight: 700;
  color: var(--text-primary);
  margin-bottom: 0.5rem;
}

.review-subtitle {
  color: var(--text-secondary);
  font-size: 1rem;
  line-height: 1.5;
}

.review-interaction {
  margin-bottom: 2rem;
}

.rating-stars {
  text-decoration: none;
  display: inline-flex;
  gap: 0.25rem;
  margin-bottom: 0.5rem;
  transition: var(--transition);
}

.star {
  font-size: 2rem;
  color: #e5e7eb;
  transition: var(--transition);
  cursor: pointer;
}

.rating-stars:hover .star {
  color: var(--accent-color);
  transform: scale(1.1);
}

.rating-text {
  font-size: 0.9rem;
  color: var(--text-secondary);
  margin: 0;
}

.review-stats {
  display: flex;
  justify-content: space-around;
  gap: 2rem;
}

.review-stat {
  text-align: center;
}

.stat-value {
  display: block;
  font-size: 1.5rem;
  font-weight: 700;
  color: var(--primary-color);
  margin-bottom: 0.25rem;
}

.stat-desc {
  font-size: 0.85rem;
  color: var(--text-secondary);
}

/* Stats Section */
.stats-section {
  background: linear-gradient(135deg, var(--background-dark) 0%, #334155 100%);
  color: var(--white);
}

.stats-header {
  text-align: center;
  margin-bottom: 4rem;
}

.stats-title {
  font-size: clamp(2rem, 4vw, 3rem);
  font-weight: 800;
  color: var(--white);
  margin-bottom: 1rem;
}

.stats-subtitle {
  font-size: 1.2rem;
  opacity: 0.8;
  max-width: 600px;
  margin: 0 auto;
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 2rem;
  max-width: 1000px;
  margin: 0 auto;
}

.stat-card {
  background: rgba(255, 255, 255, 0.1);
  backdrop-filter: blur(20px);
  border-radius: var(--border-radius-lg);
  padding: 2.5rem 2rem;
  border: 1px solid rgba(255, 255, 255, 0.2);
  transition: var(--transition);
  text-align: center;
  position: relative;
  overflow: hidden;
}

.stat-card::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 3px;
  background: linear-gradient(90deg, var(--secondary-color), var(--primary-color));
  transform: scaleX(0);
  transition: transform 0.4s ease;
}

.stat-card:hover {
  transform: translateY(-5px);
  background: rgba(255, 255, 255, 0.15);
}

.stat-card:hover::before {
  transform: scaleX(1);
}

.stat-icon {
  width: 70px;
  height: 70px;
  background: linear-gradient(135deg, var(--secondary-color), var(--primary-color));
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 1.5rem;
  font-size: 1.8rem;
  transition: var(--transition);
}

.stat-card:hover .stat-icon {
  transform: scale(1.1) rotate(5deg);
}

.stat-info {
  position: relative;
  z-index: 2;
}

.stat-number {
  font-size: 3rem;
  font-weight: 800;
  color: var(--secondary-color);
  margin-bottom: 0.5rem;
  line-height: 1;
}

.stat-label {
  font-size: 1.1rem;

  opacity: 0.8;
}

/* Responsive Design */
@media (max-width: 768px) {
  .hero-title {
    font-size: 2.5rem;
  }
  
  .hero-subtitle {
    font-size: 1.2rem;
  }
  
  .section-title {
    font-size: 2rem;
  }
  
  .cta-container {
    grid-template-columns: 1fr;
  }
  
  .form-group {
    flex-direction: column;
  }
  
  .services-grid {
    grid-template-columns: 1fr;
  }
  
  .benefits-grid {
    grid-template-columns: 1fr;
  }
  
  .filter-tabs {
    flex-wrap: wrap;
    gap: 0.25rem;
  }
  
  .filter-tab {
    padding: 0.5rem 1rem;
    font-size: 0.9rem;
  }
  
  .nav-icon {
    width: 50px;
    height: 50px;
    font-size: 1.2rem;
  }
  
  .carousel-control-prev {
    left: 1rem;
  }
  
  .carousel-control-next {
    right: 1rem;
  }
}

@media (max-width: 480px) {
  .hero-content {
    padding: 1rem;
  }
  
  .hero-title {
    font-size: 2rem;
  }
  
  .hero-subtitle {
    font-size: 1rem;
  }
  
  .btn-cta {
    padding: 1rem 2rem;
    font-size: 1rem;
  }
  
  .section-title {
    font-size: 1.5rem;
  }
  
  .about-section, .benefits-section, .services-section, .cta-section, .stats-section {
    padding: 3rem 0;
  }
  
  .cta-main, .cta-side {
    padding: 1.5rem;
  }
  
  .stats-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

/* Animation Classes for AOS */
[data-aos="zoom-in-up"] {
  transform: scale(0.6) translateY(100px);
  opacity: 0;
}

[data-aos="zoom-in-down"] {
  transform: scale(0.6) translateY(-100px);
  opacity: 0;
}
</style>