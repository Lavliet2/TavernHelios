import { API_BASE_URL } from "../config";
import { Dish } from "../types/Management";

export const fetchDishData = async (): Promise<Dish[]> => {
  try {
    const response = await fetch(`${API_BASE_URL}/api/dish`);
    if (!response.ok) {
      throw new Error("Ошибка при загрузке данных");
    }
    return response.json();
  } catch (error) {
    console.error("Ошибка при получении меню:", error);
    throw error;
  }
};

export const fetchDishById = async (dishId: string): Promise<Dish> => {
    try {
      const response = await fetch(`${API_BASE_URL}/api/dish/${dishId}`);
      if (!response.ok) throw new Error("Ошибка при загрузке блюда");
      return response.json();
    } catch (error) {
      console.error("Ошибка при получении блюда:", error);
      throw error;
    }
  }; 

export const addDish = async (newDish: Dish): Promise<Dish> => {
  try {
    const response = await fetch(`${API_BASE_URL}/api/dish`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(newDish),
    });
    
    if (!response.ok) {
      throw new Error("Ошибка при добавлении блюда");
    }
    return response.json();
  } catch (error) {
    console.error("Ошибка при добавлении блюда:", error);
    throw error;
  }
};

export const deleteDish = async (dishId: string): Promise<void> => {
  try {
    const response = await fetch(`${API_BASE_URL}/api/dish`, {
      method: "DELETE",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(dishId),
    });
    
    if (!response.ok) {
      throw new Error("Ошибка при удалении блюда");
    }
  } catch (error) {
    console.error("Ошибка при удалении блюда:", error);
    throw error;
  }
};

export const updateDish = async (dish: Dish): Promise<void> => {
  try {
    const response = await fetch(`${API_BASE_URL}/api/dish`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(dish),
    });

    if (!response.ok) {
      throw new Error("Ошибка при обновлении блюда");
    }
  } catch (error) {
    console.error("Ошибка при обновлении блюда:", error);
    throw error;
  }
};
