import React, { useEffect, useState, useRef } from "react";
import { Box } from "@mui/material";
import { useDrop } from "react-dnd";

import Sidebar from "../../components/Management/LayoutEditor/Sidebar";
import CreateLayoutModal from "../../components/Management/LayoutEditor/CreateLayoutModal";
import { ItemTypes } from "../../components/Management/LayoutEditor/LayoutItems";

import {
  fetchLayouts,
  createLayout,
  deleteLayout,
  updateLayout,
} from "../../services/layoutService";
import { Layout } from "../../types/Layout";

interface DroppedObject {
  type: string;
  x: number;
  y: number;
  tableWidth?: number;
  tableHeight?: number;
  chairRadius?: number;
}

const LayoutEditor: React.FC = () => {
  const [loading, setLoading] = useState(true);
  const [isEditing, setIsEditing] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);

  const [layouts, setLayouts] = useState<Layout[]>([]);
  const [selectedLayoutId, setSelectedLayoutId] = useState<string>("");

  const [canvasSize, setCanvasSize] = useState({ width: 800, height: 600 });
  const canvasRef = useRef<HTMLCanvasElement | null>(null);

  const [objects, setObjects] = useState<DroppedObject[]>([]);
  const [originalObjects, setOriginalObjects] = useState<DroppedObject[]>([]);

  const [selectedObjectIndex, setSelectedObjectIndex] = useState<number | null>(null);
  const [dragOffset, setDragOffset] = useState({ x: 0, y: 0 });

  const backgroundImgRef = useRef<HTMLImageElement | null>(null);

  useEffect(() => { loadLayouts(); }, []);

  useEffect(() => { updateCanvasSize(); drawCanvas(); }, [selectedLayoutId, objects, layouts]);

  const loadLayouts = async () => {
    try {
      const data = await fetchLayouts();
      setLayouts(data);
      if (data.length > 0) {
        setSelectedLayoutId(data[0].id);
        setCanvasSize({ width: data[0].width, height: data[0].height });
      }
    } finally { setLoading(false); }
  };

  useEffect(() => {
    const layout = layouts.find((l) => l.id === selectedLayoutId);
    if (layout?.imageStr) {
      const img = new Image();
      img.src = layout.imageStr;
      img.onload = () => {
        backgroundImgRef.current = img;
        drawCanvas(); 
      };
    } else {
      backgroundImgRef.current = null;
      drawCanvas();
    }
  }, [selectedLayoutId, layouts]);

  const updateCanvasSize = () => {
    const layout = layouts.find((l) => l.id === selectedLayoutId);
    if (layout) setCanvasSize({ width: layout.width, height: layout.height });
  };

  const drawCanvas = () => {
    const canvas = canvasRef.current;
    if (!canvas) return;
    const ctx = canvas.getContext("2d");
    if (!ctx) return;

    ctx.clearRect(0, 0, canvas.width, canvas.height);

    if (backgroundImgRef.current) {
      ctx.drawImage(backgroundImgRef.current, 0, 0, canvas.width, canvas.height);
    }

    objects.forEach((obj) => {
      if (obj.type === ItemTypes.TABLE) {
        ctx.fillStyle = "brown";
        ctx.fillRect(obj.x, obj.y, obj.tableWidth ?? 50, obj.tableHeight ?? 50);
      } else if (obj.type === ItemTypes.CHAIR) {
        ctx.fillStyle = "gray";
        const r = obj.chairRadius ?? 15;
        ctx.beginPath();
        ctx.arc(obj.x + r, obj.y + r, r, 0, 2 * Math.PI);
        ctx.fill();
      }
    });
  };

  const [, dropRef] = useDrop(() => ({
    accept: [ItemTypes.TABLE, ItemTypes.CHAIR],
    drop: (item: any, monitor) => {
      if (!canvasRef.current) return;
      const offset = monitor.getClientOffset();
      if (!offset) return;
      const rect = canvasRef.current.getBoundingClientRect();
      setObjects((prev) => [
        ...prev,
        { type: item.type, x: offset.x - rect.left, y: offset.y - rect.top, ...item },
      ]);
    },
  }));

  const handleMouseDown = (e: React.MouseEvent) => {
    if (!isEditing) return;
    const rect = canvasRef.current!.getBoundingClientRect();
    const mouseX = e.clientX - rect.left;
    const mouseY = e.clientY - rect.top;

    for (let i = objects.length - 1; i >= 0; i--) {
      const obj = objects[i];
      const w = obj.tableWidth ?? 50, h = obj.tableHeight ?? 50, r = obj.chairRadius ?? 15;
      const isInside = obj.type === ItemTypes.TABLE
        ? mouseX >= obj.x && mouseX <= obj.x + w && mouseY >= obj.y && mouseY <= obj.y + h
        : (mouseX - obj.x - r) ** 2 + (mouseY - obj.y - r) ** 2 <= r ** 2;
      if (isInside) {
        setSelectedObjectIndex(i);
        setDragOffset({ x: mouseX - obj.x, y: mouseY - obj.y });
        return;
      }
    }
  };

  const handleMouseMove = (e: React.MouseEvent) => {
    if (selectedObjectIndex === null) return;
    const rect = canvasRef.current!.getBoundingClientRect();
    setObjects((prev) => prev.map((obj, i) =>
      i === selectedObjectIndex ? { ...obj, x: e.clientX - rect.left - dragOffset.x, y: e.clientY - rect.top - dragOffset.y } : obj
    ));
  };

  const handleMouseUp = () => { setSelectedObjectIndex(null); };

  return (
    <Box sx={{ display: "flex", height: "100vh" }}>
      <Sidebar
        {...{
          layouts,
          selectedLayoutId,
          isEditing,
          onSelectLayout: setSelectedLayoutId,
          onCreateClick: () => setIsModalOpen(true),
          onDeleteClick: deleteLayout,
          onToggleEdit: () => setIsEditing(!isEditing),
        }}
      />
      <div
        ref={(node) => dropRef(node) as unknown as void} // ✅ фикс
        style={{ flexGrow: 1 }}
      >
        <div
          style={{
            width: "100%",
            height: "100%",
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
          }}
        >
          <canvas
            ref={canvasRef}
            width={canvasSize.width}
            height={canvasSize.height}
            style={{ border: "2px solid black" }}
            onMouseDown={handleMouseDown}
            onMouseMove={handleMouseMove}
            onMouseUp={handleMouseUp}
            onMouseLeave={handleMouseUp}
          />
        </div>
      </div>
      <CreateLayoutModal
        open={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onCreateLayout={async (layoutData) => {
          await createLayout(layoutData);
        }}
      />
    </Box>
  );
  
}  

export default LayoutEditor;
