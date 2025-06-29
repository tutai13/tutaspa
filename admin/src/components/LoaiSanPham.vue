<template>
  <div class="container mt-4">
    <h2 class="text-center mb-4">Quản lý loại sản phẩm</h2>
    <!-- Thanh tìm kiếm -->
<div class="row mb-4 justify-content-center">
  <div class="col-md-6">
    <div class="input-group">
      <input v-model="searchKeyword" @input="searchCategories" class="form-control" placeholder="Tìm theo tên loại sản phẩm..." />
      
      <button class="btn btn-secondary" @click="fetchCategories">Hiển thị tất cả</button>
    </div>
  </div>
</div>

    <!-- Form thêm/sửa loại sản phẩm -->
    <div class="row mb-4">
      <div class="col-md-6 offset-md-3">
        <div class="card shadow">
          <div class="card-header bg-success text-white fw-bold">
            {{ isEditing ? "Cập nhật loại sản phẩm" : "Thêm loại sản phẩm mới" }}
          </div>
          <div class="card-body">
            <form @submit.prevent="saveCategory">
              <div class="mb-3">
                <label class="form-label">Tên loại sản phẩm</label>
                <input v-model="category.tenLoai" class="form-control" required />
              </div>
              <div class="d-flex justify-content-end">
                <button type="submit" class="btn btn-success me-2">
                  {{ isEditing ? "Cập nhật" : "Thêm" }}
                </button>
                <button type="button" class="btn btn-secondary" @click="resetForm">Hủy</button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>

    <!-- Danh sách loại sản phẩm -->
    <div class="row">
      <div class="col-md-8 offset-md-2">
        <table class="table table-bordered table-hover text-center align-middle">
          <thead class="table-dark">
            <tr>
              <th>ID</th>
              <th>Tên loại</th>
              <th>Hành động</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="c in categories" :key="c.loaiSanPhamId">
              <td>{{ c.loaiSanPhamId }}</td>
              <td>{{ c.tenLoai }}</td>
              <td>
                <button class="btn btn-warning btn-sm me-2" @click="editCategory(c)">Sửa</button>
                <button class="btn btn-danger btn-sm" @click="deleteCategory(c.loaiSanPhamId)">Xóa</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>
<script setup>
import { ref, onMounted } from 'vue';
import apiClient from '../utils/axiosClient'; // Đảm bảo file axiosClient.js đã được cấu hình

const categories = ref([]);
const category = ref({
  loaiSanPhamId: 0,
  tenLoai: '',
});
const isEditing = ref(false);

const fetchCategories = async () => {
  try {
    const res = await apiClient.get("Category");
    categories.value = res.data;
  } catch (err) {
    console.error("Lỗi tải loại sản phẩm:", err);
  }
};

const saveCategory = async () => {
  try {
    if (isEditing.value) {
      await apiClient.put(`Category/${category.value.loaiSanPhamId}`, category.value);
    } else {
      await apiClient.post("Category", category.value);
    }
    resetForm();
    fetchCategories();
    alert(isEditing.value ? "Cập nhật thành công!" : "Thêm thành công!");
  } catch (err) {
    console.error("Lỗi lưu loại sản phẩm:", err);
  }
};

const editCategory = (c) => {
  category.value = { ...c };
  isEditing.value = true;
};

const deleteCategory = async (id) => {
  if (!confirm("Bạn có chắc muốn xóa loại sản phẩm này?")) return;
  try {
    await apiClient.delete(`Category/${id}`);
    fetchCategories();
  } catch (err) {
    console.error("Lỗi xóa loại sản phẩm:", err);
  }
};

const resetForm = () => {
  isEditing.value = false;
  category.value = {
    loaiSanPhamId: 0,
    tenLoai: '',
  };
};

onMounted(fetchCategories);
const searchKeyword = ref("");

const searchCategories = async () => {
  if (!searchKeyword.value.trim()) return fetchCategories();
  try {
    const res = await apiClient.get(`Category/search?ten=${searchKeyword.value}`);
    categories.value = res.data;
  } catch (err) {
    console.error("Lỗi tìm kiếm:", err);
    categories.value = [];
  }
};

</script>

