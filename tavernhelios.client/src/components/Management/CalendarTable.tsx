import React from "react";

interface CalendarTableProps {
  days: any[];
  selectedDates: string[];
  toggleDateSelection: (date: string) => void;
  handleMouseDown: (date: string) => void;
  handleMouseEnter: (date: string) => void;
}

const CalendarTable: React.FC<CalendarTableProps> = ({ days, selectedDates, handleMouseDown, handleMouseEnter }) => {
  return (
    <table style={{ width: "100%", borderCollapse: "collapse" }}>
      <thead>
        <tr>
          {["Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс"].map((day, index) => (
            <th key={index} style={{
              padding: "12px",
              textAlign: "center",
              backgroundColor: "#1976D2",
              color: day === "Сб" || day === "Вс" ? "red" : "#fff",
              fontWeight: "bold",
              borderBottom: "3px solid #1565C0",
            }}>
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
              {week.map((day, j) =>
                day ? (
                  <td key={day.date}
                    onMouseDown={() => handleMouseDown(day.date)}
                    onMouseEnter={() => handleMouseEnter(day.date)}
                    style={{
                      padding: "14px",
                      textAlign: "center",
                      cursor: "pointer",
                      border: "1px solid rgba(25, 118, 210, 0.2)",
                      backgroundColor: selectedDates.includes(day.date)
                        ? "#1565C0"
                        : day.hasSchedule
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
                ) : (
                  <td key={`empty-${i}-${j}`} style={{ padding: "14px", border: "none" }} />
                )
              )}
            </tr>
          ));
        })()}
      </tbody>
    </table>
  );
};

export default CalendarTable;
