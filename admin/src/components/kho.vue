<template>
  <div class="warehouse-container">
    <!-- Thông báo sản phẩm sắp hết hạn và hết hạn -->
    <div class="section notification-section" v-if="notifications.length > 0">
      <h3 class="notification-title">Thông báo</h3>
      <ul class="notification-list">
        <li v-for="notification in notifications" :key="notification.productId" class="notification-item">
          {{ notification.message }} (Mã: {{ notification.productCode }}, Tên: {{ notification.productName }})
        </li>
      </ul>
    </div>

    <!-- Tabs Xuất/Nhập hàng -->
    <div class="section switch-section">
      <button @click="switchTab('import')" :class="{ active: formTab === 'import' }">
        Nhập hàng
      </button>
      <button @click="switchTab('export')" :class="{ active: formTab === 'export' }">
        Xuất hàng
      </button>
    </div>

    <!-- Form Nhập/Xuất hàng -->
    <div class="section input-section">
      <h2>{{ formTab === 'import' ? 'Nhập Hàng' : 'Xuất Hàng' }}</h2>

      <transition name="fade">
        <div v-if="formTab === 'import'" class="import-type-selector">
          <div class="button-toggle">
            <button :class="{ active: isNewProduct }" @click="toggleProductType(true)">Sản phẩm mới</button>
            <button :class="{ active: !isNewProduct }" @click="toggleProductType(false)">Sản phẩm đã có</button>
          </div>
        </div>
      </transition>

      <transition name="fade">
        <div class="form-grid" :key="formTab + '-' + isNewProduct">
          <template v-if="formTab === 'import'">
            <template v-if="isNewProduct">
              <input type="text" v-model.trim="newProductName" placeholder="Tên sản phẩm mới" :disabled="isLoading" />
              <select v-model="newProductCategoryId" :disabled="isLoading || !categoryList.length">
                <option disabled value="">-- Chọn loại sản phẩm --</option>
                <option v-for="c in categoryList" :key="c.loaiSanPhamId" :value="c.loaiSanPhamId">
                  {{ c.tenLoai }}
                </option>
              </select>
              <input type="number" v-model.number="formCurrentSellingPrice" placeholder="Giá bán" min="0" :disabled="isLoading" />
              <input type="text" v-model.trim="newProductDescription" placeholder="Mô tả sản phẩm" :disabled="isLoading" />
              <input type="file" ref="fileInput" @change="handleImageUpload" :disabled="isLoading" accept="image/*" />
              <input type="number" v-model.number="formQuantity" placeholder="Số lượng" min="1" :disabled="isLoading" />
              <input type="text" v-model.trim="formSupplierName" placeholder="Nhà cung cấp (tùy chọn)" :disabled="isLoading" />
              <input type="number" v-model.number="formImportPrice" placeholder="Giá nhập (tùy chọn)" min="0" :disabled="isLoading" />
              <input type="date" v-model="formManufactureDate" placeholder="Ngày sản xuất" :disabled="isLoading" />
              <input type="date" v-model="formExpirationDate" placeholder="Hạn sử dụng" :disabled="isLoading" />
              <input type="text" v-model.trim="formNote" placeholder="Ghi chú (tùy chọn)" :disabled="isLoading" />
            </template>
            <template v-else>
              <select v-model="formProductId" :disabled="isLoading || !productList.length">
                <option disabled value="">-- Chọn sản phẩm --</option>
                <option v-for="p in productList" :key="p.sanPhamId" :value="p.sanPhamId">
                  {{ p.tenSP }} (Số lượng: {{ p.soLuong }})
                </option>
              </select>
              <input type="number" v-model.number="formQuantity" placeholder="Số lượng" min="1" :disabled="isLoading" />
              <input type="text" v-model.trim="formSupplierName" placeholder="Nhà cung cấp (tùy chọn)" :disabled="isLoading" />
              <input type="number" v-model.number="formImportPrice" placeholder="Giá nhập (tùy chọn)" min="0" :disabled="isLoading" />
              <input type="date" v-model="formManufactureDate" placeholder="Ngày sản xuất" :disabled="isLoading" />
              <input type="date" v-model="formExpirationDate" placeholder="Hạn sử dụng" :disabled="isLoading" />
              <input type="text" v-model.trim="formNote" placeholder="Ghi chú (tùy chọn)" :disabled="isLoading" />
            </template>
          </template>
          <template v-if="formTab === 'export'">
            <select v-model="formProductId" :disabled="isLoading || !productList.length">
              <option disabled value="">-- Chọn sản phẩm --</option>
              <option v-for="p in productList" :key="p.sanPhamId" :value="p.sanPhamId">
                {{ p.tenSP }} (Số lượng: {{ p.soLuong }})
              </option>
            </select>
            <input type="number" v-model.number="formQuantity" placeholder="Số lượng" min="1" :disabled="isLoading" />
            <input type="text" v-model.trim="formNote" placeholder="Ghi chú" :disabled="isLoading" />
          </template>
          <button class="action-button" @click="handleSubmit" :disabled="isLoading">
            {{ isLoading ? 'Đang xử lý...' : formTab === 'import' ? 'Nhập hàng' : 'Xuất hàng' }}
          </button>
        </div>
      </transition>

      <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>
    </div>

    <!-- Tabs Lịch sử -->
    <div class="section history-section">
      <h2>Lịch sử Xuất Nhập Hàng</h2>
      <div class="history-tabs">
        <button @click="activeTab = 'import'" :class="{ active: activeTab === 'import' }">
          Lịch sử Nhập
        </button>
        <button @click="activeTab = 'export'" :class="{ active: activeTab === 'export' }">
          Lịch sử Xuất
        </button>
      </div>
      <input type="text" v-model.trim="searchTerm" placeholder="Tìm kiếm theo mã hoặc tên sản phẩm hoặc ghi chú..." class="search-input" :disabled="isLoading" @input="searchHistory" />
      <div class="history-table">
        <h3>{{ activeTab === 'import' ? 'Lịch sử Nhập' : 'Lịch sử Xuất' }}</h3>
        <div v-if="isLoading" class="loading">Đang tải...</div>
        <table v-else>
          <thead>
            <tr>
              <th>Tên sản phẩm</th>
              <th>Mã sản phẩm</th>
              <th>ID Lô</th>
              <th>Số lượng</th>
              <th>Nhà cung cấp</th>
              <th>Giá nhập</th>
              <th>NSX</th>
              <th>HSD</th>
              <th>Ghi chú</th>
              <th>Thời gian</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in filteredHistory" :key="item.id">
              <td>{{ item.productName || 'N/A' }}</td>
              <td>{{ item.productCode || 'N/A' }}</td>
              <td>{{ item.batchId || 'N/A' }}</td>
              <td>{{ Math.abs(item.quantityChanged) }}</td>
              <td>{{ item.supplierName || 'N/A' }}</td>
              <td>{{ item.importPrice?.toLocaleString('vi-VN') || '-' }}</td>
              <td>{{ formatDate(item.manufactureDate) }}</td>
              <td>{{ formatDate(item.expirationDate) }}</td>
              <td>{{ item.note || 'Không có' }}</td>
              <td>{{ formatDate(item.timestamp) }}</td>
            </tr>
            <tr v-if="!filteredHistory.length">
              <td colspan="10" class="no-data">Không có dữ liệu</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, nextTick } from 'vue';
