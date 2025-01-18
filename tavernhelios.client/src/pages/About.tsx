import React from 'react';
import { Box, Container, Typography, Grid2, Card, CardContent, CardMedia, Button } from '@mui/material';
import { GitHub, LinkedIn, Mail, Facebook } from '@mui/icons-material';
import { useTranslation } from 'react-i18next';
import AlexImage from '../assets/about/Alex.jpg';
import ElviraImage from '../assets/about/Elvira.jpg';
import MarkImage from '../assets/about/Mark.jpg';
import VadimImage from '../assets/about/Vadim.jpg';

const About: React.FC = () => {
  const { t } = useTranslation();
  return (
    <Container sx={{ mt: 5 }}>
      <Typography variant="h5" color="primary" align="center" sx={{ mb: 4 }}>
        {t('aboutCompany')}
      </Typography>
      <Typography variant="body1" align="center" sx={{ mb: 4 }}>
        {t('welcomeText')}
      </Typography>
      <Typography variant="body1" align="center" sx={{ mb: 4 }}>
        {t('companyStory')}
      </Typography>
      <Typography variant="h5" color="primary" align="center" sx={{ mb: 4 }}>
        {t('ourTeam')}
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
              alt={t('alex')}
            />
            <CardContent sx={{ flexGrow: 1 }}>
              <Typography variant="h6" component="div">
                {t('alex')}
              </Typography>
              <Typography variant="body1" color="text">
                {t('alexRole')}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                {t('alexSkills')}
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
                {t('elvira')}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                {t('elviraRole')}
              </Typography>
            </CardContent>
            <Box sx={{ display: 'flex', justifyContent: 'center', pb: 2 }}>
              <Button href="https://github.com/CoffeePrograms" target="_blank" sx={{ mr: 1 }}>
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
                {t('mark')}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                {t('markRole')}
              </Typography>
            </CardContent>
            <Box sx={{ display: 'flex', justifyContent: 'center', pb: 2 }}>
                <Button href="https://github.com/MasterGorvant" target="_blank" sx={{ mr: 1 }}>
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
                {t('vadim')}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                {t('vadimRole')}
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
