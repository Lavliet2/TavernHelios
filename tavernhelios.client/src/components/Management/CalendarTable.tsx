import React, { useState } from "react";
import { Tooltip, Box, Modal, Button } from "@mui/material";
import { Menu } from "../../types/Management";
import MenuCard from "../../components/Management/MenuCard";

interface CalendarTableProps {
  days: any[];
  scheduleData: any[];
  selectedDates: string[];
  handleMouseDown: (date: string) => void;
  handleMouseEnter: (date: string) => void;
}

const CalendarTable: React.FC<CalendarTableProps> = ({
  days,
  selectedDates,
  scheduleData,
  handleMouseDown,
  handleMouseEnter,
}) => {
  const [hoveredMenu, setHoveredMenu] = useState<Menu | null>(null);
  const [open, setOpen] = useState(false);
  const [hoverTimeout, setHoverTimeout] = useState<NodeJS.Timeout | null>(null);
  const [justOpened, setJustOpened] = useState(false); // Флаг для защиты от мгновенного закрытия

  const handleMouseOver = (date: string) => {
    if (hoverTimeout) clearTimeout(hoverTimeout);

    const timeout = setTimeout(() => {
      const scheduledMenu = scheduleData.find((s) => s.dateTime.startsWith(date));
      if (scheduledMenu) {
        setHoveredMenu(scheduledMenu.menu);
        setOpen(true);
        setJustOpened(true); 
        setTimeout(() => setJustOpened(false), 500); 
      }
    }, 1500);

    setHoverTimeout(timeout);
  };

  const handleMouseLeave = () => {
    if (hoverTimeout) clearTimeout(hoverTimeout);

    setTimeout(() => {
      if (!justOpened) {
        setOpen(false);
      }
    }, 300);
  };

  return (
    <>
      <table style={{ width: "100%", borderCollapse: "collapse" }}>
        <thead>
          <tr>
            {["Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс"].map((day, index) => (
              <th
                key={index}
                style={{
                  padding: "12px",
                  textAlign: "center",
                  backgroundColor: "#1976D2",
                  color: day === "Сб" || day === "Вс" ? "red" : "#fff",
                  fontWeight: "bold",
                  borderBottom: "3px solid #1565C0",
                }}
              >
                {day}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {(() => {
            const rows = [];
            for (let i = 0; i < days.length; i += 7) {
              rows.push(days.slice(i, i + 7));
            }
            return rows.map((week, i) => (
              <tr key={i}>
                {week.map((day, j) => {
                  if (!day) return <td key={`empty-${i}-${j}`} style={{ padding: "14px", border: "none" }} />;

                  const scheduledMenu = scheduleData.find((s) => s.dateTime.startsWith(day.date));
                  const menuName = scheduledMenu ? scheduledMenu.menu.name : "";

                  return (
                    <Tooltip key={day.date} title={menuName} arrow disableHoverListener={!menuName}>
                      <td
                        onMouseDown={() => handleMouseDown(day.date)}
                        onMouseEnter={() => handleMouseEnter(day.date)}
                        onMouseOver={() => handleMouseOver(day.date)}
                        onMouseLeave={handleMouseLeave}
                        style={{
                          padding: "14px",
                          textAlign: "center",
                          cursor: "pointer",
                          border: "1px solid rgba(25, 118, 210, 0.2)",
                          backgroundColor: selectedDates.includes(day.date)
                            ? "#1565C0"
                            : scheduledMenu
                            ? "#66BB6A"
                            : day.isToday
                            ? "#BBDEFB"
                            : "white",
                          color: selectedDates.includes(day.date) ? "white" : day.isWeekend ? "red" : "black",
                          fontWeight: day.isWeekend ? "bold" : "normal",
                          borderRadius: "4px",
                          transition: "0.2s",
                          boxShadow: selectedDates.includes(day.date) ? "0px 4px 8px rgba(0,0,0,0.2)" : "none",
                          userSelect: "none",
                        }}
                      >
                        {day.day}
                      </td>
                    </Tooltip>
                  );
                })}
              </tr>
            ));
          })()}
        </tbody>
      </table>

      {/* Всплывающее окно с `MenuCard` */}
      <Modal
        open={open}
        onClose={() => setOpen(false)}
      >
        <Box
          sx={{
            position: "absolute",
            top: "50%",
            left: "50%",
            transform: "translate(-50%, -50%)",
            width: 420,
            bgcolor: "background.paper",
            boxShadow: 24,
            p: 3,
            borderRadius: 2,
          }}
        >
          {hoveredMenu && <MenuCard menu={hoveredMenu} />}
          <Box sx={{ display: "flex", justifyContent: "flex-end", mt: 3 }}>
            <Button onClick={() => setOpen(false)} variant="contained" color="secondary" >
              Отмена
            </Button>
          </Box>
        </Box>
      </Modal>
    </>
  );
};

export default CalendarTable;
