import { useDrop } from "react-dnd";
import { DroppedObject, DroppedObjectType } from "../../types/DroppedObject";
import { getNewDroppedObject } from "../../utils/dragDropUtils";
import { useSnackbar } from "../useSnackbar";

export const useDropHandler = (
  canvasRef: React.RefObject<HTMLCanvasElement | null>,
  objects: DroppedObject[],
  setObjects: React.Dispatch<React.SetStateAction<DroppedObject[]>>
) => {
  const { showSnackbar } = useSnackbar();

  return useDrop<DroppedObject>(() => ({
    accept: [DroppedObjectType.TABLE, DroppedObjectType.CHAIR],
    drop: (item, monitor) => {
      const offset = monitor.getClientOffset();
      if (!offset || !canvasRef.current) return;

      const newObj = getNewDroppedObject(
        item,
        canvasRef.current,
        offset,
        objects,
        (msg) => showSnackbar(msg, "error")
      );

      if (newObj) {
        setObjects((prev) => [...prev, newObj]);
      }
    },
  }));
};
