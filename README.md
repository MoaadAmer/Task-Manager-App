Application Requirements
User Management:
Users should be able to create an account, log in, and log out.
Each user should have a profile with basic information.
Task Management:
Users should be able to create, read, update, and delete tasks.
Each task should have a title, description, status (e.g., pending, in progress, completed), and due date.
Tasks should be associated with the user who created them.
Notifications:
Users should receive notifications for upcoming due dates and status changes.
Security:
Ensure secure authentication and authorization.
Protect against common security threats like SQL injection and cross-site scripting (XSS).
Application Design
1. Database Design:
Users Table: Stores user information.
Tasks Table: Stores task information.
Notifications Table: Stores notification information.
2. API Design:
Authentication API: Handles user registration, login, and logout.
Tasks API: Handles CRUD operations for tasks.
Notifications API: Handles notifications for tasks.
3. Architecture:
Backend: C# Web API with ADO.NET and stored procedures for database interactions.
Frontend: (Optional) A web or mobile application to interact with the API.
Database: SQL Server for storing user and task data.
4. Security Measures:
Use HTTPS for secure communication.
Implement token-based authentication (e.g., JWT).
Validate and sanitize user inputs to prevent SQL injection and XSS.
