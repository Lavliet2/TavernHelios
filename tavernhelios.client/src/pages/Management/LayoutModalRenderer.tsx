import React, { Suspense } from "react";
import type { Layout } from "../../types/Layout";

const CreateLayoutModal = React.lazy(
  () => import("../../components/Management/LayoutEditor/CreateLayoutModal")
);

interface RenderLayoutModalProps {
  open: boolean;
  onClose: () => void;
  onCreateLayout: (data: Partial<Layout>) => Promise<Layout | void>;
}

export const RenderLayoutModal: React.FC<RenderLayoutModalProps> = ({
  open,
  onClose,
  onCreateLayout,
}) => (
  <Suspense fallback={<div>Загрузка модального окна...</div>}>
    <CreateLayoutModal
      open={open}
      onClose={onClose}
      onCreateLayout={onCreateLayout}
    />
  </Suspense>
);
