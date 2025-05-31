# NewSqlMinimalApi

A minimal ASP.NET Core 8.0 Web API demonstrating CRUD operations using a NewSQL database (CockroachDB).  
Includes Docker support and GitHub Actions CI/CD pipeline for build, test, and Docker image push.

---

## Features

- ASP.NET Core Minimal API with CRUD endpoints for `Product` entity
- Uses **CockroachDB** (NewSQL) as the backend database
- Entity Framework Core with PostgreSQL provider (`Npgsql`)
- Docker multi-stage build for containerization
- GitHub Actions workflow for CI: build, test, and push Docker image

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://docs.docker.com/get-docker/)
- GitHub account with secrets configured for CI/CD
- (Optional) CockroachDB installed locally or run via Docker

---

## Getting Started

### Clone the repository

```bash
git clone https://github.com/yourusername/NewSqlMinimalApi.git
cd NewSqlMinimalApi
```

## Configure Connection String

The application uses a connection string named `CockroachDb` to connect to the CockroachDB database.

You can configure the connection string in **two ways**:

### 1. Using `appsettings.json`

Edit the `appsettings.json` file in the `NewSqlMinimalApi` project folder and update the connection string as follows:

```json
{
  "ConnectionStrings": {
    "CockroachDb": "Host=localhost;Port=26257;Database=appdb;Username=root;Password=;SSL Mode=Disable"
  }
}
```

### 2. Using Environment Variables (Recommended for Production)

You can override the connection string by setting the environment variable:

```bash
ConnectionStrings__CockroachDb="Host=your_host;Port=26257;Database=appdb;Username=root;Password=;SSL Mode=Disable"
```
## Run the API locally

After configuring the connection string, you can run the API locally by executing:

```bash
dotnet restore
dotnet build
dotnet run --project NewSqlMinimalApi
```

