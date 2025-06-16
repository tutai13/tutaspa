<template>
  <div class="container my-5">
    <h2 class="text-soft-red fw-bold mb-4 text-center">Dịch Vụ Spa Cao Cấp</h2>
    <div class="row">
      <!-- Cột trái: Tìm kiếm & Lọc -->
      <div class="col-md-3 mb-4">
        <div class="p-3 rounded-4 shadow-sm bg-white border border-light">
          <h5 class="fw-bold text-maroon mb-3">Tìm kiếm dịch vụ</h5>
          <input
            v-model="searchQuery"
            type="text"
            class="form-control rounded-pill mb-3"
            placeholder="Nhập tên dịch vụ..."
          />

          <h6 class="fw-semibold text-maroon mb-2">Lọc theo loại</h6>
          <div class="d-flex flex-column gap-2">
            <button
              class="btn btn-sm"
              :class="selectedType === null ? 'btn-maroon' : 'btn-outline-maroon'"
              @click="selectedType = null"
            >
              Tất cả
            </button>
            <button
              v-for="type in serviceTypes"
              :key="type.loaiDichVuID"
              class="btn btn-sm"
              :class="selectedType === type.loaiDichVuID ? 'btn-maroon' : 'btn-outline-maroon'"
              @click="selectedType = type.loaiDichVuID"
            >
              {{ type.tenLoai }}
            </button>
          </div>

          <!-- Gợi ý mở rộng -->
          <!-- <hr>
          <h6 class="fw-semibold mt-4">Lọc nâng cao</h6>
          <div>
            <label class="form-label">Giá tối đa</label>
            <input type="number" class="form-control" v-model="maxPrice">
          </div>
          <div>
            <label class="form-label">Đánh giá</label>
            <select class="form-select" v-model="rating">
              <option value="">Tất cả</option>
              <option v-for="r in 5" :key="r" :value="r">{{ r }} sao trở lên</option>
            </select>
          </div> -->
        </div>
      </div>

      <!-- Cột phải: Danh sách dịch vụ -->
      <div class="col-md-9">
        <div
          v-for="type in filteredTypes"
          :key="type.loaiDichVuID"
          class="mb-5"
        >
          <h3 class="text-maroon fw-bold border-start border-5 ps-3 mb-4">{{ type.tenLoai }}</h3>
          <div class="row g-4">
            <div
              class="col-md-6 col-lg-4"
              v-for="service in filteredServicesByType(type.loaiDichVuID)"
              :key="service.id"
            >
              <div class="card h-100 shadow-sm border-0 rounded-4 hover-shadow">
                <router-link :to="`/dichvu/${service.id}`" class="overflow-hidden d-block rounded-top-4">
                  <img
                    :src="'http://localhost:5055' + service.image"
                    class="card-img-top service-img"
                    :alt="service.name"
                  />
                </router-link>
                <div class="card-body">
                  <h5 class="card-title fw-bold text-maroon">{{ service.name }}</h5>
                  <p class="card-text text-muted">{{ service.description }}</p>
                </div>
                <div class="card-footer bg-white border-0 text-center">
                  <router-link :to="`/dichvu/${service.id}`" class="btn btn-outline-maroon rounded-pill px-4">
                    Xem chi tiết &raquo;
                  </router-link>
                </div>
              </div>
            </div>
            <div v-if="filteredServicesByType(type.loaiDichVuID).length === 0" class="text-muted fst-italic">
              Không có dịch vụ phù hợp.
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script setup>
import { ref, onMounted, computed } from "vue";
import axios from "axios";

const services = ref([]);
const serviceTypes = ref([]);
const searchQuery = ref("");
const selectedType = ref(null);

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
  } catch (error) {
    console.error("Lỗi khi lấy dữ liệu:", error);
  }
});

// Lọc loại có dịch vụ thỏa điều kiện
const filteredTypes = computed(() => {
  return serviceTypes.value.filter(
    (type) =>
      filteredServicesByType(type.loaiDichVuID).length > 0
  );
});

const filteredServicesByType = (typeId) => {
  return services.value.filter(
    (s) =>
      (selectedType.value === null || s.loaiDichVuID === selectedType.value) &&
      s.loaiDichVuID === typeId &&
      s.name.toLowerCase().includes(searchQuery.value.toLowerCase())
  );
};
</script>
<style scoped>
.text-maroon {
  color: #800000;
}
.btn-maroon {
  background-color: #800000;
  color: white;
}
.btn-outline-maroon {
  border: 2px solid #800000;
  color: #800000;
}
.btn-outline-maroon:hover {
  background-color: #800000;
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
.text-soft-red {
  color: #c60c34; /* Màu mận nhạt */
}
</style>
