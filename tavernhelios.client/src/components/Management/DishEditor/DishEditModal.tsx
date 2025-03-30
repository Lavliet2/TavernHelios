import React from "react";
import { Modal, Box, Typography, TextField, Select, MenuItem, FormControl, InputLabel, IconButton, Tooltip, Button } from "@mui/material";
import PhotoCameraIcon from "@mui/icons-material/PhotoCamera";
import { Dish } from "../../../types/Management";
import dishTypes from "../../../constants/dishTypes";

interface DishEditModalProps {
  dish: Dish | null;
  open: boolean;
  onClose: () => void;
  onSave: () => void;
  onChange: (field: keyof Dish, value: any) => void;
  onImageUpload: (event: React.ChangeEvent<HTMLInputElement>) => void;
}

const DishEditModal: React.FC<DishEditModalProps> = ({ dish, open, onClose, onSave, onChange, onImageUpload }) => {
  if (!dish) return null;

  return (
    <Modal open={open} onClose={onClose}>
      <Box sx={{ position: "absolute", top: "50%", left: "50%", transform: "translate(-50%, -50%)", bgcolor: "white", p: 4, borderRadius: 2, width: 400 }}>
        <Typography variant="h6" sx={{ mb: 2 }}>
          Редактирование блюда
        </Typography>
        <TextField label="Название" fullWidth value={dish.name} onChange={(e) => onChange("name", e.target.value)} sx={{ mb: 2 }} />
        <TextField label="Описание" fullWidth multiline value={dish.description} onChange={(e) => onChange("description", e.target.value)} sx={{ mb: 2 }} />
        <FormControl fullWidth sx={{ mb: 2 }}>
          <InputLabel>Тип блюда</InputLabel>
          <Select value={dish.dishType} onChange={(e) => onChange("dishType", Number(e.target.value))}>
            {dishTypes.map((type) => (
              <MenuItem key={type.value} value={type.value}>{type.label}</MenuItem>
            ))}
          </Select>
        </FormControl>
        <Box sx={{ display: "flex", alignItems: "center", gap: 1, mb: 2 }}>
          <Tooltip title="Загрузить изображение">
            <IconButton component="label" color="primary">
              <input type="file" accept="image/*" hidden onChange={onImageUpload} />
              <PhotoCameraIcon fontSize="large" />
            </IconButton>
          </Tooltip>
          <Typography variant="body2" color="textSecondary">
            Загрузите изображение блюда
          </Typography>
        </Box>
        {dish.imageBase64 && (
          <Box sx={{ mt: 2 }}>
            <img src={dish.imageBase64} alt="Предпросмотр" width="100%" style={{ maxHeight: "200px", objectFit: "contain", borderRadius: 4 }} />
          </Box>
        )}
        <Box sx={{ display: "flex", justifyContent: "flex-end", gap: 2, mt: 3 }}>
          <Button variant="contained" color="primary" onClick={onSave}>
            Сохранить
          </Button>
          <Button variant="contained" color="secondary" onClick={onClose}>
            Отмена
          </Button>
        </Box>
      </Box>
    </Modal>
  );
};

export default DishEditModal;
