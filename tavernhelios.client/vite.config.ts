import { fileURLToPath, URL } from 'node:url';
import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import fs from 'fs';
import path from 'path';
import child_process from 'child_process';
import { env } from 'process';

const isDev = env.NODE_ENV === 'development';

const baseFolder =
    env.APPDATA !== undefined && env.APPDATA !== ''
        ? `${env.APPDATA}/ASP.NET/https`
        : `${env.HOME}/.aspnet/https`;

const certificateName = "tavernhelios.client";
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

if (isDev && (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath))) {
    if (0 !== child_process.spawnSync('dotnet', [
        'dev-certs',
        'https',
        '--export-path',
        certFilePath,
        '--format',
        'Pem',
        '--no-password',
    ], { stdio: 'inherit' }).status) {
        throw new Error("Could not create certificate.");
    }
}

// 🔹 Тут прописываем реальный адрес бэкенда
const target = env.VITE_API_URL || `https://localhost:32783`;

export default defineConfig({
    plugins: [plugin()],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },
    server: {
        proxy: {
            // 🔹 Перенаправляем ВСЕ запросы, начинающиеся с `/api`
            '/api': {
                target, // API-сервер
                changeOrigin: true, // Меняет `Host` заголовок
                secure: false, // Игнорируем SSL (можно убрать для продакшена)
                rewrite: (path) => path.replace(/^\/api/, '/api') // Убираем `/api`, если нужно
            }
        },
        port: 63049,
        ...(isDev && {
            https: {
                key: fs.readFileSync(keyFilePath),
                cert: fs.readFileSync(certFilePath),
            }
        }), 
    }
});
