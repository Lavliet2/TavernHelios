// import { Description } from '@mui/icons-material';
// import { common } from '@mui/material/colors';
// import { error } from 'console';
import i18n from 'i18next';
// import { title } from 'process';
import { initReactI18next } from 'react-i18next';
// import { addDish } from './services/dishService';

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
        managementMenu: "Management",
        about: "About",
        aboutCompany: "About our company",
        welcomeText: "Welcome to Tavern Helios! Inspired by the worlds of fantasy and adventure, our team created this app to make the process of registering employees for corporate lunches simple and enjoyable, providing practical benefits and a pleasurable experience.",
        companyStory: "The legend says that in a magical corner of the world, where beer is always pouring and the smell of grilled meat lingers, the idea was born to create an app that simplifies and makes the process of registering employees for corporate lunches a delightful and convenient journey. Inspired by this goal, we gathered a team of developers and created Tavern Helios.",
        ourTeam: "Our team",
        alex: "Alex",
        alexRole: "Senior Full-Stack Developer",
        alexSkills: "Web, CI/CD",
        elvira: "Elvira",
        elviraRole: "Creator of business logic",
        mark: "Mark",
        markRole: "Database",
        vadim: "Vadim",
        vadimRole: "API Authorization", 
        copy: "© 2025 TavernHelios. All rights reserved.",

        common: {
          Add: "Add",
          edit: "Edit",
          delete: "Delete",
          save: "Save",
          Cancel: "Cancel",
          close: "Close",
          confirm: "Confirm",
          yes: "Yes",
          no: "No",
          ok: "OK",
          title: "Title",
          description: "Description",
          name: "Name",
          type: "Type",
          actions: "Actions",
          required: "Required",
          image: "Image",
          selectImage: "Select image",
          uploadImage: "Upload image",
          upload: "Upload",
          download: "Download",
          downloadImage: "Download image",
          preview: "Preview",
          previewImage: "Preview image",
        
          error: "Error",
          success: "Success",
          loading: "Loading...",
          loadingData: "Loading data...",
          noData: "No data",
          search: "Search",
          filter: "Filter",
          select: "Select",
          selectAll: "Select all",

        },

        management: {
          title: "Entity Management",
          dishes: {
            title: "Manage Dishes",
            desc: "Edit, add, or remove dishes."
          },
          menu: {
            title: "Manage Menu",
            desc: "Edit, add, or remove dishes from the menu."
          },
          schedule: {
            title: "Manage Schedule",
            desc: "Edit, add, or remove menus from the schedule."
          },
          layout: {
            title: "Manage Layout",
            desc: "Edit, add, or remove tables and chairs."
          }
        },

        editDishes: {
          title: "Manage Dishes",
          addDish: "Add dish",
          editDish: "Edit dish",
          dishType: "Dish type",
          dishes: {
            addTooltip: "Add dish",
            desc: "Редактируйте, добавляйте или удаляйте блюда."
          },
          dishList: {
            title: "Title",
            desc: "Description",
            image: "Image",
            actions: "Actions",
          },
          dishAction: {
            edit: "Edit",
            delete: "Delete",
          }
        },

        // menuAddDishModal: {
        //   title: "Add dish",
        //   dishType: "Dish type"
        // },

        editMenu:{
          title: "Manage Menu",
          addDish: "Add dish",
          Menu: "Menu",
          menu: "menu",

        },

        admin: {
          users: {
            title: "User Management",
            addUser: "Add user",
            editUser: "Edit User",
            login: "Login",
            fullName: "Fullname",
            password: "Password",
            roles: "Roles",
            roleAdmin: "Admin",
            roleManager: "Менеджер",
            actions: "Actions",
            edit: "Edit",
            delete: "Delete",
            confirmDelete: "You sure in deleting?",
          },
        },
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
        managementMenu: "Управление",
        about: "О нас",
        aboutCompany: "О нашей компании",
        welcomeText: "Добро пожаловать в Tavern Helios! Вдохновленные мирами фэнтези и приключений, наша команда создала это приложение, чтобы сделать процесс записи сотрудников на корпоративные обеды простым и удобным, приносящим как практическую пользу, так и удовольствие от использования.",
        companyStory: "Легенда гласит, что в одном из волшебных уголков мира, где всегда плещется пиво и витает аромат жареного мяса, родилась идея создать приложение, которое упростит и сделает приятным процесс записи сотрудников на корпоративные обеды, превращая его в увлекательное и удобное путешествие. Вдохновленные этой целью, мы собрали команду мастеров-разработчиков и создали Tavern Helios.",
        ourTeam: "Наша команда",
        alex: "Алексей",
        alexRole: "Senior Full-Stack Developer",
        alexSkills: "Web, CI/CD",
        elvira: "Эльвира",
        elviraRole: "Создатель бизнес-логики",
        mark: "Марк",
        markRole: "База данных",
        vadim: "Вадим",
        vadimRole: "API авторизации",
        copy: "© 2025 TavernHelios. Все права защищены.",

        common: {
          add: "Добавить",
          edit: "Редактировать",
          delete: "Удалить",
          save: "Сохранить",
          cancel: "Отмена",
          close: "Закрыть",
          confirm: "Подтвердить",
          yes: "Да",
          no: "Нет",
          ok: "ОК",
          title: "Заголовок",
          description: "Описание",
          name: "Название",
          type: "Тип",
          actions: "Действия",
          required: "Обязательно",
          image: "Изображение",
          selectImage: "Выберите изображение",
          uploadImage: "Загрузить изображение",
          upload: "Загрузить",
          download: "Скачать",
          downloadImage: "Скачать изображение",
          preview: "Предпросмотр",
          previewImage: "Предпросмотр изображения",

          error: "Ошибка",
          success: "Успех",
          loading: "Загрузка...",
          loadingData: "Загрузка данных...",
          noData: "Нет данных",
          search: "Поиск",
          filter: "Фильтр",
          select: "Выбрать",
          selectAll: "Выбрать все",
        },

        management: {
          title: "Управление сущностями",
          dishes: {
            title: "Управление блюдами",
            desc: "Редактируйте, добавляйте или удаляйте блюда."
          },
          menu: {
            title: "Управление меню",
            desc: "Редактируйте, добавляйте или удаляйте блюда из меню."
          },
          schedule: {
            title: "Управление расписанием",
            desc: "Редактируйте, добавляйте или удаляйте меню из расписания."
          },
          layout: {
            title: "Управление залом",
            desc: "Редактируйте, добавляйте или удаляйте столы и стулья."
          }
        },

        editDishes: {
          title: "Редактирование блюд",
          addDish: "Добавить блюдо",
          editDish: "Редактировать блюдо",
          dishType: "Тип блюда",
          dishes: {
            addTooltip: "Добавить блюдо",
            desc: "Редактируйте, добавляйте или удаляйте блюда."
          },
          dishList: {
            title: "Название",
            desc: "Описание",
            image: "Изображение",
            actions: "Действия",
          },
          dishAction: {
            edit: "Редактировать",
            delete: "Удалить",
          },
        },

        // menuAddDishModal: {
        //   title: "Добавить блюдо",
        //   dishType: "Тип блюда",
        // },

        editMenu:{
          title: "Редактирование меню",
          addDish: "Добавить блюдо",
          Menu: "Меню",
          menu: "меню",  
        },

        admin: {
          users: {
            title: "Управление пользователями",
            addUser: "Добавить пользователя",
            editUser: "Редактировать пользователя",
            login: "Логин",
            fullName: "Полное имя",
            password: "Пароль",
            roles: "Роли",
            roleAdmin: "Администратор",
            roleManager: "Менеджер",
            actions: "Действия",
            edit: "Редактировать",
            delete: "Удалить",
            confirmDelete: "Вы уверены, что хотите удалить этого пользователя?",
          },
        },
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
