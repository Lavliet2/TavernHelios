export interface Point {
    x: number;
    y: number;
  }
  
  export interface Table {
    name: string;
    description: string;
    p1: Point;
    p2: Point;
    p3: Point;
    p4: Point;
  }
  
  export interface Seat {
    number: number;
    description: string;
    center: Point;
  }
  
  export interface Layout {
    id: string;
    isDeleted: boolean;
    restaurantId: string;
    width: number;
    height: number;
    imageStr?: string;
    tables: Table[];
    seats: Seat[];
  }
  