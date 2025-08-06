<template>
  <div class="dich-vu-management">
    <!-- Tiêu đề trang -->
    <div class="header">
      <h1 class="title">
        <i class="fa fa-list-alt"></i>
        Quản lý Dịch Vụ
      </h1>
    </div>

    <!-- Import Section -->
    <div class="card import-section">
      <div class="card-header">
        <h2 class="section-title">
          <i class="fa fa-file-excel"></i>
          Import Dịch vụ từ Excel
        </h2>
        <button class="btn btn-outline-light btn-sm" @click="downloadTemplate">
          <i class="fa fa-download"></i>
          Tải file mẫu
        </button>
      </div>
      <div class="card-body">
        <div class="import-container">
          <div class="upload-zone" :class="{ 'dragover': isDragOver }" 
               @drop="handleDrop" 
               @dragover.prevent="isDragOver = true" 
               @dragleave="isDragOver = false"
               @click="triggerFileInput">
            <div class="upload-content">
              <i class="fa fa-cloud-upload-alt upload-icon"></i>
              <h3>Kéo thả file Excel vào đây</h3>
              <p>hoặc <span class="upload-link">nhấn để chọn file</span></p>
              <small class="upload-note">Chỉ hỗ trợ file .xlsx</small>
            </div>
            <input 
              ref="fileInput" 
              type="file" 
              accept=".xlsx" 
              @change="handleFileSelect" 
              style="display: none"
            />
          </div>
          
          <div v-if="selectedFile" class="selected-file">
            <div class="file-info">
              <i class="fa fa-file-excel file-icon"></i>
              <div class="file-details">
                <span class="file-name">{{ selectedFile.name }}</span>
                <small class="file-size">{{ formatFileSize(selectedFile.size) }}</small>
              </div>
              <button class="btn btn-sm btn-danger" @click="removeFile">
                <i class="fa fa-times"></i>
              </button>
            </div>
            <div class="import-actions">
              <button class="btn btn-success" @click="importFile" :disabled="importing">
                <i class="fa fa-upload" :class="{ 'fa-spin': importing }"></i>
                {{ importing ? 'Đang import...' : 'Import dữ liệu' }}
              </button>
            </div>
          </div>

          <!-- Import Progress -->
          <div v-if="importing" class="import-progress">
            <div class="progress-bar">
              <div class="progress-fill" :style="{ width: importProgress + '%' }"></div>
            </div>
            <p class="progress-text">Đang xử lý... {{ importProgress }}%</p>
          </div>

          <!-- Import Result -->
          <div v-if="importResult" class="import-result" :class="importResult.success ? 'success' : 'error'">
            <div class="result-icon">
              <i class="fa" :class="importResult.success ? 'fa-check-circle' : 'fa-exclamation-circle'"></i>
            </div>
            <div class="result-content">
              <h4>{{ importResult.success ? 'Import thành công!' : 'Import thất bại!' }}</h4>
              <p v-if="importResult.success">
                Đã import thành công {{ importResult.count }} dịch vụ
              </p>
              <p v-else>{{ importResult.message }}</p>
            </div>
            <button class="btn btn-sm btn-outline-secondary" @click="clearResult">
              <i class="fa fa-times"></i>
            </button>
          </div>
        </div>
      </div>
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
          <!-- Tìm kiếm theo tên -->
          <div class="filter-group">
            <label class="filter-label">
            <i class="fa-solid fa-magnifying-glass me-1" style="color:#e83e8c;"></i> Tìm theo tên
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

          <!-- Lọc theo giá -->
          <div class="filter-group price-filter">
            <label class="filter-label">
            <i class="fa-solid fa-sliders me-1"></i> Lọc giá
            </label>
            <div class="price-inputs">
              <input v-model.number="priceMin" type="number" class="form-control" placeholder="Từ" min="0" />
              <input v-model.number="priceMax" type="number" class="form-control" placeholder="Đến" min="0" />
              <button class="btn btn-primary" @click="filterByPrice">
                <i class="fa fa-sort-amount-down-alt"></i> Lọc
              </button>
            </div>
          </div>

          <!-- Xem tất cả -->
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
                  <input type="file" class="form-control" @change="handleFileChange" accept="image/*" />
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
                <button type="submit" class="btn btn-primary">
                  <i class="fa fa-save"></i> {{ isEditing ? "Cập nhật" : "Thêm" }}
                </button>
                <button type="button" class="btn btn-danger" @click="resetForm">
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
                        :src="'https://localhost:7183/images/' + dv.hinhAnh" 
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
import Swal from "sweetalert2";
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
const selectedImage = ref(null);
const isEditing = ref(false);
const searchName = ref("");
const priceMin = ref(0);
const priceMax = ref(0);

