import { useEffect } from "react";
import { Layout } from "../../../types/Layout";
import { DroppedObject, DroppedObjectType } from "../../../types/DroppedObject";

interface UseLayoutLoaderParams {
  selectedLayoutId: string;
  layouts: Layout[];
  setCanvasSize: (size: { width: number; height: number }) => void;
  setObjects: (objects: DroppedObject[]) => void;
  resetTableState: () => void;
  backgroundImgRef: React.RefObject<HTMLImageElement | null>;
}

export const useLayoutLoader = ({
  selectedLayoutId,
  layouts,
  setCanvasSize,
  setObjects,
  resetTableState,
  backgroundImgRef,
}: UseLayoutLoaderParams) => {
  useEffect(() => {
    const layout = layouts.find((l) => l.id === selectedLayoutId);
    if (!layout) return;

    setCanvasSize({ width: layout.width, height: layout.height });

    const loadedObjects: DroppedObject[] = [];

    layout.tables?.forEach((table) => {
      const minX = Math.min(table.p1.x, table.p2.x, table.p3.x, table.p4.x);
      const maxX = Math.max(table.p1.x, table.p2.x, table.p3.x, table.p4.x);
      const minY = Math.min(table.p1.y, table.p2.y, table.p3.y, table.p4.y);
      const maxY = Math.max(table.p1.y, table.p2.y, table.p3.y, table.p4.y);

      loadedObjects.push({
        type: DroppedObjectType.TABLE,
        x: minX,
        y: minY,
        tableWidth: maxX - minX,
        tableHeight: maxY - minY,
        name: table.name,
        description: table.description,
      });

      table.seats?.forEach((seat) => {
        loadedObjects.push({
          type: DroppedObjectType.CHAIR,
          x: seat.center.x - seat.radius,
          y: seat.center.y - seat.radius,
          chairRadius: seat.radius,
          name: table.name,
          description: seat.description,
          seatNumber: seat.number,
        });
      });
    });

    setObjects(loadedObjects);

    if (selectedLayoutId) {
      resetTableState();
    }

    const img = new Image();
    if (layout.imageStr) {
      img.src = layout.imageStr;
      img.onload = () => {
        backgroundImgRef.current = img;
      };
    } else {
      backgroundImgRef.current = null;
    }
  }, [selectedLayoutId, layouts]);
};
