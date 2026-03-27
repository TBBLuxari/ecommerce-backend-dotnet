# ⚙️ E-Commerce API Backend (.NET Core)

![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/c%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQLServer-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-black?style=for-the-badge&logo=JSON%20web%20tokens)

This repository contains the Backend API for a Full-Stack E-Commerce platform. It is a RESTful API built with **.NET** and **C#**, using **Entity Framework Core** for database management and **SQL Server**.

> **Note:** This is the Backend API. The Frontend (built with Angular 17+) can be found here: [ENLACE_A_TU_REPOSITORIO_FRONTEND_AQUI]

---

## 🎥 Full-Stack Demo Video
*(Coming soon)* [Link to your YouTube/Loom video showing the complete flow].

---

## 🏗️ Architecture & Core Features

This API is designed with a focus on security, scalability, and clean architecture:

* **Authentication & Authorization:** Secure login and registration flow using **JWT (JSON Web Tokens)**. Passwords are never stored in plain text; they are securely hashed using **BCrypt**.
* **Role-Based Access Control (RBAC):** Implementation of User and Admin roles. Admin roles have exclusive access to inventory management endpoints.
* **Email Verification Flow:** Integration with an SMTP service (MailKit) to send automated confirmation emails with unique verification tokens upon user registration.
* **Data Seeding:** Automated database initialization that creates a default Administrator account (`admin@mitienda.com`) if the database is empty, ensuring a plug-and-play experience for testing.
* **Entity Framework Core:** Code-First approach for database modeling and migrations.

---

## 🔐 Security Highlights

1. **Password Hashing:** `BCrypt.Net` is used to hash and salt passwords before saving them to the SQL database.
2. **Endpoint Protection:** The `[Authorize]` attribute is strictly implemented across controllers, ensuring only authenticated users (and specific roles) can perform sensitive actions like adding products or completing purchases.
3. **Email Validation:** Users cannot authenticate until they click the unique verification link sent to their email, updating their `IsVerified` status in the database.

---
## ⚙️ How to run this project locally

## ⚙️ How to run this project locally

1. Clone this repository:
    ```bash
    git clone [https://github.com/TBBLuxari/ecommerce-backend-dotnet.git](https://github.com/TBBLuxari/ecommerce-backend-dotnet.git)
    ```

2. Open the solution in **Visual Studio 2026**.

3. Configure your Database connection:
    * Open `appsettings.json`.
    * Update the `DefaultConnection` string with your local SQL Server instance details.
    * Update the `EmailSettings` with your SMTP credentials to test the email flow.

4. Run Entity Framework Migrations to create the database:
    * Open the **Package Manager Console** in Visual Studio.
    * Run the command: `Update-Database`

5. Press `F5` or click **"Run"** in Visual Studio to start the API. Swagger will automatically open to explore the endpoints.
  
