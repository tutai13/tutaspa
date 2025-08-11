<template>
  <div class="container py-5">
    <h2 class="fw-bold text-primary mb-4 text-center">
      <i class="fas fa-star-half-alt me-2"></i> Quản lý đánh giá khách hàng
    </h2>

    <!-- Bộ lọc nâng cao -->
    <div class="card shadow-sm p-4 mb-4 rounded-4 bg-light">
      <div class="row gy-3 align-items-end">
        <div class="col-md-4">
          <label class="form-label fw-semibold">Trạng thái đánh giá:</label>
          <div class="btn-group w-100">
            <button
              class="btn"
              :class="{
                'btn-success': filter === 'all',
                'btn-outline-secondary': filter !== 'all',
              }"
              @click="filter = 'all'"
            >
              Tất cả
            </button>
            <button
              class="btn"
              :class="{
                'btn-warning': filter === 'chuaduyet',
                'btn-outline-secondary': filter !== 'chuaduyet',
              }"
              @click="filter = 'chuaduyet'"
            >
              Chưa duyệt
            </button>
            <button
              class="btn"
              :class="{
                'btn-outline-success': filter === 'daduyet',
                'btn-outline-secondary': filter !== 'daduyet',
              }"
              @click="filter = 'daduyet'"
            >
              Đã duyệt
            </button>
          </div>
        </div>

        <div class="col-md-3">
          <label class="form-label fw-semibold">Tên người dùng:</label>
          <input
            type="text"
            class="form-control"
            placeholder="Nhập tên..."
            v-model="searchName"
          />
        </div>

        <div class="col-md-2">
          <label class="form-label fw-semibold">Từ ngày:</label>
          <input type="date" class="form-control" v-model="startDate" />
        </div>
        <div class="col-md-2">
          <label class="form-label fw-semibold">Đến ngày:</label>
          <input type="date" class="form-control" v-model="endDate" />
        </div>
      </div>
    </div>

    <!-- Tabs dịch vụ -->
    <div class="d-flex flex-wrap justify-content-center gap-2 mb-3">
      <button
        class="btn d-flex align-items-center gap-2"
        :class="{ 'btn-primary': showTabs, 'btn-outline-secondary': !showTabs }"
        @click="toggleTabs"
      >
        <span>Tất cả dịch vụ</span>
        <i
          class="fas"
          :class="
            showTabs
              ? 'fa-chevron-down rotate-down'
              : 'fa-chevron-right rotate-right'
          "
        ></i>
      </button>
    </div>

    <!-- Slide Tabs dịch vụ theo chiều ngang -->
    <transition name="slide-horizontal">
      <div
        v-if="showTabs"
        class="d-flex flex-wrap justify-content-center gap-2 mb-4"
      >
        <button
          v-for="dv in dichVuTabs"
          :key="dv"
          class="btn"
          :class="{
            'btn-info': selectedDichVu === dv,
            'btn-outline-secondary': selectedDichVu !== dv,
          }"
          @click="selectedDichVu = dv"
        >
          {{ dv }}
        </button>
      </div>
    </transition>

    <br />
    <!-- Danh sách đánh giá -->
    <div
      v-if="danhSachLoc.length === 0"
      class="text-center my-5 text-secondary fs-5"
    >
      <i class="fas fa-circle-info fa-2x text-muted mb-3 d-block"></i>
      Không có đánh giá phù hợp.
    </div>

    <div v-else class="table-responsive shadow rounded-4 overflow-hidden">
      <table class="table table-bordered table-hover align-middle mb-0">
        <thead class="bg-gradient bg-primary text-white">
          <tr class="text-center">
            <th><i class="fas fa-briefcase me-1"></i> Dịch vụ</th>
            <th><i class="fas fa-user me-1"></i> Người đánh giá</th>
            <th><i class="fas fa-star me-1"></i> Sao</th>
            <th><i class="fas fa-comment-dots me-1"></i> Nội dung</th>
            <th><i class="fas fa-calendar-day me-1"></i> Ngày</th>
            <th><i class="fas fa-check-circle me-1"></i> Trạng thái</th>
            <th><i class="fas fa-tools me-1"></i> Hành động</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="dg in danhSachLoc" :key="dg.id" class="text-center">
            <td class="fw-semibold text-primary">
              {{ dg.dichVu?.tenDichVu || "Không rõ" }}
            </td>
            <td>
              <span v-if="dg.anDanh" class="text-muted fst-italic"
                ><i class="fas fa-user-secret me-1"></i>Ẩn danh</span
              >
              <span v-else>{{ dg.user?.name || "Chưa rõ" }}</span>
            </td>
            <td>
              <span v-for="n in 5" :key="n">
                <i
                  class="fa-star fas"
                  :class="
                    n <= dg.soSao ? 'text-warning' : 'text-secondary opacity-25'
                  "
                ></i>
              </span>
            </td>
            <td class="text-wrap text-start px-3" style="max-width: 300px">
              <span>{{ dg.noiDung || "(Không có)" }}</span>
            </td>
            <td>{{ formatDate(dg.ngayTao) }}</td>
            <td>
              <span
                class="badge rounded-pill px-3 py-2"
                :class="dg.daDuyet ? 'bg-success' : 'bg-warning text-dark'"
              >
                <i
                  :class="
                    dg.daDuyet
                      ? 'fas fa-check-circle me-1'
                      : 'fas fa-hourglass-start me-1'
                  "
                ></i>
                {{ dg.daDuyet ? "Đã duyệt" : "Chưa duyệt" }}
              </span>
            </td>
            <td>
              <div class="d-flex justify-content-center gap-2">
                <button
                  v-if="!dg.daDuyet"
                  @click="duyetDanhGia(dg.id)"
                  class="btn btn-sm btn-outline-success rounded-pill"
                  title="Duyệt đánh giá"
                >
                  <i class="fas fa-check"></i>
                </button>
                <button
                  @click="toggleTrangThai(dg.id)"
                  class="btn btn-sm btn-outline-secondary rounded-pill"
                  :title="dg.isActive ? 'Ẩn đánh giá' : 'Hiện lại đánh giá'"
                >
                  <i
                    :class="dg.isActive ? 'fas fa-eye-slash' : 'fas fa-eye'"
                  ></i>
                </button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <br />

    <!-- Biểu đồ -->
    <div class="row mb-5">
      <div class="col-md-6">
        <h5 class="text-center mb-3">📈 Dịch vụ được đánh giá nhiều</h5>
        <BarChart :data="chartDataSoLuong" :options="chartOptions" />
      </div>
      <div class="col-md-6">
        <h5 class="text-center mb-3">🌟 Dịch vụ được đánh giá tốt</h5>
        <BarChart :data="chartDataTrungBinh" :options="chartOptions" />
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from "vue";

