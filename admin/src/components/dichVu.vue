<template>
  <div class="dich-vu-management">
    <!-- Tiêu đề trang -->
    <div class="header">
      <h1 class="title">
        <i class="fa fa-list-alt"></i>
        Quản lý Dịch Vụ
      </h1>
    </div>

    <!-- Bộ lọc -->
    <div class="card filter-section">
      <div class="card-header">
        <h2 class="section-title">
          <i class="fa fa-filter"></i>
          Bộ lọc và tìm kiếm
        </h2>
      </div>
      <div class="card-body">
        <div class="filter-grid">
          <div class="filter-group">
            <label class="filter-label">
              <i class="fa fa-search me-1"></i> Tìm theo tên
            </label>
            <div class="search-container">
              <input 
                v-model="searchName" 
                @input="searchByName" 
                type="text" 
                class="search-input" 
                placeholder="Nhập tên dịch vụ..." 
              />
              <i class="fa fa-search search-icon"></i>
            </div>
          </div>

          <div class="filter-group price-filter">
            <label class="filter-label">
              <i class="fa fa-filter me-1"></i> Lọc giá
            </label>
            <div class="price-inputs">
              <input v-model.number="priceMin" type="number" class="form-control" placeholder="Từ" min="0" />
              <input v-model.number="priceMax" type="number" class="form-control" placeholder="Đến" min="0" />
              <button class="btn btn-success" @click="filterByPrice">
                <i class="fa fa-sort-amount-down-alt"></i> Lọc
              </button>
            </div>
          </div>

          <div class="filter-group">
            <button class="btn btn-outline-primary" @click="fetchDichVus">
              <i class="fa fa-list me-1"></i> Tất cả
            </button>
          </div>
        </div>
      </div>
    </div>

    <div class="content-grid">
      <!-- Form -->
      <div class="form-section">
        <div class="card">
          <div class="card-header">
            <h2 class="section-title">
              <i class="fa fa-plus-circle"></i>
              {{ isEditing ? "Cập nhật Dịch vụ" : "Thêm Dịch vụ mới" }}
            </h2>
          </div>
          <div class="card-body">
            <form @submit.prevent="saveDichVu" class="service-form">
              <div class="form-grid">
                <div class="form-group">
                  <label class="form-label">Tên dịch vụ <span class="required">*</span></label>
                  <input v-model="dichVu.tenDichVu" class="form-control" required />
                </div>
                <div class="form-group">
                  <label class="form-label">Giá (VND) <span class="required">*</span></label>
                  <input v-model.number="dichVu.gia" type="number" class="form-control" min="0" required />
                </div>
                <div class="form-group">
                  <label class="form-label">Thời gian (phút) <span class="required">*</span></label>
                  <input v-model.number="dichVu.thoiGian" type="number" class="form-control" min="0" required />
                </div>
                <div class="form-group">
                  <label class="form-label">Hình ảnh</label>
                  <input v-model="dichVu.hinhAnh" class="form-control" placeholder="image-name" />
                </div>
                <div class="form-group full-width">
                  <label class="form-label">Mô tả</label>
                  <textarea v-model="dichVu.moTa" class="form-control" rows="3"></textarea>
                </div>
                <div class="form-group">
                  <label class="form-label">Loại dịch vụ <span class="required">*</span></label>
                  <select v-model.number="dichVu.loaiDichVuID" class="form-control" required>
                    <option disabled value="">-- Chọn loại dịch vụ --</option>
                    <option v-for="ldv in loaiDichVus" :key="ldv.loaiDichVuID" :value="ldv.loaiDichVuID">
                      {{ ldv.tenLoai }}
                    </option>
                  </select>
                </div>
                <div class="form-group">
                  <label class="form-label">Trạng thái</label>
                  <select v-model.number="dichVu.trangThai" class="form-control">
                    <option :value="1">Hoạt động</option>
                    <option :value="0">Tạm ngừng</option>
                  </select>
                </div>
              </div>
              <div class="form-actions">
                <button type="submit" class="btn btn-success">
                  <i class="fa fa-save"></i> {{ isEditing ? "Cập nhật" : "Thêm" }}
                </button>
                <button type="button" class="btn btn-secondary" @click="resetForm">
                  <i class="fa fa-undo"></i> Hủy
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>

      <!-- Danh sách -->
      <div class="list-section">
        <div class="card">
          <div class="card-header">
            <h2 class="section-title">
              <i class="fa fa-table"></i>
              Danh sách Dịch vụ
            </h2>
          </div>
          <div class="card-body">
            <div class="table-responsive">
              <table class="services-table">
                <thead>
                  <tr>
                    <th>Tên</th>
                    <th>Giá</th>
                    <th>Thời gian</th>
                    <th>Ảnh</th>
                    <th>Ngày tạo</th>
                    <th>Loại</th>
                    <th>Trạng thái</th>
                    <th>Hành động</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="dv in paginatedDichVus" :key="dv.dichVuID" class="service-row">
                    <td class="service-name">{{ dv.tenDichVu }}</td>
                    <td class="service-price">{{ dv.gia.toLocaleString() }} vnđ</td>
                    <td class="service-time">{{ dv.thoiGian }} phút</td>
                    <td class="service-image">
                      <img 
                        :src="'http://localhost:5055/images/' + dv.hinhAnh + '.jpg'" 
                        class="service-img" 
                        alt="Service image"
                      />
                    </td>
                    <td class="service-date">{{ new Date(dv.ngayTao).toLocaleDateString() }}</td>
                    <td class="service-type">
                      <span class="type-badge">{{ dv.tenLoai }}</span>
                    </td>
                    <td class="service-status">
                      <span class="status-badge" :class="dv.trangThai === 1 ? 'active' : 'inactive'">
                        {{ dv.trangThai === 1 ? 'Hoạt động' : 'Tạm ngừng' }}
                      </span>
                    </td>
                    <td class="service-actions">
                      <div class="action-buttons">
                        <button class="btn btn-sm btn-info" @click="editDichVu(dv)" title="Chỉnh sửa">
                          <i class="fa fa-edit"></i>
                        </button>
                        <button 
                          class="btn btn-sm" 
                          :class="dv.trangThai === 1 ? 'btn-warning' : 'btn-success'" 
                          @click="toggleTrangThai(dv)"
                          :title="dv.trangThai === 1 ? 'Tạm ngừng' : 'Kích hoạt'"
                        >
                          <i class="fa" :class="dv.trangThai === 1 ? 'fa-pause' : 'fa-play'"></i>
                        </button>
                      </div>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>

            <!-- Phân trang -->
            <div class="pagination-container">
              <button 
                class="btn btn-outline-primary" 
                :disabled="currentPage === 1" 
                @click="goToPage(currentPage - 1)"
              >
                <i class="fa fa-angle-left"></i>
                Trước
              </button>
              <span class="page-info">Trang {{ currentPage }} / {{ totalPages }}</span>
              <div class="page-numbers">
                <button 
                  v-for="page in totalPages" 
                  :key="page" 
                  class="btn btn-sm page-btn" 
                  :class="page === currentPage ? 'btn-primary' : 'btn-outline-secondary'" 
                  @click="goToPage(page)"
                >
                  {{ page }}
                </button>
              </div>
              <button 
                class="btn btn-outline-primary" 
                :disabled="currentPage === totalPages" 
                @click="goToPage(currentPage + 1)"
              >
                Sau
                <i class="fa fa-angle-right"></i>
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import apiClient from "../utils/axiosClient";

