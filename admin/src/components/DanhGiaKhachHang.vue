<template>
  <div class="review-management">
    <!-- Header -->
    <div class="header">
      <h1 class="title">
        <i class="fas fa-star-half-alt"></i>
        Quản lý đánh giá khách hàng
      </h1>
    </div>

    <!-- Bộ lọc nâng cao -->
    <div class="card">
      <div class="card-header">
        <h2 class="section-title">
          <i class="fas fa-filter"></i>
          Bộ lọc & Tìm kiếm
        </h2>
        <div class="list-controls">
          <button class="btn btn-outline-primary" @click="loadDanhSach">
            <i class="fas fa-sync-alt"></i>
            Làm mới
          </button>
        </div>
      </div>
      <div class="card-body">
        <div class="form-grid">
          <!-- Trạng thái đánh giá -->
          <div class="form-group">
            <label class="form-label">
              <i class="fas fa-check-circle"></i> Trạng thái đánh giá
            </label>
            <div class="status-filter-group">
              <button 
                class="btn status-btn"
                :class="{ 'btn-primary': filter === 'all', 'btn-outline-secondary': filter !== 'all' }"
                @click="filter = 'all'"
              >
                <i class="fas fa-list"></i>
                Tất cả
              </button>
              <button 
                class="btn status-btn"
                :class="{ 'btn-warning': filter === 'chuaduyet', 'btn-outline-secondary': filter !== 'chuaduyet' }"
                @click="filter = 'chuaduyet'"
              >
                <i class="fas fa-hourglass-start"></i>
                Chưa duyệt
              </button>
              <button 
                class="btn status-btn"
                :class="{ 'btn-success': filter === 'daduyet', 'btn-outline-secondary': filter !== 'daduyet' }"
                @click="filter = 'daduyet'"
              >
                <i class="fas fa-check-circle"></i>
                Đã duyệt
              </button>
            </div>
          </div>

          <!-- Tìm kiếm tên -->
          <div class="form-group">
            <label class="form-label">
              <i class="fas fa-search"></i> Tìm theo tên người dùng
            </label>
            <div class="search-container">
              <input 
                v-model="searchName" 
                type="text" 
                class="search-input" 
                placeholder="Nhập tên người dùng..."
              />
              <i class="fas fa-search search-icon"></i>
              <button 
                v-if="searchName" 
                class="clear-search show" 
                @click="searchName = ''"
                type="button"
              >
                <i class="fas fa-times"></i>
              </button>
            </div>
          </div>

          <!-- Từ ngày -->
          <div class="form-group">
            <label class="form-label">
              <i class="fas fa-calendar-alt"></i> Từ ngày
            </label>
            <input 
              v-model="startDate" 
              type="date" 
              class="form-control"
            />
          </div>

          <!-- Đến ngày -->
          <div class="form-group">
            <label class="form-label">
              <i class="fas fa-calendar-check"></i> Đến ngày
            </label>
            <input 
              v-model="endDate" 
              type="date" 
              class="form-control"
            />
          </div>
        </div>

        <!-- Service Filter Toggle -->
        <div class="service-filter-section">
          <button 
            class="btn btn-outline-primary service-toggle-btn"
            @click="toggleTabs"
          >
            <i class="fas fa-cogs"></i>
            Lọc theo dịch vụ
            <i class="fas transition-icon"
              :class="showTabs ? 'fa-chevron-up' : 'fa-chevron-down'"></i>
          </button>

          <!-- Service Tabs -->
          <transition name="slide-down">
            <div v-if="showTabs" class="service-tabs">
              <button
                class="btn service-tab"
                :class="{ 'btn-info': selectedDichVu === 'all', 'btn-outline-secondary': selectedDichVu !== 'all' }"
                @click="selectedDichVu = 'all'"
              >
                <i class="fas fa-th-large"></i>
                Tất cả dịch vụ
              </button>
              <button
                v-for="dv in dichVuTabs"
                :key="dv"
                class="btn service-tab"
                :class="{ 'btn-info': selectedDichVu === dv, 'btn-outline-secondary': selectedDichVu !== dv }"
                @click="selectedDichVu = dv"
              >
                <i class="fas fa-concierge-bell"></i>
                {{ dv }}
              </button>
            </div>
          </transition>
        </div>

        <div class="form-actions">
          <button class="btn btn-primary" @click="applyFilters">
            <i class="fas fa-search"></i>
            Áp dụng bộ lọc
          </button>
          <button class="btn btn-outline-primary" @click="resetFilters">
            <i class="fas fa-undo"></i>
            Đặt lại
          </button>
        </div>
      </div>
    </div>

    <!-- Danh sách đánh giá -->
    <div class="card">
      <div class="card-header">
        <h2 class="section-title">
          <i class="fas fa-comments"></i>
          Danh sách đánh giá
          <span class="badge">{{ danhSachLoc.length }}</span>
        </h2>
        <div class="stats-info">
          <span class="stat-item">
            <i class="fas fa-eye"></i>
            Hiển thị: {{ danhSachLoc.filter(r => r.isActive).length }}
          </span>
          <span class="stat-item">
            <i class="fas fa-hourglass-start"></i>
            Chưa duyệt: {{ danhSachLoc.filter(r => !r.daDuyet).length }}
          </span>
        </div>
      </div>
      <div class="card-body">
        <div v-if="loading" class="loading-state">
          <i class="fas fa-spinner"></i>
          Đang tải dữ liệu...
        </div>
        
        <div v-else-if="danhSachLoc.length === 0" class="empty-state">
          <i class="fas fa-comment-slash"></i>
          <p>Không có đánh giá phù hợp với bộ lọc</p>
          <!-- <button class="btn btn-primary" style="height: 100px ; width: 150px;   ;" @click="resetFilters">
            <i class="fas fa-undo"></i>
            Đặt lại bộ lọc
          </button> -->
        </div>

        <div v-else class="table-responsive">
          <table class="reviews-table">
            <thead>
              <tr>
                <th>
                  <i class="fas fa-concierge-bell"></i>
                  Dịch vụ
                </th>
                <th>
                  <i class="fas fa-user"></i>
                  Người đánh giá
                </th>
                <th>
                  <i class="fas fa-star"></i>
                  Đánh giá
                </th>
                <th>
                  <i class="fas fa-comment-dots"></i>
                  Nội dung
                </th>
                <th>
                  <i class="fas fa-calendar-day"></i>
                  Ngày tạo
                </th>
                <th>
                  <i class="fas fa-check-circle"></i>
                  Trạng thái
                </th>
                <th>
                  <i class="fas fa-tools"></i>
                  Hành động
                </th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="dg in danhSachLoc" :key="dg.id" class="review-row">
                <td>
                  <div class="service-info">
                    <i class="fas fa-concierge-bell"></i>
                    <strong>{{ dg.dichVu?.tenDichVu || 'Không rõ' }}</strong>
                  </div>
                </td>
                <td>
                  <div class="user-info">
                    <span v-if="dg.anDanh" class="anonymous-user">
                      <i class="fas fa-user-secret"></i>
                      Ẩn danh
                    </span>
                    <span v-else class="named-user">
                      <i class="fas fa-user"></i>
                      {{ dg.user?.ten || 'Chưa rõ' }}
                    </span>
                  </div>
                </td>
                <td>
                  <div class="rating-display">
                    <div class="stars">
                      <i 
                        v-for="n in 5" 
                        :key="n"
                        class="fas fa-star"
                        :class="n <= dg.soSao ? 'star-filled' : 'star-empty'"
                      ></i>
                    </div>
                    <span class="rating-number">{{ dg.soSao }}/5</span>
                  </div>
                </td>
                <td>
                  <div class="review-content">
                    {{ dg.noiDung || '(Không có nội dung)' }}
                  </div>
                </td>
                <td>
                  <div class="date-info">
                    <i class="fas fa-clock"></i>
                    {{ formatDate(dg.ngayTao) }}
                  </div>
                </td>
                <td>
                  <span class="status-badge" :class="getStatusClass(dg)">
                    <i :class="getStatusIcon(dg)"></i>
                    {{ getStatusText(dg) }}
                  </span>
                </td>
                <td>
                  <div class="action-buttons">
                    <button
                      v-if="!dg.daDuyet"
                      @click="duyetDanhGia(dg.id)"
                      class="btn btn-success btn-sm"
                      title="Duyệt đánh giá"
                    >
                      <i class="fas fa-check"></i>
                    </button>
                    <button
                      @click="toggleTrangThai(dg.id)"
                      class="btn btn-sm"
                      :class="dg.isActive ? 'btn-warning' : 'btn-info'"
                      :title="dg.isActive ? 'Ẩn đánh giá' : 'Hiện lại đánh giá'"
                    >
                      <i :class="dg.isActive ? 'fas fa-eye-slash' : 'fas fa-eye'"></i>
                    </button>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <!-- Biểu đồ thống kê -->
    <div class="card">
      <div class="card-header">
        <h2 class="section-title">
          <i class="fas fa-chart-bar"></i>
          Thống kê đánh giá
        </h2>
      </div>
      <div class="card-body">
        <div class="charts-container">
          <div class="chart-item">
            <h5 class="chart-title">
              <i class="fas fa-chart-column"></i>
              Dịch vụ được đánh giá nhiều nhất
            </h5>
            <div class="chart-wrapper">
              <BarChart :data="chartDataSoLuong" :options="chartOptions" />
            </div>
          </div>
          <div class="chart-item">
            <h5 class="chart-title">
              <i class="fas fa-star"></i>
              Dịch vụ có điểm đánh giá cao nhất
            </h5>
            <div class="chart-wrapper">
              <BarChart :data="chartDataTrungBinh" :options="chartOptions" />
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from "vue";
import axios from "axios";
import BarChart from "@/components/BarChart.vue";

