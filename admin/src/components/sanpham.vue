<template>
  <div class="container mt-4">
    <h2 class="text-center mb-4">Quản lý Sản phẩm</h2>

    <!-- Tìm kiếm và lọc sản phẩm -->
    <div class="row mb-4 align-items-end">
      <div class="col-md-4">
        <label class="form-label">Tìm theo tên sản phẩm</label>
        <div class="input-group">
          <input v-model="searchName" @input="searchByName" type="text" class="form-control" placeholder="Nhập tên sản phẩm..." />
        </div>
      </div>

      <div class="col-md-4">
        <label class="form-label">Lọc theo giá</label>
        <div class="input-group">
          <input v-model.number="priceMin" type="number" class="form-control" placeholder="Giá thấp nhất" min="0" />
          <input v-model.number="priceMax" type="number" class="form-control" placeholder="Giá cao nhất" min="0" />
          <button class="btn btn-outline-success" @click="filterByPrice">Lọc</button>
        </div>
      </div>

      <div class="col-md-4">
        <button class="btn btn-secondary w-100" @click="fetchProducts">Hiển thị tất cả</button>
      </div>
    </div>

    <div class="row">
      <!-- Form sửa sản phẩm (hiện khi click Sửa, bên trái) -->
      <div class="col-md-4" v-if="isEditing">
        <div class="card mb-4 shadow">
          <div class="card-header fw-bold bg-primary text-white">
            Cập nhật sản phẩm
          </div>
          <div class="card-body">
            <form @submit.prevent="saveProduct">
              <div class="mb-3">
                <label class="form-label">Tên sản phẩm</label>
                <input v-model="product.productName" class="form-control" required />
              </div>
              <div class="mb-3">
                <label class="form-label">Giá</label>
                <input v-model.number="product.price" type="number" class="form-control" required min="0" />
              </div>
              <div class="mb-3">
                <label class="form-label">Số lượng</label>
                <input v-model.number="product.quantity" type="number" class="form-control" disabled />
              </div>
              <div class="mb-3">
                <label class="form-label">Mô tả</label>
                <textarea v-model="product.description" class="form-control" rows="3"></textarea>
              </div>
              <div class="mb-3">
                <label class="form-label">ID loại sản phẩm</label>
                <input v-model.number="product.categoryId" type="number" class="form-control" required min="1" />
              </div>
              <div class="mb-3">
                <label class="form-label">Ảnh sản phẩm</label>
                <input type="file" class="form-control" @change="handleImage" accept="image/*" />
              </div>
              <div class="d-flex justify-content-end">
                <button type="submit" class="btn btn-primary me-2">Cập nhật</button>
                <button type="button" class="btn btn-secondary" @click="resetForm">Hủy</button>
              </div>
            </form>
          </div>
        </div>
      </div>

      <!-- Danh sách sản phẩm (bên phải hoặc toàn bộ nếu không edit) -->
      <div class="col-md-8" :class="{ 'col-md-12': !isEditing }">
        <div class="table-responsive">
          <table class="table table-bordered table-hover text-center align-middle">
            <thead class="table-dark">
              <tr>
                <th>ID</th>
                <th>Tên</th>
                <th>Giá</th>
                <th>Số lượng</th>
                <th>Mô tả</th>
                <th>Loại</th>
                <th>Ảnh</th>
                <th>Hành động</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="sp in products" :key="sp.productId">
                <td>{{ sp.productId }}</td>
                <td>{{ sp.productName }}</td>
                <td>{{ sp.price ? sp.price.toLocaleString() : '0' }} VND</td>
                <td>{{ sp.quantity }}</td>
                <td>{{ sp.description }}</td>
                <td>{{ sp.categoryId }}</td>
                <td>
                  <img :src="sp.images" alt="Ảnh sản phẩm" width="80" v-if="sp.images" />
                </td>
                <td>
                  <div class="d-flex justify-content-center gap-2">
                    <button class="btn btn-warning btn-sm" @click="editProduct(sp)">Sửa</button>
                    <button class="btn btn-danger btn-sm" @click="showDeleteConfirmation(sp.productId)">Xóa</button>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <!-- Confirmation Dialog -->
      <div v-if="showConfirmDelete" class="modal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title">Xác nhận xóa</h5>
              <button type="button" class="btn-close" @click="cancelDelete"></button>
            </div>
            <div class="modal-body">
              <p>Bạn có muốn xóa sản phẩm này không?</p>
            </div>
            <div class="modal-footer">
              <button type="button" class="btn btn-secondary" @click="cancelDelete">Cancel</button>
              <button type="button" class="btn btn-danger" @click="confirmDelete">OK</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import apiClient from "../utils/axiosClient";

