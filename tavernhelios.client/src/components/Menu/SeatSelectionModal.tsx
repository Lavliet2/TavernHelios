// import React, { useEffect, useState } from "react";
// import { Dialog, DialogTitle, DialogContent, IconButton, CircularProgress } from "@mui/material";
// import CloseIcon from "@mui/icons-material/Close";
// import SeatCanvas from "./SeatCanvas"; // внутренний компонент, рисующий схему

// interface SeatSelectionModalProps {
//   open: boolean;
//   selectedTime: string;
//   onClose: () => void;
//   onSelectSeat: (seatNumber: number, tableName: string, layoutId: string) => void;
// }

// const SeatSelectionModal: React.FC<SeatSelectionModalProps> = ({
//   open,
//   selectedTime,
//   onClose,
//   onSelectSeat,
// }) => {
//   const [reservedSeats, setReservedSeats] = useState<{ seatNumber: number; tableName: string; layoutId: string }[]>([]);
//   const [loading, setLoading] = useState(false);

//   useEffect(() => {
//     if (!open || !selectedTime) return;

//     const selectedDate = new Date();
//     const [hours, minutes] = selectedTime.split(":");
//     selectedDate.setHours(+hours, +minutes, 0, 0);
//     const iso = selectedDate.toISOString();

//     const url = `https://localhost:5555/api/Reservation?IsDeleted=false&BeginDate=${encodeURIComponent(iso)}&EndDate=${encodeURIComponent(iso)}`;

//     setLoading(true);
//     fetch(url)
//       .then((res) => res.json())
//       .then((data) => {
//         const filtered = data.filter((r: any) => r.seatNumber > 0 && r.layoutId);
//         setReservedSeats(filtered);
//       })
//       .catch((err) => console.error("Ошибка при загрузке бронирований:", err))
//       .finally(() => setLoading(false));
//   }, [open, selectedTime]);

//   return (
//     <Dialog open={open} onClose={onClose} fullScreen>
//       <DialogTitle>
//         Выбор места
//         <IconButton onClick={onClose} sx={{ position: "absolute", right: 8, top: 8 }}>
//           <CloseIcon />
//         </IconButton>
//       </DialogTitle>
//       <DialogContent>
//         {loading ? (
//           <CircularProgress />
//         ) : (
//           <SeatCanvas reservedSeats={reservedSeats} onSelectSeat={onSelectSeat} />
//         )}
//       </DialogContent>
//     </Dialog>
//   );
// };

// export default SeatSelectionModal;
