FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
ENV ASPNETCORE_URLS=https://+:5005;http://+:5006
WORKDIR /app
EXPOSE 5005
EXPOSE 5006

ARG buildname
ARG builddate
ARG buildversion

ENV name=${buildname}
ENV date=${builddate}
ENV build=${buildversion}

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /
COPY ["editorconfig", "src/server/"]
COPY ["Directory.Build.props", "src/server/"]
COPY ["inmoit.ruleset", "src/server/"]
COPY ["stylecop.json", "src/server/"]
COPY ["Host/Api/Api.csproj", "src/server/Host/Api/"]
COPY ["Shared/Shared.Dtos/Shared.Dtos.csproj", "src/server/Shared/Shared.Dtos/"]
COPY ["Shared/Shared.Core/Shared.Core.csproj", "src/server/Shared/Shared.Core/"]
COPY ["Shared/Shared.Infrastructure/Shared.Infrastructure.csproj", "src/server/Shared/Shared.Infrastructure/"]
COPY ["Modules/Identity/Modules.Identity.Core/Modules.Identity.Core.csproj", "src/server/Modules/Identity/Modules.Identity.Core/"]
COPY ["Modules/Identity/Modules.Identity.Infrastructure/Modules.Identity.Infrastructure.csproj", "src/server/Modules/Identity/Modules.Identity.Infrastructure/"]
COPY ["Modules/Identity/Modules.Identity.Api/Modules.Identity.Api.csproj", "src/server/Modules/Identity/Modules.Identity.Api/"]
COPY ["Modules/Inmo/Modules.Inmo.Core/Modules.Inmo.Core.csproj", "src/server/Modules/Inmo/Modules.Inmo.Core/"]
COPY ["Modules/Inmo/Modules.Inmo.Infrastructure/Modules.Inmo.Infrastructure.csproj", "src/server/Modules/Inmo/Modules.Inmo.Infrastructure/"]
COPY ["Modules/Inmo/Modules.Inmo.Api/Modules.Inmo.Api.csproj", "src/server/Modules/Inmo/Modules.Inmo.Api/"]
COPY ["Modules/Person/Modules.Person.Core/Modules.Person.Core.csproj", "src/server/Modules/Person/Modules.Person.Core/"]
COPY ["Modules/Person/Modules.Person.Infrastructure/Modules.Person.Infrastructure.csproj", "src/server/Modules/Person/Modules.Person.Infrastructure/"]
COPY ["Modules/Person/Modules.Person.Api/Modules.Person.Api.csproj", "src/server/Modules/Person/Modules.Person.Api/"]
COPY ["Modules/Document/Modules.Document.Core/Modules.Document.Core.csproj", "src/server/Modules/Document/Modules.Document.Core/"]
COPY ["Modules/Document/Modules.Document.Infrastructure/Modules.Document.Infrastructure.csproj", "src/server/Modules/Document/Modules.Document.Infrastructure/"]
COPY ["Modules/Document/Modules.Document.Api/Modules.Document.Api.csproj", "src/server/Modules/Document/Modules.Document.Api/"]
COPY ["Modules/Operation/Modules.Operation.Core/Modules.Operation.Core.csproj", "src/server/Modules/Operation/Modules.Operation.Core/"]
COPY ["Modules/Operation/Modules.Operation.Infrastructure/Modules.Operation.Infrastructure.csproj", "src/server/Modules/Operation/Modules.Operation.Infrastructure/"]
COPY ["Modules/Operation/Modules.Operation.Api/Modules.Operation.Api.csproj", "src/server/Modules/Operation/Modules.Operation.Api/"]
COPY ["Modules/Accounting/Modules.Accounting.Core/Modules.Accounting.Core.csproj", "src/server/Modules/Accounting/Modules.Accounting.Core/"]
COPY ["Modules/Accounting/Modules.Accounting.Infrastructure/Modules.Accounting.Infrastructure.csproj", "src/server/Modules/Accounting/Modules.Accounting.Infrastructure/"]
COPY ["Modules/Accounting/Modules.Accounting.Api/Modules.Accounting.Api.csproj", "src/server/Modules/Accounting/Modules.Accounting.Api/"]
RUN dotnet restore "src/server/Host/Api/Api.csproj" --disable-parallel

COPY . .
WORKDIR "src/server/Host/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
WORKDIR /app/Files
WORKDIR /app

RUN echo "Build Name: $name"
RUN echo "Build Date: $date"
RUN echo "Build Version: $build"

ENTRYPOINT ["dotnet", "InmoIT.Api.dll"]