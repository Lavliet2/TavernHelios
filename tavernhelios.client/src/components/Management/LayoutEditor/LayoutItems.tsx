// src/pages/Management/EditLayout/LayoutItems.tsx
import React from "react";
import { useDrag } from "react-dnd";
import { DroppedObjectType } from "../../../types/DroppedObject";


interface TableItemProps {
  tableWidth?: number;
  tableHeight?: number;
  name: string;
  seats: number;
  isDraggable?: boolean;
}

/** Компонент "Стол" – квадрат */
export const TableItem: React.FC<TableItemProps> = ({
  tableWidth = 50,
  tableHeight = 50,
  name,
  seats,
  isDraggable = true,
}) => {
  const [{ isDragging }, dragRef] = useDrag(
    () => ({
      type: DroppedObjectType.TABLE,
      item: {
        type: DroppedObjectType.TABLE,
        tableWidth,
        tableHeight,
        name,
        seats,
      },
      canDrag: () => isDraggable,
      collect: (monitor) => ({
        isDragging: !!monitor.isDragging(),
      }),
    }),
    [tableWidth, tableHeight, name, seats]
  );

  if (!isDraggable) return null;

  return dragRef(
    <div
      style={{
        width: tableWidth,
        height: tableHeight,
        backgroundColor: "brown",
        opacity: isDragging ? 0.5 : 1,
        cursor: "grab",
        marginBottom: 10,
      }}
      title="Стол"
    />
  );
};

interface ChairItemProps {
  /** Радиус стула */
  chairRadius?: number;
  /** Имя стола, к которому принадлежит стул */
  name: string;
}

/** Компонент "Стул" – круглый */
export const ChairItem: React.FC<ChairItemProps> = ({
  chairRadius = 15,
  name,
}) => {
  const [{ isDragging }, dragRef] = useDrag(
    () => ({
      type: DroppedObjectType.CHAIR,
      item: {
        type: DroppedObjectType.CHAIR,
        chairRadius,
        name,
      },
      collect: (monitor) => ({
        isDragging: !!monitor.isDragging(),
      }),
    }),
    [chairRadius, name]
  );

  const diameter = chairRadius * 2;

  return dragRef(
    <div
      style={{
        width: diameter,
        height: diameter,
        backgroundColor: "gray",
        borderRadius: "50%",
        opacity: isDragging ? 0.5 : 1,
        cursor: "grab",
        marginBottom: 10,
      }}
      title="Стул"
    />
  );
};
