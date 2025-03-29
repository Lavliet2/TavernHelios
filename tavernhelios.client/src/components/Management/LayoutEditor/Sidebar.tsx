import React, { useState, useMemo } from "react";
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
import type { DroppedObject } from "../../../pages/Management/EditLayout";
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

  tableName: string;
  setTableName: (value: string) => void;
  tableSeats: number;
  setTableSeats: (value: number) => void;
  currentSeatCount: number;

  objects: DroppedObject[];
  existingTableNames: string[];
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
  tableName,
  setTableName,
  tableSeats,
  setTableSeats,
  currentSeatCount,
  objects,
  existingTableNames,
}) => {
  const [tableWidth, setTableWidth] = useState(50);
  const [tableHeight, setTableHeight] = useState(50);
  // const [tableName, setTableName] = useState("");
  // const [tableSeats, setTableSeats] = useState(4);
  const [chairRadius, setChairRadius] = useState(10);
  const isDuplicateName = useMemo(() => {
    const trimmedName = tableName.trim().toLowerCase();
    return isEditing && trimmedName !== "" && existingTableNames.includes(trimmedName);
  }, [tableName, isEditing, existingTableNames]);
  const isMaxChairsReached = currentSeatCount >= tableSeats;
  
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
            onSaveLayout();
          }
          onToggleEdit();
        }}
      >
        {isEditing ? "Сохранить" : "Редактировать"}
      </Button>

      {isEditing && (
        <Box sx={{ p: 2, bgcolor: "#ddd", borderRadius: 2 }}>
          <Typography variant="h6">Инструменты</Typography>

          <TextField
            error={isDuplicateName}
            helperText={isDuplicateName ? "Такой стол уже существует" : ""}
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

          {isEditing && objects.length > 0 && tableName.trim() !== "" && !isDuplicateName && (
            <TableItem
              tableWidth={tableWidth}
              tableHeight={tableHeight}
              name={tableName}
              seats={tableSeats}
            />
          )}

          <TextField
            label="Радиус стула"
            type="number"
            value={chairRadius}
            onChange={(e) => setChairRadius(Number(e.target.value))}
            sx={{ my: 2 }}
            fullWidth
            error={isMaxChairsReached}
            helperText={
              isMaxChairsReached
                ? "Все стулья уже добавлены"
                : ""
            }
          />


          {/* Стул отображается только если имя стола указано и не превышено кол-во */}
          {tableName.trim() !== "" && currentSeatCount < tableSeats && (
            <ChairItem chairRadius={chairRadius} name={tableName} />
          )}
        </Box>
      )}
    </Box>
  );
};

export default Sidebar;
