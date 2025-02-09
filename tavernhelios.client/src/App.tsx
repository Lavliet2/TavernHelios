import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import MainLayout from './layouts/MainLayout';
import LoginForm from './components/LoginForm';
import Home from './pages/Home';
import Menu from './pages/Menu';
import WeatherForecast from './pages/WeatherForecast';
import About from './pages/About';
import Management from './pages/Management/Management';
import EditDishes from './pages/Management/EditDishes';
import EditMenu from './pages/Management/EditMenu';
import ErrorHandler from './components/ErrorHandler';
import { useEffect } from 'react';
import { useUserContext } from './contexts/UserContext';

function App() {
  const userContext = useUserContext();

  useEffect(() => {
    userContext.login()
      .then((data) => console.log(data));
  }, []);

  return (
    <Router>
      <ErrorHandler>

        <Routes>
          {/* Страница авторизации */}
          <Route path="/login" element={<LoginForm />} />

          {/* Защищенные маршруты */}
          {/* <Route element={<ProtectedRoute />}> */}
          <Route path="/" element={<MainLayout />}>
            <Route path="/" element={<Home />} />
            <Route path="/menu" element={<Menu />} />
            <Route path="/forecast" element={<WeatherForecast />} />
            <Route path="/about" element={<About />} />
            <Route path="/management" element={<Management />} />
            <Route path="/management/dishes" element={<EditDishes />} />
            <Route path="/management/menu" element={<EditMenu />} />
          </Route>
          {/* </Route> */}
        </Routes>
      </ErrorHandler>

    </Router>
  );
}

export default App;



