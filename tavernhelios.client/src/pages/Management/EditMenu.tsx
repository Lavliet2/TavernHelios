import React, { useState, useCallback } from "react";
import { useTranslation } from 'react-i18next';
import { Container, Typography, CircularProgress, Card,
  CardContent, List, ListItem, ListItemText, ListItemAvatar,
  Avatar, IconButton, Box, Tooltip } from "@mui/material";
import Grid from "@mui/material/Grid2";
import Snackbar from "@mui/material/Snackbar";
import Alert from "@mui/material/Alert";
import CloseIcon from "@mui/icons-material/Close";
import AddCircleOutlineIcon from "@mui/icons-material/AddCircleOutline";
import DeleteIcon from "@mui/icons-material/Delete";

import dishTypes from "../../constants/dishTypes";
import { useMenu } from "../../hooks/Management/useMenu";

import MenuAddDishModal from "../../components/Management/MenuEditor/MenuAddDishModal";
import MenuCreateModal from "../../components/Management/MenuEditor/MenuCreateModal";


const EditMenu: React.FC = () => {
  const { t } = useTranslation();
  const {
    menuData, dishesData, loading, snackbarMessage, createMenu, 
    removeMenu, addDishToMenu, removeDishFromMenu, isAddModalOpen,
    setIsAddModalOpen, isCreateMenuModalOpen, setIsCreateMenuModalOpen,
    snackbarOpen, setSnackbarOpen, setSnackbarMessage, availableDishes, setAvailableDishes
  } = useMenu();

  const [error] = useState<string | null>(null);
  const [selectedMenu, setSelectedMenu] = useState<string | null>(null);
  
  // Открытие модального окна и фильтрация доступных блюд
  const handleOpenAddDishModal = useCallback((menuId: string, category: string) => {
    setSelectedMenu(menuId);
  
    const menu = menuData.find((m) => m.id === menuId);
    if (!menu) return;
  
    const available = Object.values(dishesData).filter(
      (dish) =>
        dishTypes.find((type) => type.value === dish.dishType)?.label === category &&
        !menu.dishes.includes(dish.id)
    );
  
    setAvailableDishes(available);
    setIsAddModalOpen(true);
  }, [menuData, dishesData, setIsAddModalOpen]);

  if (loading) {
    return (
      <Container sx={{ mt: 4, display: "flex", flexDirection: "column", alignItems: "center" }}>
        <CircularProgress />
        <Typography variant="body1" sx={{ mt: 2 }}> { t('common.loadingData') }</Typography>
      </Container>
    );
  }

  if (error) {
    return (
      <Container sx={{ mt: 4, textAlign: "center" }}>
        <Typography variant="body1" color="error">
          {error}
        </Typography>
      </Container>
    );
  }

  return (
    <Container sx={{ mt: 4 }}>
      <Typography variant="h4" color="primary" align="center" sx={{ mb: 4 }}>
        { t('editMenu.title') }
      </Typography>

      <Grid container spacing={4} justifyContent="center">
        {menuData.map((menu) => (
          <Grid key={menu.id} size={{ xs: 12, sm: 6, md: 4 }}> 
            <Card>
              <CardContent>
              <Box sx={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                  <Typography variant="h6">{menu.name}</Typography>
                  <Tooltip title={ t('common.delete') + ' ' + t('editMenu.menu')}>
                    <IconButton onClick={() => removeMenu(menu.id)} color="error">
                      <DeleteIcon />
                    </IconButton>
                  </Tooltip>
                </Box>
                {dishTypes.map((category) => {
                  const categoryDishes = menu.dishes
                    .map((id) => dishesData[id])
                    .filter((dish) => dish?.dishType === category.value);

                  // if (categoryDishes.length === 0) return null;

                  return (
                    <Box key={category.value} sx={{ mt: 2 }}>
                      <Typography variant="subtitle1">{category.label}</Typography>
                      <List>
                        {categoryDishes.map((dish) => (
                          <ListItem key={dish.id} 
                            secondaryAction={
                              <IconButton 
                                edge="end" 
                                aria-label="delete"
                                onClick={() => removeDishFromMenu(menu.id, dish.id)}
                                sx={{ color: "error.main", fontSize: 16 }}
                              >
                                <CloseIcon fontSize="small" />
                              </IconButton>
                            }
                          >
                            <ListItemAvatar>
                              <Avatar src={dish.imageBase64 || ""} alt={dish.name} />
                            </ListItemAvatar>
                            <ListItemText primary={dish.name} />
                          </ListItem>
                        ))}
                        {/* Кнопка добавления */}
                        <ListItem>
                          <Tooltip title={t("editMenu.addDish")}>
                            <IconButton
                              color="primary"
                              onClick={() => handleOpenAddDishModal(menu.id, category.label)}
                            >
                              <AddCircleOutlineIcon />
                            </IconButton>
                          </Tooltip>
                          <Typography variant="body2" color="text.secondary" sx={{ ml: 1 }}>
                            {t("editMenu.addDish")}
                          </Typography>
                        </ListItem>
                      </List>
                    </Box>
                  );
                })}
              </CardContent>
            </Card>
          </Grid>
        ))}
        {/* Карточка для создания нового меню */}
        <Grid size={{ xs: 12, sm: 6, md: 4 }}>
          <Card
            sx={{
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              height: 150,
              cursor: "pointer",
              transition: "0.3s",
              transform: "scale(1)",
              backgroundColor: "rgba(206, 14, 45, 0.05)",
              "&:hover": {
                transform: "scale(1.05)",
                boxShadow: "0px 0px 15px rgba(206, 14, 45, 0.3)",
              },
            }}
            onClick={() => setIsCreateMenuModalOpen(true)}
          >
            <AddCircleOutlineIcon color="primary" sx={{ fontSize: 50 }} />
          </Card>
        </Grid>        
      </Grid>
      <MenuCreateModal open={isCreateMenuModalOpen} onClose={() => setIsCreateMenuModalOpen(false)} onCreate={createMenu} />
      <MenuAddDishModal 
        open={isAddModalOpen} 
        onClose={() => {
          setIsAddModalOpen(false);
          setAvailableDishes([]);
        }} 
        availableDishes={availableDishes} 
        onSelectDish={(dishId) => selectedMenu && addDishToMenu(selectedMenu, dishId)} 
      />
        <Snackbar 
          open={snackbarOpen} 
          autoHideDuration={3000} 
          onClose={() => {
            setSnackbarOpen(false);
            setSnackbarMessage(null);
          }}
        >
        <Alert 
          severity={snackbarMessage?.includes("Ошибка") ? "error" : "success"} 
          onClose={() => setSnackbarOpen(false)}
        >
          {snackbarMessage}
        </Alert>
      </Snackbar>
    </Container>
    
  );
};

export default EditMenu;
