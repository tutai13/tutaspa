
<template>
  <div class="warehouse-management">
    <!-- Header -->
    <div class="header">
      <h1 class="title">
        <i class="fas fa-warehouse"></i>
        Quản lý kho
      </h1>
    </div>

    <!-- Notification Section -->
    <div class="card notification-section" v-if="notifications.length > 0">
      <div class="card-header">
        <h2 class="section-title">
          <i class="fas fa-bell"></i>
          Thông báo
        </h2>
      </div>
      <div class="card-body">
        <ul class="notification-list">
          <li v-for="notification in notifications" :key="notification.productId" class="notification-item">
            <i class="fas fa-exclamation-circle"></i>
            {{ notification.message }} (Mã: {{ notification.productCode }}, Tên: {{ notification.productName }})
          </li>
        </ul>
      </div>
    </div>

    <!-- Tabs Xuất/Nhập hàng -->
    <div class="card switch-section">
      <div class="card-header">
        <h2 class="section-title">
          <i class="fas fa-exchange-alt"></i>
          Chọn thao tác
        </h2>
      </div>
      <div class="card-body">
        <div class="view-controls">
          <button @click="switchTab('import')" :class="{ 'btn-info': formTab === 'import', 'btn-outline-primary': formTab !== 'import' }">
            <i class="fas fa-sign-in-alt"></i>
            Nhập hàng
          </button>
          <button @click="switchTab('export')" :class="{ 'btn-info': formTab === 'export', 'btn-outline-primary': formTab !== 'export' }">
            <i class="fas fa-sign-out-alt"></i>
            Xuất hàng
          </button>
        </div>
      </div>
    </div>

    <!-- Form Nhập/Xuất hàng -->
    <div class="card input-section">
      <div class="card-header">
        <h2 class="section-title">
          <i class="fas" :class="formTab === 'import' ? 'fa-sign-in-alt' : 'fa-sign-out-alt'"></i>
          {{ formTab === 'import' ? 'Nhập Hàng' : 'Xuất Hàng' }}
        </h2>
      </div>
      <div class="card-body">
        <transition name="fade">
          <div v-if="formTab === 'import'" class="import-type-selector">
            <div class="button-toggle">
              <button :class="{ active: isNewProduct }" @click="toggleProductType(true)">
                <i class="fas fa-plus"></i>
                Sản phẩm mới
              </button>
              <button :class="{ active: !isNewProduct }" @click="toggleProductType(false)">
                <i class="fas fa-list"></i>
                Sản phẩm đã có
              </button>
            </div>
          </div>
        </transition>

        <transition name="fade">
          <div class="form-grid" :key="formTab + '-' + isNewProduct">
            <template v-if="formTab === 'import'">
              <template v-if="isNewProduct">
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-tag"></i> Tên sản phẩm mới
                    <span class="required">*</span>
                  </label>
                  <input type="text" v-model.trim="newProductName" placeholder="Tên sản phẩm mới" :disabled="isLoading" class="form-control" />
                </div>
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-list"></i> Loại sản phẩm
                    <span class="required">*</span>
                  </label>
                  <select v-model="newProductCategoryId" :disabled="isLoading || !categoryList.length" class="form-control">
                    <option disabled value="">-- Chọn loại sản phẩm --</option>
                    <option v-for="c in categoryList" :key="c.loaiSanPhamId" :value="c.loaiSanPhamId">
                      {{ c.tenLoai }}
                    </option>
                  </select>
                </div>
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-money-bill-wave"></i> Giá bán
                    <span class="required">*</span>
                  </label>
                  <input type="number" v-model.number="formCurrentSellingPrice" placeholder="Giá bán" min="0" :disabled="isLoading" class="form-control" />
                </div>
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-align-left"></i> Mô tả sản phẩm
                  </label>
                  <input type="text" v-model.trim="newProductDescription" placeholder="Mô tả sản phẩm" :disabled="isLoading" class="form-control" />
                </div>
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-image"></i> Ảnh sản phẩm
                  </label>
                  <input type="file" ref="fileInput" @change="handleImageUpload" :disabled="isLoading" accept="image/*" class="form-control file-input" />
                </div>
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-warehouse"></i> Số lượng
                    <span class="required">*</span>
                  </label>
                  <input type="number" v-model.number="formQuantity" placeholder="Số lượng" min="1" :disabled="isLoading" class="form-control" />
                </div>
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-truck"></i> Nhà cung cấp (tùy chọn)
                  </label>
                  <input type="text" v-model.trim="formSupplierName" placeholder="Nhà cung cấp (tùy chọn)" :disabled="isLoading" class="form-control" />
                </div>
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-dollar-sign"></i> Giá nhập (tùy chọn)
                  </label>
                  <input type="number" v-model.number="formImportPrice" placeholder="Giá nhập (tùy chọn)" min="0" :disabled="isLoading" class="form-control" />
                </div>
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-calendar-alt"></i> Ngày sản xuất
                    <span class="required">*</span>
                  </label>
                  <input type="date" v-model="formManufactureDate" placeholder="Ngày sản xuất" :disabled="isLoading" class="form-control" />
                </div>
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-calendar-times"></i> Hạn sử dụng
                    <span class="required">*</span>
                  </label>
                  <input type="date" v-model="formExpirationDate" placeholder="Hạn sử dụng" :disabled="isLoading" class="form-control" />
                </div>
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-sticky-note"></i> Ghi chú (tùy chọn)
                  </label>
                  <input type="text" v-model.trim="formNote" placeholder="Ghi chú (tùy chọn)" :disabled="isLoading" class="form-control" />
                </div>
              </template>
              <template v-else>
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-list"></i> Chọn sản phẩm
                    <span class="required">*</span>
                  </label>
                  <select v-model="formProductId" @change="autoFillSupplier" :disabled="isLoading || !productList.length" class="form-control">
                    <option disabled value="">-- Chọn sản phẩm --</option>
                    <option v-for="p in productList" :key="p.sanPhamId" :value="p.sanPhamId">
                      {{ p.tenSP }} (Số lượng: {{ p.soLuong }})
                    </option>
                  </select>
                </div>
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-warehouse"></i> Số lượng
                    <span class="required">*</span>
                  </label>
                  <input type="number" v-model.number="formQuantity" placeholder="Số lượng" min="1" :disabled="isLoading" class="form-control" />
                </div>
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-truck"></i> Nhà cung cấp (tùy chọn)
                  </label>
                  <input type="text" v-model.trim="formSupplierName" placeholder="Nhà cung cấp (tùy chọn)" :disabled="isLoading" class="form-control" />
                </div>
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-dollar-sign"></i> Giá nhập (tùy chọn)
                  </label>
                  <input type="number" v-model.number="formImportPrice" placeholder="Giá nhập (tùy chọn)" min="0" :disabled="isLoading" class="form-control" />
                </div>
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-calendar-alt"></i> Ngày sản xuất
                    <span class="required">*</span>
                  </label>
                  <input type="date" v-model="formManufactureDate" placeholder="Ngày sản xuất" :disabled="isLoading" class="form-control" />
                </div>
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-calendar-times"></i> Hạn sử dụng
                    <span class="required">*</span>
                  </label>
                  <input type="date" v-model="formExpirationDate" placeholder="Hạn sử dụng" :disabled="isLoading" class="form-control" />
                </div>
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-sticky-note"></i> Ghi chú (tùy chọn)
                  </label>
                  <input type="text" v-model.trim="formNote" placeholder="Ghi chú (tùy chọn)" :disabled="isLoading" class="form-control" />
                </div>
              </template>
            </template>
            <template v-else>
              <div class="form-group">
                <label class="form-label">
                  <i class="fas fa-list"></i> Chọn sản phẩm
                  <span class="required">*</span>
                </label>
                <select v-model="formProductId" :disabled="isLoading || !productList.length" class="form-control">
                  <option disabled value="">-- Chọn sản phẩm --</option>
                  <option v-for="p in productList" :key="p.sanPhamId" :value="p.sanPhamId">
                    {{ p.tenSP }} (Số lượng: {{ p.soLuong }})
                  </option>
                </select>
              </div>
              <div class="form-group">
                <label class="form-label">
                  <i class="fas fa-warehouse"></i> Số lượng
                  <span class="required">*</span>
                </label>
                <input type="number" v-model.number="formQuantity" placeholder="Số lượng" min="1" :disabled="isLoading" class="form-control" />
              </div>
              <div class="form-group">
                <label class="form-label">
                  <i class="fas fa-sticky-note"></i> Ghi chú (tùy chọn)
                </label>
                <input type="text" v-model.trim="formNote" placeholder="Ghi chú (tùy chọn)" :disabled="isLoading" class="form-control" />
              </div>
            </template>
          </div>
        </transition>

        <div class="form-actions">
          <button @click="handleSubmit" :disabled="isLoading || !isFormValid" class="btn btn-success">
            <i class="fas fa-check"></i>
            {{ formTab === 'import' ? 'Xác nhận nhập' : 'Xác nhận xuất' }}
          </button>
          <button @click="resetForm" :disabled="isLoading" class="btn btn-secondary">
            <i class="fas fa-undo"></i>
            Hủy
          </button>
        </div>
        <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
      </div>
    </div>

    <!-- Toast Notifications -->
    <div class="toast-container">
      <div v-for="toast in toasts" :key="toast.id" class="toast" :class="toast.type">
        <i class="fas" :class="getToastIcon(toast.type)"></i>
        {{ toast.message }}
      </div>
    </div>

    <!-- History Section -->
    <div class="card history-section">
      <div class="card-header">
        <h2 class="section-title">
          <i class="fas fa-history"></i>
          Lịch sử giao dịch
        </h2>
        <button @click="refreshHistory" class="btn btn-outline-primary btn-refresh">
          <i class="fas fa-sync-alt"></i>
          Làm mới
        </button>
      </div>
      <div class="card-body">
        <div class="history-tabs">
          <button @click="switchHistoryTab('import')" :class="{ active: historyTab === 'import' }">
            <i class="fas fa-sign-in-alt"></i>
            Nhập
          </button>
          <button @click="switchHistoryTab('export')" :class="{ active: historyTab === 'export' }">
            <i class="fas fa-sign-out-alt"></i>
            Xuất
          </button>
        </div>
        <div class="search-controls">
          <div class="search-group">
            <label>Tìm kiếm theo tên sản phẩm...</label>
            <div class="search-container">
              <input v-model="searchTerm" @input="searchHistory" type="text" class="search-input" placeholder="Nhập tên sản phẩm..." />
              <i class="fas fa-search search-icon"></i>
              <button v-if="searchTerm" class="clear-search show" @click="clearSearch" type="button">
                <i class="fas fa-times"></i>
              </button>
            </div>
          </div>
          <div class="search-group">
            <label>Tìm kiếm theo thời gian</label>
            <input v-model="searchDate" type="date" class="date-input" @input="searchHistory" />
          </div>
        </div>
        <div class="history-table">
          <h3>Lịch sử {{ historyTab === 'import' ? 'Nhập' : 'Xuất' }}</h3>
          <div v-if="isLoading" class="loading-state">
            <i class="fas fa-spinner fa-spin"></i>
            Đang tải...
          </div>
          <div v-else-if="filteredHistory.length === 0" class="empty-state">
            <i class="fas fa-box-open"></i>
            <p>Không có dữ liệu</p>
          </div>
          <div v-else class="history-scroll">
            <table>
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
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, nextTick } from 'vue';
import axios from 'axios';

