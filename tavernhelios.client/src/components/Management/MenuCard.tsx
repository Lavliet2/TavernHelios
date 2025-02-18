import React from "react";
import { Card, CardContent, Typography, IconButton, Tooltip } from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import { Menu } from "../../types/Management";

interface Props {
  menu: Menu;
  onDelete: (menuId: string) => void;
}

const MenuCard: React.FC<Props> = ({ menu, onDelete }) => {
  return (
    <Card>
      <CardContent>
        <Typography variant="h6">{menu.name}</Typography>
        <Tooltip title="Удалить меню">
          <IconButton onClick={() => onDelete(menu.id)} color="error">
            <DeleteIcon />
          </IconButton>
        </Tooltip>
      </CardContent>
    </Card>
  );
};

export default MenuCard;
