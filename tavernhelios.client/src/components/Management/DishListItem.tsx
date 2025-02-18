import React from "react";
import { ListItem, ListItemText, ListItemAvatar, Avatar, IconButton } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { Dish } from "../../types";

interface DishListItemProps {
  dish: Dish;
  onRemove: () => void;
}

const DishListItem: React.FC<DishListItemProps> = ({ dish, onRemove }) => {
  return (
    <ListItem
      secondaryAction={
        <IconButton edge="end" aria-label="delete" onClick={onRemove} sx={{ color: "error.main", fontSize: 16 }}>
          <CloseIcon fontSize="small" />
        </IconButton>
      }
    >
      <ListItemAvatar>
        <Avatar src={dish.imageBase64 || ""} alt={dish.name} />
      </ListItemAvatar>
      <ListItemText primary={dish.name} />
    </ListItem>
  );
};

export default DishListItem;
