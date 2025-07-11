<template>
  <div class="container py-5">
    <div v-if="service">
      <!-- Tiêu đề và mô tả -->
      <div class="text-center mb-5">
        <h2 class="fw-bold text-success">{{ service.tenDichVu }}</h2>
        <p class="text-muted fs-5">{{ service.moTa }}</p>
      </div>

      <!-- 3 box thời gian -->
      <div class="row g-4 mb-5" v-if="giaDichVu.length">
        <div class="col-md-4" v-for="(g, i) in giaDichVu" :key="i">
          <div class="card h-100 shadow-sm border-0 rounded-4 overflow-hidden">
            <img :src="getImageUrl(service.hinhAnh)" class="card-img-top" style="height: 220px; object-fit: cover;" />
            <div class="card-body text-center">
              <h5 class="fw-bold text-marron">{{ service.tenDichVu }} - {{ g.thoiLuong }} phút</h5>
              <p class="text-success fs-5 fw-semibold">{{ formatCurrency(g.gia) }}</p>
              <router-link :to="`/dat-lich`" class="btn btn-outline-success rounded-pill">Đặt dịch vụ</router-link>
            </div>
          </div>
        </div>
      </div>

      <!-- Đánh giá trung bình -->
      <div class="mb-4">
        <h5 class="fw-bold">Đánh giá trung bình:</h5>
        <div v-if="trungBinh !== null">
          <i class="fa-solid fa-star text-warning" v-for="n in Math.round(trungBinh)" :key="n"></i>
          <span class="text-muted">{{ trungBinh.toFixed(1) }}/5</span>
        </div>
        <div v-else class="text-muted">Chưa có đánh giá</div>
      </div>

      <!-- Danh sách đánh giá -->
      <div v-if="danhGias.length" class="mt-4">
        <h5 class="fw-bold mb-3">Phản hồi từ khách hàng:</h5>
        <div v-for="dg in danhGias" :key="dg.id" class="border rounded-4 p-3 mb-3 shadow-sm">
          <div class="mb-2">
            <i class="fa-solid fa-star text-warning me-1" v-for="n in dg.sao" :key="n"></i>
            <span class="text-muted small">{{ new Date(dg.ngayTao).toLocaleDateString() }}</span>
          </div>
          <p class="mb-0">{{ dg.noiDung }}</p>
          <div v-if="dg.anDanh" class="text-muted small fst-italic">Ẩn danh</div>
        </div>
      </div>
    </div>

    <div v-else class="text-center text-muted py-5">
      Đang tải thông tin dịch vụ...
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import axios from 'axios';

const route = useRoute();
const id = route.params.id;

const service = ref(null);
const giaDichVu = ref([]);
const danhGias = ref([]);
const trungBinh = ref(null);

onMounted(async () => {
  try {
    const [res1, res2, res3, res4] = await Promise.all([
      axios.get(`http://localhost:5055/api/DichVu/${id}`),
      axios.get(`http://localhost:5055/api/BangGiaDichVu/GetGiaTheoThoiGian/${id}`),
      axios.get(`http://localhost:5055/api/DanhGia/dichvu/${id}`),
      axios.get(`http://localhost:5055/api/DanhGia/trungbinh/${id}`)
    ]);

    service.value = res1.data;
    giaDichVu.value = res2.data;
    danhGias.value = res3.data;
    trungBinh.value = res4.data;
  } catch (err) {
    console.error('Lỗi khi tải dữ liệu dịch vụ:', err);
  }
});

const getImageUrl = (path) => `http://localhost:5055${path}`;
const formatCurrency = (num) =>
  new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(num);
</script>

<style scoped>
.text-marron {
  color: #00796B;
}
</style>
