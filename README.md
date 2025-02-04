# Unosquare CoE .NET Technical Assessment
This repository hosts the projects used to evaluate members of the Unosquare CoE .NET


## Versions

- .NET 8
    - Microsoft.EntityFrameworkCore.Sqlite 8.0.4
    - Serilog.AspNetCore 8.0.1
- Angular 18.1.0

## How to Build

- Frontend

``` 
pushd code/frontend/ta-web
npm install
npm start
```

- Backend

``` 
pushd code/backend/
dotnet clean
dotnet build
dotnet run --project TA-API/TA-API.csproj
``` 