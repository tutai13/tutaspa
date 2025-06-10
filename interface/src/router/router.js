import { createRouter, createWebHistory } from "vue-router";
import home from "../components/home.vue";
import login from "../components/login.vue";
import register from "../components/register.vue";

const routes = [
  { path: "/",
    name: "home",
    component: home 
  },
  {
    path : "/login",
    name : "login",
    component :  login
  },
  {
    path : "/register",
    name : "register",
    component :  register
  }
];
const router = createRouter({
  history: createWebHistory(),
  routes,
});
export default router;
