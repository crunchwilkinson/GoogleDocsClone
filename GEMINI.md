# GoogleDocsClone Project Context & Guidelines

## Project Context
* **App Type:** Web-based document editing system (ASP.NET Core MVC).
* **Current State:** The application allows for single-user document creation, editing, and management. It does not yet include real-time collaborative features.

## Tech Stack
* **Framework:** .NET 10 (C#)
* **Frontend:** Razor Pages/Views (`.cshtml`), Bootstrap 5, Vanilla JS.
* **Rich Text Editor:** TinyMCE (integrated via CDN).
* **Database & ORM:** PostgreSQL using Entity Framework Core (`Npgsql.EntityFrameworkCore.PostgreSQL`).
* **Authentication:** ASP.NET Core Identity.
* **Deployment:** Azure Web Apps via GitHub Actions.

## Architecture & Coding Rules
* **Project Structure:** Maintain the existing nested structure (Solution folder containing the Project folder). Do not flatten the directory structure.
* **Service Pattern:** Business logic and database calls for documents should be routed through the `IDocumentsService` interface and its implementation (`DocumentsService.cs`). Controllers (like `DocumentsController.cs`) should not interact directly with the `ApplicationDbContext`.
* **Security:** All document operations must verify the `UserId` to ensure users can only access or modify their own documents.
* **Frontend Scripts:** Page-specific JavaScript should be placed in the `@section Scripts` block within the Razor views.