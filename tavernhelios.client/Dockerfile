# (1) Сборка фронта
FROM node:24.0-slim AS build
WORKDIR /app

ARG VITE_API_URL
ARG VITE_APP_VERSION
ARG NODE_ENV

ENV VITE_API_URL=${VITE_API_URL}
ENV VITE_APP_VERSION=${VITE_APP_VERSION}
ENV NODE_ENV=${NODE_ENV}

COPY ./tavernhelios.client/package*.json ./
RUN npm install --frozen-lockfile

COPY ./tavernhelios.client ./
RUN npm run build

# (2) Финальный образ
FROM nginx:1.28.0 AS frontend
COPY --from=build /app/dist /usr/share/nginx/html
COPY ./nginx/nginx.conf.prod /etc/nginx/nginx.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
