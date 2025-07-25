<template>
  <!-- Carousel full screen width -->
  <div id="bannerCarousel" class="carousel slide rounded-0 overflow-hidden"
       data-bs-ride="carousel"
       style="width: 100vw; margin: 0; padding: 0; position: relative; left: 50%; transform: translateX(-50%);">
    <div class="carousel-inner">
      <div class="carousel-item active">
        <img src="/src/assets/img/BNR1.jpg" class="d-block w-100 banner-img" alt="Banner 1" height="450px" />
      </div>
      <div class="carousel-item">
        <img src="/src/assets/img/BNR2.jpg" class="d-block w-100 banner-img" alt="Banner 2" />
      </div>
      <div class="carousel-item">
        <img src="/src/assets/img/spa2.jpg" class="d-block w-100 banner-img" alt="Banner 3" />
      </div>
      <div class="carousel-item">
        <img src="/src/assets/img/hinh2.jpg" class="d-block w-100 banner-img" alt="Banner 4" />
      </div>
    </div>

    <button class="carousel-control-prev" type="button" data-bs-target="#bannerCarousel" data-bs-slide="prev">
      <span class="carousel-control-prev-icon"></span>
    </button>
    <button class="carousel-control-next" type="button" data-bs-target="#bannerCarousel" data-bs-slide="next">
      <span class="carousel-control-next-icon"></span>
    </button>

    <div class="carousel-indicators">
      <button type="button" data-bs-target="#bannerCarousel" data-bs-slide-to="0" class="active"></button>
      <button type="button" data-bs-target="#bannerCarousel" data-bs-slide-to="1"></button>
      <button type="button" data-bs-target="#bannerCarousel" data-bs-slide-to="2"></button>
      <button type="button" data-bs-target="#bannerCarousel" data-bs-slide-to="3"></button>
    </div>
  </div>

  <!-- Giới thiệu -->
  <div class="container-fluid text-center py-5 bg-white position-relative z-1">
    <div class="mb-3">
      <i class="fas fa-spa fa-3x text-green"></i>
    </div>
    <h2 class="fw-bold text-green mb-3 text-uppercase">ĐÔI NÉT VỀ TUTA SPA</h2>
    <div class="d-flex justify-content-center align-items-center mb-4">
      <div class="border-top border-2 border-success w-25"></div>
      <i class="fas fa-leaf text-green mx-3"></i>
      <div class="border-top border-2 border-success w-25"></div>
    </div>
    <div class="mx-auto px-3 px-md-5 col-lg-8">
      <p class="text-muted fs-5 lh-lg">
        Với đội ngũ kỹ thuật viên <strong>giàu kinh nghiệm</strong>, luôn tiên phong ứng dụng 
        <em>công nghệ hiện đại</em> và phục vụ chuyên nghiệp, 
        <span class="fw-semibold text-green">TUTA Spa</span>
        <u> hứa hẹn đem lại trải nghiệm dịch vụ tốt nhất </u> sự hài lòng của khách hàng
        <strong> là sự thành công </strong> của chúng tôi.
      </p>
    </div>
  </div>

  <!-- Lợi ích nổi bật -->
  <div class="container-fluid py-5 bg-main">
    <div class="row g-4 text-center justify-content-center">
      <div class="col-6 col-md-3" v-for="(benefit, i) in benefits" :key="i">
        <div class="benefit-box p-4 h-100">
          <div class="frame-img mx-auto mb-3">
            <img :src="benefit.image" class="img-fluid rounded-circle border border-2 border-success" />
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
          <h2 class="fw-bold text-title text-uppercase mb-3">Khám phá dịch vụ nổi bật</h2>
          <p class="text-body">Trải nghiệm làm đẹp toàn diện với nhiều lựa chọn phù hợp nhu cầu của bạn.</p>
        </div>
        <div class="col-lg-6 text-lg-end text-center">
          <div class="btn-group flex-wrap" role="group">
            <button @click="selectedTypeID = null" :class="['btn rounded-pill m-1 fw-semibold', selectedTypeID === null ? 'btn-primary text-white' : 'btn-outline-secondary']">
              Tất cả
            </button>
            <button v-for="type in serviceTypes" :key="type.loaiDichVuID" @click="selectedTypeID = type.loaiDichVuID"
                    :class="['btn rounded-pill m-1 fw-semibold', selectedTypeID === type.loaiDichVuID ? 'btn-primary text-white' : 'btn-outline-secondary']">
              {{ type.tenLoai }}
            </button>
          </div>
        </div>
      </div>

      <div class="row g-4">
        <div class="col-md-6 col-lg-4" v-for="service in services.filter(s => !selectedTypeID || s.loaiDichVuID === selectedTypeID)" :key="service.id">
          <div class="card service-card shadow rounded-4 overflow-hidden position-relative bg-card">
            <div class="position-relative">
              <img :src="'http://localhost:5055/images/' + service.image" class="service-img" />
              <h5 class="pre-hover-title text-body fw-bold position-absolute bottom-0 start-0 p-3 m-0 w-100 text-center">
                {{ service.name }}
              </h5>
              <div class="hover-overlay" @mouseenter="loadPricesForService(service.id)">
                <h5 class="hover-title fw-bold text-white mb-3">{{ service.name }}</h5>
                <span class="badge rounded-pill px-3 py-2 mb-2" style="background-color: #34a98c;">KHUYẾN MÃI LÊN ĐẾN 25%</span>
                <div v-if="servicePrices[service.id]" class="text-white small mb-3">
                  <div v-for="gia in servicePrices[service.id]" :key="gia.thoiLuong">
                    {{ gia.thoiLuong }}’: {{ gia.gia.toLocaleString('vi-VN') }}đ
                  </div>
                </div>
                <router-link :to="`/DichVuChiTiet/${service.id}`" class="btn btn-outline-light rounded-pill">Xem chi tiết</router-link>
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
        <div class="card text-white rounded-4 shadow h-100"
             style="background: linear-gradient(135deg, #1e7163, #39a99e);">
          <div class="card-body">
            <h5 class="fw-bold mb-2">ĐẶT LỊCH GIỮ CHỖ CHỈ 30 GIÂY</h5>
            <p class="text-white-50 mb-4">Trải nghiệm xong trả tiền, huỷ lịch không sao</p>
            <form class="d-flex flex-column flex-sm-row gap-3">
              <input type="tel" class="form-control form-control-lg rounded-pill px-4" placeholder="Nhập SĐT để đặt lịch" />
              <button @click.prevent="goToBooking" class="btn btn-primary fw-bold rounded-pill px-4 py-2" type="submit">
                ĐẶT LỊCH TƯ VẤN NGAY
              </button>
            </form>
          </div>
        </div>
      </div>
      <div class="col-md-4">
        <div class="card shadow-sm border-0 rounded-4 h-100 bg-card">
          <div class="card-body">
            <h6 class="text-uppercase fw-bold text-title mb-2">MỜI ANH CHỊ ĐÁNH GIÁ CHẤT LƯỢNG PHỤC VỤ</h6>
            <p class="text-body small mb-4">Phản hồi của anh sẽ giúp chúng em cải thiện chất lượng dịch vụ tốt hơn</p>
            <router-link to="/DanhGia" class="fs-4 text-decoration-none">
              <i class="fa-regular fa-star text-warning me-1" v-for="n in 5" :key="n"></i>
            </router-link>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import axios from 'axios';

