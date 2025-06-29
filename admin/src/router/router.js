import { createRouter, createWebHistory } from "vue-router";
import Dashboard from "../components/Dashboard.vue";
import khuyenMai from "../components/khuyenMai.vue";
import dichVu from "../components/dichVu.vue";
import Loaidichvu from "../components/Loaidichvu.vue";
import QLDichVu from "../components/QLDichVu.vue";
import kho from "../components/kho.vue";
import ThuNgan from "../components/ThuNgan.vue";
import login from "../components/login.vue";
import EmployeeManagerment from "../components/EmployeeManagerment.vue";

const routes = [
  { path: "/", name: "Dashboard", component: Dashboard },
  { path: "/khuyenMai", name: "khuyenMai", component: khuyenMai },
  { path: "/dichVu", name: "dichVu", component: dichVu },
  { path: "/Loaidichvu", name: "Loaidichvu", component: Loaidichvu },
  { path: "/QlDichVu", name: "QlDichVu", component: QLDichVu },
  { path: "/kho", name: "kho", component: kho },
  { path: "/ThuNgan", name: "ThuNgan", component: ThuNgan },
  { path: "/employees", name: "employees", component: EmployeeManagerment },


  {
    path: "/login", name: "login", component: login, meta: {
      layout: false, // Không sử dụng layout chung
      requiresAuth: false
    }
  },

];
const router = createRouter({
  history: createWebHistory(),
  routes,
});
export default router;
