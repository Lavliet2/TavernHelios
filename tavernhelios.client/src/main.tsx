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

i18n.init(); 

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Provider store={store}>
    {/* <AuthProvider>  */}
      {/* <BrowserRouter> */}
        <LanguageProvider>
          <App />
        </LanguageProvider>
      {/* </BrowserRouter> */}
    {/* </AuthProvider>  */}
    </Provider>
  </StrictMode>,
)
