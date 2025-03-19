// Sidebar.tsx
import React, { useState } from "react";
import {
  Box,
  Typography,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Button,
  TextField,
} from "@mui/material";
import type { Layout } from "../../../types/Layout";
import { TableItem, ChairItem } from "./LayoutItems";

interface SidebarProps {
  layouts: Layout[];
  selectedLayoutId: string;
  isEditing: boolean;

  onSelectLayout: (id: string) => void;
  onCreateClick: () => void;
  onDeleteClick: (id: string) => void;
  onToggleEdit: () => void;
}

const Sidebar: React.FC<SidebarProps> = ({
  layouts,
  selectedLayoutId,
  isEditing,
  onSelectLayout,
  onCreateClick,
  onDeleteClick,
  onToggleEdit,
}) => {
  // Размеры по умолчанию
  const [tableWidth, setTableWidth] = useState(50);
  const [tableHeight, setTableHeight] = useState(50);
  const [chairRadius, setChairRadius] = useState(15);

  return (
    <Box
      sx={{
        width: 300,
        p: 2,
        bgcolor: "#f4f4f4",
        display: "flex",
        flexDirection: "column",
        gap: 2,
      }}
    >
      <Typography variant="h5">Редактор схем зала</Typography>

      <FormControl fullWidth>
        <InputLabel>Выберите зал</InputLabel>
        <Select
          value={selectedLayoutId}
          label="Выберите зал"
          onChange={(e) => onSelectLayout(e.target.value)}
        >
          {layouts.map((layout) => (
            <MenuItem key={layout.id} value={layout.id}>
              {layout.restaurantId}
            </MenuItem>
          ))}
        </Select>
      </FormControl>

      <Button variant="contained" onClick={onCreateClick}>
        Создать новую схему
      </Button>

      <Button
        variant="outlined"
        color="error"
        onClick={() => onDeleteClick(selectedLayoutId)}
      >
        Удалить схему
      </Button>

      <Button variant="contained" color="warning" onClick={onToggleEdit}>
        {isEditing ? "Сохранить" : "Редактировать"}
      </Button>

      {isEditing && (
        <Box sx={{ p: 2, bgcolor: "#ddd", borderRadius: 2 }}>
          <Typography variant="h6">Инструменты</Typography>

          {/* Размеры для стола */}
          <TextField
            label="Ширина стола"
            type="number"
            value={tableWidth}
            onChange={(e) => setTableWidth(Number(e.target.value))}
            sx={{ mb: 1 }}
            fullWidth
          />
          <TextField
            label="Высота стола"
            type="number"
            value={tableHeight}
            onChange={(e) => setTableHeight(Number(e.target.value))}
            sx={{ mb: 2 }}
            fullWidth
          />

          <TableItem tableWidth={tableWidth} tableHeight={tableHeight} />

          {/* Размеры для стула */}
          <TextField
            label="Радиус стула"
            type="number"
            value={chairRadius}
            onChange={(e) => setChairRadius(Number(e.target.value))}
            sx={{ my: 2 }}
            fullWidth
          />

          <ChairItem chairRadius={chairRadius} />
        </Box>
      )}
    </Box>
  );
};

export default Sidebar;
