<template>
  <div class="container-fluid mt-1">
    <div class="row">
      <!-- Danh sách đã chọn -->
      <div class="col-md-9">
        <div class="card shadow-sm mb-3" style="height: 550px; font-size: 19px">
          <div
            class="card-header bg-primary text-white d-flex justify-content-between align-items-center flex-wrap"
          >
            <h5 class="mb-0 me-3">🛒 Danh sách đã chọn</h5>

            <!-- Nhóm label + input nằm trên 1 hàng -->
            <div
              class="d-flex align-items-center me-3"
              style="white-space: nowrap"
            >
              <label class="form-label mb-0 me-2">Số điện thoại:</label>
              <input
                type="text"
                class="form-control"
                v-model="soDienThoai"
                placeholder="Nhập số điện thoại khách hàng"
                style="width: 200px"
              />
            </div>

            <!-- Số lượng mục đã chọn -->
            <span class="badge bg-light text-dark">
              {{ danhSachChon.length }} mục
            </span>
          </div>

          <div class="card-body p-3 overflow-auto" style="max-height: 290px">
            <template v-if="danhSachChon.length > 0">
              <div
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
            </template>
            <div v-else class="text-center text-muted mt-4">
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

            <div v-if="hinhThuc === 'Tiền mặt'" class="mb-2">
              <div class="d-flex align-items-center flex-wrap gap-2">
                <label class="me-2 mb-0" style="white-space: nowrap"
                  >Tiền khách đưa:</label
                >
                <input
                  type="text"
                  class="form-control"
                  style="width: 150px"
                  v-model="tienKhachDuaHienThi"
                  @keypress="chiNhapSo"
                  @blur="xuLyNhapTienKhach"
                />
                <div v-if="tienKhachDua < tongTien" class="text-danger small">
                  ❌ Khách đưa chưa đủ tiền (thiếu
                  {{ (tongTien - tienKhachDua).toLocaleString() }} ₫)
                </div>
                <div v-else class="text-muted">
                  Tiền thối lại:
                  <strong class="text-success">
                    {{ (tienKhachDua - tongTien).toLocaleString() }} ₫
                  </strong>
                </div>
              </div>
            </div>

            <div v-if="hinhThuc === 'Chuyển khoản'" class="mb-2">
              <button
                class="btn btn-warning"
                @click="taoMaChuyenKhoan"
                :disabled="danhSachChon.length === 0"
              >
                <i class="fas fa-qrcode"></i> Tạo mã
              </button>
            </div>
            <div class="mb-2 d-flex align-items-start">
              <label
                class="form-label me-2 mt-2"
                style="white-space: nowrap; width: 70px"
              >
                Ghi chú:
              </label>
              <textarea
                class="form-control"
                v-model="ghiChu"
                rows="2"
                placeholder="Ghi chú thêm (nếu có)"
              ></textarea>
            </div>

            <div class="d-flex justify-content-between align-items-center">
              <h5 class="text-success mb-0">
                Tổng tiền: {{ tongTien.toLocaleString() }} ₫
              </h5>
              <button
                class="btn btn-outline-secondary ms-2"
                v-if="isSuaLich"
                @click="huySua"
              >
                Huỷ sửa
              </button>
              <button
                class="btn btn-outline-info"
                @click="isSuaLich ? capNhatLich() : datLich()"
                :disabled="danhSachChon.length === 0 || !soDienThoai.trim()"
              >
                <i class="fas fa-calendar-plus me-1"></i>
                {{ isSuaLich ? "Lưu thay đổi" : "Đặt lịch" }}
              </button>
              <button
                class="btn btn-success px-4"
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
        <div class="card shadow-sm" style="max-height: 550px">
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
          <div class="card-body p-2 overflow-auto" style="max-height: 500px">
            <ul class="list-group">
              <li
                v-for="item in tab === 'dichVu' ? dichVus : sanPhams"
                :key="tab === 'dichVu' ? item.dichVuID : item.sanPhamId"
                class="list-group-item d-flex justify-content-between align-items-center"
              >
                {{ tab === "dichVu" ? item.tenDichVu : item.tenSP }}
                <button
                  class="btn btn-sm btn-outline-primary"
                  @click="
                    tab === 'dichVu' ? chonDichVu(item) : chonSanPham(item)
                  "
                >
                  <i class="fas fa-plus"></i>
                </button>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>

    <!-- Danh sách đặt lịch -->
    <div class="mt-2">
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
              <th>Hành động</th>
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
                  >{{ lich.trangThai }}</span
                >
              </td>
              <td>
                <span
                  :class="lich.daThanhToan ? 'text-success' : 'text-warning'"
                  >{{
                    lich.daThanhToan ? "✔ Đã thanh toán" : "❌ Chưa thanh toán"
                  }}</span
                >
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
              <td>
                <button
                  class="btn btn-sm btn-outline-primary"
                  @click="doiTrangThai(lich.datLichID)"
                >
                  Đổi trạng thái
                </button>
                <button
                  v-if="!lich.daThanhToan"
                  class="btn btn-sm btn-outline-secondary me-2"
                  @click="batDauSuaLich(lich)"
                >
                  Sửa
                </button>
              </td>
            </tr>
            <tr v-if="danhSachDatLich.length === 0">
              <td colspan="8" class="text-center text-muted">
                Không có dữ liệu
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import axios from "axios";
import { useRoute } from "vue-router";
import dayjs from "dayjs";
const route = useRoute();

