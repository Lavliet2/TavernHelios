import React from 'react';
import { Container, Typography, Card, CardMedia, CardContent,
  CircularProgress, FormControl, RadioGroup, FormControlLabel,
  Radio, Box, Button
} from '@mui/material';
import Grid from '@mui/material/Grid2';
import { useNavigate } from "react-router-dom"; //MAV delete

import { useMenuDisplay } from "../hooks/Menu/useMenuDisplay";

import ReservationList from "../components/Menu/ReservationList";
import dishTypes from "../constants/dishTypes";


const MenuDisplay: React.FC = () => {
  const {
    menu,
    groupedDishes,
    loadingMenu,
    loadingDishes,
    error,
    selectedDishes,
    maxCardHeight,
    selectedTime,
    refreshKey,
    handleReservation,
    handleSelectionChange,
    setSelectedTime,
    addToCardRefs,
  } = useMenuDisplay();

  const navigate = useNavigate(); //MAV delete

  if (loadingMenu) {
    return (
      <Container sx={{ mt: 4, textAlign: 'center' }}>
        <CircularProgress />
        <Typography variant="body1">Загрузка меню...</Typography>
      </Container>
    );
  }

  if (error) {
    return (
      <Container sx={{ mt: 4, textAlign: 'center' }}>
        <Typography variant="body1" color="error"> {error}</Typography>
      </Container>
    );
  }

  return (
    <Container maxWidth="xl" sx={{ mt: 4 }}>
      <Typography variant="h4" align="center" gutterBottom>
        {menu?.name}
      </Typography>
      {loadingDishes ? (
        <Box sx={{ textAlign: 'center', mt: 2 }}>
          <CircularProgress />
          <Typography variant="body1">Загрузка блюд...</Typography>
        </Box>
      ) : (
        <Grid container spacing={10} sx={{ alignItems: 'stretch', width: '100%' }}>
          {dishTypes.map(type => (
            <Grid key={type.value} size={{ xs: 12, sm: 12, md: 6 }}>
              <Typography 
                variant="h5" 
                gutterBottom
                sx={{
                  borderBottom: '3px solid',
                  borderColor: 'primary.main',
                  display: 'inline-block',
                  paddingBottom: '4px',
                  fontWeight: 'bold',
                  width: '100%'
                }}
              >
                {type.label}
              </Typography>
              <FormControl fullWidth component="fieldset">
                <RadioGroup
                  row
                  value={selectedDishes[type.value] || ''}
                  onChange={(e) => handleSelectionChange(type.value, e.target.value)}
                >
                  <Box sx={{ width: '100%' }}>
                    <Grid container spacing={2} sx={{ width: '100%', justifyContent: 'center'}}>
                      {groupedDishes[type.value]?.map(dish => (
                        <Grid key={dish.id} size={{ xs: 12, sm: 6, md: 6}} sx={{ display: 'flex' }}>
                          <Card
                            ref={addToCardRefs}
                            onClick={() => handleSelectionChange(type.value, dish.id)}
                            sx={{
                              width: '100%',
                              flex: 1,
                              cursor: 'pointer',
                              transition: 'transform 0.2s, box-shadow 0.2s',
                              '&:hover': {
                                transform: 'scale(1.02)',
                                boxShadow: 3
                              },
                              // display: 'flex',
                              flexDirection: 'column',
                              // Если maxCardHeight найден, устанавливаем его:
                              height: maxCardHeight ? maxCardHeight : 'auto'
                            }}
                          >
                            <Box sx={{ position: 'relative', flexGrow: 1 }}>
                              {dish.imageBase64 && (
                                <CardMedia
                                  component="img"
                                  height="140"
                                  image={dish.imageBase64}
                                  alt={dish.name}
                                />
                              )}
                              <Box
                                sx={{
                                  position: 'absolute',
                                  top: 8,
                                  right: 8,
                                  backgroundColor: 'rgba(255,255,255,0.7)',
                                  borderRadius: '50%'
                                }}
                              >
                                <FormControlLabel
                                  value={dish.id}
                                  control={<Radio />}
                                  label=""
                                  sx={{
                                    m: 0,
                                    '& .MuiFormControlLabel-label': { display: 'none' }
                                  }}
                                />
                              </Box>
                            </Box>
                            <CardContent sx={{ flexShrink: 0 }}>
                              <Typography variant="h6" noWrap>
                                {dish.name}
                              </Typography>
                              <Typography variant="body2">
                                {dish.description}
                              </Typography>
                            </CardContent>
                          </Card>
                        </Grid>
                      ))}
                    </Grid>
                  </Box>
                </RadioGroup>
              </FormControl>
            </Grid>
          ))}
        </Grid>
      )}
      <Box sx={{ textAlign: 'center', mt: 2 }}>
        <Typography variant="h6" gutterBottom>Выберите время бронирования:</Typography>
        <FormControl component="fieldset">
          <RadioGroup
            row
            value={selectedTime}
            onChange={(e) => setSelectedTime(e.target.value)}
          >
            <FormControlLabel value="12:00" control={<Radio />} label="12:00" />
            <FormControlLabel value="13:00" control={<Radio />} label="13:00" />
          </RadioGroup>
        </FormControl>
      </Box>
      <Box sx={{ mt: 4, display: 'flex', justifyContent: 'center' }}>
        <Button
          variant="contained"
          color="primary"
          size="large"
          onClick={handleReservation}
          sx={{
            borderRadius: 2,  
            px: 4,            
            py: 1,             
            textTransform: 'none',
            boxShadow: 3      
          }}
        >
          Забронировать
        </Button>       
      </Box>
      <Box sx={{ mt: 4, display: 'flex', justifyContent: 'center' }}>
        <Container maxWidth="lg" sx={{ mt: 4 }}>
          <ReservationList key={refreshKey}/>
        </Container>
      </Box>
      <Box sx={{ mt: 4, display: 'flex', justifyContent: 'center' }}>
        <Button
            variant="outlined"
            color="secondary"
            size="large"
            onClick={() => navigate("../management/reservations")}
            sx={{
              borderRadius: 2,
              px: 4,
              py: 1,
              textTransform: "none",
              boxShadow: 3,
              mt: 2,
            }}
          >
            Посмотреть брони
        </Button> 
      </Box>
    </Container>
  );
};

export default MenuDisplay;
