# AuthDemo

A .NET web application demonstrating secure authentication implementation using cookie-based authentication and password encryption.

## Features

- User Registration and Login
- Secure Password Hashing using BCrypt
- Cookie-based Authentication
- In-memory User Repository (easily swappable with SQLite)
- Responsive UI using Bootstrap
- Protected Routes

## Prerequisites

- .NET SDK (9.0 or later)
- Visual Studio 2024 or VS Code
- Git

## Getting Started

1. Clone the repository
```bash
git clone <your-repository-url>
cd AuthDemo
```

2. Install dependencies
```bash
dotnet restore
```

3. Run the application
```bash
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`

## Project Structure

```
AuthDemo/
├── Program.cs                 # Application entry point
├── Models/                    # Data models
│   └── User.cs
├── Services/                  # Business logic
│   ├── IPasswordHasher.cs
│   └── PasswordHasher.cs
├── Repositories/              # Data access
│   ├── IUserRepository.cs
│   └── InMemoryUserRepository.cs
└── Pages/                     # Razor Pages
    ├── Account/
    │   ├── Login.cshtml
    │   ├── Register.cshtml
    │   └── Logout.cshtml
    └── Shared/
        └── _Layout.cshtml
```

## Authentication Flow

1. User registers with username and password
2. Password is hashed using BCrypt before storage
3. User logs in with credentials
4. Upon successful login, authentication cookie is created
5. Protected routes check for valid authentication cookie
6. Logout clears the authentication cookie

## Security Features

- Password hashing using BCrypt
- CSRF protection
- Secure cookie handling
- Input validation
- Authentication middleware
- XSS protection

## Development

### Adding SQLite Support

To switch from in-memory storage to SQLite:

1. Add NuGet package:
```bash
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```

2. Implement `SqliteUserRepository`
3. Update dependency injection in `Program.cs`

### Running Tests

```bash
dotnet test
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.
