**Task Manager Application Requirements and Design**<br><br>
**Application Requirements**<br><br>
**1.	User Management:**<br>
•	Users should be able to create an account, log in, and log out.<br>
•	Each user should have a profile with basic information.<br><br>
**2.	Task Management:**<br>
•	Users should be able to create, read, update, and delete tasks.<br>
•	Each task should have a title, description, status (e.g., pending, in progress, completed), and due date.<br>
•	Tasks should be associated with the user who created them.<br><br>
**3.	Notifications:**<br>
•	Users should receive notifications for upcoming due dates and status changes.<br><br>
**4.	Security:**<br>
•	Ensure secure authentication and authorization.<br>
•	Protect against common security threats like SQL injection and cross-site scripting (XSS).
Application Design<br><br>
**1. Database Design:**<br>
•	Users Table: Stores user information.<br>
•	Tasks Table: Stores task information.<br>
•	Notifications Table: Stores notification information.<br><br>
**2. API Design:**<br>
•	Authentication API: Handles user registration, login, and logout.<br>
•	Tasks API: Handles CRUD operations for tasks.<br>
•	Notifications API: Handles notifications for tasks.<br><br>
**3. Architecture:**<br>
•	Backend: C# Web API with ADO.NET and stored procedures for database interactions.<br>
•	Frontend: (Optional) A web or mobile application to interact with the API.<br>
•	Database: SQL Server for storing user and task data.<br><br>
**4. Security Measures:**<br>
•	Use HTTPS for secure communication.<br>
•	Implement token-based authentication (e.g., JWT).<br>
•	Validate and sanitize user inputs to prevent SQL injection and XSS.<br><br>
**API Endpoints**<br>
**•	Authentication API:**<br>
**•	POST** /api/auth/register: Register a new user.<br>
**•	POST** /api/auth/login: Log in a user.<br>
**•	POST** /api/auth/logout: Log out a user.<br><br>
**•	User** API:<br>
**•	GET** /api/users: Get a list of all users (admin only).<br>
**•	GET**/api/users/{id}: Get details of a specific user.<br>
**•	PUT**/api/users/{id}: Update user information.<br>
**•	DELETE**/api/users/{id}: Delete a user (admin only).<br><br>
**•	Tasks API:**<br>
**•	GET** /api/tasks: Get all tasks for the logged-in user.<br>
**•	POST** /api/tasks: Create a new task.
**•	PUT** /api/tasks/{id}: Update an existing task.<br>
**•	DELETE**/api/tasks/{id}: Delete a task.<br><br>
**•	Notifications API:<br>**
**•	GET** /api/notifications: Get notifications for the logged-in user.

