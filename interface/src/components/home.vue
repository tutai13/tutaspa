<template>
  <!-- Carousel -->
  <div id="bannerCarousel" class="carousel slide my-4 container rounded overflow-hidden" data-bs-ride="carousel">
    <div class="carousel-inner rounded-4 shadow">
      <div class="carousel-item active">
        <img src="/src/assets/img/h6.jpg" class="d-block w-100" alt="Banner 1" />
      </div>
      <div class="carousel-item">
        <img src="/src/assets/img/h1.jpg" class="d-block w-100" alt="Banner 2" />
      </div>
      <div class="carousel-item">
        <img src="/src/assets/img/spa2.jpg" class="d-block w-100" alt="Banner 3" />
      </div>
      <div class="carousel-item">
        <img src="/src/assets/img/hinh2.jpg" class="d-block w-100" alt="Banner 4" />
      </div>
    </div>
    <button class="carousel-control-prev" type="button" data-bs-target="#bannerCarousel" data-bs-slide="prev">
      <span class="carousel-control-prev-icon" aria-hidden="true"></span>
    </button>
    <button class="carousel-control-next" type="button" data-bs-target="#bannerCarousel" data-bs-slide="next">
      <span class="carousel-control-next-icon" aria-hidden="true"></span>
    </button>
    <div class="carousel-indicators">
      <button type="button" data-bs-target="#bannerCarousel" data-bs-slide-to="0" class="active"></button>
      <button type="button" data-bs-target="#bannerCarousel" data-bs-slide-to="1"></button>
      <button type="button" data-bs-target="#bannerCarousel" data-bs-slide-to="2"></button>
      <button type="button" data-bs-target="#bannerCarousel" data-bs-slide-to="3"></button>
    </div>
  </div>

  <!-- Giới thiệu -->
  <div class="container my-5 text-center">
    <h2 class="text-marron fw-bold mb-3">TUTA SPA - CHĂM SÓC SỨC KHỎE VÀ SẮC ĐẸP TOÀN DIỆN</h2>
    <p class="text-muted fs-5">
      Với đội ngũ chuyên viên chuyên nghiệp và không gian thư giãn, TUTA Spa là nơi lý tưởng để bạn tìm lại sự cân bằng và trẻ hóa toàn diện.
    </p>
  </div>

  <!-- Lợi ích -->
  <div class="py-5" style="background-color: #E6F2EF;">
    <div class="container">
      <div class="row text-center g-4">
        <div class="col-md-4" v-for="(benefit, i) in benefits" :key="i">
          <div class="p-4 rounded-4 shadow-sm bg-white h-100">
            <i :class="['fa-solid fa-2x text-marron mb-3', benefit.icon]"></i>
            <h5 class="fw-bold">{{ benefit.title }}</h5>
            <p class="text-muted">{{ benefit.desc }}</p>
          </div>
        </div>
      </div>
    </div>
  </div>

  <!-- Đặt lịch & Đánh giá -->
  <div class="container my-5">
    <div class="row g-4 align-items-stretch">
      <div class="col-md-8">
        <div class="card text-white rounded-4 shadow h-100" style="background: linear-gradient(135deg, #b2dfdb, #4db6ac);">
          <div class="card-body">
            <h5 class="fw-bold mb-2">ĐẶT LỊCH GIỮ CHỖ CHỈ 30 GIÂY</h5>
            <p class="text-white-50 mb-4">Trải nghiệm xong trả tiền, huỷ lịch không sao</p>
            <form class="d-flex flex-column flex-sm-row gap-3">
              <input type="tel" class="form-control form-control-lg rounded-pill px-4" placeholder="Nhập SĐT để đặt lịch" />
              <button @click.prevent="goToBooking" class="btn btn-warning text-white fw-bold rounded-pill px-4 py-2" style="background: linear-gradient(135deg, #800000, #b22222)" type="submit">
                ĐẶT LỊCH TƯ VẤN NGAY
              </button>
            </form>
          </div>
        </div>
      </div>
      <div class="col-md-4">
        <div class="card shadow-sm border-0 rounded-4 h-100">
          <div class="card-body">
            <h6 class="text-uppercase fw-bold text-marron mb-2">MờI ANH CHỊ ĐÁNH GIÁ CHẤT LƯỢNG PHỤC VỤ</h6>
            <p class="text-secondary small mb-4">
              Phản hồi của anh sẽ giúc chúng em cải thiện chất lượng dịch vụ tốt hơn
            </p>
            <router-link to="/DanhGia" class="fs-4 text-decoration-none">
              <i class="fa-regular fa-star text-warning me-1" v-for="n in 5" :key="n"></i>
            </router-link>
          </div>
        </div>
      </div>
    </div>
  </div>

  <!-- Dịch vụ nổi bật -->
  <section class="py-5" style="background: linear-gradient(to bottom right, #E6F2EF, #ffffff);">
    <div class="container">
      <div class="row align-items-center mb-5">
        <div class="col-lg-6 text-lg-start text-center">
          <h2 class="fw-bold text-success text-uppercase mb-3">Khám phá dịch vụ nổi bật</h2>
          <p class="text-muted">Trải nghiệm làm đẹp toàn diện với nhiều lựa chọn phù hợp nhu cầu của bạn.</p>
        </div>
        <div class="col-lg-6 text-lg-end text-center">
          <div class="btn-group flex-wrap" role="group">
            <button @click="selectedTypeID = null" :class="['btn rounded-pill m-1 fw-semibold', selectedTypeID === null ? 'btn-success text-white' : 'btn-outline-success']">
              Tất cả
            </button>
            <button v-for="type in serviceTypes" :key="type.loaiDichVuID" @click="selectedTypeID = type.loaiDichVuID" :class="['btn rounded-pill m-1 fw-semibold', selectedTypeID === type.loaiDichVuID ? 'btn-success text-white' : 'btn-outline-success']">
              {{ type.tenLoai }}
            </button>
          </div>
        </div>
      </div>
      <div class="row g-4">
        <div class="col-md-6 col-lg-4" v-for="service in services.filter(s => !selectedTypeID || s.loaiDichVuID === selectedTypeID)" :key="service.id">
          <div class="card service-card shadow rounded-4 overflow-hidden position-relative">
            <div class="position-relative">
              <img :src="'http://localhost:5055/images/' + service.image" class="w-100 service-img" style="height: 240px; object-fit: cover;" />
              <h5 class="text-white fw-bold position-absolute bottom-0 start-0 p-3 m-0" style="z-index:1; background: rgba(0,0,0,0.4); width: 100%;">{{ service.name }}</h5>
              <div
  class="hover-overlay"
  @mouseenter="loadPricesForService(service.id)"
