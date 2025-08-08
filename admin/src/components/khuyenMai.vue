<template>
  <div class="container mt-4">
<h2 class="voucher-title text-center mb-4">
  <i class="bi bi-ticket-perforated-fill me-2 text-primary"></i>
  Quản Lý Voucher
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
              <label class="form-label">🔑Mã code</label>
              <input
                v-model="voucher.maCode"
                class="form-control"
                required
                @input="removeVietnamese"
              />
            </div>
            <div class="col-md-4">
              <label class="form-label">💸Giá trị giảm</label>
              <input v-model.number="voucher.giaTriGiam" type="number" class="form-control" required min="1" />
            </div>
            <div class="col-md-4">
              <label class="form-label">📉Kiểu giảm giá</label>
              <select v-model="voucher.kieuGiamGia" class="form-select">
                <option :value="0">%</option>
                <option :value="1">VNĐ</option>
              </select>
            </div>

            <div class="col-md-4">
              <label class="form-label">📆Ngày bắt đầu</label>
              <input v-model="voucher.ngayBatDau" type="date" class="form-control" />
            </div>
            <div class="col-md-4">
              <label class="form-label">🕛Ngày kết thúc</label>
              <input v-model="voucher.ngayKetThuc" type="date" class="form-control" />
            </div>

            <div class="col-md-4">
              <label class="form-label">🔢Số lượng</label>
              <input
                v-model.number="voucher.soLuong"
                type="number"
                class="form-control"
                min="1"
                :disabled="voucher.voHan"
              />
            </div>

            <div class="col-md-4 d-flex align-items-end">
              <div class="form-check mt-4">
                <input
                  v-model="voucher.voHan"
                  class="form-check-input"
                  type="checkbox"
                  id="voHanCheckbox"
                />
                <label class="form-check-label" for="voHanCheckbox">
                  <i class="bi bi-star-fill text-warning me-1"></i>⭐ Phiếu Đặc Biệt
                </label>
              </div>
            </div>
          </div>
          <div class="d-flex justify-content-end mt-4">
            <button type="submit" class="btn btn-primary me-2">
              {{ isEditing ? "🔄Cập nhật" : "➕Thêm" }}
            </button>
            <button type="button" class="btn btn-secondary" @click="resetForm">❌Hủy</button>
          </div>
        </form>
      </div>
    </div>

    <!-- Bộ lọc và tìm kiếm -->
    <div class="row mb-4 g-2 align-items-end">
      <div class="col-md-2">
        <label class="form-label fw-bold small text-muted">🔍 Tìm theo mã</label>
        <input v-model="searchCode" @input="applyAllFilters" type="text" class="form-control" placeholder="VD: VIP" />
      </div>

      <div class="col-md-3">
        <label class="form-label fw-bold small text-muted">📊 Giá trị giảm</label>
        <div class="input-group">
          <input v-model.number="minValue" type="number" class="form-control" placeholder="Từ" />
          <input v-model.number="maxValue" type="number" class="form-control" placeholder="Đến" />
        </div>
      </div>

      <div class="col-md-2">
        <label class="form-label fw-bold small text-muted">⚙️ Kiểu giảm</label>
        <select v-model="selectedType" class="form-select">
          <option value="">Tất cả</option>
          <option value="0">%</option>
          <option value="1">$</option>
        </select>
      </div>

      <div class="col-md-2">
        <label class="form-label fw-bold small text-muted">⭐ Loại phiếu</label>
        <select v-model="selectedVoucherType" class="form-select">
          <option value="">Tất cả</option>
          <option value="special">Phiếu đặc biệt</option>
          <option value="normal">Phiếu thường</option>
        </select>
      </div>
      <div class="col-md-3">
        <label class="form-label fw-bold small text-muted invisible">Ẩn</label>
        <button class="btn btn-dark w-100" @click="resetFilters">
          <i class="bi bi-folder-symlink-fill me-1"></i> Hiển thị tất cả Voucher
        </button>
      </div>
    </div>
    <!-- Danh sách voucher -->
    <div class="table-responsive">
  <table class="table table-bordered table-hover text-center align-middle">
    <thead class="table-primary">
      <tr>
        <th>Mã code</th>
        <th>Giá trị</th>
        <th>Kiểu</th>
        <th>Bắt đầu</th>
        <th>Kết thúc</th>
        <th>Số lượng</th>
        <th>Trạng thái</th> <!-- ✅ Di chuyển trạng thái xuống đây -->
        <th>Hành động</th>
      </tr>
    </thead>
    <tbody>
      <tr
        v-for="v in sortedVouchers"
        :key="v.voucherID"
        :class="{ expired: isExpired(v.ngayKetThuc) }">
        <td>{{ v.maCode }}</td>
        <td>{{ v.giaTriGiam }}</td>
        <td>{{ v.kieuGiamGia === 0 ? "%" : "$" }}</td>
        <td>{{ formatDate(v.ngayBatDau) }}</td>
        <td>{{ formatDate(v.ngayKetThuc) }}</td>
        <td>
          <span v-if="Number(v.soLuong) === -1">
            <i class="bi bi-star-fill text-warning">⭐</i>
          </span>
          <span v-else>
            {{ v.soLuong }}
          </span>
        </td>
        <!-- ✅ Trạng thái ở đây -->
