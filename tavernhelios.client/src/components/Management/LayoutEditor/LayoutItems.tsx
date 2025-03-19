// src/pages/Management/EditLayout/LayoutItems.tsx
import React from "react";
import { useDrag } from "react-dnd";

export const ItemTypes = {
  TABLE: "table",
  CHAIR: "chair",
};

// Компонент "Стол" – квадрат
export const TableItem: React.FC = () => {
  const [{ isDragging }, dragRef] = useDrag(() => ({
    type: ItemTypes.TABLE,
    collect: (monitor) => ({
      isDragging: !!monitor.isDragging(),
    }),
  }));

  return (
    <div
    //   ref={(node) => node && dragRef(node)}
    ref={(node) => {
      if (node) {
          dragRef(node);
      }
      }}    
      style={{
          width: 50,
          height: 50,
          backgroundColor: "brown",
          opacity: isDragging ? 0.5 : 1,
          cursor: "grab",
          marginBottom: 10,
      }}
    />
  );
};

// Компонент "Стул" – круг
export const ChairItem: React.FC = () => {
  const [{ isDragging }, dragRef] = useDrag(() => ({
    type: ItemTypes.CHAIR,
    collect: (monitor) => ({
      isDragging: !!monitor.isDragging(),
    }),
  }));

  return (
    <div
    //   ref={(node) => node && dragRef(node)}
    ref={(node) => {
      if (node) {
        dragRef(node);
      }
    }}      
    style={{
      width: 30,
      height: 30,
      backgroundColor: "gray",
      borderRadius: "50%",
      opacity: isDragging ? 0.5 : 1,
      cursor: "grab",
    }}
    />
  );
};
