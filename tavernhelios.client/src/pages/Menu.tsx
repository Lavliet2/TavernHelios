import React, { useEffect, useState, useRef } from 'react';
import {
  Container,
  Typography,
  Card,
  CardMedia,
  CardContent,
  CircularProgress,
  FormControl,
  RadioGroup,
  FormControlLabel,
  Radio,
  Box,
  Button
} from '@mui/material';
import Grid from '@mui/material/Grid2';
import { useNavigate } from "react-router-dom"; //MAV delete
import { API_BASE_URL } from "../config";

import ReservationList from "../components/Management/ReservationList";
import dishTypes from "../constants//dishTypes";
// import { API_BASE_URL } from "../../vite.config";

interface Menu {
  id: string;
  name: string;
  dishes: string[];
}

interface Dish {
  id: string;
  name: string;
  description: string;
  dishType: number;
  imageBase64: string;
}


const MenuDisplay: React.FC = () => {
  const [menu, setMenu] = useState<Menu | null>(null);
  const [dishes, setDishes] = useState<Dish[]>([]);
  const [loadingMenu, setLoadingMenu] = useState<boolean>(true);
  const [loadingDishes, setLoadingDishes] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedDishes, setSelectedDishes] = useState<Record<number, string>>({});
  const [maxCardHeight, setMaxCardHeight] = useState<number | null>(null);
  const [username] = useState<string>(() => localStorage.getItem("username") || ""); //setUsername
  const [selectedTime, setSelectedTime] = useState<string>("12:00");
  const [refreshKey, setRefreshKey] = useState<number>(0);
  const navigate = useNavigate(); //MAV delete
  console.log(`User - ${username}`)
  console.log(`API - ${API_BASE_URL}`)

  const handleReservation = async () => {
    if (!username.trim()) {
      alert("Введите имя перед бронированием!");
      return;
    }
  
    const selectedDishIds = Object.values(selectedDishes);
    if (selectedDishIds.length === 0) {
      alert("Выберите хотя бы одно блюдо!");
      return;
    }
  
    const today = new Date();
    const [hours, minutes] = selectedTime.split(":").map(Number);
    today.setHours(hours, minutes, 0, 0);

    const reservationData = {
      personId: username, 
      date: today.toISOString(),
      dishIds: selectedDishIds
    };

    console.log("Отправляем бронь:", reservationData);
  
    try {
      const response = await fetch(`${API_BASE_URL}/api/Reservation`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(reservationData),
      });
  
      if (!response.ok) {
        throw new Error("Ошибка при создании брони");
      }
  
      alert("Бронь успешно создана!");
      setRefreshKey((prev) => prev + 1);
    } catch (error) {
      alert(`Ошибка: ${(error as Error).message}`);
    }
  };

  // Массив для хранения ref карточек
  const cardRefs = useRef<HTMLDivElement[]>([]);

  // Функция для добавления ref в массив
  const addToCardRefs = (el: HTMLDivElement | null) => {
    if (el && !cardRefs.current.includes(el)) {
      cardRefs.current.push(el);
    }
  };

  useEffect(() => {
    const fetchMenu = async () => {
      try {
        // const response = await fetch('https://localhost:32789/api/Menu');
        const response = await fetch(`${API_BASE_URL}/api/Menu`);
        console.log(`fetchMenu: ${API_BASE_URL}`, response)
        if (!response.ok) {
          throw new Error('Ошибка при загрузке меню');
        }
        const data: Menu[] = await response.json();
        if (data && data.length > 0) {
          setMenu(data[0]);
          console.log(`fetchMenu data: - ${API_BASE_URL}`, data)
        } else {
          setError('Меню не найдено');
        }
      } catch (err) {
        setError((err as Error).message);
      } finally {
        setLoadingMenu(false);
      }
    };
    fetchMenu();
  }, []);

  useEffect(() => {
    const fetchDishes = async () => {
      if (menu) {
        try {
          const dishDetails: Dish[] = await Promise.all(
            menu.dishes.map(async (dishId) => {
              // const res = await fetch(`https://localhost:32789/api/Dish/${dishId}`);
              const res = await fetch(`${API_BASE_URL}/api/Dish/${dishId}`);
              console.log(`fetchDish: ${dishId} - ${API_BASE_URL}`, res)
              // await fetch(`${API_BASE_URL}`);
              if (!res.ok) {                
                // TODO MAV если мы из блюд удаляем сущность и 
                // если она была забронирована пользователем то ошибка вылетает. 
                // Возможно на бэке надо и у пользователя удалять из списка это блюдо
                // throw new Error(`Ошибка при загрузке блюда с id ${dishId}`);
              }
              return res.json();
            })
          );
          setDishes(dishDetails);
        } catch (err) {
          console.error(err);
          setError((err as Error).message);
        } finally {
          setLoadingDishes(false);
        }
      }
    };
    fetchDishes();
  }, [menu]);

  // После завершения загрузки блюд вычисляем максимальную высоту карточек
  useEffect(() => {
    if (!loadingDishes && cardRefs.current.length > 0) {
      const heights = cardRefs.current.map(el => el.offsetHeight);
      const maxHeight = Math.max(...heights);
      setMaxCardHeight(maxHeight);
    }
  }, [loadingDishes, dishes]);

  const handleSelectionChange = (dishType: number, dishId: string) => {
    setSelectedDishes(prev => {
      const updatedSelection = { ...prev, [dishType]: dishId };
      console.log("Выбранные блюда:", updatedSelection);
      return updatedSelection;
    });
  };

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
        <Typography variant="body1" color="error">
          {error}
        </Typography>
      </Container>
    );
  }

  const groupedDishes: Record<number, Dish[]> = {};
  dishes.forEach(dish => {
    if (!groupedDishes[dish.dishType]) {
      groupedDishes[dish.dishType] = [];
    }
    groupedDishes[dish.dishType].push(dish);
  });

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
        <Grid container spacing={2} sx={{ alignItems: 'stretch', width: '100%' }}>
          {dishTypes.map(type => (
            <Grid key={type.value} size={{ xs: 12, sm: 6 }}>
              <Typography 
                variant="h5" 
                gutterBottom
                sx={{
                  borderBottom: 1.5,
                }}>
                {type.label}
              </Typography>
              <FormControl fullWidth component="fieldset">
                <RadioGroup
                  row
                  value={selectedDishes[type.value] || ''}
                  onChange={(e) => handleSelectionChange(type.value, e.target.value)}
                >
                  <Box sx={{ width: '100%' }}>
                    <Grid container spacing={2} sx={{ width: '100%' }}>
                      {groupedDishes[type.value]?.map(dish => (
                        <Grid key={dish.id} size={{ xs: 12, sm: 6 }} sx={{ display: 'flex' }}>
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
