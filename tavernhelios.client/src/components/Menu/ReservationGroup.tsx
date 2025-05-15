import React, { useState, useMemo } from "react";
import {
  Paper, Table, TableBody, TableCell, TableContainer,
  TableHead, TableRow, Typography, TableSortLabel
} from "@mui/material";

import dishTypes from "../../constants/dishTypes";

interface Reservation {
  id: string;
  personId: string;
  date: string;
  dishIds: string[];
  tableName: string;
  seatNumber?: number;
}

interface Dish {
  id: string;
  name: string;
  description: string;
  dishType: number;
}

interface Props {
  title: string;
  reservations: Reservation[];
  dishes: Dish[];
}

type Order = "asc" | "desc";

const ReservationGroup: React.FC<Props> = ({ title, reservations, dishes }) => {
  const [orderBy, setOrderBy] = useState<"personId" | "tableName">("personId");
  const [order, setOrder] = useState<Order>("asc");

  const handleSort = (property: "personId" | "tableName") => {
    const isAsc = orderBy === property && order === "asc";
    setOrder(isAsc ? "desc" : "asc");
    setOrderBy(property);
  };

  const sortedReservations = useMemo(() => {
    return [...reservations].sort((a, b) => {
      const aVal = a[orderBy] || "";
      const bVal = b[orderBy] || "";
      return aVal.localeCompare(bVal, "ru", { sensitivity: "base" }) * (order === "asc" ? 1 : -1);
    });
  }, [reservations, orderBy, order]);

  const dishesMap = useMemo(() => {
    return dishes.reduce((acc, dish) => {
      acc[dish.id] = { name: dish.name, type: dish.dishType };
      return acc;
    }, {} as Record<string, { name: string; type: number }>);
  }, [dishes]);

  return (
    <Paper sx={{ p: 2, mb: 3 }}>
      <Typography variant="h5" gutterBottom>{title}</Typography>

      {sortedReservations.length === 0 ? (
        <Typography variant="body1" align="center">Нет бронирований.</Typography>
      ) : (
        <TableContainer>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>
                  <TableSortLabel
                    active={orderBy === "personId"}
                    direction={orderBy === "personId" ? order : "asc"}
                    onClick={() => handleSort("personId")}
                  >
                    <strong>Имя</strong>
                  </TableSortLabel>
                </TableCell>
                <TableCell align="center">
                  <TableSortLabel
                    active={orderBy === "tableName"}
                    direction={orderBy === "tableName" ? order : "asc"}
                    onClick={() => handleSort("tableName")}
                  >
                  </TableSortLabel>
                </TableCell>
                <TableCell align="center">
                  <strong>Место</strong>
                </TableCell>
                {dishTypes.map(type => (
                  <TableCell key={type.value} align="center">
                    <strong>{type.label}</strong>
                  </TableCell>
                ))}
              </TableRow>
            </TableHead>
            <TableBody>
              {sortedReservations.map(reservation => {
                const dishByType: Record<number, string[]> = { 0: [], 1: [], 2: [], 3: [] };

                reservation.dishIds.forEach(dishId => {
                  const dish = dishesMap[dishId];
                  if (dish) {
                    dishByType[dish.type].push(dish.name);
                  }
                });

                return (
                  <TableRow key={reservation.id}>
                    <TableCell>{reservation.personId}</TableCell>
                    <TableCell align="center">{reservation.tableName || "—"}</TableCell>
                    <TableCell align="center">
                      {reservation.seatNumber !== undefined ? reservation.seatNumber : "—"}
                    </TableCell>
                    {dishTypes.map(type => (
                      <TableCell key={type.value} align="center">
                        {dishByType[type.value].length > 0
                          ? dishByType[type.value].join(", ")
                          : "—"}
                      </TableCell>
                    ))}
                  </TableRow>
                );
              })}
            </TableBody>
          </Table>
        </TableContainer>
      )}
    </Paper>
  );
};

export default ReservationGroup;
