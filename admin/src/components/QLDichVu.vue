<template>
    <div class="container d-flex flex-column align-items-center justify-content-start mt-5">
      <h2 class="page-title text-primary mb-4">
        <i class="fa fa-cogs me-2"></i>Quản lý Dịch vụ
      </h2>
  
      <div class="d-flex gap-3">
        <button class="btn btn-lg btn-outline-primary d-flex align-items-center gap-2" @click="DichVu">
          <i class="fa fa-gear"></i> Dịch vụ
        </button>
        <button class="btn btn-lg btn-outline-success d-flex align-items-center gap-2" @click="LoaiDichVu">
          <i class="fa fa-list-alt"></i> Loại dịch vụ
        </button>
        <button class="btn btn-lg btn-warning text-white d-flex align-items-center gap-2" @click="Banggiadichvu">
  <i class="fa fa-money-bill-wave"></i> Bảng giá 
</button>


      </div>
    </div>
  </template>
  
  <script setup>
  import { useRouter } from 'vue-router';
  
  const router = useRouter();
  
  const DichVu = () => {
    router.push("/dichVu");
  };
  
  const LoaiDichVu = () => {
    router.push("/Loaidichvu");
  };

  const Banggiadichvu = () => {
    router.push("/Banggiadichvu");
  };
  </script>
  
  <style scoped>
  .page-title {
    font-size: 2.5rem;
    font-weight: bold;
  }
  
  .btn {
    padding: 14px 28px;
    font-size: 1.2rem;
    border-radius: 10px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    transition: transform 0.2s ease;
  }
  
  .btn:hover {
    transform: translateY(-2px);
  }
  </style>
  