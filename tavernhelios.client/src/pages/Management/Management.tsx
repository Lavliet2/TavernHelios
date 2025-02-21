import React from 'react';
import { Card, CardContent, CardMedia, Typography, Box } from '@mui/material';
import Grid from "@mui/material/Grid2";
import { useNavigate } from 'react-router-dom';
import ImageLunch from '../../assets/Management/lunch.webp';
import ImageDish from '../../assets/Management/dish.webp';
import Imageschedule from '../../assets/Management/schedule.webp';

const Management: React.FC = () => {
  const navigate = useNavigate();

  const handleCardClick = (path: string) => {
    navigate(path); // Переход на нужный путь
  };

  return (
    <Box sx={{ mt: 5 }}>
      <Typography variant="h4" color="primary" align="center" sx={{ mb: 4 }}>
        Управление сущностями
      </Typography>

      <Grid container spacing={4} justifyContent="center">
        {[
          { image: ImageDish, title: "Управление блюдами", desc: "Редактируйте, добавляйте или удаляйте блюда.", path: "/management/dishes" },
          { image: ImageLunch, title: "Управление меню", desc: "Редактируйте, добавляйте или удаляйте блюда из меню.", path: "/management/menu" },
          { image: Imageschedule, title: "Управление расписанием", desc: "Редактируйте, добавляйте или удаляйте меню из расписания.", path: "/management/schedule" }
        ].map((card, index) => (
          <Grid key={index} size={{ xs: 12, sm: 6, md:2 }} sx={{ display: 'flex', justifyContent:' center' }}>
            <Card
              onClick={() => handleCardClick(card.path)}
              sx={{
                cursor: 'pointer',
                transition: '0.3s',
                "&:hover": {
                  transform: "scale(1.05)",
                  boxShadow: "0px 4px 20px rgba(0, 0, 0, 0.2)"
                }
              }}
            >
              <CardMedia component="img" height="140" image={card.image} alt={card.title} />
              <CardContent>
                <Typography variant="h6">{card.title}</Typography>
                <Typography variant="body2" color="text.secondary">{card.desc}</Typography>
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>
    </Box>
  );
};

export default Management;
