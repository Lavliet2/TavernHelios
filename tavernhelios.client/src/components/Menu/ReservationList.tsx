import React, { useState, useCallback } from "react";
import { Container, Typography, CircularProgress, TextField, Button, Box } from "@mui/material";
import ReservationGroup from "./ReservationGroup";
import { useReservations } from "../../hooks/Menu/useReservations";

const getUTCDateString = (date: Date) => {
  return date.toISOString().split("T")[0];
};

const ReservationList: React.FC = () => {
  const [selectedDate, setSelectedDate] = useState<string>(getUTCDateString(new Date()));
  const { reservations12, reservations13, dishes, loading, exportReservations, error } = useReservations(selectedDate);
  const [loadingPdf, setLoadingPdf] = useState(false);
  const [loadingExcel, setLoadingExcel] = useState(false);

  const handleDateChange = useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => setSelectedDate(e.target.value),
    []
  );

  const handleExport = async (format: "pdf" | "excel") => {
    const setLoading = format === "pdf" ? setLoadingPdf : setLoadingExcel;
    try {
      setLoading(true);
      await exportReservations(format);
    } catch (e) {
      console.error("Ошибка экспорта:", e);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Container maxWidth="md" sx={{ mt: 4 }}>
      <Typography variant="h4" align="center" gutterBottom>
        Брони на выбранную дату
      </Typography>

      <TextField
        label="Выберите дату"
        type="date"
        value={selectedDate}
        onChange={handleDateChange}
        fullWidth
        sx={{ mb: 3 }}
        slotProps={{ inputLabel: { shrink: true } }}
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

      <Box sx={{ mt: 4, display: 'flex', flexDirection: 'column', alignItems: 'center', gap: 2 }}>
        <Typography variant="h6">Скачать отчёт</Typography>
        <Box sx={{ display: 'flex', gap: 2 }}>
          <Button
            variant="contained"
            color="primary"
            onClick={() => handleExport("pdf")}
            size="small"
            sx={{ px: 2, py: 1, minWidth: 120 }}
            disabled={loadingPdf}
          >
            {loadingPdf ? "⏳ PDF..." : "PDF"}
          </Button>

          <Button
            variant="contained"
            color="primary"
            onClick={() => handleExport("excel")}
            size="small"
            sx={{ px: 2, py: 1, minWidth: 120 }}
            disabled={loadingExcel}
          >
            {loadingExcel ? "⏳ Excel..." : "Excel"}
          </Button>
        </Box>
      </Box>
    </Container>
  );
};

export default ReservationList;
