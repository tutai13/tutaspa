<template>
  <div>
    <h4 class="mb-3">📋 Danh sách đặt lịch</h4>
    <div class="table-responsive">
      <table class="table table-bordered table-hover align-middle">
        <thead class="table-primary">
          <tr>
            <th>#</th>
            <th>SĐT</th>
            <th>Thời gian</th>
            <th>Thời lượng</th>
            <th>Trạng thái</th>
            <th>Thanh toán</th>
            <th>Dịch vụ</th>
            <th>Ghi Chú</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(lich, index) in danhSachDatLich" :key="lich.datLichID">
            <td>{{ index + 1 }}</td>
            <td>{{ lich.soDienThoai }}</td>
            <td>{{ formatDateTime(lich.thoiGian) }}</td>
            <td>{{ lich.thoiLuong }} phút</td>
            <td>
              <span
                :class="
                  lich.trangThai === 'Đã đến' ? 'text-success' : 'text-danger'
                "
              >
                {{ lich.trangThai }}
              </span>
            </td>
            <td>
              <span :class="lich.daThanhToan ? 'text-success' : 'text-warning'">
                {{
                  lich.daThanhToan ? "✔ Đã thanh toán" : "❌ Chưa thanh toán"
                }}
              </span>
            </td>
            <td>
              <ul class="mb-0 ps-3">
                <li
                  v-for="dv in lich.chiTietDichVus"
                  :key="dv.chiTietDatLichID"
                >
                  {{ dv.dichVu.tenDichVu }} - {{ dv.dichVu.thoiGian }}p -
                  {{ dv.dichVu.gia.toLocaleString() }}₫
                </li>
              </ul>
            </td>
            <td>{{ lich.ghiChu }}</td>
          </tr>
          <tr v-if="danhSachDatLich.length === 0">
            <td colspan="8" class="text-center text-muted">Không có dữ liệu</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import axios from "axios";

const danhSachDatLich = ref([]);

const layDanhSach = async () => {
  try {
    const res = await axios.get("https://localhost:7183/api/DatLich");
    danhSachDatLich.value = res.data;
  } catch (err) {
    console.error("Lỗi lấy danh sách:", err);
  }
};

const formatDateTime = (dateStr) => {
  const date = new Date(dateStr);
  return `${date.toLocaleTimeString([], {
    hour: "2-digit",
    minute: "2-digit",
  })} - ${date.toLocaleDateString("vi-VN")}`;
};

onMounted(() => {
  layDanhSach();
});
</script>
