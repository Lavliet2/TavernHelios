export enum DroppedObjectType {
    TABLE = "TABLE",
    CHAIR = "CHAIR",
  }
  
  export interface DroppedObjectBase {
    type: DroppedObjectType;
    x: number;
    y: number;
    name?: string;
    description?: string;
    //
    // seatNumber?: number;
  }
  
  export interface DroppedTable extends DroppedObjectBase {
    type: DroppedObjectType.TABLE;
    tableWidth: number;
    tableHeight: number;
  }
  
  export interface DroppedChair extends DroppedObjectBase {
    type: DroppedObjectType.CHAIR;
    chairRadius: number;
    seatNumber: number; 
  }
  
  export type DroppedObject = DroppedTable | DroppedChair;
  