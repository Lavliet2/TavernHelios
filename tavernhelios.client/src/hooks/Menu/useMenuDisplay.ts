import { useState, useEffect, useCallback, useMemo, useRef } from "react";

import { fetchTodaySchedule } from "../../services/scheduleService"; 
import { fetchDish } from "../../services/dishService";
import { createReservation, fetchReservations  } from "../../services/reservationService";
import { Menu, Dish } from "../../types/Management";
import { useUser } from "../../contexts/UserContext";
import { useSnackbar } from "../../hooks/useSnackbar";


export const useMenuDisplay = () => {
  const { showSnackbar } = useSnackbar();
  const userContext = useUser();

  const [menu, setMenu] = useState<Menu | null>(null);
  const [dishes, setDishes] = useState<Dish[]>([]);
  const [loadingMenu, setLoadingMenu] = useState<boolean>(true);
  const [loadingDishes, setLoadingDishes] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedDishes, setSelectedDishes] = useState<Record<number, string>>({});
  const [maxCardHeight, setMaxCardHeight] = useState<number | null>(null);
  const [selectedTime, setSelectedTime] = useState<string>("12:00");
  const [refreshKey, setRefreshKey] = useState<number>(0);
  const cardRefs = useRef<HTMLDivElement[]>([]);

  const [selectedSeatNumber, setSelectedSeatNumber] = useState<number | null>(null);
  const [selectedTableName, setSelectedTableName] = useState<string | null>(null);
  const [selectedLayoutId, setSelectedLayoutId] = useState<string | null>(null);
  const [alreadyReserved, setAlreadyReserved] = useState<boolean>(false);
  const [isBooking, setIsBooking] = useState(false);

  

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

  useEffect(() => {
    const checkExistingReservation = async () => {
      const username = userContext?.user?.fullName;
      if (!username) return;
  
      try {
        const today = new Date();
        const formattedDate = today.toISOString().split("T")[0]; 
        const reservations = await fetchReservations(formattedDate);
        const hasReservation = reservations.some(
          (r) => r.personId === username
        );
        setAlreadyReserved(hasReservation);
      } catch (err) {
        console.error("Ошибка проверки брони:", err);
      }
    };
  
    if (userContext?.user?.fullName) {
      checkExistingReservation();
    }
  }, [userContext?.user?.fullName, refreshKey]);
  
  // Загрузка блюд
  useEffect(() => {
    const fetchDishes = async () => {
      if (!menu?.dishes.length) {
        setDishes([]);
        setLoadingDishes(false);
        return;
      }
  
      setLoadingDishes(true);
      try {
        const dishDetails = await fetchDish(false); 
        const filteredDishes = dishDetails.filter(dish => menu.dishes.includes(dish.id));
        setDishes(filteredDishes);
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

  // Бронируем место за столом
  const handleSeatSelect = useCallback((seatNumber: number, tableName: string, layoutId: string) => {
    setSelectedSeatNumber(seatNumber);
    setSelectedTableName(tableName);
    setSelectedLayoutId(layoutId);
  }, []);

  // Создание бронирования
  const handleReservation = useCallback(async () => {
    if (isBooking) return;
    setIsBooking(true);
    const currentUsername = userContext?.user?.fullName;
    if (!currentUsername.trim()) {
      showSnackbar("Введите имя перед бронированием!");
      return;
    }

    const selectedDishIds = Object.values(selectedDishes);
    if (selectedDishIds.length === 0) {
      showSnackbar("Выберите хотя бы одно блюдо!");
      return;
    }

    if (selectedSeatNumber === null || !selectedTableName || !selectedLayoutId) {
      console.log("selectedSeatNumber", selectedSeatNumber, "selectedTableName", selectedTableName, "selectedLayoutId", selectedLayoutId);
      showSnackbar("Пожалуйста, выберите место за столом.");
      return;
    }

    const [hours, minutes] = selectedTime.split(":").map(Number);
    const localDate = new Date();
    localDate.setHours(hours, minutes, 0, 0);
  
    const formattedDate = `${localDate.getFullYear()}-${String(localDate.getMonth() + 1)
      .padStart(2, "0")}-${String(localDate.getDate()).padStart(2, "0")}T${String(localDate.getHours())
      .padStart(2, "0")}:${String(localDate.getMinutes()).padStart(2, "0")}:${String(localDate.getSeconds()).padStart(2, "0")}`;

    const reservationData = {
      personId: currentUsername,
      date: formattedDate,
      dishIds: selectedDishIds,
      seatNumber: selectedSeatNumber,
      tableName: selectedTableName,
      layoutId: selectedLayoutId,
      isDeleted: false
    };

    console.log("Отправляем бронь:", reservationData);

    try {
      await createReservation(reservationData);
      showSnackbar("Бронь успешно создана!", "success");
      setRefreshKey((prev) => prev + 1);
    } catch (error) {
      showSnackbar(`Ошибка: ${(error as Error).message || "Неизвестная ошибка"}`);
    } finally {
      setIsBooking(false);
    }
  }, [
    userContext?.user,
    selectedDishes,
    selectedTime,
    selectedSeatNumber,
    selectedTableName,
    selectedLayoutId
  ]);

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

    selectedSeatNumber,
    selectedTableName,
    selectedLayoutId,
    handleSeatSelect,
    alreadyReserved,
    isBooking,
  };
};
