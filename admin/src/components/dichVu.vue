<template>
  <div class="container mt-4">
    <!-- Tiêu đề trang -->
    <h2 class="text-center mb-4 text-primary fw-bold">
      <i class="fa fa-list-alt me-2"></i>Quản lý Dịch Vụ
    </h2>

    <!-- Bộ lọc -->
    <div class="row mb-4 g-3 align-items-end">
      <div class="col-md-4">
        <label class="form-label fw-medium">
          <i class="fa fa-search me-1"></i> Tìm theo tên
        </label>
        <input v-model="searchName" @input="searchByName" type="text" class="form-control" placeholder="Nhập tên dịch vụ..." />
      </div>

      <div class="col-md-5">
        <label class="form-label fw-medium">
          <i class="fa fa-filter me-1"></i> Lọc giá
        </label>
        <div class="input-group">
          <input v-model.number="priceMin" type="number" class="form-control" placeholder="Từ" min="0" />
          <input v-model.number="priceMax" type="number" class="form-control" placeholder="Đến" min="0" />
          <button class="btn btn-success" @click="filterByPrice">
            <i class="fa fa-sort-amount-down-alt"></i> Lọc
          </button>
        </div>
      </div>

      <div class="col-md-3 text-end">
        <button class="btn btn-outline-secondary px-4" @click="fetchDichVus">
          <i class="fa fa-list me-1"></i> Tất cả
        </button>
      </div>
    </div>

    <div class="row">
      <!-- Form -->
      <div class="col-lg-4 mb-4">
        <div class="card shadow-sm border-0">
          <div class="card-header bg-primary text-white fw-bold">
            <i class="fa fa-plus-circle me-1"></i> {{ isEditing ? "Cập nhật Dịch vụ" : "Thêm Dịch vụ mới" }}
          </div>
          <div class="card-body">
            <form @submit.prevent="saveDichVu">
              <div class="row">
                <div class="col-md-6 mb-3">
                  <label class="form-label">Tên dịch vụ</label>
                  <input v-model="dichVu.tenDichVu" class="form-control" required />
                </div>
                <div class="col-md-6 mb-3">
                  <label class="form-label">Giá (VND)</label>
                  <input v-model.number="dichVu.gia" type="number" class="form-control" min="0" required />
                </div>
                <div class="col-md-6 mb-3">
                  <label class="form-label">Thời gian (phút)</label>
                  <input v-model.number="dichVu.thoiGian" type="number" class="form-control" min="0" required />
                </div>
                <div class="col-md-6 mb-3">
                  <label class="form-label">Hình ảnh</label>
                  <input v-model="dichVu.hinhAnh" class="form-control" placeholder="image-name" />
                </div>
                <div class="col-md-12 mb-3">
                  <label class="form-label">Mô tả</label>
                  <textarea v-model="dichVu.moTa" class="form-control" rows="2"></textarea>
                </div>
                <div class="col-md-6 mb-3">
                  <label class="form-label">Loại dịch vụ</label>
                  <select v-model.number="dichVu.loaiDichVuID" class="form-select" required>
                    <option disabled value="">-- Chọn loại dịch vụ --</option>
                    <option v-for="ldv in loaiDichVus" :key="ldv.loaiDichVuID" :value="ldv.loaiDichVuID">
                      {{ ldv.tenLoai }}
                    </option>
                  </select>
                </div>
                <div class="col-md-6 mb-3">
                  <label class="form-label">Trạng thái</label>
                  <select v-model.number="dichVu.trangThai" class="form-select">
                    <option :value="1">Hoạt động</option>
                    <option :value="0">Tạm ngừng</option>
                  </select>
                </div>
              </div>
              <div class="d-flex justify-content-end gap-2">
                <button type="submit" class="btn btn-primary">
                  <i class="fa fa-save me-1"></i> {{ isEditing ? "Cập nhật" : "Thêm" }}
                </button>
                <button type="button" class="btn btn-secondary" @click="resetForm">
                  <i class="fa fa-undo me-1"></i> Hủy
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>

      <!-- Danh sách -->
      <div class="col-lg-8 mb-4">
        <div class="card shadow-sm border-0">
          <div class="card-header bg-dark text-white fw-bold">
            <i class="fa fa-table me-1"></i> Danh sách Dịch vụ
          </div>
          <div class="table-responsive">
            <table class="table table-bordered table-hover align-middle mb-0 text-center">
              <thead class="table-light">
                <tr>
                  <th>Tên</th><th>Giá</th><th>Thời gian</th><th>Ảnh</th><th>Ngày tạo</th><th>Loại</th><th>Trạng thái</th><th>Hành động</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="dv in paginatedDichVus" :key="dv.dichVuID">
            
                  <td>{{ dv.tenDichVu }}</td>
                  <td class="text-success fw-medium">{{ dv.gia.toLocaleString() }} vnđ</td>
                  <td>{{ dv.thoiGian }} phút</td>
                  <td><img :src="'http://localhost:5055/images/' + dv.hinhAnh + '.jpg'" class="img-thumbnail" style="width: 80px; height: 60px; object-fit: cover;" /></td>
                  <td>{{ new Date(dv.ngayTao).toLocaleDateString() }}</td>
                  <td>{{ dv.tenLoai }}</td>
                  <td>
                    <span class="badge" :class="dv.trangThai === 1 ? 'bg-success' : 'bg-secondary'">
                      {{ dv.trangThai === 1 ? 'Hoạt động' : 'Tạm ngừng' }}
                    </span>
                  </td>
                  <td>
                    <button class="btn btn-warning btn-sm me-2" @click="editDichVu(dv)"><i class="fa fa-edit"></i></button>
                    <button class="btn btn-sm" :class="dv.trangThai === 1 ? 'btn-outline-secondary' : 'btn-outline-success'" @click="toggleTrangThai(dv)">
                      <i class="fa" :class="dv.trangThai === 1 ? 'fa-pause' : 'fa-play'"></i>
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>

          <!-- Phân trang -->
          <div class="d-flex justify-content-between align-items-center p-3">
            <div>Trang {{ currentPage }} / {{ totalPages }}</div>
            <div class="btn-group">
              <button class="btn btn-outline-secondary btn-sm" :disabled="currentPage === 1" @click="goToPage(currentPage - 1)">
                <i class="fa fa-angle-left"></i>
              </button>
              <button v-for="page in totalPages" :key="page" class="btn btn-sm" :class="page === currentPage ? 'btn-primary' : 'btn-outline-secondary'" @click="goToPage(page)">{{ page }}</button>
              <button class="btn btn-outline-secondary btn-sm" :disabled="currentPage === totalPages" @click="goToPage(currentPage + 1)">
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
    loaiDichVus.value = res.data;
  } catch (error) {
    console.error("Lỗi lấy loại dịch vụ:", error);
  }
};

const searchByName = async () => {
  if (!searchName.value.trim()) return fetchDichVus();
  try {
    const response = await apiClient.get(`DichVu/name?ten=${searchName.value}`);
    dichVus.value = response.data;
    totalItems.value = response.data.length;
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