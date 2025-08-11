<template>
  <div class="container my-5">
    <h2 class="text-center fw-bold text-primary mb-4">
      🧾 Lịch Sử Sử Dụng Dịch Vụ
    </h2>

    <div v-if="lichSu.length === 0" class="text-muted text-center">
      Bạn chưa sử dụng dịch vụ nào.
    </div>

    <div
      v-for="hoaDon in lichSu"
      :key="hoaDon.hoaDonID"
      class="card mb-4 shadow-sm"
    >
      <div class="card-header bg-light">
        🗓️ Ngày: {{ formatDate(hoaDon.ngayTao) }} - 💳
        {{ hoaDon.hinhThucThanhToan }}
      </div>
      <div class="card-body">
        <ul class="list-group list-group-flush">
          <li
            v-for="ct in hoaDon.chiTietHoaDons"
            :key="ct.chiTietHoaDonID"
            class="list-group-item"
          >
            <strong>{{ ct.dichVu.tenDichVu }}</strong
            ><br />
            Số lượng: {{ ct.soLuongSP }}<br />
            Giá: {{ formatCurrency(ct.dichVu.gia) }}<br />
            Thành tiền: {{ formatCurrency(ct.thanhTien) }}<br />

            <!-- ✅ Nút đánh giá có truyền ID -->
            <router-link
              :to="`/DanhGia/${ct.dichVu.dichVuID}`"
              class="btn btn-outline-primary btn-sm mt-2 d-inline-flex align-items-center gap-1"
            >
              <i class="fa-regular fa-calendar-check"></i> Đánh Giá
            </router-link>
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import apiClient from "../utils/axiosClient";

const lichSu = ref([]);

onMounted(() => {
  loadLichSu();
});

const loadLichSu = async () => {
  try {
    const res = await apiClient.get("/ThanhToan/by-user");
    lichSu.value = res;
  } catch (err) {
    console.error("❌ Lỗi tải lịch sử dịch vụ:", err);
  }
};

const formatDate = (str) => {
  const d = new Date(str);
  return d.toLocaleDateString("vi-VN", {
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
    hour: "2-digit",
    minute: "2-digit",
  });
};

const formatCurrency = (value) =>
  new Intl.NumberFormat("vi-VN", {
    style: "currency",
    currency: "VND",
  }).format(value);
</script>

<style scoped>
.card {
  border-radius: 15px;
}
.card-header {
  font-weight: 500;
  font-size: 1.1rem;
}
</style>
