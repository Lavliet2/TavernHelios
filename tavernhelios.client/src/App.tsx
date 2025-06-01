import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import NotFound from "./pages/NotFound";
import MainLayout from "./layouts/MainLayout";
import LoginForm from "./components/LoginForm";
import Home from "./pages/Home";
import Menu from "./pages/MenuDisplay";
import WeatherForecast from "./pages/WeatherForecast";
import About from "./pages/About";
import Management from "./pages/Management/Management";
import EditDishes from "./pages/Management/EditDishes";
import EditMenu from "./pages/Management/EditMenu";
import EditSchedule from "./pages/Management/EditSchedule";
import Reservations from "./pages/Management/Reservations";
import RegisterForm from "./components/RegisterForm";
import Layout from "./pages/Management/Layout/EditLayout";
import UserManagement from "./pages/Admin/UserManagement";
import { RoleProtectedRoute } from "./hoc/withRole";
import { USER_ROLES } from "./config";

import { SnackbarProvider } from "./hooks/useSnackbar";
import { DndProvider } from "react-dnd";
import { HTML5Backend } from "react-dnd-html5-backend";
import { useUser } from "./contexts/UserContext";
import { useEffect } from "react";
import axios from "axios";
import { API_BASE_URL } from "./config";

function App() {
  const userContext = useUser();

  useEffect(() => {
    axios.get(`${API_BASE_URL}/api/auth/userInfo`)
      .then(response => userContext?.login(response.data));
  }, [])

  return (
    <DndProvider backend={HTML5Backend}>
      <SnackbarProvider>
        <Router>
          <MainLayout>
            <Routes>
              {/* Страницы, доступные без авторизации */}
              <Route path="/" element={<Home />} />
              <Route path="/login" element={<LoginForm />} />
              <Route path="/register" element={<RegisterForm />} />
              <Route path="/about" element={<About />} />
              <Route path="/forecast" element={<WeatherForecast />} />
              <Route path="/menu" element={<Menu />} />

              {/* Защищенные маршруты */}
              <Route element={<RoleProtectedRoute requiredRoles={[USER_ROLES.Admin]} />}>
                <Route path="/admin/users" element={<UserManagement />} />
              </Route>

              <Route element={<RoleProtectedRoute requiredRoles={[USER_ROLES.Admin, USER_ROLES.Manager]} />}>
                <Route path="/management" element={<Management />} />
                <Route path="/management/dishes" element={<EditDishes />} />
                <Route path="/management/menu" element={<EditMenu />} />
                <Route path="/management/schedule" element={<EditSchedule />} />
                <Route path="/management/reservations" element={<Reservations />} />
                <Route path="/management/layout" element={<Layout />} />
              </Route>



              {/* 404 Страница */}
              <Route path="*" element={<NotFound />} />
            </Routes>
          </MainLayout>
        </Router>
      </SnackbarProvider>
    </DndProvider>
  );
}

export default App;
