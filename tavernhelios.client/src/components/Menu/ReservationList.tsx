import React, { useState, useCallback } from "react";
import { Container, Typography, CircularProgress, TextField } from "@mui/material";
import ReservationGroup from "./ReservationGroup";
import { useReservations } from "../../hooks/Menu/useReservations";

const getUTCDateString = (date: Date) => {
  return date.toISOString().split("T")[0];
};

const ReservationList: React.FC = () => {
  const [selectedDate, setSelectedDate] = useState<string>(getUTCDateString(new Date()));
  const { reservations12, reservations13, dishes, loading, error } = useReservations(selectedDate);

  // Оптимизированный обработчик изменения даты
  const handleDateChange = useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => setSelectedDate(e.target.value),
    []
  );

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
        onChange={handleDateChange} 
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
