# Используем Node.js для сборки фронта
FROM node:23.6 AS build

# Устанавливаем рабочую директорию
WORKDIR /app

# Копируем package.json и устанавливаем зависимости
COPY ./tavernhelios.client/package*.json ./
RUN npm install

# Копируем весь код фронта
COPY ./tavernhelios.client ./

# Передаём переменную окружения для Vite
ARG VITE_API_URL
ENV VITE_API_URL=${VITE_API_URL}
RUN echo "VITE_API_URL=$VITE_API_URL"
ARG NODE_ENV
ENV NODE_ENV=${NODE_ENV}
RUN echo "NODE_ENV=$NODE_ENV"
# Выполняем билд фронта
RUN npm run build

# Финальный образ для раздачи фронта через Nginx
FROM nginx:latest AS frontend
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
