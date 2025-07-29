<template>
  <!-- Carousel full screen width -->
  <div
    id="bannerCarousel"
    class="carousel slide rounded-0 overflow-hidden"
    data-bs-ride="carousel"
    style="
      width: 100vw;
      margin: 0;
      padding: 0;
      position: relative;
      left: 50%;
      transform: translateX(-50%);
    "
  >
    <div class="carousel-inner">
      <div class="carousel-item active">
        <img
          src="/src/assets/img/BNR1.jpg"
          class="d-block w-100 banner-img"
          alt="Banner 1"
          height="450px"
        />
      </div>
      <div class="carousel-item">
        <img
          src="/src/assets/img/BNR2.jpg"
          class="d-block w-100 banner-img"
          alt="Banner 2"
        />
      </div>
      <div class="carousel-item">
        <img
          src="/src/assets/img/spa2.jpg"
          class="d-block w-100 banner-img"
          alt="Banner 3"
        />
      </div>
      <div class="carousel-item">
        <img
          src="/src/assets/img/hinh2.jpg"
          class="d-block w-100 banner-img"
          alt="Banner 4"
        />
      </div>
    </div>

    <button
      class="carousel-control-prev"
      type="button"
      data-bs-target="#bannerCarousel"
      data-bs-slide="prev"
    >
      <span class="carousel-control-prev-icon"></span>
    </button>
    <button
      class="carousel-control-next"
      type="button"
      data-bs-target="#bannerCarousel"
      data-bs-slide="next"
    >
      <span class="carousel-control-next-icon"></span>
    </button>

    <div class="carousel-indicators">
      <button
        type="button"
        data-bs-target="#bannerCarousel"
        data-bs-slide-to="0"
        class="active"
      ></button>
      <button
        type="button"
        data-bs-target="#bannerCarousel"
        data-bs-slide-to="1"
      ></button>
      <button
        type="button"
        data-bs-target="#bannerCarousel"
        data-bs-slide-to="2"
      ></button>
      <button
        type="button"
        data-bs-target="#bannerCarousel"
        data-bs-slide-to="3"
      ></button>
    </div>
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

  <!-- Lợi ích nổi bật -->
  <div class="container-fluid py-5 bg-main">
    <div class="row g-4 text-center justify-content-center">
      <div class="col-6 col-md-3" v-for="(benefit, i) in benefits" :key="i">
        <div class="benefit-box p-4 h-100">
          <div class="frame-img mx-auto mb-3">
            <img
              :src="benefit.image"
              class="img-fluid rounded-circle border border-2 border-success"
            />
          </div>
          <h5 class="fw-bold text-green">{{ benefit.title }}</h5>
          <p class="small text-muted">{{ benefit.desc }}</p>
        </div>
      </div>
    </div>
  </div>

  <!-- Dịch vụ nổi bật -->
  <section class="py-5 bg-main">
    <div class="container-fluid">
      <div class="row align-items-center mb-5">
        <div class="col-lg-6 text-lg-start text-center">
          <h2 class="fw-bold text-title text-uppercase mb-3">
            Khám phá dịch vụ nổi bật
          </h2>
          <p class="text-body">
            Trải nghiệm làm đẹp toàn diện với nhiều lựa chọn phù hợp nhu cầu của
            bạn.
          </p>
        </div>
        <div class="col-lg-6 text-lg-end text-center">
          <div class="btn-group flex-wrap" role="group">
            <button
              @click="selectedTypeID = null"
              :class="[
                'btn rounded-pill m-1 fw-semibold',
                selectedTypeID === null
                  ? 'btn-primary text-white'
                  : 'btn-outline-secondary',
              ]"
            >
              Tất cả
            </button>
            <button
              v-for="type in serviceTypes"
              :key="type.loaiDichVuID"
              @click="selectedTypeID = type.loaiDichVuID"
              :class="[
                'btn rounded-pill m-1 fw-semibold',
                selectedTypeID === type.loaiDichVuID
                  ? 'btn-primary text-white'
                  : 'btn-outline-secondary',
              ]"
            >
              {{ type.tenLoai }}
            </button>
          </div>
        </div>
      </div>

      <div class="row g-4">
        <div
          class="col-md-6 col-lg-4"
          v-for="service in services.filter(
            (s) => !selectedTypeID || s.loaiDichVuID === selectedTypeID
          )"
          :key="service.id"
        >
          <div
            class="card service-card shadow rounded-4 overflow-hidden position-relative bg-card"
          >
            <div class="position-relative">
              <img
                :src="'http://localhost:5055/images/' + service.image"
                class="service-img"
              />
              <h5
                class="pre-hover-title text-body fw-bold position-absolute bottom-0 start-0 p-3 m-0 w-100 text-center"
              >
                {{ service.name }}
              </h5>
              <div
                class="hover-overlay"
                @mouseenter="loadPricesForService(service.id)"
              >
                <h5 class="hover-title fw-bold text-white mb-3">
                  {{ service.name }}
                </h5>
                <span
                  class="badge rounded-pill px-3 py-2 mb-2"
                  style="background-color: #34a98c"
                  >KHUYẾN MÃI LÊN ĐẾN 25%</span
                >
                <div
                  v-if="servicePrices[service.id]"
                  class="text-white small mb-3"
                >
                  <div
                    v-for="gia in servicePrices[service.id]"
                    :key="gia.thoiLuong"
                  >
                    {{ gia.thoiLuong }}’: {{ gia.gia.toLocaleString("vi-VN") }}đ
                  </div>
                </div>
                <router-link
                  :to="`/DichVuChiTiet/${service.id}`"
                  class="btn btn-outline-light rounded-pill"
                  >Xem chi tiết</router-link
                >
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>

  <!-- Đặt lịch & Đánh giá -->
  <div class="container-fluid my-5">
    <div class="row g-4 align-items-stretch">
      <div class="col-md-8">
        <div
          class="card text-white rounded-4 shadow h-100"
          style="background: linear-gradient(135deg, #1e7163, #39a99e)"
        >
          <div class="card-body">
            <h5 class="fw-bold mb-2">ĐẶT LỊCH GIỮ CHỖ CHỈ 30 GIÂY</h5>
            <p class="text-white-50 mb-4">
              Trải nghiệm xong trả tiền, huỷ lịch không sao
            </p>
            <form class="d-flex flex-column flex-sm-row gap-3">
              <input
                type="tel"
                class="form-control form-control-lg rounded-pill px-4"
                placeholder="Nhập SĐT để đặt lịch"
              />
              <button
                @click.prevent="goToBooking"
                class="btn btn-primary fw-bold rounded-pill px-4 py-2"
                type="submit"
              >
                ĐẶT LỊCH TƯ VẤN NGAY
              </button>
            </form>
          </div>
        </div>
      </div>
      <div class="col-md-4">
        <div class="card shadow-sm border-0 rounded-4 h-100 bg-card">
          <div class="card-body">
            <h6 class="text-uppercase fw-bold text-title mb-2">
              MỜI ANH CHỊ ĐÁNH GIÁ CHẤT LƯỢNG PHỤC VỤ
            </h6>
            <p class="text-body small mb-4">
              Phản hồi của anh sẽ giúp chúng em cải thiện chất lượng dịch vụ tốt
              hơn
            </p>
            <router-link to="/DanhGia" class="fs-4 text-decoration-none">
              <i
                class="fa-regular fa-star text-warning me-1"
                v-for="n in 5"
                :key="n"
              ></i>
            </router-link>
          </div>
        </div>
      </div>
    </div>
  </div>

  <!-- Dịch vụ nổi bật -->
  <section
    class="py-5"
    style="background: linear-gradient(to bottom right, #e6f2ef, #ffffff)"
  >
    <div class="container">
      <div class="row align-items-center mb-5">
        <div class="col-lg-6 text-lg-start text-center">
          <h2 class="fw-bold text-success text-uppercase mb-3">
            Khám phá dịch vụ nổi bật
          </h2>
          <p class="text-muted">
            Trải nghiệm làm đẹp toàn diện với nhiều lựa chọn phù hợp nhu cầu của
            bạn.
          </p>
        </div>
        <div class="col-lg-6 text-lg-end text-center">
          <div class="btn-group flex-wrap" role="group">
            <button
              @click="selectedTypeID = null"
              :class="[
                'btn rounded-pill m-1 fw-semibold',
                selectedTypeID === null
                  ? 'btn-success text-white'
                  : 'btn-outline-success',
              ]"
            >
              Tất cả
            </button>
            <button
              v-for="type in serviceTypes"
              :key="type.loaiDichVuID"
              @click="selectedTypeID = type.loaiDichVuID"
              :class="[
                'btn rounded-pill m-1 fw-semibold',
                selectedTypeID === type.loaiDichVuID
                  ? 'btn-success text-white'
                  : 'btn-outline-success',
              ]"
            >
              {{ type.tenLoai }}
            </button>
          </div>
        </div>
      </div>
      <div class="row g-4">
        <div
          class="col-md-6 col-lg-4"
          v-for="service in services.filter(
            (s) => !selectedTypeID || s.loaiDichVuID === selectedTypeID
          )"
          :key="service.id"
        >
          <div
            class="card service-card shadow rounded-4 overflow-hidden position-relative"
          >
            <div class="position-relative">
              <img
                :src="'http://localhost:5055' + service.image"
                class="w-100 service-img"
                style="height: 240px; object-fit: cover"
              />
              <h5
                class="text-white fw-bold position-absolute bottom-0 start-0 p-3 m-0"
                style="z-index: 1; background: rgba(0, 0, 0, 0.4); width: 100%"
              >
                {{ service.name }}
              </h5>
              <div
                class="hover-overlay"
                @mouseenter="loadPricesForService(service.id)"
              >
                <span class="badge bg-danger mb-2">KHUYẾN MÃI LÊN ĐẾN 25%</span>
                <div
                  v-if="servicePrices[service.id]"
                  class="text-white small mb-3"
                >
                  <div
                    v-for="gia in servicePrices[service.id]"
                    :key="gia.thoiLuong"
                  >
                    {{ gia.thoiLuong }}’: {{ gia.gia.toLocaleString("vi-VN") }}đ
                  </div>
                </div>
                <div class="d-flex gap-2">
                  <router-link
                    :to="`/DichVuChiTiet/${service.id}`"
                    class="btn btn-outline-light rounded-pill"
                    >Xem chi tiết</router-link
                  >
                  <router-link
                    :to="`/dichvu/${service.id}`"
                    class="btn btn-warning text-dark rounded-pill"
                    >Đặt dịch vụ</router-link
                  >
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup>
import { ref, onMounted, onUnmounted, nextTick } from "vue";
import { useRouter } from "vue-router";
import axios from "axios";

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
    title: "Đội ngũ nhân viên",
    desc: "Không ngừng học hỏi, trao đổi công nghệ hàng đầu",
    image: "/src/assets/img/nhanvien.jpg",
  },
  {
    title: "Thương hiệu uy tín tận tâm",
    desc: "Được đánh giá cao từ khách hàng",
    image: "/src/assets/img/logo.png",
  },
  {
    title: "Máy móc hiện đại",
    desc: "Đầu tư thiết bị tối tân, hiện đại",
    image: "/src/assets/img/maymoc.jpg",
  },
  {
    title: "Phục vụ chuyên nghiệp",
    desc: "Sự hài lòng là thành công của chúng tôi",
    image: "/src/assets/img/phucvu.png",
  },
];

