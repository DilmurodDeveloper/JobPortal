# JobPortal API

**JobPortal** is a backend service built with ASP.NET Core that allows users to search for jobs, manage their profiles, and enables employers to post job listings. It follows clean architecture principles and is designed for scalability, testability, and maintainability.

## 🛠 Technologies Used

- ASP.NET Core 8
- Entity Framework Core
- SQL Server
- xUnit (for unit testing)
- GitHub Actions (CI/CD)
## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server or any compatible relational database
- Visual Studio 2022+ or Visual Studio Code

### Clone the repository
  ```
    git clone https://github.com/DilmurodDeveloper/JobPortal.git
    cd JobPortal
  ```
## 🗄️ Set Up the Database
1. Update the connection string in `JobPortal.Api/appsettings.json`:
  ```
    "ConnectionStrings": {
    "DefaultConnection": "your-sql-server-connection-string"
    } 
  ```
2. Run EF Core migrations (if applicable):
  ```
    dotnet ef database update --project JobPortal.Api
  ```
## ▶️ Run the Application
  ```
    dotnet run --project JobPortal.Api
  ```
The API will be available at:
- `https://localhost:7067`
- `http://localhost:5128`
  
(depending on your launch settings).

## ✅ Running Tests
  ```
    dotnet test JobPortal.Api.Tests.Unit
  ```

## ⚙️ CI/CD with GitHub Actions
### GitHub Actions is configured to:
- Restore dependencies
- Build the solution
- Run unit tests
It triggers on push and pull request events on the master/main branch.

## 📄 License

This project is licensed under the [MIT License](LICENSE). See the LICENSE file for details.

## 👤 Author

**[DilmurodDeveloper](https://github.com/DilmurodDeveloper)**
