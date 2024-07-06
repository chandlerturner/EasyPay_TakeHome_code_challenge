# ProductApi

## Overview

ProductApi is a simple CRUD REST API for managing products, built using ASP.NET Core Minimal APIs. 
The API allows you to create, read, update, and delete products with basic caching functionality. 
This implementation focuses on a clean and minimal approach, using Dapper ORM for data access.

## Features

- CRUD operations for products
  - Create a new product
  - Retrieve all products
  - Retrieve a single product by ID
  - Update an existing product
  - Delete a product
- In-memory caching for product retrieval
- Simple and clean architecture
- Minimal APIs for simplicity and efficiency

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server

### Setup

1. Clone the repository.

1. Configure the connection string in `appsettings.json`.

1. Run the application.

### API Endpoints

- **GET /products**: Retrieve all products
- **GET /products/\{id}**: Retrieve a product by ID
- **POST /products**: Create a new product
- **PUT /products/\{id}**: Update an existing product
- **DELETE /products/\{id}**: Delete a product

## Implementation Details

### Money Value Object

The `Money` class encapsulates the amount and currency of the product price. This implementation follows the Money pattern to handle monetary values more effectively.

### Caching

In-memory caching is implemented to improve the performance of product retrieval. The `ProductService` class handles the caching logic.

### Data Access

Dapper ORM is used for data access, providing a simple and efficient way to interact with the SQL Server database.

## Notes for Reviewer

Due to time constraints, some aspects of the implementation are simplified:

- I used Minimal APIs for simplicity.
- I wanted to implement the Money pattern with the value type I created, but that would require separating database entities from domain models, which is the correct approach. However, I focused on getting a working prototype.
- I used `KeyNotFoundException` to indicate a 404 Not Found instead of implementing a domain exception to clearly indicate missing expected data.
- Creating a new product returns the ID created by using `SCOPE_IDENTITY` from SQL Server and then queries for the created value. This could be done more efficiently.
- Adding pagination to the products query is not implemented.
- I would create request objects for command calls to improve code organization and clarity.

## Future Improvements

- Implement proper domain exceptions to handle specific error cases.
- Separate database entities from domain models to follow best practices.
- Optimize the product creation process to avoid additional queries.
- Add pagination and caching support for product queries.
- Create request objects for command calls to improve code structure and readability.
