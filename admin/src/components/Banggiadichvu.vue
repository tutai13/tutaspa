// BangGiaDichVu.vue
<template>
  <div class="container py-4">
    <h2 class="text-center text-primary fw-bold mb-4">
      <i class="fa fa-tags me-2"></i> Quản lý Bảng Giá Dịch Vụ
    </h2>

    <!-- Form thêm/sửa bảng giá -->
    <div class="card shadow-sm mb-4">
      <div class="card-header bg-primary text-white fw-bold">
        {{ isEditing ? 'Cập nhật Giá' : 'Thêm Giá Mới' }}
      </div>
      <div class="card-body">
        <form @submit.prevent="saveBangGia">
          <div class="row g-3">
            <div class="col-md-4">
              <label class="form-label">Dịch vụ</label>
              <select v-model="bangGia.dichVuID" class="form-select" required>
                <option disabled value="">-- Chọn dịch vụ --</option>
                <option v-for="dv in dichVus" :key="dv.dichVuID" :value="dv.dichVuID">
                  {{ dv.tenDichVu }}
                </option>
              </select>
            </div>
            <div class="col-md-2">
              <label class="form-label">Thời lượng (phút)</label>
              <input v-model="bangGia.thoiLuong" type="number" class="form-control" min="0" :disabled="bangGia.kieuTinhGia === 1" />
            </div>
            <div class="col-md-3">
              <label class="form-label">Giá (VND)</label>
              <input v-model="bangGia.gia" type="number" class="form-control" min="0" required />
            </div>
            <div class="col-md-3">
              <label class="form-label">Kiểu tính giá</label>
              <select v-model="bangGia.kieuTinhGia" class="form-select">
                <option :value="0">Theo thời gian</option>
                <option :value="1">Theo quy trình</option>
              </select>
            </div>
          </div>
          <div class="d-flex justify-content-end gap-2 mt-3">
            <button class="btn btn-primary" type="submit">
              <i class="fa fa-save me-1"></i> {{ isEditing ? 'Cập nhật' : 'Thêm mới' }}
            </button>
            <button class="btn btn-secondary" type="button" @click="resetForm">
              <i class="fa fa-undo me-1"></i> Hủy
            </button>
          </div>
        </form>
      </div>
    </div>

    <!-- Danh sách bảng giá -->
    <div class="card shadow-sm">
      <div class="card-header bg-dark text-white fw-bold">
        <i class="fa fa-table me-1"></i> Danh sách Bảng Giá
      </div>
      <div class="table-responsive">
        <table class="table table-bordered table-hover mb-0 text-center align-middle">
          <thead class="table-light">
            <tr>
              <th>Dịch vụ</th>
              <th>Thời lượng</th>
              <th>Giá</th>
              <th>Kiểu tính giá</th>
              <th>Hiển thị</th>
              <th>Hành động</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in bangGias" :key="item.id">
              <td>{{ item.tenDichVu || 'Không rõ' }}</td>
              <td>{{ item.kieuTinhGia === 0 ? item.thoiLuong + ' phút' : '-' }}</td>
              <td>{{ formatCurrency(item.gia) }}</td>
              <td>{{ item.kieuTinhGia === 0 ? 'Theo thời gian' : 'Theo quy trình' }}</td>
              <td>
                <span class="badge" :class="item.isVisible ? 'bg-success' : 'bg-secondary'">
                  <!-- {{ item.isVisible ? 'Hiện' : 'Ẩn' }} -->
                </span>
                <button class="btn btn-sm" :class="item.isVisible ? 'btn-outline-warning' : 'btn-outline-success'" @click="toggleHienGia(item.id)">
                  <i class="fa" :class="item.isVisible ? 'fa-eye-slash' : 'fa-eye'"></i>
                </button>
              </td>
              <td>
                <button class="btn btn-warning btn-sm me-2" @click="editBangGia(item)">
                  <i class="fa fa-edit"></i>
                </button>
                <!-- <button class="btn btn-danger btn-sm" @click="deleteBangGia(item.id)">
                  <i class="fa fa-trash"></i>
                </button> -->
              </td>
            </tr>
            <tr v-if="!bangGias.length">
              <td colspan="6" class="text-muted">Chưa có bảng giá nào</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'

const bangGias = ref([])
const dichVus = ref([])
const isEditing = ref(false)

const bangGia = ref({
  id: 0,
  dichVuID: '',
  thoiLuong: null,
  gia: 0,
  kieuTinhGia: 0,
})

const fetchBangGias = async () => {
  try {
    const res = await axios.get('http://localhost:5055/api/BangGiaDichVu')
    bangGias.value = res.data
  } catch (err) {
    console.error('Lỗi khi lấy danh sách bảng giá:', err)
  }
}

const fetchDichVus = async () => {
  try {
    const res = await axios.get('http://localhost:5055/api/DichVu/all')
    dichVus.value = res.data
  } catch (err) {
    console.error('Lỗi khi lấy danh sách dịch vụ:', err)
  }
}

const saveBangGia = async () => {
  try {
    if (isEditing.value) {
      await axios.put(`http://localhost:5055/api/BangGiaDichVu/UpdateGiaDichVu/${bangGia.value.id}`, bangGia.value)
    } else {
      await axios.post('http://localhost:5055/api/BangGiaDichVu/AddGiaDichVu', bangGia.value)
    }
    resetForm()
    fetchBangGias()
  } catch (err) {
    console.error('Lỗi khi lưu bảng giá:', err)
  }
}

const toggleHienGia = async (id) => {
  try {
    await axios.put(`http://localhost:5055/api/BangGiaDichVu/ToggleGiaDichVu/${id}`)
    fetchBangGias()
  } catch (err) {
    console.error('Lỗi khi bật/tắt hiển thị:', err)
  }
}

const editBangGia = (item) => {
  bangGia.value = { ...item }
  isEditing.value = true
}

const deleteBangGia = async (id) => {
  if (!confirm('Bạn có chắc muốn xoá?')) return
  try {
    await axios.delete(`http://localhost:5055/api/BangGiaDichVu/DeleteGiaDichVu/${id}`)
    fetchBangGias()
  } catch (err) {
    console.error('Lỗi khi xoá bảng giá:', err)
  }
}

const resetForm = () => {
  bangGia.value = {
    id: 0,
    dichVuID: '',
    thoiLuong: null,
    gia: 0,
    kieuTinhGia: 0,
  }
  isEditing.value = false
}

const formatCurrency = (value) =>
  new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value)

onMounted(() => {
  fetchBangGias()
  fetchDichVus()
})
</script>
