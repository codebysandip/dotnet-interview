# Getting Started
This project using latest version of dotnet core 6.0. Please install dotnet core sdk version 6.0 before running.

## How to Run This project
To run this project just need to follow two simple following steps:
- Run command `dotnet restore` This command will install packages refrenced in csproj file
- Run command  `dotnet run` This command will run project.

Note: To run in watch mode run command `dotnet watch run`

### Postman Collection
[Collection link](https://www.postman.com/sandip12081992/workspace/public-apis/collection/3086995-99b4dc95-6ebd-401c-8732-10ea416e5eb4)

## Architecture

This project is designed to transform the controller response into a custom response structure, as specified in the [ApiResponse.cs](https://github.com/codebysandip/dotnet-interview/blob/main/Models/ApiResponse.cs) file. This enables the frontend to receive a standardized structure from the backend. Additionally, this project consistently sends a response status of 200 along with the actual response code in the response body. This simplifies the frontend code for response handling, resulting in cleaner code.

Furthermore, this project adheres to the repository pattern, which promotes loose coupling between the controller and business layer. This architectural approach enhances maintainability and flexibility.

For demonstration purposes, this project utilizes an in-memory SQLite database with Entity Framework as the Object-Relational Mapping (ORM) tool.