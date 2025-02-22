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
