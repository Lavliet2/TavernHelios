import { DroppedObject, DroppedObjectType } from "../types/DroppedObject";
import { getBoundingBox } from "./objectUtils";

export function drawCanvas(
  canvas: HTMLCanvasElement | null,
  backgroundImg: HTMLImageElement | null,
  objects: DroppedObject[],
  reservedSeats: { seatNumber: number; tableName: string; personId: string }[] = []
) {
  if (!canvas) return;
  const ctx = canvas.getContext("2d");
  if (!ctx) return;

  ctx.clearRect(0, 0, canvas.width, canvas.height);

  if (backgroundImg) {
    ctx.drawImage(backgroundImg, 0, 0, canvas.width, canvas.height);
  }

  // Создаём карту забронированных мест
  const reservedMap = new Map<string, string>(); // key ➜ personId
  reservedSeats.forEach((seat) => {
    const key = `${seat.tableName.trim().toLowerCase()}_${seat.seatNumber}`;
    reservedMap.set(key, seat.personId);
  });

  for (const obj of objects) {
    const { x, y, width, height } = getBoundingBox(obj);

    if (obj.type === DroppedObjectType.TABLE) {
      ctx.fillStyle = "brown";
      ctx.fillRect(x, y, width, height);

      ctx.fillStyle = "white";
      ctx.font = "14px Arial";
      ctx.textAlign = "center";
      ctx.textBaseline = "middle";
      ctx.fillText(obj.name || "", x + width / 2, y + height / 2);
    }

    else if (obj.type === DroppedObjectType.CHAIR) {
      const radius = height / 2;
      const tableName = (obj.name ?? "").trim().toLowerCase();
      const seatNumber = Number(obj.seatNumber);
      const key = `${tableName}_${seatNumber}`;

      const person = reservedMap.get(key);
      const isReserved = Boolean(person);

      // console.log(`[CHECK] Стул ${seatNumber} у стола "${obj.name}" ➜ ключ "${key}" ➜`, isReserved ? `🔒 Занят (${person})` : "🟢 Свободен");

      ctx.fillStyle = isReserved ? "#999" : "green";
      ctx.beginPath();
      ctx.arc(x + radius, y + radius, radius, 0, Math.PI * 2);
      ctx.fill();

      ctx.fillStyle = "white";
      ctx.font = "12px Arial";
      ctx.textAlign = "center";
      ctx.textBaseline = "middle";
      ctx.fillText(seatNumber.toString(), x + radius, y + radius);
    }
  }
}
