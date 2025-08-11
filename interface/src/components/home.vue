<template>
  <div>
    <section id="home" class="min-vh-100 position-relative">
      <div
        id="spaCarousel"
        class="carousel slide"
        data-bs-ride="carousel"
        data-bs-interval="5000"
      >
        <!-- Carousel Indicators -->
        <div class="carousel-indicators">
          <button
            v-for="(slide, index) in slides"
            :key="index"
            type="button"
            :data-bs-target="'#spaCarousel'"
            :data-bs-slide-to="index"
            :class="{ active: index === currentSlide }"
            :aria-current="index === currentSlide ? 'true' : 'false'"
            :aria-label="'Slide ' + (index + 1)"
          ></button>
        </div>

        <!-- Carousel Items -->
        <div class="carousel-inner">
          <div
            v-for="(slide, index) in slides"
            :key="slide.title"
            class="carousel-item"
            :class="{ active: index === currentSlide }"
            :style="{
              backgroundImage: `linear-gradient(rgba(0, 0, 0, 0.5), rgba(120, 186, 126, 0.3)), url(${slide.image})`,
            }"
          >
            <div
              class="carousel-caption d-flex flex-column justify-content-center h-100"
            >
              <h1
                class="display-2 fw-bold mb-4 font-lora animate__animated animate__fadeInDown"
              >
                Chào mừng đến TutaSpa
              </h1>
              <h2
                class="fs-3 fw-semibold text-white mb-4 animate__animated animate__fadeInUp animate__delay-1s"
              >
                {{ slides[currentSlide].subtitle }}
              </h2>
              <p
                class="lead text-white mb-5 mx-auto animate__animated animate__fadeInUp animate__delay-2s"
                style="max-width: 700px"
              >
                Hành trình thư giãn và tái tạo năng lượng với những liệu pháp tự
                nhiên, mang đến sự cân bằng hoàn hảo cho cơ thể và tâm hồn bạn
              </p>
              <div
                class="d-flex flex-wrap justify-content-center gap-4 mb-5 animate__animated animate__fadeInUp animate__delay-3s"
              >
                <div
                  v-for="(stat, index) in stats"
                  :key="index"
                  class="text-center px-3"
                >
                  <div class="fs-1 mb-2">{{ stat.icon }}</div>
                  <div class="fs-3 fw-bold text-warning">{{ stat.number }}</div>
                  <div class="text-white">{{ stat.label }}</div>
                </div>
              </div>
              <div
                class="d-flex flex-column flex-sm-row justify-content-center gap-3 animate__animated animate__fadeInUp animate__delay-4s"
              >
                <router-link
                  to="/#services"
                  class="btn btn-primary btn-lg rounded-pill px-5 py-3 fw-bold"
                >
                  Khám phá dịch vụ
                </router-link>
                <router-link
                  to="/#booking"
                  class="btn btn-outline-light btn-lg rounded-pill px-5 py-3"
                >
                  Đặt lịch ngay
                </router-link>
              </div>
            </div>
          </div>
        </div>

        <!-- Carousel Controls -->
        <button
          class="carousel-control-prev"
          type="button"
          data-bs-target="#spaCarousel"
          data-bs-slide="prev"
        >
          <span class="carousel-control-prev-icon" aria-hidden="true"></span>
          <span class="visually-hidden">Previous</span>
        </button>
        <button
          class="carousel-control-next"
          type="button"
          data-bs-target="#spaCarousel"
          data-bs-slide="next"
        >
          <span class="carousel-control-next-icon" aria-hidden="true"></span>
          <span class="visually-hidden">Next</span>
        </button>
      </div>
    </section>

    <!-- Features Section -->
    <section class="features">
      <div class="container">
        <div class="features-grid">
          <div class="feature-card">
            <div class="feature-icon">🌱</div>
            <h3>100% Tự nhiên</h3>
            <p>
              Sử dụng các sản phẩm từ thiên nhiên, không chất hóa học có hại, an
              toàn cho mọi loại da
            </p>
          </div>
          <div class="feature-card">
            <div class="feature-icon">👥</div>
            <h3>Chuyên gia giàu kinh nghiệm</h3>
            <p>
              Đội ngũ chuyên viên được đào tạo bài bản, có chứng chỉ quốc tế và
              kinh nghiệm hơn 5 năm
            </p>
          </div>
          <div class="feature-card">
            <div class="feature-icon">🏛️</div>
            <h3>Không gian sang trọng</h3>
            <p>
              Thiết kế hiện đại, thoáng mát với âm nhạc thư giãn và hương thơm
              dễ chịu
            </p>
          </div>
          <div class="feature-card">
            <div class="feature-icon">🛡️</div>
            <h3>Đảm bảo vệ sinh</h3>
            <p>
              Tuân thủ nghiêm ngặt các tiêu chuẩn vệ sinh, khử trùng dụng cụ sau
              mỗi lần sử dụng
            </p>
          </div>
        </div>
      </div>
    </section>

    <!-- Services Section -->
    <section id="services" class="services">
      <div class="container">
        <h2 class="section-title">Dịch vụ của chúng tôi</h2>
        <p class="section-subtitle">
          Trải nghiệm những dịch vụ chăm sóc sức khỏe và làm đẹp hàng đầu với
          công nghệ hiện đại và nguyên liệu tự nhiên
        </p>

        <div class="service-categories">
          <button
            class="category-btn"
            :class="{ active: currentCategory === 'all' }"
            @click="filterServices('all')"
          >
            Tất cả
          </button>
          <button
            v-for="category in categories"
            :key="category.loaiDichVuID"
            class="category-btn"
            :class="{ active: currentCategory === category.tenLoai }"
            @click="filterServices(category.tenLoai)"
          >
            {{ category.tenLoai }}
          </button>
        </div>

        <div v-if="loading" class="loading">
          <div class="loading-spinner"></div>
          <p>Đang tải dịch vụ...</p>
        </div>
        <div v-else-if="error" class="error">
          <p>{{ error }}</p>
        </div>
        <div v-else class="services-grid">
          <div
            v-for="service in filteredServices"
            :key="service.id"
            class="service-card"
          >
            <img
              :src="service.image"
              :alt="service.name"
              class="service-image"
            />
            <h3>{{ service.name }}</h3>
            <div class="service-duration">{{ service.duration }} phút</div>
            <p>{{ service.description }}</p>
            <div class="service-price">{{ service.price }} VNĐ</div>
            <button
              class="service-book-btn"
              @click="addServiceFromCard(service)"
            >
              Đặt lịch ngay
            </button>
          </div>
        </div>
      </div>
    </section>

    <!-- Testimonials Section -->
    <section class="testimonials">
      <div class="container">
        <h2 class="section-title">Khách hàng nói gì về chúng tôi</h2>
        <div class="testimonials-grid">
          <div class="testimonial-card">
            <div class="testimonial-rating">⭐⭐⭐⭐⭐</div>
            <p class="testimonial-text">
              Dịch vụ massage tại Serenity Spa thật sự tuyệt vời! Tôi cảm thấy
              thư giãn và thoải mái như chưa bao giờ. Nhân viên rất chuyên
              nghiệp và chu đáo.
            </p>
            <div class="testimonial-author">- Chị Lan Anh, 32 tuổi</div>
          </div>
          <div class="testimonial-card">
            <div class="testimonial-rating">⭐⭐⭐⭐⭐</div>
            <p class="testimonial-text">
              Sau liệu trình chăm sóc da tại đây, làn da tôi trở nên mịn màng và
              sáng khỏe hơn rất nhiều. Tôi sẽ quay lại và giới thiệu cho bạn bè.
            </p>
            <div class="testimonial-author">- Chị Minh Hương, 28 tuổi</div>
          </div>
          <div class="testimonial-card">
            <div class="testimonial-rating">⭐⭐⭐⭐⭐</div>
            <p class="testimonial-text">
              Không gian spa rất đẹp và thư giãn. Dịch vụ detox toàn thân giúp
              tôi cảm thấy nhẹ nhõm và tràn đầy năng lượng. Đáng tiền!
            </p>
            <div class="testimonial-author">- Anh Đức Minh, 35 tuổi</div>
          </div>
        </div>
      </div>
    </section>

    <!-- Booking Section -->
    <section id="booking" class="booking">
      <div class="container">
        <div class="booking-content">
          <div class="booking-info">
            <h2>Đặt lịch hẹn</h2>
            <p>
              Hãy để chúng tôi chăm sóc bạn với những dịch vụ tốt nhất. Đặt lịch
              ngay hôm nay để nhận được ưu đãi đặc biệt và trải nghiệm không
              gian thư giãn tuyệt vời.
            </p>
            <ul class="booking-benefits">
              <li>Tư vấn miễn phí từ chuyên gia</li>
              <li>Đặt lịch linh hoạt theo thời gian bạn</li>
              <li>Giảm giá 10% cho khách hàng lần đầu</li>
              <li>Cam kết dịch vụ chất lượng cao</li>
              <li>Hỗ trợ 24/7 qua hotline</li>
            </ul>
          </div>

          <form class="booking-form" @submit.prevent="submitBooking">
            <div class="form-group">
              <label for="phone">Số điện thoại *</label>
              <input
                type="tel"
                id="phone"
                v-model="bookingForm.phone"
                required
                placeholder="0123 456 789"
              />
            </div>

            <div class="form-group">
              <label for="service">Chọn dịch vụ *</label>
              <select id="service" v-model="selectedService">
                <option value="">-- Chọn dịch vụ --</option>
                <option
                  v-for="service in services"
                  :key="service.id"
                  :value="service"
                >
                  {{ service.name }} - {{ service.price }} VNĐ
                </option>
              </select>
              <div class="form-row d-flex align-items-center gap-5">
                <button
                  type="button"
                  class="add-service-btn"
                  :disabled="!selectedService"
                  @click="addService"
                >
                  Thêm dịch vụ
                </button>
                <!-- Checkbox tư vấn tại quán -->
                <label class="consult-checkbox">
                  <input
                    type="checkbox"
                    v-model="bookingForm.consultAtStore"
                    style="width: 15px; height: 15px"
                  />
                  Tới quán nhân viên tư vấn
                </label>
              </div>
            </div>

            <!-- Selected Services List -->
            <div class="selected-services" v-if="bookingForm.services.length">
              <h4>Dịch vụ đã chọn:</h4>
              <ul>
                <li
                  v-for="(service, index) in bookingForm.services"
                  :key="index"
                  class="selected-service-item"
                >
                  <span>{{ service.name }} - {{ service.price }} VNĐ</span>
                  <div class="quantity-control">
                    <label for="quantity-${index}">Số lượng:</label>
                    <input
                      type="number"
                      :id="'quantity-' + index"
                      v-model.number="service.soLuong"
                      min="1"
                      max="10"
                      required
                      class="quantity-input"
                    />
                    <button
                      type="button"
                      class="remove-service-btn"
                      @click="removeService(index)"
                    >
                      &times;
                    </button>
                  </div>
                </li>
              </ul>
            </div>

            <div class="form-row">
              <div class="form-group">
                <label for="date">Ngày hẹn *</label>
                <input
                  type="date"
                  id="date"
                  v-model="bookingForm.date"
                  :min="minDate"
                  required
                />
              </div>

              <div class="form-group">
                <label for="time">Giờ hẹn *</label>
                <select id="time" v-model="bookingForm.time" required>
                  <option value="">-- Chọn giờ --</option>
                  <option
                    v-for="slot in availableSlots"
                    :key="slot.khungGio"
                    :value="slot.khungGio"
                  >
                    {{ slot.khungGio }}
                    <span v-if="slot.conLai <= 2"
                      >(Còn {{ slot.conLai }} chỗ)</span
                    >
                  </option>
                </select>
              </div>
            </div>

            <div class="form-group">
              <label for="notes">Ghi chú</label>
              <textarea
                id="notes"
                v-model="bookingForm.notes"
                rows="4"
                placeholder="Những yêu cầu đặc biệt hoặc ghi chú khác..."
              ></textarea>
            </div>

            <button
              type="submit"
              class="submit-btn"
              :disabled="!bookingForm.services.length"
            >
              Đặt lịch hẹn
            </button>
          </form>
        </div>
      </div>
    </section>

    <!-- About Section -->
    <section id="about" class="about">
      <div class="container">
        <div class="about-content">
          <div class="about-text">
            <h2>Về TutaSpa</h2>
            <p>
              Với hơn 10 năm kinh nghiệm trong lĩnh vực chăm sóc sức khỏe và làm
              đẹp, Serenity Spa tự hào là điểm đến lý tưởng cho những ai tìm
              kiếm sự thư giãn, làm đẹp và chăm sóc toàn diện.
            </p>
            <p>
              Chúng tôi kết hợp tinh hoa của các liệu pháp truyền thống phương
              Đông với công nghệ hiện đại phương Tây, tạo nên những dịch vụ độc
              đáo và hiệu quả. Mỗi liệu trình đều được thiết kế riêng biệt, phù
              hợp với nhu cầu và tình trạng cụ thể của từng khách hàng.
            </p>
            <p>
              Không gian spa được thiết kế theo phong cách tối giản nhưng sang
              trọng, với ánh sáng dịu nhẹ, âm nhạc thư giãn và hương thơm tự
              nhiên từ các loại tinh dầu cao cấp. Đây chính là nơi bạn có thể
              tạm quên đi những lo toan trong cuộc sống và tận hưởng những phút
              giây thư giãn tuyệt đối.
            </p>
            <div class="about-features">
              <div class="about-feature">
                <span>✓</span> Chứng nhận ISO 9001:2015
              </div>
              <div class="about-feature">
                <span>✓</span> Đội ngũ chuyên gia quốc tế
              </div>
              <div class="about-feature">
                <span>✓</span> Sản phẩm organic cao cấp
              </div>
              <div class="about-feature">
                <span>✓</span> Cam kết hài lòng 100%
              </div>
            </div>
          </div>
          <div class="about-image"></div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from "vue";