const filteredServices = computed(() => {
  return services.value.filter(
    (s) => !selectedTypeID.value || s.loaiDichVuID === selectedTypeID.value
  );
});

onMounted(async () => {
  try {
    const [dvRes, ldvRes] = await Promise.all([
      axios.get("https://localhost:7183/api/DichVu"),
      axios.get("https://localhost:7183/api/LoaiDichVu"),
    ]);
    services.value = dvRes.data.map((s) => ({
      id: s.dichVuID,
      name: s.tenDichVu,
      description: s.moTa,
      image: s.hinhAnh || "default-service.jpg",
      loaiDichVuID: s.loaiDichVuID,
    }));
    serviceTypes.value = ldvRes.data;
  } catch (err) {
    console.error("Lỗi tải dữ liệu:", err);
  }
});

const servicePrices = ref({});

async function loadPricesForService(serviceId) {
  if (!servicePrices.value[serviceId]) {
    try {
      const res = await axios.get(
        `http://localhost:5055/api/BangGiaDichVu/GetGiaTheoThoiGian/${serviceId}`
      );
      servicePrices.value[serviceId] = res.data;
    } catch (err) {
      console.error("Lỗi tải giá:", err);
      servicePrices.value[serviceId] = [];
    }
  }
}

function goToBooking() {
  router.push("/DatLich");
}
</script>

<style scoped>
.text-title {
  color: #00695c;
}
.text-body {
  color: #2e2e2e;
}
.bg-main {
  background-color: #f4fbf9;
}
.bg-card {
  background-color: #e6f5ef;
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

@media (max-width: 400px) {
  .banner-img {
    height: 450px;
    object-fit: cover;
  }
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
.service-img {
  transition: transform 0.3s ease;
}

.service-card:hover .service-img {
  transform: scale(1.05);
}

.hover-overlay {
  position: absolute;
  top: 0;
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
.service-card:hover .hover-overlay {
  opacity: 1;
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
</style>