// Import related refs
const selectedFile = ref(null);
const isDragOver = ref(false);
const importing = ref(false);
const importProgress = ref(0);
const importResult = ref(null);
const fileInput = ref(null);

const currentPage = ref(1);
const pageSize = ref(5);
const totalItems = ref(0);
const totalPages = computed(() => Math.ceil(totalItems.value / pageSize.value));
const paginatedDichVus = computed(() => {
  const start = (currentPage.value - 1) * pageSize.value;
  return dichVus.value.slice(start, start + pageSize.value);
});

const handleFileChange = (e) => {
  selectedImage.value = e.target.files[0];
};

// Import functions
const triggerFileInput = () => {
  fileInput.value.click();
};

const handleFileSelect = (e) => {
  const file = e.target.files[0];
  if (file && file.name.endsWith('.xlsx')) {
    selectedFile.value = file;
    importResult.value = null;
  } else {
    alert('Chỉ hỗ trợ file .xlsx');
  }
};

const handleDrop = (e) => {
  e.preventDefault();
  isDragOver.value = false;
  const file = e.dataTransfer.files[0];
  if (file && file.name.endsWith('.xlsx')) {
    selectedFile.value = file;
    importResult.value = null;
  } else {
    alert('Chỉ hỗ trợ file .xlsx');
  }
};

const removeFile = () => {
  selectedFile.value = null;
  if (fileInput.value) {
    fileInput.value.value = '';
  }
};

const formatFileSize = (bytes) => {
  if (bytes === 0) return '0 Bytes';
  const k = 1024;
  const sizes = ['Bytes', 'KB', 'MB', 'GB'];
  const i = Math.floor(Math.log(bytes) / Math.log(k));
  return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
};

const importFile = async () => {
  if (!selectedFile.value) return;

  importing.value = true;
  importProgress.value = 0;
  importResult.value = null;

  try {
    const formData = new FormData();
    formData.append('file', selectedFile.value);

    // Simulate progress
    const progressInterval = setInterval(() => {
      if (importProgress.value < 90) {
        importProgress.value += 10;
      }
    }, 200);

    const response = await apiClient.post('/DichVu/import', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });

    clearInterval(progressInterval);
    importProgress.value = 100;

    setTimeout(() => {
      importing.value = false;
      importResult.value = {
        success: true,
        count: response.count || 0,
        message: `Import thành công ${response.count || 0} dịch vụ`
      };
      
      // Refresh the list
      fetchDichVus();
      
      // Clear file after successful import
      setTimeout(() => {
        selectedFile.value = null;
        if (fileInput.value) {
          fileInput.value.value = '';
        }
      }, 3000);
    }, 500);

  } catch (error) {
    importing.value = false;
    importProgress.value = 0;
    importResult.value = {
      success: false,
      message: error.response?.data?.message || 'Có lỗi xảy ra khi import file'
    };
    console.error('Import error:', error);
  }
};

const downloadTemplate = () => {
  const link = document.createElement('a');
  link.href = 'https://drive.google.com/uc?export=download&id=1bf_a4YgEFKGOcyT6gbmyE_J6DfU3r6XQ';
  link.download = 'LDV_DV_LSP_SP.xlsx';
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
};


const clearResult = () => {
  importResult.value = null;
};

const goToPage = (page) => {
  if (page >= 1 && page <= totalPages.value) {
    currentPage.value = page;
  }
};

