import { useEffect, useState } from "react";
import { API_BASE_URL } from "../../config";

export interface Reservation {
  id: string;
  personId: string;
  date: string;
  dishIds: string[];
}

export const useReservations = (date: string) => {
  const [reservations12, setReservations12] = useState<Reservation[]>([]);
  const [reservations13, setReservations13] = useState<Reservation[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!date) return;

    const fetchReservations = async () => {
      setLoading(true);
      setError(null);

      try {
        // Формируем даты для запроса
        const beginDate12 = encodeURIComponent(`${date}T07:00:00Z`);
        const endDate12 = encodeURIComponent(`${date}T07:00:00Z`);
        const beginDate13 = encodeURIComponent(`${date}T08:00:00Z`);
        const endDate13 = encodeURIComponent(`${date}T08:00:00Z`);

        // Делаем один запрос для обоих временных диапазонов
        const response12 = await fetch(`${API_BASE_URL}/api/Reservation?BeginDate=${beginDate12}&EndDate=${endDate12}`);
        const response13 = await fetch(`${API_BASE_URL}/api/Reservation?BeginDate=${beginDate13}&EndDate=${endDate13}`);

        if (!response12.ok || !response13.ok) {
          throw new Error("Ошибка при загрузке броней");
        }

        const data12: Reservation[] = await response12.json();
        const data13: Reservation[] = await response13.json();

        setReservations12(data12);
        setReservations13(data13);
      } catch (err) {
        setError((err as Error).message);
      } finally {
        setLoading(false);
      }
    };

    fetchReservations();
  }, [date]);

  return { reservations12, reservations13, loading, error };
};
