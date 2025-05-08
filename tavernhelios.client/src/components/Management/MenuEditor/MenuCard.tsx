import React, { useState, useEffect } from "react";
import {
  Card,
  CardContent,
  Typography,
  Avatar,
  List,
  ListItem,
  ListItemText,
  Box,
  CircularProgress,
  Collapse,
} from "@mui/material";
import { Menu, Dish } from "../../../types/Management";
import dishTypes from "../../../constants/dishTypes";
import { fetchDishById } from "../../../services/dishService";

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
          setDishes({});
          setLoading(false);
          return;
        }

        const dishResults = await Promise.all(menu.dishes.map(fetchDishById));
        const dishMap = Object.fromEntries(
          dishResults.filter(Boolean).map((dish) => [dish.id, dish])
        );
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
    <Card
      sx={{
        width: "100%",
        borderRadius: 2,
        boxShadow: 3,
        overflow: "hidden",
        transition: "all 0.3s ease",
      }}
    >
      <CardContent>
        <Typography variant="h6" align="center" sx={{ mb: 2 }}>
          {menu.name}
        </Typography>

        <Collapse in={loading} unmountOnExit>
          <Box sx={{ display: "flex", justifyContent: "center", py: 3 }}>
            <CircularProgress size={30} />
          </Box>
        </Collapse>

        <Collapse in={!loading}>
          {dishTypes.map((category) => {
            const categoryDishes = Object.values(dishes).filter(
              (dish) => dish.dishType === category.value
            );

            if (categoryDishes.length === 0) return null;

            return (
              <Box key={category.value} sx={{ mt: 2 }}>
                <Typography variant="subtitle1">{category.label}</Typography>
                <List dense>
                  {categoryDishes.map((dish) => (
                    <ListItem key={dish.id}>
                      <Avatar
                        src={dish.imageBase64}
                        alt={dish.name}
                        sx={{ width: 40, height: 40, mr: 2 }}
                      />
                      <ListItemText primary={dish.name} />
                    </ListItem>
                  ))}
                </List>
              </Box>
            );
          })}
        </Collapse>
      </CardContent>
    </Card>
  );
};

export default MenuCard;
