import React, { useEffect, useState } from "react";
import { Modal, Box, Typography, TextField, Button } from "@mui/material";
import { DroppedObject, DroppedObjectType } from "../../../types/DroppedObject";

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

  const handleChange = <K extends keyof DroppedObject>(field: K, value: DroppedObject[K]) => {
    setFormData((prev) => prev && { ...prev, [field]: value });
  };

  return (
    <Modal open={open} onClose={onClose}>
      <Box sx={{ p: 3, bgcolor: "background.paper", margin: "10% auto", width: 400, borderRadius: 2 }}>
        <Typography variant="h6" sx={{ mb: 2 }}>
          {formData.type === DroppedObjectType.TABLE ? "Редактировать стол" : "Редактировать стул"}
        </Typography>

        <TextField
          label="Название"
          fullWidth
          value={formData.name || ""}
          onChange={(e) => handleChange("name", e.target.value)}
          sx={{ mb: 2 }}
        />

        {formData.type === DroppedObjectType.TABLE && (
          <>
            <TextField
              type="number"
              label="Ширина"
              fullWidth
              value={"tableWidth" in formData ? formData.tableWidth : 50}
              onChange={(e) => handleChange("tableWidth" as any, Number(e.target.value))}
              sx={{ mb: 2 }}
            />

            <TextField
              type="number"
              label="Высота"
              fullWidth
              value={"tableHeight" in formData ? formData.tableHeight : 50}
              onChange={(e) => handleChange("tableHeight" as any, Number(e.target.value))}
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

        {formData.type === DroppedObjectType.CHAIR && (
          <TextField
            type="number"
            label="Радиус"
            fullWidth
            value={"chairRadius" in formData ? formData.chairRadius : 15}
            onChange={(e) => handleChange("chairRadius" as any, Number(e.target.value))}
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
