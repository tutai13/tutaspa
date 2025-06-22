<template>
  <div class="container mt-4">
    <h2 class="text-center mb-4">
      <i class="bi bi-ticket-perforated-fill"></i> Quản Lý Voucher
    </h2>

    <!-- Form Thêm mới Voucher -->
    <div class="card shadow-sm mb-4">
      <div class="card-header bg-primary text-white fw-bold">
        <i class="bi bi-plus-circle me-2"></i>Thêm mới Voucher
      </div>
      <div class="card-body">
        <form @submit.prevent="saveVoucher">
          <div class="row g-3">
            <div class="col-md-4">
              <label class="form-label">Mã code</label>
              <input v-model="voucher.maCode" class="form-control" required />
            </div>
            <div class="col-md-4">
              <label class="form-label">Giá trị giảm</label>
              <input v-model.number="voucher.giaTriGiam" type="number" class="form-control" required min="1" />
            </div>
            <div class="col-md-4">
              <label class="form-label">Kiểu giảm giá</label>
              <select v-model="voucher.kieuGiamGia" class="form-select">
                <option :value="0">%</option>
                <option :value="1">VNĐ</option>
              </select>
            </div>
            <div class="col-md-4">
              <label class="form-label">Ngày bắt đầu</label>
              <input v-model="voucher.ngayBatDau" type="date" class="form-control" />
            </div>
            <div class="col-md-4">
              <label class="form-label">Ngày kết thúc</label>
              <input v-model="voucher.ngayKetThuc" type="date" class="form-control" />
            </div>
            <div class="col-md-4">
              <label class="form-label">Số lượng</label>
              <input v-model.number="voucher.soLuong" type="number" class="form-control" />
            </div>
          </div>

          <div class="d-flex justify-content-end mt-4">
            <button type="submit" class="btn btn-primary me-2">
              {{ isEditing ? "Cập nhật" : "Thêm" }}
            </button>
            <button type="button" class="btn btn-secondary" @click="resetForm">Hủy</button>
          </div>
        </form>
      </div>
    </div>

    <!-- Bộ lọc và tìm kiếm -->
    <div class="row mb-4 g-3">
      <div class="col-md-3">
        <label class="form-label fw-bold small text-muted">🔍 Tìm theo mã</label>
        <div class="input-group">
          <input v-model="searchCode" type="text" class="form-control" placeholder="VD: VIP" />
          <button class="btn btn-outline-primary" @click="searchByCode">Tìm</button>
        </div>
      </div>

      <div class="col-md-4">
        <label class="form-label fw-bold small text-muted">📊 Lọc theo giá trị giảm</label>
        <div class="input-group">
          <input v-model.number="minValue" type="number" class="form-control" placeholder="Từ" />
          <input v-model.number="maxValue" type="number" class="form-control" placeholder="Đến" />
          <button class="btn btn-outline-success" @click="filterByValue">Lọc</button>
        </div>
      </div>

      <div class="col-md-2">
        <label class="form-label fw-bold small text-muted">⚙️ Kiểu giảm</label>
        <select v-model="selectedType" class="form-select" @change="filterByType">
          <option value="">Tất cả</option>
          <option value="0">%</option>
          <option value="1">VNĐ</option>
        </select>
      </div>

      <div class="col-md-3 d-flex align-items-end">
        <button class="btn btn-dark w-100" @click="fetchVouchers">
          <i class="bi bi-folder-symlink-fill me-1"></i> Hiển thị tất cả Voucher
        </button>
      </div>
    </div>

    <!-- Danh sách voucher -->
    <div class="table-responsive">
      <table class="table table-bordered table-hover text-center align-middle">
        <thead class="table-primary">
          <tr>
            <th>ID</th>
            <th>Mã code</th>
            <th>Giá trị</th>
            <th>Kiểu</th>
            <th>Bắt đầu</th>
            <th>Kết thúc</th>
            <th>Số lượng</th>
            <th>Hành động</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="v in vouchers" :key="v.voucherID">
            <td>{{ v.voucherID }}</td>
            <td>{{ v.maCode }}</td>
            <td>{{ v.giaTriGiam }}</td>
            <td>{{ v.kieuGiamGia === 0 ? "%" : "VNĐ" }}</td>
            <td>{{ v.ngayBatDau }}</td>
            <td>{{ v.ngayKetThuc }}</td>
            <td>{{ v.soLuong }}</td>
            <td>
              <button class="btn btn-warning btn-sm me-1" @click="editVoucher(v)">Sửa</button>
              <button class="btn btn-danger btn-sm" @click="deleteVoucher(v.voucherID)">Xóa</button>
            </td>
          </tr>
          <tr v-if="vouchers.length === 0">
            <td colspan="8">Không có dữ liệu voucher.</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import apiClient from "../utils/axiosClient"; // axios đã cấu hình baseURL

