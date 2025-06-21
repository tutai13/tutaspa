<template>
  <div class="warehouse-container">
    <!-- Tabs Xuất/Nhập hàng -->
    <div class="section switch-section">
      <button @click="formTab = 'import'" :class="{ active: formTab === 'import' }">Nhập hàng</button>
      <button @click="formTab = 'export'" :class="{ active: formTab === 'export' }">Xuất hàng</button>
    </div>

    <!-- Form Nhập/Xuất hàng -->
    <div class="section input-section">
      <h2>{{ formTab === 'import' ? 'Nhập Hàng' : 'Xuất Hàng' }}</h2>
      <div class="form-row">
        <select v-model="formProductId">
          <option disabled value="">-- Chọn sản phẩm --</option>
          <option v-for="p in productList" :key="p.sanPhamId" :value="p.sanPhamId">
            {{ p.tenSP }}
          </option>
        </select>
        <input type="number" v-model="formQuantity" placeholder="Số lượng" />
        <input type="text" v-model="formNote" placeholder="Ghi chú" />
        <button class="action-button" @click="handleSubmit">
          {{ formTab === 'import' ? 'Nhập hàng' : 'Xuất hàng' }}
        </button>
      </div>
    </div>

    <!-- Tabs Lịch sử -->
    <div class="section history-section">
      <h2>Lịch sử Xuất Nhập Hàng</h2>
      <div class="history-tabs">
        <button @click="activeTab = 'import'" :class="{ active: activeTab === 'import' }">Lịch sử Nhập</button>
        <button @click="activeTab = 'export'" :class="{ active: activeTab === 'export' }">Lịch sử Xuất</button>
      </div>

      <!-- Thanh tìm kiếm -->
      <input
        type="text"
        v-model="searchTerm"
        placeholder="Tìm kiếm theo tên sản phẩm hoặc ghi chú..."
        class="search-input"
      />

      <!-- Bảng Lịch sử -->
      <div class="history-table">
        <h3>{{ activeTab === 'import' ? 'Lịch sử Nhập' : 'Lịch sử Xuất' }}</h3>
        <table>
          <thead>
            <tr>
              <th>Sản phẩm</th>
              <th>Số lượng</th>
              <th>Thời gian</th>
              <th>Ghi chú</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in filteredHistory" :key="item.id">
              <td>{{ item.product.productName }}</td>
              <td>{{ activeTab === 'import' ? item.quantityChanged : -item.quantityChanged }}</td>
              <td>{{ new Date(item.timestamp).toLocaleString() }}</td>
              <td>{{ item.note }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      formTab: "import",
      formProductId: null,
      formQuantity: null,
      formNote: "",
      activeTab: "import",
      importHistory: [],
      exportHistory: [],
      productList: [],
      searchTerm: "",
      baseUrl: import.meta.env.VITE_BASE_URL,
    };
  },
  mounted() {
    this.fetchImportHistory();
    this.fetchExportHistory();
    this.fetchProducts();
  },
  computed: {
    filteredHistory() {
      const keyword = this.searchTerm.toLowerCase();
      const list = this.activeTab === "import" ? this.importHistory : this.exportHistory;
      return list.filter(item =>
        item.product.productName.toLowerCase().includes(keyword) ||
        (item.note && item.note.toLowerCase().includes(keyword))
      );
    },
  },
  methods: {
    async fetchProducts() {
      try {
        const res = await axios.get(`${this.baseUrl}/product`);
        this.productList = res.data;
      } catch (err) {
        console.error("Lỗi khi lấy danh sách sản phẩm:", err);
      }
    },
    async fetchImportHistory() {
      try {
        const res = await axios.get(`${this.baseUrl}/inventory/history/imports`);
        this.importHistory = res.data;
      } catch (err) {
        console.error("Lỗi khi lấy lịch sử nhập:", err);
      }
    },
    async fetchExportHistory() {
      try {
        const res = await axios.get(`${this.baseUrl}/inventory/history/exports`);
        this.exportHistory = res.data;
      } catch (err) {
        console.error("Lỗi khi lấy lịch sử xuất:", err);
      }
    },
    async handleSubmit() {
      if (!this.formProductId || this.formQuantity <= 0) {
        alert("Vui lòng chọn sản phẩm và nhập số lượng hợp lệ.");
        return;
      }

      const apiUrl =
        this.formTab === "import"
          ? `${this.baseUrl}/inventory/import`
          : `${this.baseUrl}/inventory/export`;

      try {
        const res = await axios.post(apiUrl, {
          productId: this.formProductId,
          quantityChanged: this.formQuantity,
          note: this.formNote.trim(),
        });

        alert(res.data || "Thành công");

        await this.fetchImportHistory();
        await this.fetchExportHistory();

        this.formProductId = null;
        this.formQuantity = null;
        this.formNote = "";
      } catch (err) {
        console.error("Lỗi xử lý:", err);
        alert("Lỗi: " + (err.response?.data || err.message || "Không thể xử lý"));
      }
    },
  },
};
</script>

<style scoped>
.warehouse-container {
  max-width: 1000px;
  margin: 40px auto;
  padding: 20px;
  font-family: 'Segoe UI', sans-serif;
  background: #f9f9f9;
  border-radius: 12px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
}
.switch-section {
  text-align: center;
  margin-bottom: 20px;
}
.switch-section button {
  padding: 10px 20px;
  margin: 0 10px;
  background: #dee2e6;
  border: none;
  border-radius: 8px;
  font-weight: bold;
  cursor: pointer;
}
.switch-section button.active {
  background: #007bff;
  color: white;
}
.input-section h2 {
  margin-bottom: 10px;
}
.form-row {
  display: flex;
  gap: 12px;
  margin-bottom: 24px;
}
input, select {
  padding: 10px;
  border-radius: 8px;
  border: 1px solid #ccc;
  flex: 1;
}
.action-button {
  background-color: #28a745;
  color: white;
  padding: 10px 16px;
  border: none;
  border-radius: 8px;
  cursor: pointer;
}
.action-button:hover {
  background-color: #218838;
}
.history-tabs {
  margin: 20px 0;
}
.history-tabs button {
  padding: 10px 20px;
  margin-right: 10px;
  background-color: #e9ecef;
  border: none;
  border-radius: 8px;
  cursor: pointer;
}
.history-tabs button.active {
  background-color: #007bff;
  color: white;
}
.search-input {
  margin-bottom: 12px;
  padding: 8px 12px;
  width: 100%;
  border-radius: 8px;
  border: 1px solid #ccc;
}
.history-table h3 {
  margin-bottom: 10px;
}
table {
  width: 100%;
  border-collapse: collapse;
  background: white;
  border-radius: 8px;
}
thead {
  background-color: #007bff;
  color: white;
}
th, td {
  padding: 12px 16px;
  border-bottom: 1px solid #ddd;
}
tr:hover {
  background: #f1f1f1;
}
</style>
