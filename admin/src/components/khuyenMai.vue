<template>
  <div class="voucher-management">
    <!-- Header -->
    <div class="header">
      <h1 class="title">
        <i class="bi bi-ticket-perforated-fill"></i>
        Quản Lý Voucher
      </h1>
    </div>

    <!-- Form Thêm mới Voucher -->
    <div class="card">
      <div class="card-header">
        <h2 class="section-title">
          <i class="bi bi-plus-circle"></i>
          {{ isEditing ? "Cập nhật Voucher" : "Thêm mới Voucher" }}
        </h2>
      </div>
      <div class="card-body">
        <form @submit.prevent="saveVoucher" class="voucher-form">
          <div class="form-grid">
            <div class="form-group">
              <label class="form-label">
                Mã code <span class="required">*</span>
              </label>
              <input 
                v-model="voucher.maCode" 
                class="form-control" 
                placeholder="Nhập mã voucher (VD: SALE20)"
                required 
              />
            </div>
            
            <div class="form-group">
              <label class="form-label">
                Giá trị giảm <span class="required">*</span>
              </label>
              <input 
                v-model.number="voucher.giaTriGiam" 
                type="number" 
                class="form-control" 
                placeholder="Nhập giá trị"
                required 
                min="1" 
              />
            </div>
            
            <div class="form-group">
              <label class="form-label">
                Kiểu giảm giá <span class="required">*</span>
              </label>
              <select v-model="voucher.kieuGiamGia" class="form-control">
                <option :value="0">Phần trăm (%)</option>
                <option :value="1">Tiền mặt (VNĐ)</option>
              </select>
            </div>
            
            <div class="form-group">
              <label class="form-label">
                Ngày bắt đầu <span class="required">*</span>
              </label>
              <input 
                v-model="voucher.ngayBatDau" 
                type="date" 
                class="form-control" 
                required
              />
            </div>
            
            <div class="form-group">
              <label class="form-label">
                Ngày kết thúc <span class="required">*</span>
              </label>
              <input 
                v-model="voucher.ngayKetThuc" 
                type="date" 
                class="form-control" 
                required
              />
            </div>
            
            <div class="form-group">
              <label class="form-label">
                Số lượng <span class="required">*</span>
              </label>
              <input 
                v-model.number="voucher.soLuong" 
                type="number" 
                class="form-control" 
                placeholder="Nhập số lượng"
                min="1"
                required
              />
            </div>
          </div>

          <div class="form-actions">
            <button type="submit" class="btn btn-primary">
              <i class="bi bi-check-circle"></i>
              {{ isEditing ? "Cập nhật" : "Thêm mới" }}
            </button>
            <button type="button" class="btn btn-secondary" @click="resetForm">
              <i class="bi bi-x-circle"></i>
              Hủy bỏ
            </button>
          </div>
        </form>
      </div>
    </div>

    <!-- Bộ lọc và tìm kiếm -->
    <div class="card">
      <div class="card-header">
        <h2 class="section-title">
          <i class="bi bi-funnel"></i>
          Tìm kiếm & Lọc dữ liệu
        </h2>
        <div class="list-controls">
          <button class="btn btn-outline-primary" @click="fetchVouchers">
            <i class="bi bi-arrow-clockwise"></i>
            Làm mới
          </button>
        </div>
      </div>
      <div class="card-body">
        <div class="form-grid">
          <!-- Tìm kiếm theo mã -->
          <div class="form-group">
            <label class="form-label">
              <i class="bi bi-search"></i> Tìm theo mã code
            </label>
            <div class="search-container">
              <input 
                v-model="searchCode" 
                type="text" 
                class="search-input" 
                placeholder="Nhập mã voucher cần tìm..."
                @keyup.enter="searchByCode"
              />
              <i class="bi bi-search search-icon"></i>
              <button 
                v-if="searchCode" 
                class="clear-search show" 
                @click="clearSearch"
                type="button"
              >
                <i class="bi bi-x"></i>
              </button>
            </div>
          </div>

          <!-- Lọc theo giá trị -->
          <div class="form-group">
            <label class="form-label">
              <i class="bi bi-currency-dollar"></i> Lọc theo giá trị giảm
            </label>
            <div style="display: flex; gap: 10px;">
              <input 
                v-model.number="minValue" 
                type="number" 
                class="form-control" 
                placeholder="Từ"
                style="flex: 1;"
              />
              <input 
                v-model.number="maxValue" 
                type="number" 
                class="form-control" 
                placeholder="Đến"
                style="flex: 1;"
              />
            </div>
          </div>

          <!-- Lọc theo kiểu giảm giá -->
          <div class="form-group">
            <label class="form-label">
              <i class="bi bi-gear"></i> Kiểu giảm giá
            </label>
            <select v-model="selectedType" class="form-control" @change="filterByType">
              <option value="">Tất cả loại</option>
              <option value="0">Phần trăm (%)</option>
              <option value="1">Tiền mặt (VNĐ)</option>
            </select>
          </div>
        </div>

        <div class="form-actions">
          <button class="btn btn-primary" @click="searchByCode">
            <i class="bi bi-search"></i>
            Tìm kiếm
          </button>
          <button class="btn btn-success" @click="filterByValue">
            <i class="bi bi-funnel-fill"></i>
            Lọc theo giá trị
          </button>
          <button class="btn btn-outline-primary" @click="resetFilters">
            <i class="bi bi-arrow-counterclockwise"></i>
            Đặt lại bộ lọc
          </button>
        </div>
      </div>
    </div>

    <!-- Danh sách voucher -->
    <div class="card">
      <div class="card-header">
        <h2 class="section-title">
          <i class="bi bi-list-ul"></i>
          Danh sách Voucher
          <span class="badge">{{ vouchers.length }}</span>
        </h2>
      </div>
      <div class="card-body">
        <div v-if="loading" class="loading-state">
          <i class="bi bi-arrow-repeat"></i>
          Đang tải dữ liệu...
        </div>
        
        <div v-else-if="vouchers.length === 0" class="empty-state">
          <i class="bi bi-inbox"></i>
          <p>Không có voucher nào được tìm thấy</p>
          <button class="btn btn-primary" @click="fetchVouchers">
            <i class="bi bi-arrow-clockwise"></i>
            Tải lại dữ liệu
          </button>
        </div>

        <div v-else class="table-responsive">
          <table class="vouchers-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Mã Voucher</th>
                <th>Giá trị giảm</th>
                <th>Loại</th>
                <th>Ngày bắt đầu</th>
                <th>Ngày kết thúc</th>
                <th>Số lượng</th>
                <th>Trạng thái</th>
                <th>Hành động</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="v in vouchers" :key="v.voucherID" class="voucher-row">
                <td>
                  <span class="voucher-id">#{{ v.voucherID }}</span>
                </td>
                <td>
                  <div class="code-container">
                    <i class="bi bi-ticket-perforated"></i>
                    <strong>{{ v.maCode }}</strong>
                  </div>
                </td>
                <td>
                  <span class="value-display">
                    {{ formatValue(v.giaTriGiam, v.kieuGiamGia) }}
                  </span>
                </td>
                <td>
                  <span :class="['type-badge', v.kieuGiamGia === 0 ? 'percentage' : 'currency']">
                    {{ v.kieuGiamGia === 0 ? 'Phần trăm' : 'Tiền mặt' }}
                  </span>
                </td>
                <td>{{ formatDate(v.ngayBatDau) }}</td>
                <td>{{ formatDate(v.ngayKetThuc) }}</td>
                <td>
                  <span class="quantity-badge">{{ v.soLuong }}</span>
                </td>
                <td>
                  <span :class="['status-badge', getVoucherStatus(v)]">
                    {{ getVoucherStatusText(v) }}
                  </span>
                </td>
                <td>
                  <div class="action-buttons">
                    <button 
                      class="btn btn-warning btn-sm" 
                      @click="editVoucher(v)"
                      title="Chỉnh sửa"
                    >
                      <i class="bi bi-pencil">Sửa</i>
                    </button>
                    <button 
                      class="btn btn-danger btn-sm" 
                      @click="deleteVoucher(v.voucherID)"
                      title="Xóa"
                    >
                      <i class="bi bi-trash">Xóa</i>
                    </button>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import apiClient from "../utils/axiosClient";

