import { API_BASE_URL } from "../config";
import { Menu } from "../types/Management";

export const fetchMenus = async (): Promise<Menu[]> => {
  try {
    const response = await fetch(`${API_BASE_URL}/api/Menu`);
    if (!response.ok) throw new Error("Ошибка при загрузке меню");
    return response.json();
  } catch (error) {
    console.error("Ошибка при получении меню:", error);
    throw error;
  }
};

export const addMenu = async (menuName: string): Promise<Menu> => {
  try {
    const response = await fetch(`${API_BASE_URL}/api/Menu`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ name: menuName, dishes: [] }),
    });

    if (!response.ok) throw new Error("Ошибка при создании меню");
    return response.json();
  } catch (error) {
    console.error("Ошибка при создании меню:", error);
    throw error;
  }
};

export const updateMenu = async (updatedMenu: Menu): Promise<void> => {
    try {
      const response = await fetch(`${API_BASE_URL}/api/Menu/${updatedMenu.id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(updatedMenu),
      });
  
      if (!response.ok) throw new Error("Ошибка при обновлении меню");
    } catch (error) {
      console.error("Ошибка при обновлении меню:", error);
      throw error;
    }
};
  
export const deleteMenu = async (menuId: string): Promise<void> => {
try {
    const response = await fetch(`${API_BASE_URL}/api/Menu/${menuId}`, {
    method: "DELETE",
    headers: { "Content-Type": "application/json" },
    });

    if (!response.ok) throw new Error("Ошибка при удалении меню");
} catch (error) {
    console.error("Ошибка при удалении меню:", error);
    throw error;
}
};
  

