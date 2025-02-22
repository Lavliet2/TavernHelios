import { useState, useEffect, useCallback, useMemo, useRef } from "react";

import { fetchTodaySchedule } from "../../services/scheduleService"; 
import { fetchDishById } from "../../services/dishService";
import { createReservation } from "../../services/reservationService";
import { Menu, Dish } from "../../types/Management";


export const useMenuDisplay = () => {
  const [menu, setMenu] = useState<Menu | null>(null);
  const [dishes, setDishes] = useState<Dish[]>([]);
  const [loadingMenu, setLoadingMenu] = useState<boolean>(true);
  const [loadingDishes, setLoadingDishes] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedDishes, setSelectedDishes] = useState<Record<number, string>>({});
  const [maxCardHeight, setMaxCardHeight] = useState<number | null>(null);
  const [selectedTime, setSelectedTime] = useState<string>("12:00");
  const [refreshKey, setRefreshKey] = useState<number>(0);
  const username = localStorage.getItem("username") || "";
  const cardRefs = useRef<HTMLDivElement[]>([]);

  useEffect(() => {
    const loadSchedule = async () => {
      setLoadingMenu(true);
      setError(null);
      try {
        const Schedule = await fetchTodaySchedule(); 
        if (Schedule && Schedule.menu) {
          setMenu(Schedule.menu); 
        } else {
          setError("На сегодня нет доступного меню");
        }
      } catch (err) {
        setError((err as Error).message);
      } finally {
        setLoadingMenu(false);
      }
    };
    loadSchedule();
  }, []);
  

  // 📥 Загрузка блюд
  useEffect(() => {
    const fetchDishes = async () => {
      if (!menu?.dishes.length) return;
      setLoadingDishes(true);
      setDishes([]);
      try {
        const dishDetails = await Promise.all(
            menu.dishes.map(async (dishId) => {
              try {
                return await fetchDishById(dishId); 
              } catch (error) {
                console.error(`Ошибка загрузки блюда ${dishId}:`, error);
                return null; 
              }
            })
          );
        setDishes(dishDetails.filter((d) => d !== null));
      } catch (err) {
        console.error("Ошибка загрузки блюд:", err);
        setError((err as Error).message);
        setDishes([]);
      } finally {
        setLoadingDishes(false);
      }
    };
    fetchDishes();
  }, [menu]);

  // Рассчитываем максимальную высоту карточек
  useEffect(() => {
    if (!loadingDishes && cardRefs.current.length > 0) {
      const heights = cardRefs.current.map((el) => el.offsetHeight);
      setMaxCardHeight(Math.max(...heights));
    }
  }, [loadingDishes, dishes]);

  // Оптимизированная группировка блюд
  const groupedDishes = useMemo(() => {
    return dishes.reduce((acc, dish) => {
      if (!acc[dish.dishType]) acc[dish.dishType] = [];
      acc[dish.dishType].push(dish);
      return acc;
    }, {} as Record<number, Dish[]>);
  }, [dishes]);

  // Добавление ref
  const addToCardRefs = useCallback((el: HTMLDivElement | null) => {
    if (el && !cardRefs.current.includes(el)) {
      cardRefs.current.push(el);
    }
  }, []);

  // Изменение выбора блюда
  const handleSelectionChange = useCallback((dishType: number, dishId: string) => {
    setSelectedDishes((prev) => ({
      ...prev,
      [dishType]: dishId,
    }));
  }, []);

  // Создание бронирования
  const handleReservation = useCallback(async () => {
    if (!username.trim()) {
      alert("Введите имя перед бронированием!");
      return;
    }

    const selectedDishIds = Object.values(selectedDishes);
    if (selectedDishIds.length === 0) {
      alert("Выберите хотя бы одно блюдо!");
      return;
    }

    const [hours, minutes] = selectedTime.split(":").map(Number);
    const localDate = new Date();
    localDate.setHours(hours, minutes, 0, 0);
  
    const formattedDate = `${localDate.getFullYear()}-${String(localDate.getMonth() + 1)
      .padStart(2, "0")}-${String(localDate.getDate()).padStart(2, "0")}T${String(localDate.getHours())
      .padStart(2, "0")}:${String(localDate.getMinutes()).padStart(2, "0")}:${String(localDate.getSeconds()).padStart(2, "0")}`;

    const reservationData = {
      personId: username,
      date: formattedDate,
      dishIds: selectedDishIds,
    };

    console.log("Отправляем бронь:", reservationData);

    try {
        await createReservation(reservationData);
        alert("Бронь успешно создана!");
        setRefreshKey((prev) => prev + 1);
      } catch (error) {
        alert(`Ошибка: ${(error as Error).message || "Неизвестная ошибка"}`);
    }
  }, [username, selectedDishes, selectedTime]);

  return {
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
  };
};
