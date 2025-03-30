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

export const useLayoutEditorLogic = () => {
  const [layouts, setLayouts] = useState<Layout[]>([]);
  const [selectedLayoutId, setSelectedLayoutId] = useState<string>("");
  const [loading, setLoading] = useState(true);

  const loadLayouts = useCallback(async () => {
    try {
      const data = await fetchLayouts();
      setLayouts(data);
      if (data.length > 0) setSelectedLayoutId(data[0].id);
    } catch (error) {
      console.error("Ошибка загрузки схем:", error);
    } finally {
      setLoading(false);
    }
  }, []);

  const handleCreateLayout = useCallback(
    async (data: Partial<Layout>) => {
      try {
        const newLayout = await createLayout(data);
        if (newLayout?.id) {
          await loadLayouts();
          setSelectedLayoutId(newLayout.id);
        }
        return newLayout;
      } catch (err) {
        console.error("Ошибка при создании схемы:", err);
        alert("Ошибка при создании схемы");
      }
    },
    [loadLayouts]
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
        alert("Схема успешно сохранена!");
      } catch (err) {
        console.error("Ошибка при сохранении схемы:", err);
        alert("Ошибка сохранения схемы");
      }
    },
    [layouts, selectedLayoutId]
  );

  const handleDeleteLayout = useCallback(
    async (id: string) => {
      await deleteLayout(id);
      await loadLayouts();
    },
    [loadLayouts]
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
