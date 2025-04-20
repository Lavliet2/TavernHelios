import React from "react";
import { Modal, Box, Typography, List, ListItem, ListItemText, ListItemAvatar, Avatar, Button } from "@mui/material";
import { Dish } from "../../../types/Management";

interface MenuAddDishModalProps {
  open: boolean;
  onClose: () => void;
  availableDishes: Dish[];
  onSelectDish: (dishId: string) => void;
}

const MenuAddDishModal: React.FC<MenuAddDishModalProps> = ({ open, onClose, availableDishes, onSelectDish }) => {
  return (
    <Modal open={open} onClose={onClose}>
      <Box
        sx={{
          position: "absolute",
          top: "50%",
          left: "50%",
          transform: "translate(-50%, -50%)",
          bgcolor: "white",
          p: 4,
          borderRadius: 2,
          width: 400,
        }}
      >
        <Typography variant="h6" sx={{ mb: 2 }}>Добавить блюдо</Typography>

        {availableDishes.length > 0 ? (
          <List>
            {availableDishes.map((dish) => (
              <ListItem
                key={dish.id}
                component="div"
                onClick={() => onSelectDish(dish.id)}
                sx={{ cursor: "pointer", "&:hover": { bgcolor: "action.hover" } }}
              >
                <ListItemAvatar>
                  <Avatar src={dish.imageBase64 || ""} alt={dish.name} />
                </ListItemAvatar>
                <ListItemText primary={dish.name} />
              </ListItem>
            ))}
          </List>
        ) : (
          <Typography variant="body2" color="text.secondary">
            Нет доступных блюд для добавления
          </Typography>
        )}

        <Box sx={{ display: "flex", justifyContent: "flex-end", mt: 2 }}>
          <Button variant="contained" color="secondary" onClick={onClose}>
            Отмена
          </Button>
        </Box>
      </Box>
    </Modal>
  );
};

export default MenuAddDishModal;