const dichVus = ref([]);
const loaiDichVus = ref([]);
const dichVu = ref({
  dichVuID: 0,
  tenDichVu: "",
  gia: 0,
  thoiGian: 0,
  moTa: "",
  hinhAnh: "",
  ngayTao: new Date().toISOString(),
  trangThai: 1,
  loaiDichVuID: 1,
});
const isEditing = ref(false);
const searchName = ref("");
const priceMin = ref(0);
const priceMax = ref(1000000);

const currentPage = ref(1);
const pageSize = ref(5);
const totalItems = ref(0);
const totalPages = computed(() => Math.ceil(totalItems.value / pageSize.value));
const paginatedDichVus = computed(() => {
  const start = (currentPage.value - 1) * pageSize.value;
  return dichVus.value.slice(start, start + pageSize.value);
});

const goToPage = (page) => {
  if (page >= 1 && page <= totalPages.value) {
    currentPage.value = page;
  }
};

const fetchDichVus = async () => {
  try {
    const response = await apiClient.get("DichVu/all");
    dichVus.value = response.data;
    totalItems.value = response.data.length;
    currentPage.value = 1;
  } catch (error) {
    console.error("Lỗi lấy danh sách dịch vụ:", error);
  }
};

const fetchLoaiDichVus = async () => {
  try {
    const res = await apiClient.get("DichVu/loai");
    loaiDichVus.value = res;
  } catch (error) {
    console.error("Lỗi lấy loại dịch vụ:", error);
  }
};

