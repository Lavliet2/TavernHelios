import { useState, useCallback } from "react";
import { Layout } from "../../types/Layout";
import { DroppedObject } from "../../types/DroppedObject";

import {
  fetchLayouts,
  createLayout,
  deleteLayout,
  updateLayout,
} from "../../services/layoutService";

import { transformObjectsToTables } from "../../utils/transformers/objectsToLayoutTables";
import { useSnackbar } from "../useSnackbar";

export const useLayoutEditorLogic = () => {
  const [layouts, setLayouts] = useState<Layout[]>([]);
  const [selectedLayoutId, setSelectedLayoutId] = useState<string>("");
  const [loading, setLoading] = useState(true);
  const { showSnackbar } = useSnackbar();

  const loadLayouts = useCallback(async (): Promise<Layout[]> => {
    try {
      const data = await fetchLayouts();
      setLayouts(data);
  
      // Проверим, есть ли выбранная схема среди загруженных
      const exists = data.find((l) => l.id === selectedLayoutId);
  
      if (!selectedLayoutId || !exists) {
        if (data.length > 0) {
          setSelectedLayoutId(data[0].id);
        } else {
          setSelectedLayoutId(""); // сбрасываем, если схем вообще нет
        }
      }
  
      return data;
    } catch (error) {
      console.error("Ошибка загрузки схем:", error);
      showSnackbar("Ошибка загрузки схем", "error");
      return [];
    } finally {
      setLoading(false);
    }
  }, [selectedLayoutId, showSnackbar]);
  

  const handleCreateLayout = useCallback(
    async (data: Partial<Layout>) => {
      try {
        const newLayout = await createLayout(data);
        if (newLayout?.id) {
          const layouts = await loadLayouts(); 
          setLayouts(layouts);                 
          setSelectedLayoutId(newLayout.id);   
        }
        return newLayout;
      } catch (err) {
        console.error("Ошибка при создании схемы:", err);
        showSnackbar("Ошибка при создании схемы", "error");
      }
    },
    [loadLayouts, showSnackbar]
  );

  const handleSaveLayout = useCallback(
    async (objects: DroppedObject[]) => {
      if (!selectedLayoutId) return;
      const layout = layouts.find((l) => l.id === selectedLayoutId);
      if (!layout) return;
  
      const updatedLayout: Layout = {
        ...layout,
        tables: transformObjectsToTables(objects),
      };
  
      try {
        await updateLayout(updatedLayout);
        showSnackbar("Схема успешно сохранена!", "success");

        setLayouts((prev) =>
          prev.map((l) => (l.id === updatedLayout.id ? updatedLayout : l))
        );
      } catch (err) {
        console.error("Ошибка при сохранении схемы:", err);
        showSnackbar("Ошибка сохранения схемы", "error");
      }
    },
    [layouts, selectedLayoutId, showSnackbar]
  );
  
  const handleDeleteLayout = useCallback(
    async (id: string) => {
      try {
        await deleteLayout(id);
        const updatedLayouts = await loadLayouts();
        if (updatedLayouts.length > 0) {
          setSelectedLayoutId(updatedLayouts[0].id);
        } else {
          setSelectedLayoutId("");
        }
        showSnackbar("Схема удалена", "info");
      } catch (error) {
        console.error("Ошибка удаления схемы:", error);
        showSnackbar("Ошибка удаления схемы", "error");
      }
    },
    [loadLayouts, showSnackbar]
  );
  
  return {
    layouts,
    selectedLayoutId,
    setSelectedLayoutId,
    loading,
    loadLayouts,
    handleCreateLayout,
    handleSaveLayout,
    handleDeleteLayout,
  };
};
