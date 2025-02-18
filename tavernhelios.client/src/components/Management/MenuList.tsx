import React from "react";
import { Card, CardContent, List, Typography, IconButton, Box, Tooltip } from "@mui/material";
import Grid from "@mui/material/Grid2";
import CloseIcon from "@mui/icons-material/Close";
import AddCircleOutlineIcon from "@mui/icons-material/AddCircleOutline";
import DeleteIcon from "@mui/icons-material/Delete";

import { Menu, Dish } from "../../types";
import dishTypes from "../../constants/dishTypes";

interface MenuListProps {
  menuData: Menu[];
  dishesData: { [key: string]: Dish };
  onAddDish: (menuId: string, category: string) => void;
  onRemoveDish: (menuId: string, dishId: string) => void;
  onDeleteMenu: (menuId: string) => void;
}

const MenuList: React.FC<MenuListProps> = ({ menuData, dishesData, onAddDish, onRemoveDish, onDeleteMenu }) => {
  return (
    <Grid container spacing={4} justifyContent="center">
      {menuData.map((menu) => (
        <Grid key={menu.id} size={{ xs: 12, sm: 6, md: 4 }}> 
          <Card>
            <CardContent>
              <Box sx={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                <Typography variant="h6">{menu.name}</Typography>
                <Tooltip title="Удалить меню">
                  <IconButton onClick={() => onDeleteMenu(menu.id)} color="error">
                    <DeleteIcon />
                  </IconButton>
                </Tooltip>
              </Box>

              {dishTypes.map((category) => {
                const categoryDishes = menu.dishes
                  .map((id) => dishesData[id])
                  .filter((dish) => dish?.dishType === category.value);

                return (
                  <Box key={category.value} sx={{ mt: 2 }}>
                    <Typography variant="subtitle1">{category.label}</Typography>
                    <List>
                      {categoryDishes.map((dish) => (
                        <Box key={dish.id} sx={{ display: "flex", alignItems: "center" }}>
                          <Typography>{dish.name}</Typography>
                          <IconButton onClick={() => onRemoveDish(menu.id, dish.id)} color="error">
                            <CloseIcon />
                          </IconButton>
                        </Box>
                      ))}
                      <Box sx={{ display: "flex", alignItems: "center", mt: 1 }}>
                        <Tooltip title="Добавить блюдо">
                          <IconButton color="primary" onClick={() => onAddDish(menu.id, category.label)}>
                            <AddCircleOutlineIcon />
                          </IconButton>
                        </Tooltip>
                        <Typography variant="body2" sx={{ ml: 1 }}>Добавить блюдо</Typography>
                      </Box>
                    </List>
                  </Box>
                );
              })}
            </CardContent>
          </Card>
        </Grid>
      ))}
    </Grid>
  );
};

export default MenuList;
