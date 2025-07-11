<template>
  <div class="container my-5">
    <h2 class="text-center text-success fw-bold mb-4">Đánh giá dịch vụ</h2>

    <div v-if="!coHoaDon" class="alert alert-warning text-center">
      Bạn cần sử dụng dịch vụ trước khi đánh giá.
    </div>

    <form
      v-if="coHoaDon"
      @submit.prevent="submitDanhGia"
      class="shadow p-4 rounded-4 bg-light"
    >
      <div class="mb-3">
        <label class="form-label">Chọn dịch vụ</label>
        <select class="form-select" v-model="danhGia.maDichVu" required>
          <option v-for="d in dichVus" :key="d.dichVuID" :value="d.dichVuID">
            {{ d.tenDichVu }}
          </option>
        </select>
      </div>

      <div class="mb-3">
        <label class="form-label">Số sao</label>
        <select class="form-select" v-model="danhGia.soSao" required>
          <option v-for="i in 5" :value="i" :key="i">{{ i }} sao</option>
        </select>
      </div>

      <div class="mb-3">
        <label class="form-label">Nội dung đánh giá</label>
        <textarea
          class="form-control"
          rows="4"
          v-model="danhGia.noiDung"
          placeholder="Nhập phản hồi của bạn"
        ></textarea>
      </div>

      <button class="btn btn-success w-100 rounded-pill fw-bold" type="submit">
        Gửi đánh giá
      </button>
    </form>

    <!-- ✅ Danh sách đánh giá hiển thị dưới form -->
    <div class="mt-5">
      <h4 class="fw-bold mb-3">Đánh giá gần đây</h4>
      <div
        v-for="danhGia in danhSach"
        :key="danhGia.id"
        class="border-bottom pb-3 mb-3"
      >
        <div class="fw-bold">{{ danhGia.soSao }} ⭐</div>
        <p>{{ danhGia.noiDung }}</p>
        <p class="text-muted">
          Người đánh giá:
          {{ danhGia.anDanh ? "Ẩn danh" : danhGia.user?.ten }}
        </p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from "vue";
import axios from "axios";

const danhGia = ref({
  maDichVu: "",
  soSao: 5,
  noiDung: "",
});

const dichVus = ref([]);
const coHoaDon = ref(false);
const danhSach = ref([]);
const userId = localStorage.getItem("userId");

onMounted(async () => {
  const res = await axios.get("http://localhost:5055/api/DichVu");
  dichVus.value = res.data;

  const hoaDonRes = await axios.get(
    `http://localhost:5055/api/HoaDon/by-user/${userId}`
  );
  coHoaDon.value = hoaDonRes.data?.some((hd) => hd.trangThai === 1);

  // Nếu đã chọn sẵn dịch vụ, load đánh giá
  if (danhGia.value.maDichVu) {
    await loadDanhGia(danhGia.value.maDichVu);
  }
});

// Khi chọn dịch vụ, load lại danh sách đánh giá
watch(
  () => danhGia.value.maDichVu,
  async (newVal) => {
    if (newVal) {
      await loadDanhGia(newVal);
    }
  }
);

async function loadDanhGia(maDichVu) {
  try {
    const res = await axios.get(
      `http://localhost:5055/api/DanhGia/dichvu/${maDichVu}`
    );
    danhSach.value = res.data;
  } catch (err) {
    console.error("Lỗi khi tải đánh giá", err);
  }
}

async function submitDanhGia() {
  try {
    await axios.post("http://localhost:5055/api/DanhGia", {
      ...danhGia.value,
      userId: userId,
    });
    alert("Gửi đánh giá thành công!");
    danhGia.value = { maDichVu: "", soSao: 5, noiDung: "" };
    await loadDanhGia(danhGia.value.maDichVu);
  } catch (err) {
    console.error(err);
    alert("Đánh giá thất bại.");
  }
}
</script>
