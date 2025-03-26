<template>
  <div id="app">
    <h1>Task Manager</h1>
    <TaskList />
  </div>
</template>

<script>
import TaskList from "./components/TaskList.vue";

export default {
  components: { TaskList },
};
</script>

<style scoped>
h1 {
  text-align: center;
}
</style>
