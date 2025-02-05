# Используем базовый образ для ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
RUN apt update && apt install -y curl
USER $APP_UID
EXPOSE 8080
EXPOSE 8081

# 1️⃣ Строим бэкенд
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

# 2️⃣ Собираем фронтенд перед публикацией
FROM node:19 AS frontend-build
WORKDIR /app

# Копируем package.json и устанавливаем зависимости
COPY package*.json ./
RUN npm install

# 🔹 Пробрасываем `VITE_API_URL` внутрь фронта
ARG VITE_API_URL
ENV VITE_API_URL=${VITE_API_URL}

# Копируем весь фронтенд
COPY . .

# 🛠 Собираем фронт (переменная вшивается в код!)
RUN npm run build

# 3️⃣ Публикуем .NET-бэкенд и фронт
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TavernHelios.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 4️⃣ Финальный контейнер с фронтом и бэком
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=frontend-build /app/dist /app/wwwroot  # Добавляем фронтенд в wwwroot

ENTRYPOINT ["dotnet", "TavernHelios.Server.dll"]
