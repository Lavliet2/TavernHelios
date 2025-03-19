import { Container, Typography, Box, Button, Snackbar, Alert  } from "@mui/material";
import monthNames from "../../constants/monthNames"
import useSchedule from "../../hooks/Management/useSchedule";
import CalendarTable from "../../components/Management/ScheduleEditor/CalendarTable";
import MenuAddScheduleModal from "../../components/Management/ScheduleEditor/ScheduleAddMenuModal";


const EditSchedule: React.FC = () => {
  const {
    getMonthDays,
    scheduleData,
    menuData,
    selectedDates,
    currentMonth,
    currentYear,
    setCurrentMonth,
    isModalOpen,
    setIsModalOpen,
    handleAddMenuToSchedule,
    handleDeleteSchedule,
    selectedMenu,
    setSelectedMenu,
    snackbarMessage,
    snackbarOpen,
    setSnackbarOpen,
    handleMouseDown,
    handleMouseEnter,
    handleMouseUp
  } = useSchedule();

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
        scheduleData={scheduleData}
        selectedDates={selectedDates}
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
        <Alert severity={snackbarMessage?.type || "success"} onClose={() => setSnackbarOpen(false)}>
          {snackbarMessage?.text}
        </Alert>
      </Snackbar>
    </Container>
  );
};

export default EditSchedule;
