import React, { useEffect, useState } from "react";
import {
  Container,
  Typography,
  CircularProgress,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Accordion,
  AccordionSummary,
  AccordionDetails,
  IconButton,
  Modal,
  Box,
  TextField,
  Select,
  MenuItem,
  InputLabel,
  FormControl,
  Button,
  Tooltip
} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import AddIcon from "@mui/icons-material/Add";
import PhotoCameraIcon from "@mui/icons-material/PhotoCamera";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import { API_BASE_URL } from "../../config";
import dishTypes from "../../constants/dishTypes";

interface MenuValue {
  id: string;
  name: string;
  description: string;
  dishType: number;
  imageBase64?: string;
}

const EditDishes: React.FC = () => {
  const [menuData, setMenuData] = useState<MenuValue[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [editingDish, setEditingDish] = useState<MenuValue | null>(null);
  const [isEditModalOpen, setIsEditModalOpen] = useState<boolean>(false);
  const [isAddModalOpen, setIsAddModalOpen] = useState<boolean>(false);
  const [newDish, setNewDish] = useState<MenuValue>({
    id: "",
    name: "",
    description: "",
    dishType: dishTypes[0].value,
    imageBase64: "",
  });

  useEffect(() => {
    fetchMenuData();
  }, []);

  const fetchMenuData = async () => {
    try {
      setLoading(true);
      const response = await fetch(`${API_BASE_URL}/api/dish`);
      if (!response.ok) {
        throw new Error("Ошибка при загрузке данных");
      }
      const data: MenuValue[] = await response.json();
      setMenuData(data);
    } catch (error) {
      setError(error instanceof Error ? error.message : "Произошла ошибка");
    } finally {
      setLoading(false);
    }
  };

  const handleAddDish = async () => {
    if (!newDish.name.trim()) {
      alert("Название блюда обязательно!");
      return;
    }
  
    try {
      const response = await fetch(`${API_BASE_URL}/api/dish`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(newDish),
      });
  
      if (response.ok) {
        const createdDish: MenuValue = await response.json();
        setMenuData((prev) => [...prev, createdDish]);
        setIsAddModalOpen(false);
        setNewDish({ id: "", name: "", description: "", dishType: dishTypes[0].value, imageBase64: "" });
      } else {
        throw new Error("Ошибка при добавлении блюда");
      }
    } catch (error) {
      alert(error instanceof Error ? error.message : "Ошибка при добавлении");
    }
  };

  // const handleNewImageUpload = (event: React.ChangeEvent<HTMLInputElement>) => {
  //   const file = event.target.files?.[0];
  //   if (!file) return;
  
  //   const reader = new FileReader();
  //   reader.onloadend = () => {
  //     setNewDish((prev) => ({ ...prev, imageBase64: reader.result as string }));
  //   };
  //   reader.readAsDataURL(file);
  // };

  const handleDelete = async (dishId: string) => {
    if (!window.confirm("Вы уверены, что хотите удалить это блюдо?")) return;

    try {
      const response = await fetch(`${API_BASE_URL}/api/dish`, {
        method: "DELETE",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(dishId)
      });

      if (response.ok) {
        setMenuData((prev) => prev.filter((dish) => dish.id !== dishId));
      } else {
        throw new Error("Ошибка при удалении блюда");
      }
    } catch (error) {
      alert(error instanceof Error ? error.message : "Ошибка при удалении");
    }
  };

  const handleEdit = (dish: MenuValue) => {
    setEditingDish(dish);
    setIsEditModalOpen(true);
  };

  const handleEditSave = async () => {
    if (!editingDish) return;

    try {
      const response = await fetch(`${API_BASE_URL}/api/dish`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(editingDish)
      });

      if (response.ok) {
        setMenuData((prev) =>
          prev.map((dish) => (dish.id === editingDish.id ? editingDish : dish))
        );
        setIsEditModalOpen(false);
      } else {
        throw new Error("Ошибка при обновлении блюда");
      }
    } catch (error) {
      alert(error instanceof Error ? error.message : "Ошибка при обновлении");
    }
  };

  const handleImageUpload = (event: React.ChangeEvent<HTMLInputElement>, isNew: boolean) => {
    const file = event.target.files?.[0];
    if (!file) return;
  
    const reader = new FileReader();
    reader.onloadend = () => {
      if (isNew) {
        setNewDish((prev) => ({ ...prev, imageBase64: reader.result as string }));
      } else {
        setEditingDish((prev) => prev ? { ...prev, imageBase64: reader.result as string } : prev);
      }
    };
    reader.readAsDataURL(file);
  };

  if (loading) {
    return (
      <Container sx={{ mt: 4, textAlign: "center" }}>
        <CircularProgress />
        <Typography variant="body1">Загрузка данных...</Typography>
      </Container>
    );
  }

  if (error) {
    return (
      <Container sx={{ mt: 4, textAlign: "center" }}>
        <Typography variant="body1" color="error">
          {error}
        </Typography>
      </Container>
    );
  }

  // Группируем блюда по `dishType`
  const groupedData = new Map<string, MenuValue[]>();

  menuData.forEach((dish) => {
    const categoryName =
      dishTypes.find((type) => type.value === dish.dishType)?.label || "Другие";

    if (!groupedData.has(categoryName)) {
      groupedData.set(categoryName, []);
    }
    groupedData.get(categoryName)?.push(dish);
  });

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
      {Array.from(groupedData.entries()).map(([category, dishes]) => (
        <Accordion key={category} defaultExpanded>
          <AccordionSummary expandIcon={<ExpandMoreIcon />}>
            <Typography variant="h6">{category}</Typography>
          </AccordionSummary>
          <AccordionDetails>
            <TableContainer component={Paper}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>Название</TableCell>
                    <TableCell>Описание</TableCell>
                    <TableCell>Изображение</TableCell>
                    <TableCell>Действия</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {dishes.map((dish) => (
                    <TableRow key={dish.id}>
                      <TableCell>{dish.name}</TableCell>
                      <TableCell>{dish.description}</TableCell>
                      <TableCell>
                        {dish.imageBase64 && (
                          <img src={dish.imageBase64} alt={dish.name} width="80" />
                        )}
                      </TableCell>
                      <TableCell>
                        <Tooltip title="Редактировать">
                          <IconButton color="primary" onClick={() => handleEdit(dish)}>
                            <EditIcon />
                          </IconButton>
                        </Tooltip>
                        <Tooltip title="Удалить">
                          <IconButton color="error" onClick={() => handleDelete(dish.id)}>
                            <DeleteIcon />
                          </IconButton>
                        </Tooltip>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          </AccordionDetails>
        </Accordion>
      ))}

      {/* Модальное окно для редактирования */}
      <Modal open={isEditModalOpen} onClose={() => setIsEditModalOpen(false)}>
        <Box sx={{ position: "absolute", top: "50%", left: "50%", transform: "translate(-50%, -50%)", bgcolor: "white", p: 4, borderRadius: 2, width: 400 }}>
          <Typography variant="h6" sx={{ mb: 2 }}>
            Редактирование блюда
          </Typography>
          <TextField label="Название" fullWidth value={editingDish?.name || ""} onChange={(e) => setEditingDish((prev) => prev ? { ...prev, name: e.target.value } : null)} sx={{ mb: 2 }} />
          <TextField label="Описание" fullWidth multiline value={editingDish?.description || ""} onChange={(e) => setEditingDish((prev) => prev ? { ...prev, description: e.target.value } : null)} sx={{ mb: 2 }} />
          <FormControl fullWidth sx={{ mb: 2 }}>
            <InputLabel>Тип блюда</InputLabel>
            <Select value={editingDish?.dishType || 0} onChange={(e) => setEditingDish((prev) => prev ? { ...prev, dishType: Number(e.target.value) } : null)}>
              {dishTypes.map((type) => (
                <MenuItem key={type.value} value={type.value}>{type.label}</MenuItem>
              ))}
            </Select>
          </FormControl>
          <Box sx={{ display: "flex", alignItems: "center", gap: 1, mb: 2 }}>
            <Tooltip title="Загрузить изображение">
              <IconButton component="label" color="primary">
                <input type="file" accept="image/*" hidden onChange={(e) => handleImageUpload(e, false)} />
                <PhotoCameraIcon fontSize="large" />
              </IconButton>
            </Tooltip>
            <Typography variant="body2" color="textSecondary">
              Загрузите изображение блюда
            </Typography>
          </Box>

          {/* Предпросмотр текущего изображения или нового загруженного */}
          {editingDish?.imageBase64 && (
            <Box sx={{ mt: 2 }}>
              <img
                src={editingDish.imageBase64}
                alt="Предпросмотр"
                width="100%"
                style={{ maxHeight: "200px", objectFit: "contain", borderRadius: 4 }}
              />
            </Box>
          )}

          <Box sx={{ display: "flex", justifyContent: "flex-end", mt: 3 }}>
            <Button variant="contained" color="primary" onClick={handleEditSave} sx={{ mt: 2 }}>
              Сохранить
            </Button>
          </Box>
        </Box>
      </Modal>
      {/* Модальное окно для добавления */}
      <Modal open={isAddModalOpen} onClose={() => setIsAddModalOpen(false)}>
        <Box sx={{ position: "absolute", top: "50%", left: "50%", transform: "translate(-50%, -50%)", bgcolor: "white", p: 4, borderRadius: 2, width: 400 }}>
          <Typography variant="h6" sx={{ mb: 2 }}>
            Добавить блюдо
          </Typography>
          <TextField
            label="Название"
            fullWidth
            required
            value={newDish.name}
            onChange={(e) => setNewDish({ ...newDish, name: e.target.value })}
            sx={{ mb: 2 }}
          />
          <TextField
            label="Описание"
            fullWidth
            multiline
            value={newDish.description}
            onChange={(e) => setNewDish({ ...newDish, description: e.target.value })}
            sx={{ mb: 2 }}
          />
          <FormControl fullWidth sx={{ mb: 2 }} required>
            <InputLabel>Тип блюда</InputLabel>
            <Select
              value={newDish.dishType}
              onChange={(e) => setNewDish({ ...newDish, dishType: Number(e.target.value) })}
            >
              {dishTypes.map((type) => (
                <MenuItem key={type.value} value={type.value}>{type.label}</MenuItem>
              ))}
            </Select>
          </FormControl>
          <Box sx={{ display: "flex", alignItems: "center", gap: 1, mb: 2 }}>
            <Tooltip title="Загрузить изображение">
              <IconButton component="label" color="primary">
              <input type="file" accept="image/*" hidden onChange={(e) => handleImageUpload(e, true)} />
                <PhotoCameraIcon fontSize="large" />
              </IconButton>
            </Tooltip>
            <Typography variant="body2" color="textSecondary">
              Загрузите изображение блюда
            </Typography>
          </Box>
          {newDish.imageBase64 && (
            <Box sx={{ mt: 2 }}>
              <img
                src={newDish.imageBase64}
                alt="Предпросмотр"
                width="100%"
                style={{ maxHeight: "200px", objectFit: "contain", borderRadius: 4 }}
              />
            </Box>
          )}
          <Box sx={{ display: "flex", justifyContent: "flex-end", mt: 3 }}>
            <Button variant="contained" color="success" onClick={handleAddDish}>
              Добавить
            </Button>
          </Box>
        </Box>
      </Modal>
    </Container>
  );
};

export default EditDishes;