import axios from 'axios';

const formTab = ref('import');
const activeTab = ref('import');
const isNewProduct = ref(true);
const formProductId = ref(null);
const formQuantity = ref(null);
const formSupplierName = ref('');
const formImportPrice = ref(null);
const formCurrentSellingPrice = ref(null);
const formManufactureDate = ref(null);
const formExpirationDate = ref(null);
const formNote = ref('');
const newProductName = ref('');
const newProductCategoryId = ref('');
const newProductDescription = ref('');
const productList = ref([]);
const categoryList = ref([]);
const importHistory = ref([]);
const exportHistory = ref([]);
const searchTerm = ref('');
const isLoading = ref(false);
const errorMessage = ref('');
const fileInput = ref(null);
const uploadedFile = ref(null);
const baseUrl = import.meta.env.VITE_BASE_URL || 'https://localhost:7183/api';
const notifications = ref([]);

// Fetch dữ liệu
const fetchProducts = async () => {
  try {
    const res = await axios.get(`${baseUrl}/product`);
    productList.value = res.data || [];
    checkNotifications(res.data);
  } catch (err) {
    errorMessage.value = 'Lỗi khi tải danh sách sản phẩm: ' + (err.message || 'Không xác định');
    console.error('Fetch products error:', err.response?.data || err);
  }
};