const fetchDichVus = async () => {
  try {
    const data = await apiClient.get("/DichVu/all");
    dichVus.value = data;
    totalItems.value = data.length;
    currentPage.value = 1;
  } catch (error) {
    console.error("Lỗi lấy danh sách dịch vụ:", error);
  }
};

const fetchLoaiDichVus = async () => {
  try {
    const data = await apiClient.get("/DichVu/loai");
    loaiDichVus.value = data;
  } catch (error) {
    console.error("Lỗi lấy loại dịch vụ:", error);
  }
};

const searchByName = async () => {
  if (!searchName.value.trim()) return fetchDichVus();
  try {
    const data = await apiClient.get(`/DichVu/name?ten=${searchName.value}`);
    dichVus.value = data;
    totalItems.value = data.length;
    currentPage.value = 1;
  } catch (error) {
    console.error("Không tìm thấy dịch vụ:", error);
  }
};

const filterByPrice = async () => {
  if (priceMin.value < 0 || priceMax.value < 0) {
    return alert("Giá không được nhỏ hơn 0");
  }

  if (priceMax.value === 0 && priceMin.value > 0) {
    priceMax.value = priceMin.value;
  }

  if (priceMin.value > priceMax.value) {
    return alert("Giá tối thiểu không được lớn hơn giá tối đa");
  }

  try {
    const data = await apiClient.get(`/DichVu/filter-by-price?min=${priceMin.value}&max=${priceMax.value}`);
    dichVus.value = data;
    totalItems.value = data.length;
    currentPage.value = 1;
  } catch (error) {
    console.error("Lỗi lọc giá:", error);
  }
};
const saveDichVu = async () => {
  try {
    let imageName = dichVu.value.hinhAnh;

    if (selectedImage.value) {
      const formData = new FormData();
      formData.append("file", selectedImage.value);
      formData.append("tenDichVu", dichVu.value.tenDichVu);

      const uploadRes = await apiClient.post("/DichVu/image", formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });

      imageName = uploadRes.fileName || uploadRes;
    }

    const payload = { ...dichVu.value, hinhAnh: imageName };

    if (isEditing.value) {
      await apiClient.put(`/DichVu/${payload.dichVuID}`, payload);
      Swal.fire({
        icon: "success",
        title: "Cập nhật dịch vụ thành công!",
        showConfirmButton: false,
        timer: 2000
      });
    } else {
      await apiClient.post("/DichVu", payload);
      Swal.fire({
        icon: "success",
        title: "Thêm dịch vụ thành công!",
        showConfirmButton: false,
        timer: 2000
      });
    }

    resetForm();
    selectedImage.value = null;
    fetchDichVus();
  } catch (error) {
    console.error("Lỗi lưu dịch vụ:", error);
    Swal.fire({
      icon: "error",
      title: "Lỗi khi lưu dịch vụ!",
      text: "Vui lòng thử lại sau.",
    });
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

/* Import Section Styles */
.import-section {
  margin-bottom: 30px;
}

.import-container {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.upload-zone {
  border: 3px dashed #cbd5e0;
  border-radius: 12px;
  padding: 40px 20px;
  text-align: center;
  background: #f8f9fa;
  cursor: pointer;
  transition: all 0.3s ease;
  position: relative;
}

.upload-zone:hover,
.upload-zone.dragover {
  border-color: #3498db;
  background: rgba(52, 152, 219, 0.05);
  transform: translateY(-2px);
}

.upload-content {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 15px;
}

.upload-icon {
  font-size: 3rem;
  color: #3498db;
  margin-bottom: 10px;
}

.upload-zone h3 {
  color: #2c3e50;
  font-size: 1.5rem;
  margin: 0;
}

.upload-zone p {
  color: #7f8c8d;
  font-size: 1.1rem;
  margin: 0;
}

.upload-link {
  color: #3498db;
  font-weight: 600;
  text-decoration: underline;
}

.upload-note {
  color: #95a5a6;
  font-style: italic;
}

.selected-file {
  background: white;
  border: 2px solid #e1e8ed;
  border-radius: 12px;
  padding: 20px;
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.file-info {
  display: flex;
  align-items: center;
  gap: 15px;
  padding: 15px;
  background: #f8f9fa;
  border-radius: 8px;
  border: 1px solid #e1e8ed;
}

.file-icon {
  font-size: 2rem;
  color: #27ae60;
}

.file-details {
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 5px;
}

.file-name {
  font-weight: 600;
  color: #2c3e50;
  font-size: 1.1rem;
}

.file-size {
  color: #7f8c8d;
  font-size: 0.9rem;
}

.import-actions {
  display: flex;
  justify-content: flex-start;
}

.import-progress {
  background: white;
  border: 2px solid #e1e8ed;
  border-radius: 12px;
  padding: 25px;
  text-align: center;
}

.progress-bar {
  width: 100%;
  height: 8px;
  background: #ecf0f1;
  border-radius: 4px;
  overflow: hidden;
  margin-bottom: 15px;
}

.progress-fill {
  height: 100%;
  background: linear-gradient(90deg, #3498db, #2ecc71);
  border-radius: 4px;
  transition: width 0.3s ease;
}

.progress-text {
  color: #2c3e50;
  font-weight: 600;
  margin: 0;
}

.import-result {
  display: flex;
  align-items: center;
  gap: 15px;
  padding: 20px;
  border-radius: 12px;
  border-left: 5px solid;
}

.import-result.success {
  background: rgba(39, 174, 96, 0.1);
  border-left-color: #27ae60;
}

.import-result.error {
  background: rgba(231, 76, 60, 0.1);
  border-left-color: #e74c3c;
}

.result-icon {
  font-size: 2rem;
}

.import-result.success .result-icon {
  color: #27ae60;
}

.import-result.error .result-icon {
  color: #e74c3c;
}

.result-content {
  flex: 1;
}

.result-content h4 {
  margin: 0 0 5px 0;
  font-size: 1.2rem;
  font-weight: 600;
}

.import-result.success .result-content h4 {
  color: #27ae60;
}

.import-result.error .result-content h4 {
  color: #e74c3c;
}

.result-content p {
  margin: 0;
  color: #7f8c8d;
  font-size: 1rem;
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

.btn-outline-secondary {
  background: transparent;
  color: #95a5a6;
  border: 2px solid #95a5a6;
}

.btn-outline-light {
  background: transparent;
  color: white;
  border: 2px solid rgba(255, 255, 255, 0.3);
}

.btn-outline-light:hover:not(:disabled) {
  background: rgba(255, 255, 255, 0.1);
  border-color: white;
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
  width: 90px;
  height: 70px;
  object-fit: cover;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.service-date {
  color: #7f8c8d;
  font-size: 0.9rem;
}

.type-badge {
  display: inline-block;
  min-width: 80px;
  text-align: center;
  background: linear-gradient(135deg, #9b59b6, #8e44ad);
  color: white;
  padding: 6px 12px;
  border-radius: 16px;
  font-size: 0.8rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  white-space: nowrap;
}

.status-badge {
  display: inline-block;
  min-width: 100px;
  text-align: center;
  padding: 6px 12px;
  border-radius: 16px;
  font-size: 0.8rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  white-space: nowrap;
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

/* Animation for import progress */
@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.fa-spin {
  animation: spin 1s linear infinite;
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

  .upload-zone {
    padding: 30px 15px;
  }

  .upload-icon {
    font-size: 2.5rem;
  }

  .upload-zone h3 {
    font-size: 1.3rem;
  }

  .upload-zone p {
    font-size: 1rem;
  }

  .file-info {
    flex-direction: column;
    align-items: flex-start;
    gap: 10px;
  }

  .import-actions {
    justify-content: center;
  }

  .card-header {
    flex-direction: column;
    align-items: flex-start;
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

  .upload-zone {
    padding: 20px 10px;
  }

  .upload-icon {
    font-size: 2rem;
  }

  .upload-zone h3 {
    font-size: 1.1rem;
  }
}
</style>