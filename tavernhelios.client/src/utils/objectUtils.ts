import { DroppedChair, DroppedObject, DroppedTable, DroppedObjectType } from "../types/DroppedObject";

export interface BoundingBox {
  x: number;
  y: number;
  width: number;
  height: number;
}

export function getBoundingBox(obj: DroppedObject): BoundingBox {
  if (obj.type === DroppedObjectType.TABLE) {
    const table = obj as DroppedTable;
    return {
      x: table.x,
      y: table.y,
      width: table.tableWidth,
      height: table.tableHeight,
    };
  } else if (obj.type === DroppedObjectType.CHAIR) {
    const chair = obj as DroppedChair;
    return {
      x: chair.x,
      y: chair.y,
      width: chair.chairRadius * 2,
      height: chair.chairRadius * 2,
    };
  }
  throw new Error("Unknown object type");
}
