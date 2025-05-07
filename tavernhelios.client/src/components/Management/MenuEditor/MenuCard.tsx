import React, { useState, useEffect } from "react";
import { Card, CardContent, Typography, Avatar, List, ListItem, ListItemText, Box, CircularProgress } from "@mui/material";
import { Menu, Dish } from "../../../types/Management";
import dishTypes from "../../../constants/dishTypes";
import { fetchDishById } from "../../../services/dishService"; // Подключаем сервис

interface MenuCardProps {
  menu: Menu;
}

const MenuCard: React.FC<MenuCardProps> = ({ menu }) => {
  const [dishes, setDishes] = useState<{ [key: string]: Dish }>({});
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadDishes = async () => {
      setLoading(true);
      try {
        if (!menu.dishes || menu.dishes.length === 0) {
          console.warn("Нет блюд для загрузки.");
          setDishes({});
          setLoading(false);
          return;
        }

        const dishPromises = menu.dishes.map((id) => {
          return fetchDishById(id);
        });

        const dishResults = await Promise.all(dishPromises);
        const dishMap = Object.fromEntries(dishResults.filter(dish => dish).map((dish) => [dish.id, dish]));
        setDishes(dishMap);
      } catch (error) {
        console.error("Ошибка загрузки блюд:", error);
      } finally {
        setLoading(false);
      }
    };

    loadDishes();
  }, [menu.dishes]);

  return (
    <Card sx={{ maxWidth: 400, p: 2, borderRadius: 2, boxShadow: 3 }}>
      <CardContent>
        <Typography variant="h6" align="center" sx={{ mb: 2 }}>
          {menu.name}
        </Typography>

        {loading ? (
          <Box sx={{ display: "flex", justifyContent: "center", mt: 2 }}>
            <CircularProgress size={30} />
          </Box>
        ) : (
          dishTypes.map((category) => {
            const categoryDishes = Object.values(dishes).filter((dish) => dish && dish.dishType === category.value);

            if (categoryDishes.length === 0) return null;

            return (
              <Box key={category.value} sx={{ mt: 2 }}>
                <Typography variant="subtitle1">{category.label}</Typography>
                <List>
                  {categoryDishes.map((dish) => (
                    <ListItem key={dish.id} sx={{ display: "flex", alignItems: "center" }}>
                      <Avatar
                        src={dish.imageBase64}
                        alt={dish.name}
                        sx={{ width: 40, height: 40, mr: 2, borderRadius: "8px" }}
                      />
                      <ListItemText primary={dish.name} />
                    </ListItem>
                  ))}
                </List>
              </Box>
            );
          })
        )}
      </CardContent>
    </Card>
  );
};

export default MenuCard;
