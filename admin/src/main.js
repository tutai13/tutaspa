import { createApp } from "vue";
//import "./style.css";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min.js";
import "@fortawesome/fontawesome-free/css/all.min.css";
import { createPinia } from 'pinia'
import App from "./App.vue";
import router from "./router/router";
createApp(App).use(createPinia()).use(router).mount("#app");