const formTab = ref('import');
const isNewProduct = ref(true);
const isLoading = ref(false);
const errorMessage = ref('');
const categoryList = ref([]);
const productList = ref([]);
const importHistory = ref([]);
const exportHistory = ref([]);
const searchTerm = ref('');
const searchDate = ref('');
const historyTab = ref('import');
const fileInput = ref(null);
const uploadedFile = ref(null);
const notifications = ref([]);
const toasts = ref([]);

const newProductName = ref('');
const newProductCategoryId = ref('');
const newProductDescription = ref('');
const formCurrentSellingPrice = ref(null);
const formQuantity = ref(null);
const formSupplierName = ref('');
const formImportPrice = ref(null);
const formManufactureDate = ref(null);
const formExpirationDate = ref(null);
const formNote = ref('');
const formProductId = ref(null);

const baseUrl = import.meta.env.VITE_BASE_URL || 'https://localhost:7183/api';

const showToast = (message, type = "info") => {
  const toast = {
    id: Date.now(),
    message,
    type,
  };
  toasts.value.push(toast);
  setTimeout(() => {
    const index = toasts.value.findIndex((t) => t.id === toast.id);
    if (index > -1) toasts.value.splice(index, 1);
  }, 3000);
};

const getToastIcon = (type) => {
  switch (type) {
    case "success":
      return "fa-check-circle";
    case "error":
      return "fa-exclamation-circle";
    case "warning":
      return "fa-exclamation-triangle";
    default:
      return "fa-info-circle";
  }
};

