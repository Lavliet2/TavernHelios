import React, { useEffect, useState } from 'react';
import { Container, Typography, CircularProgress, Grid, Card, CardContent } from '@mui/material';

// Тип данных для MenuValue
interface MenuValue {
  id: string;
  name: string;
  description: string;
  // Добавьте другие поля, которые возвращает ваш API
}

const EditDishes: React.FC = () => {
  const [menuData, setMenuData] = useState<MenuValue[]>([]);
  const [loading, setLoading] = useState<boolean>(true); // Стейт для загрузки
  const [error, setError] = useState<string | null>(null); // Стейт для ошибок

  useEffect(() => {
    // Функция для получения данных из API
    const fetchMenuData = async () => {
      try {
        const response = await fetch('http://localhost:8080/api/menu');  // URL вашего API
        if (!response.ok) {
          throw new Error('Ошибка при загрузке данных');
        }
        const data = await response.json();
        setMenuData(data); // Устанавливаем полученные данные
      } catch (error) {
        setError(error.message); // Обработка ошибки
      } finally {
        setLoading(false); // Завершаем загрузку
      }
    };

    fetchMenuData(); // Вызов функции
  }, []);

  if (loading) {
    return (
      <Container sx={{ mt: 4 }} align="center">
        <CircularProgress /> {/* Показываем индикатор загрузки */}
        <Typography variant="body1">Загрузка данных...</Typography>
      </Container>
    );
  }

  if (error) {
    return (
      <Container sx={{ mt: 4 }} align="center">
        <Typography variant="body1" color="error">{error}</Typography> 
      </Container>
    );
  }

  return (
    <Container sx={{ mt: 4 }}>
      <Typography variant="h4" color="primary" align="center" sx={{ mb: 4 }}>
        Редактирование блюд
      </Typography>
      <Typography variant="body1" color="text.secondary" align="center" sx={{ mb: 4 }}>
        Здесь вы можете редактировать блюда, добавлять новые или удалять существующие.
      </Typography>

      {/* Отображение меню */}
      <Grid container spacing={4} justifyContent="center">
        {menuData.map((menu) => (
          <Grid item xs={12} sm={6} md={3} key={menu.id}>
            <Card>
              <CardContent>
                <Typography variant="h6">{menu.name}</Typography>
                <Typography variant="body2" color="text.secondary">{menu.description}</Typography>
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>
    </Container>
  );
};

export default EditDishes;
