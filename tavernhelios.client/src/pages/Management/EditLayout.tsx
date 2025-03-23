import React, { useEffect, useState, useRef } from "react";
import { Box } from "@mui/material";
import { useDrop } from "react-dnd";

import Sidebar from "../../components/Management/LayoutEditor/Sidebar";
import CreateLayoutModal from "../../components/Management/LayoutEditor/CreateLayoutModal";
import { ItemTypes, Layout } from "../../types/Layout";
import {
  fetchLayouts,
  createLayout,
  deleteLayout as deleteLayoutService,
} from "../../services/layoutService";

interface DroppedObject {
  type: string;
  x: number;
  y: number;
  tableWidth?: number;
  tableHeight?: number;
  chairRadius?: number;
  name?: string;
  description?: string;
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
  const backgroundImgRef = useRef<HTMLImageElement | null>(null);
  const [draggingIndex, setDraggingIndex] = useState<number | null>(null);
  const dragOffset = useRef({ x: 0, y: 0 });

  useEffect(() => {
    loadLayouts();
  }, []);

  useEffect(() => {
    const layout = layouts.find((l) => l.id === selectedLayoutId);
    if (layout) {
      setCanvasSize({ width: layout.width, height: layout.height });

      const loadedTables =
        layout.tables?.map((table) => {
          const minX = Math.min(table.p1.x, table.p2.x, table.p3.x, table.p4.x);
          const maxX = Math.max(table.p1.x, table.p2.x, table.p3.x, table.p4.x);
          const minY = Math.min(table.p1.y, table.p2.y, table.p3.y, table.p4.y);
          const maxY = Math.max(table.p1.y, table.p2.y, table.p3.y, table.p4.y);

          return {
            type: ItemTypes.TABLE,
            x: minX,
            y: minY,
            tableWidth: maxX - minX,
            tableHeight: maxY - minY,
            name: table.name,
            description: table.description,
          };
        }) || [];

      const loadedSeats =
        layout.seats?.map((seat) => ({
          type: ItemTypes.CHAIR,
          x: seat.center.x - seat.radius,
          y: seat.center.y - seat.radius,
          chairRadius: seat.radius,
          name: seat.number.toString(),
          description: seat.description,
        })) || [];

      setObjects([...loadedTables, ...loadedSeats]);

      if (layout.imageStr) {
        const img = new Image();
        img.src = layout.imageStr;
        img.onload = () => {
          backgroundImgRef.current = img;
          drawCanvas([...loadedTables, ...loadedSeats]);
        };
      } else {
        backgroundImgRef.current = null;
        drawCanvas([...loadedTables, ...loadedSeats]);
      }
    }
  }, [selectedLayoutId, layouts]);

  const loadLayouts = async () => {
    const data = await fetchLayouts();
    setLayouts(data);
    if (data.length > 0) setSelectedLayoutId(data[0].id);
    setLoading(false);
  };

  const drawCanvas = (objs = objects) => {
    const canvas = canvasRef.current;
    if (!canvas) return;
    const ctx = canvas.getContext("2d");
    if (!ctx) return;

    ctx.clearRect(0, 0, canvas.width, canvas.height);

    if (backgroundImgRef.current) {
      ctx.drawImage(backgroundImgRef.current, 0, 0, canvas.width, canvas.height);
    }

    objs.forEach((obj) => {
      ctx.fillStyle = obj.type === ItemTypes.TABLE ? "brown" : "gray";

      if (obj.type === ItemTypes.TABLE) {
        const w = obj.tableWidth ?? 50;
        const h = obj.tableHeight ?? 50;
        ctx.fillRect(obj.x, obj.y, w, h);

        ctx.fillStyle = "white";
        ctx.font = "14px Arial";
        ctx.textAlign = "center";
        ctx.textBaseline = "middle";
        ctx.fillText(obj.name || "", obj.x + w / 2, obj.y + h / 2);
      }

      if (obj.type === ItemTypes.CHAIR) {
        const r = obj.chairRadius ?? 15;
        ctx.beginPath();
        ctx.arc(obj.x + r, obj.y + r, r, 0, 2 * Math.PI);
        ctx.fill();

        ctx.fillStyle = "white";
        ctx.font = "12px Arial";
        ctx.textAlign = "center";
        ctx.textBaseline = "middle";
        ctx.fillText(obj.name || "", obj.x + r, obj.y + r);
      }
    });
  };

  useEffect(() => {
    drawCanvas();
  }, [objects]);

  const [, dropRef] = useDrop(() => ({
    accept: [ItemTypes.TABLE, ItemTypes.CHAIR],
    drop: (item: any, monitor) => {
      const offset = monitor.getClientOffset();
      if (!offset || !canvasRef.current) return;

      const rect = canvasRef.current.getBoundingClientRect();
      const newObj = {
        type: item.type,
        x: offset.x - rect.left,
        y: offset.y - rect.top,
        ...item,
      };
      setObjects((prev) => [...prev, newObj]);
    },
  }));

  const handleMouseDown = (e: React.MouseEvent) => {
    const rect = canvasRef.current!.getBoundingClientRect();
    const mouseX = e.clientX - rect.left;
    const mouseY = e.clientY - rect.top;

    for (let i = objects.length - 1; i >= 0; i--) {
      const obj = objects[i];
      const w = obj.tableWidth ?? obj.chairRadius! * 2;
      const h = obj.tableHeight ?? obj.chairRadius! * 2;
      if (
        mouseX >= obj.x &&
        mouseX <= obj.x + w &&
        mouseY >= obj.y &&
        mouseY <= obj.y + h
      ) {
        setDraggingIndex(i);
        dragOffset.current = { x: mouseX - obj.x, y: mouseY - obj.y };
        return;
      }
    }
  };

  const handleMouseMove = (e: React.MouseEvent) => {
    if (draggingIndex === null) return;
    const rect = canvasRef.current!.getBoundingClientRect();
    const mouseX = e.clientX - rect.left;
    const mouseY = e.clientY - rect.top;

    setObjects((objs) =>
      objs.map((obj, i) =>
        i === draggingIndex
          ? { ...obj, x: mouseX - dragOffset.current.x, y: mouseY - dragOffset.current.y }
          : obj
      )
    );
  };

  const handleMouseUp = () => setDraggingIndex(null);

  if (loading) return <div>Загрузка...</div>;

  return (
    <Box sx={{ display: "flex", height: "100vh" }}>
      <Sidebar
        layouts={layouts}
        selectedLayoutId={selectedLayoutId}
        isEditing={isEditing}
        onSelectLayout={setSelectedLayoutId}
        onCreateClick={() => setIsModalOpen(true)}
        onDeleteClick={async (id) => {
          await deleteLayoutService(id);
          loadLayouts();
        }}
        onToggleEdit={() => setIsEditing(!isEditing)}
      />
      <div ref={(node) => dropRef(node) as unknown as void} style={{ flexGrow: 1, display: "flex", justifyContent: "center", alignItems: "center" }}>
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
      <CreateLayoutModal open={isModalOpen} onClose={() => setIsModalOpen(false)} onCreateLayout={createLayout} />
    </Box>
  );
};

export default LayoutEditor;