const fetchCategories = async () => {
  try {
    const res = await axios.get(`${baseUrl}/category`);
    categoryList.value = res.data || [];
    if (!categoryList.value.length) {
      errorMessage.value = 'Không có danh mục sản phẩm nào được tải. Vui lòng thêm danh mục trong hệ thống.';
    }
  } catch (err) {
    errorMessage.value = 'Lỗi khi tải danh sách loại sản phẩm: ' + (err.message || 'Không xác định');
    console.error('Fetch categories error:', err.response?.data || err);
  }
};

const fetchImportHistory = async () => {
  try {
    const res = await axios.get(`${baseUrl}/inventory/history/imports`);
    importHistory.value = res.data || [];
  } catch (err) {
    errorMessage.value = 'Lỗi khi tải lịch sử nhập: ' + (err.message || 'Không xác định');
    console.error('Fetch import history error:', err.response?.data || err);
  }
};

const fetchExportHistory = async () => {
  try {
    const res = await axios.get(`${baseUrl}/inventory/history/exports`);
    exportHistory.value = res.data || [];
  } catch (err) {
    errorMessage.value = 'Lỗi khi tải lịch sử xuất: ' + (err.message || 'Không xác định');
    console.error('Fetch export history error:', err.response?.data || err);
  }
};

const fetchData = async () => {
  isLoading.value = true;
  errorMessage.value = '';
  try {
    await Promise.all([fetchProducts(), fetchCategories(), fetchImportHistory(), fetchExportHistory()]);
  } catch (err) {
    errorMessage.value = 'Lỗi khi tải dữ liệu: ' + (err.message || 'Không xác định');
    console.error('Fetch data error:', err.response?.data || err);
  } finally {
    isLoading.value = false;
  };
};

// Kiểm tra thông báo sản phẩm sắp hết hạn và hết hạn
const checkNotifications = (products) => {
  const now = new Date();
  const twoMonthsLater = new Date(now.setMonth(now.getMonth() + 2));
  notifications.value = [];

  products.forEach(p => {
    if (p.expirationDate) {
      const expDate = new Date(p.expirationDate);
      const timeDiff = expDate - now;
      const daysLeft = Math.ceil(timeDiff / (1000 * 60 * 60 * 24));

      if (expDate < now) {
        notifications.value.push({
          productId: p.sanPhamId,
          productCode: p.productCode || 'N/A',
          productName: p.tenSP,
          message: 'Sản phẩm đã hết hạn sử dụng'
        });
      } else if (daysLeft <= 60) {
        notifications.value.push({
          productId: p.sanPhamId,
          productCode: p.productCode || 'N/A',
          productName: p.tenSP,
          message: `Sản phẩm chỉ còn ${daysLeft} ngày (dưới 2 tháng)`
        });
      }
    }
  });
};

// Lọc lịch sử
const filteredHistory = computed(() => {
  const keyword = searchTerm.value?.trim().toLowerCase() || '';
  const list = activeTab.value === 'import' ? importHistory.value : exportHistory.value;
  if (!keyword) return list;
  return list.filter(
    (item) =>
      (item.productName || '').toLowerCase().includes(keyword) ||
      (item.productCode || '').toLowerCase().includes(keyword) ||
      (item.note || '').toLowerCase().includes(keyword)
  );
});

// Xử lý upload ảnh
const handleImageUpload = (event) => {
  uploadedFile.value = event.target.files[0];
  if (!uploadedFile.value) {
    errorMessage.value = 'Vui lòng chọn một tệp ảnh để upload.';
  } else {
    errorMessage.value = '';
    console.log('File uploaded:', uploadedFile.value.name);
  }
};