const fetchProducts = async () => {
  try {
    const res = await axios.get(`${baseUrl}/product`);
    productList.value = res.data || [];
    checkNotifications(res.data);
  } catch (err) {
    errorMessage.value = 'Lỗi khi tải danh sách sản phẩm: ' + (err.message || 'Không xác định');
    showToast('Lỗi khi tải danh sách sản phẩm: ' + (err.message || 'Không xác định'), 'error');
    console.error('Fetch products error:', err.response?.data || err);
  }
};

const fetchCategories = async () => {
  try {
    const res = await axios.get(`${baseUrl}/category`);
    categoryList.value = res.data || [];
  } catch (err) {
    errorMessage.value = 'Lỗi khi tải danh sách loại sản phẩm: ' + (err.message || 'Không xác định');
    showToast('Lỗi khi tải danh sách loại sản phẩm: ' + (err.message || 'Không xác định'), 'error');
    console.error('Fetch categories error:', err.response?.data || err);
  }
};

const fetchImportHistory = async () => {
  try {
    const res = await axios.get(`${baseUrl}/inventory/history/imports`);
    importHistory.value = res.data || [];
  } catch (err) {
    errorMessage.value = 'Lỗi khi tải lịch sử nhập: ' + (err.message || 'Không xác định');
    showToast('Lỗi khi tải lịch sử nhập: ' + (err.message || 'Không xác định'), 'error');
    console.error('Fetch import history error:', err.response?.data || err);
  }
};

