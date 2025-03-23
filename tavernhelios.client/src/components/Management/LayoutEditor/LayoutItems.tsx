// src/pages/Management/EditLayout/LayoutItems.tsx
import React from "react";
import { useDrag } from "react-dnd";

/** Типы для Drag and Drop */
export const ItemTypes = {
  TABLE: "table",
  CHAIR: "chair",
};

interface TableItemProps {
    tableWidth?: number;
    tableHeight?: number;
    name: string;
    seats: number;
}

/** Компонент "Стол" – квадрат */
export const TableItem: React.FC<TableItemProps> = ({
    tableWidth = 50,
    tableHeight = 50,
    name,
    seats,
  }) => {
    const [{ isDragging }, dragRef] = useDrag(
      () => ({
        type: ItemTypes.TABLE,
        item: {
          type: ItemTypes.TABLE,
          tableWidth,
          tableHeight,
          name,
          seats,
        },
        collect: (monitor) => ({
          isDragging: !!monitor.isDragging(),
        }),
      }),
      [tableWidth, tableHeight, name, seats]
    );
  
    return (
      <div
        ref={dragRef}
        style={{
          width: tableWidth,
          height: tableHeight,
          backgroundColor: "brown",
          opacity: isDragging ? 0.5 : 1,
          cursor: "grab",
          marginBottom: 10,
        }}
      />
    );
  };
  

interface ChairItemProps {
  /** Радиус стула (по умолчанию 15) */
  chairRadius?: number;
}

interface ChairItemProps {
    chairRadius?: number;
    name: string;
  }
  
  export const ChairItem: React.FC<ChairItemProps> = ({
    chairRadius = 15,
    name,
  }) => {
    const [{ isDragging }, dragRef] = useDrag(
      () => ({
        type: ItemTypes.CHAIR,
        item: {
          type: ItemTypes.CHAIR,
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
  
    return (
      <div
        ref={dragRef}
        style={{
          width: diameter,
          height: diameter,
          backgroundColor: "gray",
          borderRadius: "50%",
          opacity: isDragging ? 0.5 : 1,
          cursor: "grab",
        }}
      />
    );
  };
  
