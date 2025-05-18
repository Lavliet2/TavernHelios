import { useEffect, useState } from 'react';
import {
  Box, Typography, Table, TableBody, TableCell,
  TableContainer, TableHead, TableRow, Paper,
  TextField, Button, CircularProgress
} from '@mui/material';
import { useSnackbar } from "../hooks/useSnackbar";

interface WeatherEntry {
  label: string;
  temperatureC: number;
  condition: string;
  bold?: boolean; 
}

interface WeatherSummary {
  avgTempC: number;
  condition: string;
}

const API_KEY = '25696eaa04a54bdd8cc132631251805';

function WeatherForecast() {
  const [todayData, setTodayData] = useState<WeatherEntry[]>([]);
  const [tomorrowData, setTomorrowData] = useState<WeatherEntry[]>([]);
  const [afterTomorrowData, setAfterTomorrowData] = useState<WeatherEntry[]>([]);

  const [tomorrowSummary, setTomorrowSummary] = useState<WeatherSummary | null>(null);
  const [afterTomorrowSummary, setAfterTomorrowSummary] = useState<WeatherSummary | null>(null);

  const [todayDate, setTodayDate] = useState('');
  const [tomorrowDate, setTomorrowDate] = useState('');
  const [afterTomorrowDate, setAfterTomorrowDate] = useState('');
  const [city, setCity] = useState<string>('...');
  const [customCity, setCustomCity] = useState<string>('');
  const [loading, setLoading] = useState(false);

  const { showSnackbar } = useSnackbar();

  useEffect(() => {
    navigator.geolocation.getCurrentPosition(async (position) => {
      const lat = position.coords.latitude;
      const lon = position.coords.longitude;
      await fetchWeather(`${lat},${lon}`);
    }, async () => {
      await fetchWeather('Yekaterinburg');
    });
  }, []);

  const fetchWeather = async (query: string) => {
    try {
      setLoading(true);
      const response = await fetch(`https://api.weatherapi.com/v1/forecast.json?key=${API_KEY}&q=${query}&days=3&lang=ru`);

      if (!response.ok) {
        if ([400, 403, 404].includes(response.status)) {
          throw new Error('Город не найден или доступ запрещён');
        }
        throw new Error('Ошибка загрузки данных');
      }

      const data = await response.json();

      setCity(data.location.name);

      const forecast = data.forecast.forecastday;
      const today = forecast[0];
      const tomorrow = forecast[1];
      const afterTomorrow = forecast[2];

      setTodayDate(formatDate(today.date));
      setTomorrowDate(formatDate(tomorrow.date));
      setAfterTomorrowDate(formatDate(afterTomorrow.date));

      // Today
      const hour12Today = today.hour.find((h: any) => h.time.includes('12:00'));
      const hour13Today = today.hour.find((h: any) => h.time.includes('13:00'));

      setTodayData([
        {
          label: 'Сейчас',
          temperatureC: data.current.temp_c,
          condition: data.current.condition.text,
          bold: true 
        },
        {
          label: 'Сегодня в 12:00',
          temperatureC: hour12Today?.temp_c ?? 0,
          condition: hour12Today?.condition.text ?? 'Нет данных'
        },
        {
          label: 'Сегодня в 13:00',
          temperatureC: hour13Today?.temp_c ?? 0,
          condition: hour13Today?.condition.text ?? 'Нет данных'
        }
      ]);

      // Tomorrow
      const hour12Tomorrow = tomorrow.hour.find((h: any) => h.time.includes('12:00'));
      const hour13Tomorrow = tomorrow.hour.find((h: any) => h.time.includes('13:00'));
      setTomorrowSummary({
        avgTempC: tomorrow.day.avgtemp_c,
        condition: tomorrow.day.condition.text
      });
      setTomorrowData([
        {
          label: 'Завтра в 12:00',
          temperatureC: hour12Tomorrow?.temp_c ?? 0,
          condition: hour12Tomorrow?.condition.text ?? 'Нет данных'
        },
        {
          label: 'Завтра в 13:00',
          temperatureC: hour13Tomorrow?.temp_c ?? 0,
          condition: hour13Tomorrow?.condition.text ?? 'Нет данных'
        }
      ]);

      // After tomorrow
      const hour12AfterTomorrow = afterTomorrow.hour.find((h: any) => h.time.includes('12:00'));
      const hour13AfterTomorrow = afterTomorrow.hour.find((h: any) => h.time.includes('13:00'));
      setAfterTomorrowSummary({
        avgTempC: afterTomorrow.day.avgtemp_c,
        condition: afterTomorrow.day.condition.text
      });
      setAfterTomorrowData([
        {
          label: 'Послезавтра в 12:00',
          temperatureC: hour12AfterTomorrow?.temp_c ?? 0,
          condition: hour12AfterTomorrow?.condition.text ?? 'Нет данных'
        },
        {
          label: 'Послезавтра в 13:00',
          temperatureC: hour13AfterTomorrow?.temp_c ?? 0,
          condition: hour13AfterTomorrow?.condition.text ?? 'Нет данных'
        }
      ]);
    } catch (error: any) {
      showSnackbar(error.message || 'Ошибка загрузки', 'error');
    } finally {
      setLoading(false);
    }
  };

  const handleCitySubmit = () => {
    if (customCity.trim()) {
      fetchWeather(customCity.trim());
      setCustomCity('');
    }
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter') {
      handleCitySubmit();
    }
  };

  const formatDate = (dateStr: string) => {
    const [yyyy, mm, dd] = dateStr.split('-');
    return `${dd}.${mm}.${yyyy}`;
  };

  const renderTable = (
    title: string,
    data: WeatherEntry[],
    summary?: WeatherSummary
  ) => (
    <Box sx={{ marginTop: 4 }}>
      <Typography variant="h6" gutterBottom>{title}</Typography>
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell sx={{ width: '40%' }}>Время</TableCell>
              <TableCell sx={{ width: '30%' }}>Температура (°C)</TableCell>
              <TableCell sx={{ width: '30%' }}>Погода</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {summary && (
              <TableRow>
                <TableCell><strong>Среднесуточная</strong></TableCell>
                <TableCell>{summary.avgTempC}</TableCell>
                <TableCell>{summary.condition}</TableCell>
              </TableRow>
            )}
            {data.map((entry, idx) => (
              <TableRow key={idx}>
                <TableCell>{entry.bold ? <strong>{entry.label}</strong> : entry.label}</TableCell>
                <TableCell>{entry.temperatureC}</TableCell>
                <TableCell>{entry.condition}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );

  return (
    <Box sx={{ padding: 2, maxWidth: 800, mx: 'auto' }}>
      <Typography variant="h5" align="center">Погода в {city}</Typography>

      <Box sx={{ display: 'flex', justifyContent: 'center', gap: 2, mt: 2 }}>
        <TextField
          label="Введите город"
          variant="outlined"
          value={customCity}
          onChange={(e) => setCustomCity(e.target.value)}
          onKeyDown={handleKeyDown}
        />
        <Button variant="contained" onClick={handleCitySubmit}>
          Показать
        </Button>
      </Box>

      {loading ? (
        <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
          <CircularProgress />
        </Box>
      ) : (
        <>
          {todayData.length > 0 && renderTable(`Сегодня (${todayDate})`, todayData)}
          {tomorrowData.length > 0 && renderTable(`Завтра (${tomorrowDate})`, tomorrowData, tomorrowSummary!)}
          {afterTomorrowData.length > 0 && renderTable(`Послезавтра (${afterTomorrowDate})`, afterTomorrowData, afterTomorrowSummary!)}
        </>
      )}
    </Box>
  );
}

export default WeatherForecast;
