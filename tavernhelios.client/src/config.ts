const getApiBaseUrl = () => {
    if (import.meta.env.MODE === "development") {
        return "http://localhost:32769"; 
    }
    
    if (typeof window !== "undefined" && window.VITE_API_URL) {
        return window.VITE_API_URL;
    }
  
    return "http://178.72.83.217:32040"; // Фоллбек, если ничего не передано
};

export const API_BASE_URL = getApiBaseUrl();
