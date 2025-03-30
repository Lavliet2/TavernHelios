import { DroppedObject, DroppedObjectType } from "../types/DroppedObject";
import { getBoundingBox } from "./objectUtils";

export function drawCanvas(
  canvas: HTMLCanvasElement | null,
  backgroundImg: HTMLImageElement | null,
  objects: DroppedObject[]
) {
  if (!canvas) return;
  const ctx = canvas.getContext("2d");
  if (!ctx) return;

  ctx.clearRect(0, 0, canvas.width, canvas.height);

  if (backgroundImg) {
    ctx.drawImage(backgroundImg, 0, 0, canvas.width, canvas.height);
  }

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
    } else if (obj.type === DroppedObjectType.CHAIR) {
      const radius = height / 2;
      ctx.fillStyle = "green";
      ctx.beginPath();
      ctx.arc(x + radius, y + radius, radius, 0, Math.PI * 2);
      ctx.fill();

      ctx.fillStyle = "white";
      ctx.font = "12px Arial";
      ctx.textAlign = "center";
      ctx.textBaseline = "middle";
      ctx.fillText(obj.name || "", x + radius, y + radius);
    }
  }
}
