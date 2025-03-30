import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import { ProtectedRoute } from "./hoc/withAuth";
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
import Layout from "./pages/Management/Layout/EditLayout";

import { SnackbarProvider } from "./hooks/useSnackbar";
import { DndProvider } from "react-dnd";
import { HTML5Backend } from "react-dnd-html5-backend";

function App() {
  return (
    <DndProvider backend={HTML5Backend}>
      <SnackbarProvider>
        <Router>
          <MainLayout>
            <Routes>
              {/* Страницы, доступные без авторизации */}
              <Route path="/" element={<Home />} />
              <Route path="/login" element={<LoginForm />} />
              <Route path="/about" element={<About />} />
              <Route path="/forecast" element={<WeatherForecast />} />

              {/*Защищённые маршруты */}
              <Route element={<ProtectedRoute />}>
                <Route path="/menu" element={<Menu />} />
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
