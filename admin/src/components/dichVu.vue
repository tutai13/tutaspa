<template>
  <div class="container mt-4">
    <h2 class="text-center mb-4">Quản lý Dịch Vụ</h2>

    <!-- Tìm kiếm và lọc dịch vụ -->
    <div class="row mb-4 align-items-end">
      <!-- Ô nhập tên dịch vụ và nút tìm -->
      <div class="col-md-4">
        <label class="form-label">Tìm kiếm theo tên dịch vụ</label>
        <div class="input-group">
          <input v-model="searchName" type="text" class="form-control" placeholder="Nhập tên dịch vụ..." />
          <button class="btn btn-primary" @click="searchByName">Tìm kiếm</button>
        </div>
      </div>

      <!-- Ô lọc theo giá -->
      <div class="col-md-4">
        <label class="form-label">Lọc theo khoảng giá</label>
        <div class="input-group">
          <input v-model.number="priceMin" type="number" class="form-control" placeholder="Giá thấp nhất" min="0" />
          <input v-model.number="priceMax" type="number" class="form-control" placeholder="Giá cao nhất" min="0" />
          <button class="btn btn-outline-success" @click="filterByPrice">Lọc giá</button>
        </div>
      </div>

      <!-- Nút hiển thị tất cả -->
      <div class="col-md-4">
        <button class="btn btn-secondary w-100" @click="fetchDichVus">Hiển thị tất cả dịch vụ</button>
      </div>
    </div>


    <div class="row">
      <!-- Form thêm/sửa dịch vụ -->
      <div class="col-md-4">
        <div class="card mb-4 shadow">
          <div class="card-header fw-bold bg-primary text-white">
            {{ isEditing ? "Cập nhật dịch vụ" : "Thêm dịch vụ mới" }}
          </div>
          <div class="card-body">
            <form @submit.prevent="saveDichVu">
              <div class="mb-3">
                <label class="form-label">Tên dịch vụ</label>
                <input v-model="dichVu.tenDichVu" class="form-control" required />
              </div>
              <div class="mb-3">
                <label class="form-label">Giá</label>
                <input v-model.number="dichVu.gia" type="number" class="form-control" required min="0" />
              </div>
              <div class="mb-3">
                <label class="form-label">Thời gian (phút)</label>
                <input v-model.number="dichVu.thoiGian" type="number" class="form-control" required min="0" />
              </div>
              <div class="mb-3">
                <label class="form-label">Hình ảnh (URL)</label>
                <input v-model="dichVu.hinhAnh" class="form-control" />
              </div>
              <div class="mb-3">
                <label class="form-label">Mô tả</label>
                <textarea v-model="dichVu.moTa" class="form-control" rows="3"></textarea>
              </div>
              <div class="mb-3">
                <label class="form-label">Mã loại dịch vụ</label>
                <input v-model.number="dichVu.loaiDichVuID" type="number" class="form-control" required min="1" />
              </div>
              <div class="mb-3">
                <label class="form-label">Trạng thái</label>
                <select v-model.number="dichVu.trangThai" class="form-select">
                  <option :value="1">Hoạt động</option>
                  <option :value="0">Tạm ngừng</option>
                </select>
              </div>
              <div class="mb-3">
                <label class="form-label">Ngày tạo</label>
                <input v-model="dichVu.ngayTao" class="form-control" type="datetime-local" readonly />
              </div>
              <div class="d-flex justify-content-end">
                <button type="submit" class="btn btn-primary me-2">
                  {{ isEditing ? "Cập nhật" : "Thêm" }}
                </button>
                <button type="button" class="btn btn-secondary" @click="resetForm">Hủy</button>
              </div>
            </form>
          </div>
        </div>
      </div>

      <!-- Danh sách dịch vụ -->
      <div class="col-md-8">
        <div class="table-responsive">
          <table class="table table-bordered table-hover align-middle text-center">
            <thead class="table-dark">
              <tr>
                <th>ID</th>
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
              <tr v-for="dv in dichVus" :key="dv.dichVuID">
                <td>{{ dv.dichVuID }}</td>
                <td>{{ dv.tenDichVu }}</td>
                <td>{{ dv.gia.toLocaleString() }} VND</td>
                <td>{{ dv.thoiGian }} phút</td>
                <td>
                  <img :src="'http://localhost:5055/' + dv.hinhAnh + '.jpg'" alt="Ảnh dịch vụ" class="img-thumbnail"
                    style="width: 100px; height: 80px; object-fit: cover" />
                </td>
                <td>{{ new Date(dv.ngayTao).toLocaleDateString() }}</td>
                <td>{{ dv.loaiDichVuID }}</td>
                <td>
                  <span class="badge" :class="dv.trangThai === 1 ? 'bg-success' : 'bg-secondary'">
                    {{ dv.trangThai === 1 ? 'Hoạt động' : 'Tạm ngừng' }}
                  </span>
                </td>
                <td>
                  <button class="btn btn-warning btn-sm me-2" @click="editDichVu(dv)">Sửa</button>
                  <button class="btn btn-danger btn-sm" @click="deleteDichVu(dv.dichVuID)">Xóa</button>
                  <button class="btn btn-sm"
                    :class="dv.trangThai === 1 ? 'btn-outline-secondary' : 'btn-outline-success'"
                    @click="toggleTrangThai(dv)">
                    {{ dv.trangThai === 1 ? 'Tạm ngừng' : 'Kích hoạt' }}
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import apiClient from "../utils/axiosClient";

const dichVus = ref([]);
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

const fetchDichVus = async () => {
  try {
    const response = await apiClient.get("DichVu");
    dichVus.value = response.data;
  } catch (error) {
    console.error("Lỗi lấy danh sách dịch vụ:", error);
  }
};
const searchByName = async () => {
  if (!searchName.value.trim()) {
    return fetchDichVus(); // Nếu không nhập gì thì hiển thị lại tất cả
  }
  try {
    const response = await apiClient.get(`DichVu/name?ten=${searchName.value}`);
    dichVus.value = response.data;
  } catch (error) {
    console.error("Không tìm thấy dịch vụ:", error);
    dichVus.value = [];
  }
};

const filterByPrice = async () => {
  if (priceMin.value < 0 || priceMax.value < 0) {
    alert("Giá không được nhỏ hơn 0.");
    return;
  }
  if (priceMin.value > priceMax.value) {
    alert("Giá thấp nhất không được lớn hơn giá cao nhất.");
    return;
  }
  try {
    const response = await apiClient.get(`DichVu/filter-by-price?min=${priceMin.value}&max=${priceMax.value}`);
    dichVus.value = response.data;
  } catch (error) {
    console.error("Không tìm thấy dịch vụ trong khoảng giá:", error);
    dichVus.value = [];
  }
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

const deleteDichVu = async (id) => {
  try {
    await apiClient.delete(`/DichVu/${id}`);
    fetchDichVus();
  } catch (error) {
    console.error("Lỗi xóa dịch vụ:", error);
  }
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

onMounted(fetchDichVus);
</script>