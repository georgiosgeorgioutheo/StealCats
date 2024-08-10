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

### 2.Configure the Connection String
Open the appsettings.json file in the StealCats project and configure your SQL Server connection string as follows:

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
Replace YOUR_SERVER_NAME with your SQL Server instance name.
Update the ApiKey with your API key from The Cat API.
Initialize the Database
To create the initial database schema, open a terminal in the StealCats project directory and run the following commands:
Build the Project
Restore Dependencies
Before building the project, restore the dependencies:

bash
Copy code
dotnet restore
Build the Solution
Now, build the project:

bash
Copy code
dotnet build
4. Running the Application
Run the Application
To run the application, use the following command:

bash
Copy code
dotnet run
The application will start, and you can access it via:

API: https://localhost:5001 (or http://localhost:5000)
Swagger UI: https://localhost:5001/swagger (or http://localhost:5000/swagger)
5. Using the API
Fetch and Store Cat Images
To fetch and store cat images from The Cat API:

http
Copy code
POST /api/cats/steal
Get Cats with Pagination
To retrieve cats with pagination:

http
Copy code
GET /api/cats?page=1&pageSize=10
Get Cat by ID
To retrieve a specific cat by its ID:

http
Copy code
GET /api/cats/{id}
Get Cat Image by ID
To retrieve a cat's image by its ID:

http
Copy code
GET /api/cats/{id}/image