const fetchExportHistory = async () => {
  try {
    const res = await axios.get(`${baseUrl}/inventory/history/exports`);
    exportHistory.value = res.data || [];
  } catch (err) {
    errorMessage.value = 'Lỗi khi tải lịch sử xuất: ' + (err.message || 'Không xác định');
    showToast('Lỗi khi tải lịch sử xuất: ' + (err.message || 'Không xác định'), 'error');
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
    showToast('Lỗi khi tải dữ liệu: ' + (err.message || 'Không xác định'), 'error');
    console.error('Fetch data error:', err.response?.data || err);
  } finally {
    isLoading.value = false;
  }
};

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

const switchTab = (tab) => {
  formTab.value = tab;
  resetForm();
};

const toggleProductType = async (isNew) => {
  isNewProduct.value = isNew;
  await resetForm();
};

const handleImageUpload = (event) => {
  uploadedFile.value = event.target.files[0];
  if (!uploadedFile.value) {
    errorMessage.value = 'Vui lòng chọn một tệp ảnh để upload.';
    showToast('Vui lòng chọn một tệp ảnh để upload.', 'warning');
  } else {
    errorMessage.value = '';
    console.log('File uploaded:', uploadedFile.value.name);
  }
};

const validateDates = () => {
  if (formManufactureDate.value && formExpirationDate.value) {
    const mDate = new Date(formManufactureDate.value);
    const eDate = new Date(formExpirationDate.value);
    const currentDate = new Date();
    if (mDate > currentDate || eDate < mDate || currentDate > eDate) {
      return 'Ngày sản xuất phải trước ngày hiện tại, hạn sử dụng phải sau ngày sản xuất và hạn sử dụng phải sau ngày hiện tại.';
    }
  }
  return null;
};

const isDuplicateProductName = (name) => {
  return productList.value.some(p => p.tenSP.toLowerCase().trim() === name.toLowerCase().trim());
};

const isFormValid = computed(() => {
  if (formTab.value === 'import') {
    if (isNewProduct.value) {
      return newProductName.value && newProductCategoryId.value && formCurrentSellingPrice.value > 0 && formQuantity.value > 0 && formManufactureDate.value && formExpirationDate.value;
    }
    return formProductId.value && formQuantity.value > 0;
  }
  return formProductId.value && formQuantity.value > 0;
});

