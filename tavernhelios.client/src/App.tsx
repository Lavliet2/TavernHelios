// import React, { useEffect, useState } from 'react';
// import { AppBar, Toolbar, Typography, Button, Box, Container, Grid, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper } from '@mui/material';
// import NavigationBar from './components/NavigationBar';

// interface Forecast {
//     date: string;
//     temperatureC: number;
//     temperatureF: number;
//     summary: string;
// }

// function App() {
//     const [forecasts, setForecasts] = useState<Forecast[]>();

//     useEffect(() => {
//         populateWeatherData();
//     }, []);

//     const contents = forecasts === undefined
//         ? <Typography variant="body1" align="center">Загрузка... Пожалуйста, обновите страницу, как только сервер ASP.NET будет готов. Дополнительные инструкции можно найти по ссылке: <a href="https://aka.ms/jspsintegrationreact" target="_blank" rel="noreferrer">https://aka.ms/jspsintegrationreact</a></Typography>
//         : (
//             <TableContainer component={Paper} sx={{ marginTop: 3 }}>
//                 <Table aria-labelledby="tableLabel">
//                     <TableHead>
//                         <TableRow>
//                             <TableCell>Дата</TableCell>
//                             <TableCell>Температура (C)</TableCell>
//                             <TableCell>Температура (F)</TableCell>
//                             <TableCell>Описание</TableCell>
//                         </TableRow>
//                     </TableHead>
//                     <TableBody>
//                         {forecasts.map(forecast => (
//                             <TableRow key={forecast.date}>
//                                 <TableCell>{forecast.date}</TableCell>
//                                 <TableCell>{forecast.temperatureC}</TableCell>
//                                 <TableCell>{forecast.temperatureF}</TableCell>
//                                 <TableCell>{forecast.summary}</TableCell>
//                             </TableRow>
//                         ))}
//                     </TableBody>
//                 </Table>
//             </TableContainer>
//         );

//     return (
//         <Box>
//             <NavigationBar />
//             <Container maxWidth="lg" sx={{ marginTop: 4 }}>
//                 <Typography variant="h4" color="primary" align="center" sx={{ marginBottom: 2 }}>
//                     Привет, команда Helios!
//                 </Typography>
//                 <Grid container spacing={2} justifyContent="center">
//                     <Grid item xs={12} sm={6}>
//                         <Typography variant="body1" align="center">
//                             Приложение успешно задеплоено на сервер и работает корректно.
//                         </Typography>
//                         <Typography variant="body2" color="textSecondary" align="center">
//                             При мердже или пуше в ветку main обновления будут автоматически деплоиться сюда.
//                         </Typography>
//                     </Grid>
//                 </Grid>
//                 <Typography variant="h5" color="primary" align="center" sx={{ marginTop: 4 }}>
//                     Weather forecast
//                 </Typography>
//                 {contents}
//                 <Box sx={{ textAlign: 'center', marginTop: 4 }}>
//                     <Typography variant="body2" color="textSecondary">&copy; 2025 TavernHelios. Все права защищены.</Typography>
//                 </Box>
//             </Container>
//         </Box>
//     );

//     async function populateWeatherData() {
//         const response = await fetch('weatherforecast');
//         if (response.ok) {
//             const data = await response.json();
//             setForecasts(data);
//         }
//     }
// }

// export default App;

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