const vouchers = ref([]);
const isEditing = ref(false);
const searchCode = ref("");
const minValue = ref(0);
const maxValue = ref(100);
const selectedType = ref("");

const voucher = ref({
  voucherID: 0,
  maCode: "",
  giaTriGiam: 0,
  kieuGiamGia: 0,
  ngayBatDau: "",
  ngayKetThuc: "",
  soLuong: 0,
});

const formatDate = (dateStr) => {
  return new Date(dateStr).toLocaleDateString("vi-VN");
};

const formatDateForApi = (dateStr) => {
  return new Date(dateStr).toISOString();
};

const fetchVouchers = async () => {
  try {
    const res = await apiClient.get("Vouchers");
    vouchers.value = res.data;
  } catch (err) {
    console.error("Lỗi fetch voucher:", err);
  }
};

const saveVoucher = async () => {
  try {
    const payload = {
      ...voucher.value,
      ngayBatDau: formatDateForApi(voucher.value.ngayBatDau),
      ngayKetThuc: formatDateForApi(voucher.value.ngayKetThuc),
    };

    if (isEditing.value) {
      await apiClient.put(`Vouchers/${voucher.value.voucherID}`, payload);
    } else {
      await apiClient.post("Vouchers", payload);
    }
    resetForm();
    await fetchVouchers();
  } catch (err) {
    console.error("Lỗi khi lưu voucher:", err.response?.data || err.message);
    alert("Thêm voucher thất bại.");
  }
};

const deleteVoucher = async (id) => {
  try {
    await apiClient.delete(`Vouchers/${id}`);
    await fetchVouchers();
  } catch (err) {
    console.error("Lỗi xóa voucher:", err);
  }
};

const editVoucher = (v) => {
  voucher.value = {
    ...v,
    ngayBatDau: v.ngayBatDau.split("T")[0],
    ngayKetThuc: v.ngayKetThuc.split("T")[0],
  };
  isEditing.value = true;
};

const resetForm = () => {
  isEditing.value = false;
  voucher.value = {
    voucherID: 0,
    maCode: "",
    giaTriGiam: 0,
    kieuGiamGia: 0,
    ngayBatDau: "",
    ngayKetThuc: "",
    soLuong: 0,
  };
};

const searchByCode = async () => {
  try {
    if (!searchCode.value.trim()) return fetchVouchers();
    const res = await apiClient.get(`Vouchers/code?ma=${searchCode.value}`);
    vouchers.value = res.data;
  } catch (err) {
    console.error("Lỗi tìm kiếm mã:", err);
  }
};

const filterByValue = async () => {
  try {
    const res = await apiClient.get(`Vouchers/filter-value?min=${minValue.value}&max=${maxValue.value}`);
    vouchers.value = res.data;
  } catch (err) {
    console.error("Lỗi lọc theo giá trị:", err);
  }
};

const filterByType = async () => {
  try {
    if (selectedType.value === "") {
      fetchVouchers();
    } else {
      const res = await apiClient.get(`Vouchers/filter-type?type=${selectedType.value}`);
      vouchers.value = res.data;
    }
  } catch (err) {
    console.error("Lỗi lọc theo kiểu:", err);
  }
};

onMounted(fetchVouchers);
</script>


<style scoped>
.container {
  max-width: 1200px;
}
</style>
