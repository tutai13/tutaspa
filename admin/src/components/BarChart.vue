<!-- <script setup>
import {
  Chart as ChartJS,
  BarElement,
  CategoryScale,
  LinearScale,
  Tooltip,
  Legend,
} from "chart.js";
import { Bar } from "vue-chartjs";

ChartJS.register(BarElement, CategoryScale, LinearScale, Tooltip, Legend);

defineProps(["data", "options"]);
</script>-->

<template>
  <Bar :data="data" :options="options" />
</template>
