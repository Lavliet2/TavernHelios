import { RefObject, useState, useRef } from "react";
import { DroppedObject } from "../../../types/DroppedObject";

export function useCanvasInteractions(
  canvasRef: RefObject<HTMLCanvasElement | null>,
  isEditing: boolean,
  objects: DroppedObject[],
  setObjects: React.Dispatch<React.SetStateAction<DroppedObject[]>>
) {
  const [draggingIndex, setDraggingIndex] = useState<number | null>(null);
  const dragOffset = useRef({ x: 0, y: 0 });

  const [contextMenu, setContextMenu] = useState<{ mouseX: number; mouseY: number } | null>(null);
  const [selectedObject, setSelectedObject] = useState<DroppedObject | null>(null);

  const handleMouseDown = (e: React.MouseEvent<HTMLCanvasElement>) => {
    if (!isEditing || !canvasRef.current) return;
    const rect = canvasRef.current.getBoundingClientRect();
    const mouseX = e.clientX - rect.left;
    const mouseY = e.clientY - rect.top;

    for (let i = objects.length - 1; i >= 0; i--) {
      const obj = objects[i];
      const w = "tableWidth" in obj ? obj.tableWidth : obj.chairRadius * 2;
      const h = "tableHeight" in obj ? obj.tableHeight : obj.chairRadius * 2;
      if (mouseX >= obj.x && mouseX <= obj.x + w && mouseY >= obj.y && mouseY <= obj.y + h) {
        setDraggingIndex(i);
        dragOffset.current = { x: mouseX - obj.x, y: mouseY - obj.y };
        return;
      }
    }
  };

  const handleMouseMove = (e: React.MouseEvent<HTMLCanvasElement>) => {
    if (!isEditing || draggingIndex === null || !canvasRef.current) return;
    const rect = canvasRef.current.getBoundingClientRect();
    const mouseX = e.clientX - rect.left;
    const mouseY = e.clientY - rect.top;

    setObjects((prev) =>
      prev.map((obj, i) =>
        i === draggingIndex
          ? { ...obj, x: mouseX - dragOffset.current.x, y: mouseY - dragOffset.current.y }
          : obj
      )
    );
  };

  const handleMouseUp = () => {
    if (!isEditing) return;
    setDraggingIndex(null);
  };

  const handleContextMenu = (e: React.MouseEvent<HTMLDivElement>) => {
    if (!isEditing || !canvasRef.current) return;
    e.preventDefault();
    const rect = canvasRef.current.getBoundingClientRect();
    const mouseX = e.clientX - rect.left;
    const mouseY = e.clientY - rect.top;

    const obj = objects.find((o) => {
      const w = "tableWidth" in o ? o.tableWidth : o.chairRadius * 2;
      const h = "tableHeight" in o ? o.tableHeight : o.chairRadius * 2;
      return mouseX >= o.x && mouseX <= o.x + w && mouseY >= o.y && mouseY <= o.y + h;
    });

    if (obj) {
      setSelectedObject(obj);
      setContextMenu({ mouseX: e.clientX, mouseY: e.clientY });
    }
  };

  return {
    handleMouseDown,
    handleMouseMove,
    handleMouseUp,
    handleContextMenu,
    contextMenu,
    selectedObject,
    setContextMenu,
    setSelectedObject,
  };
}
