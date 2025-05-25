import React from "react";
import { Modal, Box, Typography, TextField, Button } from "@mui/material";

interface MenuCreateModalProps {
  open: boolean;
  onClose: () => void;
  onCreate: (menuName: string) => void;
}

const MenuCreateModal: React.FC<MenuCreateModalProps> = ({ open, onClose, onCreate }) => {
  const [menuName, setMenuName] = React.useState<string>("");

  const handleCreate = () => {
    if (!menuName.trim()) return;
    onCreate(menuName);
    setMenuName("");
  };

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
        <Typography variant="h6" sx={{ mb: 2 }}>Создать новое меню</Typography>
        <TextField
          label={ "Введите название меню" }
          fullWidth
          variant="outlined"
          value={menuName}
          onChange={(e) => setMenuName(e.target.value)}
          sx={{ mb: 2 }}
        />
        <Box sx={{ display: "flex", justifyContent: "flex-end" }}>
          <Button
            variant="contained"
            onClick={handleCreate}
            color="primary"
            sx={{ mr: 2 }}
            disabled={!menuName.trim()}
          >
            Создать
          </Button>
          <Button variant="contained" color="secondary" onClick={onClose}>
            Отмена
          </Button>
        </Box>
      </Box>
    </Modal>
  );
};

export default MenuCreateModal;