const router = useRouter();
const services = ref([]);
const serviceTypes = ref([]);
const selectedTypeID = ref(null);
const servicePrices = ref({});
const benefits = [
  { title: 'Đội ngũ nhân viên', desc: 'Không ngừng học hỏi, trao đổi công nghệ hàng đầu', image: '/src/assets/img/nhanvien.jpg' },
  { title: 'Thương hiệu uy tín tận tâm', desc: 'Được đánh giá cao từ khách hàng', image: '/src/assets/img/logo.png' },
  { title: 'Máy móc hiện đại', desc: 'Đầu tư thiết bị tối tân, hiện đại', image: '/src/assets/img/maymoc.jpg' },
  { title: 'Phục vụ chuyên nghiệp', desc: 'Sự hài lòng là thành công của chúng tôi', image: '/src/assets/img/phucvu.png' }
];

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
</script>

<style scoped>
.text-title { color: #00695c; }
.text-body { color: #2e2e2e; }
.bg-main { background-color: #f4fbf9; }
.bg-card { background-color: #e6f5ef; }

.btn-primary {
  background-color: #04ee69;
  color: #fff;
  border: none;
}
.btn-primary:hover {
  background-color: #00524f;
}

@media (max-width: 400px) {
  .banner-img {
    height: 450px;
  object-fit: cover;
  }
}

.service-img {
  height: 240px;
  object-fit: cover;
  width: 100%;
  background-color: #f8f8f8;
}
.service-card {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  transition: transform 0.3s;
  min-height: 240px;
}
.service-card:hover .service-img {
  transform: scale(1.05);
}
.hover-overlay {
  position: absolute;
  top: 0; left: 0; right: 0; bottom: 0;
  background-color: rgba(0, 105, 92, 0.85);
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  text-align: center;
  padding: 20px;
  transition: opacity 0.3s ease;
  z-index: 10;
  opacity: 0;
}
.service-card:hover .hover-overlay { opacity: 1; }
.service-card:hover .pre-hover-title { opacity: 0; }

.hover-title { font-size: 1.25rem; transition: opacity 0.3s ease; }

.text-green { color: #00695c; }

.benefit-box {
  background-color: #ccf2dd;
  border-radius: 12px;
  border: 1px solid #cde1d8;
  box-shadow: 0 0 10px rgba(0,0,0,0.05);
  transition: transform 0.3s ease;
}
.benefit-box:hover {
  transform: translateY(-5px);
  box-shadow: 0 10px 20px rgba(0,0,0,0.1);
}
.frame-img {
  border: 5px double #f7f7f7;
  border-radius: 70%;
  width: 120px;
  height: 120px;
  overflow: hidden;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: white;
}
</style>
