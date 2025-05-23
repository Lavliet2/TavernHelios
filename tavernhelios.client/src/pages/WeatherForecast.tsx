import { useEffect, useState, useCallback } from 'react';
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
  iconUrl?: string;
  bold?: boolean;
}

interface WeatherSummary {
  avgTempC: number;
  maxTempC: number;
  minTempC: number;
  humidity: number;
  windKph: number;
  condition: string;
  iconUrl?: string;
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

  const sortEntries = (entries: WeatherEntry[]): WeatherEntry[] => {
    const order = ['Сейчас', 'Утром', 'В 12:00', 'В 13:00', 'Вечером'];
    return entries.slice().sort((a, b) => {
      const idxA = order.indexOf(a.label);
      const idxB = order.indexOf(b.label);
      return (idxA === -1 ? 999 : idxA) - (idxB === -1 ? 999 : idxB);
    });
  };

  useEffect(() => {
    navigator.geolocation.getCurrentPosition(async (position) => {
      const lat = position.coords.latitude;
      const lon = position.coords.longitude;
      await fetchWeather(`${lat},${lon}`);
    }, async () => {
      await fetchWeather('Yekaterinburg');
    });
  }, []);


  const fetchWeather = useCallback(async (query: string) => {
    try {
      setLoading(true);
      const response = await fetch(`${API_BASE_URL}/api/weather?city=${encodeURIComponent(query)}`);

      if (!response.ok) {
        const err = await response.json();
        throw new Error(err?.[0] || 'Ошибка загрузки');
      }

      const data: WeatherReply = await response.json();

      setCity(data.city);
      if (data.state !== 0 && data.messages?.length) {
        showSnackbar(data.messages.join('\n'), 'error');
      }
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
  }, [showSnackbar]);


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
            {/* {data.map((entry, idx) => ( */}
            {sortEntries(data).map((entry, idx) => (
              <TableRow key={idx}>
                <TableCell>{entry.bold ? <strong>{entry.label}</strong> : entry.label}</TableCell>
                <TableCell>{entry.temperatureC}</TableCell>
                <TableCell>
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                    {entry.iconUrl && (
                      <img
                        src={entry.iconUrl.startsWith('//') ? 'https:' + entry.iconUrl : entry.iconUrl}
                        alt={entry.condition}
                        width={32}
                        height={32}
                      />
                    )}
                    <span>{entry.condition}</span>
                  </Box>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
      {summary && (
        <Paper variant="outlined" sx={{ mt: 0.5, p: 1.5, backgroundColor: '#f5f5f5' }}>
          <Typography variant="body2" gutterBottom>
            <strong>🌡 Температура:</strong> макс {summary.maxTempC}°C, мин {summary.minTempC}°C
          </Typography>
          <Typography variant="body2" gutterBottom>
            <strong>💧 Влажность:</strong> {summary.humidity}% &nbsp;&nbsp;
            <strong>💨 Ветер:</strong> {summary.windKph} км/ч
          </Typography>
        </Paper>
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
          {todayData.length > 0 && renderTable(`Сегодня (${todayDate})`, todayData, todaySummary ?? undefined)}
          {tomorrowData.length > 0 && renderTable(`Завтра (${tomorrowDate})`, tomorrowData, tomorrowSummary!)}
          {afterTomorrowData.length > 0 && renderTable(`Послезавтра (${afterTomorrowDate})`, afterTomorrowData, afterTomorrowSummary!)}
        </>
      )}
    </Box>
  );
}

export default WeatherForecast;
