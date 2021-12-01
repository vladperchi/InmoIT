<!--[![Build Status][Build-shield]][Build-url]-->

[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

<!--[![MIT License][license-shield]][license-url]-->

<p align="center">
  <a href="https://github.com/vladperchi/InmoIT">
    <img src="https://raw.githubusercontent.com/vladperchi/InmoIT/master/docs/Evidence/inmoitBanner.jpg?token=ACCOXKYZ3OIBLN4XCCS4RW3BU5T6E" alt="inmoit">
  </a>
  <h3 align="center">InmoIT - Open Source Api Management Solution</h3>
  <p align="center">
    Built with ASP.NET Core 5.0 WebAPI
  </p>
</p>

### About InmoIT

In reality, there was no real need to implement microservices. InmoIT is intended to help a large Real Estate company provide information on properties in the United States. For this, a well-designed monolithic application would also work without any inconvenience, clearly taking into account that the API and the user interface would be separated to offer better opportunities in the future (Clients).

The API, ASP.NET Core 5.0 was my obvious choice. The WebAPI application is focused on modularity to improve the development experience. Entering the subject, I divided the application into logical modules such as flow, Identity, Documents, Contracts, etc. Each of these modules contains its own controllers / interfaces / dbContext. As for the database providers, mssql will be used as default, as optional would be postgres / mysql `appsettings`. A module cannot communicate directly with another module or modify its table. CrossCutting concerns would use interfaces / events. And yes, domain events are also included in the project using mediatr Handler. Each of the modules follows a clean architecture design.

The modular monolith architecture implemented in InmoIT helps to be extended to support other business modules such as warehouses, warehouses, premises, etc.

### Technology Stack

- API - ASP.NET Core 5.0 WebAPI
- Data Access - [Entity Framework Core 5.0](https://docs.microsoft.com/en-us/ef/core/)
- DB Providers - MSSQL (Optional Postgres / MySql)

### Project Status

- API - `In Progress`
- Docker Container - `Coming Soon!`

### Getting Started

InmoIT is in it's early development stage.

Clone this repository to your local machine.

#### Prerequisites to run API

1. Install the latest [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
2. Install the latest DOTNET & EF CLI Tools by using this command `dotnet tool install --global dotnet-ef`
3. Install the latest version of Visual Studio IDE 2019, preferably Visual Studio IDE 2022 (v17.0.0 and above) OR Visual Studio Code
4. It's recommended to use MsSql Server Database as it comes by default with InmoIT.
5. As for quick DB Management, me love [Azure Data Studio](https://docs.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio?view=sql-server-ver15)

#### Running the API

1. Open up `InmoIT.sln` in Visual Studio 2019, preferably VS2022.
2. Navigate to appSettings.json under `src/Host/Api/appsettings.json`
3. Add you MsSql connection string under `PersistenceSettings`. The default connection string is `"mssql": "Data Source=.;Initial Catalog=InmoITDB;Integrated Security=True;MultipleActiveResultSets=True"`
4. That is all you need to configure the API. Just create and run the API project.
5. By default, the database is migratedand ready for use.
6. Some default data is also included in this database, such as roles, users, owners, properties, images etc.

### The Core Developer

- Vladimir P. CHibás, en [GitHub][github-url] y en [LinkedIn][linkedin-url]

## License

This project is licensed with the [MIT license][license-url].

[build-shield]: https://img.shields.io/endpoint.svg?url=https%3A%2F%2Factions-badge.atrox.dev%2Fvladperchi%2FInmoIT%2Fbadge&style=flat-square
[build-url]: https://github.com/vladperchi/InmoIT/actions
[forks-shield]: https://img.shields.io/github/forks/vladperchi/InmoIT?style=flat-square
[forks-url]: https://github.com/vladperchi/InmoIT/network/members
[stars-shield]: https://img.shields.io/github/stars/vladperchi/InmoIT.svg?style=flat-square
[stars-url]: https://img.shields.io/github/stars/vladperchi/InmoIT?style=flat-square
[issues-shield]: https://img.shields.io/github/issues/vladperchi/InmoIT?style=flat-square
[issues-url]: https://github.com/vladperchi/InmoIT/issues
[license-shield]: https://img.shields.io/github/license/vladperchi/InmoIT?style=flat-square
[license-url]: https://github.com/vladperchi/InmoIT/blob/master/LICENSE
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=flat-square&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/vladperchi/
[github-url]: https://github.com/vladperchi/
