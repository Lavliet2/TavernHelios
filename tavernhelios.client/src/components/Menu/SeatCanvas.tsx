// import React from "react";
// import { DroppedObjectType } from "../../types/DroppedObject"; // путь под себя
// import useSeatObjects from "./useSeatObjects"; // ваш хук или статика с layout

// interface SeatCanvasProps {
//   reservedSeats: { seatNumber: number; tableName: string; layoutId: string }[];
//   onSelectSeat: (seatNumber: number, tableName: string, layoutId: string) => void;
// }

// const SeatCanvas: React.FC<SeatCanvasProps> = ({ reservedSeats, onSelectSeat }) => {
//   const objects = useSeatObjects(); // mock layout или API-загрузка layout

//   const handleClick = (obj: any) => {
//     if (
//       obj.type === DroppedObjectType.CHAIR &&
//       !reservedSeats.some((r) => r.seatNumber === +obj.name && r.tableName === obj.description)
//     ) {
//       onSelectSeat(+obj.name, obj.description, obj.layoutId);
//     }
//   };

//   return (
//     <div style={{ position: "relative", width: 1000, height: 800 }}>
//       {objects.map((obj, i) =>
//         obj.type === DroppedObjectType.CHAIR ? (
//           <div
//             key={i}
//             onClick={() => handleClick(obj)}
//             style={{
//               position: "absolute",
//               left: obj.x,
//               top: obj.y,
//               width: 30,
//               height: 30,
//               borderRadius: "50%",
//               backgroundColor: reservedSeats.some((r) => r.seatNumber === +obj.name && r.tableName === obj.description)
//                 ? "#aaa"
//                 : "green",
//               color: "white",
//               textAlign: "center",
//               lineHeight: "30px",
//               cursor: "pointer",
//             }}
//           >
//             {obj.name}
//           </div>
//         ) : null
//       )}
//     </div>
//   );
// };

// export default SeatCanvas;
