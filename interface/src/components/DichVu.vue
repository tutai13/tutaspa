<template>
  <div class="container my-5">
    <h2 class="text-primary-green fw-bold mb-5 text-center display-6">Dịch Vụ Spa Cao Cấp</h2>
    <div class="row gx-5">
      <!-- Bộ lọc cố định bên trái -->
      <aside class="col-lg-3 mb-4">
        <div class="sticky-filter p-4 rounded-4 shadow bg-white border">
          <h5 class="fw-bold text-primary-green mb-3">🔍 Tìm kiếm</h5>
          <input
            v-model="searchQuery"
            type="text"
            class="form-control rounded-pill mb-4"
            placeholder="Nhập tên dịch vụ..."
          />

          <h6 class="fw-semibold text-primary-green mb-3">📂 Lọc theo loại</h6>
          <div class="d-flex flex-column gap-2">
            <button
              class="btn btn-sm fw-semibold"
              :class="selectedType === null ? 'btn-primary-green' : 'btn-outline-primary-green'"
              @click="selectedType = null"
            >
              Tất cả
            </button>
            <button
              v-for="type in serviceTypes"
              :key="type.loaiDichVuID"
              class="btn btn-sm fw-semibold text-start"
              :class="selectedType === type.loaiDichVuID ? 'btn-primary-green' : 'btn-outline-primary-green'"
              @click="selectedType = type.loaiDichVuID"
            >
              {{ type.tenLoai }}
            </button>
          </div>
        </div>
      </aside>

      <!-- Danh sách dịch vụ bên phải -->
      <section class="col-lg-9">
        <div
          v-for="type in filteredTypes"
          :key="type.loaiDichVuID"
          class="mb-5"
        >
          <h3 class="text-primary-green fw-bold border-start border-5 ps-3 mb-4 fs-4">
            {{ type.tenLoai }}
          </h3>
          <div class="row g-4">
            <div
              class="col-md-6 col-lg-4"
              v-for="service in filteredServicesByType(type.loaiDichVuID)"
              :key="service.id"
            >
              <div class="card h-100 shadow-sm border-0 rounded-4 hover-shadow overflow-hidden">
                <router-link
                  :to="`/dichvu/${service.id}`"
                  class="d-block"
                >
                  <img
                    :src="'http://localhost:5055' + service.image"
                    class="card-img-top service-img rounded-top-4"
                    :alt="service.name"
                  />
                </router-link>
                <div class="card-body">
                  <h5 class="card-title fw-bold text-primary-green">{{ service.name }}</h5>
                  <p class="card-text text-muted small">{{ service.description }}</p>
                </div>
                <div class="card-footer bg-white border-0 text-center">
                  <router-link
                    :to="`/dichvu/${service.id}`"
                    class="btn btn-outline-primary-green rounded-pill px-4"
                  >
                    Xem chi tiết &raquo;
                  </router-link>
                </div>
              </div>
            </div>
            <div
              v-if="filteredServicesByType(type.loaiDichVuID).length === 0"
              class="text-muted fst-italic text-center"
            >
              Không có dịch vụ phù hợp.
            </div>
          </div>
        </div>
      </section>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import axios from 'axios';

const services = ref([]);
const serviceTypes = ref([]);
const searchQuery = ref('');
const selectedType = ref(null);

onMounted(async () => {
  try {
    const [dichVuRes, loaiDichVuRes] = await Promise.all([
      axios.get('http://localhost:5055/api/DichVu'),
      axios.get('http://localhost:5055/api/LoaiDichVu'),
    ]);

    services.value = dichVuRes.data.map((item) => ({
      id: item.dichVuID,
      name: item.tenDichVu,
      description: item.moTa,
      image: item.hinhAnh || '/images/default-service.jpg',
      loaiDichVuID: item.loaiDichVuID,
    }));

    serviceTypes.value = loaiDichVuRes.data;
  } catch (error) {
    console.error('Lỗi khi lấy dữ liệu:', error);
  }
});

const filteredTypes = computed(() => {
  return serviceTypes.value.filter(
    (type) => filteredServicesByType(type.loaiDichVuID).length > 0
  );
});

const filteredServicesByType = (typeId) => {
  return services.value.filter((s) => {
    const matchType =
      (selectedType.value === null || s.loaiDichVuID === selectedType.value) &&
      s.loaiDichVuID === typeId;

    const matchSearch = s.name.toLowerCase().includes(searchQuery.value.toLowerCase());

    return matchType && matchSearch;
  });
};
</script>

<style scoped>
.text-primary-green {
  color: #007E5A;
}
.btn-primary-green {
  background-color: #007E5A;
  color: white;
  border: none;
}
.btn-outline-primary-green {
  border: 2px solid #007E5A;
  color: #007E5A;
  background-color: white;
}
.btn-outline-primary-green:hover {
  background-color: #007E5A;
  color: white;
}
.service-img {
  height: 220px;
  object-fit: cover;
  transition: transform 0.3s ease;
}
.service-img:hover {
  transform: scale(1.05);
}
.hover-shadow {
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}
.hover-shadow:hover {
  transform: translateY(-5px);
  box-shadow: 0 10px 20px rgba(0, 0, 0, 0.15);
}
.sticky-filter {
  position: sticky;
  top: 90px;
  z-index: 100;
}
</style>
