import React from "react";
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Accordion, AccordionSummary, AccordionDetails, IconButton, Tooltip, Typography } from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";

import { Dish } from "../../../types/Management";
import dishTypes from "../../../constants/dishTypes";

interface DishListProps {
  dishes: Dish[];
  onEdit: (dish: Dish) => void;
  onDelete: (dishId: string) => void;
}

const DishList: React.FC<DishListProps> = ({ dishes, onEdit, onDelete }) => {
  // Группируем блюда по `dishType`
  const groupedData = new Map<string, Dish[]>();

  dishes.forEach((dish) => {
    const categoryName = dishTypes.find((type) => type.value === dish.dishType)?.label || "Другие";

    if (!groupedData.has(categoryName)) {
      groupedData.set(categoryName, []);
    }
    groupedData.get(categoryName)?.push(dish);
  });

  return (
    <>
      {Array.from(groupedData.entries()).map(([category, dishes]) => (
        <Accordion key={category} defaultExpanded>
          <AccordionSummary expandIcon={<ExpandMoreIcon />}>
            <Typography variant="h6">{category}</Typography>
          </AccordionSummary>
          <AccordionDetails>
            <TableContainer component={Paper}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>Название</TableCell>
                    <TableCell>Описание</TableCell>
                    <TableCell>Изображение</TableCell>
                    <TableCell>Действия</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {dishes.map((dish) => (
                    <TableRow key={dish.id}>
                      <TableCell>{dish.name}</TableCell>
                      <TableCell>{dish.description}</TableCell>
                      <TableCell>
                        {dish.imageBase64 && (
                          <img src={dish.imageBase64} alt={dish.name} width="80" />
                        )}
                      </TableCell>
                      <TableCell>
                        <Tooltip title="Редактировать">
                          <IconButton color="primary" onClick={() => onEdit(dish)}>
                            <EditIcon />
                          </IconButton>
                        </Tooltip>
                        <Tooltip title="Удалить">
                          <IconButton color="error" onClick={() => onDelete(dish.id)}>
                            <DeleteIcon />
                          </IconButton>
                        </Tooltip>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          </AccordionDetails>
        </Accordion>
      ))}
    </>
  );
};

export default DishList;
