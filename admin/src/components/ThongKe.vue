<template>
  <div>
    <h4 class="mb-3">📋 Danh sách hóa đơn</h4>
    <div class="table-responsive">
      <table class="table table-bordered table-hover align-middle">
        <thead class="table-primary">
          <tr>
            <th>#</th>
            <th>Ngày tạo</th>
            <th>Tổng tiền</th>
            <th>Khách đưa</th>
            <th>Thối lại</th>
            <th>Hình thức</th>
            <th>Trạng thái</th>
            <th>Dịch vụ</th>
            <th>Tải hóa đơn</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(hd, index) in danhSachHoaDon" :key="hd.hoaDonID">
            <td>{{ index + 1 }}</td>
            <td>{{ formatDateTime(hd.ngayTao) }}</td>
            <td>{{ hd.tongTien.toLocaleString() }}₫</td>
            <td>{{ hd.tienKhachDua?.toLocaleString() ?? "—" }}₫</td>
            <td>{{ hd.tienThoiLai?.toLocaleString() ?? "—" }}₫</td>
            <td>{{ hd.hinhThucThanhToan }}</td>
            <td>
              <span
                :class="hd.trangThai === 1 ? 'text-success' : 'text-danger'"
              >
                {{ hd.trangThai === 1 ? "✔ Hoàn tất" : "⏳ Chờ xử lý" }}
              </span>
            </td>
            <td>
              <ul class="mb-0 ps-3">
                <li v-for="ct in hd.chiTietHoaDons" :key="ct.chiTietHoaDonID">
                  {{ ct.dichVu?.tenDichVu ?? "—" }} -
                  {{ ct.dichVu?.thoiGian ?? 0 }}p -
                  {{ ct.thanhTien?.toLocaleString() }}₫
                </li>
              </ul>
            </td>
            <td>
              <button
                class="btn btn-sm btn-outline-primary"
                @click="taiHoaDon(hd.hoaDonID)"
              >
                ⬇️ Tải
              </button>
            </td>
          </tr>
          <tr v-if="danhSachHoaDon.length === 0">
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

const danhSachHoaDon = ref([]);

const layDanhSach = async () => {
  try {
    const res = await axios.get(
      "https://localhost:7183/api/ThongKe/thongKeHoaDon"
    );
    danhSachHoaDon.value = res.data;
  } catch (err) {
    console.error("Lỗi lấy danh sách hóa đơn:", err);
  }
};

const formatDateTime = (dateStr) => {
  const date = new Date(dateStr);
  return `${date.toLocaleTimeString([], {
    hour: "2-digit",
    minute: "2-digit",
  })} - ${date.toLocaleDateString("vi-VN")}`;
};
const taiHoaDon = async (hoaDonID) => {
  try {
    const response = await axios.get(
      `https://localhost:7183/api/ThanhToan/xuat-hoadon/${hoaDonID}`,
      { responseType: "blob" } // để nhận file PDF
    );

    const url = window.URL.createObjectURL(new Blob([response.data]));
    const link = document.createElement("a");
    link.href = url;
    link.setAttribute("download", `HoaDon_${hoaDonID}.pdf`);
    document.body.appendChild(link);
    link.click();
    link.remove();
  } catch (error) {
    console.error("Lỗi khi tải hóa đơn:", error);
    alert("Không thể tải hóa đơn.");
  }
};

onMounted(() => {
  layDanhSach();
});
</script>
