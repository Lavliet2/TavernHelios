# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# 🛠️ Обновляем пути, указывая новую вложенность
COPY ["WeatherService/TavernHelios.WeatherServiceServer/TavernHelios.WeatherServiceServer.csproj", "WeatherService/TavernHelios.WeatherServiceServer/"]
COPY ["WeatherService/TavernHelios.WeatherService.APICore/TavernHelios.WeatherService.APICore.csproj", "WeatherService/TavernHelios.WeatherService.APICore/"]
COPY ["Common/TavernHelios.GrpcCommon/TavernHelios.GrpcCommon.csproj", "Common/TavernHelios.GrpcCommon/"]

RUN dotnet restore "WeatherService/TavernHelios.WeatherServiceServer/TavernHelios.WeatherServiceServer.csproj"

# Копируем все исходники
COPY . .

WORKDIR "/src/WeatherService/TavernHelios.WeatherServiceServer"
RUN dotnet build "TavernHelios.WeatherServiceServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TavernHelios.WeatherServiceServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TavernHelios.WeatherServiceServer.dll"]
