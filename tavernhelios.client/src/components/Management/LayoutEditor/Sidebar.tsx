import React, { useMemo, useCallback } from "react";
import {
  Box,
  Typography,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Button,
} from "@mui/material";

import { InstructionBlock } from "./InstructionBlock";
import TableControls from "./TableControls";
import ChairControls from "./ChairControls";

import type { SidebarProps } from "../../../types/Layout";

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
  tableWidth,
  setTableWidth,
  tableHeight,
  setTableHeight,
  chairRadius,
  setChairRadius,
}) => {
  const isDuplicateName = useMemo(() => {
    const trimmed = tableName.trim().toLowerCase();
    return isEditing && trimmed !== "" && existingTableNames.includes(trimmed);
  }, [tableName, isEditing, existingTableNames]);

  const isMaxChairsReached = currentSeatCount >= tableSeats;

  const handleToggleEdit = useCallback(() => {
    if (isEditing) onSaveLayout();
    onToggleEdit();
  }, [isEditing, onSaveLayout, onToggleEdit]);

  return (
    <Box
      sx={{
        width: 300,
        p: 2,
        bgcolor: "#f4f4f4",
        display: "flex",
        flexDirection: "column",
        gap: 2,
        maxHeight: "100vh",
        overflowY: "auto",
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

      <Button variant="contained" color="warning" onClick={handleToggleEdit}>
        {isEditing ? "Сохранить" : "Редактировать"}
      </Button>

      {isEditing && (
        <Box sx={{ p: 2, bgcolor: "#ddd", borderRadius: 2 }}>
          <Typography variant="h6">Инструменты</Typography>

          <TableControls
            tableName={tableName}
            setTableName={setTableName}
            tableSeats={tableSeats}
            setTableSeats={setTableSeats}
            tableWidth={tableWidth}
            setTableWidth={setTableWidth}
            tableHeight={tableHeight}
            setTableHeight={setTableHeight}
            isDuplicateName={isDuplicateName}
            isEditing={isEditing}
            objectsLength={objects.length}
          />

          <ChairControls
            chairRadius={chairRadius}
            setChairRadius={setChairRadius}
            tableName={tableName}
            isMaxChairsReached={isMaxChairsReached}
          />
        </Box>
      )}

      <InstructionBlock />
    </Box>
  );
};

export default React.memo(Sidebar);