import apiClient from "../utils/axiosClient";

// Reactive state
const services = ref([]);
const categories = ref([]);
const currentCategory = ref("all");
const filteredServices = ref([]);
const loading = ref(true);
const error = ref(null);
const minDate = ref("");
const selectedService = ref(null);
const bookingForm = ref({
  phone: "",
  services: [],
  date: new Date().toISOString().split("T")[0],
  time: "",
  notes: "",
  consultAtStore: false,
});
const modalForm = ref({
  name: "",
  phone: "",
  email: "",
  date: "",
  time: "",
  notes: "",
});
const availableSlots = ref([]);
// Base URL for images
const IMAGE_BASE_URL = "https://localhost:7183/images/";

const currentSlide = ref(0);

const slides = [
  {
    image:
      "https://images.unsplash.com/photo-1540555700478-4be289fbecef?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2070&q=80",
    title: "Thư Giãn & Tái Tạo",
    subtitle: "Trải nghiệm không gian yên bình",
  },
  {
    image:
      "https://images.unsplash.com/photo-1596178065887-1198b6148b2b?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2070&q=80",
    title: "Chăm Sóc Chuyên Nghiệp",
    subtitle: "Với đội ngũ chuyên gia giàu kinh nghiệm",
  },
  {
    image:
      "https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2070&q=80",
    title: "Làm Đẹp Tự Nhiên",
    subtitle: "Sử dụng 100% sản phẩm thiên nhiên",
  },
];

