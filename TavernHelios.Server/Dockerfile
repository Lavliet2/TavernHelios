# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
#RUN curl -fsSL https://deb.nodesource.com/setup_23.x | bash - && \
    #apt-get install -y nodejs && \
    #npm install -g npm
WORKDIR /src
COPY ["TavernHelios.Server/TavernHelios.Server.csproj", "TavernHelios.Server/"]
COPY ["Common/TavernHelios.GrpcCommon/TavernHelios.GrpcCommon.csproj", "Common/TavernHelios.GrpcCommon/"]
COPY ["MenuService/APICore/TavernHelios.MenuService.APICore.csproj", "MenuService/APICore/"]
COPY ["ReservationService/TavernHelios.ReservationService.APICore/TavernHelios.ReservationService.APICore.csproj", "ReservationService/TavernHelios.ReservationService.APICore/"]
RUN dotnet restore "./TavernHelios.Server/TavernHelios.Server.csproj"
COPY . .
WORKDIR "/src/TavernHelios.Server"
RUN dotnet build "./TavernHelios.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TavernHelios.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TavernHelios.Server.dll"]