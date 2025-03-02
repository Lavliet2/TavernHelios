import { useState, useEffect } from 'react';
import {Box, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Typography } from '@mui/material';
import { API_BASE_URL } from "../config";

interface Forecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

function WeatherForecast() {
  const [forecasts, setForecasts] = useState<Forecast[]>();

  useEffect(() => {
    populateWeatherData();
  }, []);

  async function populateWeatherData() {
    const response = await fetch(`${API_BASE_URL}/api/weatherforecast`);
    console.log(`Запрос на: ${API_BASE_URL}`)
    if (response.ok) {
      const data = await response.json();
      setForecasts(data);
    }
  }

  const contents = forecasts === undefined ? (
    <Typography variant="body1" align="center">Загрузка...</Typography>
  ) : (
    <TableContainer component={Paper}>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>Дата</TableCell>
            <TableCell>Температура (C)</TableCell>
            <TableCell>Температура (F)</TableCell>
            <TableCell>Описание</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {forecasts.map(forecast => (
            <TableRow key={forecast.date}>
              <TableCell>{forecast.date}</TableCell>
              <TableCell>{forecast.temperatureC}</TableCell>
              <TableCell>{forecast.temperatureF}</TableCell>
              <TableCell>{forecast.summary}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );

  return (
    <Box sx={{ padding: 2 }}>
      <Typography variant="h5" color="primary" align="center" sx={{ marginTop: 4 }}>
        Weather forecast
      </Typography>
      {contents}
    </Box>
  );
}

export default WeatherForecast;
