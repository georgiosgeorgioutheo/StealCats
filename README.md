# StealCats Project

This document provides step-by-step instructions to build and run the StealCats application, including setting up the database.

## Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server or SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or any preferred IDE

## Getting Started

### 1. Clone the Repository

First, clone the repository to your local machine:

```bash
git clone https://github.com/yourusername/StealCats.git
cd StealCats 

```

### 2. Configure the Connection String
Open the appsettings.json file in the StealCats project and configure your SQL Server connection string as follows:
```bash
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=CatDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "StealCatApi": {
    "ApiKey": "your_api_key",
    "BaseUrl": "https://api.thecatapi.com/v1/images/search"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

Replace YOUR_SERVER_NAME with your SQL Server instance name.<br>
Replace the your_api_key with your API key from The Cat API.<br>

### 3. Initialize the Database
 Add the initial migration
```bash
dotnet ef migrations add InitialCreate -p Infrastructure -s StealCats
```
Update the database with the migration
```bash
dotnet ef database update -p Infrastructure -s StealCats
```

### 4. Build the Project
#### Restore Dependencies<br>
```bash
dotnet restore
```
#### Build the Solution


```bash
dotnet build
```
### 5. Running the Application
#### Run the Application
To run the application, use the following command:

```bash
Copy code
dotnet run
```
The application will start, and you can access it via:

#### HTTP: http://localhost:5157
#### HTTPS: https://localhost:7187
#### Swagger UI: https://localhost:7187/swagger or http://localhost:5157/swagger

### 6. Using the API
#### Fetch and Store Cat Images
To fetch and store cat images from The Cat API:


```bash
POST /api/cats/steal
```

Get Cats with Pagination
To retrieve cats with pagination:


```bash
GET /api/cats?page=1&pageSize=10
```

Get Cat by ID
To retrieve a specific cat by its ID:

```bash
GET /api/cats/{id}
```
Get Cat Image by ID

To retrieve a cat's image by its ID:
```bash
GET /api/cats/{id}/image
```
