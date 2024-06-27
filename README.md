# Task and Stages Management Project with Kanban View in C# with Razor

This project is a web application developed in C# and Razor, which implements a CRUD system (Create, Read, Update, Delete) to manage tasks and their respective stages. The system is secured with sessions to ensure user data privacy and security. Additionally, it offers an intuitive Kanban view, allowing tasks to be easily moved between different stages.

## Features

- **Authentication and Authorization:**
  - Secure login system using sessions.
  - Route protection to ensure only authenticated users can access and modify data.

- **Task Management:**
  - Creation of new tasks with specific details.
  - Editing of existing tasks.
  - Deletion of tasks that are no longer needed.
  - Viewing of all registered tasks.

- **Stages Management:**
  - Definition of multiple stages for tasks (e.g., To Do, In Progress, Done).
  - Customization of stage names and order as needed.

- **Kanban View:**
  - Visual Kanban-style interface for task management.
  - Drag and drop functionality to move tasks between different stages.
  - Dynamic state updates of tasks when moved between columns.

## Technologies Used

- **Programming Language:**
  - C#

- **Framework:**
  - ASP.NET Core with Razor Pages

- **Database:**
  - SQL Server (or any compatible database)

- **Security:**
  - Session management for authentication and authorization.
