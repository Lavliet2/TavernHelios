# Этап 1: Базовый образ для работы ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Этап 2: Сборка приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# Копируем csproj и восстанавливаем зависимости
COPY ["TavernHelios.Server/TavernHelios.Server.csproj", "TavernHelios.Server/"]
RUN dotnet restore "./TavernHelios.Server/TavernHelios.Server.csproj"

# Копируем все файлы и собираем приложение
COPY . .
WORKDIR "/src/TavernHelios.Server"
RUN dotnet build "TavernHelios.Server.csproj" -c Release -o /app/build

# Этап 3: Публикация приложения
FROM build AS publish
RUN dotnet publish "TavernHelios.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Этап 4: Финальный образ для продакшн
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TavernHelios.Server.dll"]
