<!-- PROJECT SHIELDS -->

[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

<!-- PROJECT LOGO -->

<br />
<p align="center">
  <a href="https://github.com/vladperchi/InmoIT">
    <img src="https://github.com/vladperchi/InmoIT/blob/master/docs/Evidence/inmoitBanner.jpg" alt="InmoIT">
  </a>
  <h3 align="center">InmoIT - WebApi Modular Architecture</h3>
  <p align="center">Open Sourced Solution built ASP.NET Core 5.0 WebApi
    <br />
    <br />
    <a href="https://github.com/vladperchi/InmoIT/issues">Report Bug</a>
    ·
    <a href="https://github.com/vladperchi/InmoIT/issues">Request Feature</a>
  </p>
</p>

## About The Project

In reality, there was no real need to implement microservices. InmoIT is intended to help a large Real Estate company provide information on properties in the United States. For this, a well-designed monolithic application would also work without any inconvenience, clearly taking into account that the API and the user interface would be separated to offer better opportunities in the future (Clients).

The API, ASP.NET Core 5.0 was my obvious choice. The WebAPI application is focused on modularity to improve the development experience. Entering the subject, I divided the application into logical modules such as flow, Identity, Documents, Leases, Sales, etc. Each of these modules contains its own controllers / interfaces / dbContext. As for the database providers, mssql will be used as default, as optional would be postgres / mysql `appsettings`. A module cannot communicate directly with another module or modify its table. CrossCutting concerns would use interfaces / events. And yes, domain events are also included in the project using mediatr Handler. Each of the modules follows a clean architecture design.

## Modular Architecture

Modular Architecture is a software design in which a monolith is made better and modular with the importance of reusing components / modules. The same implemented in InmoIT helps to be extended to support and operate with n-modules.

### Digramatic Representation

<br />
<p align="center">
    <img src="https://github.com/vladperchi/InmoIT/blob/master/docs/Evidence/Digramatic_Modular_Architecture.png" alt="Modular Architecture">
</p>

### PRO

- Clear Separation of Concerns
- Easily Scalable
- Lower complexity compared to Microservices
- Low operational / deployment costs.
- Reusability
- Organized Dependencies

### CONTRA

- Not Multi-technology compatible.
- Horizontal Scaling can be a concern. But this can be managed via load balancers.
- Since Interprocess Communication is used, messages may be lost during Application Termination. Microservices combat this issue by using external messaging brokers like Kafka, RabbitMQ.
- We can make use of message agents but no, let's keep it simple.

## Technology Stack

- API - ASP.NET Core 5.0 WebAPI
- Data Access - [Entity Framework Core 5.0](https://docs.microsoft.com/en-us/ef/core/)
- DB Providers - MSSQL SERVER (Optional Postgres / MySql)

## Features

- [x] Modular Architecture
- [x] Controller Registration
- [x] Entity Framework Core - Code First
- [x] Service-Based
- [x] Repository Pattern - Generic
- [x] CQRS using MediatR Library
- [x] MediatR Logging & Validation
- [x] Serilog
- [x] In-Memory Database
- [x] Database Seeding
- [x] Identity Seeding
- [x] Migrations
- [x] CRUD Functionalities
- [x] AutoMapper
- [x] Swagger-UI-Documentation
- [x] API Versioning
- [x] JWT Authentication
- [x] Validation Errors
- [x] Localization
- [x] Middlewares
- [x] Dynamic Service Registration
- [x] Paginated API Responses
- [x] Default User & Role Seeding
- [x] Registration (Only Admin register new users)
- [x] User Auditing
- [ ] Document
- [ ] PDF Downloads
- [ ] File Upload
- [ ] Export to Excel
- [ ] Dashboard Updates Realtime
- [ ] Chat - Integrated with Identity

## Project Structure

- src
  - Host
    - Api
  - Modules
    - Flow
      - Controllers
      - Core
        - Entities
        - Interfaces
        - Exceptions
        - CQRS
          - Handlers
          - Commands
          - Queries
      - Infrastructure
        - Context
        - Migrations
    - n-Module
  - Shared
    - Core
      - Interfaces
    - Dtos
    - Infrastructure
      - Middlewares
      - Persistence Registrations
- tests
  - FunctionalTests
    - ControllerApis
  - IntegrationTests
    - Data
  - UnitTests
    - Core

## Project Status

- API - `In Progress`
- Docker Container - `Coming Soon!`

## Getting Started

InmoIT is in it's early development stage.

Clone this repository to your local machine.

### Prerequisites to run API

1. Install the latest [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
2. Install the latest DOTNET & EF CLI Tools by using this command `dotnet tool install --global dotnet-ef`
3. Install the latest version of Visual Studio IDE 2019, preferably Visual Studio IDE 2022 (v17.0.0 and above) OR Visual Studio Code
4. It's recommended to use MsSql Server Database as it comes by default with InmoIT.
5. As for quick DB Management, me love [Azure Data Studio](https://docs.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio?view=sql-server-ver15)

## Running the API

1. Open up `InmoIT.sln` in Visual Studio 2019, preferably VS2022.
2. Navigate to appSettings.json under `src/Host/Api/appsettings.json`
3. Add you MsSql connection string under `PersistenceSettings`. The default connection string is `"mssql": "Data Source=.;Initial Catalog=InmoITDB;Integrated Security=True;MultipleActiveResultSets=True"`
4. That is all you need to configure the API. Just create and run the API project.
5. By default, the database is migratedand ready for use.
6. Some default data is also included in this database, such as roles, users, owners, properties, images etc.

## Core Developer Contact

### Vladimir P. CHibás

- Twitter - [codewithvlad][twitter-url]
- Linkedin - [Vladimir][linkedin-url]
- GitHub - [vladperchi][github-url]

## License

This project is licensed with the [MIT License][license-url].

[build-shield]: https://img.shields.io/endpoint.svg?url=https%3A%2F%2Factions-badge.atrox.dev%2Fvladperchi%2FInmoIT%2Fbadge&style=flat-square
[build-url]: https://github.com/vladperchi/InmoIT/actions
[contributors-shield]: https://img.shields.io/github/contributors/vladperchi/InmoIT.svg?style=flat-square
[contributors-url]: https://github.com/vladperchi/InmoIT/graphs/contributors
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
[blog-url]: https://www.codewithvladperchi.com
[facebook-url]: https://www.facebook.com/codewithvladperchi
[twitter-url]: https://www.twitter.com/codewithvlad