const loading = ref(false);
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
  maintainAspectRatio: false,
  plugins: { 
    legend: { display: false },
    tooltip: {
      backgroundColor: 'rgba(0,0,0,0.8)',
      titleColor: 'white',
      bodyColor: 'white'
    }
  },
  scales: { 
    y: { 
      beginAtZero: true,
      grid: {
        color: 'rgba(0,0,0,0.1)'
      }
    },
    x: {
      grid: {
        display: false
      }
    }
  },
};

onMounted(async () => await loadDanhSach());

const toggleTabs = () => {
  showTabs.value = !showTabs.value;
  if (!showTabs.value) {
    selectedDichVu.value = 'all';
  }
};

const loadDanhSach = async () => {
  loading.value = true;
  try {
    const res = await axios.get("http://localhost:5055/api/DanhGia/admin");
    danhSach.value = res.data;
    updateCharts();
  } catch (err) {
    console.error("Lỗi khi tải đánh giá:", err);
  } finally {
    loading.value = false;
  }
};

const duyetDanhGia = async (id) => {
  if (confirm("Bạn có chắc muốn duyệt đánh giá này?")) {
    try {
      await axios.put(`http://localhost:5055/api/DanhGia/duyet/${id}`);
      await loadDanhSach();
    } catch (err) {
      console.error("Lỗi khi duyệt:", err);
      alert("Có lỗi xảy ra khi duyệt đánh giá");
    }
  }
};

