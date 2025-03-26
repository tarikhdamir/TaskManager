<template>
  <div class="modal-overlay" @click.self="$emit('close')">
    <div class="modal">
      <h2>Create Task</h2>
      <form @submit.prevent="createTask">
        <label>Title:</label>
        <input v-model="title" required />

        <label>Description:</label>
        <input v-model="description" required />

        <label>Assigned User ID:</label>
        <input type="number" v-model="assignedUserId" />

        <div class="modal-buttons">
          <button type="submit" :disabled="loading">
            {{ loading ? "Creating..." : "Create Task" }}
          </button>
          <button class="cancel-button" type="button" @click="$emit('close')">Cancel</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
import { ref } from "vue";
import axios from "axios";

export default {
  emits: ["close", "taskCreated"],
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
        emit("close");
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
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
}

.modal {
  background: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  width: 300px;
}

form {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.modal-buttons {
  display: flex;
  justify-content: space-between;
  margin-top: 10px;
}

button {
  padding: 8px 12px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

button[type="submit"] {
  background-color: #007bff;
  color: white;
}

button[type="submit"]:hover {
  background-color: #0056b3;
}

.cancel-button {
  background-color: #dc3545;
  color: white;
}

.cancel-button:hover {
  background-color: #c82333;
}
</style>
