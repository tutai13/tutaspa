<template>
  <div class="loai-dich-vu-management">
    <!-- Header -->
    <div class="header">
      <h1 class="title">
        <i class="fa fa-list-alt"></i>
        Quản lý Loại Dịch Vụ
      </h1>
    </div>

    <!-- Form Section -->
    <div class="card form-section">
      <div class="card-header">
        <h2 class="section-title">
          <i class="fa" :class="isEditing ? 'fa-edit' : 'fa-plus-circle'"></i>
          {{ isEditing ? 'Cập nhật' : 'Thêm mới' }} Loại Dịch Vụ
        </h2>
      </div>
      <div class="card-body">
        <form @submit.prevent="isEditing ? updateLoaiDichVu() : createLoaiDichVu()" class="service-type-form">
          <div class="form-group-container">
            <div class="form-group">
              <label class="form-label">Tên loại dịch vụ <span class="required">*</span></label>
              <input 
                type="text" 
                class="form-control" 
                v-model="form.tenLoai" 
                placeholder="Nhập tên loại dịch vụ..." 
                required
              />
            </div>
            <button type="submit" class="btn btn-success form-submit-btn">
              <i class="fa" :class="isEditing ? 'fa-save' : 'fa-plus'"></i>
              {{ isEditing ? 'Cập nhật' : 'Thêm mới' }}
            </button>
          </div>
          <div class="form-actions" v-if="isEditing">
            <button 
              type="button"
              class="btn btn-secondary" 
              @click="resetForm"
            >
              <i class="fa fa-undo"></i> Hủy
            </button>
          </div>
        </form>
      </div>
    </div>

    <!-- List Section -->
    <div class="card list-section">
      <div class="card-header">
        <h2 class="section-title">
          <i class="fa fa-table"></i>
          Danh sách loại dịch vụ
        </h2>
        <div class="list-controls">
          <div class="search-container">
            <input
              v-model="searchName"
              @input="searchByName"
              type="text"
              class="search-input"
              placeholder="Tìm theo tên loại dịch vụ..."
            />
            <i class="fa fa-search search-icon"></i>
            <button
              v-if="searchName"
              @click="resetSearch"
              class="clear-search"
              title="Xóa tìm kiếm"
            >
              <i class="fa fa-times"></i>
            </button>
          </div>
          <button @click="fetchData" class="btn btn-outline-primary">
            <i class="fa fa-sync-alt"></i>
            Làm mới
          </button>
        </div>
      </div>
      <div class="card-body">
        <!-- Empty State -->
        <div v-if="!loaiDichVus?.length" class="empty-state">
          <i class="fa fa-folder-open"></i>
          <p>Không có loại dịch vụ nào</p>
        </div>

        <!-- Table -->
        <div v-else class="table-responsive">
          <table class="service-types-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Tên Loại</th>
                <th>Hành động</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in loaiDichVus" :key="item.loaiDichVuID" class="service-type-row">
                <td class="service-id">{{ item.loaiDichVuID }}</td>
                <td class="service-name">{{ item.tenLoai }}</td>
                <td class="service-actions">
                  <div class="action-buttons">
                    <button 
                      class="btn btn-sm btn-info" 
                      @click="editLoaiDichVu(item)"
                      title="Chỉnh sửa"
                    >
                      <i class="fa fa-edit"></i>
                    </button>
                    <button 
                      class="btn btn-sm btn-danger" 
                      @click="deleteLoaiDichVu(item.loaiDichVuID)"
                      title="Xóa"
                    >
                      <i class="fa fa-trash"></i>
                    </button>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>

      </div>
    </div>

    <!-- Toast Notifications -->
    <div class="toast-container">
      <div 
        v-for="toast in toasts" 
        :key="toast.id" 
        class="toast" 
        :class="toast.type"
      >
        <i class="fa" :class="getToastIcon(toast.type)"></i>
        {{ toast.message }}
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import apiClient from "../utils/axiosClient";
// State
const loaiDichVus = ref([]);        // Dữ liệu hiển thị
const allLoaiDichVus = ref([]);     // Dữ liệu gốc từ API
const form = ref({
  loaiDichVuID: 0,
  tenLoai: ''
});
const isEditing = ref(false);
const searchName = ref('');
const toasts = ref([]);