const vouchers = ref([]);
const loading = ref(false);
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

const formatValue = (value, type) => {
  if (type === 0) {
    return `${value}%`;
  } else {
    return new Intl.NumberFormat('vi-VN', {
      style: 'currency',
      currency: 'VND'
    }).format(value);
  }
};

const getVoucherStatus = (voucher) => {
  const now = new Date();
  const startDate = new Date(voucher.ngayBatDau);
  const endDate = new Date(voucher.ngayKetThuc);
  
  if (now < startDate) return 'upcoming';
  if (now > endDate) return 'expired';
  if (voucher.soLuong <= 0) return 'outofstock';
  return 'active';
};

const getVoucherStatusText = (voucher) => {
  const status = getVoucherStatus(voucher);
  const statusTexts = {
    upcoming: 'Sắp diễn ra',
    active: 'Đang hoạt động',
    expired: 'Đã hết hạn',
    outofstock: 'Hết số lượng'
  };
  return statusTexts[status];
};

const fetchVouchers = async () => {
  loading.value = true;
  try {
    const res = await apiClient.get("Vouchers");
    vouchers.value = res;
  } catch (err) {
    console.error("Lỗi fetch voucher:", err);
  } finally {
    loading.value = false;
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
    alert("Thao tác thất bại. Vui lòng thử lại.");
  }
};

