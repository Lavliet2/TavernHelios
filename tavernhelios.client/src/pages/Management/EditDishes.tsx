import React from "react";
import { useTranslation } from 'react-i18next';
import { Container, Typography, IconButton, Tooltip } from "@mui/material";
import AddIcon from "@mui/icons-material/Add";

import useDishes from "../../hooks/Management/useDishes";
import DishList from "../../components/Management/DishEditor/DishList";
import DishAddModal from "../../components/Management/DishEditor/DishAddModal";
import DishEditModal from "../../components/Management/DishEditor/DishEditModal";
import DishListSkeleton from "../../components/Management/DishEditor/DishListSkeleton";
import { useSnackbar } from "../../hooks/useSnackbar";

const EditDishes: React.FC = () => {
  const { t } = useTranslation();
  const {
    DishData,
    loading,
    newDish,
    editingDish,
    isEditModalOpen,
    isAddModalOpen,
    handleAddDish,
    handleDelete,
    handleEdit,
    handleEditSave,
    handleImageUpload,
    handleAddChange,
    handleEditChange,
    setIsEditModalOpen,
    setIsAddModalOpen,
  } = useDishes();

  const { showSnackbar } = useSnackbar(); 

  const handleAddDishWrapped = async () => {
    try {
      await handleAddDish();
      showSnackbar("Блюдо успешно добавлено", "success");
    } catch (error: any) {
      showSnackbar(error.message || "Ошибка при добавлении блюда", "error");
    }
  };

  const handleEditSaveWrapped = async () => {
    try {
      await handleEditSave();
      showSnackbar("Изменения успешно сохранены", "success");
    } catch (error: any) {
      showSnackbar(error.message || "Ошибка при редактировании", "error");
    }
  };

  const handleDeleteWrapped = async (id: string) => {
    try {
      await handleDelete(id);
      showSnackbar("Блюдо удалено", "success");
    } catch (error: any) {
      showSnackbar(error.message || "Ошибка при удалении", "error");
    }
  };

  if (loading) {
    return (
      <Container sx={{ mt: 4 }}>
        <Typography variant="h4" color="primary" align="center" sx={{ mb: 4 }}>
          {t('editDishes.title')}
        </Typography>
        <DishListSkeleton />
      </Container>
    );
  }

  return (
    <Container sx={{ mt: 4 }}>
      <Typography variant="h4" color="primary" align="center" sx={{ mb: 4 }}>
        {t('editDishes.title')}
      </Typography>

      <Tooltip title={t('editDishes.dishes.addTooltip')}>
        <IconButton color="success" onClick={() => setIsAddModalOpen(true)} sx={{ mb: 2 }}>
          <AddIcon />
        </IconButton>
      </Tooltip>

      <DishList
        dishes={DishData}
        onEdit={handleEdit}
        onDelete={handleDeleteWrapped} 
      />

      <DishEditModal
        dish={editingDish}
        open={isEditModalOpen}
        onClose={() => setIsEditModalOpen(false)}
        onSave={handleEditSaveWrapped}
        onChange={handleEditChange}
        onImageUpload={(e) => handleImageUpload(e, false)}
      />

      <DishAddModal
        dish={newDish}
        open={isAddModalOpen}
        onClose={() => setIsAddModalOpen(false)}
        onSave={handleAddDishWrapped}
        onChange={handleAddChange}
        onImageUpload={(e) => handleImageUpload(e, true)}
      />
    </Container>
  );
};

export default EditDishes;
