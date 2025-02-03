import React from 'react';
import { Grid2, Card, CardContent, CardMedia, Typography, Box } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import ImageLunch from '../../assets/Management/lunch.webp'
import ImageDish from '../../assets/Management/dish.webp'

const Management: React.FC = () => {
  const navigate = useNavigate();

  const handleCardClick = (path: string) => {
    navigate(path);  // Переход на нужный путь
  };

  return (
    <Box sx={{ mt: 5 }}>
      <Typography variant="h4" color="primary" align="center" sx={{ mb: 4 }}>
        Управление сущностями
      </Typography>

      {/* Используем Grid2 для карточек, указываем контейнер и item */}
      <Grid2 container spacing={4} justifyContent="center">
        
        {/* Карточка для управления блюдами */}
        <Grid2 >
          <Card onClick={() => handleCardClick('/management/dishes')} sx={{ cursor: 'pointer' }}>
            <CardMedia
              component="img"
              height="140"
              image= {ImageDish}
              alt="Manage dishes"
            />
            <CardContent>
              <Typography variant="h6" component="div">
                Управление блюдами
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Редактируйте, добавляйте или удаляйте блюда.
              </Typography>
            </CardContent>
          </Card>
        </Grid2>
        <Grid2 >
          <Card onClick={() => handleCardClick('/management/menu')} sx={{ cursor: 'pointer' }}>
            <CardMedia
              component="img"
              height="140"
              image= {ImageLunch}
              alt="Manage menu"
            />
            <CardContent>
              <Typography variant="h6" component="div">
                Управление меню
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Редактируйте, добавляйте или удаляйте блюда из меню.
              </Typography>
            </CardContent>
          </Card>
        </Grid2>
      </Grid2>
    </Box>
  );
};

export default Management;
