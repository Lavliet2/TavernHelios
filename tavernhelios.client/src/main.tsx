import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
// import { BrowserRouter } from "react-router-dom";
import { LanguageProvider } from './contexts/LanguageContext';
import './index.css'
import App from './App.tsx'
import i18n from './i18n';
import axios from 'axios';
import { UserProvider } from './contexts/UserContext.tsx';
import { AuthProvider } from './contexts/AuthContext.tsx';

i18n.init();
axios.defaults.withCredentials = true;
//axios.defaults.headers.common['Access-Control-Allow-Credentials'] = true;

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <UserProvider>
      <AuthProvider>
        {/* <BrowserRouter> */}
        <LanguageProvider>
          <App />
        </LanguageProvider>
        {/* </BrowserRouter> */}


      </AuthProvider>
    </UserProvider>
  </StrictMode>,
)