// Toast methods
const showToast = (message, type = 'info') => {
  const toast = {
    id: Date.now(),
    message,
    type
  }
  toasts.value.push(toast)
  setTimeout(() => {
    const index = toasts.value.findIndex(t => t.id === toast.id)
    if (index > -1) toasts.value.splice(index, 1)
  }, 3000)
}

const getToastIcon = (type) => {
  switch (type) {
    case 'success': return 'fa-check-circle'
    case 'error': return 'fa-exclamation-circle'
    case 'warning': return 'fa-exclamation-triangle'
    default: return 'fa-info-circle'
  }
}

// Methods
const fetchData = async () => {
  try {
    const res = await apiClient.get('/LoaiDichVu');
    allLoaiDichVus.value = res;
    loaiDichVus.value = res;
  } catch (error) {
    showToast("Lỗi khi tải danh sách loại dịch vụ", 'error');
    console.error(error);
  }
};

const createLoaiDichVu = async () => {
  if (!form.value.tenLoai.trim()) {
    showToast("Vui lòng nhập tên loại", 'warning');
    return;
  }
  try {
    await apiClient.post('/LoaiDichVu', form.value);
    showToast("Thêm loại dịch vụ thành công!", 'success');
    resetForm();
    fetchData();
  } catch (error) {
    showToast("Lỗi khi thêm loại dịch vụ", 'error');
    console.error(error);
  }
};

const updateLoaiDichVu = async () => {
  try {
    await apiClient.put(`/LoaiDichVu/${form.value.loaiDichVuID}`, form.value);
    showToast("Cập nhật loại dịch vụ thành công!", 'success');
    resetForm();
    fetchData();
  } catch (error) {
    showToast("Lỗi khi cập nhật loại dịch vụ", 'error');
    console.error(error);
  }
};

const deleteLoaiDichVu = async (id) => {
  if (confirm("Bạn có chắc muốn xóa loại dịch vụ này?")) {
    try {
      await apiClient.delete(`/LoaiDichVu/${id}`);
      showToast("Xóa loại dịch vụ thành công!", 'success');
      fetchData();  
    } catch (error) {
      showToast("Lỗi khi xóa loại dịch vụ", 'error');
      console.error(error);
    }
  }
};

const editLoaiDichVu = (item) => {
  form.value = { ...item };
  isEditing.value = true;
};

const resetForm = () => {
  form.value = {
    loaiDichVuID: 0,
    tenLoai: ''
  };
  isEditing.value = false;
};

const searchByName = () => {
  const keyword = searchName.value.trim().toLowerCase();
  if (!keyword) {
    loaiDichVus.value = [...allLoaiDichVus.value];
    return;
  }
  loaiDichVus.value = allLoaiDichVus.value.filter(item =>
    item.tenLoai.toLowerCase().includes(keyword)
  );
};

const resetSearch = () => {
  searchName.value = '';
  loaiDichVus.value = [...allLoaiDichVus.value];
};

// Fetch data on mount
onMounted(() => {
  fetchData();
});
</script>

