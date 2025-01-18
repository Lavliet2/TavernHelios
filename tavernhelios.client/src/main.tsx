import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { LanguageProvider } from './contexts/LanguageContext'; 
import './index.css'
import App from './App.tsx'
import i18n from './i18n';

i18n.init(); 

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <LanguageProvider>
      <App />
    </LanguageProvider>
  </StrictMode>,
)
