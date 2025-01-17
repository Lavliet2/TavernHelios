import { Typography, Box, Container } from '@mui/material';

function Menu() {
  return (
    <Box sx={{ padding: 2 }}>
        <Container maxWidth="lg" sx={{ marginTop: 4 }}>
            <Typography variant="h5" color="primary" align="center" sx={{ marginBottom: 2 }}>
                Ждём от марка бэк с меню
            </Typography>
        </Container>
    </Box>
 
  );
}

export default Menu;
