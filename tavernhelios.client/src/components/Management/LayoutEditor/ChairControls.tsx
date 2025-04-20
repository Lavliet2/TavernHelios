import React from "react";
import { Box, TextField } from "@mui/material";
import { ChairItem } from "./LayoutItems";

interface ChairControlsProps {
  chairRadius: number;
  setChairRadius: (value: number) => void;
  tableName: string;
  isMaxChairsReached: boolean;
}

const ChairControls: React.FC<ChairControlsProps> = ({
  chairRadius,
  setChairRadius,
  tableName,
  isMaxChairsReached,
}) => {
    const handleRadiusChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        let radius = Number(e.target.value);
        if (isNaN(radius)) radius = 5;
        if (radius < 5) radius = 5;
        setChairRadius(radius);
    };

  const canAddChair = tableName.trim() !== "" && !isMaxChairsReached;

  return (
    <Box>
      <TextField
        label="Радиус стула"
        type="number"
        value={chairRadius}
        onChange={handleRadiusChange}
        slotProps={{input: {inputProps: {min: 5,},},}}
        sx={{ my: 2 }}
        fullWidth
        error={isMaxChairsReached}
        helperText={isMaxChairsReached ? "Все стулья уже добавлены" : ""}
      />

      {canAddChair && <ChairItem chairRadius={chairRadius} name={tableName} />}
    </Box>
  );
};

export default React.memo(ChairControls);
