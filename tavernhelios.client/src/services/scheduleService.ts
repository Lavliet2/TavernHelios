import { API_BASE_URL } from "../config";
import { Schedule } from "../types/Management";
import getLocalISODate from "../constants/localDate";

export const fetchScheduleData = async (): Promise<Schedule[]> => {
  try {
    const response = await fetch(`${API_BASE_URL}/api/MenuSchedule`);
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
    const response = await fetch(`${API_BASE_URL}/api/MenuSchedule`, {
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
    const response = await fetch(`${API_BASE_URL}/api/MenuSchedule`, {
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


