<template>
    <div class="task-list">
        <div class="header">
            <h2>Task List</h2>
            <button class="create-task-button" @click="showModal = true">+ Create Task</button>
        </div>

        <!-- Filters -->
        <div class="filters">
            <label for="statusFilter">Status:</label>
            <select v-model="statusFilter" @change="fetchTasks">
                <option value="">All</option>
                <option value="New">New</option>
                <option value="In Progress">In Progress</option>
                <option value="Completed">Completed</option>
            </select>

            <label for="assigneeFilter">Assigned User ID:</label>
            <input type="number" v-model.number="assigneeFilter" @input="fetchTasks" placeholder="Enter User ID" />

            <label for="dateFilter">Created After:</label>
            <input type="date" v-model="dateFilter" @change="fetchTasks" />
        </div>

        <!-- Task Table -->
        <table>
            <thead>
                <tr>
                    <th>Title</th>
                    <th>Description</th>
                    <th>Status</th>
                    <th>Assigned User</th>
                    <th>Created Date</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="task in filteredTasks" :key="task.id">
                    <td>{{ task.title }}</td>
                    <td>{{ task.description }}</td>
                    <td><span :class="getStatusClass(task.status)">{{ task.status }}</span></td>
                    <td>{{ task.assignedUserId || "Unassigned" }}</td>
                    <td>{{ formatDate(task.createdAt) }}</td>
                    <td class="actions">
                        <button class="edit-button" @click="editTask(task)">✏️</button>
                        <button class="delete-button" @click="confirmDelete(task.id)">🗑</button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <!-- Modals -->
    <TaskModal v-if="showModal" @close="showModal = false" @taskCreated="fetchTasks" />
    <EditTaskModal v-if="editModalTask" :task="editModalTask" @close="editModalTask = null" @taskUpdated="fetchTasks" />
</template>

<script>
    import axios from "axios";
    import { ref, computed, onMounted } from "vue";
    import TaskModal from "./TaskModal.vue";
    import EditTaskModal from "./EditTaskModal.vue";

    export default {
        components: { TaskModal, EditTaskModal },
        setup() {
            const tasks = ref([]);
            const showModal = ref(false);
            const editModalTask = ref(null);
            const statusFilter = ref("");
            const assigneeFilter = ref(null);
            const dateFilter = ref("");

            const fetchTasks = () => {
                axios.get("https://localhost:7129/api/task")
                    .then(response => (tasks.value = response.data))
                    .catch(error => console.error("Error loading tasks", error));
            };

            const filteredTasks = computed(() => {
                return tasks.value.filter(task => {
                    const matchesStatus = statusFilter.value ? task.status === statusFilter.value : true;
                    const matchesAssignee = assigneeFilter.value ? task.assignedUserId == assigneeFilter.value : true;
                    const matchesDate = dateFilter.value ? new Date(task.createdAt) >= new Date(dateFilter.value) : true;
                    return matchesStatus && matchesAssignee && matchesDate;
                });
            });

            const editTask = (task) => {
                editModalTask.value = task;
            };

            const confirmDelete = (taskId) => {
                if (confirm("Are you sure you want to delete this task?")) {
                    deleteTask(taskId);
                }
            };

            const deleteTask = (taskId) => {
                axios.delete(`https://localhost:7129/api/task/${taskId}`)
                    .then(() => fetchTasks())
                    .catch(error => console.error("Error deleting task", error));
            };

            const getStatusClass = (status) => ({
                "status-new": status === "New",
                "status-inprogress": status === "In Progress",
                "status-completed": status === "Completed",
            });

            const formatDate = (dateString) => {
                return new Date(dateString).toLocaleDateString();
            };

            onMounted(fetchTasks);

            return { tasks, showModal, editModalTask, statusFilter, assigneeFilter, dateFilter, filteredTasks, fetchTasks, editTask, confirmDelete, getStatusClass, formatDate };
        }
    };
</script>

<style scoped>
    .task-list {
        max-width: 800px;
        margin: auto;
        background: white;
        padding: 20px;
        border-radius: 8px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    }

    .header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 20px;
    }

    .filters {
        display: flex;
        align-items: center;
        gap: 10px;
        margin-bottom: 15px;
    }

        .filters label {
            font-weight: bold;
        }

        .filters select, .filters input {
            padding: 5px;
            border-radius: 4px;
            border: 1px solid #ccc;
        }

    .create-task-button {
        background-color: #28a745;
        color: white;
        padding: 8px 12px;
        border: none;
        border-radius: 4px;
        cursor: pointer;
    }

        .create-task-button:hover {
            background-color: #218838;
        }

    .edit-button {
        background-color: #007bff;
        color: white;
        padding: 5px 8px;
        border: none;
        border-radius: 4px;
        cursor: pointer;
    }

        .edit-button:hover {
            background-color: #0056b3;
        }

    .delete-button {
        background-color: #dc3545;
        color: white;
        padding: 5px 8px;
        border: none;
        border-radius: 4px;
        cursor: pointer;
        margin-left: 5px;
    }

        .delete-button:hover {
            background-color: #c82333;
        }

    table {
        width: 100%;
        border-collapse: collapse;
    }

    th, td {
        border: 1px solid #ddd;
        padding: 10px;
        text-align: left;
    }

    th {
        background-color: #f4f4f4;
    }

    .status-new {
        color: blue;
        font-weight: bold;
    }

    .status-inprogress {
        color: orange;
        font-weight: bold;
    }

    .status-completed {
        color: green;
        font-weight: bold;
    }

    .actions {
        display: flex;
        gap: 5px;
    }
</style>
