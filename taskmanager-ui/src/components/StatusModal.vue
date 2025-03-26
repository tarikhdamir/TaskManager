<template>
  <div class="modal-overlay" @click.self="$emit('close')">
    <div class="modal">
      <h2>Change Task Status</h2>
      <select v-model="newStatus">
        <option value="New">New</option>
        <option value="In Progress">In Progress</option>
        <option value="Completed">Completed</option>
      </select>

      <div class="modal-buttons">
        <button @click="updateStatus">Update</button>
        <button class="cancel-button" @click="$emit('close')">Cancel</button>
      </div>
    </div>
  </div>
</template>

<script>
import { ref } from 'vue';
import axios from 'axios';

export default {
  props: ['task'],
  emits: ['close', 'statusUpdated'],
  setup(props, { emit }) {
    const newStatus = ref(props.task.status);

    const updateStatus = () => {
        axios.patch(`https://localhost:7129/api/task/${props.task.id}/status`, {
            taskId: props.task.id,
            status: newStatus.value
        })

        .then(() => {
          emit('statusUpdated');
          emit('close');
        })
        .catch(error => console.error('Error updating status', error));
    };

    return { newStatus, updateStatus };
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
.modal {
  background: white;
  padding: 20px;
  border-radius: 8px;
  width: 300px;
}
</style>