const stats = [
  { number: "10+", label: "Năm kinh nghiệm", icon: "🏆" },
  { number: "5000+", label: "Khách hàng hài lòng", icon: "👥" },
  { number: "24", label: "Dịch vụ chuyên nghiệp", icon: "🌿" },
];

// Methods

const addServiceFromCard = (service) => {
  if (!bookingForm.value.services.some((s) => s.id === service.id)) {
    bookingForm.value.services.push({
      ...service,
      soLuong: 1,
    });
  }
  // Scroll to booking section
  const bookingSection = document.getElementById("booking");
  if (bookingSection) {
    bookingSection.scrollIntoView({ behavior: "smooth" });
  }
};
const fetchSlots = async () => {
  if (!bookingForm.value.date) return;
  try {
    const res = await apiClient.get("/DatLich/slots", {
      params: { ngay: bookingForm.value.date },
    });
    availableSlots.value = res.filter((slot) => slot.conLai > 0);
  } catch (err) {
    console.error("Lỗi khi lấy giờ hẹn:", err);
    availableSlots.value = [];
  }
};

// Gọi API khi đổi ngày
watch(
  () => bookingForm.value.date,
  () => {
    fetchSlots();
    bookingForm.value.time = "";
  }
);

const fetchCategories = async () => {
  try {
    const response = await apiClient.get("/LoaiDichVu");
    const data = response;
    categories.value = Array.isArray(data) ? data : data.data || [];
    if (categories.value.length === 0) {
      throw new Error("Không có danh mục dịch vụ nào được trả về");
    }
  } catch (err) {
    console.error("Lỗi khi tải danh mục:", err);
    error.value = "Không thể tải danh mục dịch vụ. Hiển thị danh mục mặc định.";
    categories.value = [
      { loaiDichVuID: 1, tenLoai: "Triệt lông" },
      { loaiDichVuID: 2, tenLoai: "Massage" },
      { loaiDichVuID: 3, tenLoai: "Chăm sóc da" },
    ];
  }
};

