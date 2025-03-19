// src/pages/Management/EditLayout/index.tsx
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
}

const LayoutEditor: React.FC = () => {
  const [loading, setLoading] = useState(true);

  // Редактирование
  const [isEditing, setIsEditing] = useState(false);

  // Модалка "Создать схему"
  const [isModalOpen, setIsModalOpen] = useState(false);

  // Список схем, выбранная схема
  const [layouts, setLayouts] = useState<Layout[]>([]);
  const [selectedLayoutId, setSelectedLayoutId] = useState<string>("");

  // Canvas
  const [canvasSize, setCanvasSize] = useState({ width: 800, height: 600 });
  const canvasRef = useRef<HTMLCanvasElement | null>(null);

  // "Объекты" на канвасе
  const [objects, setObjects] = useState<DroppedObject[]>([]);
  const [originalObjects, setOriginalObjects] = useState<DroppedObject[]>([]);

  // 1) Загружаем схемы
  useEffect(() => {
    loadLayouts();
  }, []);

  // 2) Перерисовка при изменениях
  useEffect(() => {
    updateCanvasSize();
    drawCanvas();
  }, [selectedLayoutId, objects, layouts]);

  // Загрузить список схем
  const loadLayouts = async () => {
    try {
      const data = await fetchLayouts();
      setLayouts(data);
      if (data.length > 0) {
        setSelectedLayoutId(data[0].id);
        setCanvasSize({
          width: data[0].width,
          height: data[0].height,
        });
      }
    } catch (error) {
      console.error("Ошибка загрузки схем:", error);
    } finally {
      setLoading(false);
    }
  };

  // Подогнать размеры canvas
  const updateCanvasSize = () => {
    const layout = layouts.find((l) => l.id === selectedLayoutId);
    if (layout) {
      setCanvasSize({
        width: layout.width || 800,
        height: layout.height || 600,
      });
      // Если у layout есть сохранённые objects, можно load objects...
    }
  };

  // Отрисовка canvas
  const drawCanvas = () => {
    const canvas = canvasRef.current;
    if (!canvas) return;

    const ctx = canvas.getContext("2d");
    if (!ctx) return;

    ctx.clearRect(0, 0, canvas.width, canvas.height);

    const layout = layouts.find((l) => l.id === selectedLayoutId);
    if (layout?.imageStr) {
      const img = new Image();
      img.src = layout.imageStr;
      img.onload = () => {
        ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
        drawObjects(ctx);
      };
    } else {
      drawObjects(ctx);
    }
  };

  // "Рисуем" объекты
  const drawObjects = (ctx: CanvasRenderingContext2D) => {
    objects.forEach((obj) => {
      if (obj.type === ItemTypes.TABLE) {
        ctx.fillStyle = "brown";
        ctx.fillRect(obj.x, obj.y, 50, 50);
      } else {
        ctx.fillStyle = "gray";
        ctx.beginPath();
        ctx.arc(obj.x + 15, obj.y + 15, 15, 0, 2 * Math.PI);
        ctx.fill();
      }
    });
  };

  // DnD Drop
  const [, dropRef] = useDrop(() => ({
    accept: [ItemTypes.TABLE, ItemTypes.CHAIR],
    drop: (item: { type: string }, monitor) => {
      if (!canvasRef.current) return;
      const offset = monitor.getClientOffset();
      if (!offset) return;

      const rect = canvasRef.current.getBoundingClientRect();
      const x = offset.x - rect.left;
      const y = offset.y - rect.top;

      setObjects((prev) => [...prev, { type: item.type, x, y }]);
    },
  }));

  // Создать схему
  const handleCreateLayout = async (layoutData: Partial<Layout>) => {
    try {
      const created = await createLayout(layoutData);
      setLayouts((prev) => [...prev, created]);
    } catch (error) {
      console.error("Ошибка при создании схемы:", error);
    }
  };

  // Удалить схему
  const handleDeleteLayout = async (id: string) => {
    if (!id) return;
    try {
      await deleteLayout(id);
      setLayouts((prev) => prev.filter((l) => l.id !== id));
      if (selectedLayoutId === id) {
        setSelectedLayoutId("");
        setObjects([]);
      }
    } catch (error) {
      console.error("Ошибка удаления схемы:", error);
    }
  };

  // "Редактировать" / "Сохранить"
  const handleToggleEdit = async () => {
    if (!isEditing) {
      setOriginalObjects(objects);
      setIsEditing(true);
    } else {
      // Сохранить изменения
      const layout = layouts.find((l) => l.id === selectedLayoutId);
      if (!layout) return;

      try {
        const updatedLayout: Layout = {
          ...layout,
          // TODO: если у вас есть поле layout.objects:
          // objects: objects
        };
        const result = await updateLayout(updatedLayout);
        // Обновим стейт
        setLayouts((prev) =>
          prev.map((l) => (l.id === result.id ? result : l))
        );
        setIsEditing(false);
      } catch (error) {
        console.error("Ошибка при сохранении схемы:", error);
      }
    }
  };

  // "Отмена"
  const handleCancelEdit = () => {
    setObjects(originalObjects);
    setIsEditing(false);
  };

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
        onToggleEdit={handleToggleEdit}
        // Если захотите добавить onCancelEdit
      />

      <div
        // Убираем "return" из колбэка => нет конфликтов
        ref={(node) => {
          if (node) dropRef(node);
        }}
        style={{
          flexGrow: 1,
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          backgroundColor: "#fff",
        }}
      >
        <canvas
          ref={canvasRef}
          width={canvasSize.width}
          height={canvasSize.height}
          style={{ border: "2px solid black" }}
        />
      </div>

      <CreateLayoutModal
        open={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onCreateLayout={handleCreateLayout}
      />
    </Box>
  );
};

export default LayoutEditor;
