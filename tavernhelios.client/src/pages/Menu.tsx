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

const dishTypes = [
  { value: 0, label: 'Супы' },
  { value: 1, label: 'Горячее' },
  { value: 2, label: 'Салаты' },
  { value: 3, label: 'Напитки' }
];

const MenuDisplay: React.FC = () => {
  const [menu, setMenu] = useState<Menu | null>(null);
  const [dishes, setDishes] = useState<Dish[]>([]);
  const [loadingMenu, setLoadingMenu] = useState<boolean>(true);
  const [loadingDishes, setLoadingDishes] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedDishes, setSelectedDishes] = useState<Record<number, string>>({});
  const [maxCardHeight, setMaxCardHeight] = useState<number | null>(null);
  // const API_BASE_URL = import.meta.env.VITE_API_URL  || '/api/Menu';
  const API_BASE_URL = "http://tavernhelios-server-service:5040"
  console.log(`API - ${API_BASE_URL}`)

  
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
        if (!response.ok) {
          throw new Error('Ошибка при загрузке меню');
        }
        const data: Menu[] = await response.json();
        if (data && data.length > 0) {
          setMenu(data[0]);
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
              // await fetch(`${API_BASE_URL}`);
              if (!res.ok) {
                throw new Error(`Ошибка при загрузке блюда с id ${dishId}`);
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
    setSelectedDishes(prev => ({ ...prev, [dishType]: dishId }));
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
      <Box sx={{ mt: 4, display: 'flex', justifyContent: 'center' }}>
        <Button
          variant="contained"
          color="primary"
          size="large"
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
    </Container>
  );
};

export default MenuDisplay;
