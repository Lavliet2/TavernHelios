# 1️⃣ Сборочный контейнер с Node.js
FROM node:23.6 AS build
# Устанавливаем рабочую директорию
WORKDIR /app
# Копируем package.json и package-lock.json, чтобы кешировать npm install
COPY ./tavernhelios.client/package*.json ./
# Устанавливаем зависимости
RUN npm install --frozen-lockfile
# Копируем весь проект (после установки зависимостей)
COPY ./tavernhelios.client ./
# Передаём переменные окружения для Vite
ARG VITE_API_URL
ENV VITE_API_URL=${VITE_API_URL}
ARG NODE_ENV
ENV NODE_ENV=${NODE_ENV}
# Показываем переменные для отладки
RUN echo "VITE_API_URL=$VITE_API_URL" && \
    echo "NODE_ENV=$NODE_ENV"
# Собираем проект
RUN npm run build

# Финальный контейнер с Nginx
FROM nginx:latest AS frontend
# Копируем собранный фронт
COPY --from=build /app/dist /usr/share/nginx/html
# Копируем конфиг Nginx для SPA-маршрутов
COPY ./nginx/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
