import { useState, useEffect, useCallback } from "react";
import { fetchMenus, addMenu, updateMenu, deleteMenu } from "../../services/menuService";
import { fetchDish } from "../../services/dishService";
import { Menu, Dish } from "../../types/Management";

export const useMenu = () => {
  const [menuData, setMenuData] = useState<Menu[]>([]);
  const [dishesData, setDishesData] = useState<{ [key: string]: Dish }>({});
  const [loading, setLoading] = useState<boolean>(true);

  const [snackbarMessage, setSnackbarMessage] = useState<string | null>(null);
  const [snackbarOpen, setSnackbarOpen] = useState(false);

  // Управление модальными окнами
  const [isAddModalOpen, setIsAddModalOpen] = useState<boolean>(false);
  const [isCreateMenuModalOpen, setIsCreateMenuModalOpen] = useState<boolean>(false);
  const [availableDishes, setAvailableDishes] = useState<Dish[]>([]);

  const showSnackbar = useCallback((message: string) => {
    setSnackbarMessage(message);
    setSnackbarOpen(true);
  }, []);

  useEffect(() => {
    const loadData = async () => {
      try {
        setLoading(true);
        const [menus, dishes] = await Promise.all([fetchMenus(), fetchDish()]);
        setMenuData(menus);
        setDishesData(Object.fromEntries(dishes.map((dish) => [dish.id, dish])));
      } catch (error) {
        showSnackbar("Ошибка загрузки данных");
      } finally {
        setLoading(false);
      }
    };
    loadData();
  }, [showSnackbar]);

  const createMenu = useCallback(async (name: string) => {
    if (!name.trim()) return;
    try {
      const newMenu = await addMenu(name);
      setMenuData((prev) => [...prev, newMenu]);
      setIsCreateMenuModalOpen(false);
      showSnackbar("Меню успешно создано!");
    } catch (error) {
      showSnackbar("Ошибка при создании меню");
    }
  }, [setMenuData]);
  

  const removeMenu = useCallback(async (menuId: string) => {
    try {
      await deleteMenu(menuId);
      setMenuData(prev => prev.filter(menu => menu.id !== menuId));
      showSnackbar("Меню удалено!");
    } catch (error) {
      showSnackbar("Ошибка при удалении меню");
    }
  }, [setMenuData]);
  
  const addDishToMenu = useCallback(async (menuId: string, dishId: string) => {
    try {
      const menu = menuData.find((m) => m.id === menuId);
      if (!menu) {
        showSnackbar("Меню не найдено");
        return;
      }
  
      const updatedMenu = { ...menu, dishes: [...menu.dishes, dishId] };
      await updateMenu(updatedMenu);
  
      setMenuData((prev) => prev.map((m) => (m.id === menuId ? updatedMenu : m)));
      setIsAddModalOpen(false);
      showSnackbar("Блюдо добавлено в меню!");
    } catch (error) {
      showSnackbar("Ошибка при добавлении блюда в меню");
    }
  }, [menuData]);  
  
  const removeDishFromMenu = useCallback(async (menuId: string, dishId: string) => {
    try {
      const menu = menuData.find((m) => m.id === menuId);
      if (!menu) {
        showSnackbar("Меню не найдено");
        return;
      }
  
      const updatedMenu = { ...menu, dishes: menu.dishes.filter(id => id !== dishId) };
      await updateMenu(updatedMenu);
  
      setMenuData((prev) => prev.map((m) => (m.id === menuId ? updatedMenu : m)));
      showSnackbar("Блюдо удалено!");
    } catch (error) {
      showSnackbar("Ошибка при удалении блюда из меню");
    }
  }, [menuData]);    

  return {
    menuData,
    dishesData,
    loading,
    snackbarMessage,
    setSnackbarMessage,
    snackbarOpen,
    setSnackbarOpen,
    createMenu,
    removeMenu,
    addDishToMenu,
    removeDishFromMenu,
    showSnackbar,
    isAddModalOpen,
    setIsAddModalOpen,
    isCreateMenuModalOpen,
    setIsCreateMenuModalOpen,
    availableDishes, 
    setAvailableDishes 
  };
};

export default useMenu;