const fetchServices = async () => {
  try {
    const response = await apiClient.get("/DichVu");
    const data = response;
    const serviceData = Array.isArray(data) ? data : data.data || [];
    services.value = serviceData
      .filter((service) => service.trangThai === 1)
      .map((service) => ({
        id: service.dichVuID,
        name: service.tenDichVu,
        category:
          categories.value.find(
            (cat) => cat.loaiDichVuID === service.loaiDichVuID
          )?.tenLoai || "Khác",
        price: service.gia.toLocaleString("vi-VN"),
        duration: service.thoiGian,
        description: service.moTa,
        image: `${IMAGE_BASE_URL}${service.hinhAnh}`,
      }));
    if (services.value.length === 0) {
      throw new Error("Không có dịch vụ nào được trả về");
    }
  } catch (err) {
    console.error("Lỗi khi tải dịch vụ:", err);
    error.value = "Không thể tải danh sách dịch vụ. Hiển thị dịch vụ mặc định.";
    services.value = [
      {
        id: 1,
        name: "Triệt lông full chân",
        category: "Triệt lông",
        price: "500.000",
        duration: 45,
        description:
          "Triệt lông vùng chân bằng công nghệ ánh sáng SHR không đau rát, hiệu quả cao",
        image: `${IMAGE_BASE_URL}triet_long_full_chan.jpg`,
      },
    ];
  }
};