import BarChart from "@/components/BarChart.vue";
import axiosClient from "../utils/axiosClient";
const showTabs = ref(false);

const danhSach = ref([]);
const filter = ref("all");
const searchName = ref("");
const startDate = ref("");
const endDate = ref("");
const selectedDichVu = ref("all");

const chartDataSoLuong = ref({ labels: [], datasets: [] });
const chartDataTrungBinh = ref({ labels: [], datasets: [] });

const chartOptions = {
  responsive: true,
  plugins: { legend: { display: false } },
  scales: { y: { beginAtZero: true } },
};

onMounted(async () => await loadDanhSach());

const toggleTabs = () => {
  showTabs.value = !showTabs.value;
  selectedDichVu.value = "all";
};

const loadDanhSach = async () => {
  try {
    const res = await axiosClient.get("DanhGia/admin");
    danhSach.value = res;
    updateCharts();
  } catch (err) {
    console.error("Lỗi khi tải đánh giá:", err);
  }
};

const duyetDanhGia = async (id) => {
  if (confirm("Bạn có chắc muốn duyệt đánh giá này?")) {
    try {
      await axiosClient.put(`DanhGia/duyet/${id}`);
      await loadDanhSach();
    } catch (err) {
      console.error("Lỗi khi duyệt:", err);
    }
  }
};

