/**
 * Главный компонент редактора схем зала.
 * Отвечает за рендеринг холста, боковой панели и контекстного меню.
 * Управляет логикой загрузки, редактирования и сохранения схем.
 */
import React, { useEffect, useRef, useState, lazy, Suspense } from "react";
import { Box, Menu, MenuItem } from "@mui/material";
import { useDrop } from "react-dnd";

import Sidebar from "../../components/Management/LayoutEditor/Sidebar";
const CreateLayoutModal = lazy(() => import("../../components/Management/LayoutEditor/CreateLayoutModal"));
import LayoutCanvas from "./LayoutCanvas";

import { useLayoutEditorState } from "../../hooks/Management/useLayoutEditorState";
import { useCanvasInteractions } from "../../hooks/Management/useCanvasInteractions";
import { useLayoutEditorLogic } from "../../hooks/Management/useLayoutEditorLogic";

import {
  DroppedObject,
  DroppedObjectType,
} from "../../types/DroppedObject";
import {
  DEFAULT_CHAIR_RADIUS,
  DEFAULT_TABLE_HEIGHT,
  DEFAULT_TABLE_WIDTH,
  // CANVAS_BORDER,
  CANVAS_WIDTH,
  CANVAS_HEIGHT,
} from "../../constants/layoutDefaults";

import { drawCanvas } from "../../utils/canvasUtils";

