# Book Management API

## 📌 Overview

The **Book Management API** is a RESTful service built with ASP.NET Core that allows users to manage books and users. It includes authentication, authorization, and role-based access control (RBAC) using IdentityServer.

## 🛠 Tech Stack

- **ASP.NET Core 8**
- **Entity Framework Core** (MySQL, SQL Server)
- **IdentityServer** for authentication
- **Swagger** for API documentation
- **N-tier architecture**
- **Docker & Docker Compose**

## 🚀 Features

- User authentication and authorization
- Role-based access control (**Admin, User**)
- CRUD operations for books
- Bulk operations for books
- User management (Admin only)
- API documentation with Swagger
- Dockerized deployment with **Docker Compose**

## 🐜 Getting Started

### Prerequisites

- .NET SDK 8.0+
- SQL Server (configured in `appsettings.json/appsettings.Development.json`)
- Docker & Docker Compose
- Visual Studio or VS Code

### Installation & Setup

1. **Clone the repository**

   ```sh
   git clone https://github.com/makhammadsoliyev/BookManagement.git
   cd BookManagement
   ```

2. **Install dependencies**

   ```sh
   dotnet restore
   ```

3. **Configure database** (update `appsettings.json/appsettings.Development.json` with your database connection string)

4. **Apply migrations**

   ```sh
   dotnet ef database update
   ```

5. **Run the application**

   ```sh
   dotnet run
   ```

## 🐳 Running with Docker Compose

1. **Build and start the services**

   ```sh
   docker-compose up --build
   ```

2. **Stop the services**

   ```sh
   docker-compose down
   ```

3. **Check running containers**

   ```sh
   docker ps
   ```

## 🔒 Authentication & Authorization

- Uses **IdentityServer** for authentication.
- JWT tokens required for accessing protected endpoints.
- Roles: `Admin`, `User`.

### Register a New User

```http
POST /api/auth/register
```

```json
{
  "username": "testuser",
  "email": "test@example.com",
  "password": "SecurePassword123!"
}
```

### Login

```http
POST /api/auth/login
```

```json
{
  "email": "test@example.com",
  "password": "SecurePassword123!"
}
```

This returns a JWT token to be used in `Authorization: Bearer <token>` header.

## 📚 API Endpoints

### Books

| Method   | Endpoint             | Description               | Role        |
| -------- | -------------------- | ------------------------- | ----------- |
| `POST`   | `/api/books`         | Add a new book            | User, Admin |
| `POST`   | `/api/books/bulk`    | Add multiple books        | User, Admin |
| `GET`    | `/api/books/{id}`    | Get book by ID            | User, Admin |
| `GET`    | `/api/books/{title}` | Get book by title         | User, Admin |
| `GET`    | `/api/books`         | Get paginated book titles | User, Admin |
| `PUT`    | `/api/books/{id}`    | Update a book             | User, Admin |
| `DELETE` | `/api/books/{id}`    | Soft delete a book        | User, Admin |
| `DELETE` | `/api/books/bulk`    | Bulk delete books         | User, Admin |

### Users (Admin Only)

| Method | Endpoint          | Description    |
| ------ | ----------------- | -------------- |
| `GET`  | `/api/users/{id}` | Get user by ID |
| `GET`  | `/api/users`      | Get all users  |

## 📝 API Documentation

- Run the project and navigate to:
  ```
  https://localhost:18081/swagger/index.html
  ```
- The Swagger UI will show all available endpoints with request and response examples.

## 📝 License

This project is licensed under the **MIT License**.

---

Feel free to reach out if you have any questions!

## 👤 Contact

- **LinkedIn:** [makhammadsoliyev](https://www.linkedin.com/in/makhammadsoliyev)
- **Telegram:** [@home3538](https://t.me/hope3538)
- **Email:** makhammadsoliyev@gmail.com
