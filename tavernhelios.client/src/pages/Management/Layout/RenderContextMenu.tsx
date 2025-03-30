import { Menu, MenuItem } from "@mui/material";

export const RenderContextMenu = ({
  contextMenu,
  selectedObject,
  onClose,
  onDelete,
}: {
  contextMenu: { mouseX: number; mouseY: number } | null;
  selectedObject: any;
  onClose: () => void;
  onDelete: () => void;
}) => (
  <Menu
    open={!!contextMenu}
    onClose={onClose}
    anchorReference="anchorPosition"
    anchorPosition={
      contextMenu ? { top: contextMenu.mouseY, left: contextMenu.mouseX } : undefined
    }
  >
    <MenuItem onClick={() => console.log("Редактировать", selectedObject)}>
      Редактировать
    </MenuItem>
    <MenuItem onClick={onDelete}>Удалить</MenuItem>
  </Menu>
);
