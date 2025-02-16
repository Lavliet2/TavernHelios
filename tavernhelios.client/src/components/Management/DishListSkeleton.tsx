import React from "react";
import { Skeleton, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper } from "@mui/material";

const DishListSkeleton: React.FC = () => {
  return (
    <TableContainer component={Paper}>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell><Skeleton variant="text" width={100} height={30} /></TableCell>
            <TableCell><Skeleton variant="text" width={150} height={30} /></TableCell>
            <TableCell><Skeleton variant="text" width={80} height={30} /></TableCell>
            <TableCell><Skeleton variant="text" width={120} height={30} /></TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {[...Array(3)].map((_, index) => (
            <TableRow key={index}>
              <TableCell><Skeleton variant="text" width="80%" height={25} /></TableCell>
              <TableCell><Skeleton variant="text" width="90%" height={25} /></TableCell>
              <TableCell><Skeleton variant="rectangular" width={80} height={50} /></TableCell>
              <TableCell><Skeleton variant="text" width="60%" height={25} /></TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default DishListSkeleton;
