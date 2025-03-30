// src/components/Management/LayoutEditor/CreateLayoutModal.tsx
import React, { useState } from "react";
import { Modal, Box, Typography, TextField, Button } from "@mui/material";
import type { Layout } from "../../../types/Layout";
import { useSnackbar } from "../../../hooks/useSnackbar";


interface CreateLayoutModalProps {
    open: boolean;
    onClose: () => void;
    onCreateLayout: (layoutData: Partial<Layout>) => Promise<Layout | void>;
}

const CreateLayoutModal: React.FC<CreateLayoutModalProps> = ({
  open,
  onClose,
  onCreateLayout,
}) => {
  const { showSnackbar } = useSnackbar();
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

  const handleFileChange = (file?: File) => {
    if (!file) return;
    const reader = new FileReader();
    reader.onloadend = () => {
      if (typeof reader.result === "string") {
        setFormData((prev) => ({
          ...prev,
          imageStr: reader.result as string,
        }));
      }
    };
    reader.readAsDataURL(file);
  };

  const handleSave = async () => {
    if (!formData.restaurantId) {
      showSnackbar("Введите название зала!", "error");
      return;
    }
  
    const result = await onCreateLayout(formData);
  
    if (result) {
      showSnackbar("Схема успешно создана", "success");
    } else {
      showSnackbar("Не удалось создать схему", "error");
    }
  
    setFormData({
      restaurantId: "",
      width: 800,
      height: 600,
      imageStr: "",
    });
    onClose();
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

        <Button variant="contained" color="success" fullWidth onClick={handleSave}>
          Сохранить
        </Button>
      </Box>
    </Modal>
  );
};

export default CreateLayoutModal;
