import { createRouter, createWebHistory } from "vue-router";
import Dashboard from "../components/Dashboard.vue";
import khuyenMai from "../components/khuyenMai.vue";
import dichVu from "../components/dichVu.vue";
import kho from "../components/kho.vue";
import ThuNgan from "../components/ThuNgan.vue";
import login from "../components/login.vue";

const routes = [
  { path: "/", name: "Dashboard", component: Dashboard },
  { path: "/khuyenMai", name: "khuyenMai", component: khuyenMai },
  { path: "/dichVu", name: "dichVu", component: dichVu },

  { path: "/kho", name: "kho", component: kho },
  { path: "/ThuNgan", name: "ThuNgan", component: ThuNgan },


  {path : "/login", name: "login", component: login , meta: {
      layout: false, // Không sử dụng layout chung
      requiresAuth: false
    }},

];
const router = createRouter({
  history: createWebHistory(),
  routes,
});
export default router;
