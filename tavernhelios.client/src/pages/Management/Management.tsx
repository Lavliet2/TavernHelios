import React from 'react';
import { Card, CardContent, CardMedia, Typography, Box } from '@mui/material';
import Grid from "@mui/material/Grid2";
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import ImageLunch from '../../assets/Management/lunch.webp';
import ImageDish from '../../assets/Management/dish.webp';
import ImageSchedule from '../../assets/Management/schedule.webp';
import ImageLayout from '../../assets/Management/layout.webp';

const Management: React.FC = () => {
  const navigate = useNavigate();
  const { t } = useTranslation();

  const handleCardClick = (path: string) => {
    navigate(path); // Переход на нужный путь
  };

  return (
    <Box sx={{ mt: 5 }}>
      <Typography variant="h4" color="primary" align="center" sx={{ mb: 4 }}>
        {t('management.title') }
      </Typography>

      <Grid container spacing={4} justifyContent="center">
        {[
          { image: ImageDish, title: t('management.dishes.title'), desc: t('management.dishes.desc'), path: "/management/dishes" },
          { image: ImageLunch, title: t('management.menu.title'), desc: t('management.menu.desc'), path: "/management/menu" },
          { image: ImageSchedule, title: t('management.schedule.title'), desc: t('management.schedule.desc'), path: "/management/schedule" },
          { image: ImageLayout, title: t('management.layout.title'), desc: t('management.layout.desc'), path: "/management/layout" }
        ].map((card, index) => (
          <Grid key={index} size={{ xs: 12, sm: 6, md:2 }} sx={{ display: 'flex', justifyContent:' center' }}>
            <Card
              onClick={() => handleCardClick(card.path)}
              sx={{
                width: 350,
                minHeight: 260,
                display: 'flex',
                flexDirection: 'column',
                justifyContent: 'space-between',
                cursor: 'pointer',
                transition: '0.3s',
                "&:hover": {
                  transform: "scale(1.05)",
                  boxShadow: "0px 4px 20px rgba(0, 0, 0, 0.2)"
                }
              }}
            >
              <CardMedia
                component="img"
                image={card.image}
                alt={card.title}
                sx={{ height: 140, objectFit: 'cover' }}
              />
              <CardContent sx={{ flexGrow: 1 }}>
                <Typography variant="h6">{card.title}</Typography>
                <Typography variant="body2" color="text.secondary">
                  {card.desc}
                </Typography>
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>
    </Box>
  );
};

export default Management;
