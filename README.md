# ⚙️ E-Commerce API Backend (.NET Core)

![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/c%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQLServer-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-black?style=for-the-badge&logo=JSON%20web%20tokens)

This repository contains the Backend API for a Full-Stack E-Commerce platform. It is a RESTful API built with **.NET** and **C#**, using **Entity Framework Core** for database management and **SQL Server**.

> **Note:** This is the Backend API. The Frontend (built with Angular 17+) can be found here: [ecommerce-frontend-angular](https://github.com/TBBLuxari/ecommerce-frontend-angular)

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
  
<img width="1920" height="1080" alt="Sin usuario" src="https://github.com/user-attachments/assets/b9f0730f-1801-4c0a-8009-615e6b4f623f" />

<img width="1920" height="1080" alt="Create an Account" src="https://github.com/user-attachments/assets/58c7c18f-beaa-4fb1-9501-90dcdb83fa86" />

<img width="1920" height="1080" alt="Correo Confiramcion Compra" src="https://github.com/user-attachments/assets/49c1cb9f-636a-43fb-ac20-406317ffbc4a" />

<img width="1920" height="1080" alt="Correo" src="https://github.com/user-attachments/assets/bd5c5f37-b992-465a-8ead-a1713192847b" />

<img width="1920" height="1080" alt="confirmacion" src="https://github.com/user-attachments/assets/fa7e82c4-9120-4805-be92-c765edb005a6" />

---

## 🔐 Security Highlights

1. **Password Hashing:** `BCrypt.Net` is used to hash and salt passwords before saving them to the SQL database.
2. **Endpoint Protection:** The `[Authorize]` attribute is strictly implemented across controllers, ensuring only authenticated users (and specific roles) can perform sensitive actions like adding products or completing purchases.
3. **Email Validation:** Users cannot authenticate until they click the unique verification link sent to their email, updating their `IsVerified` status in the database.


<img width="1920" height="1080" alt="Product User Car Confirmation" src="https://github.com/user-attachments/assets/d4f69d95-f938-4983-8aba-caecda6376d4" />

<img width="1920" height="1080" alt="Correo Confiramcion Compra" src="https://github.com/user-attachments/assets/9517e59e-be40-4c52-b050-f504bc8eb3a5" />


<img width="1920" height="1080" alt="Correo Confiramcion Compra 2" src="https://github.com/user-attachments/assets/2377019e-d938-45be-9eed-2d8cd403f88d" />

---
## ⚙️ How to run this project locally

1. Clone this repository:
    ```bash
    git clone [https://github.com/TBBLuxari/ecommerce-backend-dotnet.git](https://github.com/TBBLuxari/ecommerce-backend-dotnet.git)
    ```

2. Open the solution in **Visual Studio 2026**.

3. Configure your Database connection:
    * Open `appsettings.json`.
    * Update the `DefaultConnection` string with your local SQL Server instance details.
      <img width="538" height="645" alt="Base" src="https://github.com/user-attachments/assets/f283b01c-9d89-4d23-9d69-0e866f29aa3c" />

    * Update the `EmailSettings` with your SMTP credentials to test the email flow.

4. Run Entity Framework Migrations to create the database:
    * Open the **Package Manager Console** in Visual Studio.
    * Run the command: `Update-Database`

5. Press `F5` or click **"Run"** in Visual Studio to start the API. Swagger will automatically open to explore the endpoints.
  
