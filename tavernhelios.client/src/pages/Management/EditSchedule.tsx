import React, { useState } from "react";
import { Container, Typography, Box, Button, Snackbar, Alert  } from "@mui/material";
import monthNames from "../../constants/monthNames"
import useSchedule from "../../hooks/Management/useSchedule";
import useMenu from "../../hooks/Management/useMenu";
import CalendarTable from "../../components/Management/CalendarTable";
import MenuAddScheduleModal from "../../components/Management/MenuAddScheduleModal";


const EditSchedule: React.FC = () => {
  const {
    getMonthDays,
    scheduleData,
    selectedDates,
    currentMonth,
    currentYear,
    setCurrentMonth,
    toggleDateSelection,
    isModalOpen,
    setIsModalOpen,
    handleAddMenuToSchedule,
    handleDeleteSchedule,
    selectedMenu,
    setSelectedMenu,
    snackbarMessage,
    snackbarOpen,
    setSnackbarOpen
  } = useSchedule();

  const { menuData } = useMenu();
  const [isMouseDown, setIsMouseDown] = useState(false);

  const handleMouseDown = (date: string) => {
    setIsMouseDown(true);
    toggleDateSelection(date);
  };

  const handleMouseEnter = (date: string) => {
    if (isMouseDown) {
      toggleDateSelection(date);
    }
  };

  const handleMouseUp = () => {
    setIsMouseDown(false);
  };

  // const getMonthDays = () => {
  //   const firstDayIndex = new Date(currentYear, currentMonth, 1).getDay();
  //   const daysInMonth = new Date(currentYear, currentMonth + 1, 0).getDate();
  
  //   const days = [];
  //   const offset = firstDayIndex === 0 ? 6 : firstDayIndex - 1;
  
  //   for (let i = 0; i < offset; i++) {
  //     days.push(null);
  //   }
  
  //   for (let i = 1; i <= daysInMonth; i++) {
  //     const dateObj = new Date(currentYear, currentMonth, i);
  //     const localDate = dateObj.toLocaleDateString("sv-SE"); 
  
  //     days.push({
  //       day: i,
  //       date: localDate,
  //       isWeekend: dateObj.getDay() === 0 || dateObj.getDay() === 6,
  //       isToday: new Date().toLocaleDateString("sv-SE") === localDate,
  //       hasSchedule: scheduleData.some((s) => s.dateTime.startsWith(localDate)),
  //     });
  //   }
  
  //   return days;
  // };
  
  return (
    <Container sx={{ mt: 4 }} onMouseUp={handleMouseUp}>
      <Typography variant="h4" color="primary" align="center" sx={{ mb: 4 }}>
        Управление расписанием
      </Typography>

      <Box sx={{ display: "flex", justifyContent: "space-between", alignItems: "center", mb: 2 }}>
        <Button onClick={() => setCurrentMonth((m) => (m === 0 ? 11 : m - 1))}>
          {"<"} {monthNames[currentMonth === 0 ? 11 : currentMonth - 1]}
        </Button>
        <Typography variant="h6">
          {monthNames[currentMonth]} {currentYear}
        </Typography>
        <Button onClick={() => setCurrentMonth((m) => (m === 11 ? 0 : m + 1))}>
          {monthNames[currentMonth === 11 ? 0 : currentMonth + 1]} {">"}
        </Button>
      </Box>
      <CalendarTable
        days={getMonthDays()}
        selectedDates={selectedDates}
        toggleDateSelection={toggleDateSelection}
        handleMouseDown={handleMouseDown}
        handleMouseEnter={handleMouseEnter}
      />
      <Box sx={{ textAlign: "center", mt: 3 }}>
        <Button 
          variant="contained" 
          color="success" 
          sx={{ mx: 1 }} 
          onClick={() => setIsModalOpen(true)}
          disabled={
            selectedDates.length === 0 || 
            selectedDates.some(date => scheduleData.some(s => s.dateTime.startsWith(date))) 
          }
        >
          Добавить меню
        </Button>
        <Button 
          variant="contained" 
          color="error" 
          sx={{ mx: 1 }} 
          onClick={handleDeleteSchedule}
          disabled={
            selectedDates.length === 0 || 
            !selectedDates.some(date => scheduleData.some(s => s.dateTime.startsWith(date))) 
          }
        >
          Удалить расписание
        </Button>
      </Box>
      <MenuAddScheduleModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        menuData={menuData}
        selectedMenu={selectedMenu}
        setSelectedMenu={setSelectedMenu}
        handleAddMenuToSchedule={handleAddMenuToSchedule}
      />
      <Snackbar
        open={snackbarOpen}
        autoHideDuration={3000}
        onClose={() => setSnackbarOpen(false)}
        anchorOrigin={{ vertical: "bottom", horizontal: "center" }}
      >
        <Alert severity={snackbarMessage?.includes("Ошибка") ? "error" : "success"} onClose={() => setSnackbarOpen(false)}>
          {snackbarMessage}
        </Alert>
      </Snackbar>
    </Container>
  );
};

export default EditSchedule;
