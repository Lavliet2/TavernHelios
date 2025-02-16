import React from "react";
import { Container, Typography, CircularProgress, IconButton, Tooltip, Snackbar, Alert } from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import useDishes from "../../hooks/Management/useDishes";
import DishList from "../../components/Management/DishList";
import DishAddModal from "../../components/Management/DishAddModal";
import DishEditModal from "../../components/Management/DishEditModal";
import DishListSkeleton from "../../components/Management/DishListSkeleton";


const EditDishes: React.FC = () => {
  const {
    DishData,
    loading,
    newDish,
    editingDish,
    isEditModalOpen,
    isAddModalOpen,
    snackbarMessage,
    snackbarOpen,
    setSnackbarOpen,
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

  if (loading) {
    return (
      <Container sx={{ mt: 4 }}>
        <Typography variant="h4" color="primary" align="center" sx={{ mb: 4 }}>
          Редактирование блюд
        </Typography>
        <DishListSkeleton />
      </Container>
    );
  }

  return (
    <Container sx={{ mt: 4 }}>
      <Typography variant="h4" color="primary" align="center" sx={{ mb: 4 }}>
        Редактирование блюд
      </Typography>
      <Tooltip title="Добавить блюдо">
        <IconButton color="success" onClick={() => setIsAddModalOpen(true)} sx={{ mb: 2 }}>
          <AddIcon />
        </IconButton>
      </Tooltip>
      <DishList dishes={DishData} onEdit={handleEdit} onDelete={handleDelete} />
      <DishEditModal dish={editingDish} open={isEditModalOpen} onClose={() => setIsEditModalOpen(false)} onSave={handleEditSave} onChange={handleEditChange} onImageUpload={(e) => handleImageUpload(e, false)} />
      <DishAddModal dish={newDish} open={isAddModalOpen} onClose={() => setIsAddModalOpen(false)} onSave={handleAddDish} onChange={handleAddChange} onImageUpload={(e) => handleImageUpload(e, true)} />
      <Snackbar open={snackbarOpen} autoHideDuration={3000} onClose={() => setSnackbarOpen(false)} anchorOrigin={{ vertical: "bottom", horizontal: "center" }}>
        <Alert severity="info" onClose={() => setSnackbarOpen(false)}>
          {snackbarMessage}
        </Alert>
      </Snackbar>
    </Container>
  );
};

export default EditDishes;
