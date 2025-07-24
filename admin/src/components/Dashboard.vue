<template>
  <div>
    <!-- Thẻ thống kê -->
    <div style="display: flex; gap: 20px; margin-bottom: 30px; flex-wrap: wrap">
      <div class="stat-card" style="background: #f44336; width: 24%">
        <div class="stat-icon">👁️</div>
        <div class="stat-info">
          <div class="stat-number">{{ visits.toLocaleString() }}</div>
          <div class="stat-label">Daily Visits</div>
        </div>
      </div>

      <div class="stat-card" style="background: #2196f3; width: 24%">
        <div class="stat-icon">🛒</div>
        <div class="stat-info">
          <div class="stat-number">{{ sales.toLocaleString() }} đ</div>
          <div class="stat-label">Doanh thu tháng này</div>
        </div>
      </div>

      <div class="stat-card" style="background: #009688; width: 24%">
        <div class="stat-icon">💬</div>
        <div class="stat-info">
          <div class="stat-number">{{ comments.toLocaleString() }}</div>
          <div class="stat-label">Comments</div>
        </div>
      </div>

      <div class="stat-card" style="background: #ffc107; width: 24%">
        <div class="stat-icon">💰</div>
        <div class="stat-info">
          <div class="stat-number">{{ profits.toLocaleString() }} đ</div>
          <div class="stat-label">Doanh thu hôm nay</div>
        </div>
      </div>
    </div>

    <!-- Biểu đồ -->
    <div style="display: flex; gap: 20px; flex-wrap: wrap">
      <!-- Biểu đồ doanh thu từng ngày -->
      <div
        id="dailyRevenueChart"
        style="width: 100%; height: 500px; margin-top: 30px"
      ></div>
      <div id="container" style="width: 49%; height: 500px"></div>
      <div id="container1" style="width: 49%; height: 500px"></div>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref } from "vue";
import axios from "axios";
import anychart from "anychart";

// Các số liệu thống kê
const visits = ref(44023); // (cứng)
const comments = ref(56150); // (cứng)
const sales = ref(0); // lấy từ API: /TongTienThangNay
const profits = ref(0); // lấy từ API: /TongTienHomNay

onMounted(async () => {
  try {
    // 👉 Lấy số liệu tổng tiền tháng này
    const salesRes = await axios.get(
      "http://localhost:5055/api/ThongKe/TongTienThangNay"
    );
    sales.value = salesRes.data.tongTien;

    // 👉 Lấy số liệu tổng tiền hôm nay
    const profitRes = await axios.get(
      "http://localhost:5055/api/ThongKe/TongTienHomNay"
    );
    profits.value = profitRes.data.tongTien;

    // 📦 Biểu đồ 1: Sản phẩm
    const spRes = await axios.get(
      "http://localhost:5055/api/ThongKe/SoLuongSanPham"
    );
    const spData = spRes.data;
    const spChartData = spData.map((item) => [item.productName, item.soLuong]);

    const spChart = anychart.pie(spChartData);
    spChart.title("Tỷ lệ sản phẩm bán ra trong tháng hiện tại");
    spChart.labels().format("{%X}: {%Value} sản phẩm");
    spChart
      .tooltip()
      .format("Chiếm: {%PercentValue}{decimalsCount: 1}%\nSố lượng: {%Value}");
    spChart.container("container");
    spChart.draw();

    // 📦 Biểu đồ 2: Dịch vụ
    const dvRes = await axios.get(
      "http://localhost:5055/api/ThongKe/SoLuongDichVu"
    );
    const dvData = dvRes.data;
    const dvChartData = dvData.map((item) => [item.serviceName, item.soLuong]);

    const dvChart = anychart.pie(dvChartData);
    dvChart.title("Tỷ lệ dịch vụ bán ra trong tháng hiện tại");
    dvChart.labels().format("{%X}: {%Value} lượt");
    dvChart
      .tooltip()
      .format("Chiếm: {%PercentValue}{decimalsCount: 1}%\nSố lượng: {%Value}");
    dvChart.container("container1");
    dvChart.draw();

    // 📈 Biểu đồ đường: Doanh thu từng ngày
    const dtRes = await axios.get(
      "http://localhost:5055/api/ThongKe/DoanhThuTungNgayTrongThang"
    );
    const dtData = dtRes.data;

    // Giữ nguyên dữ liệu gốc từ API
    const dtChartData = dtData.map((item) => ({
      x: item.ngay, // sử dụng chuỗi ngày gốc
      value: item.tongTien,
    }));

    // Tạo biểu đồ đường
    const lineChart = anychart.line();
    lineChart.data(dtChartData);

    // ❗ Format lại trục x để hiển thị đúng ngày từ API (dd/MM)
    lineChart
      .xAxis()
      .labels()
      .format(function () {
        const date = new Date(this.value);
        const day = String(date.getDate()).padStart(2, "0");
        const month = String(date.getMonth() + 1).padStart(2, "0");
        return `${day}/${month}`;
      });

    // Tiêu đề và nhãn trục
    lineChart.title("Biểu đồ doanh thu từng ngày trong tháng");
    //lineChart.xAxis().title("Ngày");
    lineChart.yAxis().title("Doanh thu (VNĐ)");
    // Thêm lưới ngang và đơn vị theo bậc 500.000
    lineChart.yGrid(true);
    lineChart.yScale().ticks().interval(500000);

    // Tooltip
    lineChart.tooltip().format("Doanh thu: {%value}{groupsSeparator: ','} đ");

    // Gắn vào container
    lineChart.container("dailyRevenueChart");
    lineChart.draw();
  } catch (error) {
    console.error("Lỗi khi tải dữ liệu thống kê:", error);
  }
});
</script>

<style scoped>
.stat-card {
  display: flex;
  align-items: center;
  color: white;
  padding: 15px 20px;
  border-radius: 8px;
  width: 220px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}
.stat-icon {
  font-size: 32px;
  margin-right: 15px;
}
.stat-info .stat-number {
  font-size: 22px;
  font-weight: bold;
}
.stat-info .stat-label {
  font-size: 14px;
  opacity: 0.9;
}
</style>
