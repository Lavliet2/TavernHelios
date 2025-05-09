import { useEffect, useState, useCallback } from "react";
import debounce from "lodash/debounce";
import { fetchScheduleData, addSchedule, deleteSchedule } from "../../services/scheduleService";
import { fetchMenus } from "../../services/menuService";
import { Schedule, Menu } from "../../types/Management";

const useSchedule = () => {
  const [scheduleData, setScheduleData] = useState<Schedule[]>([]);
  const [menuData, setMenuData] = useState<Menu[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedDates, setSelectedDates] = useState<string[]>([]);
  const [currentMonth, setCurrentMonth] = useState<number>(new Date().getMonth());
  const [currentYear, setCurrentYear] = useState<number>(new Date().getFullYear());
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const [selectedMenu, setSelectedMenu] = useState<string | null>(null);
  const [snackbarMessage, setSnackbarMessage] = useState<{ text: string; type: "success" | "error" | "warning" | "info" } | null>(null);
  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [isMouseDown, setIsMouseDown] = useState(false);
  const [isAdding, setIsAdding] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);

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
  
  const showSnackbar = useCallback((message: string, type: "success" | "error" | "warning" | "info" = "success") => {
    setSnackbarMessage({ text: message, type }); 
    setSnackbarOpen(true);
  }, []);

  const loadSchedule = async () => {
    try {
      setLoading(true);
      const [schedule, menus] = await Promise.all([
        fetchScheduleData(),
        fetchMenus(),
      ]);
      setScheduleData(schedule);
      setMenuData(menus);
    } catch (error) {
      setError("Ошибка загрузки данных расписания");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadSchedule();
  }, [currentMonth, currentYear]);

  const toggleDateSelection = useCallback((date: string) => {
    setSelectedDates((prev) =>
      prev.includes(date) ? prev.filter((d) => d !== date) : [...prev, date]
    );
  }, []);

  const handleAddMenuToSchedule = useCallback(async () => {
    if (!selectedMenu) {
      showSnackbar("Выберите меню перед добавлением!", "warning");
      return;
    }
    setIsAdding(true);
    try {
      await Promise.all(selectedDates.map((date) => addSchedule(date, selectedMenu)));
  
      await loadSchedule();
      setSelectedDates([]);
      setIsModalOpen(false);
      showSnackbar("Меню успешно добавлено!", "success");
    } catch (error) {
      showSnackbar("Ошибка при добавлении меню");
    } finally {
      setIsAdding(false);
    }
  }, [selectedMenu, selectedDates, loadSchedule, showSnackbar]);

  const handleDeleteSchedule = useCallback(async () => {
    setIsDeleting(true)
    try {
      const idsToDelete = selectedDates
        .map(date => scheduleData.find(s => s.dateTime.startsWith(date))?.id)
        .filter((id): id is string => Boolean(id));

      if (idsToDelete.length === 0) {
        showSnackbar("Нет записей для удаления", "error");
        return;
      }
      await Promise.all(idsToDelete.map(id => deleteSchedule(id)));

      await loadSchedule();
      setSelectedDates([]);
      showSnackbar("Расписание успешно удалено!", "success");
    } catch (error) {
      showSnackbar("Ошибка при удалении расписания", "error");
    } finally {
      setIsDeleting(false);
    }
  }, [selectedDates, scheduleData, loadSchedule, showSnackbar]);

  const handleMouseDown = useCallback((date: string) => {
    setIsMouseDown(true);
    toggleDateSelection(date);
  }, [toggleDateSelection]);
  

  const handleMouseEnter = useCallback(debounce((date: string) => {
    if (isMouseDown) {
      toggleDateSelection(date);
    }
  }, 50), [isMouseDown, toggleDateSelection]);
  
  const handleMouseUp = useCallback(() => {
    setIsMouseDown(false);
  }, []);  

  return {
    getMonthDays,
    scheduleData,
    menuData,
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
    showSnackbar,
    handleMouseDown,
    handleMouseEnter, 
    handleMouseUp,
    isAdding,
    isDeleting
  };
};

export default useSchedule;
