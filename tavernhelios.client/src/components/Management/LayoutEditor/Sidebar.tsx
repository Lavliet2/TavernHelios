// src/pages/Management/EditLayout/Sidebar.tsx
import React from "react";
import {
  Box,
  Typography,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Button,
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
          <TableItem />
          <ChairItem />
        </Box>
      )}
    </Box>
  );
};

export default Sidebar;