<td>
  <span
    :class="{
      'text-muted fw-bold': getVoucherStatus(v.ngayBatDau, v.ngayKetThuc) === 'Chưa bắt đầu',
      'text-success fw-bold': getVoucherStatus(v.ngayBatDau, v.ngayKetThuc) === 'Còn hạn',
      'text-danger fw-bold': getVoucherStatus(v.ngayBatDau, v.ngayKetThuc) === 'Hết hạn'
    }"
  >
    {{ getVoucherStatus(v.ngayBatDau, v.ngayKetThuc) }}
  </span>
</td>
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
import apiClient from "../utils/axiosClient";

const vouchers = ref([]);
const isEditing = ref(false);
const searchCode = ref("");
const minValue = ref();
const maxValue = ref();
const selectedType = ref("");
const selectedVoucherType = ref("");


const getTodayDate = () => {
  const now = new Date();
  const yyyy = now.getFullYear();
  const mm = String(now.getMonth() + 1).padStart(2, "0");
  const dd = String(now.getDate()).padStart(2, "0");
  return `${yyyy}-${mm}-${dd}`;
};
function isExpired(dateString) {
  const today = new Date();
  const endDate = new Date(dateString);
  // Set cả hai về 0h00 để so sánh chỉ theo ngày
  today.setHours(0, 0, 0, 0);
  endDate.setHours(0, 0, 0, 0);
  return endDate < today;
}

const voucher = ref({
  voucherID: 0,
  maCode: "",
  giaTriGiam: 0,
  kieuGiamGia: 0,
  ngayBatDau: getTodayDate(),
  ngayKetThuc: "",
  soLuong: 1,
  voHan: false,
});

const formatDate = (isoDate) => {
  if (!isoDate) return "";
  return isoDate.split("T")[0];
};

const formatDateForApi = (dateStr) => {
  const d = new Date(dateStr);
  return d.toISOString();
};

const fetchVouchers = async () => {
  try {
    const res = await apiClient.get("Vouchers");
    vouchers.value = res;
  } catch (err) {
    console.error("Lỗi fetch voucher:", err);
  }
};

const saveVoucher = async () => {
  const ngayBatDauDate = new Date(voucher.value.ngayBatDau);
  const ngayKetThucDate = new Date(voucher.value.ngayKetThuc);

  if (!voucher.value.ngayBatDau || !voucher.value.ngayKetThuc) {
    alert("❌ Vui lòng nhập đủ ngày bắt đầu và ngày kết thúc.");
    return;
  }

  if (ngayBatDauDate > ngayKetThucDate) {
    alert("❌ Ngày bắt đầu không được lớn hơn ngày kết thúc.");
    return;
  }

  if (!voucher.value.voHan && voucher.value.soLuong < 1) {
    alert("❌ Số lượng phải lớn hơn hoặc bằng 1.");
    return;
  }

  try {
    const payload = {
      ...voucher.value,
      ngayBatDau: formatDateForApi(voucher.value.ngayBatDau),
      ngayKetThuc: formatDateForApi(voucher.value.ngayKetThuc),
      soLuong: voucher.value.voHan ? -1 : voucher.value.soLuong,
    };

   if (isEditing.value) {
      await apiClient.put(`Vouchers/${voucher.value.voucherID}`, payload);
      alert("✔️ Cập nhật voucher thành công!");
    } else {
      await apiClient.post("Vouchers", payload);
      alert("✔️ Thêm voucher thành công!");
    }

    resetForm();
    await fetchVouchers();
  } catch (err) {
    console.error("Lỗi khi lưu voucher:", err.response?.data || err.message);
    alert("Thêm voucher thất bại.");
  }
};

