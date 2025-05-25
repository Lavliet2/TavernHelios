Write-Host "Остановка сервисов приложения (app)..."
docker-compose -f docker-compose.app.yml down

Write-Host "Остановка инфраструктуры (infra)..."
docker-compose -f docker-compose.infra.yml down

Write-Host "Всё остановлено!"
