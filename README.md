# TaskManager API

A task management system built with ASP.NET Core, Entity Framework Core, and MediatR.

## 📌 Features
- Task CRUD (Create, Read, Update, Delete)
- Soft Delete (Mark as deleted instead of removing from DB)
- Status Management: `Новая`, `В работе`, `Выполнена`
- User Assignment
- Filter Tasks by Status, Assignee, and Date

## 🚀 Installation & Setup

### **1️⃣ Clone the Repository**
```sh
git clone https://github.com/yourusername/TaskManager.git
cd TaskManager
```
### **2️⃣ Install Dependencies**
```sh
dotnet restore
```
### **3️⃣ Apply Migrations**
```sh
dotnet ef database update
```
### **4️⃣ Run the Project**
```sh
dotnet run --project API
```
The API will be available at https://localhost:5001.

## 📖 API Endpoints

### ✅ Create a Task
```http
POST /api/task
```
Request Body:

```JSON
{
  "title": "Документация",
  "description": "Написать README",
  "status": "В работе",
  "assignedUserId": 2
}
```
### ✅ Get All Tasks
```http
GET /api/task
```
### ✅ Update Task Status
```http
PATCH /api/task/{taskId}/status
```
Request Body:

```JSON
{
  "taskId": 1,
  "status": "Выполнена"
}
```
### ✅ Soft Delete Task
```http
DELETE /api/task/{taskId}
```
## 🛠 Technologies Used
- **ASP.NET Core 8** - Web framework for building APIs.
- **Entity Framework Core** - ORM for database management.
- **SQLite** - Lightweight relational database.
- **MediatR** - Implements CQRS and simplifies request handling.
- **xUnit & Moq** - Used for unit testing.


📌 License
Distributed under the Unlicense License. See ```LICENSE.txt``` for more information.