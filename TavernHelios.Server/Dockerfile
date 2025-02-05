# 1️⃣ База с ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
RUN apt update && apt install -y curl
USER $APP_UID
EXPOSE 8080
EXPOSE 8081

# 2️⃣ Сборка .NET-бэкенда
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TavernHelios.Server/TavernHelios.Server.csproj", "TavernHelios.Server/"]
COPY ["Common/TavernHelios.GrpcCommon/TavernHelios.GrpcCommon.csproj", "Common/TavernHelios.GrpcCommon/"]
COPY ["tavernhelios.client/tavernhelios.client.esproj", "tavernhelios.client/"]
RUN dotnet restore "./TavernHelios.Server/TavernHelios.Server.csproj"
COPY . .
WORKDIR "/src/TavernHelios.Server"
RUN dotnet build "./TavernHelios.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 3️⃣ Сборка Фронтенда
FROM node:19 AS frontend-build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

# 4️⃣ Публикация .NET и фронта
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TavernHelios.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 5️⃣ Финальный контейнер
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=frontend-build /app/dist /app/wwwroot  # Копируем фронтенд в wwwroot

# 🟢 Вставляем скрипт для динамической настройки API-адреса
RUN echo "window.VITE_API_URL='${VITE_API_URL}';" > /app/wwwroot/env-config.js

ENTRYPOINT ["dotnet", "TavernHelios.Server.dll"]
