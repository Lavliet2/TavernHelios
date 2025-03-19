// src/pages/Management/EditLayout/LayoutItems.tsx
import React from "react";
import { useDrag } from "react-dnd";

/** Типы для Drag and Drop */
export const ItemTypes = {
  TABLE: "table",
  CHAIR: "chair",
};

interface TableItemProps {
  /** Ширина стола (по умолчанию 50) */
  tableWidth?: number;
  /** Высота стола (по умолчанию 50) */
  tableHeight?: number;
}

/** Компонент "Стол" – квадрат */
export const TableItem: React.FC<TableItemProps> = ({
    tableWidth = 50,
    tableHeight = 50,
  }) => {
    const [{ isDragging }, dragRef] = useDrag(
      () => ({
        type: ItemTypes.TABLE,
        item: {
          type: ItemTypes.TABLE,
          tableWidth,
          tableHeight,
        },
        collect: (monitor) => ({
          isDragging: !!monitor.isDragging(),
        }),
      }),
      [tableWidth, tableHeight] // ✅ Вот здесь нужно добавить зависимости!
    );
  
    return (
      <div
        ref={(node) => {
            if (node) dragRef(node);
        }}
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

/** Компонент "Стул" – круг */
export const ChairItem: React.FC<ChairItemProps> = ({
    chairRadius = 15,
  }) => {
    const [{ isDragging }, dragRef] = useDrag(
      () => ({
        type: ItemTypes.CHAIR,
        item: {
          type: ItemTypes.CHAIR,
          chairRadius,
        },
        collect: (monitor) => ({
          isDragging: !!monitor.isDragging(),
        }),
      }),
      [chairRadius] // ✅ И здесь тоже нужно добавить зависимости!
    );
  
    const diameter = chairRadius * 2;
  
    return (
      <div
        ref={(node) => {
            if (node) dragRef(node);
        }}
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
  
