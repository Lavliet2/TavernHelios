import { useDrop } from "react-dnd";
import { DroppedObject, DroppedObjectType } from "../../../types/DroppedObject";
import { getNewDroppedObject } from "../../../utils/dragDropUtils";
import { useSnackbar } from "../../useSnackbar";

export const useDropHandler = (
  canvasRef: React.RefObject<HTMLCanvasElement | null>,
  onDropObject: (object: DroppedObject) => void,
  getObjects: () => DroppedObject[] // передаём getter, чтобы не терять актуальность
) => {
  const { showSnackbar } = useSnackbar();

  return useDrop<DroppedObject>(() => ({
    accept: [DroppedObjectType.TABLE, DroppedObjectType.CHAIR],
    drop: (item, monitor) => {
      const offset = monitor.getClientOffset();
      const canvas = canvasRef.current;
      if (!offset || !canvas) return;

      const newObj = getNewDroppedObject(
        item,
        canvas,
        offset,
        getObjects(),
        (msg) => showSnackbar(msg, "error")
      );

      if (newObj) {
        onDropObject(newObj);
      }
    },
  }));
};
