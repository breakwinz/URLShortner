# URLShortner
A URL Shortening Service with a ASP.NET Core MVC frontend

Requirements:
.NET 5 https://dotnet.microsoft.com/download

Dapper https://www.nuget.org/packages/Dapper/

SQL Server Express LocalDB https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver15

Installation:
Open .sln solution file with visual studio

Restore all nuget packages for solution

Run the publish profile under URLShortnerDB to publish your database to MSSQLLocalDB



Optionally, connect to your own database with same table set up, simply change your connectionstring in appsettings.json in the URLShortnerMVC project

