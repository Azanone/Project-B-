# BabylonMarkt

BabylonMarkt is a .NET 8 console application for a supermarket-style store flow. It includes customer login and registration, guest shopping, product browsing, cart and wishlist actions, checkout and payment, receipts, reviews, and admin management for products, users, finance, and stock.

## Features

- Login, register, or continue as guest
- Product browsing and product details
- Wishlist and shopping cart management
- Checkout and payment method selection
- Receipt generation and purchase history
- Product reviews and product popularity sorting
- Admin product management
- Admin user management
- Financial overview and stock handling
- Email activation and password security checks

## Tech Stack

- C# / .NET 8
- SQLite
- Dapper
- Microsoft.Data.Sqlite
- Otp.NET
- QRCoder

## Project Structure

- `Project/Program.cs` - application entry point
- `Project/Presentation/` - console screens and menus
- `Project/Logic/` - business logic layer
- `Project/DataAccess/` - database access layer and connection handling
- `Project/DataModels/` - domain models used across the app
- `Project/DataSources/project.db` - SQLite database file

## Requirements

- .NET 8 SDK
- No extra services are required for the core app; it uses the local SQLite database included in `Project/DataSources/`

## Environment Variables

The application uses SMTP settings for email verification during registration. Set these environment variables before running the app if you want email activation enabled:

- `SMTP_FROM` - Sender email address used in verification emails. This is required.
- `SMTP_HOST` - SMTP server host. If this is not set and `SMTP_FROM` ends with `@gmail.com`, the app falls back to `smtp.gmail.com`.
- `SMTP_PORT` - SMTP server port. Default: `587`.
- `SMTP_USER` - SMTP username. If this is not set, the app uses `SMTP_FROM`.
- `SMTP_PASSWORD` - SMTP password or app password.

If the SMTP settings are not configured, email verification will not be available.

## Run the Project

1. Open a terminal in the repository root.
2. Restore packages if needed:

   ```bash
   dotnet restore Project/Project.csproj
   ```

3. Run the application:

   ```bash
   dotnet run --project Project/Project.csproj
   ```

The app starts in the main menu, where you can log in, register, or continue as a guest.

## Database

The application reads from `Project/DataSources/project.db`. The database file is copied to the build output and is used by the connection layer at runtime.

## Notes

- The project uses a layered structure with presentation, logic, and data access separated.
- Admin functionality includes managing products, users, stock, and financial views.
- Customer functionality includes browsing products, shopping cart actions, wishlist, reviews, and checkout.