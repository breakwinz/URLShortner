# URLShortner
A URL Shortening Service with a ASP.NET Core MVC frontend

Requirements:
.NET 5 https://dotnet.microsoft.com/download

Dapper https://www.nuget.org/packages/Dapper/

SQL Server Express LocalDB https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver15

**Installation:**

1: Open .sln solution file with visual studio

2: If required, restore all nuget packages for solution

3: Run the publish profile under URLShortnerDB to publish your database to MSSQLLocalDB

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Optionally, connect to your own database with same table set up, simply change your connectionstring in appsettings.json in the URLShortnerMVC project

