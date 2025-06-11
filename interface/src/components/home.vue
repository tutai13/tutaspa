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
  <div class="container my-5">
    <div class="text-center">
      <h2 class="text-marron fw-bold mb-3">TUTA SPA - CHĂM SÓC SỨC KHỎE VÀ SẮC ĐẸP TOÀN DIỆN</h2>
      <p class="text-muted fs-5">
        Với đội ngũ chuyên viên chuyên nghiệp và không gian thư giãn, TUTA Spa là nơi lý tưởng để bạn tìm lại sự cân bằng và trẻ hóa toàn diện.
      </p>
    </div>
  </div>

  <!-- Lợi ích -->
  <div class="bg-light py-5">
    <div class="container">
      <div class="row text-center g-4">
        <div class="col-md-4">
          <div class="p-4 rounded-4 shadow-sm bg-white h-100">
            <i class="fa-solid fa-leaf fa-2x text-marron mb-3"></i>
            <h5 class="fw-bold">Liệu trình thiên nhiên</h5>
            <p class="text-muted">Sử dụng sản phẩm từ thiên nhiên, an toàn và hiệu quả.</p>
          </div>
        </div>
        <div class="col-md-4">
          <div class="p-4 rounded-4 shadow-sm bg-white h-100">
            <i class="fa-solid fa-heartbeat fa-2x text-marron mb-3"></i>
            <h5 class="fw-bold">Cải thiện sức khỏe</h5>
            <p class="text-muted">Giúp lưu thông máu, giảm stress và tái tạo năng lượng.</p>
          </div>
        </div>
        <div class="col-md-4">
          <div class="p-4 rounded-4 shadow-sm bg-white h-100">
            <i class="fa-solid fa-user-nurse fa-2x text-marron mb-3"></i>
            <h5 class="fw-bold">Chuyên viên tận tâm</h5>
            <p class="text-muted">Đội ngũ nhân viên được đào tạo bài bản, phục vụ tận tình.</p>
          </div>
        </div>
      </div>
    </div>
  </div>

  <!-- Đặt lịch + Đánh giá -->
  <div class="container my-5">
    <div class="row g-4 align-items-stretch">
      <!-- Đặt lịch -->
      <div class="col-md-8">
        <div class="card text-white rounded-4 shadow h-100" style="background: linear-gradient(135deg, #800000, #a52a2a)">
          <div class="card-body">
            <h5 class="fw-bold mb-2">ĐẶT LỊCH GIỮ CHỖ CHỈ 30 GIÂY</h5>
            <p class="text-white-50 mb-4">Trải nghiệm xong trả tiền, huỷ lịch không sao</p>
            <form class="d-flex flex-column flex-sm-row gap-3">
              <input type="tel" class="form-control form-control-lg rounded-pill px-4" placeholder="Nhập SĐT để đặt lịch" />
              <button
                @click.prevent="goToBooking"
                class="btn btn-warning text-white fw-bold rounded-pill px-4 py-2"
                style="background: linear-gradient(135deg, #800000, #b22222)"
                type="submit"
              >
                ĐẶT LỊCH TƯ VẤN NGAY
              </button>
            </form>
          </div>
        </div>
      </div>

      <!-- Đánh giá -->
      <div class="col-md-4">
        <div class="card shadow-sm border-0 rounded-4 h-100">
          <div class="card-body">
            <h6 class="text-uppercase fw-bold text-marron mb-2">MỜI ANH CHỊ ĐÁNH GIÁ CHẤT LƯỢNG PHỤC VỤ</h6>
            <p class="text-secondary small mb-4">Phản hồi của anh sẽ giúp chúng em cải thiện chất lượng dịch vụ tốt hơn</p>
            <div class="fs-4">
              <i class="fa-regular fa-star text-warning me-1"></i>
              <i class="fa-regular fa-star text-warning me-1"></i>
              <i class="fa-regular fa-star text-warning me-1"></i>
              <i class="fa-regular fa-star text-warning me-1"></i>
              <i class="fa-regular fa-star text-warning"></i>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>

  <!-- Chọn loại dịch vụ -->
  <div class="container text-center my-5">
    <div class="btn-group btn-group-lg flex-wrap shadow-sm rounded-pill" role="group">
      <button
        @click="selectedTypeID = null"
        :class="[
          'btn rounded-pill fw-semibold px-4 py-2',
          selectedTypeID === null ? 'btn-marron' : 'btn-outline-marron',
        ]"
      >
        TẤT CẢ
      </button>
      <button
        v-for="type in serviceTypes"
        :key="type.loaiDichVuID"
        @click="selectedTypeID = type.loaiDichVuID"
        :class="[
          'btn rounded-pill fw-semibold px-4 py-2',
          selectedTypeID === type.loaiDichVuID ? 'btn-marron' : 'btn-outline-marron',
        ]"
      >
        {{ type.tenLoai }}
      </button>
    </div>
  </div>

  <!-- Danh sách dịch vụ -->
  <section class="services py-5 bg-light">
    <div class="container">
      <h2 class="text-center mb-5 fw-bold text-marron text-uppercase">Dịch vụ nổi bật</h2>
      <div class="row g-4 justify-content-center">
        <div
          class="col-md-6 col-lg-4"
          v-for="service in services.filter((s) => selectedTypeID === null || s.loaiDichVuID === selectedTypeID)"
          :key="service.id"
        >
          <div class="card h-100 shadow-sm border-0 rounded-4">
            <router-link :to="`/dichvu/${service.id}`" class="overflow-hidden d-block rounded-top-4">
              <img
                :src="'http://localhost:5055' + service.image"
                class="card-img-top service-img"
                :alt="service.name"
                style="height: 220px; object-fit: cover; transition: transform 0.3s ease;"
              />
            </router-link>
            <div class="card-body">
              <h5 class="card-title fw-bold text-marron">{{ service.name }}</h5>
              <p class="card-text text-muted">{{ service.description }}</p>
            </div>
            <div class="card-footer bg-white border-0 text-center">
              <router-link :to="`/dichvu/${service.id}`" class="btn btn-outline-marron rounded-pill px-4">
                Xem chi tiết...
              </router-link>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup>
import { onMounted, ref } from "vue";
import axios from "axios";
import { useRouter } from "vue-router";

const router = useRouter();
const services = ref([]);
const serviceTypes = ref([]);
const selectedTypeID = ref(null);

onMounted(async () => {
  try {
    const [dichVuRes, loaiDichVuRes] = await Promise.all([
      axios.get("http://localhost:5055/api/DichVu"),
      axios.get("http://localhost:5055/api/LoaiDichVu"),
    ]);

    services.value = dichVuRes.data.map((item) => ({
      id: item.dichVuID,
      name: item.tenDichVu,
      description: item.moTa,
      image: item.hinhAnh || "/images/default-service.jpg",
      loaiDichVuID: item.loaiDichVuID,
    }));

    serviceTypes.value = loaiDichVuRes.data;
    selectedTypeID.value = null;
  } catch (error) {
    console.error("Lỗi khi lấy dữ liệu:", error);
  }
});

function goToBooking() {
  router.push("/dat-lich");
}
</script>

<style scoped>
.text-marron {
  color: #800000;
}
.btn-marron {
  background-color: #800000;
  color: white;
  border-color: #800000;
}
.btn-marron:hover {
  background-color: #a52a2a;
  border-color: #a52a2a;
}
.btn-outline-marron {
  color: #800000;
  border: 1.5px solid #800000;
}
.btn-outline-marron:hover {
  background-color: #800000;
  color: white;
}
.service-img:hover {
  transform: scale(1.05);
}
</style>
