// src/pages/Management/EditLayout/LayoutItems.tsx
import React from "react";
import { useDrag } from "react-dnd";
import { ItemTypes } from "../../../types/Layout";


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
        type: ItemTypes.TABLE,
        item: {
          type: ItemTypes.TABLE,
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
          backgroundColor: isDraggable ? "brown" : "lightgray",
          opacity: isDragging ? 0.5 : 1,
          cursor: isDraggable ? "grab" : "not-allowed",
          marginBottom: 10,
        }}
        title={isDraggable ? "" : "Такой стол уже есть на схеме"}
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
  
    return dragRef(
      <div
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
  
