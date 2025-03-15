import { API_BASE_URL } from "../config";
import { Layout } from "../types/Layout";

export const fetchLayouts = async (): Promise<Layout[]> => {
  try {
    const response = await fetch(`${API_BASE_URL}/api/Layout?IsDeleted=false`);
    console.log('Service fetchLayout', response);
    if (!response.ok) {
      throw new Error("Ошибка при получении схем зала");
    }
    return response.json();
  } catch (error) {
    console.error("Ошибка при получении схем зала:", error);
    throw error;
  }
};

export const createLayout = async (layout: Partial<Layout>): Promise<Layout> => {
  try {
    const response = await fetch(`${API_BASE_URL}/api/Layout`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(layout),
    });

    if (!response.ok) {
      throw new Error("Ошибка при создании схемы зала");
    }

    return response.json();
  } catch (error) {
    console.error("Ошибка при создании схемы зала:", error);
    throw error;
  }
};

export const updateLayout = async (layout: Layout): Promise<Layout> => {
  try {
    const response = await fetch(`${API_BASE_URL}/api/layout`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(layout),
    });

    if (!response.ok) {
      throw new Error("Ошибка при обновлении схемы зала");
    }
    return response.json();
  } catch (error) {
    console.error("Ошибка при обновлении схемы зала:", error);
    throw error;
  }
};

export const deleteLayout = async (layoutId: string): Promise<void> => {
  try {
    const response = await fetch(`${API_BASE_URL}/api/layout`, {
      method: "DELETE",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(layoutId),
    });

    if (!response.ok) {
      throw new Error("Ошибка при удалении схемы зала");
    }
  } catch (error) {
    console.error(`Ошибка при удалении схемы зала: ${layoutId}  -IDDD`, error);
    throw error;
  }
};
