### если сеть не создана 
# docker network create app_network
# ./docker-compos-up.ps1

Write-Host "Запуск инфраструктуры + сервисов приложения..."
docker-compose -f docker-compose.infra.yml -f docker-compose.app.yml up --build -d
Write-Host "Всё готово!"