<style scoped>
.loai-dich-vu-management {
  max-width: 1000px;
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

.list-controls {
  display: flex;
  gap: 10px;
}

/* Search Section */
.search-container {
  position: relative;
  max-width: 500px;
  margin: 0 auto;
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
  color: #7f8c8d;
  font-size: 1.1rem;
}

.clear-search {
  position: absolute;
  right: 40px;
  top: 50%;
  transform: translateY(-50%);
  background: none;
  border: none;
  color: #7f8c8d;
  cursor: pointer;
  padding: 5px;
  border-radius: 50%;
  transition: all 0.3s ease;
}

.clear-search:hover {
  background: rgba(127, 140, 141, 0.1);
  color: #2c3e50;
}

/* Form Section */
.service-type-form {
  max-width: 600px;
  margin: 0 auto;
}

.form-group-container {
  display: flex;
  align-items: flex-end;
  gap: 15px;
  margin-bottom: 25px;
}

.form-group {
  flex: 1;
  margin-bottom: 0;
}

.form-label {
  font-weight: 600;
  color: #2c3e50;
  margin-bottom: 8px;
  font-size: 0.95rem;
  display: block;
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
  width: 100%;
}

.form-control:focus {
  outline: none;
  border-color: #3498db;
  background: white;
  box-shadow: 0 0 0 3px rgba(52, 152, 219, 0.1);
}

.form-submit-btn {
  padding: 12px 24px;
}

.form-actions {
  display: flex;
  justify-content: center;
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

.btn-sm {
  padding: 8px 12px;
  font-size: 0.85rem;
}

/* Empty State */
.empty-state {
  text-align: center;
  padding: 60px 20px;
  color: #7f8c8d;
  font-size: 1.1rem;
}

.empty-state i {
  font-size: 4rem;
  margin-bottom: 20px;
  display: block;
  color: #bdc3c7;
}

/* Table Styles */
.table-responsive {
  overflow-x: auto;
}

.service-types-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 20px;
}

.service-types-table th {
  background: linear-gradient(135deg, #34495e, #2c3e50);
  color: white;
  padding: 15px 12px;
  text-align: left;
  font-weight: 600;
  font-size: 0.95rem;
  border-bottom: 3px solid #3498db;
}

.service-types-table th:first-child {
  width: 15%;
  text-align: center;
}

.service-types-table th:last-child {
  width: 25%;
  text-align: center;
}

.service-types-table td {
  padding: 15px 12px;
  border-bottom: 1px solid #ecf0f1;
  vertical-align: middle;
}

.service-type-row:hover {
  background: #f8f9fa;
}

.service-id {
  text-align: center;
  font-family: 'Courier New', monospace;
  font-weight: 600;
  color: #7f8c8d;
  font-size: 0.9rem;
}

.service-name {
  font-weight: 600;
  color: #2c3e50;
}

.service-actions {
  text-align: center;
}

.action-buttons {
  display: flex;
  gap: 8px;
  justify-content: center;
  align-items: center;
}

/* Toast Notifications */
.toast-container {
  position: fixed;
  top: 20px;
  right: 20px;
  z-index: 1100;
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.toast {
  padding: 15px 20px;
  border-radius: 8px;
  color: white;
  font-weight: 500;
  display: flex;
  align-items: center;
  gap: 10px;
  min-width: 250px;
  animation: slideIn 0.3s ease;
}

.toast.success {
  background: linear-gradient(135deg, #27ae60, #229954);
}

.toast.error {
  background: linear-gradient(135deg, #e74c3c, #c0392b);
}

.toast.warning {
  background: linear-gradient(135deg, #f39c12, #e67e22);
}

.toast.info {
  background: linear-gradient(135deg, #3498db, #2980b9);
}

@keyframes slideIn {
  from {
    transform: translateX(100%);
    opacity: 0;
  }
  to {
    transform: translateX(0);
    opacity: 1;
  }
}

/* Responsive Design */
@media (max-width: 768px) {
  .loai-dich-vu-management {
    padding: 15px;
  }

  .title {
    font-size: 2rem;
  }

  .card-header {
    flex-direction: column;
    gap: 15px;
    align-items: stretch;
  }

  .form-group-container {
    flex-direction: column;
    align-items: stretch;
  }

  .form-submit-btn {
    width: 100%;
  }

  .service-types-table {
    font-size: 0.9rem;
  }

  .service-types-table th,
  .service-types-table td {
    padding: 10px 8px;
  }

  .action-buttons {
    flex-direction: column;
    gap: 5px;
  }

  .toast-container {
    left: 10px;
    right: 10px;
  }

  .toast {
    min-width: auto;
  }
}

@media (max-width: 480px) {
  .table-responsive {
    font-size: 0.8rem;
  }
  
  .service-types-table th,
  .service-types-table td {
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
  
  .form-actions {
    align-items: stretch;
  }
}
</style>