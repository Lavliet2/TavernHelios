docker build -t heliostavern-frontend .
docker run -d -p 63049:80 --name TavernHelios.Client heliostavern-frontend

docker build -t heliostavern-backend .
docker build -t heliostavern-backend:latest -f TavernHelios.Server/Dockerfile D:\Projects\TavernHelios
docker run -d -p 8080:8080 --name heliostavern-backend heliostavern-backend:latest



docker down