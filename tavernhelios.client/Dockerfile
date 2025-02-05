# Этап 1: Сборка приложения с помощью Node.js
FROM node:19 AS build

# Устанавливаем рабочую директорию внутри контейнера
WORKDIR /app

# Копируем package.json и package-lock.json (или yarn.lock)
COPY package*.json ./

# Устанавливаем зависимости
RUN npm install

# Устанавливаем переменную окружения перед билдом
ARG VITE_API_URL
ENV VITE_API_URL=$VITE_API_URL

# Копируем все файлы проекта
COPY . .

# Строим проект
RUN npm run build  # Сборка React приложения

# Этап 2: Обслуживание собранных файлов с помощью Nginx
FROM nginx:alpine

# Копируем собранные файлы из этапа сборки в директорию Nginx
COPY --from=build /app/dist /usr/share/nginx/html/

# Открываем порт для доступа к контейнеру
EXPOSE 80

# Запускаем Nginx для обслуживания статической страницы
CMD ["nginx", "-g", "daemon off;"]
