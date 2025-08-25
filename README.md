Task Manager Application Requirements and Design
Application Requirements
1.	User Management:
•	Users should be able to create an account, log in, and log out.
•	Each user should have a profile with basic information.
2.	Task Management:
•	Users should be able to create, read, update, and delete tasks.
•	Each task should have a title, description, status (e.g., pending, in progress, completed), and due date.
•	Tasks should be associated with the user who created them.
3.	Notifications:
•	Users should receive notifications for upcoming due dates and status changes.
4.	Security:
•	Ensure secure authentication and authorization.
•	Protect against common security threats like SQL injection and cross-site scripting (XSS).
Application Design
1. Database Design:
•	Users Table: Stores user information.
•	Tasks Table: Stores task information.
•	Notifications Table: Stores notification information.
2. API Design:
•	Authentication API: Handles user registration, login, and logout.
•	Tasks API: Handles CRUD operations for tasks.
•	Notifications API: Handles notifications for tasks.
3. Architecture:
•	Backend: C# Web API with ADO.NET and stored procedures for database interactions.
•	Frontend: (Optional) A web or mobile application to interact with the API.
•	Database: SQL Server for storing user and task data.
4. Security Measures:
•	Use HTTPS for secure communication.
•	Implement token-based authentication (e.g., JWT).
•	Validate and sanitize user inputs to prevent SQL injection and XSS.
API Endpoints
•	Authentication API:
•	POST /api/auth/register: Register a new user.
•	POST /api/auth/login: Log in a user.
•	POST /api/auth/logout: Log out a user.
•	User API:
•	GET /api/users: Get a list of all users (admin only).
•	GET /api/users/{id}: Get details of a specific user.
•	PUT /api/users/{id}: Update user information.
•	DELETE /api/users/{id}: Delete a user (admin only).
•	Tasks API:
•	GET /api/tasks: Get all tasks for the logged-in user.
•	POST /api/tasks: Create a new task.
•	PUT /api/tasks/{id}: Update an existing task.
•	DELETE /api/tasks/{id}: Delete a task.
•	Notifications API:
•	GET /api/notifications: Get notifications for the logged-in user.