const tab = ref("dichVu");
const dichVus = ref([]);
const sanPhams = ref([]);
const danhSachChon = ref([]);
const hinhThuc = ref("Tiền mặt");
const tienKhachDua = ref(0); // số thực dùng tính toán
const tienKhachDuaHienThi = ref("");
const danhSachDatLich = ref([]);
const soDienThoai = ref("");
const ghiChu = ref("");
const isSuaLich = ref(false);
const lichDangSua = ref(null);

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
    nhanVienID: "6c5dad3a-b67c-4ad7-9ac5-ba64b0aabd5f",
    userID: "6c5dad3a-b67c-4ad7-9ac5-ba64b0aabd5f",
    chiTietHoaDon: danhSachChon.value.map((item) => ({
      sanPhamID: item.sanPhamID || null,
      dichVuID: item.dichVuID || null,
      soLuongSP: item.soLuong,
      thanhTien: item.thanhTien,
    })),
  };

  try {
    await axios.post("http://localhost:5055/api/ThanhToan/tao-hoadon", data);

    if (isSuaLich.value && lichDangSua.value?.datLichID) {
      await axios.put(
        `http://localhost:5055/api/DatLich/capnhat-thanhtoan/${lichDangSua.value.datLichID}`
      );
    }
    alert("Tạo hóa đơn thành công");
    danhSachChon.value = [];
    tienKhachDua.value = 0;
    tienKhachDuaHienThi.value = "";
    hinhThuc.value = "Tiền mặt";
    localStorage.removeItem("checkoutData");
    layDanhSach();
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
const layDanhSach = async () => {
  try {
    const resDL = await axios.get("https://localhost:7183/api/DatLich");
    danhSachDatLich.value = resDL.data;

    const resDV = await axios.get("http://localhost:5055/api/DichVu");
    dichVus.value = resDV.data;

    const resSP = await axios.get("http://localhost:5055/api/Product");
    sanPhams.value = resSP.data;
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

const datLich = async () => {
  const dichVuIDs = danhSachChon.value
    .filter((item) => item.dichVuID)
    .map((item) => item.dichVuID);

  if (dichVuIDs.length === 0) {
    alert("⚠ Vui lòng chọn ít nhất một dịch vụ để đặt lịch.");
    return;
  }

  if (!soDienThoai.value.trim()) {
    alert("⚠ Vui lòng nhập số điện thoại khách hàng.");
    return;
  }

  const payload = {
    soDienThoai: soDienThoai.value,
    thoiGian: dayjs().format("YYYY-MM-DDTHH:mm:ss"),
    dichVuIDs: dichVuIDs,
    ghiChu: ghiChu.value,
  };

  try {
    const res = await axios.post("http://localhost:5055/api/DatLich", payload);
    alert("📅 Đặt lịch thành công!");

    // Reset
    soDienThoai.value = "";
    ghiChu.value = "";
    danhSachChon.value = [];
    layDanhSach(); // cập nhật lại danh sách
  } catch (err) {
    console.error("Lỗi đặt lịch:", err);
    alert("❌ Đặt lịch thất bại. Vui lòng thử lại.");
  }
};
const doiTrangThai = async (id) => {
  try {
    const res = await axios.put(
      `http://localhost:5055/api/DatLich/doitrangthai/${id}`
    );
    layDanhSach();
  } catch (err) {
    alert("Lỗi khi đổi trạng thái!");
    console.error(err);
  }
};
const batDauSuaLich = (lich) => {
  isSuaLich.value = true;
  lichDangSua.value = lich;

  soDienThoai.value = lich.soDienThoai;
  ghiChu.value = lich.ghiChu;

  // Chuyển các dịch vụ đã chọn vào danh sách chọn
  danhSachChon.value = lich.chiTietDichVus.map((ct) => ({
    ten: ct.dichVu.tenDichVu,
    dichVuID: ct.dichVuID,
    soLuong: 1,
    donGia: ct.dichVu.gia,
    thanhTien: ct.dichVu.gia,
  }));
};

const capNhatLich = async () => {
  const dichVuIDs = danhSachChon.value
    .filter((item) => item.dichVuID)
    .map((item) => item.dichVuID);

  if (dichVuIDs.length === 0) {
    alert("⚠ Vui lòng chọn ít nhất một dịch vụ để cập nhật.");
    return;
  }

  const payload = {
    soDienThoai: soDienThoai.value,
    thoiGian: lichDangSua.value.thoiGian,
    dichVuIDs,
    ghiChu: ghiChu.value,
  };

  try {
    await axios.put(
      `http://localhost:5055/api/DatLich/${lichDangSua.value.datLichID}`,
      payload
    );
    alert("✅ Cập nhật lịch thành công!");
    isSuaLich.value = false;
    lichDangSua.value = null;
    danhSachChon.value = [];
    soDienThoai.value = "";
    ghiChu.value = "";
    layDanhSach();
  } catch (err) {
    console.error("❌ Lỗi cập nhật lịch:", err);
    alert("❌ Cập nhật lịch thất bại.");
  }
};
const huySua = () => {
  isSuaLich.value = false;
  lichDangSua.value = null;
  danhSachChon.value = [];
  soDienThoai.value = "";
  ghiChu.value = "";
};

onMounted(async () => {
  try {
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
    layDanhSach();
  } catch (err) {
    console.error("Lỗi tải dữ liệu:", err);
  }
});
</script>
