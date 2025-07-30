<template>
  <section class="py-5 bg-light min-vh-100">
    <div class="container">
      <div class="text-center mb-5">
        <h2 class="fw-bold text-title text-uppercase mb-3">Dịch vụ tại TuTa Spa</h2>
        <p class="text-muted fs-5">Khám phá dịch vụ đa dạng và thư giãn tại không gian đẳng cấp của chúng tôi</p>
      </div>

      <!-- Ưu đãi nổi bật -->
      <!-- <div class="alert alert-success text-center rounded-4 fw-bold shadow-sm mb-5">
        🎁 Giảm 30% dịch vụ massage toàn thân – Chỉ hôm nay!
      </div> -->

      <div class="row g-4">
        <!-- Bộ lọc bên trái -->
        <div class="col-lg-3">
          <div class="position-sticky sticky-top shadow-sm p-4 rounded-4 bg-white" style="top: 100px">
            <h5 class="fw-semibold text-green mb-3">Bộ lọc dịch vụ</h5>
            <input v-model="searchKeyword" type="text" class="form-control rounded-pill px-4 mb-3" placeholder="Tìm kiếm dịch vụ...">

            <div class="mb-3">
              <h6 class="fw-bold mb-2 text-dark">Loại dịch vụ</h6>
              <button @click="selectedTypeID = null" :class="['btn w-100 mb-2 rounded-pill fw-semibold', selectedTypeID === null ? 'btn-green text-white' : 'btn-outline-secondary']">
                <i class="fa-solid fa-layer-group me-2"></i>Tất cả
              </button>
              <button v-for="type in serviceTypes" :key="type.loaiDichVuID" @click="selectedTypeID = type.loaiDichVuID" :class="['btn w-100 mb-2 rounded-pill fw-semibold', selectedTypeID === type.loaiDichVuID ? 'btn-green text-white' : 'btn-outline-secondary']">
                <i class="fa-solid fa-leaf me-2 text-success"></i>{{ type.tenLoai }}
              </button>
            </div>

            <div>
              <h6 class="fw-bold mb-2 text-dark">Khoảng giá</h6>
              <input type="range" v-model="priceRange" min="0" max="2000000" step="50000" class="form-range">
              <div class="small text-muted">Tối đa: {{ priceRange.toLocaleString('vi-VN') }}đ</div>
            </div>
          </div>
        </div>

        <!-- Danh sách dịch vụ -->
        <div class="col-lg-9">
          <transition-group name="fade" tag="div" class="row g-4">
            <div class="col-md-6 col-lg-4" v-for="service in filteredServices" :key="service.id">
              <div class="card service-card shadow-sm rounded-4 overflow-hidden bg-white">
                <div class="position-relative">
                  <img :src="'http://localhost:5055/images/' + service.image" class="w-100 service-img rounded-top" style="height: 240px; object-fit: cover;" />
                  <h5 class="pre-hover-title text-dark fw-bold position-absolute bottom-0 start-0 p-3 m-0 w-100 text-center bg-light bg-opacity-75">
                    {{ service.name }}
                  </h5>
                  <div class="hover-overlay" @mouseenter="loadPricesForService(service.id)">
                    <h5 class="hover-title fw-bold text-dark mb-2">{{ service.name }}</h5>
                    <span class="badge bg-danger mb-2">KHUYẾN MÃI LÊN ĐẾN 25%</span>
                    <!--  -->
                    <div v-if="servicePrices[service.id]" class="text-dark small mb-3">
                      <div v-for="gia in servicePrices[service.id]" :key="gia.thoiLuong">
                        {{ gia.thoiLuong }}’: {{ gia.gia.toLocaleString('vi-VN') }}đ
                      </div>
                    </div>
                    <router-link :to="`/DichVuChiTiet/${service.id}`" class="btn btn-outline-success rounded-pill">Xem chi tiết</router-link>
                  </div>
                </div>
              </div>
            </div>
          </transition-group>
          <div v-if="filteredServices.length === 0" class="text-center py-5">
            <p class="text-muted">Không tìm thấy dịch vụ phù hợp</p>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import axios from 'axios';

const services = ref([]);
const serviceTypes = ref([]);
const selectedTypeID = ref(null);
const searchKeyword = ref('');
const priceRange = ref(2000000);
const servicePrices = ref({});

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

const filteredServices = computed(() => {
  return services.value.filter(s => {
    const matchType = !selectedTypeID.value || s.loaiDichVuID === selectedTypeID.value;
    const matchKeyword = s.name.toLowerCase().includes(searchKeyword.value.toLowerCase());
    const priceList = servicePrices.value[s.id] || [];
    const matchPrice = priceList.some(p => p.gia <= priceRange.value) || priceList.length === 0;
    return matchType && matchKeyword && matchPrice;
  });
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
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Playfair+Display:wght@500;700&display=swap');

.text-title {
  color: #2f3e46;
  font-family: 'Playfair Display', serif;
}
.text-muted {
  color: #6c757d;
}
.text-green {
  color: #007E5A;
}
.bg-light {
  background: linear-gradient(90deg, #a0e5a8 0%, #fff8e1 100%) !important;
}


.bg-card {
  background-color: #ffffff;
}

.btn-green {
  background-color: #007E5A;
  color: white;
}
.btn-green:hover {
  background-color: #099c6a;
}

.service-card {
  transition: transform 0.3s;
  font-family: 'Playfair Display', serif;
}
.service-card:hover {
  transform: translateY(-5px);
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
  background-color: rgba(248, 240, 226, 0.92);
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  text-align: center;
  padding: 20px;
  transition: opacity 0.3s ease;
  opacity: 0;
  z-index: 10;
}
.service-card:hover .hover-overlay {
  opacity: 1;
}
.service-card:hover .pre-hover-title {
  opacity: 0;
}
.hover-title {
  font-size: 1.25rem;
  transition: opacity 0.3s ease;
  font-family: 'Playfair Display', serif;
}

.fade-enter-active,
.fade-leave-active {
  transition: all 0.3s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
  transform: translateY(10px);
}
</style>
