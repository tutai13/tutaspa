<template>
  <div class="container-fluid">
    <!-- Stat Cards -->
    <div class="row g-3 mb-4">
      <!-- Dịch vụ hoàn thành -->
      <div class="col-xl-3 col-lg-6 col-md-6">
        <div
          class="card h-100 border-0 shadow-lg stat-card"
          style="
            background: linear-gradient(135deg, #00b09b 0%, #96c93d 100%);
            border-radius: 15px;
            transition: all 0.3s ease;
            position: relative;
            overflow: hidden;
          "
          @mouseover="handleHover($event, true)"
          @mouseleave="handleHover($event, false)"
        >
          <div class="progress-bar"></div>
          <div class="card-body d-flex align-items-center p-4">
            <div class="stat-icon me-3">✅</div>
            <div class="stat-info text-white">
              <div class="stat-number fs-2 fw-bold mb-1">
                {{ dvHoanThanh.toLocaleString() }}
              </div>
              <div class="stat-label fs-6 fw-medium">Dịch vụ hoàn thành</div>
            </div>
          </div>
        </div>
      </div>

      <!-- Lịch hẹn hôm nay -->
      <div class="col-xl-3 col-lg-6 col-md-6">
        <div
          class="card h-100 border-0 shadow-lg stat-card"
          style="
            background: linear-gradient(135deg, #2193b0 0%, #6dd5ed 100%);
            border-radius: 15px;
            transition: all 0.3s ease;
            position: relative;
            overflow: hidden;
          "
          @mouseover="handleHover($event, true)"
          @mouseleave="handleHover($event, false)"
        >
          <div class="progress-bar"></div>
          <div class="card-body d-flex align-items-center p-4">
            <div class="stat-icon me-3">📅</div>
            <div class="stat-info text-white">
              <div class="stat-number fs-2 fw-bold mb-1">
                {{ lichHen.toLocaleString() }}
              </div>
              <div class="stat-label fs-6 fw-medium">Lịch hẹn hôm nay</div>
            </div>
          </div>
        </div>
      </div>

      <!-- Đánh giá trung bình -->
      <div class="col-xl-3 col-lg-6 col-md-6">
        <div
          class="card h-100 border-0 shadow-lg stat-card"
          style="
            background: linear-gradient(135deg, #ff9a9e 0%, #fecfef 100%);
            border-radius: 15px;
            transition: all 0.3s ease;
            position: relative;
            overflow: hidden;
          "
          @mouseover="handleHover($event, true)"
          @mouseleave="handleHover($event, false)"
        >
          <div class="progress-bar"></div>
          <div class="card-body d-flex align-items-center p-4">
            <div class="stat-icon me-3">⭐</div>
            <div class="stat-info text-white">
              <div class="stat-number fs-2 fw-bold mb-1">
                {{ danhGia }}
              </div>
              <div class="stat-label fs-6 fw-medium">Đánh giá trung bình</div>
            </div>
          </div>
        </div>
      </div>

      <!-- Doanh thu hôm nay -->
      <div class="col-xl-3 col-lg-6 col-md-6">
        <div
          class="card h-100 border-0 shadow-lg stat-card"
          style="
            background: linear-gradient(135deg, #f7971e 0%, #ffd200 100%);
            border-radius: 15px;
            transition: all 0.3s ease;
            position: relative;
            overflow: hidden;
          "
          @mouseover="handleHover($event, true)"
          @mouseleave="handleHover($event, false)"
        >
          <div class="progress-bar"></div>
          <div class="card-body d-flex align-items-center p-4">
            <div class="stat-icon me-3">💵</div>
            <div class="stat-info text-white">
              <div class="stat-number fs-2 fw-bold mb-1">
                {{ doanhThu.toLocaleString() }} đ
              </div>
              <div class="stat-label fs-6 fw-medium">Doanh thu hôm nay</div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Revenue Chart -->
    <div class="row">
      <div class="col-9">
        <div
          class="card border-0 shadow-lg"
          style="border-radius: 20px; overflow: hidden"
        >
          <div class="card-header bg-white border-0 p-4">
            <div class="d-flex justify-content-between align-items-center">
              <h4 class="mb-0 fw-bold text-primary">📊 Biểu đồ Doanh thu</h4>
              <div class="btn-group" role="group">
                <button
                  type="button"
                  class="btn btn-outline-primary"
                  :class="{ active: selectedTimeRange === 'thang' }"
                  @click="changeTimeRange('thang')"
                  style="border-radius: 25px 0 0 25px"
                >
                  Tháng
                </button>
                <button
                  type="button"
                  class="btn btn-outline-primary"
                  :class="{ active: selectedTimeRange === 'nam' }"
                  @click="changeTimeRange('nam')"
                  style="border-radius: 0 25px 25px 0"
                >
                  Năm
                </button>
              </div>
            </div>
          </div>
          <div class="card-body p-4">
            <div v-if="isLoading" class="text-center p-5">
              <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Loading...</span>
              </div>
              <p class="mt-2 text-muted">Đang tải dữ liệu...</p>
            </div>
            <div
              v-else-if="chartData.length > 0"
              ref="chartContainer"
              id="revenueChart"
              style="min-height: 300px; width: 100%"
            ></div>
            <div v-else class="text-center p-5 text-muted">
              <i class="fas fa-chart-bar fa-3x mb-3"></i>
              <p>Không có dữ liệu để hiển thị</p>
              <p class="small">Debug: {{ debugInfo }}</p>
            </div>
          </div>
        </div>
      </div>
      <div class="col-3">
        <div
          class="card border-0 shadow-lg"
          style="border-radius: 20px; overflow: hidden"
        >
          <div class="lich-hen-hom-nay p-4">
            <h4>📅 Lịch Hẹn Hôm Nay</h4>
            <div
              v-for="lich in lichHenHienThi"
              :key="lich.datLichID"
              class="lich-item"
            >
              <div
                class="lich-icon"
                :style="{ backgroundColor: getColor(lich) }"
              ></div>
              <div class="lich-info">
                <div class="lich-ten">
                  {{ lich.soDienThoai }}
                </div>
                <div class="lich-dichvu">
                  {{ getTenDichVu(lich) }}
                </div>
                <div class="lich-thoigian">
                  {{ formatGio(lich.thoiGian) }} - {{ tinhGioKetThuc(lich) }}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Service Distribution Chart -->
    <div class="row mt-4">
      <div class="col-6">
        <div
          class="card border-0 shadow-lg"
          style="border-radius: 20px; overflow: hidden"
        >
          <div class="card-header bg-white border-0 p-4">
            <h4 class="mb-0 fw-bold text-primary">📊 Phân bố Dịch vụ</h4>
          </div>
          <div class="card-body p-4">
            <div v-if="isLoadingService" class="text-center p-5">
              <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Loading...</span>
              </div>
              <p class="mt-2 text-muted">Đang tải dữ liệu...</p>
            </div>
            <div
              v-else-if="serviceData.length > 0"
              style="height: 400px; position: relative"
            >
              <canvas
                ref="pieChart"
                style="max-height: 400px; width: 100%"
              ></canvas>
            </div>
            <div v-else class="text-center p-5 text-muted">
              <i class="fas fa-chart-pie fa-3x mb-3"></i>
              <p>Không có dữ liệu dịch vụ để hiển thị</p>
            </div>
          </div>
        </div>
      </div>
      <div class="col-6">
        <div
          class="card border-0 shadow-lg"
          style="border-radius: 20px; overflow: hidden"
        >
          <div class="card-header bg-white border-0 p-4">
            <h4 class="mb-0 fw-bold text-primary">📊 Phân bố Sản phẩm</h4>
          </div>
          <div class="card-body p-4">
            <div v-if="isLoadingProduct" class="text-center p-5">
              <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Loading...</span>
              </div>
              <p class="mt-2 text-muted">Đang tải dữ liệu...</p>
            </div>
            <div
              v-else-if="productData.length > 0"
              style="height: 400px; position: relative"
            >
              <canvas
                ref="productPieChart"
                style="max-height: 400px; width: 100%"
              ></canvas>
            </div>
            <div v-else class="text-center p-5 text-muted">
              <i class="fas fa-chart-pie fa-3x mb-3"></i>
              <p>Không có dữ liệu sản phẩm để hiển thị</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, nextTick, watch } from "vue";
import apiClient from "../utils/axiosClient";
import Chart from "chart.js/auto";

// Stats
const lichHen = ref(0);
const danhGia = ref("4.7 / 5");
const dvHoanThanh = ref(0);
const doanhThu = ref(0);

// Chart variables
const chartContainer = ref(null);
const selectedTimeRange = ref("thang");
const isLoading = ref(false);
const isLoadingService = ref(false);
const chartData = ref([]);
const debugInfo = ref("");

// Pie chart variables
const pieChart = ref(null);
const serviceData = ref([]);
let chartInstance = null;
let pieChartInstance = null;

// Missing functions that seem to be used in template
const lichHenHienThi = ref([]);
const getColor = (lich) => "#4285f4";
const getTenDichVu = (lich) => lich.tenDichVu || "";
const formatGio = (time) => time || "";
const tinhGioKetThuc = (lich) => "";

// Handle hover effect for stat cards
const handleHover = (event, isHover) => {
  event.currentTarget.style.transform = isHover
    ? "translateY(-5px)"
    : "translateY(0)";
  event.currentTarget.style.boxShadow = isHover
    ? "0 12px 35px rgba(0,0,0,0.25)"
    : "0 8px 25px rgba(0,0,0,0.15)";
};

// Format chart data
const formatDataForChart = (data) => {
  console.log("Raw API data:", data);

  if (!data) {
    debugInfo.value = "No data received from API";
    return [];
  }

  let formattedData = [];

  if (Array.isArray(data)) {
    formattedData = data.map((item) => ({
      x: item.period || item.label || item.name || "Unknown",
      value: Number(item.value || item.amount || item.total || 0),
    }));
  } else if (typeof data === "object") {
    // If data is an object, convert to array
    formattedData = Object.keys(data).map((key) => ({
      x: key,
      value: Number(data[key] || 0),
    }));
  }

  debugInfo.value = `Formatted ${formattedData.length} items`;
  console.log("Formatted chart data:", formattedData);
  return formattedData;
};

// Create custom bar chart
const createChart = (data) => {
  console.log("Creating chart with data:", data);

  if (!chartContainer.value) {
    console.error("Chart container not found");
    debugInfo.value = "Chart container not found";
    return;
  }

  if (!data || data.length === 0) {
    console.log("No data available for chart");
    debugInfo.value = "No data available for chart";
    return;
  }

  // Clear previous chart
  chartContainer.value.innerHTML = "";

  const maxValue = Math.max(...data.map((d) => d.value));
  const dataLength = data.length;

  const { barWidth, minChartWidth, spacing } =
    dataLength <= 12
      ? { barWidth: "60px", minChartWidth: "100%", spacing: "8px" }
      : {
          barWidth: "35px",
          minChartWidth: `${dataLength * 50}px`,
          spacing: "5px",
        };

  const scrollContainer = document.createElement("div");
  scrollContainer.style.cssText = `
    width: 100%;
    overflow-x: auto;
    overflow-y: hidden;
    padding-bottom: 10px;
  `;

  const chartDiv = document.createElement("div");
  chartDiv.style.cssText = `
    display: flex;
    align-items: end;
    justify-content: ${dataLength <= 12 ? "space-around" : "flex-start"};
    height: 350px;
    padding: 20px;
    background: linear-gradient(135deg, #f8f9ff 0%, #ffffff 100%);
    border-radius: 15px;
    min-width: ${minChartWidth};
    gap: ${spacing};
  `;

  data.forEach((item, index) => {
    const barContainer = document.createElement("div");
    barContainer.style.cssText = `
      display: flex;
      flex-direction: column;
      align-items: center;
      width: ${barWidth};
      min-width: ${barWidth};
    `;

    const barHeight = Math.max((item.value / maxValue) * 280, 20);
    const valueLabel = document.createElement("div");

    // Format value display
    if (item.value >= 1000000) {
      valueLabel.textContent = (item.value / 1000000).toFixed(1) + "M";
    } else if (item.value >= 1000) {
      valueLabel.textContent = (item.value / 1000).toFixed(0) + "K";
    } else {
      valueLabel.textContent = item.value.toLocaleString();
    }

    valueLabel.style.cssText = `
      font-size: ${dataLength > 12 ? "10px" : "12px"};
      font-weight: bold;
      color: #333;
      margin-bottom: 5px;
      text-align: center;
      white-space: nowrap;
    `;

    const bar = document.createElement("div");
    bar.style.cssText = `
      width: 100%;
      height: ${barHeight}px;
      background: linear-gradient(135deg, #4285f4 0%, #34a853 100%);
      border-radius: 8px 8px 4px 4px;
      transition: all 0.3s ease;
      cursor: pointer;
      box-shadow: 0 2px 8px rgba(66, 133, 244, 0.3);
    `;

    bar.addEventListener("mouseenter", () => {
      bar.style.transform = "scale(1.05) translateY(-5px)";
      bar.style.boxShadow = "0 8px 16px rgba(66, 133, 244, 0.4)";
    });
    bar.addEventListener("mouseleave", () => {
      bar.style.transform = "scale(1) translateY(0)";
      bar.style.boxShadow = "0 2px 8px rgba(66, 133, 244, 0.3)";
    });

    const xLabel = document.createElement("div");
    xLabel.textContent = item.x;
    xLabel.style.cssText = `
      font-size: ${dataLength > 12 ? "10px" : "12px"};
      color: #666;
      margin-top: 10px;
      text-align: center;
      font-weight: 500;
      white-space: nowrap;
    `;

    barContainer.append(valueLabel, bar, xLabel);
    chartDiv.appendChild(barContainer);
  });

  scrollContainer.appendChild(chartDiv);
  chartContainer.value.appendChild(scrollContainer);

  debugInfo.value = `Chart created with ${data.length} bars`;
  console.log("Chart created successfully");
};

// Load chart data
const loadChartData = async (timeRange) => {
  console.log("Loading chart data for:", timeRange);

  try {
    isLoading.value = true;
    debugInfo.value = "Loading data...";

    const endpoint =
      timeRange === "thang"
        ? "/ThongKe/DoanhThuThangHienTai"
        : "/ThongKe/DoanhThuNamHienTai";

    let data;
    try {
      data = await apiClient.get(endpoint);
      console.log("API Response:", data);
    } catch (apiError) {
      console.warn("API failed, using mock data:", apiError);
      // Use mock data if API fails
      data =
        timeRange === "thang"
          ? mockChartData
          : mockChartData.map((item) => ({
              ...item,
              period: `2024-${item.period.replace("T", "")}`,
            }));
    }

    const formattedData = formatDataForChart(data);
    chartData.value = formattedData;

    if (formattedData.length > 0) {
      await nextTick();
      // Add delay to ensure DOM is ready
      setTimeout(() => {
        createChart(formattedData);
      }, 100);
    } else {
      debugInfo.value = "No data after formatting";
      console.warn("No chart data after formatting");
    }
  } catch (error) {
    console.error("Error loading chart data:", error);
    debugInfo.value = `Error: ${error.message}`;

    // Fallback to mock data
    const formattedData = formatDataForChart(mockChartData);
    chartData.value = formattedData;

    await nextTick();
    setTimeout(() => {
      createChart(formattedData);
    }, 100);
  } finally {
    isLoading.value = false;
  }
};

// Change time range
const changeTimeRange = (timeRange) => {
  selectedTimeRange.value = timeRange;
  loadChartData(timeRange);
};

// Load service data for pie chart
const loadServiceData = async () => {
  console.log("Loading service data...");

  try {
    isLoadingService.value = true;

    const response = await apiClient.get("/ThongKe/SoLuongDichVu");
    console.log("Service API Response:", response);

    // API returns array directly based on your example
    if (Array.isArray(response) && response.length > 0) {
      serviceData.value = response;
    } else {
      console.warn("No service data received, using mock data");
      serviceData.value = mockServiceData;
    }

    if (serviceData.value.length > 0) {
      await nextTick();
      setTimeout(() => {
        createPieChart();
      }, 100);
    }
  } catch (error) {
    console.error("Error loading service data:", error);
    // Use mock data as fallback
    serviceData.value = mockServiceData;
    await nextTick();
    setTimeout(() => {
      createPieChart();
    }, 100);
  } finally {
    isLoadingService.value = false;
  }
};

// Create pie chart
const createPieChart = () => {
  console.log("Creating pie chart with data:", serviceData.value);

  if (!pieChart.value) {
    console.error("Pie chart canvas not found");
    return;
  }

  if (!serviceData.value || serviceData.value.length === 0) {
    console.warn("No service data for pie chart");
    return;
  }

  // Destroy existing chart
  if (pieChartInstance) {
    pieChartInstance.destroy();
  }

  const ctx = pieChart.value.getContext("2d");

  pieChartInstance = new Chart(ctx, {
    type: "pie",
    data: {
      labels: serviceData.value.map((item) => item.serviceName),
      datasets: [
        {
          data: serviceData.value.map((item) => item.phanTram),
          backgroundColor: [
            "#FF6B6B",
            "#4ECDC4",
            "#45B7D1",
            "#96CEB4",
            "#FFEEAD",
            "#D4A5A5",
            "#95E1D3",
            "#F38BA8",
          ],
          borderWidth: 2,
          borderColor: "#fff",
        },
      ],
    },
    options: {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: {
          position: "right",
          labels: {
            font: {
              size: 12,
              family: "Arial",
              weight: "500",
            },
            color: "#333",
            padding: 15,
            usePointStyle: true,
          },
        },
        tooltip: {
          callbacks: {
            label: (context) => {
              const label = context.label || "";
              const value = context.parsed || 0;
              const dataItem = serviceData.value[context.dataIndex];
              return [
                `${label}`,
                `Số lượng: ${dataItem.soLuong.toLocaleString()}`,
                `Tỷ lệ: ${value.toFixed(2)}%`,
              ];
            },
          },
          backgroundColor: "rgba(255, 255, 255, 0.95)",
          titleColor: "#333",
          bodyColor: "#666",
          borderColor: "#ddd",
          borderWidth: 1,
        },
      },
      animation: {
        animateScale: true,
        animateRotate: true,
        duration: 1000,
      },
    },
  });

  console.log("Pie chart created successfully");
};

// Thêm biến cho phần sản phẩm
const productPieChart = ref(null);
const productData = ref([]);
let productPieChartInstance = null;
const isLoadingProduct = ref(false);

// Load product data for pie chart
const loadProductData = async () => {
  console.log("Loading product data...");

  try {
    isLoadingProduct.value = true;

    const response = await apiClient.get("/ThongKe/SoLuongSanPham");
    console.log("Product API Response:", response);

    // API returns array directly based on your example
    if (Array.isArray(response) && response.length > 0) {
      // Tính tổng số lượng
      const total = response.reduce((sum, item) => sum + item.soLuong, 0);
      
      // Thêm phần trăm
      productData.value = response.map(item => ({
        productName: item.productName,
        soLuong: item.soLuong,
        phanTram: total > 0 ? parseFloat(((item.soLuong / total) * 100).toFixed(2)) : 0
      }));
    } else {
      console.warn("No product data received, using mock data");
      productData.value = mockProductData; // Bạn có thể thêm mock data nếu cần
    }

    if (productData.value.length > 0) {
      await nextTick();
      setTimeout(() => {
        createProductPieChart();
      }, 100);
    }
  } catch (error) {
    console.error("Error loading product data:", error);
    // Use mock data as fallback
    productData.value = mockProductData || [];
    await nextTick();
    setTimeout(() => {
      createProductPieChart();
    }, 100);
  } finally {
    isLoadingProduct.value = false;
  }
};

// Create product pie chart
const createProductPieChart = () => {
  console.log("Creating product pie chart with data:", productData.value);

  if (!productPieChart.value) {
    console.error("Product pie chart canvas not found");
    return;
  }

  if (!productData.value || productData.value.length === 0) {
    console.warn("No product data for pie chart");
    return;
  }

  // Destroy existing chart
  if (productPieChartInstance) {
    productPieChartInstance.destroy();
  }

  const ctx = productPieChart.value.getContext("2d");

  productPieChartInstance = new Chart(ctx, {
    type: "pie",
    data: {
      labels: productData.value.map((item) => item.productName),
      datasets: [
        {
          data: productData.value.map((item) => item.phanTram),
          backgroundColor: [
            "#FF6B6B",
            "#4ECDC4",
            "#45B7D1",
            "#96CEB4",
            "#FFEEAD",
            "#D4A5A5",
            "#95E1D3",
            "#F38BA8",
          ],
          borderWidth: 2,
          borderColor: "#fff",
        },
      ],
    },
    options: {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: {
          position: "right",
          labels: {
            font: {
              size: 12,
              family: "Arial",
              weight: "500",
            },
            color: "#333",
            padding: 15,
            usePointStyle: true,
          },
        },
        tooltip: {
          callbacks: {
            label: (context) => {
              const label = context.label || "";
              const value = context.parsed || 0;
              const dataItem = productData.value[context.dataIndex];
              return [
                `${label}`,
                `Số lượng: ${dataItem.soLuong.toLocaleString()}`,
                `Tỷ lệ: ${value.toFixed(2)}%`,
              ];
            },
          },
          backgroundColor: "rgba(255, 255, 255, 0.95)",
          titleColor: "#333",
          bodyColor: "#666",
          borderColor: "#ddd",
          borderWidth: 1,
        },
      },
      animation: {
        animateScale: true,
        animateRotate: true,
        duration: 1000,
      },
    },
  });

  console.log("Product pie chart created successfully");
};

// Watch for data changes
watch(chartData, (newData) => {
  if (newData && newData.length > 0 && chartContainer.value) {
    nextTick(() => {
      createChart(newData);
    });
  }
});

watch(serviceData, (newData) => {
  if (newData && newData.length > 0 && pieChart.value) {
    nextTick(() => {
      createPieChart();
    });
  }
});

// Watch for product data changes
watch(productData, (newData) => {
  if (newData && newData.length > 0 && productPieChart.value) {
    nextTick(() => {
      createProductPieChart();
    });
  }
});

// Initialize component
onMounted(async () => {
  console.log("Component mounted, initializing...");

  try {
    // Load basic stats
    try {
      const revenueData = await apiClient.get("/ThongKe/TongTienHomNay");
      doanhThu.value = revenueData.tongTien || 0;

      const appointmentData = await apiClient.get("/ThongKe/lichHenToday");
      lichHen.value = appointmentData.soLichHomNay || 0;

      const serviceCompletedData = await apiClient.get(
        "/ThongKe/dvHoanThanhToday"
      );
      dvHoanThanh.value = serviceCompletedData.soLuotDichVu || 0;
    } catch (statsError) {
      console.warn("Error loading stats, using defaults:", statsError);
      // Use default values
    }

    // Load charts
    await Promise.all([
      loadServiceData(),
      loadProductData(),
      loadChartData(selectedTimeRange.value),
    ]);
  } catch (error) {
    console.error("Initialization error:", error);
  }
});
</script>

<style scoped>
.card {
  animation: fadeIn 0.6s ease-out;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.stat-card {
  cursor: pointer;
}

.stat-icon {
  font-size: 3rem;
  opacity: 0.9;
  filter: drop-shadow(0 2px 8px rgba(0, 0, 0, 0.2));
}

.stat-number {
  text-shadow: 0 2px 8px rgba(0, 0, 0, 0.2);
}

.stat-label {
  opacity: 0.9;
  letter-spacing: 0.5px;
}

.progress-bar {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 4px;
  background: linear-gradient(
    90deg,
    rgba(255, 255, 255, 0.3),
    rgba(255, 255, 255, 0.7),
    rgba(255, 255, 255, 0.3)
  );
}

.btn-group .btn.active {
  background-color: #0d6efd;
  color: white;
  border-color: #0d6efd;
}

/* Custom scrollbar for chart */
#revenueChart::-webkit-scrollbar {
  height: 8px;
}

#revenueChart::-webkit-scrollbar-track {
  background: #f1f1f1;
  border-radius: 10px;
}

#revenueChart::-webkit-scrollbar-thumb {
  background: linear-gradient(135deg, #4285f4 0%, #34a853 100%);
  border-radius: 10px;
}

#revenueChart::-webkit-scrollbar-thumb:hover {
  background: linear-gradient(135deg, #3367d6 0%, #2e7d32 100%);
}

@media (max-width: 768px) {
  .stat-number {
    font-size: 1.5rem !important;
  }
  .stat-icon {
    font-size: 2.5rem !important;
  }
  .btn-group {
    flex-direction: column;
    width: 100%;
  }
  .btn-group .btn {
    border-radius: 8px !important;
    margin-bottom: 5px;
  }
}
</style>