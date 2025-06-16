<template>
  <section class="contact-section py-5" style="background-color: #fff4f5;">
    <div class="container">
      <!-- Tiêu đề -->
      <div class="text-center mb-5">
        <h2 class="display-5 fw-bold text-marron text-uppercase">Liên hệ với TuTa Spa</h2>
        <p class="text-muted">Hãy để chúng tôi chăm sóc vẻ đẹp và sự thư giãn của bạn!</p>
      </div>

      <!-- Bố cục 3 cột -->
      <div class="row g-4 align-items-stretch">
        <!-- Thông tin liên hệ -->
        <div class="col-lg-4">
          <div class="bg-white p-4 rounded-4 shadow h-100">
            <h5 class="fw-bold mb-4 text-marron"><i class="fa-solid fa-spa me-2"></i>Thông tin liên hệ</h5>
            <ul class="list-unstyled fs-6">
              <li class="mb-3">
                <i class="fa-solid fa-location-dot me-2 text-marron"></i>
                31 Nguyễn Mộng Tuân, phường Hòa Minh, quận Liên Chiểu, TP.Đà Nẵng
              </li>
              <li class="mb-3">
                <i class="fa-solid fa-phone me-2 text-success"></i>
                <a href="tel:0909123456" class="text-decoration-none text-dark">0909 123 456</a>
              </li>
              <li class="mb-3">
                <i class="fa-solid fa-envelope me-2 text-danger"></i>
                <a href="mailto:contact@tatusapa.vn" class="text-decoration-none text-dark">contact@tatusapa.vn</a>
              </li>
              <li>
                <i class="fa-solid fa-globe me-2 text-info"></i>
                <a href="https://tatusapa.vn" class="text-decoration-none text-dark" target="_blank">www.tatusapa.vn</a>
              </li>
            </ul>
            <div class="mt-4">
              <h6 class="fw-bold">Giờ làm việc:</h6>
              <p class="mb-1">Thứ 2 - Thứ 6: 8:00 - 18:00</p>
              <p>Thứ 7 - Chủ Nhật: 9:00 - 15:00</p>
            </div>
          </div>
        </div>

        <!-- Form liên hệ -->
        <div class="col-lg-4">
          <form class="bg-white p-4 rounded-4 shadow h-100" @submit.prevent="submitForm">
            <h5 class="fw-bold text-marron mb-4"><i class="fa-solid fa-pen-nib me-2"></i>Gửi lời nhắn</h5>
            <div class="row g-3">
              <div class="col-md-6">
                <input type="text" class="form-control" placeholder="Họ tên" v-model="form.name" required />
              </div>
              <div class="col-md-6">
                <input type="email" class="form-control" placeholder="Email" v-model="form.email" required />
              </div>
              <div class="col-md-12">
                <input type="text" class="form-control" placeholder="Tiêu đề liên hệ" v-model="form.subject" />
              </div>
              <div class="col-md-12">
                <textarea class="form-control" rows="4" placeholder="Nội dung tin nhắn..." v-model="form.message" required></textarea>
              </div>
              <div class="col-12 text-end">
                <button class="btn btn-marron px-4 py-2 rounded-pill" type="submit">
                  <i class="fa-solid fa-paper-plane me-2"></i>Gửi liên hệ
                </button>
              </div>
            </div>
          </form>
        </div>

        <!-- Google Map -->
        <div class="col-lg-4">
          <div class="map-wrapper rounded-4 overflow-hidden shadow h-100">
            <iframe
              src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3919.5106201460623!2d106.70042327481844!3d10.77272725922164!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x31752f45c6c9f7a3%3A0xd5d2a360e1be45d4!2zMTIzIMSQxrDhu51uZyBTcGEgLCBRdeG6rW4gMSwgSOG7kyBDaMOidSwgVMOibiBUaOG7iyBUaOG7iyBN4bu5LCBIw7JhIE5hbmc!5e0!3m2!1svi!2s!4v1683959479821!5m2!1svi!2s"
              width="100%" height="100%" style="border:0;" allowfullscreen="" loading="lazy"
              referrerpolicy="no-referrer-when-downgrade"></iframe>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
export default {
  data() {
    return {
      form: {
        name: '',
        email: '',
        subject: '',
        message: '',
      },
    };
  },
  methods: {
    submitForm() {
      console.log('Dữ liệu form:', this.form);
      alert('Cảm ơn bạn đã liên hệ! Chúng tôi sẽ phản hồi sớm.');
      this.form = {
        name: '',
        email: '',
        subject: '',
        message: '',
      };
    },
  },
};
</script>

<style scoped>
.text-marron {
  color: #800020;
}
.btn-marron {
  background-color: #800020;
  color: white;
  border: none;
}
.btn-marron:hover {
  background-color: #a12735;
}
.map-wrapper {
  height: 100%;
  min-height: 320px;
}
.contact-section input,
.contact-section textarea {
  border-radius: 0.75rem;
}
</style>