const filterServices = (category) => {
  currentCategory.value = category;
  filteredServices.value =
    category === "all"
      ? [...services.value]
      : services.value.filter((service) => service.category === category);
};

const addService = () => {
  if (
    selectedService.value &&
    !bookingForm.value.services.some((s) => s.id === selectedService.value.id)
  ) {
    bookingForm.value.services.push({ ...selectedService.value, soLuong: 1 });
    selectedService.value = null; // Reset dropdown
  }
};

const removeService = (index) => {
  bookingForm.value.services.splice(index, 1);
};

const submitBooking = async () => {
  try {
    const thoiGian = new Date(
      new Date(
        `${bookingForm.value.date}T${bookingForm.value.time}`
      ).getTime() +
        7 * 60 * 60 * 1000
    ).toISOString();

    const dichVus = bookingForm.value.consultAtStore
      ? []
      : bookingForm.value.services.map((s) => ({
          dichVuID: s.id,
          soLuong: s.soLuong,
        }));

    const payload = {
      soDienThoai: bookingForm.value.phone,
      thoiGian,
      dichVus,
      ghiChu: bookingForm.value.notes,
      datTruoc: true,
    };

    const res = await apiClient.post("/DatLich", payload);

    alert("Đặt lịch thành công!");
    bookingForm.value = {
      phone: "",
      services: [],
      date: new Date().toISOString().split("T")[0],
      time: "",
      notes: "",
    };
    selectedService.value = null;
  } catch (err) {
    console.error("Lỗi đặt lịch:", err);
    alert("Đặt lịch thất bại!");
  }
};

const resetBookingForm = () => {
  bookingForm.value = {
    phone: "",
    services: [],
    date: "",
    time: "",
    notes: "",
  };
  selectedService.value = null;
};

const resetModalForm = () => {
  modalForm.value = {
    name: "",
    phone: "",
    email: "",
    date: "",
    time: "",
    notes: "",
  };
};