// Validate ngày tháng
const validateDates = () => {
  if (formManufactureDate.value && formExpirationDate.value) {
    const mDate = new Date(formManufactureDate.value);
    const eDate = new Date(formExpirationDate.value);
    const currentDate = new Date();
    if (mDate > currentDate || eDate < mDate) {
      return 'Ngày sản xuất phải trước ngày hiện tại và hạn sử dụng phải sau ngày sản xuất.';
    }
  }
  return null;
};

// Xử lý submit
const handleSubmit = async () => {
  const dateError = validateDates();
  if (dateError) {
    errorMessage.value = dateError;
    return;
  }

  if (formTab.value === 'import' && isNewProduct.value) {
    if (!newProductName.value.trim() || !newProductCategoryId.value || !formCurrentSellingPrice.value ||
        !newProductDescription.value.trim() || !formQuantity.value || !uploadedFile.value) {
      errorMessage.value = 'Vui lòng điền đầy đủ thông tin bắt buộc.';
      return;
    }
    if (formCurrentSellingPrice.value <= 0 || formQuantity.value <= 0) {
      errorMessage.value = 'Giá bán và số lượng phải lớn hơn 0.';
      return;
    }
  } else if (formTab.value === 'import' && !isNewProduct.value) {
    if (!formProductId.value || !formQuantity.value) {
      errorMessage.value = 'Vui lòng chọn sản phẩm và nhập số lượng.';
      return;
    }
    if (formQuantity.value <= 0) {
      errorMessage.value = 'Số lượng phải lớn hơn 0.';
      return;
    }
  } else if (formTab.value === 'export') {
    if (!formProductId.value || !formQuantity.value) {
      errorMessage.value = 'Vui lòng chọn sản phẩm và nhập số lượng.';
      return;
    }
    const selectedProduct = productList.value.find(p => p.sanPhamId === formProductId.value);
    if (selectedProduct.soLuong < formQuantity.value) {
      errorMessage.value = 'Số lượng xuất vượt quá số lượng tồn kho.';
      return;
    }
    if (formQuantity.value <= 0) {
      errorMessage.value = 'Số lượng phải lớn hơn 0.';
      return;
    }
  }

  isLoading.value = true;
  errorMessage.value = '';
  const apiUrl = formTab.value === 'import'
    ? `${baseUrl}/inventory/import-with-batch`
    : `${baseUrl}/inventory/export-with-batch`; 

  try {
    if (formTab.value === 'import' && isNewProduct.value) {
      const formData = new FormData();
      formData.append('ProductName', newProductName.value.trim());
      formData.append('Description', newProductDescription.value.trim());
      formData.append('CategoryId', newProductCategoryId.value);
      formData.append('CurrentSellingPrice', formCurrentSellingPrice.value);
      formData.append('SupplierName', formSupplierName.value.trim() || '');
      formData.append('ImportPrice', formImportPrice.value || 0);
      formData.append('Quantity', formQuantity.value);
      formData.append('ManufactureDate', formManufactureDate.value);
      formData.append('ExpirationDate', formExpirationDate.value);
      formData.append('Image', uploadedFile.value);
      formData.append('Note', formNote.value.trim() || '');

      await axios.post(apiUrl, formData, { headers: { 'Content-Type': 'multipart/form-data' } });
      alert('Nhập hàng thành công.');
    } else if (formTab.value === 'import' && !isNewProduct.value) {
      const payload = {
        ProductId: formProductId.value,
        ImportPrice: formImportPrice.value || 0,
        Quantity: formQuantity.value,
        SupplierName: formSupplierName.value.trim() || '',
        ManufactureDate: formManufactureDate.value,
        ExpiryDate: formExpirationDate.value,
        Note: formNote.value.trim() || ''
      };
      await axios.post(`${baseUrl}/productbatch`, payload, { headers: { 'Content-Type': 'application/json' } });
      alert('Nhập lô hàng thành công.');
    } else {
      const payload = {
        ProductId: formProductId.value,
        Quantity: formQuantity.value,
        Note: formNote.value.trim() || ''
      };
      console.log('Export payload:', payload); // Log để kiểm tra dữ liệu gửi đi
      await axios.post(apiUrl, payload, { headers: { 'Content-Type': 'application/json' } });
      alert('Xuất hàng thành công.');
    }
    await fetchData();
    resetForm();
  } catch (err) {
    errorMessage.value = `Lỗi: ${err.response?.data?.message || err.message || 'Không xác định'}`;
    console.error('Submit error:', err.response?.data || err);
  } finally {
    isLoading.value = false;
    if (fileInput.value) fileInput.value.value = '';
    uploadedFile.value = null;
  }
};

