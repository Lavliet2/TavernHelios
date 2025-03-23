# (1) Сборка фронта
FROM node:23.6 AS build
WORKDIR /app

COPY ./tavernhelios.client/package*.json ./
RUN npm install --frozen-lockfile

COPY ./tavernhelios.client ./
ARG VITE_API_URL
ENV VITE_API_URL=${VITE_API_URL}
ARG NODE_ENV
ENV NODE_ENV=${NODE_ENV}
RUN npm run build

# (2) Финальный образ
FROM nginx:latest as frontend
# Копируем статические файлы
COPY --from=build /app/dist /usr/share/nginx/html
# Копируем production-конфиг (где listen 80, без SSL)
COPY ./nginx/nginx.conf.prod /etc/nginx/nginx.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
