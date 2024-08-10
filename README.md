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
}```