// Reset form
const resetForm = async () => {
  formProductId.value = null;
  formQuantity.value = null;
  formSupplierName.value = '';
  formImportPrice.value = null;
  formCurrentSellingPrice.value = null;
  formManufactureDate.value = null;
  formExpirationDate.value = null;
  formNote.value = '';
  newProductName.value = '';
  newProductCategoryId.value = '';
  newProductDescription.value = '';
  await nextTick();
  if (fileInput.value) fileInput.value.value = '';
  uploadedFile.value = null;
  errorMessage.value = '';
};

// Chuyển đổi tab Nhập/Xuất
const switchTab = (tab) => {
  formTab.value = tab;
  resetForm();
};

// Chuyển đổi loại sản phẩm (mới/đã có)
const toggleProductType = async (isNew) => {
  isNewProduct.value = isNew;
  await resetForm();
};

// Tìm kiếm lịch sử
const searchHistory = () => {
  // Trigger filtering when input changes
};

// Format ngày
const formatDate = (date) => {
  if (!date) return 'N/A';
  return new Date(date).toLocaleString('vi-VN', { dateStyle: 'short', timeStyle: 'short' });
};

// Khởi động
onMounted(() => {
  fetchData();
});
</script>

<style scoped>
.warehouse-container {
  max-width: 1000px;
  margin: 40px auto;
  padding: 20px;
  font-family: "Segoe UI", sans-serif;
  background: #f9f9f9;
  border-radius: 12px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
}

.section {
  margin-bottom: 20px;
}

.notification-section {
  background-color: #fff3cd;
  border-left: 4px solid #856404;
  padding: 15px;
  border-radius: 8px;
  margin-bottom: 20px;
}

.notification-title {
  font-size: 1.2rem;
  font-weight: 600;
  color: #856404;
  margin-bottom: 10px;
}

.notification-list {
  list-style: none;
  padding: 0;
}

.notification-item {
  color: #856404;
  margin-bottom: 5px;
}

.switch-section {
  display: flex;
  justify-content: center;
  gap: 10px;
  margin-bottom: 20px;
}

.switch-section button {
  padding: 10px 20px;
  background: #dee2e6;
  border: none;
  border-radius: 8px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
  z-index: 1;
}

.switch-section button:hover {
  background: #c6c9cc;
  transform: scale(1.05);
}

.switch-section button.active {
  background: #007bff;
  color: white;
  transform: scale(1.05);
  box-shadow: 0 2px 8px rgba(0, 123, 255, 0.4);
}

.switch-section button::after {
  content: '';
  position: absolute;
  top: 50%;
  left: 50%;
  width: 0;
  height: 0;
  background: rgba(255, 255, 255, 0.3);
  border-radius: 50%;
  transform: translate(-50%, -50%);
  transition: width 0.4s ease, height 0.4s ease;
  z-index: -1;
}

.switch-section button:hover::after {
  width: 200%;
  height: 200%;
}

.button-toggle {
  display: flex;
  gap: 10px;
  margin-bottom: 15px;
}

.button-toggle button {
  padding: 8px 16px;
  background: #e9ecef;
  border: none;
  border-radius: 8px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
  z-index: 1;
}

.button-toggle button:hover {
  background: #d3d6da;
  transform: scale(1.05);
}

.button-toggle button.active {
  background: #007bff;
  color: white;
  box-shadow: 0 2px 8px rgba(0, 123, 255, 0.4);
  transform: scale(1.05);
}

