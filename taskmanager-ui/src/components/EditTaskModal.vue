<template>
  <div class="modal-overlay" @click.self="$emit('close')">
    <div class="modal">
      <h2>Edit Task</h2>
      <form @submit.prevent="updateTask">
        <label>Title:</label>
        <input v-model="updatedTask.title" required />

        <label>Description:</label>
        <input v-model="updatedTask.description" required />

        <label>Status:</label>
        <select v-model="updatedTask.status">
          <option value="New">New</option>
          <option value="In Progress">In Progress</option>
          <option value="Completed">Completed</option>
        </select>

        <label>Assigned User ID:</label>
        <input type="number" v-model="updatedTask.assignedUserId" />

        <div class="modal-buttons">
          <button type="submit" :disabled="loading">
            {{ loading ? "Updating..." : "Save Changes" }}
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
  props: ["task"],
  emits: ["close", "taskUpdated"],
  setup(props, { emit }) {
    const updatedTask = ref({ ...props.task });
    const loading = ref(false);

    const updateTask = () => {
      loading.value = true;
      axios.put(`https://localhost:7129/api/task/${props.task.id}`, {
            taskId: props.task.id, // ✅ Include taskId explicitly!
            title: updatedTask.value.title,
            description: updatedTask.value.description,
            status: updatedTask.value.status,
            assignedUserId: updatedTask.value.assignedUserId,
        })
        .then(() => {
          emit("taskUpdated");
          emit("close");
          loading.value = false;
        })
        .catch(error => {
          console.error("Error updating task", error);
          loading.value = false;
        });
    };

    return { updatedTask, loading, updateTask };
  }
};
</script>

<style scoped>
/* Full-screen overlay */
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

/* Centered modal */
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
