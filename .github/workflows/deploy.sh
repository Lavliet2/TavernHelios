set -e

DEPLOY_BRANCH=${1:-main}

echo "🚀 Deploying from branch: $DEPLOY_BRANCH"

# Очистка и клонирование репозитория
rm -rf /home/lavliet/heliostavern-app
echo "📥 Клонируем ветку: $DEPLOY_BRANCH"
git clone --branch "$DEPLOY_BRANCH" --depth 1 https://github.com/Lavliet2/TavernHelios.git /home/lavliet/heliostavern-app
cd /home/lavliet/heliostavern-app || exit 1

echo "✅ Репозиторий успешно клонирован. Запускаем деплой..."

# Apply manifests
kubectl apply -f .k8s/tavernhelios-ingress.yaml
kubectl apply -f .k8s/mongoMenuService-deployment.yaml
kubectl apply -f .k8s/mongoLayoutService-deployment.yaml
kubectl apply -f .k8s/postgresReservation-deployment.yaml
kubectl apply -f .k8s/postgresAuth-deployment.yaml
kubectl apply -f .k8s/clickhouse-monitoring-deployment.yaml
kubectl apply -f .k8s/rabbitmq-monitoring-deployment.yaml
kubectl apply -f .k8s/prometheus-monitoring-deployment.yaml
kubectl apply -f .k8s/grafana-monitoring-deployment.yaml
kubectl apply -f .k8s/redis-weather-deployment.yaml

kubectl apply -f .k8s/menuService-deployment.yaml
kubectl apply -f .k8s/reservationService-deployment.yaml
kubectl apply -f .k8s/layoutService-deployment.yaml
kubectl apply -f .k8s/authService-deployment.yaml
kubectl apply -f .k8s/adminService-deployment.yaml
kubectl apply -f .k8s/weatherService-deployment.yaml
kubectl apply -f .k8s/tavernhelios-server-deployment.yaml
kubectl apply -f .k8s/tavernhelios-client-deployment.yaml

echo "🔄 Перезапускаем поды..."
kubectl rollout restart deployment menuservice
kubectl rollout restart deployment reservationservice
kubectl rollout restart deployment layoutservice
kubectl rollout restart deployment authservice
kubectl rollout restart deployment adminservice
kubectl rollout restart deployment weatherservice
kubectl rollout restart deployment tavernhelios-server
kubectl rollout restart deployment tavernhelios-client

echo "✅ Деплой успешно завершён!"