// Lifecycle hook
onMounted(async () => {
  minDate.value = new Date().toISOString().split("T")[0];
  loading.value = true;
  await fetchCategories();
  await fetchServices();
  loading.value = false;
  filterServices("all");
  fetchSlots();
});
</script>

<style>
.carousel-item {
  height: 100vh;
  background-size: cover !important;
  background-position: center !important;
}

.carousel-caption {
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.5);
}

.font-lora {
  font-family: "Lora", serif;
}

.stat-number {
  font-size: 2.5rem;
  font-weight: bold;
  color: #fbbf24;
}

.stat-label {
  font-size: 0.9rem;
  opacity: 0.9;
}

/* Features Section */
.features {
  padding: 6rem 2rem;
  background: white;
}

.features-grid {
  display: grid;
  grid-template-columns: repeat(4, minmax(280px, 1fr));
  gap: 3rem;
  max-width: 1200px;
  margin: 0 auto;
}

.feature-card {
  text-align: center;
  padding: 2rem;
  border-radius: 15px;
  background: linear-gradient(135deg, #f0fdf4 0%, #dcfce7 100%);
  transition: all 0.3s ease;
}

.feature-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 10px 25px rgba(120, 186, 126, 0.2);
}

.feature-icon {
  font-size: 3rem;
  margin-bottom: 1rem;
  color: #78ba7e;
}

.feature-card h3 {
  font-size: 1.4rem;
  margin-bottom: 1rem;
  color: #2d4a2d;
}

/* Services Section */
.services {
  padding: 8rem 2rem;
  background: linear-gradient(135deg, #f8fdf8 0%, #f0fdf4 50%, #e8f5e8 100%);
}

.container {
  max-width: 1400px;
  margin: 0 auto;
}

.section-title {
  text-align: center;
  font-size: 2.8rem;
  margin-bottom: 1rem;
  color: #2d4a2d;
  position: relative;
  font-family: "Lora", serif;
}

.section-subtitle {
  text-align: center;
  font-size: 1.1rem;
  color: #4b5563;
  margin-bottom: 4rem;
  max-width: 600px;
  margin-left: auto;
  margin-right: auto;
}

.section-title::after {
  content: "";
  display: block;
  width: 80px;
  height: 3px;
  background: linear-gradient(45deg, #78ba7e, #5e8c64);
  margin: 1rem auto 0;
  border-radius: 2px;
}

.service-categories {
  display: flex;
  justify-content: center;
  gap: 1rem;
  margin-bottom: 3rem;
  flex-wrap: wrap;
}

.category-btn {
  background: white;
  border: 2px solid #78ba7e;
  color: #78ba7e;
  padding: 0.8rem 2rem;
  border-radius: 25px;
  cursor: pointer;
  font-weight: 500;
  transition: all 0.3s ease;
}

.category-btn.active,
.category-btn:hover {
  background: #78ba7e;
  color: white;
  transform: translateY(-2px);
  box-shadow: 0 5px 15px rgba(120, 186, 126, 0.4);
}

.services-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(320px, 1fr));
  gap: 2.5rem;
  margin-top: 3rem;
}

.service-card {
  background: white;
  padding: 2.5rem 2rem;
  border-radius: 20px;
  text-align: center;
  box-shadow: 0 8px 25px rgba(120, 186, 126, 0.12);
  transition: all 0.4s ease;
  position: relative;
  overflow: hidden;
  border: 1px solid #e8f5e8;
}

.service-card::before {
  content: "";
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(
    90deg,
    transparent,
    rgba(120, 186, 126, 0.15),
    transparent
  );
  transition: all 0.6s;
}

.service-card:hover::before {
  left: 100%;
}

.service-card:hover {
  transform: translateY(-8px);
  box-shadow: 0 20px 40px rgba(120, 186, 126, 0.2);
}

.service-image {
  width: 100%;
  height: 200px;
  object-fit: cover;
  border-radius: 10px;
  margin-bottom: 1.5rem;
}

.service-card h3 {
  font-size: 1.4rem;
  margin-bottom: 1rem;
  color: #2d4a2d;
  font-family: "Lora", serif;
}

