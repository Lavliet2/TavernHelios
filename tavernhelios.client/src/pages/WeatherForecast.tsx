import { useEffect, useState } from 'react';
import {
  Box, Typography, Table, TableBody, TableCell,
  TableContainer, TableHead, TableRow, Paper,
  TextField, Button, CircularProgress
} from '@mui/material';
import { useSnackbar } from "../hooks/useSnackbar";
import { API_BASE_URL } from "../config";


interface WeatherEntry {
  label: string;
  temperatureC: number;
  condition: string;
  bold?: boolean;
}

interface WeatherSummary {
  avgTempC: number;
  maxTempC: number;
  minTempC: number;
  humidity: number;
  windKph: number;
  condition: string;
}

interface WeatherReply {
  city: string;
  todayDate: string;
  tomorrowDate: string;
  afterTomorrowDate: string;
  today: WeatherEntry[];
  tomorrow: WeatherEntry[];
  todaySummary: WeatherSummary | null;
  afterTomorrow: WeatherEntry[];
  tomorrowSummary: WeatherSummary | null;
  afterTomorrowSummary: WeatherSummary | null;
  state: number;
  messages: string[];
}

function WeatherForecast() {
  const [todayData, setTodayData] = useState<WeatherEntry[]>([]);
  const [tomorrowData, setTomorrowData] = useState<WeatherEntry[]>([]);
  const [afterTomorrowData, setAfterTomorrowData] = useState<WeatherEntry[]>([]);

  const [todaySummary, setTodaySummary] = useState<WeatherSummary | null>(null);
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
      const response = await fetch(`${API_BASE_URL}/api/weather?city=${encodeURIComponent(query)}`);

      if (!response.ok) {
        const err = await response.json();
        throw new Error(err?.[0] || 'Ошибка загрузки');
      }

      const data: WeatherReply = await response.json();

      setCity(data.city);
      setTodayDate(data.todayDate);
      setTomorrowDate(data.tomorrowDate);
      setAfterTomorrowDate(data.afterTomorrowDate);
      setTodayData(data.today || []);
      setTomorrowData(data.tomorrow || []);
      setTodaySummary(data.todaySummary || null);
      setTomorrowSummary(data.tomorrowSummary || null);
      setAfterTomorrowData(data.afterTomorrow || []);
      setAfterTomorrowSummary(data.afterTomorrowSummary || null);
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
    {summary && (
      <Box sx={{ mt: 1, ml: 1 }}>
        <Typography variant="body2">🌡 Макс: {summary.maxTempC}°C | Мин: {summary.minTempC}°C</Typography>
        <Typography variant="body2">💧 Влажность: {summary.humidity}% | 💨 Ветер: {summary.windKph} км/ч</Typography>
      </Box>
    )}
  </Box>
);



  return (
    <Box sx={{ padding: 2, maxWidth: 800, mx: 'auto' }}>
      <Typography variant="h5" align="center">Погода в городе "{city}"</Typography>

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
          {todayData.length > 0 && renderTable(`Сегодня (${todayDate})`, todayData, todaySummary!)}
          {tomorrowData.length > 0 && renderTable(`Завтра (${tomorrowDate})`, tomorrowData, tomorrowSummary!)}
          {afterTomorrowData.length > 0 && renderTable(`Послезавтра (${afterTomorrowDate})`, afterTomorrowData, afterTomorrowSummary!)}
        </>
      )}
    </Box>
  );
}

export default WeatherForecast;
