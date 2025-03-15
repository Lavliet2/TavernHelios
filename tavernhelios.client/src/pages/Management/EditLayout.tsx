import React, { useEffect, useState, useRef } from "react";
import { fetchLayouts, createLayout, deleteLayout } from "../../services/layoutService";
import { Layout } from "../../types/Layout";
import { Button, TextField, Typography, Box, Modal, MenuItem, Select, FormControl, InputLabel } from "@mui/material";

const LayoutEditor = () => {
  const [layouts, setLayouts] = useState<Layout[]>([]);
  const [loading, setLoading] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedLayoutId, setSelectedLayoutId] = useState<string>("");
  const [canvasSize, setCanvasSize] = useState({ width: 800, height: 600 });
  const [newLayout, setNewLayout] = useState({ restaurantId: "", width: 800, height: 600, imageStr: "" });
  const canvasRef = useRef<HTMLCanvasElement | null>(null);

  useEffect(() => {
    loadLayouts();
  }, []);

  useEffect(() => {
    updateCanvasSize();
    drawCanvas();
  }, [selectedLayoutId, layouts]);

  const loadLayouts = async () => {
    try {
      const data = await fetchLayouts();
      setLayouts(data);
      if (data.length > 0) {
        setSelectedLayoutId(data[0].id);
        setCanvasSize({ width: data[0].width, height: data[0].height });
      }
    } catch (error) {
      console.error("Ошибка загрузки схем:", error);
    } finally {
      setLoading(false);
    }
  };

  const handleCreateLayout = async () => {
    try {
      const created = await createLayout(newLayout);
      setLayouts((prev) => [...prev, created]);
      setNewLayout({ width: 800, height: 600, restaurantId: "", imageStr: "" });
      setIsModalOpen(false);
    } catch (error) {
      console.error("Ошибка создания схемы:", error);
    }
  };

  const handleDeleteLayout = async (layoutId: string) => {
    try {
      await deleteLayout(layoutId);
      setLayouts((prev) => prev.filter((layout) => layout.id !== layoutId));
      const newSelected = layouts.length > 1 ? layouts[0] : null;
      setSelectedLayoutId(newSelected?.id || "");
      setCanvasSize({ width: newSelected?.width || 800, height: newSelected?.height || 600 });
    } catch (error) {
      console.error(`Ошибка удаления схемы: ${layoutId}ID`, error);
    }
  };

  const updateCanvasSize = () => {
    const selectedLayout = layouts.find((layout) => layout.id === selectedLayoutId);
    if (selectedLayout) {
      setCanvasSize({ width: selectedLayout.width, height: selectedLayout.height });
    }
  };

  const drawCanvas = () => {
    const canvas = canvasRef.current;
    if (!canvas) return;
    const ctx = canvas.getContext("2d");
    if (!ctx) return;

    const selectedLayout = layouts.find((layout) => layout.id === selectedLayoutId);
    if (!selectedLayout || !selectedLayout.imageStr) {
      ctx.clearRect(0, 0, canvas.width, canvas.height);
      return;
    }

    const img = new Image();
    img.src = selectedLayout.imageStr;
    img.onload = () => {
      ctx.clearRect(0, 0, canvas.width, canvas.height);
      ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
    };
  };

  if (loading) return <div>Загрузка...</div>;

  return (
    <Box sx={{ display: "flex", height: "100vh" }}>
      {/* Левая боковая панель */}
      <Box sx={{ width: "300px", p: 2, bgcolor: "#f4f4f4", display: "flex", flexDirection: "column", gap: 2 }}>
        <Typography variant="h5">Редактор схем зала</Typography>

        <FormControl fullWidth>
          <InputLabel id="layout-select-label">Выберите зал</InputLabel>
          <Select
            labelId="layout-select-label"
            value={selectedLayoutId}
            label="Выберите зал"
            onChange={(e) => {
              setSelectedLayoutId(e.target.value);
              updateCanvasSize();
            }}
          >
            {layouts.map((layout) => (
              <MenuItem key={layout.id} value={layout.id}>
                {layout.restaurantId}
              </MenuItem>
            ))}
          </Select>
        </FormControl>

        <Button variant="contained" color="primary" onClick={() => setIsModalOpen(true)}>
          Создать новую схему
        </Button>
        <Button variant="outlined" color="error" onClick={() => handleDeleteLayout(selectedLayoutId)}>
          Удалить выбранную схему
        </Button>

        {/* Панель инструментов */}
        <Box sx={{ mt: 2, p: 2, bgcolor: "#ddd", borderRadius: 2 }}>
          <Typography variant="h6">Инструменты</Typography>
          <Button variant="contained" fullWidth sx={{ mt: 1 }}>
            Добавить стол
          </Button>
          <Button variant="contained" fullWidth sx={{ mt: 1 }}>
            Добавить стул
          </Button>
        </Box>
      </Box>

      {/* Правая часть с Canvas */}
      <Box sx={{ flexGrow: 1, display: "flex", justifyContent: "center", alignItems: "center", bgcolor: "#fff" }}>
        <canvas
          ref={canvasRef}
          width={canvasSize.width}
          height={canvasSize.height}
          style={{
            border: "2px solid black",
            backgroundColor: "#f0f0f0",
          }}
        />
      </Box>

      {/* Модальное окно для создания схемы */}
      <Modal open={isModalOpen} onClose={() => setIsModalOpen(false)}>
        <Box sx={{ p: 3, bgcolor: "background.paper", margin: "10% auto", width: 400, borderRadius: 2 }}>
          <Typography variant="h6" sx={{ mb: 2 }}>Новая схема зала</Typography>
          <TextField
            label="Имя схемы зала"
            fullWidth
            value={newLayout.restaurantId}
            onChange={(e) => setNewLayout({ ...newLayout, restaurantId: e.target.value })}
            sx={{ mb: 2 }}
          />
          <TextField
            type="number"
            label="Ширина"
            fullWidth
            value={newLayout.width}
            onChange={(e) => setNewLayout({ ...newLayout, width: parseInt(e.target.value, 10) })}
            sx={{ mb: 2 }}
          />
          <TextField
            type="number"
            label="Высота"
            fullWidth
            value={newLayout.height}
            onChange={(e) => setNewLayout({ ...newLayout, height: parseInt(e.target.value, 10) })}
            sx={{ mb: 2 }}
          />
          <Button variant="contained" component="label" sx={{ mb: 2 }}>
            Загрузить картинку
            <input type="file" hidden />
          </Button>
          <Button variant="contained" color="success" fullWidth onClick={handleCreateLayout}>
            Сохранить
          </Button>
        </Box>
      </Modal>
    </Box>
  );
};

export default LayoutEditor;
