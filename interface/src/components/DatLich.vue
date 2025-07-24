<template>
  <div class="container mt-5 pb-5">
    <h2 class="mb-4 text-center text-primary fw-bold">
      🧖‍♀️ Đặt lịch hẹn tại Spa
    </h2>

    <!-- 💆‍♀️ Chọn dịch vụ -->
    <div class="card mb-4 shadow-sm">
      <div class="card-header bg-info text-white fw-semibold">
        💆‍♀️ Chọn dịch vụ (có thể chọn nhiều hoặc để nhân viên tư vấn)
      </div>
      <div class="card-body">
        <!-- Search Box -->
        <input
          type="text"
          v-model="tuKhoa"
          placeholder="🔍 Tìm dịch vụ..."
          class="form-control mb-3"
        />

        <!-- Danh sách dịch vụ -->
        <div class="row dich-vu-list">
          <div v-for="dv in dichVuLoc" :key="dv.dichVuID" class="col-md-4 mb-4">
            <div
              class="card h-100 shadow-sm position-relative"
              :class="{
                'border border-primary border-3 bg-light': dichVuIDs.includes(
                  dv.dichVuID
                ),
              }"
              @click="toggleDichVu(dv.dichVuID)"
              style="cursor: pointer"
            >
              <img
                :src="
                  dv.hinhAnh.startsWith('http')
                    ? dv.hinhAnh
                    : '/images/' + dv.hinhAnh
                "
                class="card-img-top"
                style="height: 180px; object-fit: cover"
              />
              <div class="card-body">
                <h5 class="card-title text-primary">{{ dv.tenDichVu }}</h5>
                <p class="card-text small">{{ dv.moTa }}</p>
                <p class="text-muted">
                  💸 {{ dv.gia.toLocaleString() }}đ – ⏱️ {{ dv.thoiGian }} phút
                </p>
              </div>
              <span
                v-if="dichVuIDs.includes(dv.dichVuID)"
                class="position-absolute top-0 end-0 badge bg-success rounded-pill m-2"
              >
                ✅ Đã chọn
              </span>
            </div>
          </div>
        </div>

        <!-- Nút không chọn dịch vụ -->
        <div class="text-center mt-3">
          <button
            class="btn"
            :class="
              dichVuIDs.length === 0
                ? 'btn-info text-white fw-bold'
                : 'btn-outline-info'
            "
            @click="dichVuIDs = []"
          >
            💬 Không chọn dịch vụ trước – Nhân viên tư vấn sau
          </button>
        </div>
      </div>
    </div>

    <!-- 📆 Chọn ngày & giờ -->
    <div class="card mb-4 shadow-sm">
      <div class="card-header bg-info text-white fw-semibold">
        📆 Chọn ngày & khung giờ
      </div>
      <div class="card-body row g-3">
        <div class="col-md-6">
          <label class="form-label fw-semibold">📅 Ngày đến spa</label>
          <input
            type="date"
            v-model="ngay"
            class="form-control"
            @change="layKhungGio"
          />
        </div>
        <div class="col-md-6">
          <label class="form-label fw-semibold">🕒 Khung giờ</label>
          <div class="d-flex flex-wrap gap-2">
            <button
              v-for="gio in danhSachKhungGio"
              :key="gio.khungGio"
              class="btn btn-sm"
              :class="[
                gio.khungGio === khungGioChon
                  ? 'btn-success'
                  : 'btn-outline-secondary',
                gio.conLai === 0
                  ? 'text-decoration-line-through text-danger'
                  : '',
              ]"
              :disabled="gio.conLai === 0"
              @click="khungGioChon = gio.khungGio"
            >
              {{ gio.khungGio }} <span v-if="gio.conLai === 0">❌ Hết</span>
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- ☎️ Nhập số điện thoại -->
    <div class="card mb-4 shadow-sm">
      <div class="card-header bg-info text-white fw-semibold">
        📞 Nhập số điện thoại
      </div>
      <div class="card-body">
        <input
          v-model="soDienThoai"
          type="tel"
          class="form-control"
          placeholder="📱 Nhập số điện thoại của bạn"
        />
      </div>
    </div>

    <!-- ✅ Xác nhận -->
    <div class="text-center">
      <button
        class="btn btn-lg btn-primary px-5 fw-bold"
        :disabled="!khungGioChon || !soDienThoai"
        @click="datLich"
      >
        ✅ Xác nhận đặt lịch
      </button>
    </div>

    <!-- Thông báo -->
    <div
      v-if="thongBao"
      class="alert mt-4 text-center"
      :class="{
        'alert-success': thongBao.startsWith('🎉'),
        'alert-danger': thongBao.startsWith('❌'),
      }"
    >
      {{ thongBao }}
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "DatLichCard",
  data() {
    return {
      danhSachDichVu: [],
      tuKhoa: "",
      dichVuIDs: [],
      ngay: new Date().toISOString().substring(0, 10),
      danhSachKhungGio: [],
      khungGioChon: "",
      soDienThoai: "",
      thongBao: "",
    };
  },
  computed: {
    dichVuLoc() {
      return this.danhSachDichVu.filter((dv) =>
        dv.tenDichVu.toLowerCase().includes(this.tuKhoa.toLowerCase())
      );
    },
  },
  methods: {
    async layDichVu() {
      const res = await axios.get("http://localhost:5055/api/dichvu");
      this.danhSachDichVu = res.data;
    },
    async layKhungGio() {
      const res = await axios.get("http://localhost:5055/api/datlich/slots", {
        params: { ngay: this.ngay },
      });
      this.danhSachKhungGio = res.data;
    },
    toggleDichVu(id) {
      const index = this.dichVuIDs.indexOf(id);
      if (index === -1) this.dichVuIDs.push(id);
      else this.dichVuIDs.splice(index, 1);
    },
    async datLich() {
      const thoiGian = `${this.ngay}T${this.khungGioChon}:00`;

      try {
        await axios.post("http://localhost:5055/api/DatLich", {
          soDienThoai: this.soDienThoai,
          thoiGian,
          dichVuIDs: this.dichVuIDs,
        });

        this.thongBao =
          "🎉 Đặt lịch thành công! Nhân viên sẽ tư vấn nếu bạn chưa chọn dịch vụ.";
        this.khungGioChon = "";
        this.dichVuIDs = [];
        this.soDienThoai = "";
        await this.layKhungGio();
      } catch (err) {
        if (err.response?.status === 400) {
          this.thongBao = "❌ " + err.response.data;
        } else {
          this.thongBao = "❌ Đã có lỗi xảy ra. Vui lòng thử lại sau.";
        }
      }
    },
  },
  mounted() {
    this.layDichVu();
    this.layKhungGio();
  },
};
</script>

<style scoped>
.dich-vu-list {
  max-height: 500px;
  overflow-y: auto;
}
.card:hover {
  transform: scale(1.01);
  transition: 0.2s;
}
</style>
