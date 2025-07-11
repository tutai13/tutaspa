<template>
  <div class="container py-5">
    <h2 class="fw-bold text-primary mb-4 text-center">
      <i class="fas fa-star-half-alt me-2"></i> Quản lý đánh giá khách hàng
    </h2>

    <!-- Bộ lọc -->
    <div class="row mb-4 justify-content-center align-items-end">
      <div class="col-auto">
        <div class="btn-group">
          <button
            class="btn"
            :class="{ 'btn-success': filter === 'all', 'btn-outline-secondary': filter !== 'all' }"
            @click="filter = 'all'"
          >
            Tất cả
          </button>
          <button
            class="btn"
            :class="{ 'btn-warning': filter === 'chuaduyet', 'btn-outline-secondary': filter !== 'chuaduyet' }"
            @click="filter = 'chuaduyet'"
          >
            Chưa duyệt
          </button>
          <button
            class="btn"
            :class="{ 'btn-outline-success': filter === 'daduyet', 'btn-outline-secondary': filter !== 'daduyet' }"
            @click="filter = 'daduyet'"
          >
            Đã duyệt
          </button>
        </div>
      </div>

      <!-- Lọc theo tên -->
      <div class="col-md-3 mt-3 mt-md-0">
        <input
          type="text"
          class="form-control"
          placeholder="Lọc theo tên người dùng..."
          v-model="searchName"
        />
      </div>

      <!-- Lọc theo ngày -->
      <div class="col-md-2 mt-3 mt-md-0">
        <label class="form-label">Từ ngày</label>
        <input type="date" class="form-control" v-model="startDate" />
      </div>
      <div class="col-md-2 mt-3 mt-md-0">
        <label class="form-label">Đến ngày</label>
        <input type="date" class="form-control" v-model="endDate" />
      </div>
    </div>

<!-- tab dịch vụ -->
 <!-- Tabs dịch vụ -->
<div class="d-flex flex-wrap justify-content-center gap-2 mb-4">
  <button
    class="btn"
    :class="{ 'btn-primary': selectedDichVu === 'all', 'btn-outline-secondary': selectedDichVu !== 'all' }"
    @click="selectedDichVu = 'all'"
  >
    Tất cả dịch vụ
  </button>
  <button
    v-for="dv in dichVuTabs"
    :key="dv"
    class="btn"
    :class="{ 'btn-info': selectedDichVu === dv, 'btn-outline-secondary': selectedDichVu !== dv }"
    @click="selectedDichVu = dv"
  >
    {{ dv }}
  </button>
</div>


    <!-- Danh sách đánh giá -->
    <div v-if="danhSachLoc.length === 0" class="text-muted text-center">
      Không có đánh giá nào phù hợp.
    </div>

    <div
      v-for="dg in danhSachLoc"
      :key="dg.id"
      class="card border-0 rounded-4 shadow-sm mb-4 p-4 bg-white review-card"
    >
      <div class="d-flex justify-content-between align-items-start flex-wrap">
        <div class="flex-grow-1 me-3">
          <h5 class="mb-2 text-primary">
            <i class="fas fa-spa me-2"></i>{{ dg.dichVu?.tenDichVu }}
          </h5>

          <!-- ⭐ Hiển thị sao -->
          <div class="mb-2 text-warning">
            <i
              v-for="n in 5"
              :key="n"
              class="fas fa-star"
              :class="{ 'text-muted': n > dg.soSao }"
            ></i>
          </div>

          <p class="mb-2"><strong>Nội dung:</strong> {{ dg.noiDung || '(Không có)' }}</p>
          <p class="mb-1 text-muted small">
            Người đánh giá:
            <strong>{{ dg.anDanh ? 'Ẩn danh' : dg.user?.ten || 'Chưa rõ' }}</strong>
          </p>
          <p class="text-muted small">Ngày: {{ formatDate(dg.ngayTao) }}</p>
        </div>

        <div class="text-end d-flex flex-column gap-2 mt-3 mt-md-0 align-items-end">
          <span class="badge" :class="dg.daDuyet ? 'bg-success' : 'bg-warning text-dark'">
            {{ dg.daDuyet ? 'Đã duyệt' : 'Chưa duyệt' }}
          </span>

          <div class="btn-group">
            <button
              v-if="!dg.daDuyet"
              @click="duyetDanhGia(dg.id)"
              class="btn btn-sm btn-outline-success"
              title="Duyệt đánh giá"
            >
              ✅
            </button>
            <button
              @click="tatDanhGia(dg.id)"
              class="btn btn-sm btn-outline-warning"
              title="Tắt đánh giá"
            >
              📴
            </button>
          </div>
        </div>
      </div>
    </div>
