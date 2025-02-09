import React, { useState, useEffect } from "react";
import { Container, Typography, CircularProgress, TextField } from "@mui/material";
import ReservationGroup from "./ReservationGroup";
import { useReservations } from "../../hooks/Management/useReservations";
import { API_BASE_URL } from "../../config";

interface Dish {
  id: string;
  name: string;
  description: string;
  dishType: number;
}

const ReservationList: React.FC = () => {
  const [selectedDate, setSelectedDate] = useState<string>(new Date().toISOString().split("T")[0]);
  const { reservations12, reservations13, loading, error } = useReservations(selectedDate);
  const [dishes, setDishes] = useState<Dish[]>([]);
  
  useEffect(() => {
    const fetchDishes = async () => {
      try {
        const response = await fetch(`${API_BASE_URL}/api/Dish`);
        if (!response.ok) {
          throw new Error("Ошибка при загрузке блюд");
        }
        const data: Dish[] = await response.json();
        setDishes(data);
      } catch (err) {
        console.error(err);
      }
    };

    fetchDishes();
  }, []);

  return (
    <Container maxWidth="md" sx={{ mt: 4 }}>
      <Typography variant="h4" align="center" gutterBottom>
        Брони на выбранную дату
      </Typography>

      {/* Поле выбора даты */}
      <TextField
        label="Выберите дату"
        type="date"
        value={selectedDate}
        onChange={(e) => setSelectedDate(e.target.value)}
        fullWidth
        sx={{ mb: 3 }}
        InputLabelProps={{ shrink: true }}
      />

      {loading ? (
        <Typography variant="body1" align="center">
          <CircularProgress /> Загрузка броней...
        </Typography>
      ) : error ? (
        <Typography variant="body1" color="error" align="center">
          {error}
        </Typography>
      ) : (
        <>
          <ReservationGroup title="Брони на 12:00" reservations={reservations12} dishes={dishes} />
          <ReservationGroup title="Брони на 13:00" reservations={reservations13} dishes={dishes} />
        </>
      )}
    </Container>
  );
};

export default ReservationList;
