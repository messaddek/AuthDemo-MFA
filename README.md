# AuthDemo

A .NET web application demonstrating secure authentication implementation using cookie-based authentication, password encryption, and Multi-Factor Authentication (MFA).

## Features

- User Registration and Login
- Secure Password Hashing using BCrypt
- Two-Factor Authentication (2FA)
  - TOTP (Time-based One-Time Password) support
  - QR code generation for authenticator apps
  - MFA enablement/verification flow
- Cookie-based Authentication
- In-memory User Repository (easily swappable with SQLite)
- Responsive UI using Bootstrap
- Protected Routes

## Prerequisites

- .NET SDK (9.0 or later)
- Visual Studio 2024 or VS Code
- Git
- Google Authenticator or similar TOTP app

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
│   ├── PasswordHasher.cs
│   ├── IMfaService.cs
│   └── MfaService.cs
├── Repositories/             # Data access
│   ├── IUserRepository.cs
│   └── InMemoryUserRepository.cs
├── Helpers/                  # Utility classes
│   └── QrCodeHelper.cs
└── Pages/                    # Razor Pages
    ├── Account/
    │   ├── Login.cshtml
    │   ├── Register.cshtml
    │   ├── Logout.cshtml
    │   ├── EnableMfa.cshtml
    │   └── VerifyMfa.cshtml
    └── Shared/
        └── _Layout.cshtml
```

## Authentication Flow

1. User registers with username and password
2. Password is hashed using BCrypt before storage
3. User logs in with credentials
4. If MFA is enabled:
   - User is prompted to enter TOTP code
   - Code is verified against stored secret
   - Upon successful verification, authentication cookie is created
5. If MFA is not enabled:
   - Authentication cookie is created immediately after password verification
6. Protected routes check for valid authentication cookie
7. Logout clears the authentication cookie

## MFA Implementation

### Setup Flow

1. User enables MFA from profile settings
2. System generates a secure secret key
3. QR code is displayed for scanning with authenticator app
4. User verifies setup with first TOTP code
5. MFA is enabled upon successful verification

### Verification Flow

1. User logs in with username/password
2. If MFA is enabled, user is prompted for TOTP code
3. System verifies TOTP code against stored secret
4. Access granted upon successful verification

## Security Features

- Password hashing using BCrypt
- TOTP-based Two-Factor Authentication
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

### Required Packages for MFA

```xml
<PackageReference Include="QRCoder" Version="1.4.3" />
<PackageReference Include="Otp.NET" Version="1.3.0" />
```

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
