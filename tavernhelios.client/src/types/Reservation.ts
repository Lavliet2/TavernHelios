export interface ReservationRequest {
    personId: string;
    date: string;
    dishIds: string[];
  }

export interface Reservation {
  id: string;
  personId: string;
  date: string;
  dishIds: string[];
  seatNumber: number;
  tableName: string;
  isDeleted: boolean;
  layoutId: string;
}
  
// export interface Reservation {
//     id: string;
//     personId: string;
//     date: string;
//     dishIds: string[];
//     time: string;
// }

