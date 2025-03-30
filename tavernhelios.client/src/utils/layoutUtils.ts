import {
    DroppedChair,
    DroppedObject,
    DroppedObjectType,
    DroppedTable,
  } from "../types/DroppedObject";
  import { Layout } from "../types/Layout";
  
  export function transformLayoutToObjects(layout: Layout): DroppedObject[] {
    const tables: DroppedTable[] =
      layout.tables?.map((table) => {
        const minX = Math.min(table.p1.x, table.p2.x, table.p3.x, table.p4.x);
        const maxX = Math.max(table.p1.x, table.p2.x, table.p3.x, table.p4.x);
        const minY = Math.min(table.p1.y, table.p2.y, table.p3.y, table.p4.y);
        const maxY = Math.max(table.p1.y, table.p2.y, table.p3.y, table.p4.y);
  
        return {
          type: DroppedObjectType.TABLE,
          x: minX,
          y: minY,
          tableWidth: maxX - minX,
          tableHeight: maxY - minY,
          name: table.name,
          description: table.description,
        };
      }) ?? [];
  
    const chairs: DroppedChair[] =
      layout.tables?.flatMap((table) =>
        table.seats?.map((seat) => ({
          type: DroppedObjectType.CHAIR,
          x: seat.center.x - seat.radius,
          y: seat.center.y - seat.radius,
          chairRadius: seat.radius,
          name: table.name,
          description: seat.description,
        })) ?? []
      ) ?? [];
  
    return [...tables, ...chairs];
  }
  