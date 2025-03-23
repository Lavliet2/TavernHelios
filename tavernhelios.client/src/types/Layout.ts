export interface Point {
  x: number;
  y: number;
}

export const ItemTypes = {
  TABLE: "table",
  CHAIR: "chair",
};

export interface Table {
  name: string;
  description?: string; // <-- количество мест
  p1: Point;
  p2: Point;
  p3: Point;
  p4: Point;
}

export interface Seat {
  number: number;
  description?: string;
  center: Point;
  radius: number;
}

export interface Layout {
  id: string;
  restaurantId: string;
  width: number;
  height: number;
  imageStr?: string;
  tables?: Table[];
  seats?: Seat[];
  isDeleted?: boolean;
}
