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
  }
  
  export interface DroppedTable extends DroppedObjectBase {
    type: DroppedObjectType.TABLE;
    tableWidth: number;
    tableHeight: number;
  }
  
  export interface DroppedChair extends DroppedObjectBase {
    type: DroppedObjectType.CHAIR;
    chairRadius: number;
  }
  
  export type DroppedObject = DroppedTable | DroppedChair;
  