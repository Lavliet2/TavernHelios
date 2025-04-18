# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER root
WORKDIR /app
#RUN apt update && apt install -y curl
USER $APP_UID
EXPOSE 8080
EXPOSE 8081
EXPOSE 5065

# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ReservationService/TavernHelios.ReservationServiceServer/TavernHelios.ReservationServiceServer.csproj", "ReservationService/TavernHelios.ReservationServiceServer/"]
COPY ["Common/TavernHelios.GrpcCommon/TavernHelios.GrpcCommon.csproj", "Common/TavernHelios.GrpcCommon/"]
COPY ["ReservationService/TavernHelios.ReservationService.APICore/TavernHelios.ReservationService.APICore.csproj", "ReservationService/TavernHelios.ReservationService.APICore/"]
COPY ["ReservationService/TavernHelios.ReservationService.PostgreRepository/TavernHelios.ReservationService.PostgreRepository.csproj", "ReservationService/TavernHelios.ReservationService.PostgreRepository/"]
RUN dotnet restore "./ReservationService/TavernHelios.ReservationServiceServer/TavernHelios.ReservationServiceServer.csproj"
COPY . .
WORKDIR "/src/ReservationService/TavernHelios.ReservationServiceServer"
RUN dotnet build "./TavernHelios.ReservationServiceServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TavernHelios.ReservationServiceServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TavernHelios.ReservationServiceServer.dll"]