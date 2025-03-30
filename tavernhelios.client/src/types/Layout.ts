import type { DroppedObject } from "../types/DroppedObject";


export interface CanvasInteractionResult {
  handleMouseDown: (e: React.MouseEvent<HTMLCanvasElement>) => void;
  handleMouseMove: (e: React.MouseEvent<HTMLCanvasElement>) => void;
  handleMouseUp: () => void;
  handleContextMenu: (e: React.MouseEvent<HTMLDivElement>) => void;
  contextMenu: { mouseX: number; mouseY: number } | null;
  selectedObject: DroppedObject | null;
  setContextMenu: React.Dispatch<React.SetStateAction<{ mouseX: number; mouseY: number } | null>>;
  setSelectedObject: React.Dispatch<React.SetStateAction<DroppedObject | null>>;
}


export interface Point {
  x: number;
  y: number;
}

export interface Seat {
  number: number;
  description?: string;
  center: Point;
  radius: number;
}

export interface Table {
  name: string;
  description?: string;
  seats?: Seat[];
  p1: Point;
  p2: Point;
  p3: Point;
  p4: Point;
}

export interface Layout {
  id: string;
  restaurantId: string;
  width: number;
  height: number;
  imageStr?: string;
  tables?: Table[];
  isDeleted?: boolean;
}

export interface SidebarProps {
  layouts: Layout[];
  selectedLayoutId: string;
  isEditing: boolean;

  onSelectLayout: (id: string) => void;
  onCreateClick: () => void;
  onDeleteClick: (id: string) => void;
  onToggleEdit: () => void;
  onSaveLayout: () => void;

  tableName: string;
  setTableName: (value: string) => void;
  tableSeats: number;
  setTableSeats: (value: number) => void;
  currentSeatCount: number;

  objects: DroppedObject[];
  existingTableNames: string[];

  tableWidth: number;
  tableHeight: number;
  chairRadius: number;
  setTableWidth: (value: number) => void;
  setTableHeight: (value: number) => void;
  setChairRadius: (value: number) => void;
}

export interface TableControlsProps {
  tableName: string;
  setTableName: (value: string) => void;
  tableSeats: number;
  setTableSeats: (value: number) => void;
  tableWidth: number;
  setTableWidth: (value: number) => void;
  tableHeight: number;
  setTableHeight: (value: number) => void;
  isDuplicateName: boolean;
  isEditing: boolean;
  objectsLength: number;
}

export interface ChairControlsProps {
  chairRadius: number;
  setChairRadius: (value: number) => void;
  tableName: string;
  isMaxChairsReached: boolean;
}
