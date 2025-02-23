import { API_BASE_URL } from "../config";
import { ReservationRequest, Reservation } from "../types/Reservation"


export const createReservation = async (reservationData: ReservationRequest): Promise<void> => {
  try {
    const response = await fetch(`${API_BASE_URL}/api/Reservation`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(reservationData),
    });

    if (!response.ok) throw new Error("Ошибка при создании брони");
  } catch (error) {
    console.error("Ошибка бронирования:", error);
    throw error;
  }
};

export const fetchReservations = async (date: string): Promise<Reservation[]> => {
  try {
    const beginDate = encodeURIComponent(`${date}T00:00:00Z`);
    const endDate = encodeURIComponent(`${date}T23:59:59Z`);

    const response = await fetch(`${API_BASE_URL}/api/Reservation?BeginDate=${beginDate}&EndDate=${endDate}`);
    if (!response.ok) throw new Error("Ошибка при загрузке броней");

    return response.json();
  } catch (error) {
    console.error("Ошибка при получении броней:", error);
    throw error;
  }
};

export const exportReservationsFile = async (date: string) => {
    const url = `${API_BASE_URL}/api/reservation/export?date=${date}&format=pdf`;
  
    try {
      const response = await fetch(url);
      if (!response.ok) throw new Error("Ошибка при загрузке отчета");
  
      const blob = await response.blob();
      const link = document.createElement("a");
      link.href = window.URL.createObjectURL(blob);
      link.download = `Reservations_${date}.pdf`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    } catch (error) {
      console.error("Ошибка экспорта бронирований:", error);
      throw error;
    }
  };
