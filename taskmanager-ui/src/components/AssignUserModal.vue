<template>
  <div class="modal-overlay" @click.self="$emit('close')">
    <div class="modal">
      <h2>Assign User to Task</h2>
      <input type="number" v-model="userId" placeholder="Enter User ID" />

      <div class="modal-buttons">
        <button @click="assignUser">Assign</button>
        <button class="cancel-button" @click="$emit('close')">Cancel</button>
      </div>
    </div>
  </div>
</template>

<script>
import { ref } from "vue";
import axios from "axios";

export default {
  props: ["task"],
  emits: ["close", "userAssigned"],
  setup(props, { emit }) {
    const userId = ref(null);

    const assignUser = () => {
      axios.patch(`https://localhost:7129/api/task/${props.task.id}/assign`, {
            taskId: props.task.id,
            userId: userId.value
        })
        .then(() => {
          emit("userAssigned");
          emit("close");
        })
        .catch(error => console.error("Error assigning user", error));
    };

    return { userId, assignUser };
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
  width: 300px;
}
</style>