>
  <span class="badge bg-danger mb-2">KHUYẾN MÃI LÊN ĐẾN 25%</span>
  <div v-if="servicePrices[service.id]" class="text-white small mb-3">
    <div v-for="gia in servicePrices[service.id]" :key="gia.thoiLuong">
      {{ gia.thoiLuong }}’: {{ gia.gia.toLocaleString('vi-VN') }}đ
    </div>
  </div>
  <div class="d-flex gap-2">
    <router-link :to="`/DichVuChiTiet/${service.id}`" class="btn btn-outline-light rounded-pill">Xem chi tiết</router-link>
    <router-link :to="`/dichvu/${service.id}`" class="btn btn-warning text-dark rounded-pill">Đặt dịch vụ</router-link>
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
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import axios from 'axios';

const router = useRouter();
const services = ref([]);
const serviceTypes = ref([]);
const selectedTypeID = ref(null);
const benefits = [
  { icon: 'fa-leaf', title: 'Liệu trình thiên nhiên', desc: 'Sử dụng sản phẩm từ thiên nhiên, an toàn và hiệu quả.' },
  { icon: 'fa-heartbeat', title: 'Cải thiện sức khỏe', desc: 'Giúc lưu thông máu, giảm stress và tái tạo năng lượng.' },
  { icon: 'fa-user-nurse', title: 'Chuyên viên tận tâm', desc: 'Nhân viên được đào tạo bài bản, phục vụ tận tình.' }
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
      image: s.hinhAnh|| "default-service.jpg",
      loaiDichVuID: s.loaiDichVuID
    }));
    serviceTypes.value = ldvRes.data;
  } catch (err) {
    console.error('Lỗi tải dữ liệu:', err);
  }
});

const servicePrices = ref({});

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
  router.push('/dat-lich');
}
</script>

<style scoped>
.text-marron {
  color: #00796B;
}
.service-card {
  position: relative;
  overflow: hidden;
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
  top: 0; left: 0; right: 0; bottom: 0;
  background-color: rgba(0, 0, 0, 0.7);
  opacity: 0;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  text-align: center;
  padding: 20px;
  transition: opacity 0.3s ease;
  z-index: 10;
}
.service-card:hover .hover-overlay {
  opacity: 1;
}
</style>
