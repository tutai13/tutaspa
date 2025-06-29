<template>
  <div class="container-fluid mt-4">
    <div class="row">
      <!-- Danh sách đã chọn -->
      <div class="col-md-9" style="font-size: 19px">
        <div class="card shadow-sm mb-3">
          <div
            class="card-header bg-primary text-white d-flex justify-content-between align-items-center"
          >
            <h5 class="mb-0">🛒 Danh sách đã chọn</h5>
            <span class="badge bg-light text-dark"
              >{{ danhSachChon.length }} mục</span
            >
          </div>
          <div
            class="card-body p-3"
            style="max-height: 500px; overflow-y: auto"
          >
            <div
              v-if="danhSachChon.length > 0"
              v-for="(item, index) in danhSachChon"
              :key="index"
              class="card mb-3 border border-secondary-subtle"
            >
              <div class="card-body p-2">
                <div class="row align-items-center">
                  <div class="col-md-5">
                    <h5 class="mb-1">{{ item.ten }}</h5>
                    <small class="text-muted"
                      >Đơn giá: {{ item.donGia.toLocaleString() }} ₫</small
                    >
                  </div>
                  <div class="col-md-4">
                    <div class="input-group input-group-sm">
                      <button
                        class="btn btn-outline-secondary"
                        @click="
                          item.soLuong > 1 && item.soLuong--;
                          capNhatThanhTien(index);
                        "
                      >
                        −
                      </button>
                      <input
                        type="number"
                        min="1"
                        class="form-control text-center"
                        v-model.number="item.soLuong"
                        @input="capNhatThanhTien(index)"
                      />
                      <button
                        class="btn btn-outline-primary"
                        @click="
                          item.soLuong++;
                          capNhatThanhTien(index);
                        "
                      >
                        +
                      </button>
                    </div>
                    <div class="mt-1 small">
                      Thành tiền:
                      <strong>{{ item.thanhTien.toLocaleString() }} ₫</strong>
                    </div>
                  </div>
                  <div class="col-md-3 text-end">
                    <button
                      class="btn btn-sm btn-danger"
                      @click="xoaItem(index)"
                    >
                      <i class="fas fa-trash-alt"></i> Xóa
                    </button>
                  </div>
                </div>
              </div>
            </div>

            <div
              v-if="danhSachChon.length === 0"
              class="text-center text-muted mt-4"
            >
              <i class="fas fa-box-open fa-2x"></i>
              <p class="mt-2">Chưa có dịch vụ hoặc sản phẩm nào được chọn.</p>
            </div>
          </div>

          <!-- card-footer -->
          <div class="card-footer bg-light">
            <div class="mb-2">
              <label class="form-label me-3">Hình thức thanh toán:</label>
              <div class="form-check form-check-inline">
                <input
                  class="form-check-input"
                  type="radio"
                  id="tienMat"
                  value="Tiền mặt"
                  v-model="hinhThuc"
                />
                <label class="form-check-label" for="tienMat">Tiền mặt</label>
              </div>
              <div class="form-check form-check-inline">
                <input
                  class="form-check-input"
                  type="radio"
                  id="chuyenKhoan"
                  value="Chuyển khoản"
                  v-model="hinhThuc"
                />
                <label class="form-check-label" for="chuyenKhoan"
                  >Chuyển khoản</label
                >
              </div>
            </div>

            <!-- Nếu là Tiền mặt -->
            <div v-if="hinhThuc === 'Tiền mặt'" class="mb-2">
              <label>Tiền khách đưa:</label>
              <input
                type="text"
                class="form-control"
                v-model="tienKhachDuaHienThi"
                @keypress="chiNhapSo"
                @blur="xuLyNhapTienKhach"
              />
              <div class="mt-1" v-if="tienKhachDua < tongTien">
                <small class="text-danger"
                  >❌ Khách đưa chưa đủ tiền (thiếu
                  {{ (tongTien - tienKhachDua).toLocaleString() }} ₫)</small
                >
              </div>
              <div class="mt-1 text-muted" v-else>
                Tiền thối lại:
                <strong class="text-success"
                  >{{ (tienKhachDua - tongTien).toLocaleString() }} ₫</strong
                >
              </div>
            </div>

            <!-- Nếu là Chuyển khoản -->
            <div v-if="hinhThuc === 'Chuyển khoản'" class="mb-2">
              <button
                class="btn btn-warning"
                @click="taoMaChuyenKhoan"
                :disabled="danhSachChon.length === 0"
              >
                <i class="fas fa-qrcode"></i> Tạo mã
              </button>
            </div>

            <div class="text-end">
              <h5 class="text-success">
                Tổng tiền: {{ tongTien.toLocaleString() }} ₫
              </h5>
              <button
                class="btn btn-success mt-2 px-4"
                @click="taoThanhToan"
                :disabled="
                  danhSachChon.length === 0 ||
                  (hinhThuc === 'Tiền mặt' && tienKhachDua < tongTien) ||
                  hinhThuc === 'Chuyển khoản'
                "
              >
                <i class="fas fa-money-bill-wave me-1"></i> Tạo thanh toán
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Tabs: Dịch vụ / Sản phẩm -->
      <div class="col-md-3">
        <div class="card shadow-sm">
          <div class="card-header p-0">
            <ul class="nav nav-tabs nav-fill" role="tablist">
              <li class="nav-item">
                <a
                  class="nav-link"
                  :class="{ active: tab === 'dichVu' }"
                  href="#"
                  @click.prevent="tab = 'dichVu'"
                  >Dịch vụ</a
                >
              </li>
              <li class="nav-item">
                <a
                  class="nav-link"
                  :class="{ active: tab === 'sanPham' }"
                  href="#"
                  @click.prevent="tab = 'sanPham'"
                  >Sản phẩm</a
                >
              </li>
            </ul>
          </div>
          <div
            class="card-body p-2"
            style="max-height: 500px; overflow-y: auto"
          >
            <div v-if="tab === 'dichVu'">
              <ul class="list-group">
                <li
                  v-for="dv in dichVus"
                  :key="dv.dichVuID"
                  class="list-group-item d-flex justify-content-between align-items-center"
                >
                  {{ dv.tenDichVu }}
                  <button
                    class="btn btn-sm btn-outline-primary"
                    @click="chonDichVu(dv)"
                  >
                    <i class="fas fa-plus"></i>
                  </button>
                </li>
              </ul>
            </div>

            <div v-if="tab === 'sanPham'">
              <ul class="list-group">
                <li
                  v-for="sp in sanPhams"
                  :key="sp.sanPhamId"
                  class="list-group-item d-flex justify-content-between align-items-center"
                >
                  {{ sp.tenSP }}
                  <button
                    class="btn btn-sm btn-outline-primary"
                    @click="chonSanPham(sp)"
                  >
                    <i class="fas fa-plus"></i>
                  </button>
                </li>
              </ul>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import axios from "axios";