const autoFillSupplier = async () => {
  if (formTab.value === 'import' && !isNewProduct.value && formProductId.value) {
    const selectedProduct = productList.value.find(p => p.sanPhamId === formProductId.value);
    if (selectedProduct && !formSupplierName.value) {
      formSupplierName.value = selectedProduct.nhaCungCap || '';
      try {
        const res = await axios.get(`${baseUrl}/ProductBatch/product/${formProductId.value}`);
        const batches = res.data;
        if (batches.length > 0) {
          const latestBatch = batches.sort((a, b) => new Date(b.manufactureDate) - new Date(a.manufactureDate))[0];
          if (!formSupplierName.value && latestBatch.supplierName) {
            formSupplierName.value = latestBatch.supplierName;
          }
        }
      } catch (err) {
        console.error('Không thể lấy thông tin lô hàng gần nhất', err);
      }
    }
  }
};

const handleSubmit = async () => {
  const dateError = validateDates();
  if (dateError) {
    errorMessage.value = dateError;
    showToast(dateError, 'error');
    return;
  }

  if (formTab.value === 'import' && isNewProduct.value) {
    if (!newProductName.value.trim() || !newProductCategoryId.value || !formCurrentSellingPrice.value ||
        !newProductDescription.value.trim() || !formQuantity.value || !uploadedFile.value) {
      errorMessage.value = 'Vui lòng điền đầy đủ thông tin bắt buộc.';
      showToast('Vui lòng điền đầy đủ thông tin bắt buộc.', 'warning');
      return;
    }
    if (isDuplicateProductName(newProductName.value)) {
      errorMessage.value = 'Sản phẩm đã tồn tại, vui lòng nhập thêm số lượng ở phần "Sản phẩm đã có"';
      showToast('Sản phẩm đã tồn tại, vui lòng nhập thêm số lượng ở phần "Sản phẩm đã có"', 'warning');
      return;
    }
    if (formCurrentSellingPrice.value <= 0 || formQuantity.value <= 0) {
      errorMessage.value = 'Giá bán và số lượng phải lớn hơn 0.';
      showToast('Giá bán và số lượng phải lớn hơn 0.', 'warning');
      return;
    }
  } else if (formTab.value === 'import' && !isNewProduct.value) {
    if (!formProductId.value || !formQuantity.value) {
      errorMessage.value = 'Vui lòng chọn sản phẩm và nhập số lượng.';
      showToast('Vui lòng chọn sản phẩm và nhập số lượng.', 'warning');
      return;
    }
    if (formQuantity.value <= 0) {
      errorMessage.value = 'Số lượng phải lớn hơn 0.';
      showToast('Số lượng phải lớn hơn 0.', 'warning');
      return;
    }
    const selectedProduct = productList.value.find(p => p.sanPhamId === formProductId.value);
    if (!formImportPrice.value && selectedProduct?.giaNhap) {
      formImportPrice.value = selectedProduct.giaNhap;
    }
    if (!formSupplierName.value && selectedProduct?.nhaCungCap) {
      formSupplierName.value = selectedProduct.nhaCungCap;
    }
    try {
      const res = await axios.get(`${baseUrl}/ProductBatch/product/${formProductId.value}`);
      const batches = res.data;
      if (batches.length > 0) {
        const latestBatch = batches.sort((a, b) => new Date(b.manufactureDate) - new Date(a.manufactureDate))[0];
        if (!formImportPrice.value && latestBatch.importPrice) {
          formImportPrice.value = latestBatch.importPrice;
        }
        if (!formSupplierName.value && latestBatch.supplierName) {
          formSupplierName.value = latestBatch.supplierName;
        }
      }
    } catch (err) {
      console.error('Không thể lấy thông tin lô hàng gần nhất', err);
    }
  } else if (formTab.value === 'export') {
    if (!formProductId.value || !formQuantity.value) {
      errorMessage.value = 'Vui lòng chọn sản phẩm và nhập số lượng.';
      showToast('Vui lòng chọn sản phẩm và nhập số lượng.', 'warning');
      return;
    }
    const selectedProduct = productList.value.find(p => p.sanPhamId === formProductId.value);
    if (selectedProduct.soLuong < formQuantity.value) {
      errorMessage.value = 'Số lượng xuất vượt quá số lượng tồn kho.';
      showToast('Số lượng xuất vượt quá số lượng tồn kho.', 'warning');
      return;
    }
    if (formQuantity.value <= 0) {
      errorMessage.value = 'Số lượng phải lớn hơn 0.';
      showToast('Số lượng phải lớn hơn 0.', 'warning');
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
    } else {
      const payload = {
        ProductId: formProductId.value,
        Quantity: formQuantity.value,
        Note: formNote.value.trim() || ''
      };
      await axios.post(apiUrl, payload, { headers: { 'Content-Type': 'application/json' } });
    }
    showToast(`${formTab.value === 'import' ? 'Nhập hàng' : 'Xuất hàng'} thành công!`, 'success');
    await fetchData();
    resetForm();
  } catch (err) {
    errorMessage.value = `Lỗi: ${err.response?.data?.message || err.message || 'Không xác định'}`;
    showToast(`Lỗi: ${err.response?.data?.message || err.message || 'Không xác định'}`, 'error');
    console.error('Submit error:', err.response?.data || err);
  } finally {
    isLoading.value = false;
    if (fileInput.value) fileInput.value.value = '';
    uploadedFile.value = null;
  }
};

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

