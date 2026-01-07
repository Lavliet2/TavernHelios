// console.log("VITE_API_URL from env:", import.meta.env.VITE_API_URL, import.meta.env.MODE);

// const getApiBaseUrl = () => {
//     if (import.meta.env.MODE === "development") {
//         console.log("Development mode detected");
//         return import.meta.env.VITE_API_URL || "https://localhost:5555";
//     }

//     if (typeof window !== "undefined" && window.VITE_API_URL) {
//         console.log("Using VITE_API_URL:", window.VITE_API_URL);
//         return window.VITE_API_URL;
//     }

//     console.log("Production mode detected");
//     return "http://178.72.83.217:32040"; // Фоллбек
// };


// export const API_BASE_URL = getApiBaseUrl();


// В Docker используем пустую строку для относительных путей (nginx проксирует /api/)
// Для локальной разработки можно использовать "https://localhost:5555"
// Для продакшена - "https://tavernhelios.duckdns.org"
const getApiBaseUrl = () => {
  const envUrl = import.meta.env.VITE_API_URL;
  
  // Если пустая строка или не задано - используем относительные пути (для Docker)
  if (!envUrl || envUrl === "") {
    return "";
  }
  
  return envUrl;
};

export const API_BASE_URL = getApiBaseUrl();
export const USER_ROLES = {
    Admin: 1,
    Manager: 2
};
console.log("VITE_API_URL from env:", import.meta.env.VITE_API_URL);