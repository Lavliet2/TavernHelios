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
  onSaveLayout: () => void;
}

const Sidebar: React.FC<SidebarProps> = ({
  layouts,
  selectedLayoutId,
  isEditing,
  onSelectLayout,
  onCreateClick,
  onDeleteClick,
  onToggleEdit,
  onSaveLayout,
}) => {
  // Размеры по умолчанию
  const [tableWidth, setTableWidth] = useState(50);
  const [tableHeight, setTableHeight] = useState(50);
  const [tableName, setTableName] = useState("");
  const [tableSeats, setTableSeats] = useState(4);

  const [chairRadius, setChairRadius] = useState(10);

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

      <Button
        variant="contained"
        color="warning"
        onClick={() => {
          if (isEditing) {
            onSaveLayout(); // ✅ Сохраняем, если уже в режиме редактирования
          }
          onToggleEdit(); // ✅ Переключаем режим редактирования
        }}
      >
        {isEditing ? "Сохранить" : "Редактировать"}
      </Button>

      {isEditing && (
        <Box sx={{ p: 2, bgcolor: "#ddd", borderRadius: 2 }}>
          <Typography variant="h6">Инструменты</Typography>

          <TextField
            label="Номер стола"
            value={tableName}
            onChange={(e) => setTableName(e.target.value)}
            sx={{ mb: 1 }}
            fullWidth
          />

          <Box sx={{ display: "flex", gap: 1, mb: 1 }}>
            <TextField
              label="Ширина стола"
              type="number"
              value={tableWidth}
              onChange={(e) => setTableWidth(Number(e.target.value))}
              fullWidth
            />

            <TextField
              label="Высота стола"
              type="number"
              value={tableHeight}
              onChange={(e) => setTableHeight(Number(e.target.value))}
              fullWidth
            />
          </Box>

          <TextField
            label="Количество мест"
            type="number"
            value={tableSeats}
            onChange={(e) => setTableSeats(Number(e.target.value))}
            sx={{ mb: 2 }}
            fullWidth
          />

          <TableItem
            tableWidth={tableWidth}
            tableHeight={tableHeight}
            name={tableName}
            seats={tableSeats}
          />

          {/* Стулья автоматически наследуют имя стола */}
          <ChairItem chairRadius={chairRadius} name={tableName} />
        </Box>
      )}
    </Box>
  );
};

export default Sidebar;
