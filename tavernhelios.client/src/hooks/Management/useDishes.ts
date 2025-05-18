import { useEffect, useState } from "react";
import { Dish } from "../../types/Management";
import { fetchDish, addDish, deleteDish, updateDish } from "../../services/dishService";
import dishTypes from "../../constants/dishTypes";
import { useSnackbar } from "../../hooks/useSnackbar";
const useDishes = () => {
  const [DishData, setDishData] = useState<Dish[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
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

  const { showSnackbar } = useSnackbar(); 

  useEffect(() => {
    const loadDish = async () => {
      try {
        setLoading(true);
        const data = await fetchDish();
        setDishData(data);
      } catch {
        setError("Ошибка загрузки меню");
        showSnackbar("Ошибка загрузки меню", "error");
      } finally {
        setLoading(false);
      }
    };
    loadDish();
  }, []);

  const handleAddDish = async () => {
    if (!newDish.name.trim()) {
      throw new Error("Название блюда обязательно!");
    }

    const createdDish = await addDish(newDish);
    setDishData((prev) => [...prev, createdDish]);
    setIsAddModalOpen(false);
    setNewDish({ id: "", name: "", description: "", dishType: dishTypes[0].value, imageBase64: "" });
  };

  const handleDelete = async (dishId: string) => {
    const confirmed = window.confirm("Вы уверены, что хотите удалить это блюдо?");
    if (!confirmed) return;

    await deleteDish(dishId);
    setDishData((prev) => prev.filter((dish) => dish.id !== dishId));
  };

  const handleEdit = (dish: Dish) => {
    setEditingDish(dish);
    setIsEditModalOpen(true);
  };

  const handleEditSave = async () => {
    if (!editingDish || !editingDish.name.trim()) {
      throw new Error("Название блюда не может быть пустым!");
    }

    await updateDish(editingDish);
    setDishData((prev) =>
      prev.map((dish) => (dish.id === editingDish.id ? editingDish : dish))
    );
    setIsEditModalOpen(false);
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
