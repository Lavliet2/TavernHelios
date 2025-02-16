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


export const API_BASE_URL = import.meta.env.VITE_API_URL || "https://tavernhelios.duckdns.org";
console.log("VITE_API_URL from env:", import.meta.env.VITE_API_URL);