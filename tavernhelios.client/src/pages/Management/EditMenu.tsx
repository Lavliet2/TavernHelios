import React, { useEffect, useState } from 'react';
import { Container, Typography, CircularProgress, Grid, Card, CardContent } from '@mui/material';


// TODO MAV полностью переделать
interface MenuValue {
  id: string;
  name: string;
  description: string;
}

const EditDishes: React.FC = () => {
  const [menuData, setMenuData] = useState<MenuValue[]>([]);
  const [loading, setLoading] = useState<boolean>(true); 
  const [error, setError] = useState<string | null>(null); 

  useEffect(() => {
    // Функция для получения данных из API
    const fetchMenuData = async () => {
      try {
        const response = await fetch('https://localhost:8081/api/Menu'); 
        if (!response.ok) {
          throw new Error('Ошибка при загрузке данных');
        }
        const data = await response.json();
        setMenuData(data); 
      } catch (error) {
        setError(error.message);
      } finally {
        setLoading(false);
      }
    };

    fetchMenuData();
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
