import { useEffect, useState, useCallback } from "react";
import { fetchScheduleData, addSchedule, deleteSchedule } from "../../services/scheduleService";
import { Schedule } from "../../types/Management";

const useSchedule = () => {
  const [scheduleData, setScheduleData] = useState<Schedule[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedDates, setSelectedDates] = useState<string[]>([]);
  const [currentMonth, setCurrentMonth] = useState<number>(new Date().getMonth());
  const [currentYear, setCurrentYear] = useState<number>(new Date().getFullYear());
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const [selectedMenu, setSelectedMenu] = useState<string | null>(null);
  const [snackbarMessage, setSnackbarMessage] = useState<string | null>(null);
  const [snackbarOpen, setSnackbarOpen] = useState(false);

  const getMonthDays = () => {
    const firstDayIndex = new Date(currentYear, currentMonth, 1).getDay();
    const daysInMonth = new Date(currentYear, currentMonth + 1, 0).getDate();
  
    const days = [];
    const offset = firstDayIndex === 0 ? 6 : firstDayIndex - 1;
  
    for (let i = 0; i < offset; i++) {
      days.push(null);
    }
  
    for (let i = 1; i <= daysInMonth; i++) {
      const dateObj = new Date(currentYear, currentMonth, i);
      const localDate = dateObj.toLocaleDateString("sv-SE");
  
      days.push({
        day: i,
        date: localDate,
        isWeekend: dateObj.getDay() === 0 || dateObj.getDay() === 6,
        isToday: new Date().toLocaleDateString("sv-SE") === localDate,
        hasSchedule: scheduleData.some((s) => s.dateTime.startsWith(localDate)),
      });
    }
  
    return days;
  };
  

  const showSnackbar = useCallback((message: string) => {
    setSnackbarMessage(message);
    setSnackbarOpen(true);
  }, []);

  const loadSchedule = async () => {
    try {
      setLoading(true);
      const data = await fetchScheduleData();
      setScheduleData(data);
    } catch (error) {
      setError("Ошибка загрузки расписания");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadSchedule();
  }, []);

  const toggleDateSelection = (date: string) => {
    setSelectedDates((prev) =>
      prev.includes(date) ? prev.filter((d) => d !== date) : [...prev, date]
    );
  };

  const handleAddMenuToSchedule = async () => {
    if (!selectedMenu) return;

    try {
      await Promise.all(selectedDates.map((date) => addSchedule(date, selectedMenu)));

      await loadSchedule();
      setSelectedDates([]);
      setIsModalOpen(false);

      showSnackbar("Меню успешно добавлено!");
    } catch (error) {
      showSnackbar("Ошибка при добавлении меню");
    }
  };

  const handleDeleteSchedule = async () => {
    try {
      const idsToDelete = selectedDates
        .map(date => scheduleData.find(s => s.dateTime.startsWith(date))?.id)
        .filter((id): id is string => Boolean(id));

      if (idsToDelete.length === 0) {
        showSnackbar("Нет записей для удаления");
        return;
      }

      await Promise.all(idsToDelete.map(id => deleteSchedule(id)));

      await loadSchedule();
      setSelectedDates([]);
      showSnackbar("Расписание успешно удалено!");
    } catch (error) {
      showSnackbar("Ошибка при удалении расписания");
    }
  };

  return {
    getMonthDays,
    scheduleData,
    loading,
    error,
    selectedDates,
    currentMonth,
    currentYear,
    setCurrentMonth,
    setCurrentYear,
    toggleDateSelection,
    isModalOpen,
    setIsModalOpen,
    handleAddMenuToSchedule,
    handleDeleteSchedule,
    selectedMenu,
    setSelectedMenu,
    snackbarMessage,
    setSnackbarMessage,
    snackbarOpen,
    setSnackbarOpen,
  };
};

export default useSchedule;
