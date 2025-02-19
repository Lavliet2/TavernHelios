import { useEffect, useState } from "react";
import { Dish } from "../../types/Management";
import { fetchDishData, addDish, deleteDish, updateDish } from "../../services/dishService";
import dishTypes from "../../constants/dishTypes";

const useDishes = () => {
  const [DishData, setDishData] = useState<Dish[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [snackbarMessage, setSnackbarMessage] = useState<string | null>(null);
  const [snackbarOpen, setSnackbarOpen] = useState<boolean>(false);
  const [editingDish, setEditingDish] = useState<Dish | null>(null);
  const [isEditModalOpen, setIsEditModalOpen] = useState<boolean>(false);
  const [isAddModalOpen, setIsAddModalOpen] = useState<boolean>(false);
  const [newDish, setNewDish] = useState<Dish>({
    id: "",
    name: "",
    description: "",
    dishType: dishTypes[0].value,
    imageBase64: "",
  });

  useEffect(() => {
    const loadDish = async () => {
      try {
        setLoading(true);
        const data = await fetchDishData();
        setDishData(data);
      } catch (error) {
        setError("Ошибка загрузки меню");
      } finally {
        setLoading(false);
      }
    };
    loadDish();
  }, []);

  const showSnackbar = (message: string) => {
    setSnackbarMessage(message);
    setSnackbarOpen(true);
  };

  const handleAddDish = async () => {
    if (!newDish.name.trim()) {
      showSnackbar("Название блюда обязательно!");
      return;
    }
    try {
      const createdDish = await addDish(newDish);
      setDishData((prev) => [...prev, createdDish]);
      setIsAddModalOpen(false);
      setNewDish({ id: "", name: "", description: "", dishType: dishTypes[0].value, imageBase64: "" });
      showSnackbar("Блюдо добавлено!");
    } catch (error) {
      showSnackbar("Ошибка при добавлении блюда");
    }
  };

  const handleDelete = async (dishId: string) => {
    if (!window.confirm("Вы уверены, что хотите удалить это блюдо?")) return;

    try {
      await deleteDish(dishId);
      setDishData((prev) => prev.filter((dish) => dish.id !== dishId));
      showSnackbar("Блюдо удалено!");
    } catch (error) {
      showSnackbar("Ошибка при удалении блюда");
    }
  };

  const handleEdit = (dish: Dish) => {
    setEditingDish(dish);
    setIsEditModalOpen(true);
  };

  const handleEditSave = async () => {
    if (!editingDish || !editingDish.name.trim()) {
      showSnackbar("Название блюда не может быть пустым!");
      return;
    }

    try {
      await updateDish(editingDish);
      setDishData((prev) =>
        prev.map((dish) => (dish.id === editingDish.id ? editingDish : dish))
      );
      setIsEditModalOpen(false);
      showSnackbar("Блюдо обновлено!");
    } catch (error) {
      showSnackbar("Ошибка при обновлении блюда");
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

  const handleAddChange = (field: keyof Dish, value: any) => {
    setNewDish((prev) => ({ ...prev, [field]: value }));
  };

  const handleEditChange = (field: keyof Dish, value: any) => {
    setEditingDish((prev) => prev ? { ...prev, [field]: value } : prev);
  };

  const handleCancelEdit = () => {
    setIsEditModalOpen(false);
    setEditingDish(null);
  };

  const handleCancelAdd = () => {
    setIsAddModalOpen(false);
    setNewDish({ id: "", name: "", description: "", dishType: dishTypes[0].value, imageBase64: "" });
  };

  return {
    DishData,
    loading,
    error,
    newDish,
    editingDish,
    isEditModalOpen,
    isAddModalOpen,
    snackbarMessage,
    snackbarOpen,
    setSnackbarOpen,
    setSnackbarMessage,
    handleAddDish,
    handleDelete,
    handleEdit,
    handleEditSave,
    handleImageUpload,
    handleAddChange,
    handleEditChange,
    setIsEditModalOpen,
    setIsAddModalOpen,
    handleCancelEdit,
    handleCancelAdd,
  };
};

export default useDishes;
