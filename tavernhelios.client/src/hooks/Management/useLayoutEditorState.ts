import { useState, useMemo } from "react";
import { DroppedObject, DroppedObjectType } from "../../types/DroppedObject";
// import { ItemTypes } from "../../types/Layout";

export const useLayoutEditorState = () => {
  const [objects, setObjects] = useState<DroppedObject[]>([]);
  const [tableName, setTableName] = useState("");
  const [tableSeats, setTableSeats] = useState(4);

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

  const resetTableState = () => {
    setTableName("");
    setTableSeats(4);
  };

  return {
    objects,
    setObjects,
    tableName,
    setTableName,
    tableSeats,
    setTableSeats,
    existingTableNames,
    currentSeatCount,
    resetTableState,
  };
};