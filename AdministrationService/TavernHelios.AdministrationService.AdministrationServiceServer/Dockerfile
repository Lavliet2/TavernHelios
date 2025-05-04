# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AdministrationService/TavernHelios.AdministrationService.AdministrationServiceServer/TavernHelios.AdministrationService.AdministrationServiceServer.csproj", "AdministrationService/TavernHelios.AdministrationService.AdministrationServiceServer/"]
COPY ["AdministrationService/TavernHelios.AdministrationService.RabbitMq/TavernHelios.AdministrationService.RabbitMq.csproj", "AdministrationService/TavernHelios.AdministrationService.RabbitMq/"]
COPY ["TavernHelios.RabbitMq/TavernHelios.RabbitMq.csproj", "TavernHelios.RabbitMq/"]
COPY ["AdministrationService/TavernHelios.AdministrationService.ClickHouse/TavernHelios.AdministrationService.ClickHouse.csproj", "AdministrationService/TavernHelios.AdministrationService.ClickHouse/"]
COPY ["Common/TavernHelios.ClickHouse/TavernHelios.ClickHouse.csproj", "Common/TavernHelios.ClickHouse/"]
RUN dotnet restore "./AdministrationService/TavernHelios.AdministrationService.AdministrationServiceServer/TavernHelios.AdministrationService.AdministrationServiceServer.csproj"
COPY . .
WORKDIR "/src/AdministrationService/TavernHelios.AdministrationService.AdministrationServiceServer"
RUN dotnet build "./TavernHelios.AdministrationService.AdministrationServiceServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TavernHelios.AdministrationService.AdministrationServiceServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TavernHelios.AdministrationService.AdministrationServiceServer.dll"]