const switchHistoryTab = (tab) => {
  historyTab.value = tab;
};

const refreshHistory = async () => {
  searchTerm.value = '';
  searchDate.value = '';
  await fetchData();
};

const clearSearch = () => {
  searchTerm.value = '';
  searchHistory();
};

const searchHistory = () => {
  // Trigger filtering by updating the reactive searchDate and searchTerm
};

const filteredHistory = computed(() => {
  const keyword = searchTerm.value?.trim().toLowerCase() || '';
  const dateFilter = searchDate.value ? new Date(searchDate.value) : null;
  const list = historyTab.value === 'import' ? importHistory.value : exportHistory.value;

  return list.filter((item) => {
    const matchesName = !keyword || 
      (item.productName || '').toLowerCase().includes(keyword) || 
      (item.productCode || '').toLowerCase().includes(keyword) || 
      (item.note || '').toLowerCase().includes(keyword);
    
    const itemDate = item.timestamp ? new Date(item.timestamp) : null;
    const matchesDate = !dateFilter || 
      (itemDate && 
       itemDate.getFullYear() === dateFilter.getFullYear() && 
       itemDate.getMonth() === dateFilter.getMonth() && 
       itemDate.getDate() === dateFilter.getDate());
    
    return matchesName && matchesDate;
  });
});

const formatDate = (date) => {
  if (!date) return 'N/A';
  return new Date(date).toLocaleString('vi-VN', { dateStyle: 'short', timeStyle: 'short' });
};

onMounted(() => {
  fetchData();
});
</script>