.button-toggle button::after {
  content: '';
  position: absolute;
  top: 50%;
  left: 50%;
  width: 0;
  height: 0;
  background: rgba(255, 255, 255, 0.3);
  border-radius: 50%;
  transform: translate(-50%, -50%);
  transition: width 0.4s ease, height 0.4s ease;
  z-index: -1;
}

.button-toggle button:hover::after {
  width: 200%;
  height: 200%;
}

.input-section h2 {
  margin-bottom: 15px;
  font-size: 1.5rem;
}

.form-grid {
  display: flex;
  gap: 12px;
  margin-bottom: 20px;
  flex-wrap: wrap;
}

input,
select {
  padding: 10px;
  border-radius: 8px;
  border: 1px solid #ccc;
  flex: 1;
  min-width: 150px;
  transition: border-color 0.3s ease;
}

input:focus,
select:focus {
  border-color: #007bff;
  outline: none;
}

.action-button {
  background-color: #28a745;
  color: white;
  padding: 10px 16px;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 600;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
  z-index: 1;
}

.action-button:hover:not(:disabled) {
  background-color: #218838;
  transform: scale(1.05);
  box-shadow: 0 2px 8px rgba(33, 136, 56, 0.4);
}

.action-button:disabled {
  background-color: #6c757d;
  cursor: not-allowed;
}

.action-button::after {
  content: '';
  position: absolute;
  top: 50%;
  left: 50%;
  width: 0;
  height: 0;
  background: rgba(255, 255, 255, 0.3);
  border-radius: 50%;
  transform: translate(-50%, -50%);
  transition: width 0.4s ease, height 0.4s ease;
  z-index: -1;
}

.action-button:hover::after {
  width: 200%;
  height: 200%;
}

.error-message {
  color: #dc3545;
  margin-top: 10px;
}

.history-tabs {
  display: flex;
  gap: 10px;
  margin: 20px 0;
}

.history-tabs button {
  padding: 10px 20px;
  background-color: #e9ecef;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 600;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
  z-index: 1;
}

.history-tabs button:hover {
  background-color: #d3d6da;
  transform: scale(1.05);
}

.history-tabs button.active {
  background-color: #007bff;
  color: white;
  box-shadow: 0 2px 8px rgba(0, 123, 255, 0.4);
  transform: scale(1.05);
}

.history-tabs button::after {
  content: '';
  position: absolute;
  top: 50%;
  left: 50%;
  width: 0;
  height: 0;
  background: rgba(255, 255, 255, 0.3);
  border-radius: 50%;
  transform: translate(-50%, -50%);
  transition: width 0.4s ease, height 0.4s ease;
  z-index: -1;
}

.history-tabs button:hover::after {
  width: 200%;
  height: 200%;
}

.search-input {
  margin-bottom: 12px;
  padding: 8px 12px;
  width: 100%;
  border-radius: 8px;
  border: 1px solid #ccc;
  box-sizing: border-box;
  transition: border-color 0.3s ease;
}

.search-input:focus {
  border-color: #007bff;
  outline: none;
}

.history-table h3 {
  margin-bottom: 10px;
  font-size: 1.25rem;
}

table {
  width: 100%;
  border-collapse: collapse;
  background: white;
  border-radius: 8px;
  overflow: hidden;
}

thead {
  background-color: #007bff;
  color: white;
}

th,
td {
  padding: 12px 16px;
  border-bottom: 1px solid #ddd;
  text-align: left;
}

tr:hover {
  background: #f1f1f1;
}

.no-data {
  text-align: center;
  padding: 20px;
  color: #6c757d;
}

.loading {
  text-align: center;
  padding: 20px;
  color: #007bff;
}

/* Hiệu ứng transition cho form */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease, transform 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
  transform: translateY(10px);
}

@media (max-width: 600px) {
  .form-grid {
    flex-direction: column;
  }

  .switch-section,
  .button-toggle,
  .history-tabs {
    flex-direction: column;
    align-items: center;
  }

  .switch-section button,
  .button-toggle button,
  .history-tabs button {
    width: 100%;
    max-width: 200px;
  }
}
</style>