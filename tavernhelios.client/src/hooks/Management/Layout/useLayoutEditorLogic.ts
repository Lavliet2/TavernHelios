import { useCallback } from "react";
import { DroppedObject } from "../../../types/DroppedObject";
import { Layout } from "../../../types/Layout";
import { updateLayout } from "../../../services/layoutService";
import { transformObjectsToTables } from "../../../utils/transformers/objectsToLayoutTables";
import { useSnackbar } from "../../useSnackbar";

export const useLayoutEditorLogic = (
    layouts: Layout[],
    selectedLayoutId: string,
    setLayouts: React.Dispatch<React.SetStateAction<Layout[]>>
) => {
  const { showSnackbar } = useSnackbar();

  const handleSaveLayout = useCallback(
    async (objects: DroppedObject[]) => {
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
    [layouts, selectedLayoutId, setLayouts, showSnackbar]
  );

  return { handleSaveLayout };
};
