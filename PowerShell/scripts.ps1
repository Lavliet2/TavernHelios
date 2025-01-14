docker build -t heliostavern-frontend .
docker run -d -p 63049:80 --name TavernHelios.Client heliostavern-frontend


docker down