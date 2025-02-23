import { useEffect, useState, useCallback } from "react";
import { fetchReservations, exportReservationsFile } from "../../services/reservationService"; 
import { fetchDishData } from "../../services/dishService"; 
import { Reservation } from "../../types/Reservation";
import { Dish } from "../../types/Management";

export const useReservations = (date: string) => {
  const [reservations, setReservations] = useState<Reservation[]>([]);
  const [dishes, setDishes] = useState<Dish[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  
  const loadReservations = useCallback(async () => {
    if (!date) return;

    setLoading(true);
    setError(null);

    try {
      const data = await fetchReservations(date); 
      setReservations(data);
    } catch (err) {
      setError((err as Error).message);
    } finally {
      setLoading(false);
    }
  }, [date]);

  const loadDishes = useCallback(async () => {
    try {
      const data = await fetchDishData();
      setDishes(data);
    } catch (err) {
      console.error("Ошибка загрузки блюд:", err);
    }
  }, []);

  useEffect(() => {
    loadReservations();
  }, [loadReservations]);

  useEffect(() => {
    loadDishes();
  }, [loadDishes]);

  const reservations12 = reservations.filter((res) => {
    const resDate = new Date(res.date);
    return resDate.getUTCHours() === 12;
  });
  
  const reservations13 = reservations.filter((res) => {
    const resDate = new Date(res.date);
    return resDate.getUTCHours() === 13;
  });

  const exportReservations = async () => {
    try {
      await exportReservationsFile(date);
    } catch (err) {
      console.error("Ошибка экспорта бронирований:", err);
    }
  };

  return { reservations12, reservations13, dishes, loading, exportReservations, error };
};
