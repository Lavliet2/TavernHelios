import { API_BASE_URL } from "../config";
import { Schedule } from "../types/Management";
import getLocalISODate from "../constants/localDate";

export const fetchScheduleData = async (isDeleted = false): Promise<Schedule[]> => {
  try {
    const response = await customFetch(`${API_BASE_URL}/api/MenuSchedule?isDeleted=${isDeleted}`);
    if (!response.ok) throw new Error("Ошибка при загрузке расписания");
    return response.json();
  } catch (error) {
    console.error("Ошибка при получении расписания:", error);
    throw error;
  }
};

export const addSchedule = async (date: string, menuId: string): Promise<void> => {
  try {
    const fullDateTime = getLocalISODate(date);
    const response = await customFetch(`${API_BASE_URL}/api/MenuSchedule`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ dateTime: fullDateTime, menuId }),
    });

    if (!response.ok) {
      const errorResponse = await response.json();
      throw new Error(`Ошибка при добавлении расписания: ${errorResponse.message || response.statusText}`);
    }
  } catch (error) {
    console.error("Ошибка при добавлении расписания:", error);
    throw error;
  }
};

export const deleteSchedule = async (id: string): Promise<void> => {
  try {
    const response = await customFetch(`${API_BASE_URL}/api/MenuSchedule`, {
      method: "DELETE",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(id),
    });

    if (!response.ok) {
      const errorResponse = await response.json();
      throw new Error(`Ошибка при удалении: ${errorResponse.message || response.statusText}`);
    }
  } catch (error) {
    console.error("Ошибка при удалении расписания:", error);
    throw error;
  }
};

export const fetchTodaySchedule = async (isDeleted = false): Promise<Schedule | null> => {
  try {
    const today = new Date();
    const formattedDate = today.toISOString().split("T")[0];

    const beginDate = encodeURIComponent(`${formattedDate}T00:00:00Z`);
    const endDate = encodeURIComponent(`${formattedDate}T23:59:59Z`);

    const response = (await customFetch(`${API_BASE_URL}/api/MenuSchedule?BeginDate=${beginDate}&EndDate=${endDate}&IsDeleted=${isDeleted}`));

    if (!response.ok) throw new Error("Ошибка при загрузке расписания");

    const schedules: Schedule[] = await response.json();

    return schedules.length > 0 ? schedules[0] : null;
  } catch (error) {
    console.error("Ошибка при получении расписания:", error);
    throw error;
  }
};

const customFetch = async (url: string, options?: any) => {
  const fetchOptions = {
    ...options,
    credentials: 'include', // Включаем куки в запрос
  };
  const response = await fetch(url, fetchOptions);
  if (response.status === 401) {
    window.location.href = '/login';
    return response;
  }
  return response;
}