const deleteVoucher = async (id) => {
  if (!confirm("Bạn có chắc chắn muốn xóa voucher này?")) return;
  
  try {
    await apiClient.delete(`Vouchers/${id}`);
    await fetchVouchers();
  } catch (err) {
    console.error("Lỗi xóa voucher:", err);
    alert("Xóa voucher thất bại. Vui lòng thử lại.");
  }
};

const editVoucher = (v) => {
  voucher.value = {
    ...v,
    ngayBatDau: v.ngayBatDau.split("T")[0],
    ngayKetThuc: v.ngayKetThuc.split("T")[0],
  };
  isEditing.value = true;
  // Scroll to form
  document.querySelector('.voucher-form').scrollIntoView({ behavior: 'smooth' });
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
    loading.value = true;
    const res = await apiClient.get(`Vouchers/code?ma=${searchCode.value}`);
    vouchers.value = res.data;
  } catch (err) {
    console.error("Lỗi tìm kiếm mã:", err);
  } finally {
    loading.value = false;
  }
};

const filterByValue = async () => {
  try {
    loading.value = true;
    const res = await apiClient.get(`Vouchers/filter-value?min=${minValue.value}&max=${maxValue.value}`);
    vouchers.value = res.data;
  } catch (err) {
    console.error("Lỗi lọc theo giá trị:", err);
  } finally {
    loading.value = false;
  }
};

const filterByType = async () => {
  try {
    loading.value = true;
    if (selectedType.value === "") {
      fetchVouchers();
    } else {
      const res = await apiClient.get(`Vouchers/filter-type?type=${selectedType.value}`);
      vouchers.value = res.data;
    }
  } catch (err) {
    console.error("Lỗi lọc theo kiểu:", err);
  } finally {
    loading.value = false;
  }
};

const clearSearch = () => {
  searchCode.value = "";
  fetchVouchers();
};

const resetFilters = () => {
  searchCode.value = "";
  minValue.value = 0;
  maxValue.value = 100;
  selectedType.value = "";
  fetchVouchers();
};

onMounted(fetchVouchers);
</script>

