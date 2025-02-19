import React from "react";
import ReservationList from "../../components/Menu/ReservationList";
import { Container, Typography } from "@mui/material";

const ReservationsPage: React.FC = () => {
  return (
    <Container maxWidth="lg" sx={{ mt: 4 }}>
      <Typography variant="h3" align="center" gutterBottom>
        Управление бронированиями
      </Typography>
      <ReservationList />
    </Container>
  );
};

export default ReservationsPage;