.service-card p {
  color: #6b7280;
  margin-bottom: 1.5rem;
  line-height: 1.6;
}

.service-duration {
  display: inline-block;
  background: #f0fdf4;
  color: #4a6741;
  padding: 0.3rem 1rem;
  border-radius: 15px;
  font-size: 0.85rem;
  margin-bottom: 1rem;
}

.service-price {
  font-size: 1.5rem;
  font-weight: bold;
  color: #f59e0b;
  margin-bottom: 1.5rem;
  font-family: "Lora", serif;
}

.service-book-btn {
  background: linear-gradient(45deg, #78ba7e, #5e8c64);
  color: white;
  padding: 0.9rem 2.5rem;
  border: none;
  border-radius: 25px;
  cursor: pointer;
  font-weight: 600;
  transition: all 0.3s ease;
  font-size: 0.95rem;
}

.service-book-btn:hover {
  transform: scale(1.05);
  box-shadow: 0 6px 20px rgba(120, 186, 126, 0.5);
}

/* Testimonials */
.testimonials {
  padding: 8rem 2rem;
  background: white;
}

.testimonials-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 2rem;
  max-width: 1200px;
  margin: 4rem auto 0;
}

.testimonial-card {
  background: linear-gradient(135deg, #f8fdf8 0%, #f0fdf4 100%);
  padding: 2rem;
  border-radius: 15px;
  position: relative;
}

.testimonial-card::before {
  content: '"';
  position: absolute;
  top: -10px;
  left: 20px;
  font-size: 4rem;
  color: #78ba7e;
  opacity: 0.3;
}

.testimonial-text {
  font-style: italic;
  margin-bottom: 1rem;
  color: #374151;
}

.testimonial-author {
  font-weight: bold;
  color: #2d4a2d;
}

.testimonial-rating {
  color: #fbbf24;
  margin-bottom: 0.5rem;
}

/* Booking Section */
.booking {
  padding: 8rem 2rem;
  background: linear-gradient(135deg, #78ba7e 0%, #6ba371 50%, #5e8c64 100%);
  color: white;
}

.booking-content {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 4rem;
  max-width: 1200px;
  margin: 0 auto;
  align-items: center;
}

.booking-info h2 {
  font-size: 2.5rem;
  margin-bottom: 2rem;
  font-family: "Lora", serif;
}

.booking-info p {
  font-size: 1.1rem;
  margin-bottom: 2rem;
  opacity: 0.9;
}

.booking-benefits {
  list-style: none;
  margin-bottom: 2rem;
}

.booking-benefits li {
  margin-bottom: 1rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.booking-benefits li::before {
  content: "✓";
  color: #fbbf24;
  font-weight: bold;
  font-size: 1.2rem;
}

.booking-form {
  background: rgba(255, 255, 255, 0.15);
  padding: 3rem;
  border-radius: 20px;
  backdrop-filter: blur(10px);
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.1);
}

.form-group {
  margin-bottom: 2rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: 600;
  color: white;
}

.form-group input,
.form-group select,
.form-group textarea {
  width: 100%;
  padding: 1rem;
  border: none;
  border-radius: 10px;
  font-size: 1rem;
  background: rgba(255, 255, 255, 0.9);
  transition: all 0.3s ease;
  color: #2d4a2d;
}

.form-group input:focus,
.form-group select:focus,
.form-group textarea:focus {
  outline: none;
  background: white;
  transform: scale(1.02);
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
}

.submit-btn {
  background: linear-gradient(45deg, #f59e0b, #f97316);
  color: white;
  padding: 1.2rem 3rem;
  border: none;
  border-radius: 50px;
  font-size: 1.1rem;
  font-weight: bold;
  cursor: pointer;
  transition: all 0.3s ease;
  width: 100%;
  margin-top: 1rem;
}

.submit-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 10px 25px rgba(245, 158, 11, 0.4);
}

.add-service-btn {
  background: linear-gradient(45deg, #78ba7e, #5e8c64);
  color: white;
  padding: 0.8rem 2rem;
  border: none;
  border-radius: 25px;
  margin-top: 1rem;
  cursor: pointer;
  font-weight: 600;
  transition: all 0.3s ease;
  display: block;
  width: fit-content;
}

.add-service-btn:disabled {
  background: #d1d5db;
  cursor: not-allowed;
}

.add-service-btn:hover:not(:disabled) {
  transform: scale(1.05);
  box-shadow: 0 6px 20px rgba(120, 186, 126, 0.5);
}

.selected-services {
  margin-bottom: 2rem;
}

.selected-services h4 {
  font-size: 1.2rem;
  color: white;
  margin-bottom: 1rem;
}

.selected-services ul {
  list-style: none;
  padding: 0;
}

.selected-service-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background: rgba(255, 255, 255, 0.1);
  padding: 0.8rem 1rem;
  border-radius: 10px;
  margin-bottom: 0.5rem;
  color: white;
}

.remove-service-btn {
  background: #ef4444;
  color: white;
  border: none;
  border-radius: 50%;
  width: 24px;
  height: 24px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  font-size: 1rem;
  transition: all 0.3s ease;
}

.remove-service-btn:hover {
  background: #dc2626;
  transform: scale(1.1);
}

/* About Section */
.about {
  padding: 8rem 2rem;
  background: white;
}

.about-content {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 5rem;
  align-items: center;
  max-width: 1200px;
  margin: 0 auto;
}

.about-text h2 {
  font-size: 2.5rem;
  margin-bottom: 2rem;
  color: #2d4a2d;
  font-family: "Lora", serif;
}

.about-text p {
  font-size: 1.1rem;
  margin-bottom: 2rem;
  color: #374151;
  line-height: 1.8;
}

.about-features {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 1rem;
  margin-top: 2rem;
}

.about-feature {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: #78ba7e;
  font-weight: 500;
}

.about-image {
  background: linear-gradient(
    135deg,
    #78ba7e 0%,
    #8bc792 30%,
    #6ba371 70%,
    #5e8c64 100%
  );
  height: 500px;
  border-radius: 20px;
  position: relative;
  overflow: hidden;
  box-shadow: 0 15px 35px rgba(120, 186, 126, 0.25);
}

/* Modal */
.modal {
  display: block;
  position: fixed;
  z-index: 2000;
  left: 0;
  top: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.6);
  backdrop-filter: blur(5px);
}

.modal-content {
  background-color: white;
  margin: 3% auto;
  padding: 2.5rem;
  border-radius: 20px;
  width: 90%;
  max-width: 500px;
  position: relative;
  animation: slideIn 0.3s ease;
  max-height: 90vh;
  overflow-y: auto;
}

.close {
  color: #6b7280;
  float: right;
  font-size: 2rem;
  font-weight: bold;
  cursor: pointer;
  transition: color 0.3s;
}

.close:hover {
  color: #78ba7e;
}

/* Error */
.error {
  text-align: center;
  padding: 2rem;
  color: #ef4444;
}

/* Loading */
.loading {
  text-align: center;
  padding: 2rem;
  color: #6b7280;
}

.loading-spinner {
  border: 4px solid #f3f4f6;
  border-top: 4px solid #78ba7e;
  border-radius: 50%;
  width: 50px;
  height: 50px;
  animation: spin 1s linear infinite;
  margin: 0 auto 1rem;
}

/* Animations */
@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes slideIn {
  from {
    transform: translateY(-50px);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}

/* Responsive */
@media (max-width: 768px) {
  .features-grid {
    grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  }

  .services-grid {
    grid-template-columns: 1fr;
  }

  .about-content,
  .booking-content {
    grid-template-columns: 1fr;
    gap: 3rem;
  }

  .form-row {
    grid-template-columns: 1fr;
  }

  .category-btn {
    font-size: 0.9rem;
    padding: 0.6rem 1.5rem;
  }

  .section-title {
    font-size: 2.2rem;
  }

  .service-image {
    height: 150px;
  }
}
.quantity-control {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.quantity-input {
  width: 60px;

  border-radius: 5px;
  text-align: center;
}

.selected-service-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background: rgba(255, 255, 255, 0.1);
  padding: 0.8rem 1rem;
  border-radius: 10px;
  margin-bottom: 0.5rem;
  color: white;
}

.quantity-control label {
  color: white;
  font-size: 0.9rem;
}
</style>
