import { createRouter, createWebHistory } from "vue-router";

// Import các component
import Home from "../components/home.vue";
import DichVu from "../components/DichVu.vue";
import GioiThieu from "../components/GioiThieu.vue";
import LienHe from "../components/LienHe.vue";
import ChiTietDichVu from "../components/ChiTietDichVu.vue";
import login from "../components/login.vue";
import register from "../components/register.vue";
import DatLich from "../components/DatLich.vue";

const routes = [
  { path: "/", name: "Home", component: Home },
  { path: "/DichVu", name: "DichVu", component: DichVu },
  { path: "/GioiThieu", name: "GioiThieu", component: GioiThieu },
  { path: "/LienHe", name: "LienHe", component: LienHe },
  { path: "/DatLich", name: "DatLich", component: DatLich },
  { path: "/ChiTietDichVu", name: "ChiTietDichVu", component: ChiTietDichVu },
  {
    path: "/login",
    name: "login",
    component: login,
  },
  {
    path: "/register",
    name: "register",
    component: register,
  },

  // Route động cho chi tiết dịch vụ
  {
    path: "/dichvu/:id",
    name: "ChiTietDichVu",
    component: () => import("../components/ChiTietDichVu.vue"),
    props: true, // nếu muốn truyền :id làm prop
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