const toggleTrangThai = async (id) => {
  if (confirm("Bạn có chắc muốn thay đổi trạng thái hiển thị của đánh giá này?")) {
    try {
      await axios.put(`http://localhost:5055/api/DanhGia/toggle/${id}`);
      await loadDanhSach();
    } catch (err) {
      console.error("Lỗi khi thay đổi trạng thái:", err);
      alert("Có lỗi xảy ra khi thay đổi trạng thái");
    }
  }
};

const formatDate = (dateStr) => {
  return new Date(dateStr).toLocaleString("vi-VN", {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit'
  });
};

const getStatusClass = (review) => {
  if (!review.daDuyet) return 'status-pending';
  if (!review.isActive) return 'status-hidden';
  return 'status-approved';
};

const getStatusIcon = (review) => {
  if (!review.daDuyet) return 'fas fa-hourglass-start';
  if (!review.isActive) return 'fas fa-eye-slash';
  return 'fas fa-check-circle';
};

const getStatusText = (review) => {
  if (!review.daDuyet) return 'Chưa duyệt';
  if (!review.isActive) return 'Đã ẩn';
  return 'Đã duyệt';
};

const applyFilters = () => {
  // Trigger reactivity by updating computed
  console.log('Filters applied');
};

const resetFilters = () => {
  filter.value = "all";
  searchName.value = "";
  startDate.value = "";
  endDate.value = "";
  selectedDichVu.value = "all";
  showTabs.value = false;
};

