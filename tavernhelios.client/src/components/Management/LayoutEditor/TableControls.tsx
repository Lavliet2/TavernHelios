import React from "react";
import { Box, TextField, Typography } from "@mui/material";
import { TableItem } from "./LayoutItems";

interface TableControlsProps {
  tableName: string;
  setTableName: (value: string) => void;
  tableSeats: number;
  setTableSeats: (value: number) => void;
  tableWidth: number;
  setTableWidth: (value: number) => void;
  tableHeight: number;
  setTableHeight: (value: number) => void;
  isDuplicateName: boolean;
  isEditing: boolean;
  objectsLength: number;
}

const TableControls: React.FC<TableControlsProps> = ({
  tableName,
  setTableName,
  tableSeats,
  setTableSeats,
  tableWidth,
  setTableWidth,
  tableHeight,
  setTableHeight,
  isDuplicateName,
  isEditing,
}) => {
  return (
    <Box>
      <Typography variant="subtitle1">Настройки стола</Typography>

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

        {isEditing && tableName.trim() !== "" && !isDuplicateName && (
        <TableItem
          tableWidth={tableWidth}
          tableHeight={tableHeight}
          name={tableName}
          seats={tableSeats}
        />
      )}
    </Box>
  );
};

export default React.memo(TableControls);
