import React from 'react';
import { Box, Container, Typography, Grid2, Card, CardContent, CardMedia, Button } from '@mui/material';
import { GitHub, LinkedIn, Mail, Facebook } from '@mui/icons-material';
import AlexImage from '../assets/about/Alex.jpg';
import ElviraImage from '../assets/about/Elvira.jpg';
import MarkImage from '../assets/about/Mark.jpg';
import VadimImage from '../assets/about/Vadim.jpg';

const About: React.FC = () => {
  return (
    <Container sx={{ mt: 5 }}>
      <Typography variant="h5" color="primary" align="center" sx={{ mb: 4 }}>
        О нашей компании
      </Typography>
      <Typography variant="body1" align="center" sx={{ mb: 4 }}>
        Добро пожаловать в <strong>Tavern Helios</strong>! Вдохновленные мирами фэнтези и приключений, наша команда создала это приложение, чтобы сделать процесс записи сотрудников на корпоративные обеды простым и удобным, приносящим как практическую пользу, так и удовольствие от использования.
      </Typography>
      <Typography variant="body1" align="center" sx={{ mb: 4 }}>
        Легенда гласит, что в одном из волшебных уголков мира, где всегда плещется пиво и витает аромат жареного мяса, родилась идея создать приложение, которое упростит и сделает приятным процесс записи сотрудников на корпоративные обеды, превращая его в увлекательное и удобное путешествие. Вдохновленные этой целью, мы собрали команду мастеров-разработчиков и создали Tavern Helios.
      </Typography>
      <Typography variant="h5" color="primary" align="center" sx={{ mb: 4 }}>
        Наша команда
      </Typography>
      <Grid2 container spacing={4} justifyContent="center">
        <Grid2 size={{ xs: 12, sm: 3 }} >
          <Card
            sx={{
              display: 'flex',
              flexDirection: 'column',
              height: '100%',
              transition: 'transform 0.3s ease-in-out', 
              '&:hover': {
                transform: 'scale(1.05)'
              }
            }}
          >
            <CardMedia
              component="img"
              height="140"
              image={AlexImage}
              alt="Developer Alex"
            />
            <CardContent sx={{ flexGrow: 1 }}>
              <Typography variant="h6" component="div">
                Алексей
              </Typography>
              <Typography variant="body1" color="text">
                Senior programmist fullstack
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Web, CI/CD
              </Typography>
            </CardContent>
            <Box sx={{ display: 'flex', justifyContent: 'center', pb: 2 }}>
              <Button href="https://github.com/Lavliet2" target="_blank" sx={{ mr: 1 }}>
                <GitHub />
              </Button>
              <Button href="mailto:Mantigor2@gmail.com" target="_blank" sx={{ mr: 1 }}>
                <Mail />
              </Button>
              <Button href="https://www.facebook.com/lavliet.mantigor" target="_blank">
                <Facebook />
              </Button>
            </Box>
          </Card>
        </Grid2>

        <Grid2 size={{ xs: 12, sm: 3 }} >
          <Card
            sx={{
              display: 'flex',
              flexDirection: 'column',
              height: '100%',
              transition: 'transform 0.3s ease-in-out', 
              '&:hover': {
                transform: 'scale(1.05)'
              }
            }}
          >
            <CardMedia
              component="img"
              height="140"
              image={ElviraImage}
              alt="Developer Elvira"
            />
            <CardContent sx={{ flexGrow: 1 }}>
              <Typography variant="h6" component="div">
                Эльвира
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Создатель бизнес-логики.
              </Typography>
            </CardContent>
            <Box sx={{ display: 'flex', justifyContent: 'center', pb: 2 }}>
              <Button href="https://github.com" target="_blank" sx={{ mr: 1 }}>
                <GitHub />
              </Button>
              <Button href="https://linkedin.com" target="_blank">
                <LinkedIn />
              </Button>
            </Box>
          </Card>
        </Grid2>

        <Grid2 size={{ xs: 12, sm: 3 }} >
          <Card
            sx={{
              display: 'flex',
              flexDirection: 'column',
              height: '100%',
              transition: 'transform 0.3s ease-in-out', 
              '&:hover': {
                transform: 'scale(1.05)'
              }
            }}
          >
            <CardMedia
              component="img"
              height="140"
              image={MarkImage}
              alt="Developer Mark"
            />
            <CardContent sx={{ flexGrow: 1 }}>
              <Typography variant="h6" component="div">
                Марк
              </Typography>
              <Typography variant="body2" color="text.secondary">
                База данных.
              </Typography>
            </CardContent>
            <Box sx={{ display: 'flex', justifyContent: 'center', pb: 2 }}>
              <Button href="https://github.com" target="_blank" sx={{ mr: 1 }}>
                <GitHub />
              </Button>
              <Button href="https://linkedin.com" target="_blank">
                <LinkedIn />
              </Button>
            </Box>
          </Card>
        </Grid2>

        <Grid2 size={{ xs: 12, sm: 3 }} >
          <Card
            sx={{
              display: 'flex',
              flexDirection: 'column',
              height: '100%',
              transition: 'transform 0.3s ease-in-out', 
              '&:hover': {
                transform: 'scale(1.05)'
              }
            }}
          >
            <CardMedia
              component="img"
              height="140"
              image={VadimImage}
              alt="Developer Vadim"
            />
            <CardContent sx={{ flexGrow: 1 }}>
              <Typography variant="h6" component="div">
                Вадим
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Api авторизации
              </Typography>
            </CardContent>
            <Box sx={{ display: 'flex', justifyContent: 'center', pb: 2 }}>
              <Button href="https://github.com" target="_blank" sx={{ mr: 1 }}>
                <GitHub />
              </Button>
              <Button href="https://linkedin.com" target="_blank">
                <LinkedIn />
              </Button>
            </Box>
          </Card>
        </Grid2>
      </Grid2>
    </Container>
  );
};

export default About;
