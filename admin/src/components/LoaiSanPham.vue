```vue
<template>
  <div class="category-management">
    <!-- Header -->
    <div class="header">
      <h1 class="title">
        <i class="fas fa-tags"></i>
        Quản lý loại sản phẩm
      </h1>
    </div>

    <!-- Add Category Section -->
    <div class="card add-category-section">
      <div class="card-header">
        <h2 class="section-title">
          <i class="fas fa-plus-circle"></i>
          {{ isEditing ? "Cập nhật loại sản phẩm" : "Thêm loại sản phẩm mới" }}
        </h2>
        <button 
          @click="toggleAddForm" 
          class="btn btn-primary"
          :class="{ 'active': showAddForm }"
        >
          <i class="fas" :class="showAddForm ? 'fa-minus' : 'fa-plus'"></i>
          {{ showAddForm ? 'Ẩn form' : 'Hiển thị form' }}
        </button>
      </div>
      
      <div v-if="showAddForm" class="card-body">
        <form @submit.prevent="saveCategory" class="category-form">
          <div class="form-grid">
            <div class="form-group full-width">
              <label for="tenLoai">Tên loại sản phẩm <span class="required">*</span></label>
              <input
                id="tenLoai"
                v-model="category.tenLoai"
                type="text"
                class="form-control"
                placeholder="Nhập tên loại sản phẩm"
                required
              />
            </div>
          </div>

          <div class="form-actions">
            <button 
              type="submit" 
              class="btn btn-success"
              :disabled="loading"
            >
              <i class="fas fa-save"></i>
              {{ loading ? 'Đang lưu...' : (isEditing ? 'Cập nhật' : 'Thêm') }}
            </button>
            <button 
              type="button" 
              @click="resetForm" 
              class="btn btn-secondary"
            >
              <i class="fas fa-undo"></i>
              Hủy
            </button>
          </div>
        </form>
      </div>
    </div>

    <!-- Categories List Section -->
    <div class="card categories-list-section">
      <div class="card-header">
        <h2 class="section-title">
          <i class="fas fa-list"></i>
          Danh sách loại sản phẩm
        </h2>
        <div class="list-controls">
          <div class="search-container">
            <input
              v-model="searchKeyword"
              @input="searchCategories"
              type="text"
              class="search-input"
              placeholder="Tìm theo tên loại sản phẩm..."
            />
            <button
              v-if="searchKeyword"
              @click="clearSearch"
              class="clear-search"
              title="Xóa tìm kiếm"
            >
              <i class="fas fa-times"></i>
            </button>
            <i class="fas fa-search search-icon"></i>
          </div>
          <button @click="fetchCategories" class="btn btn-outline-primary">
            <i class="fas fa-sync-alt"></i>
            Làm mới
          </button>
        </div>
      </div>

      <div class="card-body">
        <!-- Loading State -->
        <div v-if="loading" class="loading-state">
          <i class="fas fa-spinner fa-spin"></i>
          Đang tải...
        </div>

        <!-- Empty State -->
        <div v-else-if="!filteredCategories.length" class="empty-state">
          <i class="fas fa-tags"></i>
          <p>Không có loại sản phẩm nào</p>
        </div>

        <!-- Categories Table -->
        <div v-else class="table-responsive">
          <table class="categories-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Tên loại sản phẩm</th>
                <th>Thao tác</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="c in filteredCategories" :key="c.loaiSanPhamId" class="category-row">
                <td class="category-id">
                  <span class="id-badge">{{ c.loaiSanPhamId }}</span>
                </td>
                <td class="category-name">
                  <div class="name-container">
                    <i class="fas fa-tag"></i>
                    {{ c.tenLoai }}
                  </div>
                </td>
                <td class="category-actions">
                  <div class="action-buttons">
                    <button 
                      @click="editCategory(c)" 
                      class="btn btn-sm btn-info"
                      title="Chỉnh sửa"
                    >
                      <i class="fas fa-edit"></i>
                    </button>
                    <button 
                      @click="deleteCategory(c.loaiSanPhamId)" 
                      class="btn btn-sm btn-danger"
                      title="Xóa"
                    >
                      <i class="fas fa-trash"></i>
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
        <i class="fas" :class="getToastIcon(toast.type)"></i>
        {{ toast.message }}
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import apiClient from '../utils/axiosClient';

const categories = ref([]);
const category = ref({
  loaiSanPhamId: 0,
  tenLoai: '',
});
const isEditing = ref(false);
const loading = ref(false);
const showAddForm = ref(false);
const searchKeyword = ref("");
const toasts = ref([]);

// Computed filtered categories
const filteredCategories = computed(() => {
  if (!searchKeyword.value.trim()) return categories.value;
  return categories.value.filter((c) =>
    c.tenLoai.toLowerCase().includes(searchKeyword.value.toLowerCase())
  );
});

// Toast notification methods
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

const toggleAddForm = () => {
  showAddForm.value = !showAddForm.value
  if (!showAddForm.value) {
    resetForm()
  }
}

const fetchCategories = async () => {
  try {
    loading.value = true;
    const res = await apiClient.get("/Category");
    categories.value = res; // Sử dụng trực tiếp res vì phản hồi là mảng
    searchKeyword.value = "";
  } catch (err) {
    console.error("Lỗi tải loại sản phẩm:", err);
    showToast('Lỗi khi tải danh sách loại sản phẩm', 'error');
  } finally {
    loading.value = false;
  }
};

const saveCategory = async () => {
  try {
    loading.value = true;
    if (isEditing.value) {
      await apiClient.put(`Category/${category.value.loaiSanPhamId}`, category.value);
      showToast('Cập nhật loại sản phẩm thành công!', 'success');
    } else {
      await apiClient.post("Category", category.value);
      showToast('Thêm loại sản phẩm thành công!', 'success');
    }
    resetForm();
    showAddForm.value = false;
    fetchCategories();
  } catch (err) {
    console.error("Lỗi lưu loại sản phẩm:", err);
    showToast('Lỗi khi lưu loại sản phẩm', 'error');
  } finally {
    loading.value = false;
  }
};

const editCategory = (c) => {
  category.value = { ...c };
  isEditing.value = true;
  showAddForm.value = true;
};

const deleteCategory = async (id) => {
  if (!confirm("Bạn có chắc muốn xóa loại sản phẩm này?")) return;
  try {
    loading.value = true;
    await apiClient.delete(`Category/${id}`);
    showToast('Xóa loại sản phẩm thành công!', 'success');
    fetchCategories();
  } catch (err) {
    console.error("Lỗi xóa loại sản phẩm:", err);
    showToast('Lỗi khi xóa loại sản phẩm', 'error');
  } finally {
    loading.value = false;
  }
};

const resetForm = () => {
  isEditing.value = false;
  category.value = {
    loaiSanPhamId: 0,
    tenLoai: '',
  };
};

const searchCategories = () => {
  // Lọc cục bộ, không gọi API
};

const clearSearch = () => {
  searchKeyword.value = "";
};

onMounted(fetchCategories);
</script>

<style scoped>
.category-management {
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

.category-form {
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

.form-group.full-width {
  grid-column: 1 / -1;
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

.btn-primary.active {
  background: linear-gradient(135deg, #e74c3c, #c0392b);
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

.list-controls {
  display: flex;
  gap: 10px;
}

.search-container {
  position: relative;
  flex: 1;
  max-width: 400px;
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
  color: rgba(13, 13, 13, 0.8);
  cursor: pointer;
  padding: 5px;
  border-radius: 50%;
  transition: all 0.3s ease;
}

.clear-search:hover {
  background: rgba(13, 13, 13, 0.1);
  color: #2c3e50;
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

.categories-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 20px;
}

.categories-table th {
  background: linear-gradient(135deg, #34495e, #2c3e50);
  color: white;
  padding: 15px 12px;
  text-align: left;
  font-weight: 600;
  font-size: 0.95rem;
  border-bottom: 3px solid #3498db;
}

.categories-table td {
  padding: 15px 12px;
  border-bottom: 1px solid #ecf0f1;
  vertical-align: middle;
}

.category-row:hover {
  background: #f8f9fa;
}

.category-id {
  text-align: center;
}

.id-badge {
  background: linear-gradient(135deg, #667eea, #764ba2);
  color: white;
  padding: 6px 12px;
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: 600;
  display: inline-block;
}

.name-container {
  display: flex;
  align-items: center;
  gap: 10px;
  font-weight: 500;
}

.name-container i {
  color: #3498db;
  font-size: 1.1rem;
}

.action-buttons {
  display: flex;
  gap: 8px;
  justify-content: center;
}

.btn-sm {
  padding: 8px 12px;
  font-size: 0.85rem;
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
  .category-management {
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

  .categories-table {
    font-size: 0.9rem;
  }

  .categories-table th,
  .categories-table td {
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
  
  .categories-table th,
  .categories-table td {
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