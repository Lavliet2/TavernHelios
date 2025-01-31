// import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
// import MainLayout from './layouts/MainLayout';
// import LoginForm from './components/LoginForm'
// import Home from './pages/Home';
// import Menu from './pages/Menu';
// import WeatherForecast from './pages/WeatherForecast';
// import About from './pages/About';

// function App() {
//   return (
//     <Router>
//       <Routes>        
//         <Route path="/login" element={<LoginForm />} />        
//         <Route path="/" element={<MainLayout />}>
//           <Route path="/" element={<Home />} /> 
//           <Route path="/Menu" element={<Menu />} /> 
//           <Route path="/forecast" element={<WeatherForecast />} /> 
//           <Route path="/About" element={<About />} /> 
//         </Route>
//       </Routes>
//     </Router>
//   );
// }

// export default App;

import { BrowserRouter as Router, Route, Routes, Navigate, Outlet } from 'react-router-dom';
import { useAuth } from './contexts/AuthContext'; // Импортируем контекст аутентификации
import MainLayout from './layouts/MainLayout';
import LoginForm from './components/LoginForm';
import Home from './pages/Home';
import Menu from './pages/Menu';
import WeatherForecast from './pages/WeatherForecast';
import About from './pages/About';
import Management from './pages/Management/Management';
import EditDishes from './pages/Management/EditDishes';

// Защищенный маршрут
const ProtectedRoute = () => {
  const { isAuthenticated } = useAuth();

  return isAuthenticated ? <Outlet /> : <Navigate to="/login" />;
};

function App() {
  return (
    <Router>
      <Routes>
        {/* Страница авторизации */}
        <Route path="/login" element={<LoginForm />} />

        {/* Защищенные маршруты */}
        <Route element={<ProtectedRoute />}>
          <Route path="/" element={<MainLayout />}>
            <Route path="/" element={<Home />} />
            <Route path="/menu" element={<Menu />} />
            <Route path="/forecast" element={<WeatherForecast />} />
            <Route path="/about" element={<About />} />
            <Route path="/management" element={<Management />} />
            <Route path="/management/dishes" element={<EditDishes />} />
          </Route>
        </Route>
      </Routes>
    </Router>
  );
}

export default App;



