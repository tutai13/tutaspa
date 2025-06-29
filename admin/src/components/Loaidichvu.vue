<template>
    <div class="container mt-4">
      <h2 class="text-center text-primary mb-4">
        <i class="fa fa-list-alt me-2"></i>Quản lý Loại Dịch Vụ
      </h2>
  
      <!-- Tìm kiếm -->
      <div class="input-group mb-4 w-50 mx-auto">
        <span class="input-group-text bg-light"><i class="fa fa-search text-secondary"></i></span>
        <input
          v-model="searchName"
          @input="searchByName"
          type="text"
          class="form-control"
          placeholder="Tìm theo tên loại dịch vụ..."
        />
        <button class="btn btn-outline-secondary" @click="resetSearch">
          <i class="fa fa-times"></i>
        </button>
      </div>
  
      <!-- Form thêm / cập nhật -->
      <div class="card shadow-sm mb-4">
        <div class="card-body">
          <h5 class="card-title text-success">
            <i class="fa" :class="isEditing ? 'fa-edit' : 'fa-plus-circle'"></i>
            {{ isEditing ? 'Cập nhật' : 'Thêm mới' }} Loại Dịch Vụ
          </h5>
          <div class="mb-3">
            <label class="form-label fw-semibold">Tên loại dịch vụ</label>
            <input type="text" class="form-control" v-model="form.tenLoai" placeholder="Nhập tên loại dịch vụ..." />
          </div>
          <button class="btn btn-success" @click="isEditing ? updateLoaiDichVu() : createLoaiDichVu()">
            <i class="fa" :class="isEditing ? 'fa-save' : 'fa-plus'"></i>
            {{ isEditing ? 'Cập nhật' : 'Thêm mới' }}
          </button>
          <button v-if="isEditing" class="btn btn-secondary ms-2" @click="resetForm">
            <i class="fa fa-undo"></i> Hủy
          </button>
        </div>
      </div>
  
      <!-- Bảng danh sách -->
      <div class="table-responsive">
        <table class="table table-bordered table-hover align-middle">
          <thead class="table-primary text-center">
            <tr>
              <th style="width: 10%">ID</th>
              <th>Tên Loại</th>
              <th style="width: 20%">Hành động</th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="loaiDichVus.length === 0">
              <td colspan="3" class="text-center text-muted">Không có dữ liệu</td>
            </tr>
            <tr v-for="item in loaiDichVus" :key="item.loaiDichVuID">
              <td class="text-center">{{ item.loaiDichVuID }}</td>
              <td>{{ item.tenLoai }}</td>
              <td class="text-center">
                <button class="btn btn-sm btn-warning me-2" @click="editLoaiDichVu(item)">
                  <i class="fa fa-edit"></i> Sửa
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue';
  import apiClient from "../utils/axiosClient";
  
  // State
  const loaiDichVus = ref([]);
  const allLoaiDichVus = ref([]); // lưu tất cả để reset khi xóa tìm kiếm
  const form = ref({
    loaiDichVuID: 0,
    tenLoai: ''
  });
  const isEditing = ref(false);
  const searchName = ref('');
  
  // Methods
  const fetchData = async () => {
    try {
      const res = await apiClient.get('/LoaiDichVu');
      loaiDichVus.value = res.data;
      allLoaiDichVus.value = res.data;
    } catch (error) {
      console.error("Lỗi khi tải danh sách loại dịch vụ:", error);
    }
  };
  
  const createLoaiDichVu = async () => {
    if (!form.value.tenLoai.trim()) {
      alert("Vui lòng nhập tên loại");
      return;
    }
    try {
      await apiClient.post('/LoaiDichVu', form.value);
      resetForm();
      fetchData();
    } catch (error) {
      console.error("Lỗi khi thêm loại dịch vụ:", error);
    }
  };
  
  const updateLoaiDichVu = async () => {
    try {
      await apiClient.put(`/LoaiDichVu/${form.value.loaiDichVuID}`, form.value);
      resetForm();
      fetchData();
    } catch (error) {
      console.error("Lỗi khi cập nhật loại dịch vụ:", error);
    }
  };
  
  const deleteLoaiDichVu = async (id) => {
    if (confirm("Bạn có chắc muốn xóa?")) {
      try {
        await apiClient.delete(`/LoaiDichVu/${id}`);
        fetchData();
      } catch (error) {
        console.error("Lỗi khi xóa loại dịch vụ:", error);
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
  .container {
    max-width: 900px;
  }
  </style>
  