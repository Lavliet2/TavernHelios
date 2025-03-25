import React, { useEffect, useState } from "react";
import { Modal, Box, Typography, TextField, Button } from "@mui/material";
import { DroppedObject } from "../../../pages/Management/EditLayout";

interface EditTableModalProps {
  open: boolean;
  object: DroppedObject | null;
  onClose: () => void;
  onSave: (updatedObj: DroppedObject) => void;
}

const EditTableModal: React.FC<EditTableModalProps> = ({
  open,
  object,
  onClose,
  onSave,
}) => {
  const [formData, setFormData] = useState<DroppedObject | null>(null);

  useEffect(() => {
    setFormData(object);
  }, [object]);

  if (!formData) return null;

  const handleChange = (field: keyof DroppedObject, value: any) => {
    setFormData((prev) => prev && { ...prev, [field]: value });
  };

  return (
    <Modal open={open} onClose={onClose}>
      <Box sx={{ p: 3, bgcolor: "background.paper", margin: "10% auto", width: 400, borderRadius: 2 }}>
        <Typography variant="h6" sx={{ mb: 2 }}>
          {formData.type === "table" ? "Редактировать стол" : "Редактировать стул"}
        </Typography>

        <TextField
          label="Название"
          fullWidth
          value={formData.name || ""}
          onChange={(e) => handleChange("name", e.target.value)}
          sx={{ mb: 2 }}
        />

        {formData.type === "table" && (
          <>
            <TextField
              type="number"
              label="Ширина"
              fullWidth
              value={formData.tableWidth || 50}
              onChange={(e) => handleChange("tableWidth", Number(e.target.value))}
              sx={{ mb: 2 }}
            />

            <TextField
              type="number"
              label="Высота"
              fullWidth
              value={formData.tableHeight || 50}
              onChange={(e) => handleChange("tableHeight", Number(e.target.value))}
              sx={{ mb: 2 }}
            />

            <TextField
              label="Количество мест"
              type="number"
              fullWidth
              value={formData.description || ""}
              onChange={(e) => handleChange("description", e.target.value)}
              sx={{ mb: 2 }}
            />
          </>
        )}

        {formData.type === "chair" && (
          <TextField
            type="number"
            label="Радиус"
            fullWidth
            value={formData.chairRadius || 15}
            onChange={(e) => handleChange("chairRadius", Number(e.target.value))}
            sx={{ mb: 2 }}
          />
        )}

        <Button
          variant="contained"
          color="success"
          fullWidth
          onClick={() => onSave(formData)}
        >
          Сохранить изменения
        </Button>
      </Box>
    </Modal>
  );
};

export default EditTableModal;