const toggleTrangThai = async (id) => {
  if (confirm("Bạn có chắc muốn thay đổi trạng thái đánh giá này?")) {
    try {
      await axiosClient.put(`DanhGia/toggle/${id}`);
      await loadDanhSach();
    } catch (err) {
      console.error("Lỗi khi thay đổi trạng thái:", err);
    }
  }
};

const formatDate = (dateStr) => new Date(dateStr).toLocaleString("vi-VN");

const dichVuTabs = computed(() => {
  const tenDVs = danhSach.value
    .map((dg) => dg.dichVu?.tenDichVu)
    .filter(Boolean);
  return [...new Set(tenDVs)];
});

const danhSachLoc = computed(() => {
  return danhSach.value.filter((d) => {
    const matchFilter =
      filter.value === "all" ||
      (filter.value === "chuaduyet" && !d.daDuyet) ||
      (filter.value === "daduyet" && d.daDuyet);

    const matchSearch =
      !searchName.value ||
      (!d.anDanh &&
        d.user?.name?.toLowerCase().includes(searchName.value.toLowerCase()));

    const createdAt = new Date(d.ngayTao);
    const start = startDate.value ? new Date(startDate.value) : null;
    const end = endDate.value ? new Date(endDate.value + "T23:59:59") : null;

    const matchDate =
      (!start || createdAt >= start) && (!end || createdAt <= end);

    const matchDichVu =
      selectedDichVu.value === "all" ||
      d.dichVu?.tenDichVu === selectedDichVu.value;

    return matchFilter && matchSearch && matchDate && matchDichVu;
  });
});

const updateCharts = () => {
  const groupByService = {};
  danhSach.value.forEach((dg) => {
    const ten = dg.dichVu?.tenDichVu || "Không rõ";
    if (!groupByService[ten]) groupByService[ten] = { count: 0, total: 0 };
    groupByService[ten].count++;
    groupByService[ten].total += dg.soSao;
  });

  const labels = Object.keys(groupByService);
  const counts = labels.map((ten) => groupByService[ten].count);
  const averages = labels.map((ten) =>
    (groupByService[ten].total / groupByService[ten].count).toFixed(2)
  );

  chartDataSoLuong.value = {
    labels,
    datasets: [
      { label: "Số lượt đánh giá", backgroundColor: "#4caf50", data: counts },
    ],
  };

  chartDataTrungBinh.value = {
    labels,
    datasets: [
      { label: "Điểm trung bình", backgroundColor: "#ff9800", data: averages },
    ],
  };
};
</script>

<style scoped>
.review-card {
  transition: transform 0.2s, box-shadow 0.3s;
}
.review-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.08);
}
input.form-control {
  border-radius: 2rem;
}
.btn-group .btn {
  min-width: 90px;
}
/* Gợi ý nâng cao cho table */
.table thead th {
  vertical-align: middle;
  font-weight: 600;
  font-size: 15px;
}
.table tbody td {
  font-size: 14px;
}

/* Slide ngang */
.slide-horizontal-enter-active,
.slide-horizontal-leave-active {
  transition: all 0.3s ease;
  transform-origin: left;
}
.slide-horizontal-enter-from,
.slide-horizontal-leave-to {
  opacity: 0;
  transform: scaleX(0);
}

/* Icon xoay mũi tên */
.rotate-right {
  transition: transform 0.3s ease;
  transform: rotate(0deg);
}
.rotate-down {
  transition: transform 0.3s ease;
  transform: rotate(90deg);
}
</style>
