﻿<!-- PROJECT SHIELDS -->

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
    <img src="https://github.com/vladperchi/InmoIT/blob/master/docs/Images/InmoIT_Banner.png" alt="InmoIT">
  </a>
    <br />
    <a href="https://github.com/vladperchi/InmoIT/issues">Report Bug</a>
    ·
    <a href="https://github.com/vladperchi/InmoIT/issues">Request Feature</a>
   <br />
 <br />
</p>

## About The Project

In reality, there was no real need to implement microservices. InmoIT is intended to help a large Real Estate company provide information on properties in the United States. For this, a well-designed monolithic application would also work without any inconvenience, clearly taking into account that the API and the user interface would be separated to offer better opportunities in the future (Clients).

The API, ASP.NET Core 5.0 was my obvious choice. The WebAPI application is focused on modularity to improve the development experience. Entering the subject, I divided the application into logical modules such as flow, Identity, Documents, Leases, Sales, etc. Each of these modules contains its own `controllers / interfaces / dbContext`. As for the database providers, mssql will be used as default `appsettings`. A module cannot communicate directly with another module or modify its table. CrossCutting concerns would use `interfaces / events`. And yes, domain events are also included in the project using mediatr Handler. Each of the modules follows a clean architecture design.

## Modular Architecture

Modular Architecture is a software design in which a monolith is made better and modular with the importance of reusing components / modules. The same implemented in InmoIT helps to be extended to support and operate with n-modules.

### Diagrammatic Representation

<br />
<p align="center">
    <img src="https://github.com/vladperchi/InmoIT/blob/master/docs/Images/Digramatic_Modular_Architecture.png" alt="Modular Architecture">
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
- DB Providers - MSSQL SERVER

## Features & Plus

- [x] Modular Architecture
- [x] NET 5.0
- [x] Service-Based
- [x] Controller Registration
- [x] Entity Framework Core - Code First
- [x] Auto DB Migrations
- [x] Seeding (Database, Identity)
- [x] Logging
- [x] CQRS using MediatR at API Level
- [x] Validation
- [x] In-Memory Database
- [x] AutoMapper
- [x] Custom API Response
- [x] Custom Errors
- [x] Localization
- [x] Middlewares
- [x] Paginated API Responses
- [x] Specification Pattern
- [x] Default User & Role Seeding
- [x] Registration
- [x] Refresh Tokens
- [x] Claims-Based Authorization
- [x] JWT Authentication
- [x] HTTP Interceptor
- [x] CRUD Operations
- [x] Email Service
- [x] EventLogs
- [x] Swagger
- [x] API Versioning
- [ ] Docker Support
- [ ] Document
- [x] File Upload
- [x] Export Excel
- [x] Hangfire
- [x] Message Service - Twilio Api
- [ ] Dashboard Updates

## Project Structure

- Take a look at the [structure][structure-url] of the solution

## Project Status

- API - `In Progress`
- Docker - `In Progress`

## Getting Started

Inmo IT is currently under development.

Clone this repository to your local machine.

## Prerequisites to run API

1. Install the latest [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
2. Install the latest DOTNET & EF CLI Tools by using this command `dotnet tool install --global dotnet-ef`
3. Install the latest version of Visual Studio IDE 2019, preferably Visual Studio IDE 2022 (v17.0.0 and above) OR Visual Studio Code
4. It's recommended to use MsSql Server Database as it comes by default with InmoIT.
5. As for quick DB Management, me love [Azure Data Studio](https://docs.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio?view=sql-server-ver15)

## Running the API

1. Open up `InmoIT.sln` in Visual Studio 2019, preferably VS2022.
2. Navigate to appSettings.json under `src/Host/Api/appsettings.json`
3. Add you MsSql connection string under `PersistenceSettings`. The default connection string:
   `"mssql": "Data Source=.;Initial Catalog=InmoIT;Integrated Security=True;MultipleActiveResultSets=True"`
4. That is all you need to configure the API. Just create and run the API project.
5. By default, the database is migrated. Take a look at the [migrations][migrations-url] of the solution
6. Some default data is also included in this database, such as roles, users, owners, properties, images etc.
7. Browse to https://localhost:5000/ to Api InmoIT!

## Docker in Windows

- Install Docker on Windows via `https://docs.docker.com/docker-for-windows/install/`
- Open up Powershell on Windows and run the following
  - `cd c:\`
  - `dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p securePassword123`
  - `dotnet dev-certs https --trust`
  - Note - Make sure that you use the same password that has been configured in the `docker-compose.yml` file. By default, `securePassword123` is configured.
- 5005 is the port setup to run InmoIT on Docker, so make sure that these port is free. You could also change the port in the
  - `docker-compose.yml` and `Server\Dockerfile` files.
- Now navigate back to the root of the InmoIT Project on your local machine and run the following via terminal
  - `docker-compose -f 'docker-compose.yml' up --build`
- This will start pulling MSSQL Server Image from Docker Hub if you don't already have this image.
- That's almost everything. Once the containers are available, migrations are updated in the MSSQL DB, default data is seeded.
- Browse to https://localhost:5005/ to use your version of InmoIT!

## Core Developer Contact

### Vladimir P. CHibás

- Twitter - [codewithvlad][twitter-url]
- Linkedin - [Vladimir][linkedin-url]
- GitHub - [vladperchi][github-url]

## License

This project is licensed with the [MIT License][license-url].

[structure-url]: https://github.com/vladperchi/InmoIT/blob/master/docs/Md/api-project-structure.md
[migrations-url]: https://github.com/vladperchi/InmoIT/blob/master/docs/Md/api-migrations-guide.md
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
