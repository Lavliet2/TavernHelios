import React, { useEffect, useState } from "react";
import { Container, Typography, CircularProgress, Card, TextField,
  CardContent, List, ListItem, ListItemText, ListItemAvatar,
  Avatar, IconButton, Modal, Box, Tooltip, Button} from "@mui/material";
import Grid from "@mui/material/Grid2";
import Snackbar from "@mui/material/Snackbar";
import Alert from "@mui/material/Alert";
import CloseIcon from "@mui/icons-material/Close";
import AddCircleOutlineIcon from "@mui/icons-material/AddCircleOutline";
import DeleteIcon from "@mui/icons-material/Delete";

import dishTypes from "../../constants/dishTypes";
import { Menu, Dish } from "../../types"

import { fetchMenus, addMenu, updateMenu, deleteMenu } from "../../services/menuService";
import { fetchDishData } from "../../services/dishService";


const EditMenu: React.FC = () => {
  const [menuData, setMenuData] = useState<Menu[]>([]);
  const [dishesData, setDishesData] = useState<{ [key: string]: Dish }>({});
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedMenu, setSelectedMenu] = useState<string | null>(null);
  const [availableDishes, setAvailableDishes] = useState<Dish[]>([]);
  const [isAddModalOpen, setIsAddModalOpen] = useState<boolean>(false);
  const [isCreateMenuModalOpen, setIsCreateMenuModalOpen] = useState<boolean>(false);
  const [newMenuName, setNewMenuName] = useState<string>("");
  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState("");
 
  const showSnackbar = (message: string) => {
    setSnackbarMessage(message);
    setSnackbarOpen(true);
  };

  useEffect(() => {
    const loadData = async () => {
      try {
        setLoading(true);
        const [menus, dishes] = await Promise.all([fetchMenus(), fetchDishData()]);
        setMenuData(menus);
        const dishesMap = Object.fromEntries(dishes.map((dish) => [dish.id, dish]));
        setDishesData(dishesMap);
        // setDishesData(dishes.reduce((acc, dish) => ({ ...acc, [dish.id]: dish }), {}));
      } catch (error) {
        showSnackbar("Ошибка загрузки данных");;
        // setError("Ошибка загрузки данных");
      } finally {
        setLoading(false);
      }
    };

    loadData();
  }, []);



  // Создание нового меню
  const handleCreateMenu = async () => {
    if (!newMenuName.trim()) return;

    try {
      const createdMenu = await addMenu(newMenuName);
      setMenuData((prev) => [...prev, createdMenu]);
      setIsCreateMenuModalOpen(false);
      setNewMenuName("");
    } catch (error) {
      showSnackbar("Ошибка при создании меню:");
      console.error("Ошибка при создании меню:", error);
    }
  };

  // Удаление меню
  const handleDeleteMenu = async (menuId: string) => {
    if (!menuId) return;
    try {
      await deleteMenu(menuId);
      setMenuData((prev) => prev.filter((menu) => menu.id !== menuId));
    } catch (error) {
      showSnackbar("Ошибка при удалении меню:");
      console.error("Ошибка при удалении меню:", error);
    }
  };

  // Открытие модального окна и фильтрация доступных блюд
  const handleOpenAddDishModal = (menuId: string, category: string) => {
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
  };

  const updateMenuState = (updatedMenu: Menu) => {
    setMenuData((prev) => prev.map((m) => (m.id === updatedMenu.id ? updatedMenu : m)));
  };  
  
  // Добавление блюда в меню
  const handleAddDishToMenu = async (dishId: string) => {
    if (!selectedMenu) return;
  
    try {
      const menu = menuData.find((m) => m.id === selectedMenu);
      if (!menu) throw new Error("Меню не найдено");
  
      const updatedMenu = {
        ...menu,
        dishes: [...menu.dishes, dishId],
      };
  
      await updateMenu(updatedMenu);
      updateMenuState(updatedMenu);
      setIsAddModalOpen(false);
    } catch (error) {
      showSnackbar("Ошибка при добавлении блюда в меню:");
      console.error("Ошибка при добавлении блюда в меню:", error);
    }
  };

  // Удаление блюда из меню
  const handleRemoveDishFromMenu = async (menuId: string, dishId: string) => {
    try {
      const menu = menuData.find((m) => m.id === menuId);
      if (!menu) throw new Error("Меню не найдено");

      const updatedMenu = {
        ...menu,
        dishes: menu.dishes.filter((id) => id !== dishId),
      };

      await updateMenu(updatedMenu);
      updateMenuState(updatedMenu);
    } catch (error) {
      showSnackbar("Ошибка при удалении блюда из меню:");
      console.error("Ошибка при удалении блюда из меню:", error);
    }
  };

  if (loading) {
    return (
      <Container sx={{ mt: 4, display: "flex", flexDirection: "column", alignItems: "center" }}>
        <CircularProgress />
        <Typography variant="body1" sx={{ mt: 2 }}>Загрузка данных...</Typography>
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
        Редактирование меню
      </Typography>

      <Grid container spacing={4} justifyContent="center">
        {menuData.map((menu) => (
          <Grid key={menu.id} size={{ xs: 12, sm: 6, md: 4 }}> 
            <Card>
              <CardContent>
              <Box sx={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                  <Typography variant="h6">{menu.name}</Typography>
                  <Tooltip title="Удалить меню">
                    <IconButton onClick={() => handleDeleteMenu(menu.id)} color="error">
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
                                onClick={() => handleRemoveDishFromMenu(menu.id, dish.id)}
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
                          <Tooltip title="Добавить блюдо">
                            <IconButton
                              color="primary"
                              onClick={() => handleOpenAddDishModal(menu.id, category.label)}
                            >
                              <AddCircleOutlineIcon />
                            </IconButton>
                          </Tooltip>
                          <Typography variant="body2" color="text.secondary" sx={{ ml: 1 }}>
                            Добавить блюдо
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

      {/* Модальное окно для добавления блюда */}
      <Modal open={isAddModalOpen} onClose={() => setIsAddModalOpen(false)}>
        <Box
          sx={{
            position: "absolute",
            top: "50%",
            left: "50%",
            transform: "translate(-50%, -50%)",
            bgcolor: "white",
            p: 4,
            borderRadius: 2,
            width: 400,
          }}
        >
          <Typography variant="h6" sx={{ mb: 2 }}>Добавить блюдо</Typography>

          {availableDishes.length > 0 ? (
            <List>
              {availableDishes.map((dish) => (
                <ListItem
                  key={dish.id}
                  component="div"
                  onClick={() => handleAddDishToMenu(dish.id)}
                  sx={{ cursor: "pointer", "&:hover": { bgcolor: "action.hover" } }}
                >
                  <ListItemAvatar>
                    <Avatar src={dish.imageBase64 || ""} alt={dish.name} />
                  </ListItemAvatar>
                  <ListItemText primary={dish.name} />
                </ListItem>
              ))}
            </List>
          ) : (
            <Typography variant="body2" color="text.secondary">
              Нет доступных блюд для добавления
            </Typography>
          )}

          <Box sx={{ display: "flex", justifyContent: "flex-end", mt: 2 }}>
            <Button variant="contained" color="secondary" onClick={() => setIsAddModalOpen(false)}>
              Отмена
            </Button>
          </Box>
        </Box>
      </Modal>
      {/* Модальное окно для создания нового меню */}
      <Modal open={isCreateMenuModalOpen} onClose={() => setIsCreateMenuModalOpen(false)}>
        <Box
          sx={{
            position: "absolute",
            top: "50%",
            left: "50%",
            transform: "translate(-50%, -50%)",
            bgcolor: "white",
            p: 4,
            borderRadius: 2,
            width: 400,
          }}
        >
          <Typography variant="h6" sx={{ mb: 2 }}>Создать новое меню</Typography>
          <TextField
            label="Название меню"
            fullWidth
            variant="outlined"
            value={newMenuName}
            onChange={(e) => setNewMenuName(e.target.value)}
            sx={{ mb: 2 }}
          />
          <Box sx={{ display: "flex", justifyContent: "flex-end" }}>
            <Button variant="contained" onClick={handleCreateMenu} color="primary" sx={{ mr: 2 }}>
              Создать
            </Button>
            <Button variant="contained" color="secondary" onClick={() => setIsCreateMenuModalOpen(false)}>
              Отмена
            </Button>
          </Box>
        </Box>
      </Modal>      
      <Snackbar open={snackbarOpen} autoHideDuration={3000} onClose={() => setSnackbarOpen(false)}>
        <Alert severity="error" onClose={() => setSnackbarOpen(false)}>
          {snackbarMessage}
        </Alert>
      </Snackbar>
    </Container>
    
  );
};

export default EditMenu;
