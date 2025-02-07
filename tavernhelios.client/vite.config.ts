import { fileURLToPath, URL } from 'node:url';
import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import fs from 'fs';
import path from 'path';
import { env } from 'process';

// Определяем путь к сертификатам для Windows и Linux
const isWindows = process.platform === "win32";
const certDir = isWindows
    ? path.join(process.cwd(), "nginx", "ssl")  // Локальная папка в проекте
    : "/etc/nginx/ssl";                         // Для Linux

const certFilePath = path.join(certDir, "tavernhelios.client.pem");
const keyFilePath = path.join(certDir, "tavernhelios.client.key");

// Тут прописываем реальный адрес бэкенда
console.log("NODE_ENV from process:", env.NODE_ENV);
console.log("VITE_API_URL from process:", env.VITE_API_URL);
const target = env.VITE_API_URL || `http://178.72.83.217:32040`;

export default defineConfig({
    plugins: [plugin()],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },
    server: {
        proxy: {           
            '/api': {
                target,
                changeOrigin: true,
                secure: false,
                rewrite: (path) => path.replace(/^\/api/, '/api')
            }
        },
        port: 8888,
        https: {
            key: fs.existsSync(keyFilePath) ? fs.readFileSync(keyFilePath) : undefined,
            cert: fs.existsSync(certFilePath) ? fs.readFileSync(certFilePath) : undefined,
        }
    }
});
