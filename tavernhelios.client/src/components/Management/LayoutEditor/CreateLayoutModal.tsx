// src/components/Management/LayoutEditor/CreateLayoutModal.tsx
import React, { useState } from "react";
import { Modal, Box, Typography, TextField, Button } from "@mui/material";
import type { Layout } from "../../../types/Layout";

/** Пропсы для CreateLayoutModal */
interface CreateLayoutModalProps {
  /** Флаг, открыт ли Modal */
  open: boolean;
  /** Закрыть Modal */
  onClose: () => void;
  /** Колбэк, когда пользователь нажимает "Сохранить" */
  onCreateLayout: (layoutData: Partial<Layout>) => Promise<void>;
}

/**
 * Модалка для создания новой схемы зала
 */
const CreateLayoutModal: React.FC<CreateLayoutModalProps> = ({
  open,
  onClose,
  onCreateLayout,
}) => {
  // Локальный стейт для полей формы
  const [formData, setFormData] = useState<Partial<Layout>>({
    restaurantId: "",
    width: 800,
    height: 600,
    imageStr: "",
  });

  const handleChange = (field: keyof Layout, value: string | number) => {
    setFormData((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  // Чтение файла
  const handleFileChange = (file?: File) => {
    if (!file) return;
    const reader = new FileReader();
    reader.onloadend = () => {
      if (typeof reader.result === "string") {
        setFormData((prev) => ({
          ...prev,
          imageStr: reader.result,
        }));
      }
    };
    reader.readAsDataURL(file);
  };

  // Нажатие "Сохранить"
  const handleSave = async () => {
    if (!formData.restaurantId) {
      alert("Введите название зала!");
      return;
    }
    // Вызываем переданный колбэк
    await onCreateLayout(formData);
    // Закрываем модалку
    onClose();
    // Сбрасываем форму
    setFormData({
      restaurantId: "",
      width: 800,
      height: 600,
      imageStr: "",
    });
  };

  return (
    <Modal open={open} onClose={onClose}>
      <Box
        sx={{
          p: 3,
          bgcolor: "background.paper",
          margin: "10% auto",
          width: 400,
          borderRadius: 2,
        }}
      >
        <Typography variant="h6" sx={{ mb: 2 }}>
          Новая схема
        </Typography>

        <TextField
          label="Имя зала"
          fullWidth
          value={formData.restaurantId || ""}
          onChange={(e) => handleChange("restaurantId", e.target.value)}
          sx={{ mb: 2 }}
        />

        <TextField
          type="number"
          label="Ширина"
          fullWidth
          value={String(formData.width || 800)}
          onChange={(e) =>
            handleChange("width", parseInt(e.target.value, 10) || 800)
          }
          sx={{ mb: 2 }}
        />

        <TextField
          type="number"
          label="Высота"
          fullWidth
          value={String(formData.height || 600)}
          onChange={(e) =>
            handleChange("height", parseInt(e.target.value, 10) || 600)
          }
          sx={{ mb: 2 }}
        />

        <Button variant="contained" component="label" sx={{ mb: 2 }}>
          Загрузить картинку
          <input
            type="file"
            hidden
            onChange={(e) => {
              const file = e.target.files?.[0];
              handleFileChange(file);
            }}
          />
        </Button>

        <Button
          variant="contained"
          color="success"
          fullWidth
          onClick={handleSave}
        >
          Сохранить
        </Button>
      </Box>
    </Modal>
  );
};

export default CreateLayoutModal;
