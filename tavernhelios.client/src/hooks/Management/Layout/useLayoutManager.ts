import { useState, useCallback } from "react";
import { Layout } from "../../../types/Layout";
import { fetchLayouts, createLayout, deleteLayout } from "../../../services/layoutService";
import { useSnackbar } from "../../useSnackbar";

export const useLayoutManager = () => {
  const [layouts, setLayouts] = useState<Layout[]>([]);
  const [selectedLayoutId, setSelectedLayoutId] = useState<string>("");
  const [loading, setLoading] = useState(true);
  const { showSnackbar } = useSnackbar();

  const loadLayouts = useCallback(async () => {
    try {
      const data = await fetchLayouts();
      setLayouts(data);

      const exists = data.find((l) => l.id === selectedLayoutId);
      if (!selectedLayoutId || !exists) {
        setSelectedLayoutId(data.length > 0 ? data[0].id : "");
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

  const createNewLayout = useCallback(
    async (data: Partial<Layout>) => {
      try {
        const newLayout = await createLayout(data);
        if (newLayout?.id) {
          const layouts = await loadLayouts();
          setLayouts(layouts);
          setSelectedLayoutId(newLayout.id);
          showSnackbar("Схема создана", "success");
        }
        return newLayout;
      } catch (err) {
        console.error("Ошибка при создании схемы:", err);
        showSnackbar("Ошибка при создании схемы", "error");
      }
    },
    [loadLayouts, showSnackbar]
  );

  const removeLayout = useCallback(
    async (id: string) => {
      try {
        await deleteLayout(id);
        const updated = await loadLayouts();
        setSelectedLayoutId(updated.length > 0 ? updated[0].id : "");
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
    setLayouts,
    selectedLayoutId,
    setSelectedLayoutId,
    loading,
    loadLayouts,
    createNewLayout,
    removeLayout,
  };
};
