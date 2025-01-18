import React, { createContext, useState, ReactNode } from 'react';
import { useTranslation } from 'react-i18next';

// Создаем тип для контекста
interface LanguageContextType {
  changeLanguage: (lang: string) => void;
}

// Создаем контекст
export const LanguageContext = createContext<LanguageContextType | undefined>(undefined);

// Тип для компонента-поставщика контекста, который принимает children
interface LanguageProviderProps {
  children: ReactNode;
}

// Компонент провайдер контекста
export const LanguageProvider: React.FC<LanguageProviderProps> = ({ children }) => {
  const { i18n } = useTranslation();

  // Функция изменения языка
  const changeLanguage = (lang: string) => {
    console.log("Switching language to:", lang);
    i18n.changeLanguage(lang);
  };

  return (
    <LanguageContext.Provider value={{ changeLanguage }}>
      {children}
    </LanguageContext.Provider>
  );
};