const searchByName = async () => {
  if (!searchName.value.trim()) return fetchDichVus();
  try {
    const response = await apiClient.get(`DichVu/name?ten=${searchName.value}`);
    dichVus.value = response;
    totalItems.value = response.length;
    currentPage.value = 1;
  } catch (error) {
    console.error("Không tìm thấy dịch vụ:", error);
  }
};

const filterByPrice = async () => {
  if (priceMin.value < 0 || priceMax.value < 0) return alert("Giá không được nhỏ hơn 0");
  try {
    const response = await apiClient.get(`DichVu/filter-by-price?min=${priceMin.value}&max=${priceMax.value}`);
    dichVus.value = response.data;
    totalItems.value = response.data.length;
    currentPage.value = 1;
  } catch (error) {
    console.error("Lỗi lọc giá:", error);
  }
};

const saveDichVu = async () => {
  try {
    const payload = { ...dichVu.value };
    if (isEditing.value) {
      await apiClient.put(`/DichVu/${payload.dichVuID}`, payload);
    } else {
      await apiClient.post("DichVu", payload);
    }
    resetForm();
    fetchDichVus();
  } catch (error) {
    console.error("Lỗi lưu dịch vụ:", error);
  }
};

const editDichVu = (dv) => {
  dichVu.value = { ...dv };
  isEditing.value = true;
};

const resetForm = () => {
  isEditing.value = false;
  dichVu.value = {
    dichVuID: 0,
    tenDichVu: "",
    gia: 0,
    thoiGian: 0,
    moTa: "",
    hinhAnh: "",
    ngayTao: new Date().toISOString(),
    trangThai: 1,
    loaiDichVuID: 1,
  };
};

const toggleTrangThai = async (dv) => {
  try {
    const updatedDv = { ...dv, trangThai: dv.trangThai === 1 ? 0 : 1 };
    await apiClient.put(`/DichVu/${dv.dichVuID}`, updatedDv);
    fetchDichVus();
  } catch (error) {
    console.error("Lỗi cập nhật trạng thái:", error);
  }
};

onMounted(() => {
  fetchDichVus();
  fetchLoaiDichVus();
});
</script>

