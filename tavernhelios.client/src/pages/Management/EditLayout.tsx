/**
 * Главный компонент редактора схем зала.
 * Отвечает за рендеринг холста, боковой панели и контекстного меню.
 * Управляет логикой загрузки, редактирования и сохранения схем.
 */
import React, {
  useEffect,
  useRef,
  useState,
  lazy,
  Suspense,
} from "react";
import { Box, Menu, MenuItem } from "@mui/material";

import Sidebar from "../../components/Management/LayoutEditor/Sidebar";
import LayoutCanvas from "./LayoutCanvas";
import { useLayoutEditorState } from "../../hooks/Management/useLayoutEditorState";
import { useCanvasInteractions } from "../../hooks/Management/useCanvasInteractions";
import { useLayoutEditorLogic } from "../../hooks/Management/useLayoutEditorLogic";
import { useLayoutLoader } from "../../hooks/Management/useLayoutLoader";
import { useCanvasRenderer } from "../../hooks/Management/useCanvasRenderer";
import { useDropHandler } from "../../hooks/Management/useDropHandler";

import {
  DEFAULT_CHAIR_RADIUS,
  DEFAULT_TABLE_HEIGHT,
  DEFAULT_TABLE_WIDTH,
  CANVAS_WIDTH,
  CANVAS_HEIGHT,
} from "../../constants/layoutDefaults";
import { drawCanvas } from "../../utils/canvasUtils";

const CreateLayoutModal = lazy(
  () => import("../../components/Management/LayoutEditor/CreateLayoutModal")
);

const LayoutEditor: React.FC = () => {
  const canvasRef = useRef<HTMLCanvasElement>(null);
  const backgroundImgRef = useRef(null as unknown) as React.RefObject<HTMLImageElement>;

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
    resetTableState,
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

  useLayoutLoader({
    selectedLayoutId,
    layouts,
    setCanvasSize,
    setObjects,
    resetTableState,
    backgroundImgRef,
    canvasRef,
    drawCanvas,
  });

  useCanvasRenderer({
    canvasRef,
    backgroundImgRef,
    objects,
    drawCanvas,
  });

  const [, dropRef] = useDropHandler(canvasRef, objects, setObjects);

  const handleDeleteObject = () => {
    setObjects((prev) => prev.filter((obj) => obj !== selectedObject));
    setContextMenu(null);
  };
  
  useEffect(() => {
    loadLayouts();
  }, [loadLayouts]);

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
