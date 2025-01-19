import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
// import { BrowserRouter } from "react-router-dom";
import { LanguageProvider } from './contexts/LanguageContext'; 
import { AuthProvider } from './contexts/AuthContext.tsx';
import './index.css'
import App from './App.tsx'
import i18n from './i18n';

i18n.init(); 

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <AuthProvider> 
      {/* <BrowserRouter> */}
        <LanguageProvider>
          <App />
        </LanguageProvider>
      {/* </BrowserRouter> */}
    </AuthProvider> 
  </StrictMode>,
)