<style scoped>
.dich-vu-management {
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

.card-body {
  padding: 25px;
}

/* Filter Section Styles */
.filter-section {
  margin-bottom: 30px;
}

.filter-grid {
  display: grid;
  grid-template-columns: 1fr 2fr auto;
  gap: 20px;
  align-items: end;
}

.filter-group {
  display: flex;
  flex-direction: column;
}

.filter-label {
  font-weight: 600;
  color: #2c3e50;
  margin-bottom: 8px;
  font-size: 0.95rem;
}

.search-container {
  position: relative;
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

.search-icon {
  position: absolute;
  right: 15px;
  top: 50%;
  transform: translateY(-50%);
  color: #7f8c8d;
  font-size: 1.1rem;
}

.price-filter .price-inputs {
  display: flex;
  gap: 10px;
  align-items: center;
}

/* Content Grid */
.content-grid {
  display: grid;
  grid-template-columns: 1fr 2fr;
  gap: 30px;
}

/* Form Styles */
.service-form {
  max-width: none;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 20px;
  margin-bottom: 25px;
}

.form-group {
  display: flex;
  flex-direction: column;
}

.form-group.full-width {
  grid-column: 1 / -1;
}

.form-label {
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

/* Button Styles */
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

.btn-info {
  background: linear-gradient(135deg, #3498db, #2980b9);
  color: white;
}

.btn-warning {
  background: linear-gradient(135deg, #f39c12, #e67e22);
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

.btn-outline-secondary {
  background: transparent;
  color: #95a5a6;
  border: 2px solid #95a5a6;
}

.btn-sm {
  padding: 8px 12px;
  font-size: 0.85rem;
}

/* Table Styles */
.table-responsive {
  overflow-x: auto;
}

.services-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 20px;
}

.services-table th {
  background: linear-gradient(135deg, #34495e, #2c3e50);
  color: white;
  padding: 15px 12px;
  text-align: left;
  font-weight: 600;
  font-size: 0.95rem;
  border-bottom: 3px solid #3498db;
}

.services-table td {
  padding: 15px 12px;
  border-bottom: 1px solid #ecf0f1;
  vertical-align: middle;
  text-align: center;
}

.service-row:hover {
  background: #f8f9fa;
}

.service-name {
  font-weight: 600;
  color: #2c3e50;
  text-align: left;
}

.service-price {
  color: #27ae60;
  font-weight: 600;
}

.service-time {
  color: #7f8c8d;
}

.service-img {
  width: 80px;
  height: 60px;
  object-fit: cover;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.service-date {
  color: #7f8c8d;
  font-size: 0.9rem;
}

.type-badge {
  background: linear-gradient(135deg, #9b59b6, #8e44ad);
  color: white;
  padding: 6px 12px;
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.status-badge {
  padding: 6px 12px;
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.status-badge.active {
  background: linear-gradient(135deg, #27ae60, #229954);
  color: white;
}

.status-badge.inactive {
  background: linear-gradient(135deg, #95a5a6, #7f8c8d);
  color: white;
}

.action-buttons {
  display: flex;
  gap: 8px;
  justify-content: center;
  align-items: center;
}

/* Pagination Styles */
.pagination-container {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 30px;
  padding-top: 20px;
  border-top: 1px solid #ecf0f1;
}

.page-info {
  font-weight: 600;
  color: #2c3e50;
}

.page-numbers {
  display: flex;
  gap: 5px;
}

.page-btn {
  min-width: 40px;
  height: 40px;
  display: flex;
  align-items: center;
  justify-content: center;
}

/* Responsive Design */
@media (max-width: 1200px) {
  .content-grid {
    grid-template-columns: 1fr;
  }
  
  .form-section {
    order: 1;
  }
  
  .list-section {
    order: 2;
  }
}

@media (max-width: 768px) {
  .dich-vu-management {
    padding: 15px;
  }

  .title {
    font-size: 2rem;
  }

  .filter-grid {
    grid-template-columns: 1fr;
    gap: 15px;
  }

  .price-filter .price-inputs {
    flex-direction: column;
    align-items: stretch;
  }

  .form-grid {
    grid-template-columns: 1fr;
  }

  .services-table {
    font-size: 0.9rem;
  }

  .services-table th,
  .services-table td {
    padding: 10px 8px;
  }

  .pagination-container {
    flex-direction: column;
    gap: 15px;
  }
}

@media (max-width: 480px) {
  .table-responsive {
    font-size: 0.8rem;
  }
  
  .services-table th,
  .services-table td {
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

  .service-img {
    width: 60px;
    height: 45px;
  }
}
</style>