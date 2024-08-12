# Architecture and Technologies Used in the Project
## The StealCats project is designed in Clean Architecture
### It has the following Layers:

#### Core Layer: Contains  entities, interfaces,DTOs and mappings. This layer is independent of any external frameworks and is at the center of the architecture.
#### Application Layer: Contains Validation Rules and Helper,  Application services that implement business use cases. It interacts with the Core layer and coordinates operations across the system.
#### Infrastructure Layer:Contains the Database Context,The reposittories and is responsible for data persistence, external API communication, and other external concerns. This layer implements interfaces for external APIs from the Core layer.
#### API Layer: This is the entry point for the application, using ASP.NET Core Minimal APIs to expose endpoints to clients.

## Testing
The project includes unit tests for various components, ensuring that the business logic, validation, and data processing work as expected.

## Technologies Used

#### .NET 8 SDK 
.NET 8 is used in this application as the foundational framework for building and running the web API. 

#### SQL Server or SQL Server Express
MS SQL Server is used as the relational database management system for this application.

#### ASP.NET Core Minimal APIs
The project uses ASP.NET Core Minimal APIs for high-performance endpoints.

#### Swagger
Swagger is used in this application to automatically generate interactive API documentation. 

#### Global Exception Handling
A global exception handler is implemented to catch and handle exceptions consistently across the application.

####  Serilog
is used for structured logging throughout the application.

#### FluentValidation
FluentValidation is used for validating request data and entities.

#### Testing
xUnit is used as the testing framework.


