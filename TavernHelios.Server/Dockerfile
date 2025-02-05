# Этап 1: Сборка приложения с помощью Node.js
FROM node:19 AS frontend-build

WORKDIR /app

# Копируем package.json и устанавливаем зависимости
COPY package*.json ./
RUN npm install

# Устанавливаем API URL перед билдом
ARG VITE_API_URL
ENV VITE_API_URL=${VITE_API_URL}
COPY . .

# Строим проект (встраиваем `VITE_API_URL`)
RUN npm run build

# Этап 2: Объединяем фронт и бэк
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
RUN apt update && apt install -y curl
USER $APP_UID
EXPOSE 8080
EXPOSE 8081

# Этап 3: Сборка бекенда
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TavernHelios.Server/TavernHelios.Server.csproj", "TavernHelios.Server/"]
COPY ["Common/TavernHelios.GrpcCommon/TavernHelios.GrpcCommon.csproj", "Common/TavernHelios.GrpcCommon/"]
COPY ["tavernhelios.client/tavernhelios.client.esproj", "tavernhelios.client/"]
RUN dotnet restore "./TavernHelios.Server/TavernHelios.Server.csproj"
COPY . .
WORKDIR "/src/TavernHelios.Server"
RUN dotnet build "./TavernHelios.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этап 4: Публикация бекенда
FROM backend-build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TavernHelios.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Финальный этап: Объединяем бекенд и фронт
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=frontend-build /app/dist /app/wwwroot
ENTRYPOINT ["dotnet", "TavernHelios.Server.dll"]