<style scoped>
.warehouse-management {
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
  color: #e67e22;
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

.notification-list {
  list-style: none;
  padding: 0;
}

.notification-item {
  padding: 12px 20px;
  border-radius: 8px;
  font-size: 0.95rem;
  color: white;
  display: flex;
  align-items: center;
  gap: 10px;
  background: #e74c3c;
  margin-bottom: 10px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.view-controls {
  display: flex;
  gap: 10px;
}

.button-toggle {
  display: flex;
  gap: 10px;
  margin-bottom: 20px;
}

.button-toggle button {
  padding: 10px 20px;
  border: 2px solid #e1e8ed;
  border-radius: 8px;
  background: #fafbfc;
  cursor: pointer;
  transition: all 0.3s ease;
  color: #2c3e50;
}

.button-toggle button.active {
  background: linear-gradient(135deg, #3498db 0%, #2980b9 100%);
  color: white;
  border-color: #2980b9;
}

.button-toggle button:hover:not(.active) {
  background: #f0f0f0;
  border-color: #3498db;
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

.form-label {
  font-weight: 600;
  color: #2c3e50;
  margin-bottom: 8px;
  font-size: 0.95rem;
  display: flex;
  align-items: center;
  gap: 8px;
}

.form-label i {
  color: #3498db;
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

.form-control:disabled {
  background: #e1e8ed;
  cursor: not-allowed;
}

.file-input {
  padding: 10px;
}

.form-actions {
  display: flex;
  gap: 15px;
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

.btn-info {
  background: linear-gradient(135deg, #3498db, #2980b9);
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

.btn-refresh {
  padding: 8px 16px;
  font-size: 0.9rem;
}

.error-message {
  color: #e74c3c;
  margin-top: 10px;
}

.history-tabs {
  display: flex;
  gap: 10px;
  margin-bottom: 20px;
}

.history-tabs button {
  padding: 10px 20px;
  background: #f0f0f0;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 600;
  transition: all 0.3s ease;
  color: #2c3e50;
}

.history-tabs button.active {
  background: linear-gradient(135deg, #3498db 0%, #2980b9 100%);
  color: white;
}

.history-tabs button:hover:not(.active) {
  background: #e0e0e0;
  color: #3498db;
}

.search-controls {
  display: flex;
  gap: 10px;
  margin-bottom: 20px;
  align-items: flex-start;
}

.search-group {
  flex: 1;
  display: flex;
  flex-direction: column;
}

.search-group label {
  font-weight: 600;
  color: #2c3e50;
  margin-bottom: 5px;
  display: block;
}

.search-container {
  position: relative;
}

.search-input, .date-input {
  width: 100%;
  padding: 12px 45px 12px 15px;
  border: 2px solid #e1e8ed;
  border-radius: 25px;
  font-size: 1rem;
  background: #fafbfc;
  color: #2c3e50;
  transition: all 0.3s ease;
}

.search-input:focus, .date-input:focus {
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
  color: rgba(0, 0, 0, 0.8);
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

.clear-search.show {
  display: inline-block;
}

.history-table h3 {
  margin-bottom: 10px;
  font-size: 1.25rem;
  color: #2c3e50;
}

table {
  width: 100%;
  border-collapse: collapse;
  background: white;
  border-radius: 10px;
  overflow: hidden;
}

thead {
  background: #f7f9fb;
  color: #34495e;
}

th,
td {
  padding: 14px 16px;
  border-bottom: 1px solid #e1e8ed;
  text-align: left;
  font-size: 0.95rem;
}

tr:hover {
  background: #f0f8ff;
}

.history-scroll {
  max-height: 300px;
  overflow-y: auto;
}

.loading-state,
.empty-state {
  text-align: center;
  padding: 40px 20px;
  color: #7f8c8d;
  font-size: 1.1rem;
}

.loading-state i {
  font-size: 2.5rem;
  margin-bottom: 10px;
}

.empty-state i {
  font-size: 2.5rem;
  margin-bottom: 15px;
  color: #bdc3c7;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease, transform 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
  transform: translateY(10px);
}

.toast-container {
  position: fixed;
  bottom: 20px;
  right: 20px;
  z-index: 9999;
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.toast {
  padding: 12px 20px;
  border-radius: 8px;
  font-size: 0.95rem;
  color: white;
  display: flex;
  align-items: center;
  gap: 10px;
  animation: fadeInOut 3s ease forwards;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.toast.info {
  background: #3498db;
}

.toast.success {
  background: #2ecc71;
}

.toast.warning {
  background: #f39c12;
}

.toast.error {
  background: #e74c3c;
}

@keyframes fadeInOut {
  0% {
    opacity: 0;
    transform: translateY(20px);
  }
  10% {
    opacity: 1;
    transform: translateY(0);
  }
  90% {
    opacity: 1;
    transform: translateY(0);
  }
  100% {
    opacity: 0;
    transform: translateY(20px);
  }
}

@media (max-width: 768px) {
  .warehouse-management {
    padding: 15px;
  }

  .title {
    font-size: 2rem;
  }

  .form-grid {
    grid-template-columns: 1fr;
  }

  .view-controls,
  .button-toggle,
  .history-tabs,
  .search-controls {
    flex-direction: column;
    align-items: stretch;
  }

  .view-controls button,
  .button-toggle button,
  .history-tabs button,
  .search-input,
  .date-input {
    width: 100%;
    margin-bottom: 10px;
  }

  table th,
  table td {
    padding: 10px 8px;
    font-size: 0.9rem;
  }
}

@media (max-width: 480px) {
  .form-control {
    font-size: 0.9rem;
    padding: 10px;
  }

  .btn {
    padding: 10px 16px;
    font-size: 0.9rem;
  }
}
</style>