const dichVuTabs = computed(() => {
  const tenDVs = danhSach.value.map(dg => dg.dichVu?.tenDichVu).filter(Boolean);
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
    if (!groupByService[ten]) groupByService[ten] = { count: 0, total: 0 };
    groupByService[ten].count++;
    groupByService[ten].total += dg.soSao;
  });

  const labels = Object.keys(groupByService);
  const counts = labels.map((ten) => groupByService[ten].count);
  const averages = labels.map((ten) => (groupByService[ten].total / groupByService[ten].count).toFixed(1));

  chartDataSoLuong.value = {
    labels,
    datasets: [{
      label: "Số lượt đánh giá",
      backgroundColor: "linear-gradient(135deg, #667eea 0%, #764ba2 100%)",
      borderColor: "#667eea",
      borderWidth: 2,
      data: counts
    }],
  };

  chartDataTrungBinh.value = {
    labels,
    datasets: [{
      label: "Điểm trung bình",
      backgroundColor: "linear-gradient(135deg, #f093fb 0%, #f5576c 100%)",
      borderColor: "#f093fb",
      borderWidth: 2,
      data: averages
    }],
  };
};
</script>

<style scoped>
.review-management {
  max-width: 1400px;
  margin: 0 auto;
  padding: 20px;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

.header {
  margin-bottom: 30px;
}

.title {
  color: #2c3e50;
  font-size: 2.5rem;
  font-weight: 600;
  margin: 0;
  display: flex;
  align-items: center;
  gap: 15px;
}

.title i {
  color: #f39c12;
}

.card {
  background: white;
  border-radius: 12px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.07);
  margin-bottom: 30px;
  overflow: hidden;
  border: 1px solid #e1e8ed;
}

