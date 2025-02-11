

docker network inspect app_network
docker network rm app_network
docker network create --driver bridge --attachable app_network
docker network connect app_network TavernHelios.Server_2 #имя центрального сервера
# и перезагрузите контейнер


docker down
docker-compose down
docker-compose up --build -d

docker exec -it TavernHelios.Server sh


docker build -t heliostavern-frontend .
docker run -d -p 63049:80 --name TavernHelios.Client heliostavern-frontend

docker build -t heliostavern-backend .
docker build -t heliostavern-backend:latest -f TavernHelios.Server/Dockerfile D:\Projects\TavernHelios
docker run -d -p 8080:8080 --name heliostavern-backend heliostavern-backend:latest







# Kubernetis
ssh - p 22 lavliet@ 192.168.0.194
kubectl delete deployment tavernhelios-server
kubectl delete service tavernhelios-server-service
kubectl delete ingress tavernhelios-ingress

cd /home/lavliet/heliostavern-app/
docker build -t ghcr.io/lavliet2/heliostavern-backend:latest .



kubectl apply -f /home/lavliet/heliostavern-app/.k8s/tavernhelios-server-deployment.yaml
kubectl apply -f /home/lavliet/heliostavern-app/.k8s/tavernhelios-ingress.yaml

kubectl get pods
kubectl describe pod tavernhelios-server-84575cf97-6kwbx
kubectl get services
kubectl get ingress
kubectl logs tavernhelios-server-6b6cd58df9-zqlmm
kubectl get svc


kubectl logs -n ingress-nginx ingress-nginx-controller-tavernhelios-server-6b6cd58df9-zqlmm
kubectl logs -n ingress-nginx tavernhelios-server-6b6cd58df9-zqlmm


kubectl exec -it tavernhelios-server-6b6cd58df9-zqlmm -- /bin/bash
kubectl cp tavernhelios-server-6b6cd58df9-zqlmm:/var/www/html/config/config.php ./config.php

tavernhelios-server-6b67747d4f-lzsjp


kubectl get pods
kubectl exec -it tavernhelios-server-84575cf97-6kwbx -- env | grep VITE_API_URL


