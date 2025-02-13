import { fileURLToPath, URL } from 'node:url';
import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import fs from 'fs';
import path from 'path';
import { env } from 'process';

// Определяем путь к сертификатам для Windows и Linux
const isDocker = env.DOCKER === "true";
const isWindows = process.platform === "win32";
const certDir = isWindows
    ? path.join(process.cwd(), "nginx", "ssl")  // Локальная папка в проекте
    : "/etc/nginx/ssl";                         // Для Linux

const certFilePath = path.join(certDir, "nginx.crt");
const keyFilePath = path.join(certDir, "nginx.key");

console.log("isDocker:", isDocker);
console.log("isWindows:", isWindows);
console.log("Используем сертификаты:");
console.log("CERT:", certFilePath);
console.log("KEY:", keyFilePath);
console.log("NODE_ENV from process:", env.NODE_ENV);
console.log("VITE_API_URL from process:", env.VITE_API_URL);

// const target = env.VITE_API_URL || `http://178.72.83.217:32040`;
const target = env.VITE_API_URL || `https://tavernhelios.duckdns.org`;
// const target = env.VITE_API_URL 
//     || (!isDocker ? `https://localhost:5555` : `http://178.72.83.217:32040`);

console.log("API Target:", target);

const httpsOptions = isDocker && fs.existsSync(keyFilePath) && fs.existsSync(certFilePath)
    ? { key: fs.readFileSync(keyFilePath), cert: fs.readFileSync(certFilePath) }
    : undefined;
    
export const API_BASE_URL = target;
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
        https: httpsOptions
    }
});

