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

    const response = await fetch(`${API_BASE_URL}/api/Reservation?IsDeleted=false&BeginDate=${beginDate}&EndDate=${endDate}`);
    if (!response.ok) throw new Error("Ошибка при загрузке броней");

    return response.json();
  } catch (error) {
    console.error("Ошибка при получении броней:", error);
    throw error;
  }
};

export const fetchReservedSeatsForTime = async (
  date: string, // в формате "2025-05-04"
  time: string, // "12:00" или "13:00"
  layoutId: string
): Promise<{ seatNumber: number; tableName: string; personId: string }[]> => {
  try {
    const [hours, minutes] = time.split(":").map(Number);
    const targetDate = new Date(date);
    targetDate.setUTCHours(hours, minutes, 0, 0);
    const iso = targetDate.toISOString();

    const url = `${API_BASE_URL}/api/Reservation?IsDeleted=false&BeginDate=${encodeURIComponent(iso)}&EndDate=${encodeURIComponent(iso)}`;
    const response = await fetch(url);

    if (!response.ok) throw new Error("Ошибка при получении забронированных мест");

    const data: Reservation[] = await response.json();

    return data
      .filter((r) => r.layoutId === layoutId && r.seatNumber > 0)
      .map((r) => ({
        seatNumber: r.seatNumber,
        tableName: r.tableName,
        personId: r.personId,
      }));
  } catch (error) {
    console.error("Ошибка загрузки мест:", error);
    return [];
  }
};

export const exportReservationsFile = async (date: string, format: "pdf" | "excel" = "pdf") => {
  const url = `${API_BASE_URL}/api/reservation/export?date=${date}&format=${format}`;

  try {
    const response = await fetch(url);
    if (!response.ok) throw new Error("Ошибка при загрузке отчета");

    const blob = await response.blob();
    const link = document.createElement("a");
    link.href = window.URL.createObjectURL(blob);
    link.download = `Reservations_${date}.${format === "excel" ? "xlsx" : "pdf"}`;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  } catch (error) {
    console.error("Ошибка экспорта бронирований:", error);
    throw error;
  }
};

