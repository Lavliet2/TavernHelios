import axios from 'axios';
import { API_BASE_URL } from "../config";
import { Schedule } from "../types/Management";
import getLocalISODate from "../constants/localDate";
import { isHttpResponseSuccess } from '../utils/httpUtils';

export const fetchScheduleData = async (isDeleted = false): Promise<Schedule[]> => {
  try {
    const response = await axios.get(`${API_BASE_URL}/api/MenuSchedule`, {
      params: { isDeleted }
    });

    return response.data;
  } catch (error) {
    console.error("Ошибка при получении расписания:", error);
    throw error;
  }
};

export const addSchedule = async (date: string, menuId: string): Promise<void> => {
  try {
    const fullDateTime = getLocalISODate(date);
    const response = await axios.post(
      `${API_BASE_URL}/api/MenuSchedule`,
      { dateTime: fullDateTime, menuId },
      {
        headers: { "Content-Type": "application/json" }
      }
    );

    if (!isHttpResponseSuccess(response.status)) {
      throw new Error(`Ошибка при добавлении расписания: ${response.data.message || response.statusText}`);
    }
  } catch (error) {
    console.error("Ошибка при добавлении расписания:", error);
    throw error;
  }
};

export const deleteSchedule = async (id: string): Promise<void> => {
  try {
    const response = await axios.delete(
      `${API_BASE_URL}/api/MenuSchedule`,
      {
        data: JSON.stringify(id),
        headers: { "Content-Type": "application/json" }
      }
    );

    if (!isHttpResponseSuccess(response.status)) {
      throw new Error(`Ошибка при удалении: ${response.data.message || response.statusText}`);
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

    const response = await axios.get(`${API_BASE_URL}/api/MenuSchedule?BeginDate=${beginDate}&EndDate=${endDate}&IsDeleted=${isDeleted}`);

    const schedules: Schedule[] = response.data;

    return schedules.length > 0 ? schedules[0] : null;
  } catch (error) {
    console.error("Ошибка при получении расписания:", error);
    throw error;
  }
};