import { useRoute } from "vue-router";
const route = useRoute();

const tab = ref("dichVu");
const dichVus = ref([]);
const sanPhams = ref([]);
const danhSachChon = ref([]);
const hinhThuc = ref("Tiền mặt");
const tienKhachDua = ref(0); // số thực dùng tính toán
const tienKhachDuaHienThi = ref(""); // hiển thị định dạng 1.000.000

const xuLyNhapTienKhach = () => {
  const so = parseInt(tienKhachDuaHienThi.value.replace(/\D/g, ""));
  tienKhachDua.value = isNaN(so) ? 0 : so;
  tienKhachDuaHienThi.value = tienKhachDua.value.toLocaleString();
};

const chiNhapSo = (e) => {
  const char = String.fromCharCode(e.which);
  if (!/[0-9]/.test(char)) e.preventDefault();
};

const tongTien = computed(() => {
  return danhSachChon.value.reduce((sum, item) => sum + item.thanhTien, 0);
});

const chonDichVu = (dv) => {
  const existing = danhSachChon.value.find((x) => x.dichVuID === dv.dichVuID);
  if (existing) {
    existing.soLuong += 1;
    existing.thanhTien += dv.gia;
  } else {
    danhSachChon.value.push({
      ten: dv.tenDichVu,
      dichVuID: dv.dichVuID,
      soLuong: 1,
      donGia: dv.gia,
      thanhTien: dv.gia,
    });
  }
};

