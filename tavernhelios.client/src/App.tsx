import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import MainLayout from './layouts/MainLayout';
import Login from './pages/Login';
import Home from './pages/Home';
import Menu from './pages/Menu';
import WeatherForecast from './pages/WeatherForecast';
import About from './pages/About';

function App() {
  return (
    <Router>
      <Routes>        
        <Route path="/login" element={<Login />} />        
        <Route path="/" element={<MainLayout />}>
          <Route path="/" element={<Home />} /> 
          <Route path="/Menu" element={<Menu />} /> 
          <Route path="/forecast" element={<WeatherForecast />} /> 
          <Route path="/About" element={<About />} /> 
        </Route>
      </Routes>
    </Router>
  );
}

export default App;

