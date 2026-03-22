# GoogleDocsClone

![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)
![.NET Version](https://img.shields.io/badge/.NET-9.0-blueviolet.svg)
[![CI/CD](https://img.shields.io/badge/Deploy-GitHub_Actions-2088FF?style=flat&logo=github-actions&logoColor=white)](https://github.com/features/actions)
![Database](https://img.shields.io/badge/Database-PostgreSQL-336791.svg?logo=postgresql&logoColor=white)

A centralized, web-based document editing system built with **ASP.NET Core MVC**. This project replicates core functionalities of Google Docs, allowing users to create, edit, and manage documents seamlessly in a web environment.

---

## 🚀 Features

* **Document Management:** Create, view, and organize documents.
* **Web-Based Editor:** Clean UI for editing text and formatting.
* **Database Integration:** Persistent storage for all document data.
* **Automated CI/CD:** Integrated with GitHub Actions for automated testing and deployment.

## 🛠️ Tech Stack

* **Backend:** ASP.NET Core MVC 9.0 (C#)
* **Frontend:** HTML5, CSS3, JavaScript (Razor Pages)
* **Database:** PostgreSQL (Entity Framework Core)
* **DevOps:** GitHub Actions & Azure

## ⚙️ Getting Started
**Prerequisites**
* .NET 9.0 SDK
* PostgreSQL
* Visual Studio 2022 or VS Code

## Installation
Clone the repository:

```bash
git clone [https://github.com/crunchwilkinson/GoogleDocsClone.git](https://github.com/crunchwilkinson/GoogleDocsClone.git)
```

## Configure the Database:
Update the ConnectionStrings in appsettings.json with your PostgreSQL credentials:

JSON
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=GoogleDocsCloneDB;Username=your_user;Password=your_password"
}

## Apply Migrations:

```bash
dotnet ef database update
```

## Run the application:

```bash
dotnet run --project GoogleDocsClone
```

## 🧪 CI/CD Pipeline
This project uses GitHub Actions to ensure code quality. Every push to the main branch triggers:

* **Build:** Verifies that the code compiles correctly.

* **Test:** Runs automated unit tests.

* **Deploy:** Automatically prepares the application for Azure Web Apps.
