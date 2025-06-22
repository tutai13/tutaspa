<template>
  <div class="container d-flex flex-column align-items-center justify-content-start mt-5">
    <h2 class="page-title mb-4">Quản lý kho</h2>

    <div class="d-flex gap-3">
      <button class="btn btn-lg btn-outline-primary" @click="goToProducts">Quản lý sản phẩm</button>
      <button class="btn btn-lg btn-outline-success" @click="goToCategories">Quản lý loại sản phẩm</button>
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
</script>

<style scoped>
.page-title {
  font-size: 2.5rem; /* tăng kích cỡ tiêu đề */
  font-weight: bold;
  text-align: center;
}

.btn {
  padding: 12px 24px;
  font-size: 1.2rem; /* tăng kích cỡ chữ trong nút */
  border-radius: 8px;
}
</style>
