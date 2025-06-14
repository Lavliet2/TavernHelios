import { useEffect, useState, useCallback, useRef  } from "react";
import * as signalR from "@microsoft/signalr";
import { fetchReservations, exportReservationsFile } from "../../services/reservationService"; 
import { fetchDish } from "../../services/dishService"; 
import { Reservation } from "../../types/Reservation";
import { Dish } from "../../types/Management";
import { useSnackbar } from "../useSnackbar";
import { API_BASE_URL } from "../../config";

export const useReservations = (date: string) => {
  const [reservations, setReservations] = useState<Reservation[]>([]);
  const [dishes, setDishes] = useState<Dish[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const connectionRef = useRef<signalR.HubConnection | null>(null);

  const { showSnackbar } = useSnackbar();
  
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
      const data = await fetchDish();
      setDishes(data);
    } catch (err) {
      console.error("Ошибка загрузки блюд:", err);
    }
  }, []);

    useEffect(() => {
    if (connectionRef.current) return;

    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`${API_BASE_URL}/hubs/reservations`)
      .withAutomaticReconnect()
      .build();

    connection.start()
      .then(() => {
        console.log("SignalR connected");

        connection.on("ReservationCreated", (reservation: Reservation) => {
          console.log("📩 ReservationCreated received", reservation);
          showSnackbar(`🔔 ${reservation.personId} сделал(а) бронирование`, "info");
          loadReservations();
        });
      })
      .catch((err) => console.error("SignalR connection error:", err));

    connectionRef.current = connection;

    return () => {
      connection.stop();
      connectionRef.current = null;
    };
  }, [loadReservations, showSnackbar]);

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

  const exportReservations = async (format: "pdf" | "excel" = "pdf") => {
    try {
      await exportReservationsFile(date, format);
    } catch (err) {
      console.error("Ошибка экспорта бронирований:", err);
    }
  };

  return { reservations12, reservations13, dishes, loading, exportReservations, error };
};