const chonSanPham = (sp) => {
  const existing = danhSachChon.value.find((x) => x.sanPhamID === sp.sanPhamId);
  if (existing) {
    existing.soLuong += 1;
    existing.thanhTien += sp.gia;
  } else {
    danhSachChon.value.push({
      ten: sp.tenSP,
      sanPhamID: sp.sanPhamId,
      soLuong: 1,
      donGia: sp.gia,
      thanhTien: sp.gia,
    });
  }
};

const capNhatThanhTien = (index) => {
  const item = danhSachChon.value[index];
  if (item.soLuong < 1 || isNaN(item.soLuong)) {
    item.soLuong = 1;
  }
  item.thanhTien = item.soLuong * item.donGia;
};

const xoaItem = (index) => {
  danhSachChon.value.splice(index, 1);
};

const taoThanhToan = async () => {
  const tienKhach = hinhThuc.value === "Tiền mặt" ? tienKhachDua.value : null;
  const tienThoi =
    hinhThuc.value === "Tiền mặt"
      ? Math.max(0, tienKhach - tongTien.value)
      : null;
  const data = {
    ngayTao: new Date().toISOString(),
    maGiamGia: "GG20",
    hinhThucThanhToan: hinhThuc.value,
    trangThai: 1,
    tienKhachDua: tienKhach,
    tienThoiLai: tienThoi,
    nhanVienID: 2,
    userID: "555cd045-8676-4a93-8021-4685218d9f61",
    chiTietHoaDon: danhSachChon.value.map((item) => ({
      sanPhamID: item.sanPhamID || null,
      dichVuID: item.dichVuID || null,
      soLuongSP: item.soLuong,
      thanhTien: item.thanhTien,
    })),
  };

  try {
    await axios.post("http://localhost:5055/api/ThanhToan/tao-hoadon", data);
    alert("Tạo hóa đơn thành công");
    danhSachChon.value = [];
    tienKhachDua.value = 0;
    tienKhachDuaHienThi.value = "";
    hinhThuc.value = "Tiền mặt";
    localStorage.removeItem("checkoutData");
  } catch (err) {
    console.error("Lỗi tạo hóa đơn:", err);
  }
};

const taoMaChuyenKhoan = async () => {
  const items = danhSachChon.value.map((item) => ({
    name: item.ten,
    quantity: item.soLuong,
    amount: item.donGia,
  }));

  const payload = {
    totalAmount: tongTien.value,
    description: "Thanh toán hóa đơn",
    items: items,
    cancelUrl: window.location.href,
    returnUrl: window.location.href,
  };
  localStorage.setItem(
    "checkoutData",
    JSON.stringify({
      danhSachChon: danhSachChon.value,
      hinhThuc: hinhThuc.value,
    })
  );

  try {
    const response = await axios.post(
      "http://localhost:5055/api/ThanhToan/create-link",
      payload
    );

    if (response.data && response.data.checkoutUrl) {
      window.location.href = response.data.checkoutUrl;
    } else {
      console.error("Không có checkoutUrl trong phản hồi:", response.data);
    }
  } catch (error) {
    console.error("Lỗi tạo mã thanh toán:", error);
    toast.error("Có lỗi xảy ra khi tạo mã chuyển khoản.");
  }
};

onMounted(async () => {
  try {
    const resDV = await axios.get("http://localhost:5055/api/DichVu");
    dichVus.value = resDV.data;

    const resSP = await axios.get("http://localhost:5055/api/Product");
    sanPhams.value = resSP.data;

    const status = route.query.status;

    const storedData = localStorage.getItem("checkoutData");
    if (storedData) {
      const parsed = JSON.parse(storedData);
      danhSachChon.value = parsed.danhSachChon;
      hinhThuc.value = parsed.hinhThuc;

      if (status === "PAID") {
        await taoThanhToan();
        //localStorage.removeItem("checkoutData");
      }
    }
    localStorage.removeItem("checkoutData");
  } catch (err) {
    console.error("Lỗi tải dữ liệu:", err);
  }
});
</script>
