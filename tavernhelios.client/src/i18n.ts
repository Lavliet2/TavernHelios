import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';

const storedLanguage = localStorage.getItem('language') || 'ru'; 

i18n.use(initReactI18next).init({
  resources: {
    en: {
      translation: {        
        login: "Login",
        password: "Password",
        signIn: "Sign In",
        home: "Home",
        menu: "Menu",
        forecast: "Forecast",
        management: "Management",
        about: "About",
        aboutCompany: "About our company",
        welcomeText: "Welcome to Tavern Helios! Inspired by the worlds of fantasy and adventure, our team created this app to make the process of registering employees for corporate lunches simple and enjoyable, providing practical benefits and a pleasurable experience.",
        companyStory: "The legend says that in a magical corner of the world, where beer is always pouring and the smell of grilled meat lingers, the idea was born to create an app that simplifies and makes the process of registering employees for corporate lunches a delightful and convenient journey. Inspired by this goal, we gathered a team of developers and created Tavern Helios.",
        ourTeam: "Our team",
        alex: "Alex",
        alexRole: "Senior programmist fullstack",
        alexSkills: "Web, CI/CD",
        elvira: "Elvira",
        elviraRole: "Creator of business logic",
        mark: "Mark",
        markRole: "Database",
        vadim: "Vadim",
        vadimRole: "API Authorization",
        copy: "© 2025 TavernHelios. All rights reserved.",
      }
    },
    ru: {
      translation: {
        login: "Логин",
        password: "Пароль",
        signIn: "Войти",
        home: "Главная",
        menu: "Меню",
        forecast: "Прогноз погоды",
        management: "Администрирование",
        about: "О нас",
        aboutCompany: "О нашей компании",
        welcomeText: "Добро пожаловать в Tavern Helios! Вдохновленные мирами фэнтези и приключений, наша команда создала это приложение, чтобы сделать процесс записи сотрудников на корпоративные обеды простым и удобным, приносящим как практическую пользу, так и удовольствие от использования.",
        companyStory: "Легенда гласит, что в одном из волшебных уголков мира, где всегда плещется пиво и витает аромат жареного мяса, родилась идея создать приложение, которое упростит и сделает приятным процесс записи сотрудников на корпоративные обеды, превращая его в увлекательное и удобное путешествие. Вдохновленные этой целью, мы собрали команду мастеров-разработчиков и создали Tavern Helios.",
        ourTeam: "Наша команда",
        alex: "Алексей",
        alexRole: "Senior programmist fullstack",
        alexSkills: "Web, CI/CD",
        elvira: "Эльвира",
        elviraRole: "Создатель бизнес-логики",
        mark: "Марк",
        markRole: "База данных",
        vadim: "Вадим",
        vadimRole: "API авторизации",
        copy: "© 2025 TavernHelios. Все права защищены."
      }
    }
  },
  lng: storedLanguage,
  fallbackLng: 'ru',
  interpolation: {
    escapeValue: false
  }
});

export default i18n;
