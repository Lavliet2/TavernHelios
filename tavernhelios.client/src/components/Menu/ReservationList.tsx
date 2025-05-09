import React, { useState, useCallback } from "react";
import { Container, Typography, CircularProgress, TextField, Button, Box } from "@mui/material";
import DownloadIcon from '@mui/icons-material/Download';
import HourglassTopIcon from '@mui/icons-material/HourglassTop';

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
      <Box
        sx={{
          mt: 4,
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          gap: 2,
          backgroundColor: '#f0f4f8',
          p: 3,
          borderRadius: 2,
          boxShadow: 3,
          border: '1px solid #ddd',
          // maxWidth: 400,
          mx: 'auto',
        }}
      >
        <Typography
          variant="h6"
          sx={{ color: 'primary.main', fontWeight: 600 }}
        >
          📥 Скачать отчёт
        </Typography>

        <Box sx={{ display: 'flex', gap: 2, justifyContent: 'center' }}>
          <Button
            variant="contained"
            color="primary"
            onClick={() => handleExport("pdf")}
            size="medium"
            aria-label="Экспорт в pdf"
            startIcon={loadingPdf ? <HourglassTopIcon /> : <DownloadIcon />}
            disabled={loadingPdf || (reservations12.length === 0 && reservations13.length === 0)}
            sx={{
              px: 3,
              py: 1,
              minWidth: 130,
              textTransform: 'none',
              fontWeight: 'bold',
            }}
            // disabled={loadingPdf}
          >
            PDF
            {/* {loadingPdf ? "⏳ PDF..." : "PDF"} */}
          </Button>

          <Button
            variant="contained"
            color="primary"
            onClick={() => handleExport("excel")}
            size="medium"
            aria-label="Экспорт в excel"
            startIcon={loadingExcel ? <HourglassTopIcon /> : <DownloadIcon />}
            disabled={loadingExcel || (reservations12.length === 0 && reservations13.length === 0)}
            sx={{
              px: 3,
              py: 1,
              minWidth: 130,
              textTransform: 'none',
              fontWeight: 'bold',
            }}
          >
            Excel
          </Button>
        </Box>
      </Box>
    </Container>
  );
};

export default ReservationList;