<!-- biểu đồ -->
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
import axios from "axios";
import BarChart from "@/components/BarChart.vue";

const danhSach = ref([]);
const filter = ref("all");
const searchName = ref("");
const startDate = ref("");
const endDate = ref("");
const selectedDichVu = ref("all");

const chartDataSoLuong = ref({
  labels: [],
  datasets: [],
});

const chartDataTrungBinh = ref({
  labels: [],
  datasets: [],
});

const chartOptions = {
  responsive: true,
  plugins: {
    legend: { display: false },
  },
  scales: {
    y: { beginAtZero: true },
  },
};

onMounted(async () => {
  await loadDanhSach();
});

const loadDanhSach = async () => {
  try {
    const res = await axios.get("http://localhost:5055/api/DanhGia/admin");
    danhSach.value = res.data;
    updateCharts();
  } catch (err) {
    console.error("Lỗi khi tải đánh giá:", err);
  }
};

const duyetDanhGia = async (id) => {
  if (confirm("Bạn có chắc muốn duyệt đánh giá này?")) {
    try {
      await axios.put(`http://localhost:5055/api/DanhGia/duyet/${id}`);
      await loadDanhSach();
    } catch (err) {
      console.error("Lỗi khi duyệt:", err);
    }
  }
};

const tatDanhGia = async (id) => {
  if (confirm("Bạn có chắc muốn ẩn đánh giá này?")) {
    try {
      await axios.put(`http://localhost:5055/api/DanhGia/tat/${id}`);
      await loadDanhSach();
    } catch (err) {
      console.error("Lỗi khi tắt đánh giá:", err);
    }
  }
};

const formatDate = (dateStr) => {
  const date = new Date(dateStr);
  return date.toLocaleString("vi-VN");
};

// 👉 Tabs dịch vụ
const dichVuTabs = computed(() => {
  const tenDVs = danhSach.value.map(dg => dg.dichVu?.tenDichVu).filter(Boolean);
  return [...new Set(tenDVs)];
});

// 👉 Danh sách lọc
const danhSachLoc = computed(() => {
  return danhSach.value.filter((d) => {
    const matchFilter =
      filter.value === "all" ||
      (filter.value === "chuaduyet" && !d.daDuyet) ||
      (filter.value === "daduyet" && d.daDuyet);

    const matchSearch =
      !searchName.value ||
      (!d.anDanh && d.user?.ten?.toLowerCase().includes(searchName.value.toLowerCase()));

    const createdAt = new Date(d.ngayTao);
    const start = startDate.value ? new Date(startDate.value) : null;
    const end = endDate.value ? new Date(endDate.value + "T23:59:59") : null;

    const matchDate =
      (!start || createdAt >= start) &&
      (!end || createdAt <= end);

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
    if (!groupByService[ten]) {
      groupByService[ten] = { count: 0, total: 0 };
    }
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
      {
        label: "Số lượt đánh giá",
        backgroundColor: "#4caf50",
        data: counts,
      },
    ],
  };

  chartDataTrungBinh.value = {
    labels,
    datasets: [
      {
        label: "Điểm trung bình",
        backgroundColor: "#ff9800",
        data: averages,
      },
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
.fa-star {
  font-size: 1.2rem;
  margin-right: 2px;
}
input.form-control {
  border-radius: 2rem;
}
</style>