.card-header {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 20px 25px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.section-title {
  margin: 0;
  font-size: 1.4rem;
  font-weight: 500;
  display: flex;
  align-items: center;
  gap: 10px;
}

.badge {
  background: rgba(255, 255, 255, 0.2);
  padding: 4px 12px;
  border-radius: 20px;
  font-size: 0.9rem;
  margin-left: 10px;
}

.stats-info {
  display: flex;
  gap: 20px;
  font-size: 0.9rem;
}

.stat-item {
  display: flex;
  align-items: center;
  gap: 5px;
  background: rgba(255, 255, 255, 0.1);
  padding: 5px 10px;
  border-radius: 15px;
}

.card-body {
  padding: 25px;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 20px;
  margin-bottom: 25px;
}

.form-group {
  display: flex;
  flex-direction: column;
}

.form-group label {
  font-weight: 600;
  color: #2c3e50;
  margin-bottom: 8px;
  font-size: 0.95rem;
}

.form-control {
  padding: 12px 15px;
  border: 2px solid #e1e8ed;
  border-radius: 8px;
  font-size: 1rem;
  transition: all 0.3s ease;
  background: #fafbfc;
}

.form-control:focus {
  outline: none;
  border-color: #3498db;
  background: white;
  box-shadow: 0 0 0 3px rgba(52, 152, 219, 0.1);
}

.status-filter-group {
  display: flex;
  gap: 10px;
  flex-wrap: wrap;
}

.status-btn {
  flex: 1;
  min-width: 120px;
  display: flex;
  align-items: center;
  gap: 8px;
  justify-content: center;
}

/* Search Bar Styles */
.search-container {
  position: relative;
}

.search-input {
  width: 100%;
  padding: 12px 45px 12px 15px;
  border: 2px solid #e1e8ed;
  border-radius: 25px;
  font-size: 1rem;
  background: rgba(255, 255, 255, 0.9);
  color: #2c3e50;
  transition: all 0.3s ease;
}

.search-input:focus {
  outline: none;
  border-color: #3498db;
  background: white;
  box-shadow: 0 0 0 3px rgba(52, 152, 219, 0.1);
}

.search-icon {
  position: absolute;
  right: 15px;
  top: 50%;
  transform: translateY(-50%);
  color: rgba(13, 13, 13, 0.8);
  font-size: 1.1rem;
}

.clear-search {
  position: absolute;
  right: 40px;
  top: 50%;
  transform: translateY(-50%);
  background: none;
  border: none;
  color: rgba(0, 0, 0, 0.6);
  cursor: pointer;
  padding: 5px;
  border-radius: 50%;
  transition: all 0.3s ease;
  display: none;
}

.clear-search:hover {
  background: rgba(0, 0, 0, 0.1);
  color: #e74c3c;
}

.clear-search.show {
  display: block;
}

.service-filter-section {
  margin-top: 25px;
  padding-top: 20px;
  border-top: 1px solid #e1e8ed;
}

.service-toggle-btn {
  width: 100%;
  justify-content: space-between;
  margin-bottom: 15px;
}

.transition-icon {
  transition: transform 0.3s ease;
}

.service-tabs {
  display: flex;
  gap: 10px;
  flex-wrap: wrap;
}

.service-tab {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 8px 16px;
  font-size: 0.9rem;
}

.slide-down-enter-active,
.slide-down-leave-active {
  transition: all 0.3s ease;
  overflow: hidden;
}

.slide-down-enter-from,
.slide-down-leave-to {
  opacity: 0;
  max-height: 0;
  transform: translateY(-10px);
}

.slide-down-enter-to,
.slide-down-leave-from {
  opacity: 1;
  max-height: 200px;
  transform: translateY(0);
}

.form-actions {
  display: flex;
  gap: 15px;
  justify-content: flex-start;
  margin-top: 20px;
}

.btn {
  padding: 12px 24px;
  border: none;
  border-radius: 8px;
  font-size: 1rem;
  font-weight: 500;
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  gap: 8px;
  transition: all 0.3s ease;
  text-decoration: none;
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-primary {
  background: linear-gradient(135deg, #3498db, #2980b9);
  color: white;
}

.btn-primary:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 4px 15px rgba(52, 152, 219, 0.4);
}

.btn-success {
  background: linear-gradient(135deg, #27ae60, #229954);
  color: white;
}

.btn-success:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 4px 15px rgba(39, 174, 96, 0.4);
}

.btn-warning {
  background: linear-gradient(135deg, #f39c12, #e67e22);
  color: white;
}

.btn-info {
  background: linear-gradient(135deg, #3498db, #2980b9);
  color: white;
}

.btn-outline-primary {
  background: transparent;
  color: #3498db;
  border: 2px solid #3498db;
}

.btn-outline-primary:hover:not(:disabled) {
  background: #3498db;
  color: white;
}

.btn-outline-secondary {
  background: transparent;
  color: #6c757d;
  border: 2px solid #6c757d;
}

.list-controls {
  display: flex;
  gap: 10px;
}

.loading-state, .empty-state {
  text-align: center;
  padding: 60px 20px;
  color: #7f8c8d;
  font-size: 1.1rem;
}

.loading-state i {
  font-size: 2rem;
  margin-bottom: 15px;
  display: block;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.empty-state i {
  font-size: 4rem;
  margin-bottom: 20px;
  display: block;
  color: #bdc3c7;
}

.table-responsive {
  overflow-x: auto;
}

.reviews-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 20px;
}

.reviews-table th {
  background: linear-gradient(135deg, #34495e, #2c3e50);
  color: white;
  padding: 15px 12px;
  text-align: left;
  font-weight: 600;
  font-size: 0.95rem;
  border-bottom: 3px solid #3498db;
}

.reviews-table td {
  padding: 15px 12px;
  border-bottom: 1px solid #ecf0f1;
  vertical-align: middle;
}

.review-row:hover {
  background: #f8f9fa;
}

/* Continuation of Review Management CSS */

.service-info {
  display: flex;
  align-items: center;
  gap: 8px;
  color: #2c3e50;
}

.service-info i {
  color: #3498db;
  font-size: 1.1rem;
}

.user-info {
  display: flex;
  align-items: center;
  gap: 8px;
}

.anonymous-user {
  color: #7f8c8d;
  font-style: italic;
  display: flex;
  align-items: center;
  gap: 6px;
}

.anonymous-user i {
  color: #95a5a6;
}

.named-user {
  color: #2c3e50;
  font-weight: 500;
  display: flex;
  align-items: center;
  gap: 6px;
}

.named-user i {
  color: #3498db;
}

.rating-display {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 5px;
}

.stars {
  display: flex;
  gap: 2px;
}

.stars i {
  font-size: 1rem;
}

.star-filled {
  color: #f39c12;
}

.star-empty {
  color: #ddd;
}

.rating-number {
  font-size: 0.85rem;
  font-weight: 600;
  color: #2c3e50;
  background: #f8f9fa;
  padding: 2px 8px;
  border-radius: 12px;
}

.review-content {
  max-width: 200px;
  line-height: 1.4;
  color: #2c3e50;
  font-size: 0.95rem;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.date-info {
  display: flex;
  align-items: center;
  gap: 6px;
  color: #7f8c8d;
  font-size: 0.9rem;
}

.date-info i {
  color: #95a5a6;
}

.status-badge {
  padding: 6px 12px;
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  display: inline-flex;
  align-items: center;
  gap: 6px;
}

.status-pending {
  background: linear-gradient(135deg, #f39c12, #e67e22);
  color: white;
}

.status-approved {
  background: linear-gradient(135deg, #27ae60, #229954);
  color: white;
}

.status-hidden {
  background: linear-gradient(135deg, #95a5a6, #7f8c8d);
  color: white;
}

.action-buttons {
  display: flex;
  gap: 8px;
  align-items: center;
  justify-content: center;
}

.btn-sm {
  padding: 8px 12px;
  font-size: 0.85rem;
  min-width: auto;
}

.btn-info {
  background: linear-gradient(135deg, #3498db, #2980b9);
  color: white;
}

.btn-info:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 4px 15px rgba(52, 152, 219, 0.4);
}

/* Charts Container */
.charts-container {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(400px, 1fr));
  gap: 30px;
  margin-top: 20px;
}

.chart-item {
  background: #f8f9fa;
  border-radius: 12px;
  padding: 20px;
  border: 1px solid #e1e8ed;
}

.chart-title {
  color: #2c3e50;
  font-size: 1.2rem;
  font-weight: 600;
  margin: 0 0 20px 0;
  display: flex;
  align-items: center;
  gap: 10px;
}

.chart-title i {
  color: #3498db;
}

.chart-wrapper {
  height: 300px;
  position: relative;
}

/* Enhanced Form Styles */
.form-label {
  font-weight: 600;
  color: #2c3e50;
  margin-bottom: 8px;
  font-size: 0.95rem;
  display: flex;
  align-items: center;
  gap: 8px;
}

.form-label i {
  color: #3498db;
  font-size: 1rem;
}

/* Enhanced Search Container */
.search-container {
  position: relative;
  width: 100%;
  max-width: 400px;
}

.search-input {
  width: 100%;
  padding: 12px 45px 12px 15px;
  border: 2px solid #e1e8ed;
  border-radius: 25px;
  font-size: 1rem;
  background: rgba(255, 255, 255, 0.9);
  color: #2c3e50;
  transition: all 0.3s ease;
}

.search-input:focus {
  outline: none;
  border-color: #3498db;
  background: white;
  box-shadow: 0 0 0 3px rgba(52, 152, 219, 0.1);
}

.search-input::placeholder {
  color: rgba(67, 63, 63, 0.7);
}

.search-icon {
  position: absolute;
  right: 15px;
  top: 50%;
  transform: translateY(-50%);
  color: rgba(13, 13, 13, 0.8);
  font-size: 1.1rem;
  cursor: pointer;
  transition: color 0.3s ease;
}

.search-icon:hover {
  color: #3498db;
}

.clear-search {
  position: absolute;
  right: 40px;
  top: 50%;
  transform: translateY(-50%);
  background: none;
  border: none;
  color: rgba(0, 0, 0, 0.6);
  cursor: pointer;
  padding: 5px;
  border-radius: 50%;
  transition: all 0.3s ease;
}

.clear-search:hover {
  background: rgba(0, 0, 0, 0.1);
  color: #e74c3c;
}

/* Enhanced Button Styles */
.btn {
  padding: 12px 24px;
  border: none;
  border-radius: 8px;
  font-size: 1rem;
  font-weight: 500;
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  gap: 8px;
  transition: all 0.3s ease;
  text-decoration: none;
  white-space: nowrap;
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  transform: none !important;
}

.btn-primary {
  background: linear-gradient(135deg, #3498db, #2980b9);
  color: white;
}

.btn-primary:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 4px 15px rgba(52, 152, 219, 0.4);
}

.btn-success {
  background: linear-gradient(135deg, #27ae60, #229954);
  color: white;
}

.btn-success:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 4px 15px rgba(39, 174, 96, 0.4);
}

.btn-warning {
  background: linear-gradient(135deg, #f39c12, #e67e22);
  color: white;
}

.btn-warning:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 4px 15px rgba(243, 156, 18, 0.4);
}

.btn-outline-primary {
  background: transparent;
  color: #3498db;
  border: 2px solid #3498db;
}

.btn-outline-primary:hover:not(:disabled) {
  background: #3498db;
  color: white;
  transform: translateY(-2px);
}

.btn-outline-secondary {
  background: transparent;
  color: #6c757d;
  border: 2px solid #6c757d;
}

.btn-outline-secondary:hover:not(:disabled) {
  background: #6c757d;
  color: white;
  transform: translateY(-2px);
}

/* Service Toggle and Tabs */
.service-toggle-btn {
  width: 100%;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 15px;
  padding: 15px 20px;
}

.transition-icon {
  transition: transform 0.3s ease;
  margin-left: auto;
}

.service-tabs {
  display: flex;
  gap: 10px;
  flex-wrap: wrap;
  margin-top: 15px;
}

.service-tab {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 10px 16px;
  font-size: 0.9rem;
  border-radius: 6px;
  transition: all 0.3s ease;
}

.service-tab:hover:not(:disabled) {
  transform: translateY(-1px);
}

/* Animations */
.slide-down-enter-active,
.slide-down-leave-active {
  transition: all 0.3s ease;
  overflow: hidden;
}

.slide-down-enter-from,
.slide-down-leave-to {
  opacity: 0;
  max-height: 0;
  transform: translateY(-10px);
}

.slide-down-enter-to,
.slide-down-leave-from {
  opacity: 1;
  max-height: 200px;
  transform: translateY(0);
}

/* Loading and Empty States */
.loading-state, .empty-state {
  text-align: center;
  padding: 60px 20px;
  color: #7f8c8d;
  font-size: 1.1rem;
}

.loading-state i {
  font-size: 2rem;
  margin-bottom: 15px;
  display: block;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.empty-state i {
  font-size: 4rem;
  margin-bottom: 20px;
  display: block;
  color: #bdc3c7;
}

.empty-state p {
  margin-bottom: 20px;
  font-size: 1.2rem;
}

/* Enhanced Table Styles */
.table-responsive {
  overflow-x: auto;
  margin-top: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.reviews-table {
  width: 100%;
  border-collapse: collapse;
  background: white;
}

.reviews-table th {
  background: linear-gradient(135deg, #34495e, #2c3e50);
  color: white;
  padding: 15px 12px;
  text-align: left;
  font-weight: 600;
  font-size: 0.95rem;
  border-bottom: 3px solid #3498db;
  position: sticky;
  top: 0;
  z-index: 10;
}

.reviews-table th i {
  margin-right: 8px;
  color: #3498db;
}

.reviews-table td {
  padding: 15px 12px;
  border-bottom: 1px solid #ecf0f1;
  vertical-align: middle;
  transition: background-color 0.3s ease;
}

.review-row:hover {
  background: #f8f9fa;
}

.review-row:nth-child(even) {
  background: #fafbfc;
}

.review-row:nth-child(even):hover {
  background: #f1f3f4;
}

/* Responsive Design */
@media (max-width: 1200px) {
  .charts-container {
    grid-template-columns: 1fr;
  }
  
  .chart-item {
    min-width: 300px;
  }
}

@media (max-width: 768px) {
  .review-management {
    padding: 15px;
  }

  .title {
    font-size: 2rem;
  }

  .form-grid {
    grid-template-columns: 1fr;
    gap: 15px;
  }

  .card-header {
    flex-direction: column;
    gap: 15px;
    align-items: stretch;
  }

  .stats-info {
    flex-direction: column;
    gap: 10px;
  }

  .service-tabs {
    flex-direction: column;
  }

  .service-tab {
    justify-content: center;
  }

  .reviews-table {
    font-size: 0.9rem;
  }

  .reviews-table th,
  .reviews-table td {
    padding: 10px 8px;
  }

  .review-content {
    max-width: 150px;
  }

  .action-buttons {
    flex-direction: column;
    gap: 5px;
  }

  .chart-wrapper {
    height: 250px;
  }

  .search-container {
    max-width: none;
  }

  .form-actions {
    flex-direction: column;
    align-items: stretch;
  }

  .btn {
    justify-content: center;
  }
}

@media (max-width: 480px) {
  .table-responsive {
    font-size: 0.8rem;
  }
  
  .reviews-table th,
  .reviews-table td {
    padding: 8px 6px;
  }
  
  .btn {
    padding: 10px 16px;
    font-size: 0.9rem;
  }
  
  .btn-sm {
    padding: 6px 10px;
    font-size: 0.8rem;
  }

  .review-content {
    max-width: 100px;
  }

  .rating-display {
    align-items: flex-start;
  }

  .stars {
    flex-wrap: wrap;
  }

  .chart-wrapper {
    height: 200px;
  }

  .status-filter-group {
    flex-direction: column;
  }

  .status-btn {
    min-width: auto;
  }
}

/* Print Styles */
@media print {
  .card-header,
  .form-actions,
  .action-buttons,
  .charts-container {
    display: none !important;
  }

  .card {
    box-shadow: none;
    border: 1px solid #ddd;
  }

  .reviews-table {
    font-size: 12px;
  }

  .reviews-table th,
  .reviews-table td {
    padding: 8px 4px;
  }
}

/* High Contrast Mode */
@media (prefers-contrast: high) {
  .card {
    border: 2px solid #000;
  }

  .btn {
    border: 2px solid currentColor;
  }

  .form-control {
    border: 2px solid #000;
  }

  .status-badge {
    border: 1px solid #000;
  }
}

/* Reduced Motion */
@media (prefers-reduced-motion: reduce) {
  * {
    animation-duration: 0.01ms !important;
    animation-iteration-count: 1 !important;
    transition-duration: 0.01ms !important;
  }

  .btn:hover {
    transform: none !important;
  }
}

/* Dark Mode Support */
@media (prefers-color-scheme: dark) {
  .review-management {
    background-color: #1a1a1a;
    color: #e1e1e1;
  }

  .card {
    background: #2d2d2d;
    border-color: #404040;
  }

  .form-control {
    background: #404040;
    border-color: #555;
    color: #e1e1e1;
  }

  .form-control:focus {
    border-color: #3498db;
    background: #4a4a4a;
  }

  .reviews-table td {
    border-bottom-color: #404040;
  }

  .review-row:hover {
    background: #3a3a3a;
  }

  .chart-item {
    background: #404040;
    border-color: #555;
  }

  .loading-state, .empty-state {
    color: #bbb;
  }
}
</style>