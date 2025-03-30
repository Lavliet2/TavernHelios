import { DroppedChair, DroppedObject, DroppedObjectType, DroppedTable } from "../../types/DroppedObject";
import { Table } from "../../types/Layout";

export function transformObjectsToTables(objects: DroppedObject[]): Table[] {
  return objects
    .filter((obj): obj is DroppedTable => obj.type === DroppedObjectType.TABLE)
    .map((table) => {
      const relatedSeats = objects
        .filter(
          (seat): seat is DroppedChair =>
            seat.type === DroppedObjectType.CHAIR && seat.name === table.name
        )
        .map((seat, index) => ({
          number: index + 1,
          description: seat.description || "",
          center: {
            x: seat.x + seat.chairRadius,
            y: seat.y + seat.chairRadius,
          },
          radius: seat.chairRadius,
        }));

      return {
        name: table.name || "",
        description: table.description || "",
        seats: relatedSeats,
        p1: { x: table.x, y: table.y },
        p2: { x: table.x + table.tableWidth, y: table.y },
        p3: { x: table.x + table.tableWidth, y: table.y + table.tableHeight },
        p4: { x: table.x, y: table.y + table.tableHeight },
      };
    });
}
