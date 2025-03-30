/**
 * Главный компонент редактора схем зала.
 *
 * - Отвечает за рендеринг холста (canvas)
 * - Включает боковую панель с управлением объектами (Sidebar)
 * - Загружает и сохраняет схемы из API
 * - Обрабатывает взаимодействие drag-n-drop
 * - Использует хуки управления: загрузкой, отрисовкой, редактором и холстом
 */
import React, {
  useEffect,
  useRef,
  useState,
  useCallback,
} from "react";
import { Box, Typography } from "@mui/material";

import Sidebar from "../../components/Management/LayoutEditor/Sidebar";
import LayoutCanvas from "./LayoutCanvas";
import { useLayoutEditorState } from "../../hooks/Management/useLayoutEditorState";
import { useCanvasInteractions } from "../../hooks/Management/useCanvasInteractions";
import { useLayoutManager } from "../../hooks/Management/useLayoutManager";
import { useLayoutEditorLogic } from "../../hooks/Management/useLayoutEditorLogic";
import { useLayoutLoader } from "../../hooks/Management/useLayoutLoader";
import { useCanvasRenderLoop } from "../../hooks/Management/useCanvasRenderLoop";
import { useDropHandler } from "../../hooks/Management/useDropHandler";

import {
  DEFAULT_CHAIR_RADIUS,
  DEFAULT_TABLE_HEIGHT,
  DEFAULT_TABLE_WIDTH,
  CANVAS_WIDTH,
  CANVAS_HEIGHT,
} from "../../constants/layoutDefaults";
import { drawCanvas } from "../../utils/canvasUtils";

import { RenderLayoutModal } from "./LayoutModalRenderer";
import { RenderContextMenu } from "./RenderContextMenu";

const LayoutEditor: React.FC = React.memo(() => {
  const canvasRef = useRef<HTMLCanvasElement>(null);
  const backgroundImgRef = useRef<HTMLImageElement>(null);

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
    setLayouts,
    selectedLayoutId,
    setSelectedLayoutId,
    loading,
    loadLayouts,
    createNewLayout,
    removeLayout,    
  } = useLayoutManager();

  const {
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
  } = useLayoutEditorState();

  const {
    handleSaveLayout
  } = useLayoutEditorLogic(layouts, selectedLayoutId, setLayouts);

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

  useCanvasRenderLoop({
    canvasRef,
    backgroundImgRef,
    objects,
    drawCanvas,
  });

  const [, dropRef] = useDropHandler(
    canvasRef,
    addObject,
    getObjects
  );

  const handleDeleteObject = useCallback(() => {
    if (selectedObject) {
      removeObject(selectedObject);
    }
    setContextMenu(null);
  }, [selectedObject, removeObject, setContextMenu]);

  useEffect(() => {
    loadLayouts();
  }, [loadLayouts]);

  if (loading) {
    return (
      <Box sx={{ display: "flex", justifyContent: "center", alignItems: "center", height: "100vh" }}>
        <Typography variant="h6">Загрузка схемы...</Typography>
      </Box>
    );
  }

  return (
    <Box sx={{ display: "flex", height: "100vh" }}>
      <Sidebar
        layouts={layouts}
        selectedLayoutId={selectedLayoutId}
        isEditing={isEditing}
        onSelectLayout={setSelectedLayoutId}
        onCreateClick={() => setIsModalOpen(true)}
        onDeleteClick={removeLayout}
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

      <RenderContextMenu
        contextMenu={contextMenu}
        selectedObject={selectedObject}
        onClose={() => setContextMenu(null)}
        onDelete={handleDeleteObject}
      />

      <RenderLayoutModal
        open={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onCreateLayout={createNewLayout}
      />
    </Box>
  );
});

export default LayoutEditor;
