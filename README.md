# Taply

Taply is a mobile-first QR information page MVP built with ASP.NET Core 8 MVC, EF Core, SQL Server, Bootstrap 5 and vanilla JavaScript.

## Run locally

1. Install .NET 8 SDK and SQL Server LocalDB (or use a SQL Server instance).
2. Set the SQL connection string in appsettings.json or with the ConnectionStrings__TaplyDb environment variable.
3. Run dotnet restore and dotnet run.
4. Visit /admin/login.

The first run creates the database schema and seeds an example place, page and sections. Demo credentials: admin@taply.vn / Taply123!. Change these before exposing an instance to the internet.

## Routes

- /admin/login and /admin/places: owner editor
- /admin/places/{id}: pages at a place
- /admin/pages/{id}/edit: page sections and QR
- /p/{slug}: public information page
- /p/{slug}/{sectionSlug}: section details
- /qr/{slug}: QR PNG encoding only the stable public URL

Images are validated by extension and size (5 MB) and stored in wwwroot/uploads/{userId}. EF Core migration InitialCreate is included and is applied automatically at startup.

Bootstrap and Bootstrap Icons are loaded from jsDelivr.
