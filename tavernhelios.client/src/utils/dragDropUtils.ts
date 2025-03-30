import { DroppedObject, DroppedObjectType } from "../types/DroppedObject";

interface Offset {
  x: number;
  y: number;
}

type ErrorReporter = (message: string) => void;

/**
 * Создаёт новый объект на холсте, вычисляя координаты с учётом позиции курсора.
 * @param item Перетаскиваемый объект
 * @param canvas HTML-элемент canvas
 * @param offset Координаты курсора мыши
 * @param existingObjects Список уже добавленных объектов
 * @param onError Функция для отображения ошибки (например, Snackbar)
 * @returns Новый объект или null, если добавление невозможно
 */
export function getNewDroppedObject(
  item: DroppedObject,
  canvas: HTMLCanvasElement,
  offset: Offset,
  existingObjects: DroppedObject[],
  onError?: ErrorReporter
): DroppedObject | null {
  const rect = canvas.getBoundingClientRect();
  const x = offset.x - rect.left;
  const y = offset.y - rect.top;

  const baseObject: DroppedObject = {
    ...item,
    x,
    y,
  };

  if (item.type === DroppedObjectType.TABLE) {
    const name = (item.name || "").trim();
    if (!name) {
      onError?.("Введите имя стола перед добавлением.");
      return null;
    }

    const isDuplicate = existingObjects.some(
      (obj) =>
        obj.type === DroppedObjectType.TABLE &&
        (obj.name || "").trim().toLowerCase() === name.toLowerCase()
    );

    if (isDuplicate) {
      onError?.(`Стол с именем "${name}" уже существует.`);
      return null;
    }

    return {
      ...baseObject,
      name,
    };
  }

  return baseObject;
}
