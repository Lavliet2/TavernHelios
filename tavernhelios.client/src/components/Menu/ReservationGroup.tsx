import React from "react";
import { Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from "@mui/material";

import dishTypes from "../../constants/dishTypes";


interface Reservation {
  id: string;
  personId: string;
  date: string;
  dishIds: string[];
  tableName: string;
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

const ReservationGroup: React.FC<Props> = ({ title, reservations, dishes }) => {
  // Создаем объект { dishId: { name, type } } для быстрого поиска
  const dishesMap = React.useMemo(() => {
    return dishes.reduce((acc, dish) => {
      acc[dish.id] = { name: dish.name, type: dish.dishType };
      return acc;
    }, {} as Record<string, { name: string; type: number }>);
  }, [dishes]);
  return (
    <Paper sx={{ p: 2, mb: 3 }}>
      <Typography variant="h5" gutterBottom>{title}</Typography>
      
      {reservations.length === 0 ? (
        <Typography variant="body1" align="center">Нет бронирований.</Typography>
      ) : (
        <TableContainer>
          <Table>
            {/* Заголовок таблицы */}
            <TableHead>
              <TableRow>
                <TableCell><strong>Имя</strong></TableCell>
                <TableCell align="center"><strong>Стол</strong></TableCell>
                {dishTypes.map(type => (
                  <TableCell key={type.value} align="center"><strong>{type.label}</strong></TableCell>
                ))}
              </TableRow>
            </TableHead>
            <TableBody>
              {reservations.map(reservation => {
                // Заполняем массив блюд по категориям
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
                    <TableCell align="center">
                      {reservation.tableName ? reservation.tableName : "—"}
                    </TableCell>
                    {dishTypes.map(type => (
                      <TableCell key={type.value} align="center">
                        {dishByType[type.value].length > 0 ? dishByType[type.value].join(", ") : "—"}
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