const deleteVoucher = async (id) => {
  const confirmed = confirm("🗑️ Bạn có chắc chắn muốn xóa voucher này?");
  if (!confirmed) return;

  try {
    await apiClient.delete(`Vouchers/${id}`);
    alert("✔️ Xóa voucher thành công!");
    await fetchVouchers();
  } catch (err) {
    console.error("❌ Lỗi xóa voucher:", err.response?.data || err.message);
    alert("❌ Xóa voucher thất bại.");
  }
};

function removeVietnamese(event) {
  const raw = event.target.value
  const noAccent = raw
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '') // bỏ dấu

  event.target.value = noAccent
}

const editVoucher = (v) => {
  voucher.value = {
    ...v,
    ngayBatDau: v.ngayBatDau.split("T")[0],
    ngayKetThuc: v.ngayKetThuc.split("T")[0],
    voHan: v.soLuong === -1,
    soLuong: v.soLuong === -1 ? 1 : v.soLuong,
  };
  isEditing.value = true;
};

const resetForm = () => {
  isEditing.value = false;
  voucher.value = {
    voucherID: 0,
    maCode: "",
    giaTriGiam: "",
    kieuGiamGia: "",
    ngayBatDau: getTodayDate(),
    ngayKetThuc: "",
    soLuong: 1,
    voHan: false,
  };
};
const getVoucherStatus = (startDate, endDate) => {
  const today = new Date();
  today.setHours(0, 0, 0, 0);

  const start = new Date(startDate);
  const end = new Date(endDate);
  start.setHours(0, 0, 0, 0);
  end.setHours(0, 0, 0, 0);

  if (start > today) return "Chưa bắt đầu";
  if (end < today) return "Hết hạn";
  return "Còn hạn";
};

const applyAllFilters = async () => {
  try {
    // Lấy dữ liệu gốc từ server
    const res = await apiClient.get("Vouchers");
    let data = res;

    // 🔍 Lọc theo mã code
    if (searchCode.value.trim()) {
      data = data.filter((v) =>
        v.maCode.toLowerCase().includes(searchCode.value.trim().toLowerCase())
      );
    }
const min = Number(minValue.value);
const max = Number(maxValue.value);

let filteredData = [...data]; // tạo bản sao từ danh sách gốc

// Nếu chỉ nhập "Từ"
if (!isNaN(min) && minValue.value !== "" && (isNaN(max) || maxValue.value === "")) {
  filteredData = filteredData.filter((v) => v.giaTriGiam >= min);
}

// Nếu chỉ nhập "Đến"
if (!isNaN(max) && maxValue.value !== "" && (isNaN(min) || minValue.value === "")) {
  filteredData = filteredData.filter((v) => v.giaTriGiam <= max);
}

// Nếu nhập cả hai
if (!isNaN(min) && minValue.value !== "" && !isNaN(max) && maxValue.value !== "") {
  filteredData = filteredData.filter((v) => v.giaTriGiam >= min && v.giaTriGiam <= max);
}
data = filteredData;

    // ⚙️ Lọc theo kiểu giảm
    if (selectedType.value !== "") {
      data = data.filter((v) => String(v.kieuGiamGia) === selectedType.value);
    }

    // ⭐ Lọc loại phiếu
    if (selectedVoucherType.value === "special") {
      data = data.filter((v) => v.soLuong === -1);
    } else if (selectedVoucherType.value === "normal") {
      data = data.filter((v) => v.soLuong !== -1);
    }

    vouchers.value = data;
  } catch (err) {
    console.error("Lỗi lọc dữ liệu:", err);
  }
};
const resetFilters = () => {
  searchCode.value = "";
  minValue.value = "";
  maxValue.value = ""; // hoặc giới hạn tối đa bạn muốn
  selectedType.value = "";
  selectedVoucherType.value = "";
  fetchVouchers();
};

