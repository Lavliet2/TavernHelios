import React, { useState } from "react";
import { Box, Typography, Button, Modal } from "@mui/material";
import MenuCard from "../../components/Management/MenuCard";
import { Menu } from "../../types/Management";

interface MenuAddScheduleModalProps {
  isOpen: boolean;
  onClose: () => void;
  menuData: Menu[];
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
  const [expandedMenu, setExpandedMenu] = useState<Menu | null>(null);

  const handleMenuClick = (menu: Menu) => {
    setSelectedMenu(menu.id);
    setExpandedMenu((prev) => (prev?.id === menu.id ? null : menu));
  };

  return (
    <Modal open={isOpen} onClose={onClose}>
      <Box
        sx={{
          width: "400px",
          margin: "auto",
          padding: "20px",
          bgcolor: "background.paper",
          borderRadius: "8px",
          mt: "10%",
          boxShadow: 3,
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          maxHeight: "90vh",
          overflowY: "auto",
        }}
      >
        <Typography variant="h6" align="center" sx={{ mb: 2 }}>
          Выберите меню для добавления
        </Typography>

        {menuData.length === 0 ? (
          <Typography color="textSecondary" align="center">
            Нет доступных меню
          </Typography>
        ) : (
          menuData.map((menu) => (
            <Button
              key={menu.id}
              fullWidth
              variant={selectedMenu === menu.id ? "contained" : "outlined"}
              sx={{ mb: 1 }}
              onClick={() => handleMenuClick(menu)}
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

        {/* Блок с карточкой блюд, который отображается под кнопками */}
        {expandedMenu && (
          <Box sx={{ mt: 3, width: "100%", border: "1px solid #ddd", borderRadius: 2, p: 2, bgcolor: "#f9f9f9",
            maxHeight: "300px", overflowY: "auto", 
           }}>
            <MenuCard menu={expandedMenu} />
          </Box>
        )}
      </Box>
    </Modal>
  );
};

export default MenuAddScheduleModal;