const products = ref([]);
const product = ref({
  productId: 0,
  productName: "",
  price: 0,
  quantity: 0,
  description: "",
  categoryId: 1,
});
const imageFile = ref(null);
const isEditing = ref(false);
const searchName = ref("");
const priceMin = ref(0);
const priceMax = ref(100000000);
const showConfirmDelete = ref(false);
const productToDelete = ref(null);

const handleImage = (e) => {
  imageFile.value = e.target.files[0];
};

const fetchProducts = async () => {
  try {
    const res = await apiClient.get("Product");
    const imageBaseUrl = "https://localhost:7183/images/";
    products.value = res.data.map(p => ({
      productId: p.sanPhamId,
      productName: p.tenSP,
      price: p.gia,
      quantity: p.soLuong,
      description: p.moTa,
      categoryId: p.loaiSanPhamId,
      images: p.hinhAnh,
    }));
  } catch (err) {
    console.error("Lỗi tải sản phẩm:", err);
  }
};

const searchByName = async () => {
  if (!searchName.value.trim()) return fetchProducts();
  try {
    const res = await apiClient.get(`Product/name?ten=${searchName.value}`);
    const imageBaseUrl = "https://localhost:7183/images/";
    products.value = res.data.map(p => ({
      productId: p.sanPhamId,
      productName: p.tenSP,
      price: p.gia,
      quantity: p.soLuong,
      description: p.moTa,
      categoryId: p.loaiSanPhamId,
      images: p.hinhAnh,
    }));
  } catch (err) {
    console.error("Không tìm thấy sản phẩm:", err);
    products.value = [];
  }
};

const filterByPrice = async () => {
  if (priceMin.value > priceMax.value) return alert("Giá thấp nhất không được lớn hơn giá cao nhất");
  try {
    const res = await apiClient.get(`Product/filter-by-price?min=${priceMin.value}&max=${priceMax.value}`); 
    const imageBaseUrl = "https://localhost:7183/images/";
    products.value = res.data.map(p => ({
      productId: p.sanPhamId,
      productName: p.tenSP,
      price: p.gia,
      quantity: p.soLuong,
      description: p.moTa,
      categoryId: p.loaiSanPhamId,
      images: p.hinhAnh,
    }));
  } catch (err) {
    console.error("Lỗi lọc giá:", err);
    products.value = [];
  }
};

const saveProduct = async () => {
  try {
    const formData = new FormData();
    formData.append("productName", product.value.productName);
    formData.append("sellingPrice", product.value.price);
    // Không gửi quantity vì nó bị disabled
    formData.append("description", product.value.description);
    formData.append("categoryId", product.value.categoryId);
    if (imageFile.value) {
      formData.append("image", imageFile.value);
    }

    await apiClient.put(`Product/${product.value.productId}`, formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
    alert("Cập nhật sản phẩm thành công!");
    resetForm();
    fetchProducts();
  } catch (err) {
    console.error("Lỗi lưu sản phẩm:", err);
  }
};

const editProduct = (sp) => {
  product.value = {
    productId: sp.productId,
    productName: sp.productName,
    price: sp.price,
    quantity: sp.quantity,
    description: sp.description,
    categoryId: sp.categoryId,
  };
  imageFile.value = null;
  isEditing.value = true;
};

const showDeleteConfirmation = (id) => {
  productToDelete.value = id;
  showConfirmDelete.value = true;
};

const confirmDelete = async () => {
  if (productToDelete.value) {
    try {
      await apiClient.delete(`Product/${productToDelete.value}`);
      fetchProducts();
    } catch (err) {
      console.error("Lỗi xóa:", err);
    }
  }
  showConfirmDelete.value = false;
  productToDelete.value = null;
};

const cancelDelete = () => {
  showConfirmDelete.value = false;
  productToDelete.value = null;
};

const resetForm = () => {
  isEditing.value = false;
  product.value = {
    productId: 0,
    productName: "",
    price: 0,
    quantity: 0,
    description: "",
    categoryId: 1,
  };
  imageFile.value = null;
};

onMounted(fetchProducts);
</script>

<style scoped>
.modal {
  display: flex;
  justify-content: center;
  align-items: center;
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  z-index: 1050;
}

.modal-dialog {
  max-width: 500px;
  margin: 1.75rem auto;
}

.modal-content {
  border-radius: 8px;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3);
}

.modal-header {
  border-bottom: 1px solid #dee2e6;
  padding: 1rem;
}

.modal-title {
  font-size: 1.25rem;
  font-weight: 500;
}

.btn-close {
  padding: 0.5rem 0.75rem;
  background: none;
  border: none;
  font-size: 1.5rem;
  cursor: pointer;
}

.modal-body {
  padding: 1rem;
}

.modal-footer {
  border-top: 1px solid #dee2e6;
  padding: 1rem;
  justify-content: flex-end;
}

.modal-footer .btn {
  margin-left: 0.5rem;
}
</style>