import { watch } from "vue";

// Tự động áp dụng lọc khi người dùng thay đổi các trường filter
watch(searchCode, applyAllFilters);
watch(minValue, applyAllFilters);
watch(maxValue, applyAllFilters);
watch(selectedType, applyAllFilters);
watch(selectedVoucherType, applyAllFilters);

import { computed } from 'vue';

const sortedVouchers = computed(() => {
  const today = new Date();
  today.setHours(0, 0, 0, 0);

  return [...vouchers.value].sort((a, b) => {
    const aDate = new Date(a.ngayKetThuc);
    const bDate = new Date(b.ngayKetThuc);
    aDate.setHours(0, 0, 0, 0);
    bDate.setHours(0, 0, 0, 0);

    const aExpired = aDate < today;
    const bExpired = bDate < today;

    if (aExpired && !bExpired) return 1;
    if (!aExpired && bExpired) return -1;

    // Nếu cả hai cùng trạng thái thì sắp theo ngày kết thúc tăng dần
    return aDate - bDate;
  });
});

// Các sự kiện thay đổi gọi applyAllFilters
const searchByCode = () => applyAllFilters();
const filterByValue = () => applyAllFilters();
const filterByType = () => applyAllFilters();
const filterByVoucherType = () => applyAllFilters();

onMounted(fetchVouchers);
</script>

<style scoped>
/* .container {
  max-width: 1200px;
} */
 .voucher-title {
  font-size: 3.0rem;
}

.card-header {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 20px 25px;
  justify-content: space-between;
  align-items: center;
}

.expired {
  text-decoration: line-through;
  opacity: 0.4;
  background-color: #c92c2c;
}
/* ========== Tổng thể ========== */
.container {
  max-width: 1200px;
  /* background: linear-gradient(to right, #d03939, #26bc53); */
  background: linear-gradient(to right, #8ba0b5, #8ba0b5);
  padding: 32px;
  border-radius: 20px;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.07);
  font-family: "Poppins", "Segoe UI", sans-serif;
}
.table-responsive {
  border-radius: 12px;
  overflow-x: auto;
}
.table th {
  background: linear-gradient(to right, #7f63f4, #53c0f0); /* Tím - xanh */
  color: #fff;
  font-weight: 600;
  font-size: 14px;
  vertical-align: middle;
  text-transform: uppercase;
  padding: 12px;
}

.table td {
  vertical-align: middle;
  font-size: 14px;
  padding: 10px 12px;
  background-color: #fff;
}

.badge {
  padding: 6px 10px;
  border-radius: 20px;
  font-size: 13px;
  font-weight: 600;
}

.badge-special {
  background: linear-gradient(to right, #ffc107, #ff9800);
  color: #212529;
}

.badge-expired {
  background: #ff4d4f;
  color: #fff;
}

.badge-active {
  background: #198754;
  color: #fff;
}

/* ========== Nút hành động ========== */
.btn {
  border: none;
  padding: 6px 14px;
  font-weight: 500;
  font-size: 14px;
  border-radius: 10px;
  transition: all 0.2s ease-in-out;
}

.btn-warning {
  background: linear-gradient(to right, #ffe259, #ffa751);
  color: #212529;
}
.btn-warning:hover {
  opacity: 0.9;
}

.btn-danger {
  background: linear-gradient(to right, #ff416c, #ff4b2b);
  color: #fff;
}
.btn-danger:hover {
  opacity: 0.9;
}

/* ========== Input, Label, Filter ========== */
/* .form-label {
}

.form-control,
.form-select { */
  
/* } */
.form-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 20px;
  margin-bottom: 25px;
}

/* ========== Icon số lượng đặc biệt ========== */
td .bi-star-fill {
  font-size: 20px;
  color: gold;
  animation: pulse 1.5s infinite;
}

/* Hiệu ứng nhấp nháy ngôi sao */
@keyframes pulse {
  0% { transform: scale(1); opacity: 1; }
  50% { transform: scale(1.2); opacity: 0.8; }
  100% { transform: scale(1); opacity: 1; }
}

/* ========== Responsive Table ========== */
.table-responsive {
  border-radius: 12px;
  overflow-x: auto;
}
</style>