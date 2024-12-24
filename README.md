# Community Groups

## Overview
Community Groups is a cross-platform project built with **ASP.NET Core 9**.

## Purpose and Use Cases
This project offers RESTful services to support the creation and management of communities and their members. Originally developed as part of an interview exercise, it has recently undergone modernization efforts and is still under active refactoring to improve its structure and align with current best practices.

Key features include:

### Person Management
- **Create** a person entry with the following details:
  - First Name
  - Last Name
  - Email (unique)
  - Occupation (optional)
- **Edit** a person entry
- **Delete** a person entry
- **Bulk Create**: Import a CSV file to create multiple person entries
- **Search, Order, and Paginate** person entries

### Community Group Management
- **Create** a community group with a name
- **Edit** the name of a community group
- **Delete** a community group
- **Assign** users to a community group
- **Remove** users from a community group
- **Retrieve** a community group with its members

### User Authentication and Authorization
- **User Registration and Login** with email and password
- All people and community groups are scoped to the logged-in user

## API Documentation
Explore the API endpoints via Swagger:
- **Swagger URL**: [https://localhost:44308/swagger/index.html](https://localhost:44308/swagger/index.html)

### Authentication
Authentication is required for accessing most endpoints. Use the following credentials for demo purposes:
- **Login Path**: `/api/v1/Login/login`
- **Username**: `crea`
- **Password**: `crea`

After logging in, include the generated JWT token in the **Authorize** section of Swagger to access the services.

## Security
- **JWT Authentication**: Ensures secure access to endpoints.
- Tokens must be included in requests after login.

## Architecture
The project adheres to modern software engineering practices, ensuring scalability and maintainability.

- **Framework**: Built on .NET Core 9
- **Authentication**: JWT-based authentication
- **Data Access**: Implements the Generic Repository Pattern
- **Error Handling**: Centralized exception handling using Middleware
- **Database**: Entity Framework Code-First approach
  - Commands: Use `add-migration` and `update-database` to synchronize the database

## License
[![MIT License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

This project is licensed under the MIT License, which allows you to freely use, modify, and distribute the code. See the [`LICENSE`](LICENSE) file for full details.
