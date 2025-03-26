<template>
  <div>
    <h2>Create Task</h2>
    <form @submit.prevent="createTask">
      <label>Title:</label>
      <input v-model="title" required />

      <label>Description:</label>
      <input v-model="description" required />

      <label>Assigned User ID:</label>
      <input type="number" v-model="assignedUserId" />

      <button type="submit" :disabled="loading">
        {{ loading ? "Creating..." : "Create Task" }}
      </button>
    </form>
  </div>
</template>

<script>
import { ref } from "vue";
import axios from "axios";

export default {
  emits: ["taskCreated"],
  setup(_, { emit }) {
    const title = ref("");
    const description = ref("");
    const assignedUserId = ref(null);
    const loading = ref(false);

    const createTask = () => {
      loading.value = true;
      axios.post("https://localhost:7129/api/task", {
        title: title.value,
        description: description.value,
        status: "New",
        assignedUserId: assignedUserId.value || null,
      })
      .then(() => {
        emit("taskCreated");
        title.value = "";
        description.value = "";
        assignedUserId.value = null;
        loading.value = false;
      })
      .catch(error => {
        console.error("Error creating task", error);
        loading.value = false;
      });
    };

    return { title, description, assignedUserId, loading, createTask };
  }
};
</script>

<style scoped>
form {
  display: flex;
  flex-direction: column;
  gap: 10px;
  max-width: 300px;
}
</style>
