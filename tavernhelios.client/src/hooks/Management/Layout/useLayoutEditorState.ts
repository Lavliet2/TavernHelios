import { useState, useMemo, useCallback } from "react";
import { DroppedObject, DroppedObjectType } from "../../../types/DroppedObject";

export const useLayoutEditorState = () => {
  const [objects, setObjects] = useState<DroppedObject[]>([]);
  const [tableName, setTableName] = useState("");
  const [tableSeats, setTableSeats] = useState(4);

  const resetTableState = useCallback(() => {
    setTableName("");
    setTableSeats(4);
  }, []);

  const addObject = useCallback((object: DroppedObject) => {
    setObjects((prev) => {
      if (object.type === DroppedObjectType.CHAIR) {
        const tableName = object.name?.trim().toLowerCase();
        if (!tableName) return prev;
  
        const chairSeatNumbers = prev
          .filter(
            (o) =>
              o.type === DroppedObjectType.CHAIR &&
              o.name?.trim().toLowerCase() === tableName
          )
          .map((o) => o.seatNumber)
          .filter((n): n is number => typeof n === "number")
          .sort((a, b) => a - b);
  
        // Найдём минимальный свободный номер
        let seatNumber = 1;
        for (let i = 0; i < chairSeatNumbers.length; i++) {
          if (chairSeatNumbers[i] !== i + 1) {
            seatNumber = i + 1;
            break;
          }
          seatNumber = chairSeatNumbers.length + 1;
        }
  
        return [
          ...prev,
          {
            ...object,
            seatNumber,
          },
        ];
      }
  
      return [...prev, object];
    });
  }, []);
  

  const removeObject = useCallback((object: DroppedObject) => {
    setObjects((prev) => prev.filter((obj) => obj !== object));
  }, []);

  const getObjects = useCallback(() => {
    return [...objects]; // возвращаем копию для предотвращения мутаций
  }, [objects]);

  const existingTableNames = useMemo(() => {
    return objects
      .filter((obj) => obj.type === DroppedObjectType.TABLE && obj.name)
      .map((obj) => (obj.name || "").trim().toLowerCase());
  }, [objects]);

  const currentSeatCount = useMemo(() => {
    return objects.filter(
      (o) => o.type === DroppedObjectType.CHAIR && o.name === tableName
    ).length;
  }, [objects, tableName]);

  return {
    objects,
    setObjects,
    addObject,
    removeObject,
    getObjects,
    tableName,
    setTableName,
    tableSeats,
    setTableSeats,
    existingTableNames,
    currentSeatCount,
    resetTableState,
  };
};
