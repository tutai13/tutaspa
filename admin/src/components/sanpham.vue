```vue
<template>
  <div class="product-management">
    <!-- Header -->
    <div class="header">
      <h1 class="title">
        <i class="fas fa-box"></i>
        Quản lý sản phẩm
      </h1>
    </div>

    <!-- Filters and Search Section -->
    <div class="card filters-section">
      <div class="card-header">
        <h2 class="section-title">
          <i class="fas fa-filter"></i>
          Tìm kiếm & Lọc sản phẩm
        </h2>
        <div class="list-controls">
          <button class="btn btn-outline-primary" @click="fetchProducts">
            <i class="fas fa-sync-alt"></i>
            Làm mới
          </button>
        </div>
      </div>
      <div class="card-body">
        <div class="form-grid">
          <!-- Search by name -->
          <div class="form-group">
            <label class="form-label">
              <i class="fas fa-search"></i> Tìm theo tên sản phẩm
            </label>
            <div class="search-container">
              <input
                v-model="searchName"
                @input="searchByName"
                type="text"
                class="search-input"
                placeholder="Nhập tên sản phẩm..."
              />
              <i class="fas fa-search search-icon"></i>
              <button
                v-if="searchName"
                class="clear-search show"
                @click="clearSearch"
                type="button"
              >
                <i class="fas fa-times"></i>
              </button>
            </div>
          </div>

          <!-- Price filter -->
          <div class="form-group">
            <label class="form-label">
              <i class="fas fa-dollar-sign"></i> Lọc theo giá
            </label>
            <div class="price-filter-group">
              <input
                v-model.number="priceMin"
                type="number"
                class="form-control price-input"
                placeholder="Giá thấp nhất"
                min="0"
              />
              <span class="price-separator">-</span>
              <input
                v-model.number="priceMax"
                type="number"
                class="form-control price-input"
                placeholder="Giá cao nhất"
                min="0"
              />
              <button
                class="btn btn-primary price-filter-btn"
                @click="filterByPrice"
              >
                <i class="fas fa-filter"></i>
                Lọc
              </button>
            </div>
          </div>

          <!-- Category filter -->
          <div class="form-group">
            <label class="form-label">
              <i class="fas fa-tags"></i> Lọc theo danh mục
            </label>
            <select
              v-model="selectedCategory"
              @change="filterByCategory"
              class="form-control"
            >
              <option value="">Tất cả danh mục</option>
              <option v-for="cat in categories" :key="cat.loaiSanPhamId" :value="cat.loaiSanPhamId">
                {{ cat.tenLoai }}
              </option>
            </select>
          </div>
        </div>
      </div>
    </div>

    <div class="main-content">
      <!-- Edit Product Section -->
      <div class="card edit-product-section" v-if="showEditForm">
        <div class="card-header">
          <h2 class="section-title">
            <i class="fas fa-edit"></i>
            Cập nhật sản phẩm
          </h2>
          <button
            @click="toggleEditForm"
            class="btn btn-primary"
          >
            <i class="fas fa-minus"></i>
            Ẩn form
          </button>
        </div>

        <div class="card-body">
          <form @submit.prevent="saveProduct" class="product-form" enctype="multipart/form-data">
            <div class="form-grid">
              <div class="form-group">
                <label class="form-label">
                  <i class="fas fa-tag"></i> Tên sản phẩm
                  <span class="required">*</span>
                </label>
                <input
                  v-model="product.productName"
                  type="text"
                  class="form-control"
                  placeholder="Nhập tên sản phẩm"
                  required
                />
              </div>

              <div class="form-group">
                <label class="form-label">
                  <i class="fas fa-money-bill-wave"></i> Giá
                  <span class="required">*</span>
                </label>
                <input
                  v-model.number="product.price"
                  type="number"
                  class="form-control"
                  placeholder="Nhập giá sản phẩm"
                  min="0"
                  required
                />
              </div>

              <div class="form-group">
                <label class="form-label">
                  <i class="fas fa-warehouse"></i> Số lượng
                  <span class="required">*</span>
                </label>
                <input
                  v-model.number="product.quantity"
                  type="number"
                  class="form-control"
                  placeholder="Số lượng"
                  disabled
                />
              </div>

              <div class="form-group">
                <label class="form-label">
                  <i class="fas fa-list"></i> Danh mục
                  <span class="required">*</span>
                </label>
                <select
                  v-model.number="product.categoryId"
                  class="form-control"
                  required
                >
                  <option value="">Chọn danh mục</option>
                  <option v-for="cat in categories" :key="cat.loaiSanPhamId" :value="cat.loaiSanPhamId">
                    {{ cat.tenLoai }}
                  </option>
                </select>
              </div>

              <div class="form-group full-width">
                <label class="form-label">
                  <i class="fas fa-align-left"></i> Mô tả
                </label>
                <textarea
                  v-model="product.description"
                  class="form-control"
                  rows="3"
                  placeholder="Nhập mô tả sản phẩm"
                ></textarea>
              </div>

              <div class="form-group full-width">
                <label class="form-label">
                  <i class="fas fa-image"></i> Ảnh sản phẩm
                </label>
                <div class="image-upload-container">
                  <input
                    type="file"
                    class="form-control file-input"
                    @change="handleImage"
                    accept="image/*"
                    id="imageUpload"
                  />
                  <label for="imageUpload" class="file-upload-label">
                    <i class="fas fa-cloud-upload-alt"></i>
                    Chọn ảnh sản phẩm
                  </label>
                  <div v-if="imagePreview" class="image-preview">
                    <img :src="imagePreview" alt="Preview" />
                    <button
                      type="button"
                      @click="removeImage"
                      class="remove-image-btn"
                    >
                      <i class="fas fa-times"></i>
                    </button>
                  </div>
                </div>
              </div>
            </div>

            <div class="form-actions">
              <button type="submit" class="btn btn-success" :disabled="loading">
                <i class="fas fa-save"></i>
                {{ loading ? "Đang lưu..." : "Cập nhật" }}
              </button>
              <button
                style="margin-left: 5px"
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

      <!-- Products List Section -->
      <div class="card products-list-section">
        <div class="card-header">
          <h2 class="section-title">
            <i class="fas fa-list"></i>
            Danh sách sản phẩm
            <span class="badge">{{ filteredProducts.length }}</span>
          </h2>
          <div class="view-controls">
            <button
              @click="viewMode = 'table'"
              class="btn btn-sm"
              :class="
                viewMode === 'table' ? 'btn-info' : 'btn-outline-secondary'
              "
            >
              <i class="fas fa-table"></i>
              Bảng
            </button>
            <button
              @click="viewMode = 'grid'"
              class="btn btn-sm"
              :class="
                viewMode === 'grid' ? 'btn-info' : 'btn-outline-secondary'
              "
            >
              <i class="fas fa-th"></i>
              Lưới
            </button>
          </div>
        </div>

        <div class="card-body">
          <!-- Loading State -->
          <div v-if="loading" class="loading-state">
            <i class="fas fa-spinner fa-spin"></i>
            Đang tải sản phẩm...
          </div>

          <!-- Empty State -->
          <div v-else-if="!filteredProducts.length" class="empty-state">
            <i class="fas fa-box-open"></i>
            <p>Không có sản phẩm nào</p>
          </div>

          <!-- Table View -->
          <div v-else-if="viewMode === 'table'" class="table-responsive">
            <table class="products-table">
              <thead>
                <tr>
                  <th>
                    <i class="fas fa-hashtag"></i>
                    ID
                  </th>
                  <th>
                    <i class="fas fa-image"></i>
                    Ảnh
                  </th>
                  <th>
                    <i class="fas fa-tag"></i>
                    Tên sản phẩm
                  </th>
                  <th>
                    <i class="fas fa-money-bill-wave"></i>
                    Giá
                  </th>
                  <th>
                    <i class="fas fa-warehouse"></i>
                    Số lượng
                  </th>
                  <th>
                    <i class="fas fa-list"></i>
                    Danh mục
                  </th>
                  <th>
                    <i class="fas fa-align-left"></i>
                    Mô tả
                  </th>
                  <th>
                    <i class="fas fa-tools"></i>
                    Hành động
                  </th>
                </tr>
              </thead>
              <tbody>
                <tr
                  v-for="sp in filteredProducts"
                  :key="sp.sanPhamId"
                  class="product-row"
                >
                  <td class="product-id">{{ sp.sanPhamId }}</td>
                  <td class="product-image">
                    <div class="image-container">
                      <img
                        v-if="sp.hinhAnh"
                        :src="sp.hinhAnh"
                        :alt="sp.tenSP"
                        @error="handleImageError"
                      />
                      <div v-else class="no-image">
                        <i class="fas fa-image"></i>
                        <span>Không có ảnh</span>
                      </div>
                    </div>
                  </td>
                  <td class="product-name">
                    <div class="name-container">
                      <strong>{{ sp.tenSP }}</strong>
                    </div>
                  </td>
                  <td class="product-price">
                    <span class="price-value">
                      {{ formatPrice(sp.gia) }}
                    </span>
                  </td>
                  <td class="product-quantity">
                    <span
                      class="quantity-badge"
                      :class="getQuantityClass(sp.soLuong)"
                    >
                      {{ sp.soLuong }}
                    </span>
                  </td>
                  <td class="product-category">
                    <span class="category-badge">
                      {{ getCategoryName(sp.loaiSanPhamId) }}
                    </span>
                  </td>
                  <td class="product-description">
                    <div class="description-text">
                      {{ sp.moTa || "(Không có mô tả)" }}
                    </div>
                  </td>
                  <td class="product-actions">
                    <div class="action-buttons">
                      <button
                        @click="editProduct(sp)"
                        class="btn btn-sm btn-warning"
                        title="Chỉnh sửa"
                      >
                        <i class="fas fa-edit"></i>
                      </button>
                      <button
                        @click="deleteProduct(sp.sanPhamId)"
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

          <!-- Grid View -->
          <div v-else class="products-grid">
            <div
              v-for="sp in filteredProducts"
              :key="sp.sanPhamId"
              class="product-card"
            >
              <div class="product-card-image">
                <img
                  v-if="sp.hinhAnh"
                  :src="sp.hinhAnh"
                  :alt="sp.tenSP"
                  @error="handleImageError"
                />
                <div v-else class="no-image-placeholder">
                  <i class="fas fa-image"></i>
                </div>
                <div class="product-overlay">
                  <button
                    @click="editProduct(sp)"
                    class="btn btn-sm btn-warning"
                  >
                    <i class="fas fa-edit"></i>
                  </button>
                  <button
                    @click="deleteProduct(sp.sanPhamId)"
                    class="btn btn-sm btn-danger"
                  >
                    <i class="fas fa-trash"></i>
                  </button>
                </div>
              </div>
              <div class="product-card-content">
                <h5 class="product-card-title">{{ sp.tenSP }}</h5>
                <p class="product-card-price">{{ formatPrice(sp.gia) }}</p>
                <div class="product-card-info">
                  <span class="quantity-info">
                    <i class="fas fa-warehouse"></i>
                    {{ sp.soLuong }}
                  </span>
                  <span class="category-info">
                    <i class="fas fa-tag"></i>
                    {{ getCategoryName(sp.loaiSanPhamId) }}
                  </span>
                </div>
                <p class="product-card-description">
                  {{ sp.moTa || "Không có mô tả" }}
                </p>
              </div>
            </div>
          </div>
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

      <!-- Confirmation Dialog -->
      <div v-if="showConfirmDelete" class="modal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title">Xác nhận xóa</h5>
              <button
                type="button"
                class="btn-close"
                @click="cancelDelete"
              ></button>
            </div>
            <div class="modal-body">
              <p>Bạn có muốn xóa sản phẩm này không?</p>
            </div>
            <div class="modal-footer">
              <button
                type="button"
                class="btn btn-secondary"
                @click="cancelDelete"
              >
                Cancel
              </button>
              <button
                type="button"
                class="btn btn-danger"
                @click="confirmDelete"
              >
                OK
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

// Reactive data
const products = ref([]);
const categories = ref([]);
const loading = ref(false);
const showEditForm = ref(false);
const viewMode = ref("table");
const toasts = ref([]);
const showConfirmDelete = ref(false);
const productToDelete = ref(null);

// Form data
const product = ref({
  productId: 0,
  productName: "",
  price: 0,
  quantity: 0,
  description: "",
  categoryId: null,
});

// Filters
const searchName = ref("");
const priceMin = ref(0);
const priceMax = ref(100000000);
const selectedCategory = ref("");

// Image handling
const imageFile = ref(null);
const imagePreview = ref(null);

// Computed filtered products
const filteredProducts = computed(() => {
  return products.value.filter((sp) => {
    const matchesName = !searchName.value || sp.tenSP.toLowerCase().includes(searchName.value.toLowerCase());
    const matchesPrice = sp.gia >= priceMin.value && sp.gia <= priceMax.value;
    const matchesCategory = !selectedCategory.value || sp.loaiSanPhamId === Number(selectedCategory.value);
    return matchesName && matchesPrice && matchesCategory;
  });
});

// Methods
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

const toggleEditForm = () => {
  showEditForm.value = !showEditForm.value;
  if (!showEditForm.value) resetForm();
};

const handleImage = (e) => {
  const file = e.target.files[0];
  if (file) {
    imageFile.value = file;
    const reader = new FileReader();
    reader.onload = (e) => {
      imagePreview.value = e.target.result;
    };
    reader.readAsDataURL(file);
  }
};

const removeImage = () => {
  imageFile.value = null;
  imagePreview.value = null;
  document.getElementById("imageUpload").value = "";
};

const handleImageError = (e) => {
  e.target.style.display = "none";
  e.target.nextElementSibling?.classList.add("show");
};

const formatPrice = (price) => {
  return price ? price.toLocaleString("vi-VN") + " VND" : "0 VND";
};

const getQuantityClass = (quantity) => {
  if (quantity === 0) return "quantity-zero";
  if (quantity < 10) return "quantity-low";
  return "quantity-normal";
};

const getCategoryName = (categoryId) => {
  const category = categories.value.find((cat) => cat.loaiSanPhamId === categoryId);
  return category ? category.tenLoai : "Không xác định";
};

const fetchProducts = async () => {
  try {
    loading.value = true;
    const res = await apiClient.get("Product");
    products.value = res;
  } catch (err) {
    console.error("Lỗi tải sản phẩm:", err);
    showToast("Lỗi khi tải danh sách sản phẩm: " + (err.message || "Không xác định"), "error");
    products.value = [];
  } finally {
    loading.value = false;
  }
};

const fetchCategories = async () => {
  try {
    loading.value = true;
    const res = await apiClient.get("category");
    categories.value = res;
  } catch (err) {
    console.error("Fetch categories error:", err);
    showToast("Lỗi khi tải danh sách danh mục: " + (err.message || "Không xác định"), "error");
  } finally {
    loading.value = false;
  }
};

const searchByName = () => {
  // Lọc cục bộ dựa trên tên sản phẩm
};

const clearSearch = () => {
  searchName.value = "";
};

const filterByPrice = () => {
  if (priceMin.value > priceMax.value) {
    showToast("Giá thấp nhất không được lớn hơn giá cao nhất", "warning");
    return;
  }
};

const filterByCategory = () => {
  // Lọc cục bộ dựa trên danh mục
};

const saveProduct = async () => {
  try {
    loading.value = true;
    const formData = new FormData();
    formData.append("productName", product.value.productName);
    formData.append("sellingPrice", product.value.price);
    formData.append("description", product.value.description);
    formData.append("categoryId", product.value.categoryId);
    if (imageFile.value) {
      formData.append("image", imageFile.value);
    }

    const url = `Product/${product.value.productId || ""}`;
    const method = product.value.productId ? "put" : "post";

    await apiClient({ url, method, data: formData, headers: { "Content-Type": "multipart/form-data" } });
    showToast("Cập nhật sản phẩm thành công!", "success");

    resetForm();
    await fetchProducts();
    showEditForm.value = false;
  } catch (err) {
    console.error("Lỗi lưu sản phẩm:", err);
    showToast("Lỗi khi lưu sản phẩm: " + (err.message || "Không xác định"), "error");
  } finally {
    loading.value = false;
  }
};

const editProduct = (sp) => {
  product.value = {
    productId: sp.sanPhamId,
    productName: sp.tenSP,
    price: sp.gia,
    quantity: sp.soLuong,
    description: sp.moTa,
    categoryId: sp.loaiSanPhamId,
  };
  imagePreview.value = sp.hinhAnh || null;
  imageFile.value = null;
  showEditForm.value = true;
};

const deleteProduct = (id) => {
  showConfirmDelete.value = true;
  productToDelete.value = id;
};

const confirmDelete = async () => {
  try {
    loading.value = true;
    await apiClient.delete(`Product/${productToDelete.value}`);
    showToast("Xóa sản phẩm thành công!", "success");
    await fetchProducts();
  } catch (err) {
    console.error("Lỗi xóa:", err);
    showToast("Lỗi khi xóa sản phẩm: " + (err.message || "Không xác định"), "error");
  } finally {
    loading.value = false;
    showConfirmDelete.value = false;
    productToDelete.value = null;
  }
};

const cancelDelete = () => {
  showConfirmDelete.value = false;
  productToDelete.value = null;
};

const resetForm = () => {
  product.value = {
    productId: 0,
    productName: "",
    price: 0,
    quantity: 0,
    description: "",
    categoryId: null,
  };
  imageFile.value = null;
  imagePreview.value = null;
  if (document.getElementById("imageUpload")) {
    document.getElementById("imageUpload").value = "";
  }
  showEditForm.value = false;
};

// Lifecycle
onMounted(async () => {
  await Promise.all([fetchProducts(), fetchCategories()]);
});
</script>

<style scoped>
/* Giữ nguyên style từ phiên bản cũ */
.product-management {
  max-width: 1400px;
  margin: 0 auto;
  padding: 20px;
  font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
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
  font-size: 1rem;
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

/* Search Container */
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

.clear-search.show {
  display: inline-block;
}

.price-filter-group {
  display: flex;
  align-items: center;
  gap: 10px;
}

.price-input {
  width: 100%;
}

.price-separator {
  font-weight: bold;
  font-size: 1.2rem;
}

.price-filter-btn {
  padding: 10px 16px;
}

/* Toast Notification */
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

/* Table styles */
.products-table {
  width: 100%;
  border-collapse: collapse;
  background: white;
  border-radius: 10px;
  overflow: hidden;
}

.products-table th,
.products-table td {
  padding: 14px 16px;
  text-align: left;
  border-bottom: 1px solid #e1e8ed;
  font-size: 0.95rem;
}

.products-table th {
  background: #f7f9fb;
  font-weight: 600;
  color: #34495e;
}

.products-table tr:hover {
  background: #f0f8ff;
}

.image-container img {
  width: 60px;
  height: 60px;
  object-fit: cover;
  border-radius: 8px;
}

.no-image {
  text-align: center;
  font-size: 0.85rem;
  color: #7f8c8d;
}

.quantity-badge {
  display: inline-block;
  padding: 5px 10px;
  border-radius: 12px;
  font-weight: 600;
  font-size: 0.85rem;
  color: white;
}

.quantity-zero {
  background: #e74c3c;
}

.quantity-low {
  background: #f39c12;
}

.quantity-normal {
  background: #2ecc71;
}

.category-badge {
  background: #bdc3c7;
  padding: 4px 10px;
  border-radius: 12px;
  font-size: 0.85rem;
  color: #2c3e50;
}

/* Grid view */
.products-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(260px, 1fr));
  gap: 20px;
}

.product-card {
  background: white;
  border: 1px solid #e1e8ed;
  border-radius: 10px;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.05);
  transition: transform 0.2s ease;
}

.product-card:hover {
  transform: translateY(-5px);
}

.product-card-image {
  position: relative;
  height: 180px;
  overflow: hidden;
}

.product-card-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.no-image-placeholder {
  background: #ecf0f1;
  color: #7f8c8d;
  display: flex;
  align-items: center;
  justify-content: center;
  height: 100%;
}

.product-overlay {
  position: absolute;
  bottom: 10px;
  right: 10px;
  display: flex;
  gap: 8px;
  opacity: 0;
  transition: opacity 0.3s ease;
}

.product-card:hover .product-overlay {
  opacity: 1;
}

.product-card-content {
  padding: 15px;
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.product-card-title {
  font-weight: 600;
  font-size: 1rem;
  color: #2c3e50;
  margin: 0;
}

.product-card-price {
  color: #e67e22;
  font-weight: bold;
  font-size: 1.1rem;
}

.product-card-info {
  display: flex;
  justify-content: space-between;
  font-size: 0.85rem;
  color: #7f8c8d;
}

.product-card-description {
  font-size: 0.9rem;
  color: #34495e;
}

/* Loading and Empty state */
.loading-state,
.empty-state {
  text-align: center;
  padding: 40px 20px;
  color: #7f8c8d;
  font-size: 1.1rem;
}

.empty-state i {
  font-size: 2.5rem;
  margin-bottom: 10px;
}

.view-controls .btn {
  margin-left: 10px;
}

/* Action Buttons */
.action-buttons {
  display: flex;
  gap: 8px;
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

.action-buttons .btn i {
  pointer-events: none;
}

.modal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-dialog {
  background: white;
  border-radius: 8px;
  width: 400px;
  max-width: 90%;
}

.modal-content {
  padding: 20px;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-bottom: 1px solid #e1e8ed;
  padding-bottom: 10px;
}

.modal-title {
  font-size: 1.2rem;
  font-weight: 600;
  color: #2c3e50;
}

.btn-close {
  background: none;
  border: none;
  font-size: 1.5rem;
  color: #7f8c8d;
  cursor: pointer;
}

.modal-body {
  padding: 20px 0;
  color: #34495e;
}

.modal-footer {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  border-top: 1px solid #e1e8ed;
  padding-top: 10px;
}
</style>