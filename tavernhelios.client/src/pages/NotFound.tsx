import React from "react";
import { useNavigate } from "react-router-dom";
import { Button, Box, Typography } from "@mui/material";

const NotFound: React.FC = () => {
  const navigate = useNavigate();

  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
        height: "100vh",
        textAlign: "center",
      }}
    >
      <Typography variant="h2" color="error">
        404
      </Typography>
      <Typography variant="h5" sx={{ mt: 2 }}>
        Страница не найдена
      </Typography>
      <Typography variant="body1" sx={{ mt: 1, mb: 3 }}>
        Возможно, страница была перемещена или удалена.
      </Typography>
      <Button
        variant="contained"
        color="primary"
        onClick={() => navigate("/")}
      >
        Вернуться на главную
      </Button>
    </Box>
  );
};

export default NotFound;
