import React from "react";
import { Box, Typography, Button, Modal } from "@mui/material";

interface MenuAddScheduleModalProps {
  isOpen: boolean;
  onClose: () => void;
  menuData: { id: string; name: string }[];
  selectedMenu: string | null;
  setSelectedMenu: (id: string) => void;
  handleAddMenuToSchedule: () => void;
}

const MenuAddScheduleModal: React.FC<MenuAddScheduleModalProps> = ({
  isOpen,
  onClose,
  menuData,
  selectedMenu,
  setSelectedMenu,
  handleAddMenuToSchedule,
}) => {
  return (
    <Modal open={isOpen} onClose={onClose}>
      <Box sx={{ width: "400px", margin: "auto", padding: "20px", bgcolor: "background.paper", borderRadius: "8px", mt: "10%", boxShadow: 3 }}>
        <Typography variant="h6" align="center" sx={{ mb: 2 }}>
          Выберите меню для добавления
        </Typography>
        {menuData.length === 0 ? (
          <Typography color="textSecondary" align="center">Нет доступных меню</Typography>
        ) : (
          menuData.map((menu) => (
            <Button
              key={menu.id}
              fullWidth
              variant={selectedMenu === menu.id ? "contained" : "outlined"}
              sx={{ mb: 1 }}
              onClick={() => setSelectedMenu(menu.id)}
            >
              {menu.name}
            </Button>
          ))
        )}
        <Box sx={{ textAlign: "center", mt: 2 }}>
          <Button variant="contained" color="success" onClick={handleAddMenuToSchedule}>
            Добавить
          </Button>
          <Button variant="contained" color="secondary" sx={{ ml: 2 }} onClick={onClose}>
            Отмена
          </Button>
        </Box>
      </Box>
    </Modal>
  );
};

export default MenuAddScheduleModal;
