<template>
  <div class="container py-5">
    <div class="text-center mb-4">
      <h2 class="text-danger fw-bold">📅 Đặt lịch hẹn tại Spa</h2>
      <p class="text-muted">Chọn giờ đến hoặc dịch vụ bạn muốn trải nghiệm</p>
    </div>

    <div class="row g-4">
      <!-- Form đặt lịch -->
      <div class="col-md-6">
        <div class="card shadow border-0 rounded-4">
          <div class="card-body">
            <h5 class="card-title text-dark mb-3">🕓 Đặt lịch theo giờ đến</h5>
            <form @submit.prevent="submitByTime">
              <div class="mb-3">
                <label class="form-label">Họ tên</label>
                <input type="text" class="form-control" v-model="booking.name" required />
              </div>
              <div class="mb-3">
                <label class="form-label">Số điện thoại</label>
                <input type="tel" class="form-control" v-model="booking.phone" required />
              </div>
              <div class="mb-3">
                <label class="form-label">Ngày đặt</label>
                <input type="date" class="form-control" v-model="booking.date" required />
              </div>
              <div class="mb-3">
                <label class="form-label">Giờ đến</label>
                <input type="time" class="form-control" v-model="booking.time" required />
              </div>
              <button type="submit" class="btn btn-danger w-100">📝 Đặt lịch đến quán</button>
            </form>
          </div>
        </div>
      </div>

      <!-- Form đặt lịch dịch vụ -->
      <div class="col-md-6">
        <div class="card shadow border-0 rounded-4">
          <div class="card-body">
            <h5 class="card-title text-dark mb-3">💆‍♀️ Đặt lịch dịch vụ spa</h5>
            <form @submit.prevent="submitByService">
              <div class="mb-3">
                <label class="form-label">Họ tên</label>
                <input type="text" class="form-control" v-model="service.name" required />
              </div>
              <div class="mb-3">
                <label class="form-label">Số điện thoại</label>
                <input type="tel" class="form-control" v-model="service.phone" required />
              </div>
              <div class="mb-3">
                <label class="form-label">Chọn dịch vụ</label>
                <select class="form-select" v-model="service.selectedService" required>
                  <option disabled value="">-- Chọn dịch vụ --</option>
                  <option v-for="item in services" :key="item">{{ item }}</option>
                </select>
              </div>
              <div class="mb-3">
                <label class="form-label">Ngày hẹn</label>
                <input type="date" class="form-control" v-model="service.date" required />
              </div>
              <button type="submit" class="btn btn-danger w-100">🧖 Đặt dịch vụ</button>
            </form>
          </div>
        </div>
      </div>
    </div>

    <!-- Thông báo -->
    <div v-if="message" class="alert alert-success mt-4 text-center" role="alert">
      {{ message }}
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      booking: {
        name: "",
        phone: "",
        date: "",
        time: ""
      },
      service: {
        name: "",
        phone: "",
        selectedService: "",
        date: ""
      },
      services: [
        "Massage thư giãn",
        "Chăm sóc da mặt",
        "Trị liệu toàn thân",
        "Tắm trắng",
        "Triệt lông"
      ],
      message: ""
    };
  },
  methods: {
    submitByTime() {
      // Gửi API đặt lịch đến quán
      console.log("Đặt giờ:", this.booking);
      this.message = "✅ Đã đặt lịch đến quán thành công!";
      setTimeout(() => (this.message = ""), 4000);
    },
    submitByService() {
      // Gửi API đặt dịch vụ
      console.log("Đặt dịch vụ:", this.service);
      this.message = "✅ Đã đặt lịch dịch vụ thành công!";
      setTimeout(() => (this.message = ""), 4000);
    }
  }
};
</script>

<style scoped>
.card {
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}
.card:hover {
  transform: scale(1.02);
  box-shadow: 0 0 20px rgba(128, 0, 64, 0.2);
}
.btn-danger {
  background-color: #8B0000; /* đỏ mận */
  border: none;
}
.btn-danger:hover {
  background-color: #a3002e;
}
</style>