<style scoped>
.voucher-management {
  max-width: 1400px;
  margin: 0 auto;
  padding: 20px;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

.header {
  margin-bottom: 30px;
}

.title {
  color: #2c3e50;
  font-size: 2.5rem;
  font-weight: 600;
  margin: 0;
  display: flex;
  align-items: center;
  gap: 15px;
}

.title i {
  color: #3498db;
}

.card {
  background: white;
  border-radius: 12px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.07);
  margin-bottom: 30px;
  overflow: hidden;
  border: 1px solid #e1e8ed;
}

.card-header {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 20px 25px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.section-title {
  margin: 0;
  font-size: 1.4rem;
  font-weight: 500;
  display: flex;
  align-items: center;
  gap: 10px;
}

.badge {
  background: rgba(255, 255, 255, 0.2);
  padding: 4px 12px;
  border-radius: 20px;
  font-size: 0.9rem;
  margin-left: 10px;
}

.card-body {
  padding: 25px;
}

.voucher-form {
  max-width: none;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 20px;
  margin-bottom: 25px;
}

.form-group {
  display: flex;
  flex-direction: column;
}

.form-group label {
  font-weight: 600;
  color: #2c3e50;
  margin-bottom: 8px;
  font-size: 0.95rem;
}

.required {
  color: #e74c3c;
}

.form-control {
  padding: 12px 15px;
  border: 2px solid #e1e8ed;
  border-radius: 8px;
  font-size: 1rem;
  transition: all 0.3s ease;
  background: #fafbfc;
}

.form-control:focus {
  outline: none;
  border-color: #3498db;
  background: white;
  box-shadow: 0 0 0 3px rgba(52, 152, 219, 0.1);
}

.form-actions {
  display: flex;
  gap: 15px;
  justify-content: flex-start;
}

.btn {
  padding: 12px 24px;
  border: none;
  border-radius: 8px;
  font-size: 1rem;
  font-weight: 500;
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  gap: 8px;
  transition: all 0.3s ease;
  text-decoration: none;
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-primary {
  background: linear-gradient(135deg, #3498db, #2980b9);
  color: white;
}

.btn-primary:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 4px 15px rgba(52, 152, 219, 0.4);
}

.btn-success {
  background: linear-gradient(135deg, #27ae60, #229954);
  color: white;
}

.btn-success:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 4px 15px rgba(39, 174, 96, 0.4);
}

.btn-secondary {
  background: linear-gradient(135deg, #95a5a6, #7f8c8d);
  color: white;
}

.btn-warning {
  background: linear-gradient(135deg, #f39c12, #e67e22);
  color: white;
}

.btn-danger {
  background: linear-gradient(135deg, #e74c3c, #c0392b);
  color: white;
}

.btn-outline-primary {
  background: transparent;
  color: #3498db;
  border: 2px solid #3498db;
}

.btn-outline-primary:hover:not(:disabled) {
  background: #3498db;
  color: white;
}

.list-controls {
  display: flex;
  gap: 10px;
}

/* Search Bar Styles */
.search-container {
  position: relative;
  flex: 1;
}

.search-input {
  width: 100%;
  padding: 12px 45px 12px 15px;
  border: 2px solid #e1e8ed;
  border-radius: 25px;
  font-size: 1rem;
  background: rgba(255, 255, 255, 0.9);
  color: #2c3e50;
  transition: all 0.3s ease;
}

.search-input:focus {
  outline: none;
  border-color: #3498db;
  background: white;
  box-shadow: 0 0 0 3px rgba(52, 152, 219, 0.1);
}

.search-input::placeholder {
  color: rgba(67, 63, 63, 0.7);
}

.search-icon {
  position: absolute;
  right: 15px;
  top: 50%;
  transform: translateY(-50%);
  color: rgba(13, 13, 13, 0.8);
  font-size: 1.1rem;
}

.clear-search {
  position: absolute;
  right: 40px;
  top: 50%;
  transform: translateY(-50%);
  background: none;
  border: none;
  color: rgba(0, 0, 0, 0.6);
  cursor: pointer;
  padding: 5px;
  border-radius: 50%;
  transition: all 0.3s ease;
  display: none;
}

.clear-search:hover {
  background: rgba(0, 0, 0, 0.1);
  color: #e74c3c;
}

.clear-search.show {
  display: block;
}

.loading-state, .empty-state {
  text-align: center;
  padding: 60px 20px;
  color: #7f8c8d;
  font-size: 1.1rem;
}

.loading-state i {
  font-size: 2rem;
  margin-bottom: 15px;
  display: block;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.empty-state i {
  font-size: 4rem;
  margin-bottom: 20px;
  display: block;
  color: #bdc3c7;
}

.table-responsive {
  overflow-x: auto;
}

.vouchers-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 20px;
}

.vouchers-table th {
  background: linear-gradient(135deg, #34495e, #2c3e50);
  color: white;
  padding: 15px 12px;
  text-align: left;
  font-weight: 600;
  font-size: 0.95rem;
  border-bottom: 3px solid #3498db;
}

.vouchers-table td {
  padding: 15px 12px;
  border-bottom: 1px solid #ecf0f1;
  vertical-align: middle;
}

.voucher-row:hover {
  background: #f8f9fa;
}

.voucher-id {
  font-family: 'Courier New', monospace;
  font-weight: 600;
  color: #7f8c8d;
  font-size: 0.9rem;
}

.code-container {
  display: flex;
  align-items: center;
  gap: 8px;
}

.code-container i {
  color: #3498db;
  font-size: 1.2rem;
}

.value-display {
  font-weight: 600;
  color: #27ae60;
  font-size: 1.1rem;
}

.type-badge {
  padding: 6px 12px;
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.type-badge.percentage {
  background: linear-gradient(135deg, #9b59b6, #8e44ad);
  color: white;
}

.type-badge.currency {
  background: linear-gradient(135deg, #f39c12, #e67e22);
  color: white;
}

.quantity-badge {
  background: linear-gradient(135deg, #3498db, #2980b9);
  color: white;
  padding: 4px 8px;
  border-radius: 12px;
  font-size: 0.9rem;
  font-weight: 600;
}

.status-badge {
  padding: 6px 12px;
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.status-badge.upcoming {
  background: linear-gradient(135deg, #3498db, #2980b9);
  color: white;
}

.status-badge.active {
  background: linear-gradient(135deg, #27ae60, #229954);
  color: white;
}

.status-badge.expired {
  background: linear-gradient(135deg, #95a5a6, #7f8c8d);
  color: white;
}

.status-badge.outofstock {
  background: linear-gradient(135deg, #e74c3c, #c0392b);
  color: white;
}

.action-buttons {
  display: flex;
  gap: 8px;
  align-items: center;
}

.btn-sm {
  padding: 8px 12px;
  font-size: 0.85rem;
}

/* Responsive Design */
@media (max-width: 768px) {
  .voucher-management {
    padding: 15px;
  }

  .title {
    font-size: 2rem;
  }

  .form-grid {
    grid-template-columns: 1fr;
  }

  .card-header {
    flex-direction: column;
    gap: 15px;
    align-items: stretch;
  }

  .vouchers-table {
    font-size: 0.9rem;
  }

  .vouchers-table th,
  .vouchers-table td {
    padding: 10px 8px;
  }

  .action-buttons {
    flex-direction: column;
    gap: 5px;
  }
}

@media (max-width: 480px) {
  .table-responsive {
    font-size: 0.8rem;
  }
  
  .vouchers-table th,
  .vouchers-table td {
    padding: 8px 6px;
  }
  
  .btn {
    padding: 10px 16px;
    font-size: 0.9rem;
  }
  
  .btn-sm {
    padding: 6px 10px;
    font-size: 0.8rem;
  }
}
</style>