const LayoutEditor: React.FC = () => {
  const backgroundImgRef = useRef<HTMLImageElement | null>(null);
  const canvasRef = useRef<HTMLCanvasElement>(null);

  const [canvasSize, setCanvasSize] = useState({
    width: CANVAS_WIDTH,
    height: CANVAS_HEIGHT,
  });

  const [tableWidth, setTableWidth] = useState(DEFAULT_TABLE_WIDTH);
  const [tableHeight, setTableHeight] = useState(DEFAULT_TABLE_HEIGHT);
  const [chairRadius, setChairRadius] = useState(DEFAULT_CHAIR_RADIUS);
  const [isEditing, setIsEditing] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);

  const {
    layouts,
    selectedLayoutId,
    setSelectedLayoutId,
    loading,
    loadLayouts,
    handleCreateLayout,
    handleSaveLayout,
    handleDeleteLayout,
  } = useLayoutEditorLogic();

  const {
    objects,
    setObjects,
    tableName,
    setTableName,
    tableSeats,
    setTableSeats,
    existingTableNames,
    currentSeatCount,
  } = useLayoutEditorState();

  const {
    handleMouseDown,
    handleMouseMove,
    handleMouseUp,
    handleContextMenu,
    contextMenu,
    selectedObject,
    setContextMenu,
  } = useCanvasInteractions(canvasRef, isEditing, objects, setObjects);

  const [, dropRef] = useDrop<DroppedObject>(() => ({
    accept: [DroppedObjectType.TABLE, DroppedObjectType.CHAIR],
    drop: (item, monitor) => {
      const offset = monitor.getClientOffset();
      if (!offset || !canvasRef.current) return;

      const rect = canvasRef.current.getBoundingClientRect();
      const newObj: DroppedObject = {
        ...item,
        x: offset.x - rect.left,
        y: offset.y - rect.top,
      };

      if (item.type === DroppedObjectType.TABLE) {
        const newName = (item.name || "").trim();
        if (!newName) return alert("Введите имя стола перед добавлением.");

        const isDuplicate = objects.some(
          (obj) =>
            obj.type === DroppedObjectType.TABLE &&
            (obj.name || "").trim() === newName
        );
        if (isDuplicate)
          return alert(`Стол с именем "${newName}" уже существует`);

        newObj.name = newName;
      }

      setObjects((prev) => [...prev, newObj]);
    },
  }), [objects, setObjects]);

  const handleDeleteObject = () => {
    setObjects((prev) => prev.filter((obj) => obj !== selectedObject));
    setContextMenu(null);
  };

  useEffect(() => {
    loadLayouts();
  }, [loadLayouts]);

  useEffect(() => {
    const layout = layouts.find((l) => l.id === selectedLayoutId);
    if (!layout) return;
    setCanvasSize({ width: layout.width, height: layout.height });

    const loadedObjects: DroppedObject[] = [];

    layout.tables?.forEach((table) => {
      const minX = Math.min(table.p1.x, table.p2.x, table.p3.x, table.p4.x);
      const maxX = Math.max(table.p1.x, table.p2.x, table.p3.x, table.p4.x);
      const minY = Math.min(table.p1.y, table.p2.y, table.p3.y, table.p4.y);
      const maxY = Math.max(table.p1.y, table.p2.y, table.p3.y, table.p4.y);

      loadedObjects.push({
        type: DroppedObjectType.TABLE,
        x: minX,
        y: minY,
        tableWidth: maxX - minX,
        tableHeight: maxY - minY,
        name: table.name,
        description: table.description,
      });

      table.seats?.forEach((seat) => {
        loadedObjects.push({
          type: DroppedObjectType.CHAIR,
          x: seat.center.x - seat.radius,
          y: seat.center.y - seat.radius,
          chairRadius: seat.radius,
          name: table.name,
          description: seat.description,
        });
      });
    });

    setObjects(loadedObjects);

    if (layout.imageStr) {
      const img = new Image();
      img.src = layout.imageStr;
      img.onload = () => {
        backgroundImgRef.current = img;
        if (canvasRef.current)
          drawCanvas(canvasRef.current, img, loadedObjects);
      };
    } else {
      backgroundImgRef.current = null;
      if (canvasRef.current)
        drawCanvas(canvasRef.current, null, loadedObjects);
    }
  }, [selectedLayoutId, layouts, setObjects]);

  useEffect(() => {
    if (canvasRef.current)
      drawCanvas(canvasRef.current, backgroundImgRef.current, objects);
  }, [objects]);

  if (loading) return <div>Загрузка...</div>;

  return (
    <Box sx={{ display: "flex", height: "100vh" }}>
      <Sidebar
        layouts={layouts}
        selectedLayoutId={selectedLayoutId}
        isEditing={isEditing}
        onSelectLayout={setSelectedLayoutId}
        onCreateClick={() => setIsModalOpen(true)}
        onDeleteClick={handleDeleteLayout}
        onToggleEdit={() => setIsEditing(!isEditing)}
        onSaveLayout={() => handleSaveLayout(objects)}
        tableName={tableName}
        setTableName={setTableName}
        tableSeats={tableSeats}
        setTableSeats={setTableSeats}
        currentSeatCount={currentSeatCount}
        objects={objects}
        existingTableNames={existingTableNames}
        tableWidth={tableWidth}
        tableHeight={tableHeight}
        chairRadius={chairRadius}
        setTableWidth={setTableWidth}
        setTableHeight={setTableHeight}
        setChairRadius={setChairRadius}
      />

      <LayoutCanvas
        canvasRef={canvasRef}
        canvasSize={canvasSize}
        dropRef={(node) => dropRef(node) as unknown as void}
        onMouseDown={handleMouseDown}
        onMouseMove={handleMouseMove}
        onMouseUp={handleMouseUp}
        onMouseLeave={handleMouseUp}
        onContextMenu={handleContextMenu}
      />

      <Suspense fallback={<div>Загрузка модального окна...</div>}>
        <CreateLayoutModal
          open={isModalOpen}
          onClose={() => setIsModalOpen(false)}
          onCreateLayout={handleCreateLayout}
        />
      </Suspense>

      <Menu
        open={!!contextMenu}
        onClose={() => setContextMenu(null)}
        anchorReference="anchorPosition"
        anchorPosition={
          contextMenu
            ? { top: contextMenu.mouseY, left: contextMenu.mouseX }
            : undefined
        }
      >
        <MenuItem onClick={() => console.log("Редактировать", selectedObject)}>
          Редактировать
        </MenuItem>
        <MenuItem onClick={handleDeleteObject}>Удалить</MenuItem>
      </Menu>
    </Box>
  );
};

export default LayoutEditor;
