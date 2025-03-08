import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
// import { BrowserRouter } from "react-router-dom";
import { LanguageProvider } from './contexts/LanguageContext'; 
// import { AuthProvider } from './contexts/AuthContext.tsx';
import './index.css'
import App from './App.tsx'
import i18n from './i18n';


import { Provider } from "react-redux";
import { store } from "./store";
import axios from 'axios';
import { UserContextProvider } from './contexts/UserContext.tsx';

i18n.init(); 
axios.defaults.withCredentials = true;


createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Provider store={store}>
    <UserContextProvider> 
      {/* <BrowserRouter> */}
        <LanguageProvider>
          <App />
        </LanguageProvider>
      {/* </BrowserRouter> */}
    </UserContextProvider> 
    </Provider>
  </StrictMode>,
)
