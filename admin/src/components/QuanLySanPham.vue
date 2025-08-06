<template>
  <div class="container d-flex flex-column align-items-center justify-content-start mt-5 min-vh-100" style="background: linear-gradient(135deg, #f0f4f8 0%, #d9e2f2 100%);">
    <h2 class="page-title mb-5 text-dark">Quản lý kho</h2>

    <div class="d-flex flex-wrap gap-4 justify-content-center">
      <div class="card btn-card" @click="goToProducts">
        <div class="card-body text-center">
          <i class="bi bi-box-seam display-4 text-primary"></i>
          <h4 class="card-title mt-3">Quản lý sản phẩm</h4>
        </div>
      </div>
      <div class="card btn-card" @click="goToCategories">
        <div class="card-body text-center">
          <i class="bi bi-list-ul display-4 text-success"></i>
          <h4 class="card-title mt-3">Quản lý loại sản phẩm</h4>
        </div>
      </div>
      <div class="card btn-card" @click="gotoInventory">
        <div class="card-body text-center">
          <i class="bi bi-arrow-left-right display-4 text-success"></i>
          <h4 class="card-title mt-3">Quản lý xuất/nhập hàng</h4>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useRouter } from 'vue-router';

const router = useRouter();

const goToProducts = () => {
  router.push("/Products");
};

const goToCategories = () => {
  router.push("/categories");
};

const gotoInventory = () => {
  router.push("/Kho");
};
</script>

<style scoped>
.page-title {
  font-size: 2.8rem;
  font-weight: 700;
  color: #333;
  text-shadow: 1px 1px 4px rgba(0, 0, 0, 0.1);
  text-align: center;
  margin-bottom: 3rem;
}

.btn-card {
  width: 250px;
  height: 200px;
  border: none;
  border-radius: 15px;
  background: white;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
  transition: transform 0.3s ease, box-shadow 0.3s ease;
  cursor: pointer;
}

.btn-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.2);
}

.card-body {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  height: 100%;
}

.card-title {
  font-size: 1.3rem;
  font-weight: 600;
  color: #444;
}

.display-4 {
  font-size: 2.5rem;
}

@media (max-width: 768px) {
  .page-title {
    font-size: 2.2rem;
  }

  .btn-card {
    width: 100%;
    max-width: 250px;
    height: 180px;
  }

  .card-title {
    font-size: 1.1rem;
  }

  .display-4 {
    font-size: 2rem;
  }
}
</style>